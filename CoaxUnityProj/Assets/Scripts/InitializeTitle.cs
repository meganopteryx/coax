using UnityEngine;
using System.Collections;

public class InitializeTitle : MonoBehaviour {

    public GameObject entityType;

	// Use this for initialization
	void Start () {
        Instantiate(entityType, new Vector3(0, 0, -11), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
