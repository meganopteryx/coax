using UnityEngine;
using System.Collections;

public class GenerateEntities : MonoBehaviour {
	
	public GameObject entityType;
	public int numOfEntities = 1000;
	
	public float minX,maxX,minY,maxY;
	
	// Use this for initialization
	void Start () {
		//minX = minY = -5;
		//maxX = maxY = 5;
		
		if(entityType){
			for(int i=0;i<=numOfEntities;i++){
				Instantiate(entityType,new Vector3(Random.Range (minX,maxX),Random.Range(minY,maxY),-10),Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
