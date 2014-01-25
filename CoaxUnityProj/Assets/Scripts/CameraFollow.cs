using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float camZoomLerp = 10.0f;
    public float camMoveLerp = 6.0f;
    public float camMaxDist = 5.0f;
    //public float maxZoomIn = 50.0f;
    //public float maxZoomOut = 200.0f;
    GameObject player;
    
	// Use this for initialization
	void Start () 
    {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
        MoveCam();
	}

    void MoveCam()
    {
        Vector3 targetPosition;
        Vector3 campos = camera.transform.position;

        //Set Distance
        float distance = (player.rigidbody.velocity.magnitude *camMaxDist)+ 15;

        //Lerp Position and z-depth
        targetPosition = Vector3.Lerp(campos, player.transform.position, Time.deltaTime * camMoveLerp);
        targetPosition.z = Mathf.Lerp(campos.z, -distance, Time.deltaTime * camZoomLerp);
        camera.transform.position = Vector3.Lerp(campos, targetPosition, Time.deltaTime * camZoomLerp);

        //Debug.Log("Dist:"+ distance);
    }
}
