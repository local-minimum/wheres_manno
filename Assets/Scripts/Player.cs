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
	[SerializeField] Transform[] pathToMoon;

	public event Bash OnBash;
	RigidbodyFirstPersonController fpsController;
	HeadBob headBob;

	Vector3 startPosition;
	Quaternion startRotation;
	Vector3 startScale;

	Rigidbody playerBody;
	Collider playerCollider;
	AudioSource soundPlayer;
	[SerializeField] AudioClip bashClip;

	public float velocity {
		get {
			return playerBody.velocity.magnitude;
		}
	}

	void Awake() {
		story = GameObject.FindObjectOfType<Story>();
		fpsController = GetComponentInChildren<RigidbodyFirstPersonController>();
		headBob = GetComponentInChildren<HeadBob>();
		health = GetComponent<Health>();
		playerBody = GetComponent<Rigidbody>();
		playerCollider = GetComponent<Collider>();
		soundPlayer = GetComponentInChildren<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
		startRotation = transform.localRotation;
		startScale = transform.localScale;
	}

	void Update() {
		if (Input.GetButtonDown("Fire1") && OnBash != null && fpsController.enabled == true)
			OnBash();
		else if (Input.GetButtonDown("Cancel"))
			Application.LoadLevel(menuSceneName);

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
			StartCoroutine(Win());
		else if (sleepType == SleepTypes.GaveUp)
			StartCoroutine(Fail());
	}

	IEnumerator<WaitForSeconds> Win() {
		playerBody.isKinematic = true;
		playerCollider.isTrigger = true;
		eventsAnimator.SetTrigger(winTrigger);
		bool hasPlayedSound = false;

		for (float p=0; p<4f;p+=0.01f) {
			iTween.PutOnPath(gameObject, pathToMoon, Mathf.Clamp01(p));
			if (p>0.5f && !hasPlayedSound) {
				soundPlayer.PlayOneShot(story.HuggingMoonClip);
				hasPlayedSound = true;
			}
			yield return new WaitForSeconds(0.03f);
		}
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(menuSceneName);
	}

	IEnumerator<WaitForSeconds> Fail() {
		soundPlayer.PlayOneShot(story.GiveUpClip);
		while (soundPlayer.isPlaying)
			yield return new WaitForSeconds(0.1f);
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(menuSceneName);
	}

	void HandleBash() {
		eventsAnimator.SetTrigger(bashTrigger);
		StartCoroutine(BashSound());
	}

	IEnumerator<WaitForSeconds> BashSound() {
		yield return new WaitForSeconds(0.1f);
		soundPlayer.PlayOneShot(bashClip);
	}
	
	IEnumerator<WaitForSeconds> SleepCycle(SleepTypes sleepType) {
		if (sleepType == SleepTypes.FoundImposter)
			soundPlayer.PlayOneShot(story.ImposterClip);
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

	public float GetDistance(GameObject other) {
		return Vector3.Distance(transform.position, other.transform.position);
	}
}
