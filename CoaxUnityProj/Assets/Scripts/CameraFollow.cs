using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float camRotateLerp = 6.0f;
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
        Vector3 campos = camera.transform.position;
        Player p = player.GetComponent<Player>();
        float targetAngle;

        if (p.isConversing)
        {
            float distance = 13f;
            Vector3 targetPosition = Vector3.Lerp(
               campos,
               (p.transform.position + p.engagedStranger.transform.position) / 2,
               Time.deltaTime * camMoveLerp);
            targetPosition.z = Mathf.Lerp(campos.z, -distance, Time.deltaTime * camZoomLerp);
            camera.transform.position = Vector3.Lerp(campos, targetPosition, Time.deltaTime * camZoomLerp);

            targetAngle = Mathf.Atan2(
                p.engagedStranger.transform.position.y - p.transform.position.y,
                p.engagedStranger.transform.position.x - p.transform.position.x);
        }
        else
        {
            //Set Distance
            float distance = (player.rigidbody.velocity.magnitude * camMaxDist) + 15;

            //Lerp Position and z-depth
            Vector3 targetPosition = Vector3.Lerp(campos, player.transform.position, Time.deltaTime * camMoveLerp);
            targetPosition.z = Mathf.Lerp(campos.z, -distance, Time.deltaTime * camZoomLerp);
            camera.transform.position = Vector3.Lerp(campos, targetPosition, Time.deltaTime * camZoomLerp);

            targetAngle = 0;
        }

        camera.transform.rotation = Quaternion.Lerp(
            camera.transform.rotation,
            Quaternion.Euler(0, 0, targetAngle * 180f / Mathf.PI),
            Time.deltaTime * camRotateLerp);

        //Debug.Log("Dist:"+ distance);
    }
}
