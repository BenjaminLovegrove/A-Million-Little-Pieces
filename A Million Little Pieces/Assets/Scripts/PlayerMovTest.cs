using UnityEngine;
using System.Collections;

public class PlayerMovTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Input.GetAxis ("Horizontal") * 0.5f, Input.GetAxis ("Vertical") * 0.5f, 0);
	}
}
