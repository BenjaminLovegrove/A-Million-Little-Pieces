using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject fadeToBlack;

	Renderer FTB;
	float FTBLerp = 2f;
	float FTBStartAlpha = 0f;
	float FTBTargAlpha = 0f;
	float FTBCurrentAlpha = 0f;


	void Start () {
		FTB = fadeToBlack.GetComponent<Renderer> ();
	}
	

	void Update () {
	
		if (FTBLerp < 1f) {
			FTBLerp += Time.deltaTime * 0.2f;
			FTBCurrentAlpha = Mathf.Lerp (FTBStartAlpha, FTBTargAlpha, FTBLerp);
			FTB.material.SetColor ("_Color", new Color(0,0,0, FTBCurrentAlpha));
		}

	}

	void FadeToBlack(float targAlpha){
		FTBTargAlpha = targAlpha;
		if (FTBLerp > 1f) {
			FTBLerp = 0f;
		}
	}
}
