﻿using UnityEngine;
using System.Collections;
using System.Linq;

public enum SleepTypes {FoundImposter, FoundTarget, GaveUp, GotTired};

public delegate void PlayerSleep(SleepTypes sleepType);

public class Story : MonoBehaviour {

	[SerializeField] int playIteration = 0;
	[SerializeField] AudioClip[] imposterVocalisations;
	[SerializeField] AudioClip[] ramblings;
	int mannoIndex = -1;
	int sleepCycles = 0;
	[SerializeField] int maxSleepCycles = 10;

    public AudioClip GiveUpClip;
	public AudioClip HuggingMoonClip;
	public AudioClip ImposterClip;

	public event PlayerSleep OnPlayerSleep;
	public event PlayerSleep OnPlayerWakeUp;

	private int highestImposterNumber;

	void Start() {
		highestImposterNumber = GameObject.FindObjectsOfType<Imposter>().Max(i => i.ActiveIteration);
	}

	public int PlayIteration {
		get {
			return playIteration;
		}
	}

	public AudioClip ImposterVocalisation {
		get {
			if (imposterVocalisations.Length == 0)
				return null;

			mannoIndex ++;
			mannoIndex %= imposterVocalisations.Length;

			return imposterVocalisations[mannoIndex];
		}
	}

	public AudioClip NextRambling {
		get {
			if (ramblings.Length == 0)
				return null;

			return ramblings[Random.Range(0, ramblings.Length)];
		}
	}

	public void Sleep() {
		sleepCycles++;
		if (sleepCycles - playIteration >= maxSleepCycles)
			GiveUp();
		else if (OnPlayerSleep != null)
			OnPlayerSleep(SleepTypes.GotTired);
	}

	public void FoundImposter() {
		playIteration++;
		if (playIteration > highestImposterNumber)
			FoundTarget();
		else {
			sleepCycles++;
			if (OnPlayerSleep != null)
				OnPlayerSleep(SleepTypes.FoundImposter);
		}
	}

	public void FoundTarget() {
		if (OnPlayerSleep != null)
			OnPlayerSleep(SleepTypes.FoundTarget);
	}

	public void GiveUp() {
		if (OnPlayerSleep != null)
			OnPlayerSleep(SleepTypes.GaveUp);
	}

	public void AwakePlayer(SleepTypes sleepType) {
		if (OnPlayerWakeUp != null)
			OnPlayerWakeUp(sleepType);
	}

	public bool FirstIteration {
		get {
			return playIteration == 0;
		}
	}

	public bool LastIteration {
		get {
			return playIteration == highestImposterNumber;
		}
	}
}
