using UnityEngine;
using System.Collections;

public class Imposter : MonoBehaviour {

	[SerializeField] static protected Story story;
	[SerializeField] int activeIteration;
	[SerializeField] float lightFlashingFrequency;
	[SerializeField] float callOutSpacing;


	AudioSource player;
	bool active = false;
	
	float imposterLightMaxIntensity;
	float lastCallOutTime;
	Light imposterLight;

	public int ActiveIteration {
		get {
			return activeIteration;
		}
	}

	void OnEnable() {
		story.OnPlayerSleep += HandleSleep;
	}

	void OnDisable() {
		story.OnPlayerSleep -= HandleSleep;
	}

	void HandleSleep(SleepTypes sleepType) {
		if (sleepType == SleepTypes.FoundImposter)
			active = Imposter.story.PlayIteration == activeIteration;
		else if (active)
			active = sleepType == SleepTypes.GaveUp;

		imposterLight.enabled = active;
	}

	void Awake() {
		if (Imposter.story == null)
			Imposter.story = GameObject.FindObjectOfType<Story>();
	}

	void Start () {


		active = Imposter.story.PlayIteration == activeIteration;
		player = GetComponent<AudioSource>();
		imposterLight = GetComponent<Light>();
		imposterLightMaxIntensity = imposterLight.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			if (IsCallOutTime)
				CallOut();

			FlashLight();
		} else
			imposterLight.enabled = false;
	}

	void CallOut() {
		player.PlayOneShot(story.ImposterVocalisation);
		lastCallOutTime = Time.timeSinceLevelLoad;
	}

	bool IsCallOutTime {
		get {
			return !player.isPlaying && Time.timeSinceLevelLoad - lastCallOutTime > callOutSpacing;
		}
	}

	void FlashLight() {
		imposterLight.intensity = Mathf.Pow(Mathf.Sin(Time.timeSinceLevelLoad * lightFlashingFrequency), 2) * imposterLightMaxIntensity; 
	}

	void OnTriggerEnter(Collider other) {
		if (active && other.tag == "Player")
			story.FoundImposter();
	}

}
