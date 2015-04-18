using UnityEngine;
using System.Collections.Generic;

public class DizzyEffect : MonoBehaviour {

	[SerializeField] float dizzySpeed;
	[SerializeField] float dizzyDuration;
	[SerializeField] float easeDuration;

	MeshRenderer meshRenderer;
	bool dizzy = false;
	float angle;
	float spinDelta = 0.03f;
	float spinUntilTime;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.enabled = false;
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

		meshRenderer.enabled = true;
		dizzy = true;
		spinUntilTime = Time.timeSinceLevelLoad + dizzyDuration + easeDuration;

		while (Time.timeSinceLevelLoad < spinUntilTime) {
			angle += dizzySpeed * spinDelta;
			transform.Rotate(Vector3.forward, dizzySpeed * spinDelta, Space.Self);
			yield return new WaitForSeconds(spinDelta);
		}
		dizzy = false;
		meshRenderer.enabled = false;
	}
}
