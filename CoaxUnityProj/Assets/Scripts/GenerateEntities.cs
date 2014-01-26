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
        Transform bounds = GameObject.Find("Boundaries").transform;
		
		if(entityType){
			for(int i=0;i<=numOfEntities;i++){
				GameObject entity = Instantiate(entityType,new Vector3(Random.Range (minX,maxX),Random.Range(minY,maxY),-9+Random.Range(-0.5f,0.5f)),Quaternion.identity) as GameObject;
                entity.GetComponent<KeepInBounds>().bounds = bounds;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
