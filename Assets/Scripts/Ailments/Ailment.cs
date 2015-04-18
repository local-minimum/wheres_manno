using UnityEngine;
using System.Collections;

public abstract class Ailment : MonoBehaviour {

	[Range(0, 1)] [SerializeField] protected float intensity;
	
}
