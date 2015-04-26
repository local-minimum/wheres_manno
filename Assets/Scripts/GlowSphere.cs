using UnityEngine;
using System.Collections.Generic;

public class GlowSphere : MonoBehaviour {

	ParticleSystem emitter;
	Transform[] path = new Transform[]{};
	AudioSource soundSystem;
	MeshRenderer meshRenderer;

	void Awake() {
		emitter = GetComponent<ParticleSystem>();
		soundSystem = GetComponent<AudioSource>();
		meshRenderer = GetComponent<MeshRenderer>();
		OnGlowSphereEnd();
	}

	void OnGlowSphereStart(Transform[] path) {
		this.path = path;
		transform.position = path[0].position;
		meshRenderer.enabled = true;
		emitter.enableEmission = true;
		soundSystem.Play();
	}

	void OnGlowSphereEnd() {
		emitter.enableEmission = false;
		meshRenderer.enabled = false;
		soundSystem.Stop();
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		for (int i=0; i<path.Length-1;i++)
			Gizmos.DrawLine(path[i].position, path[i + 1].position);
	}
}
