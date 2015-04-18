using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Vortex_World : Ailment {

	Vortex vortex;
	[SerializeField] float periodLower;
	[SerializeField] float periodUpper;

	float vortexAmplitude;
	float angleTime = 0;

	// Use this for initialization
	void Start () {
		vortex = GetComponentInChildren<Vortex>();
		vortexAmplitude = Mathf.Abs(vortex.angle);
	}
	
	// Update is called once per frame
	void Update () {
		angleTime += Time.deltaTime * Mathf.Lerp(periodLower, periodUpper, intensity);
		vortex.angle = Mathf.Cos(angleTime) * vortexAmplitude;
	}
}
