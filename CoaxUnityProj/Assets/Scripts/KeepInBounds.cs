using UnityEngine;
using System.Collections;

public class KeepInBounds : MonoBehaviour {
	
	public Transform bounds;
    //public enum WrapOptions {Stop, Bounce, Wrap};
	//public WrapOptions wrapMode = 0;
	
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
	
	void FixedUpdate () {   
		//wrap
		if(transform.position.x < minX){
            transform.position = new Vector3(maxX, transform.position.y,transform.position.z);
        }
        else if(transform.position.x > maxX){
            transform.position = new Vector3(minX, transform.position.y,transform.position.z);
        }
        if(transform.position.y < minY){
            transform.position = new Vector3(transform.position.x,maxY,transform.position.z);
        }
        else if(transform.position.y > maxY){
            transform.position = new Vector3(transform.position.x,minY,transform.position.z);
        }	
	
	}
}
