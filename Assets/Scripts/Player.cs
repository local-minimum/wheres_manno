using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public delegate void Bash();

public class Player : MonoBehaviour {

	[SerializeField] Animator bashAnimator;
	[SerializeField] string bashTrigger;

	Story story;
	Health health;

	public event Bash OnBash;
	RigidbodyFirstPersonController fpsController;
	HeadBob headBob;

	Vector3 startPosition;
	Quaternion startRotation;
	Vector3 startScale;

	void Awake() {
		story = GameObject.FindObjectOfType<Story>();
		fpsController = GetComponentInChildren<RigidbodyFirstPersonController>();
		headBob = GetComponentInChildren<HeadBob>();
		health = GetComponent<Health>();
	}

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
		startRotation = transform.localRotation;
		startScale = transform.localScale;
	}

	void Update() {
		if (Input.GetButtonDown("Fire1") && OnBash != null)
			OnBash();

	}

	void OnEnable() {
		story.OnPlayerSleep += HandleSleep;
		story.OnPlayerWakeUp += HandleAwake;
		OnBash += HandleBash;
	}

	void OnDisable() {
		story.OnPlayerSleep -= HandleSleep;
		story.OnPlayerWakeUp -= HandleAwake;
		OnBash -= HandleBash;
	}

	void HandleSleep(SleepTypes sleepType) {
		fpsController.enabled = false;
		headBob.enabled = false;
		if (sleepType == SleepTypes.FoundImposter || sleepType == SleepTypes.GotTired)
			StartCoroutine( SleepCycle(sleepType));
	}

	void HandleBash() {
		bashAnimator.SetTrigger(bashTrigger);
	}
	
	IEnumerator<WaitForSeconds> SleepCycle(SleepTypes sleepType) {
		health.FadeOut(0.4f);
		yield return new WaitForSeconds(1f);
		transform.localPosition = startPosition;
		transform.localScale = startScale;
		transform.localRotation = startRotation;
		story.AwakePlayer(sleepType);
	}

	void HandleAwake(SleepTypes sleepType) {
		fpsController.enabled = sleepType == SleepTypes.GotTired || sleepType == SleepTypes.FoundImposter;
		headBob.enabled = fpsController.enabled;
	}
}
