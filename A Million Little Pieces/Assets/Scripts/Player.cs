using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//General
	public float speedMod = 5f;
	Animator anim;
	AudioSource runningSFX;
	Vector3 startPos;

	//Drinking
	bool moveLock = false;
	Vector3 enemyTargetPos;
	Vector3 startLerpPos;
	float lerpTimer = 2f;
	float xDifference;
	float yDifference;
	bool spotted = false;

	bool holdDrink = false;
	public SpriteRenderer bottle;
	public SpriteRenderer bottleArm;
	public SpriteRenderer normalArm;

	void Start () {
		startPos = transform.position;
		anim = GetComponent<Animator> ();
		runningSFX = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (holdDrink) {
			bottle.enabled = true;
			bottleArm.enabled = true;
			normalArm.enabled = false;
		} else {
			normalArm.enabled = true;
			bottle.enabled = false;
			bottleArm.enabled = false;
		}

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

		lerpTimer += 0.5f * Time.deltaTime;
		if (lerpTimer < 1f && spotted) {
			xDifference = Mathf.Abs(enemyTargetPos.x - transform.position.x);
			yDifference = Mathf.Abs(enemyTargetPos.y - transform.position.y);
			if ((xDifference > 4f) || (yDifference > 4f)){
				transform.position = Vector3.Lerp (startLerpPos, enemyTargetPos, lerpTimer);
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
			Camera.main.SendMessage ("Hangover");
			LockMovement ();
			startLerpPos = transform.position;
			enemyTargetPos = enemypos;
			Invoke ("MoveTo", 2f);
			spotted = true;
		}
	}

	void MoveTo(){
		lerpTimer = 0f;
		holdDrink = true;
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
		spotted = false;
		holdDrink = false;
	}

	void NewDay(){
		transform.position = startPos;
		UnlockMovement ();
	}
}
