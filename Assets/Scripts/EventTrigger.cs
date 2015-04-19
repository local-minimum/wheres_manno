using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

	[SerializeField] Animator animator;
	[SerializeField] string onEnterTrigger;
	[SerializeField] string onExitTrigger;
	[SerializeField] bool triggerFirstIteration;
	[SerializeField] bool triggerIntermediateIterations;
	[SerializeField] bool triggerLastIteration;

	bool active;
	Story story;

	void Awake() {
		story = GameObject.FindObjectOfType<Story>();
	}

	void OnEnable() {
		story.OnPlayerWakeUp += HandleWake;
	}

	void OnDisable() {
		story.OnPlayerWakeUp -= HandleWake;
	}
	

	void HandleWake(SleepTypes sleepType) {
		if (sleepType == SleepTypes.FoundImposter || sleepType == SleepTypes.GotTired) {
			if (story.FirstIteration)
				active = triggerFirstIteration;
			else if (story.LastIteration)
				active = triggerLastIteration;
			else
				active = triggerIntermediateIterations;

		} else
			active = false;
	}

	void OnTriggerEnter(Collider other) {
		if (active && other.tag == "Player" && onEnterTrigger != "")
			animator.SetTrigger(onEnterTrigger);
	}

	void OnTriggerExit(Collider other) {
		if (active && other.tag == "Player" && onExitTrigger != "")
			animator.SetTrigger(onExitTrigger);
	}
}
