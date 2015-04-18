using UnityEngine;
using System.Collections;

public abstract class Ailment : MonoBehaviour {

	[Range(0, 1)] [SerializeField] protected float intensity;

	public float Intensity {
		set {
			intensity = Mathf.Clamp01(value);
		}
	}
	
}
