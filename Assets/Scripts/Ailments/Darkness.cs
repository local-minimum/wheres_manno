using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Darkness : Ailment {

	VignetteAndChromaticAberration effect;
	Tonemapping effect2;
	float minValue;
	float baseExposure;
	[SerializeField] float maxValue = 10f;

	// Use this for initialization
	void Start () {
		effect = GetComponentInChildren<VignetteAndChromaticAberration>();
		effect2 = GetComponentInChildren<Tonemapping>();
		minValue = effect.intensity;
		baseExposure = effect2.exposureAdjustment;
	}
	
	// Update is called once per frame
	void Update () {
		effect.intensity = Mathf.Lerp(minValue, maxValue, intensity);
		effect2.exposureAdjustment = baseExposure - Mathf.Pow(intensity, 2f);
	}
}
