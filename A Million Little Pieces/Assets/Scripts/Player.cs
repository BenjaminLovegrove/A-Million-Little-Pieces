using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speedMod = 5f;
	Animator anim;
	AudioSource runningSFX;
	bool moveLock = false;
	Vector3 enemyTargetPos;
	float lerpTimer = 2f;

	void Start () {
		anim = GetComponent<Animator> ();
		runningSFX = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		if (!moveLock) {
			transform.Translate (Input.GetAxis ("Horizontal") * Time.deltaTime * speedMod, Input.GetAxis ("Vertical") * Time.deltaTime * speedMod, 0);

			//Animation
			if ((Input.GetAxis ("Horizontal") != 0) || (Input.GetAxis ("Vertical") != 0)) {
				anim.SetBool ("Walking", true);
				if (!runningSFX.isPlaying) {
					runningSFX.Play ();
				}
			} else {
				anim.SetBool ("Walking", false);
				if (runningSFX.isPlaying) {
					runningSFX.Stop ();
				}
			}
		}

		if (lerpTimer < 1f) {
			lerpTimer += 0.05f * Time.deltaTime;
			//if (Vector2.Distance (transform.position, enemyTargetPos) > 3.5f) {
			if (((enemyTargetPos.x - transform.position.x) > 3.5f) || ((enemyTargetPos.y - transform.position.y) > 1f)){
				transform.position = Vector3.Lerp (transform.position, enemyTargetPos, lerpTimer);
				anim.SetBool ("Walking", true);
				if (!runningSFX.isPlaying) {
					runningSFX.Play ();
				}
			} else {
				anim.SetBool ("Walking", false);
				if (runningSFX.isPlaying) {
					runningSFX.Stop ();
				}
			}
		}
	}

	void Spotted(Vector3 enemypos){
		if (!moveLock) {
			LockMovement ();
			enemyTargetPos = enemypos;
			Invoke ("MoveTo", 1f);
		}
	}

	void MoveTo(){
		lerpTimer = 0f;
	}

	void LockMovement(){
		moveLock = true;
		anim.SetBool ("Walking", false);
		if (runningSFX.isPlaying) {
			runningSFX.Stop ();
		}
	}

	void UnlockMovement(){
		moveLock = false;
	}
}
