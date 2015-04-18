using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Health : MonoBehaviour {

	[SerializeField] float fullHealth;
	[SerializeField] float sleepRegen;
	[SerializeField] float bashRegen;
	[SerializeField] float bashDeterioration;
	[SerializeField] float baseDeterioration;
	[SerializeField] float stepDuration;

	RigidbodyFirstPersonController fpsController;
	Ailment[] ailments;
	float health;
	float deterioration;
	float nextStep;
	float stepTime;

	void Start() {
		ailments = GetComponentsInChildren<Ailment>();
		fpsController = GetComponentInChildren<RigidbodyFirstPersonController>();
		Reset();
	}

	void Update() {
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
		foreach (Ailment ailment in ailments)
			ailment.Intensity = 1 - health / fullHealth;
	}
}
