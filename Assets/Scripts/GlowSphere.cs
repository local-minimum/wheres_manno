using UnityEngine;
using System.Collections;

public class GlowSphere : MonoBehaviour {

	ParticleSystem emitter;
	Transform[] path = new Transform[]{};
	AudioSource soundSystem;

	void Awake() {
		emitter = GetComponent<ParticleSystem>();
		soundSystem = GetComponent<AudioSource>();
	}

	void OnGlowSphereStart(Transform[] path) {
		this.path = path;
		transform.position = path[0].position;
		emitter.enableEmission = true;
		soundSystem.Play();
	}

	void OnGlowSphereEnd() {
		emitter.enableEmission = false;
		soundSystem.Stop();
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		for (int i=0; i<path.Length-1;i++)
			Gizmos.DrawLine(path[i].position, path[i + 1].position);
	}
}
