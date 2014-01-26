using UnityEngine;
using System.Collections;

//attach this script to player for interactivity
public class DirectionalPulseControl : MonoBehaviour {
	
	public GameObject pulseObject;
	public AudioClip actionSound;
	float AdditionalSpeed = 200;
	
	private GameObject tempPulse;
	
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.Space)|| Input.GetMouseButtonDown(0)){
			Debug.Log("PRESSED SPACE");
			audio.PlayOneShot(actionSound);
			
			tempPulse = Instantiate(pulseObject,transform.position,transform.rotation) as GameObject;
			tempPulse.rigidbody.velocity = rigidbody.velocity;
			tempPulse.rigidbody.AddRelativeForce(new Vector3(0,AdditionalSpeed,0));
		}
	}
}
