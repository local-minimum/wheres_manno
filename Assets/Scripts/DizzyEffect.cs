using UnityEngine;
using System.Collections.Generic;

public class DizzyEffect : MonoBehaviour {

	[SerializeField] float dizzySpeed;
	[SerializeField] float dizzyDuration;
	[SerializeField] float easeDuration;

	ParticleSystem emitter;
	bool dizzy = false;
	float angle;
	float spinDelta = 0.03f;
	float spinUntilTime;

	// Use this for initialization
	void Start () {
		emitter = GetComponent<ParticleSystem>();
		emitter.Pause();
	}

	void Update() {
		if (Input.GetButtonDown("Fire1"))
			MakeDizzy();
	}
	
	public void MakeDizzy() {

		if (!dizzy)
			StartCoroutine(Dizzify());
		else
			spinUntilTime += spinDelta;
	}

	IEnumerator<WaitForSeconds> Dizzify() {

		emitter.Play();
		dizzy = true;
		spinUntilTime = Time.timeSinceLevelLoad + dizzyDuration + easeDuration;

		while (Time.timeSinceLevelLoad < spinUntilTime) {
			angle += dizzySpeed * spinDelta;
			transform.Rotate(Vector3.forward, dizzySpeed * spinDelta, Space.Self);
			yield return new WaitForSeconds(spinDelta);
		}
		emitter.Stop();
		emitter.Clear();
		dizzy = false;
	}
}
