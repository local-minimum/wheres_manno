using UnityEngine;
using System.Collections;

public abstract class Ailment : MonoBehaviour {

	static protected float intensity;

	public static float Intensity {
		set {
			intensity = Mathf.Clamp01(value);
		}
	}
	
}
