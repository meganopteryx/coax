using UnityEngine;
using System.Collections;

public class InitializeTitle : MonoBehaviour {

    public GameObject entityType;

	// Use this for initialization
	void Start () 
    {
        GameObject title = Instantiate(entityType, new Vector3(0, 0, -11), Quaternion.identity) as GameObject;
        title.transform.parent = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
