using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public SpriteRenderer speechBubble;
	
	void OnTriggerEnter (Collider col) {
		if ((col.transform.position.x < transform.position.x) && transform.localScale.x > 0) {
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
			Invoke ("ResetScale", 15f);
		}

		if ((col.transform.position.x > transform.position.x) && transform.localScale.x < 0) {
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
			Invoke ("ResetScale", 15f);
		}

		speechBubble.enabled = true;
		col.SendMessage ("Spotted", transform.position, SendMessageOptions.DontRequireReceiver);
		Invoke ("Reset", 4f);
	}

	void Reset (){
		speechBubble.enabled = false;
	}

	void ResetScale(){
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
	}
	
}
