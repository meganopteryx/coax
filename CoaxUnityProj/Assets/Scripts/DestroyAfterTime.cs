using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	
	public float waitSeconds = 2;
	public float fadeSeconds = 1;
	
	private bool isFading = false;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(DestroySelf());
		if(fadeSeconds < waitSeconds){
			StartCoroutine(StartFading());
		}
	}
	
	void FixedUpdate(){
		if(isFading){
			//lerp alpha to end
		}
	}
	
	IEnumerator StartFading(){
		yield return new WaitForSeconds(waitSeconds - fadeSeconds);
		isFading = true;
	}
	
	IEnumerator DestroySelf(){
		yield return new WaitForSeconds(waitSeconds);
		Destroy(gameObject);
	}
}
