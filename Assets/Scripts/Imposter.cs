using UnityEngine;
using System.Collections;

public class Imposter : MonoBehaviour {

	[SerializeField] Color imposterLight;
	[SerializeField] static protected Story story;
	[SerializeField] int activeIteration;
	[SerializeField] Material imposterMaterial;
	[SerializeField] bool addLightFirst = true;

	MeshRenderer meshRenderer;
	bool active = false;

	// Use this for initialization
	void Start () {
		if (Imposter.story == null)
			Imposter.story = GameObject.FindObjectOfType<Story>();
		meshRenderer = GetComponent<MeshRenderer>();
		active = Imposter.story.PlayIteration == activeIteration;
		if (active)
			AddImposterLight();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {

//			FlashLight();
		}
	}

	void FlashLight() {
		imposterMaterial.SetColor("_Emission",
		                          imposterLight * Mathf.Clamp01(Mathf.Sin (Time.timeSinceLevelLoad)));
		DynamicGI.UpdateMaterials(meshRenderer);
		DynamicGI.UpdateEnvironment();
	}

	void AddImposterLight() {
		Material[] materials = new Material[meshRenderer.materials.Length + 1];
		materials[addLightFirst ? 0 : materials.Length - 1] = imposterMaterial;
		for (int i=0; i<meshRenderer.materials.Length; i++)
			materials[i + (addLightFirst ? 1 : 0)] = meshRenderer.materials[i];
		meshRenderer.materials = materials;
		imposterMaterial.EnableKeyword("_Emission");
	}

}
