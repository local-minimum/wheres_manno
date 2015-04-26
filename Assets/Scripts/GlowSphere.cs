using UnityEngine;
using System.Collections;

public class GlowSphere : MonoBehaviour {

	ParticleSystem emitter;

	void Awake() {
		emitter = GetComponent<ParticleSystem>();
	}

	void OnGlowSphereStart(Transform start) {
		transform.position = start.position;
		emitter.enableEmission = true;
	}

	void OnGlowSphereEnd() {
		emitter.enableEmission = false;
	}
}
