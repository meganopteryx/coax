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
	void FixedUpdate () {
		//if not in quicktime event
		if(following && player != null){
			//Debug.Log("Follow playyerrrr");
			if(Vector3.Distance(player.transform.position, transform.position) > maxDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
				//rigidbody.AddForce(Vector3.MoveTowards(transform.position,player.transform.position,maxVelocity));
			}
			else if(Vector3.Distance(player.transform.position, transform.position) > minDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*Random.Range(.2f,.6f));
			}
		}
	}
	
	void OnCollisionEnter(Collision c){
        if(following)
			return;
		//add logic for when it is hit with sound wave here
		//if hit with soundwave go to quicktime event thingy minigame
		//if in quicktime event, do not move, interact differently
		if(c.collider.tag == "ActionPulse")
        {
            Debug.Log("col enter");
			HearPlayer();
			//Destroy(c.collider.gameObject);
			c.collider.GetComponent<Collider>().enabled = false;
		}
	}
	
	void OnTriggerEnter(Collider c){
        if(following)
			return;
		//add logic for when it is hit with sound wave here
		//if hit with soundwave go to quicktime event thingy minigame
		//if in quicktime event, do not move, interact differently
		if(c.tag == "ActionPulse")
        {
            Debug.Log("col enter");
			HearPlayer();
			//Destroy(c.collider.gameObject);
			c.GetComponent<Collider>().enabled = false;
		}
	}
	
	//for others to call
	public void HearPlayer(){
		//just transition to following for now & show true state		
		revealed = true;
		following = true;
		renderer.material.mainTexture = trueAppearance; //instant switch
	}
	
}
