using UnityEngine;
using System.Collections;

public enum SleepTypes {FoundImposter, FoundTarget, GaveUp, GotTired};

public delegate void PlayerSleep(SleepTypes sleepType);

public class Story : MonoBehaviour {

	[SerializeField] int playIteration = 0;
	[SerializeField] AudioClip[] imposterVocalisations;
	[SerializeField] AudioClip[] ramblings;
	int rambling = -1;
	int sleepCycles = 0;
	[SerializeField] int maxSleepCycles = 10;
	[HideInInspector]
	public event PlayerSleep OnSleep;

	public int PlayIteration {
		get {
			return playIteration;
		}
	}

	public AudioClip ImposterVocalisation {
		get {
			if (imposterVocalisations.Length == 0)
				return null;
			return imposterVocalisations[Random.Range(0, imposterVocalisations.Length)];
		}
	}

	public AudioClip NextRambling {
		get {
			if (ramblings.Length == 0)
				return null;
			rambling ++;
			rambling %= ramblings.Length;

			return ramblings[rambling];
		}
	}

	public void Sleep() {
		sleepCycles++;
		if (sleepCycles > maxSleepCycles)
			GiveUp();
		else if (OnSleep != null)
			OnSleep(SleepTypes.GotTired);
	}

	public void FoundImposter() {
		playIteration++;
		sleepCycles++;
		if (OnSleep != null)
			OnSleep(SleepTypes.FoundImposter);
	}

	public void FoundTarget() {
		if (OnSleep != null)
			OnSleep(SleepTypes.FoundTarget);
	}

	public void GiveUp() {
		if (OnSleep != null)
			OnSleep(SleepTypes.GaveUp);
	}
}
