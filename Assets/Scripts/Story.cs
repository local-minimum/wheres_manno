using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	[SerializeField] int playIteration = 0;
	[SerializeField] AudioClip[] imposterVocalisations;

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

}
