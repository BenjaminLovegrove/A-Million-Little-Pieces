using UnityEngine;
using System.Collections;

public class ObjectDepth : MonoBehaviour {

	Vector3 depthPos;
	public bool tallObj = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (tallObj) {
			depthPos = new Vector3 (transform.position.x, transform.position.y, transform.position.y -2);
		} else {
			depthPos = new Vector3 (transform.position.x, transform.position.y, transform.position.y);
		}

		transform.position = depthPos;
	}
}
