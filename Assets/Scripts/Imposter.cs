using UnityEngine;
using System.Collections;

public class Imposter : MonoBehaviour {

	[SerializeField] static protected Story story;
	[SerializeField] int activeIteration;
	[SerializeField] float lightFlashingFrequency;
	[SerializeField] float callOutSpacing;

	[SerializeField] GameObject visualCallEffect;
	[SerializeField][Range(0, 1)] float pathLookAhead = 0.4f;
	[SerializeField] float pathDelay = 0.4f;
	[SerializeField] float pathSpeed = 5f;

	static public Imposter current;

	Player player;
	AudioSource soundPlayer;
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
		if (sleepType == SleepTypes.FoundImposter || sleepType == SleepTypes.GotTired)
			active = Imposter.story.PlayIteration == activeIteration;
		else if (active)
			active = sleepType == SleepTypes.GaveUp;

		if (active)
			Imposter.current = this;
		imposterLight.enabled = active;
	}

	void Awake() {
		if (Imposter.story == null)
			Imposter.story = GameObject.FindObjectOfType<Story>();
		player = GameObject.FindObjectOfType<Player>();
	}

	void Start () {


		active = Imposter.story.PlayIteration == activeIteration;
		soundPlayer = GetComponent<AudioSource>();
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
		soundPlayer.PlayOneShot(story.ImposterVocalisation, vocalizationVolume);
		Transform[] path = Checkpoint.GetPathFromTo(transform, player.transform);
		iTween.MoveTo(visualCallEffect, iTween.Hash(
			"path", path ,
			"lookahead", pathLookAhead,
			"speed", pathSpeed,
			"delay", pathDelay,
			"easetype", iTween.EaseType.easeInExpo,
			"onstart", "OnGlowSphereStart",
			"onstarttarget", visualCallEffect,
			"onstartparams", path,
			"oncomplete", "OnGlowSphereEnd",
			"oncompletetarget", visualCallEffect));
		lastCallOutTime = Time.timeSinceLevelLoad;
	}

	float vocalizationVolume {
		get {
			return Mathf.Clamp01(player.GetDistance(gameObject) / soundPlayer.maxDistance);
		}
	}

	bool IsCallOutTime {
		get {
			return !soundPlayer.isPlaying && Time.timeSinceLevelLoad - lastCallOutTime > callOutSpacing;
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
