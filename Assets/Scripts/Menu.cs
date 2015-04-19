using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	[SerializeField] string playScene;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel"))
			Application.Quit();
		else if (Input.anyKeyDown)
			Application.LoadLevel(playScene);
	}
}
