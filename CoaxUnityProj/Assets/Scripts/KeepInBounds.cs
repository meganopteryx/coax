using UnityEngine;
using System.Collections;

public class KeepInBounds : MonoBehaviour {
	
	public Transform bounds;
	public bool bounce = true;
	
	private float minX,minY,maxX,maxY;
	
	// Use this for initialization
	void Start () {       
        if(bounds){
            minX = bounds.position.x - bounds.localScale.x/2;
            minY = bounds.position.y - bounds.localScale.y/2;
            maxX = bounds.position.x + bounds.localScale.x/2;
            maxY = bounds.position.y + bounds.localScale.y/2;        
        }
        else{
            Debug.Log("No bounds, use camera bounds");
            minX = - Camera.main.orthographicSize;
            minY = - Camera.main.orthographicSize;
            maxX = Camera.main.orthographicSize;
            maxY = Camera.main.orthographicSize;
        }
	
	}
	
	//FixedUpdate is called once every so many milliseconds
	void FixedUpdate () {                
		if(transform.position.x < minX){
            transform.position = new Vector3(minX, transform.position.y,transform.position.z);
        }
        else if(transform.position.x > maxX){
            transform.position = new Vector3(maxX, transform.position.y,transform.position.z);
        }
        if(transform.position.y < minY){
            transform.position = new Vector3(transform.position.x,minY,transform.position.z);
        }
        else if(transform.position.y > maxY){
            transform.position = new Vector3(transform.position.x,maxY,transform.position.z);
        }
		
		
        if(gameObject.GetComponent<Rigidbody>()){
			if(bounce){
		        if(transform.position.x == minX || transform.position.x == maxX){
		                rigidbody.velocity.Set(-rigidbody.velocity.x,rigidbody.velocity.y,rigidbody.velocity.z);
		        }
		        if(transform.position.y == minY || transform.position.y == maxY){
		                rigidbody.velocity.Set(rigidbody.velocity.x,-rigidbody.velocity.y,rigidbody.velocity.z);
		        }
			}
			else{
		        if(transform.position.x == minX || transform.position.x == maxX){
		                rigidbody.velocity.Set(0,rigidbody.velocity.y,rigidbody.velocity.z);
		        }
		        if(transform.position.y == minY || transform.position.y == maxY){
		                rigidbody.velocity.Set(rigidbody.velocity.x,0,rigidbody.velocity.z);
		        }
			}
        }
	
	}
}
