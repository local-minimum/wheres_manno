using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Occlusion_Worries : Ailment {

	ScreenSpaceAmbientOcclusion effect;
	float maxEffect;

	// Use this for initialization
	void Start () {
		effect = GetComponentInChildren<ScreenSpaceAmbientOcclusion>();
		maxEffect = effect.m_OcclusionIntensity;

	}
	
	// Update is called once per frame
	void Update () {
		effect.m_OcclusionIntensity = maxEffect * Mathf.Pow(intensity, 2);
	}
}
