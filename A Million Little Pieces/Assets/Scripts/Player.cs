using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//General
	public float speedMod = 5f;
	Animator anim;
	AudioSource runningSFX;
	Vector3 startPos;
	float lerpSpeed;

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

	//Find Bed
	public GameObject bed;
	Vector3 bedDir;
	float minBedTimer = 0f;

	//Sounds
	public AudioClip yawn;
	public AudioClip crowd;

	void Start () {
		startPos = transform.position;
		anim = GetComponent<Animator> ();
		runningSFX = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		minBedTimer -= Time.deltaTime;
		//Find bed
		if ((Vector3.Distance (this.transform.position, startPos) > 60) && minBedTimer < 0f) {
			minBedTimer = 25f;
			bedDir = (transform.position - startPos).normalized;
			Object endBed = Instantiate(bed, transform.position + bedDir * 25, Quaternion.identity);
			LockMovement();
			startLerpPos = transform.position;
			enemyTargetPos = transform.position + bedDir * 25;
			Invoke ("MoveToBed", 1f);
			spotted = true;
			Destroy (endBed, 12f);
		}

		//Hold bottle when drinking
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

		//Move slower when lerping to bed
		if (minBedTimer > 15f) {
			lerpSpeed = 0.15f;
		} else {
			lerpSpeed = 0.5f;
		}

		//Lerping to drinkers
		lerpTimer += lerpSpeed * Time.deltaTime;
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
			AudioSource.PlayClipAtPoint (crowd, transform.position);
			Camera.main.SendMessage ("GoodSleep", false);
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

	void MoveToBed(){
		lerpTimer = 0f;
		Camera.main.SendMessage ("NoHangover");
		Camera.main.SendMessage ("FadeToBlack", 1.1f);
		Camera.main.SendMessage ("GoodSleep", true);
		AudioSource.PlayClipAtPoint (yawn, transform.position);
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
