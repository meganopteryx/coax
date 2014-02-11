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
        var p = GameObject.Find("Player").GetComponent<Player>();
		if( Input.GetButtonDown("Fire1"))
        {

            if (!p.isConversing)
			{
				// calling out
                audio.PlayOneShot(actionSound);

                tempPulse = Instantiate(pulseObject, transform.position, transform.rotation) as GameObject;
                tempPulse.rigidbody.velocity = rigidbody.velocity;
                tempPulse.rigidbody.AddRelativeForce(new Vector3(0, AdditionalSpeed, 0));
            }
		}
	}
}
