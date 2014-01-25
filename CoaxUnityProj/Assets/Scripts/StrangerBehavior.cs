using UnityEngine;
using System.Collections;

public class StrangerBehavior : MonoBehaviour {
	
	public float maxVelocity = 50;
	
	// Use this for initialization
	void Start () {
		if(rigidbody){
			//rigidbody.velocity.Set(Random.Range (0,maxVelocity),Random.Range (0,maxVelocity),0);
			rigidbody.AddForce(new Vector3(Random.Range (-maxVelocity,maxVelocity),Random.Range (-maxVelocity,maxVelocity),0));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
