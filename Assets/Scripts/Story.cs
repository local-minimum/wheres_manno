using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	[SerializeField] int playIteration = 0;

	public int PlayIteration {
		get {
			return playIteration;
		}
	}

}
