using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speedMod = 5f;
	Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		transform.Translate (Input.GetAxis ("Horizontal") * Time.deltaTime * speedMod, Input.GetAxis ("Vertical") * Time.deltaTime * speedMod, 0);

		//Animation
		if ((Input.GetAxis ("Horizontal") != 0) || (Input.GetAxis ("Vertical") != 0)) {
			anim.SetBool ("Walking", true);
		} else {
			anim.SetBool ("Walking", false);
		}
	}
}
