using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	[SerializeField] string playScene;
	[SerializeField] string helpTrigger;
	[SerializeField] Animator animations;

	void OnEnable() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel"))
			Exit();
	}

	public void Play() {
		Application.LoadLevel(playScene);
	}

	public void Exit() {
		Application.Quit();
	}

	public void Help() {
		animations.SetTrigger(helpTrigger);
	}
}
