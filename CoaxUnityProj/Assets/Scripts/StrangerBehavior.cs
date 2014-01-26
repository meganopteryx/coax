using UnityEngine;
using System.Collections;

public class StrangerBehavior : MonoBehaviour {
	
	public float maxForce = 50;
	public float maxVelocity = 5;
	//these should match with the types
	public AudioClip[] responseSounds;
	public Texture2D[] trueAppearances;
	
	[HideInInspector]
	public int strangerType = 0;
	[HideInInspector]
	public bool following = false;
	
	private float minDistance = 1; //distance from player
	private float maxDistance = 3;
	
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		if(rigidbody){
			rigidbody.AddForce(new Vector3(Random.Range (-maxForce,maxForce),Random.Range (-maxForce,maxForce),0));
		}
		player = GameObject.Find("Player");
		strangerType = Random.Range(0,trueAppearances.Length);
	}
	
	void FixedUpdate () {
		//if not in quicktime event
		if(following && player != null && !player.GetComponent<Player>().isConversing){
			if(Vector3.Distance(player.transform.position, transform.position) > maxDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
			}
			else if(Vector3.Distance(player.transform.position, transform.position) > minDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*Random.Range(.2f,.6f));
			}
		}
	}
	
	void OnCollisionEnter(Collision c){
        if(following)
			return;
		if(c.collider.tag == "ActionPulse")
        {
			if(responseSounds[strangerType])
				audio.PlayOneShot(responseSounds[strangerType]);
            Debug.Log("col enter");
			HearPlayer();
			//Destroy(c.collider.gameObject);
			c.collider.GetComponent<Collider>().enabled = false;
		}
	}
	
	void OnTriggerEnter(Collider c){
        if(following)
			return;
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
		following = true;
		renderer.material.mainTexture = trueAppearances[strangerType]; //instant switch

        player.GetComponent<Player>().startConversingWith(gameObject);
	}
	
}
