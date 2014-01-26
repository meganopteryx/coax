using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
    public enum ControlScheme {MOUSE_WSDF, CONTROLLER};
    public ControlScheme controlScheme = ControlScheme.MOUSE_WSDF;
    public float maxTurnSpeed = 10;
    public float maxSpeed = 100;
    public float thrust = 15;

    public AudioClip[] responseSounds;
	
	public GameObject speakPulseObject;

    public float stageDist = 2f;
	
	[HideInInspector]
    public bool isConversing = false;
	[HideInInspector]
	public bool canSpeak = true;
	[HideInInspector]
    public GameObject engagedStranger;
	
	int convoPingLimit = 2;
	int convoPingCount;
	
	private GameObject tempPulse;
	float AdditionalSpeed = 200;

    int numPulsesExpected;
    float responseTimeAllowed;
    int numPulsesSpoken;

    void setOtherStrangersAlpha(float alpha)
    {
        float a;
        foreach (GameObject s in GameObject.FindGameObjectsWithTag("Entities"))
        {
            a = (s == engagedStranger) ? 1f : alpha;
            Color oldColor = s.renderer.material.GetColor("_TintColor");
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, a);
            s.renderer.material.SetColor("_TintColor", newColor);
        }
    }

    // initiate conversation state variables and call on stranger to engage
    public void startConversingWith(GameObject stranger)
    {
        //gameObject.GetComponent<OmniPulse>().stopPulse();

        canSpeak = false;
        isConversing = true;
        engagedStranger = stranger;
        setOtherStrangersAlpha(0);

        convoPingCount = 0;

        StartCoroutine(waitForStranger());
    }
    IEnumerator waitForStranger()
    {
        yield return new WaitForSeconds(1f);
        engagedStranger.GetComponent<StrangerBehavior>().engagePlayer();
    }
	
    // Called by stranger initiating a conversation "test"
	public void requestSpeak(int numPulsesExpected, float responseTimeAllowed)
	{
        this.numPulsesExpected = numPulsesExpected;
        this.responseTimeAllowed = responseTimeAllowed;
        numPulsesSpoken = 0;

        canSpeak = true;
		
		StartCoroutine(respondToStranger());
    }
    IEnumerator respondToStranger()
    {
        yield return new WaitForSeconds(this.responseTimeAllowed);
        engagedStranger.GetComponent<StrangerBehavior>().responseFromPlayer(numPulsesSpoken);
    }

    // Send a single ping
    void speak()
    {
        numPulsesSpoken++;

        // talking one-on-one
        int i = Random.Range(0, responseSounds.Length - 1);
        audio.PlayOneShot(responseSounds[i]);

        tempPulse = Instantiate(speakPulseObject, transform.position, transform.rotation) as GameObject;
        tempPulse.rigidbody.velocity = rigidbody.velocity;
        tempPulse.rigidbody.AddRelativeForce(new Vector3(0, AdditionalSpeed, 0));
    }
	
	
	void updateConvoDistance()
	{
		if (!isConversing) {
			return;
		}
		var speed = 3.0f;
		
		Vector3 v = engagedStranger.transform.position - transform.position;
		Vector3 targetPosition;
		if (v.magnitude > 0) {
			targetPosition = transform.position + v.normalized * stageDist;
		}
		else {
			targetPosition = transform.position + new Vector3(0,stageDist,0);
		}
		engagedStranger.transform.position = Vector3.Lerp(
			engagedStranger.transform.position,
			targetPosition,
			Time.deltaTime * speed);
	}
	

    IEnumerator transitionConvoOut()
    {
        yield return new WaitForSeconds(1.0f);
        stopConversing();
    }

    public void stopConversing()
    {
        setOtherStrangersAlpha(1);
        isConversing = false;
        engagedStranger = null;
        StartCoroutine(startOmniPulse());
    }

    IEnumerator startOmniPulse()
    {
        yield return new WaitForSeconds(1);
        //gameObject.GetComponent<OmniPulse>().startPulse();
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
            updateConvoDistance();
            if (canSpeak && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                speak();
            }
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
