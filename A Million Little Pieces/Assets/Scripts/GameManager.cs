using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject fadeToBlack;
	public GameObject variableGameObjects;
	float hangoverState = 0;

	
	// Fading	
	Renderer FTB;
	float FTBLerp = 2f;
	float FTBStartAlpha = 0f;
	float FTBTargAlpha = 0f;
	float FTBCurrentAlpha = 0f;
	bool fadedIn = false;
	float lerpSpeed;
	bool goodSleep = false;
	bool endGame = false;

	//Text messages
	public Text txtMsg;
	public string[] txtMessages;

	//Sounds
	public AudioClip alarm;
	float txtStartLerp;
	float txtTargLerp;
	float txtLerpTimer;
	float txtAlpha = 0f;
	AudioSource music;
	float audioStartVol;
	float audioOutTimer = 0;
	float audioVol;

	void Start () {
		music = GetComponent<AudioSource> ();
		txtMessages = new string[7];
		txtMessages [0] = "Hungover AGAIN? Don't bother coming in.";
		txtMessages [1] = "I can't believe what you did last night, we're done.";
		txtMessages [2] = "Dude, you have to stop vomiting and passing out..";
		txtMessages [3] = "I waited an hour for you last night, what the fuck is wrong with you?!";
		txtMessages [4] = "You'd better not forget to pay me back. I hope you can afford it this time m8.";
		txtMessages [5] = "You NEED to stop calling me at 4am, you're embarrassing yourself..";
		txtMessages [6] = "Our assignment was due an hour ago. Where the hell were you last night!?";
		FTB = fadeToBlack.GetComponent<Renderer> ();
		variableGameObjects.BroadcastMessage ("Cycle", hangoverState, SendMessageOptions.DontRequireReceiver);

	}
	

	void Update () {
		if (endGame) {
			audioOutTimer -= Time.deltaTime;
			audioStartVol = music.volume;
			audioVol = Mathf.Lerp (audioStartVol, 0f, audioOutTimer);
			music.volume = audioVol;
		}

		txtLerpTimer += 0.2f * Time.deltaTime;
		if (txtLerpTimer < 1f) {
			txtAlpha = Mathf.Lerp (txtStartLerp, txtTargLerp, txtLerpTimer);
		}
	
		txtMsg.color = new Color (255, 255, 255, txtAlpha);

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel("Menu");
		}

		//Lerp slower when moving to bed.
		if (FTBTargAlpha == 1.1f) {
			lerpSpeed = 0.15f;
		} else {
			lerpSpeed = 0.2f;
		}

		if (FTBLerp < 1f) {
			FTBLerp += Time.deltaTime * lerpSpeed;
			FTBCurrentAlpha = Mathf.Lerp (FTBStartAlpha, FTBTargAlpha, FTBLerp);
			FTB.material.SetColor ("_Color", new Color(0,0,0, FTBCurrentAlpha));
			if ((fadedIn == false) && (FTBLerp > .9f)){
				fadedIn = true;
				Invoke ("FadeIn", 2f);
			}
		}

	}

	void FadeToBlack(float targAlpha){
		FTBTargAlpha = targAlpha;
		FTBStartAlpha = 0f;
		if (FTBLerp > 1f) {
			FTBLerp = 0f;
			fadedIn = false;
		}
	}

	void FadeIn(){
		AudioSource.PlayClipAtPoint (alarm, transform.position, 0.5f);

		if (goodSleep && hangoverState < -2) {
			goodSleep = false;
		}

		if (!goodSleep) {
			if (hangoverState > 2 || hangoverState < -2){
				if (hangoverState > 2){
					txtMsg.text = "Life is made up of millions of little pieces,\n \nAnd every small decision defines the life you lead.\n \nMake every piece count.";
					endGame = true;
					Invoke ("EndGame", 15f);
				} else if (hangoverState < -2){
					txtMsg.text = "Life is made up of millions of little pieces,\n \nAnd every small decision defines the life you lead.\n \nMake every piece count.";
					endGame = true;
					Invoke ("EndGame", 15f);
				}
			} else {
				int RNG = Random.Range (0, txtMessages.Length);
				txtMsg.text = txtMessages[RNG];
			}
			txtLerpTimer = 0f;
			txtStartLerp = 0f;
			txtTargLerp = 1f;
			Invoke ("WipeText", 5f);
		}
		

		if (hangoverState < 2 || hangoverState > -2) {
			variableGameObjects.BroadcastMessage ("Cycle", hangoverState, SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessageUpwards ("NewDay");
			if (!endGame){
				FTBTargAlpha = 0f;
				FTBStartAlpha = 1f;
				if (FTBLerp > 1f) {
					if (goodSleep) {
						FTBLerp = 0f;
					} else {
						FTBLerp = -2f;
					}
				}
			}
		}
	}

	void Hangover(){
		hangoverState ++;
	}

	void NoHangover(){
		if (hangoverState > 0) {
			hangoverState = -1;
		} else {
			hangoverState --;
		}
	}

	void GoodSleep (bool goodSleepBool){
		goodSleep = goodSleepBool;
	}

	void WipeText(){
		txtLerpTimer = 0f;
		txtStartLerp = 1f;
		txtTargLerp = 0f;
	}

	void EndGame (){
		Application.LoadLevel ("Menu");
	}
}
