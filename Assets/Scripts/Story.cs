using UnityEngine;
using System.Collections;
using System.Linq;

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
		if (playIteration > highestImposterNumber)
			FoundTarget();
		else {
			sleepCycles++;
			if (OnSleep != null)
				OnSleep(SleepTypes.FoundImposter);
		}
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
