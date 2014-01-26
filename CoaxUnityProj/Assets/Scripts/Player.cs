using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
    public enum ControlScheme {MOUSE_WSDF, CONTROLLER};
    public ControlScheme controlScheme = ControlScheme.MOUSE_WSDF;
    public float maxTurnSpeed = 10;
    public float maxSpeed = 100;
    public float thrust = 15;

    public float stageDist = 5.0f;
    public bool isConversing = false;
    public GameObject engagedStranger;

    public void startConversingWith(GameObject stranger)
    {
        isConversing = true;
        engagedStranger = stranger;
        StartCoroutine(tempConvoStopper());
    }

    IEnumerator tempConvoStopper()
    {
        yield return new WaitForSeconds(2.0f);
        stopConversing();
    }

    public void stopConversing()
    {
        isConversing = false;
        engagedStranger = null;
    }

    void Start () 
    {
       
	}

    void pointPlayerAtEngagedStranger()
    {
        // make player face stranger
        Vector3 v = engagedStranger.transform.position - transform.position;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
	
	// Update is called once per frame
	void Update () 
    {
        //Mouse-WSDF Controll Scheme
        switch (controlScheme)
        {
            case ControlScheme.CONTROLLER:
                break;
            default: //Mouse-WSDF default
                if (!isConversing) {
                    FollowMouse();
                    WSDF();
                }

                break;
        }

        if (isConversing)
        {
            pointPlayerAtEngagedStranger();
        }
	}

    void FollowMouse()
    {
        //get mouse and object screen pos
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);

        //ATan2
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

        //Set Rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0,0,angle-90)), Time.deltaTime * (maxTurnSpeed/2));
    }

    void WSDF()
    {
        //X-Axis
        rigidbody.AddForce(Input.GetAxis("Horizontal") * thrust, 0, 0);
        //Y-Axis
        rigidbody.AddForce(0, Input.GetAxis("Vertical") * thrust, 0);

        //Speed Limit
        if (rigidbody.velocity.magnitude > 0)
            rigidbody.velocity = rigidbody.velocity / rigidbody.velocity.magnitude * Mathf.Min(rigidbody.velocity.magnitude, maxSpeed);
    }
}
