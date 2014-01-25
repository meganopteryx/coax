using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float camZoomLerp = 2.0f;
    public float camMoveLerp = 2.0f;
    public float maxZoomIn = 10.0f;
    public float maxZoomOut = 10.0f;
    
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        CamFollow();
	}

    void CamFollow()
    {

    }
}
