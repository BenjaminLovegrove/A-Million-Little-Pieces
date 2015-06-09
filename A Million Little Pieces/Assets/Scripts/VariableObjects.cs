using UnityEngine;
using System.Collections;

public class VariableObjects : MonoBehaviour {

	public int threshold;

	void Cycle (int state) {
		foreach (Transform child in transform){
			if (state == threshold) {
				if (child.gameObject.activeSelf == false){
					child.gameObject.SetActive (true);
				}
			}

			if (child.gameObject.activeSelf == true){
				if (child.gameObject.tag == "good" && state > threshold){
					child.gameObject.SetActive (false);
				} else if (child.gameObject.tag == "bad" && state < threshold){
					child.gameObject.SetActive (false);
				}
			}
		}
	}
}
