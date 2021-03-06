﻿using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Health : MonoBehaviour {

	[SerializeField] float sleepThreshold;
	[SerializeField] float fullHealth;
	[SerializeField] float sleepRegen;
	[SerializeField] float bashRegen;
	[SerializeField] float bashDeterioration;
	[SerializeField] float baseDeterioration;
	[SerializeField] float stepDuration;

	[SerializeField] AudioClip BeatA;
	[SerializeField] float baseBeatFreq = 1.2f;
	[SerializeField] float intensityFactor = 0.5f;
	[SerializeField] float beatStrength = 0.1f;
	[SerializeField] float beatDuration = 0.2f;
	[SerializeField] GameObject beatHead;
	[SerializeField] float ImposterDeterioration;

	AudioSource soundPlayer;
	RigidbodyFirstPersonController fpsController;
	Story story;
	
	float health;
	float deterioration;
	float nextStep;
	float stepTime;
	bool sleeping = false;

	void Awake() {
		story = GameObject.FindObjectOfType<Story>();
	}

	void Start() {
		fpsController = GetComponentInChildren<RigidbodyFirstPersonController>();
		soundPlayer = GetComponent<AudioSource>();
		StartCoroutine(KeepTheBeat());
		Reset();
	}

	void OnEnable() {
		story.OnPlayerSleep += HandleSleep;
		story.OnPlayerWakeUp += HandlePlayerWakeUp;
		GetComponent<Player>().OnBash += Bash;
	}

	void OnDisable() {
		story.OnPlayerSleep -= HandleSleep;
		story.OnPlayerWakeUp -= HandlePlayerWakeUp;
		GetComponent<Player>().OnBash -= Bash;
	}

	void HandleSleep(SleepTypes sleepType) {
		sleeping = true;
		if (sleepType == SleepTypes.GotTired)
			Sleep();
		else if (sleepType == SleepTypes.FoundImposter)
			deterioration += ImposterDeterioration;
		else if (sleepType == SleepTypes.FoundTarget)
			StartCoroutine(Fade(intensity, 0, 0.1f));
	}
	
	void HandlePlayerWakeUp(SleepTypes sleepType) {
		sleeping = sleepType != SleepTypes.GotTired && sleepType != SleepTypes.FoundImposter;
		if (!sleeping)
			FadeIn(0.2f);
	}

	void Update() {
		if (sleeping)
			return;

		if (isWalking)
			updateStepTime();

		if (hasStepped)
			Step();
	}

	void updateStepTime() {
		if (fpsController.Running)
			stepTime += Time.deltaTime * 2f * fpsController.movementSettings.RunMultiplier;
		else
			stepTime += Time.deltaTime;

	}

	bool isWalking {
		get {
			return Input.GetButton("Horizontal") || Input.GetButton("Vertical");
		}
	}

	bool hasStepped {
		get {
			return stepTime > nextStep;
		}
	}

	void SetNextStepTime() {
		nextStep = stepTime + stepDuration;
	}

	void Reset () {
		health = fullHealth;
		deterioration = baseDeterioration;
	}

	void Step() {
		health -= deterioration;
		Mathf.Max(health, 0);
		if (health <= Mathf.Max(0, sleepThreshold))
			story.Sleep();
		UpdateAilments();
		SetNextStepTime();
	}

	void Sleep() {
		health += sleepRegen;
		Mathf.Min(health, fullHealth);
		UpdateAilments();
	}

	void Bash() {
		deterioration += bashDeterioration;
		health += bashRegen;
		health = Mathf.Min(health, fullHealth);
		UpdateAilments();
	}

	void UpdateAilments() {
		if (sleeping)
			return;

		Ailment.Intensity = 1 - health / fullHealth;
	}

	float intensity {
		get {
			return 1 - health / fullHealth;
		}
	}

	public void FadeOut(float duration) {
		
		StartCoroutine(Fade(intensity, 1, duration));
	}

	public void FadeIn(float duration) {
		StartCoroutine(Fade(1, intensity, duration));
	}

	IEnumerator<WaitForSeconds> Fade(float from, float to, float duration) {
		float startTime = Time.timeSinceLevelLoad;
		float endTime = startTime + duration;
		float curTime = startTime;
		while (curTime < endTime) {
			curTime = Time.timeSinceLevelLoad;
			Ailment.Intensity = Mathf.Lerp(from, to, (curTime - startTime) / duration);
			yield return new WaitForSeconds(0.05f);
		}
	}

	IEnumerator<WaitForSeconds> KeepTheBeat() {
		while (true) {
			yield return new WaitForSeconds(baseBeatFreq - intensityFactor * intensity);
			soundPlayer.PlayOneShot(BeatA);
			if (!sleeping)
				iTween.ShakePosition(beatHead, Vector3.down * beatStrength, beatDuration);
		}
	}
}
