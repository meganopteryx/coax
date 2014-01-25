using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float camZoomLerp = 10.0f;
    public float camMoveLerp = 6.0f;
    public float maxZoomIn = 50.0f;
    public float maxZoomOut = 200.0f;
    
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        MoveCam();
	}

    void MoveCam()
    {
        Vector3 targetPosition;
        Vector3 campos = cam.transform.position;

        //Set Distance

        float distance = Vector3.Distance(player.position, enemy.position) * 2.0f;
        if (distance < maxZoomIn)
            distance = maxZoomIn;

        //Lerp Camera in front of player on maxzoomout (see further ahead)
        if (distance > maxZoomOut)
        {
            targetPosition = Vector3.Lerp(campos, player.position + (player.forward * 40), Time.deltaTime * lerpMoveAmount);
            targetPosition.z = Mathf.Lerp(campos.z, -maxZoomOut, Time.deltaTime * lerpZoomAmount);
        }
        //Lerp Camera between player and enemy
        else
        {
            targetPosition = Vector3.Lerp(campos, (player.position + enemy.position) / 2, Time.deltaTime * lerpMoveAmount);
            targetPosition.z = Mathf.Lerp(campos.z, -distance, Time.deltaTime * lerpZoomAmount);
        }
        cam.transform.position = Vector3.Lerp(campos, targetPosition, Time.deltaTime * lerpZoomAmount);

        
    }
}
