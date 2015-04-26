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
	[SerializeField] TextMesh moonText;

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

	bool allowPlayerControl {
		get {
			return fpsController.enabled && headBob.enabled;
		}

		set {
			fpsController.enabled = value;
			headBob.enabled = value;
		}
	}

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
		startRotation = transform.localRotation;
		startScale = transform.localScale;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
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
		allowPlayerControl = false;
		if (sleepType == SleepTypes.GotTired)
			StartCoroutine( SleepCycle(sleepType));
		else if (sleepType == SleepTypes.FoundImposter) {
			soundPlayer.PlayOneShot(story.ImposterClip);
			story.AwakePlayer(sleepType);
		} else if (sleepType == SleepTypes.FoundTarget)
			StartCoroutine(Win());
		else if (sleepType == SleepTypes.GaveUp)
			StartCoroutine(Fail());
	}

	IEnumerator<WaitForSeconds> Win() {
		playerBody.isKinematic = true;
		playerCollider.isTrigger = true;
		eventsAnimator.SetTrigger(winTrigger);
		bool hasPlayedSound = false;
		Camera cam = headBob.GetComponentInChildren<Camera>();

		for (float p=0; p<4f;p+=0.01f) {
			iTween.PutOnPath(gameObject, pathToMoon, Mathf.Clamp01(p));
			if (p>0.5f && !hasPlayedSound) {
				soundPlayer.PlayOneShot(story.HuggingMoonClip);
				moonText.gameObject.SetActive(true);
				hasPlayedSound = true;
			} else if (p>0.5f) {
				moonText.offsetZ = Mathf.Lerp(-30, 0, p/4);
			}
			cam.transform.LookAt(pathToMoon[pathToMoon.Length - 1]);
			yield return new WaitForSeconds(0.03f);
		}
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(menuSceneName);
	}

	IEnumerator<WaitForSeconds> Fail() {
		soundPlayer.PlayOneShot(story.GiveUpClip);
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
		health.FadeOut(0.4f);
		yield return new WaitForSeconds(1f);
		transform.localPosition = startPosition;
		transform.localScale = startScale;
		transform.localRotation = startRotation;
		story.AwakePlayer(sleepType);
	}

	void HandleAwake(SleepTypes sleepType) {
		if (sleepType == SleepTypes.GotTired || sleepType == SleepTypes.FoundImposter)
			allowPlayerControl = true;
	}

	public float GetDistance(GameObject other) {
		return Vector3.Distance(transform.position, other.transform.position);
	}
}
