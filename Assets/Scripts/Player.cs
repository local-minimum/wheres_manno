using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public delegate void Bash();

public class Player : MonoBehaviour {

	[SerializeField] Animator eventsAnimator;
	[SerializeField] string bashTrigger;
	[SerializeField] string winTrigger;
	[SerializeField] string menuSceneName;

	Story story;
	Health health;
	[SerializeField] iTweenPath pathToMoon;

	public event Bash OnBash;
	RigidbodyFirstPersonController fpsController;
	HeadBob headBob;

	Vector3 startPosition;
	Quaternion startRotation;
	Vector3 startScale;

	Rigidbody playerBody;
	Collider playerCollider;

	void Awake() {
		story = GameObject.FindObjectOfType<Story>();
		fpsController = GetComponentInChildren<RigidbodyFirstPersonController>();
		headBob = GetComponentInChildren<HeadBob>();
		health = GetComponent<Health>();
		playerBody = GetComponent<Rigidbody>();
		playerCollider = GetComponent<Collider>();
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
		else if (sleepType == SleepTypes.FoundTarget)
			Win();
		else if (sleepType == SleepTypes.GaveUp)
			StartCoroutine(Fail());
	}

	void Win() {
		playerBody.isKinematic = true;
		playerCollider.isTrigger = true;
		eventsAnimator.SetTrigger(winTrigger);

	}

	IEnumerator<WaitForSeconds> Fail() {
		AudioSource player = GetComponentInChildren<AudioSource>();
		player.PlayOneShot(story.GameOverClip);
		while (player.isPlaying)
			yield return new WaitForSeconds(0.1f);
		yield return new WaitForSeconds(2f);
		Application.LoadLevel(menuSceneName);
	}

	void HandleBash() {
		eventsAnimator.SetTrigger(bashTrigger);
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
