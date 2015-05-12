using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	[SerializeField] string playScene;
	[SerializeField] string helpTrigger;
	[SerializeField] Animator animations;
	string helpTextDefault;
	[SerializeField] string[] helpTexts;
	[SerializeField] Text text;

	void Awake() {
		helpTextDefault = text.text;
		for (int i=0; i<helpTexts.Length;i++)
			helpTexts[i] = helpTexts[i].Replace("[]","\n");
	}

	void OnEnable() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		ResetText();
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

	public void SetText(int index) {
		text.text = helpTexts[index];
	}

	public void ResetText() {
		text.text = helpTextDefault;
	}
}
