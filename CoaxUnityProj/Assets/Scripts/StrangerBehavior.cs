using UnityEngine;
using System.Collections;

public class StrangerBehavior : MonoBehaviour {
	
	public float maxForce = 50;
	public float maxVelocity = 5;
	public Texture2D trueAppearance;
	
	private bool following = false;
	private bool revealed = false; //this might be redundant with following
	private float minDistance = 1; //distance from player
	private float maxDistance = 3;
	
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		if(rigidbody){
			//rigidbody.velocity.Set(Random.Range (0,maxVelocity),Random.Range (0,maxVelocity),0);
			rigidbody.AddForce(new Vector3(Random.Range (-maxForce,maxForce),Random.Range (-maxForce,maxForce),0));
		}
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//add logic for when it is hit with sound wave here
		//if hit with soundwave go to quicktime event thingy minigame
		//if in quicktime event, do not move, interact differently
		//else
		if(following && player != null){
			if(Vector3.Distance(player.position, transform.position) > maxDistance){
				Vector3.MoveTowards(transform.position,player.position,maxVelocity);
			}
		}
	}
	
	
	public void HearPlayer(){
		
	}
	
}
