using UnityEngine;
using System.Collections.Generic;

public class DizzyEffect : MonoBehaviour {

	[SerializeField] float dizzySpeed;
	[SerializeField] float dizzyDuration;
	[SerializeField] float delay;

	MeshRenderer[] meshRenderers;
	bool dizzy = false;
	float angle;
	float spinDelta = 0.03f;
	float spinUntilTime;

	// Use this for initialization
	void Start () {
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
		renderMesh = false;
	}

	bool renderMesh {
		set {
			foreach (MeshRenderer meshRenderer in meshRenderers)
				meshRenderer.enabled = value;
		}
	}

	void OnEnable() {
		GetComponentInParent<Player>().OnBash += MakeDizzy;
	}

	void OnDisable() {
		Player player = GetComponentInParent<Player>();
		if (player != null)
			player.OnBash -= MakeDizzy;
	}
	
	public void MakeDizzy() {

		if (!dizzy)
			StartCoroutine(Dizzify());
		else
			spinUntilTime += spinDelta;
	}

	IEnumerator<WaitForSeconds> Dizzify() {
		dizzy = true;
		spinUntilTime = Time.timeSinceLevelLoad + dizzyDuration + delay;
		yield return new WaitForSeconds(delay);
		renderMesh = true;
		while (Time.timeSinceLevelLoad < spinUntilTime) {
			angle += dizzySpeed * spinDelta;
			transform.Rotate(Vector3.forward, dizzySpeed * spinDelta, Space.Self);
			yield return new WaitForSeconds(spinDelta);
		}
		dizzy = false;
		renderMesh = false;
	}
}
