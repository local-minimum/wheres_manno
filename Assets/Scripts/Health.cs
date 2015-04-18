using UnityEngine;
using System.Collections.Generic;

public class Health : MonoBehaviour {

	[SerializeField] float fullHealth;
	[SerializeField] float sleepRegen;
	[SerializeField] float bashRegen;
	[SerializeField] float bashDeterioration;
	[SerializeField] float baseDeterioration;
	[SerializeField] float stepDuration;

	Ailment[] ailments;
	float health;
	float deterioration;
	float nextStep;
	float stepTime;

	void Start() {
		ailments = GetComponentsInChildren<Ailment>();
		Reset();
	}

	void Update() {
		if (isWalking)
			stepTime += Time.deltaTime;

		if (hasStepped)
			Step();
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
