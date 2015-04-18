using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	[SerializeField] int playIteration = 0;
	[SerializeField] AudioClip[] imposterVocalisations;
	[SerializeField] AudioClip[] ramblings;
	int rambling = -1;

	public int PlayIteration {
		get {
			return playIteration;
		}
	}

	public AudioClip ImposterVocalisation {
		get {
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

}
