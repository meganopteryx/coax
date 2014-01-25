using UnityEngine;
using System.Collections;

public class ActionPulse : MonoBehaviour {
	
	float waitSeconds = 0;
	float fadeSpeed = 0.005f;
	float scaleIncrease = 1.02f;
   	Color color; 
	
	private bool isFading = false;
	
	// Use this for initialization
	void Start () { 
		color = renderer.material.GetColor("_TintColor");
        color.r = 0.5f; color.g = 0.5f; color.b = 0.5f; color.a = 0.5f;
		StartCoroutine(StartTimer());
	}
	
	void FixedUpdate(){
        transform.localScale = transform.localScale * scaleIncrease;
		if(isFading){
        	color.a -= fadeSpeed;
       		renderer.material.SetColor("_TintColor", color);
		}
		if(color.a <= 0){
			Destroy (gameObject);
		}
	}
	
	IEnumerator StartTimer(){
		yield return new WaitForSeconds(waitSeconds);		
		isFading = true;
	}
}
