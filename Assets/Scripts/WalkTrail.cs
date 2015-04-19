using UnityEngine;
using System.Collections;

public class WalkTrail : MonoBehaviour {

	Story story;
	Player player;
	ParticleSystem emitter;

	// Use this for initialization
	void Awake () {
		story = GameObject.FindObjectOfType<Story>();
		player = GameObject.FindObjectOfType<Player>();
		emitter = GetComponent<ParticleSystem>();
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
		if (sleepType == SleepTypes.FoundTarget) {
			emitter.Clear();
			emitter.Stop();

		}
	}

	void HandleAwake(SleepTypes sleepType) {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
