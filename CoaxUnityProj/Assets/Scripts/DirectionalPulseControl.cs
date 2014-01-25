using UnityEngine;
using System.Collections;

//attach this script to player for interactivity
public class DirectionalPulseControl : MonoBehaviour {
	
	public GameObject pulseObject;
	
	private float pulseDelaySeconds = .2f;
	
	// Use this for initialization
	void Start () {
		//set key 
		if( Input.GetKeyDown(KeyCode.Space)|| Input.GetMouseButtonDown(0)){
			Instantiate(pulseObject,transform.position,transform.rotation);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
