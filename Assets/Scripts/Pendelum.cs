using UnityEngine;
using System.Collections;

public class Pendelum : MonoBehaviour {

	[SerializeField] float y;
	[SerializeField] float rotationTime;

	// Use this for initialization
	void Start () {
		iTween.RotateTo(
			gameObject,
			iTween.Hash(
				"y", y,
				"time", rotationTime,
				"easetype", iTween.EaseType.easeInOutCubic,
				"looptype", iTween.LoopType.pingPong
			));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
