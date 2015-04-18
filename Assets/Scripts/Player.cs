using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

	Story story;
	Health health;

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
	
	void OnEnable() {
		story.OnPlayerSleep += HandleSleep;
		story.OnPlayerWakeUp += HandleAwake;
	}

	void OnDisable() {
		story.OnPlayerSleep -= HandleSleep;
		story.OnPlayerWakeUp -= HandleAwake;
	}

	void HandleSleep(SleepTypes sleepType) {
		fpsController.enabled = false;
		headBob.enabled = false;
		if (sleepType == SleepTypes.FoundImposter || sleepType == SleepTypes.GotTired)
			StartCoroutine( SleepCycle(sleepType));
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
