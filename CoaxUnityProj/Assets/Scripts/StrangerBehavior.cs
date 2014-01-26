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
	
	public GameObject speakPulseObject;
	
	private GameObject player;
	
	private GameObject tempPulse;
	float AdditionalSpeed = 200;
	
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
	
	public void speak()
	{
		// TODO: play random sound from bank
		
        tempPulse = Instantiate(speakPulseObject, transform.position, transform.rotation) as GameObject;
        tempPulse.rigidbody.velocity = rigidbody.velocity;
        tempPulse.rigidbody.AddRelativeForce(new Vector3(0, AdditionalSpeed, 0));
		
		StartCoroutine(waitForPlayer());
	}
	
	IEnumerator waitForPlayer()
	{
		yield return new WaitForSeconds(1.0f);
		player.GetComponent<Player>().allowSpeak();
	}
	
	public void reveal()
	{
		renderer.material.mainTexture = trueAppearances[strangerType]; //instant switch
	}
	
	public void follow()
	{
		following = true;
	}
	
	//for others to call
	public void HearPlayer(){
        player.GetComponent<Player>().startConversingWith(gameObject);
	}
	
}
