using UnityEngine;
using System.Collections;

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


	void Start () {
		FTB = fadeToBlack.GetComponent<Renderer> ();
	}
	

	void Update () {
	
		if (FTBLerp < 1f) {
			FTBLerp += Time.deltaTime * 0.2f;
			FTBCurrentAlpha = Mathf.Lerp (FTBStartAlpha, FTBTargAlpha, FTBLerp);
			FTB.material.SetColor ("_Color", new Color(0,0,0, FTBCurrentAlpha));
			if ((fadedIn == false) && (FTBLerp > .9f)){
				fadedIn = true;
				Invoke ("FadeIn", 2.5f);
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
		variableGameObjects.BroadcastMessage ("Cycle", hangoverState, SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessageUpwards ("NewDay");
		FTBTargAlpha = 0f;
		FTBStartAlpha = 1f;
		if (FTBLerp > 1f) {
			FTBLerp = 0f;
		}
	}

	void Hangover(){
		hangoverState ++;
	}

	void NoHangover(){
		hangoverState --;
	}
}
