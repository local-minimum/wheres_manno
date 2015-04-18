using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Story story;

	Vector3 startPosition;
	Quaternion startRotation;
	Vector3 startScale;

	void Awake() {
		story = GameObject.FindObjectOfType<Story>();
	}

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
		startRotation = transform.localRotation;
		startScale = transform.localScale;
	}
	
	void OnEnable() {
		story.OnSleep += HandleSleep;
	}

	void OnDisable() {
		story.OnSleep -= HandleSleep;
	}

	void HandleSleep(SleepTypes sleepType) {
		if (sleepType == SleepTypes.FoundImposter || sleepType == SleepTypes.GotTired)
			ResetStartValues();
	}

	void ResetStartValues() {
		transform.localPosition = startPosition;
		transform.localScale = startScale;
		transform.localRotation = startRotation;
	}
}
