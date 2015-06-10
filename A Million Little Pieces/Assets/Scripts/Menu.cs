using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Play () {
		Application.LoadLevel("Main");
	}
	
	// Update is called once per frame
	void Quit () {
		Application.Quit();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
