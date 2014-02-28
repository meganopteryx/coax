using UnityEngine;
using System.Collections;

public class StrangerBehavior : MonoBehaviour {
	
	public float maxForce = 50;
	public float maxVelocity = 5;
	//these should match with the types
	public AudioClip[] responseSounds;
	public Texture2D[] trueAppearances;

    public Texture2D[] responseArcs;
	
	[HideInInspector]
	public int strangerType = 0;
	[HideInInspector]
	public bool following = false;
	public bool transforming = false;
	public bool revealed = false;
	
	private float minDistance = 1; //distance from player
	private float maxDistance = 3;
	
	public GameObject speakPulseObject;
	
	private GameObject player;
	
	private GameObject tempPulse;
	float AdditionalSpeed = 200;

    int numPulsesExpected;
    float responseTimeExpected;

    int numResponseTriesAllowed = 1;
    int numResponseTries;
	
	public HelpMessenger helpMessenger;

	Color origTriangleColor;
	Color origCircleColor;

	public AudioClip[] pulses;

	MusicConductor musicConductor;

	// Use this for initialization
    void Start()
	{
		musicConductor = GameObject.Find ("LevelController").GetComponent<MusicConductor>();
		helpMessenger = GameObject.Find ("LevelController").GetComponent<HelpMessenger>();

        sndBombBlip = Resources.Load("bombBlip") as AudioClip;
        sndBomb = Resources.Load("bomb") as AudioClip;
		player = GameObject.Find("Player");
		strangerType = Random.Range(0,trueAppearances.Length);
		addRandomForce();

		origTriangleColor = renderer.materials[5].GetColor("_TintColor");
		origCircleColor = renderer.materials[0].GetColor ("_TintColor");

		SetTriangleAlpha (0);
	}

	public void PlayPulse(int index, float vol) {
		audio.PlayOneShot(pulses[index], vol);
	}

	public void SetTriangleAlpha(float alpha) {
		
		
		int i = 5;//Random.Range(2, 5); //always triangle

		Color clr = origTriangleColor;
		
		//SetAllTranslucent
		clr.a = 0;
		renderer.materials[0].SetColor("_TintColor", clr);
		renderer.materials[1].SetColor("_TintColor", clr);
		renderer.materials[2].SetColor("_TintColor", clr);
		renderer.materials[3].SetColor("_TintColor", clr);
		renderer.materials[4].SetColor("_TintColor", clr);
		renderer.materials[5].SetColor("_TintColor", clr);

		if (alpha == 1) {
			//Set THE one alpha
			clr.a = 1;
			renderer.materials[i].SetColor("_TintColor", clr);
		}
		else {
			// set the circle alpha
			renderer.materials[0].SetColor("_TintColor", origCircleColor);
		}
	}

	void makeSpin()
	{
		rigidbody.angularDrag = 0;
		rigidbody.AddTorque(new Vector3(0,0,300));
	}

	public void addRandomForce()
	{
		if(rigidbody)
		{
			Vector3 dir = new Vector3(Random.Range (-maxForce,maxForce),Random.Range (-maxForce,maxForce),0);
			float angle = Mathf.Atan2(dir.y, dir.x);
			rigidbody.AddForce(dir);
			transform.rotation = Quaternion.Euler(new Vector3(0,0,angle*Mathf.Rad2Deg));
		}
	}
	
	void FixedUpdate () {
        //Keep Moving
        /*
		(Hey guys, I commented this out because the strangers stopped moving after some time.
		I guessed that this code was trying to fix this problem, but I just zeroed out the Drag
		property from the Stranger's rigid body in the editor.)
        if (!player.GetComponent<Player>().isConversing)
            rigidbody.velocity = rigidbody.velocity * 1.01f;
		*/

		//if not in quicktime event
		/*
		(I commented this out because we decided to remove the following logic from the strangers)
		if(following && player != null && !player.GetComponent<Player>().isConversing)
        {
            if(Vector3.Distance(player.transform.position, transform.position) > maxDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
			}
			else if(Vector3.Distance(player.transform.position, transform.position) > minDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*Random.Range(.2f,.6f));
			}
		}
		*/
	}
	
	void OnCollisionEnter(Collision c){
        if(revealed)
			return;
		if(c.collider.tag == "ActionPulse")
        {
			//if(responseSounds[strangerType])
			//	audio.PlayOneShot(responseSounds[strangerType]);
            Debug.Log("col enter");
			HearPlayer();
			//Destroy(c.collider.gameObject);
			c.collider.GetComponent<Collider>().enabled = false;
		}
	}

    void OnTriggerEnter(Collider c)
    {
        if (revealed)
            return;
        if (c.tag == "ActionPulse")
        {
            //Bomb or Response
            int bomb = Random.Range(0, 10);
            if (bomb == 1)
            {
                //Kill actionPulse
                GameObject[] objs = GameObject.FindGameObjectsWithTag("ActionPulse");
                foreach (GameObject obj in objs)
                    Destroy(obj);

				aboutToBlow = true;
				makeSpin ();
				musicConductor.AddMusicCallback(delegate() {
					StartCoroutine(coBomb());
				});
            }
            else
            {
                //Debug.Log("col enter");
                HearPlayer();
                //Destroy(c.collider.gameObject);
                c.GetComponent<Collider>().enabled = false;
            }
        }
    }

    void speak()
    {
        // talking one-on-one
        int i = Random.Range(0, responseSounds.Length - 1);
        audio.PlayOneShot(responseSounds[i]);

        Transform t = Instantiate(player.transform) as Transform;
        t.Rotate(new Vector3(0, 0, 180));

        tempPulse = Instantiate(speakPulseObject, transform.position, t.rotation) as GameObject;
        i = Random.Range(0, responseArcs.Length - 1);
        tempPulse.renderer.material.mainTexture = responseArcs[i];
        tempPulse.rigidbody.velocity = rigidbody.velocity;
        tempPulse.rigidbody.AddRelativeForce(new Vector3(0, AdditionalSpeed, 0));

        Destroy(t.gameObject);
    }
	
	public void engagePlayer()
    {
        numPulsesExpected = Random.Range(1, 5);
        responseTimeExpected = 1.0f + numPulsesExpected * 0.3f;
        numResponseTries = 0;

		rigidbody.angularVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;

		StartCoroutine (tryTeachingBeforeSendingPings());
	}

	IEnumerator tryTeachingBeforeSendingPings() {

		// This will return immediately if we've already taught the player
		yield return StartCoroutine(helpMessenger.teachConversation());

		// Start sending pings at the next musical measure
		// (so they're singing to the rhythm)
		musicConductor.AddMusicCallback(delegate() {
			StartCoroutine(sendPings());
		});
	}

	IEnumerator sendPings()
	{
        for (int i = 0; i < numPulsesExpected; i++)
        {
            speak();
            yield return new WaitForSeconds(0.5f);
        }
		player.GetComponent<Player>().requestSpeak(numPulsesExpected, responseTimeExpected);
	}

    IEnumerator win()
    {
		transforming = true;

		makeSpin();
		yield return new WaitForSeconds(0.2f);

		musicConductor.AddMusicCallback(delegate() {
			StartCoroutine(reveal());
		});
    }

    public void responseFromPlayer(int numPulses)
    {
        numResponseTries++;
        if (numPulses == numPulsesExpected)
        {
            StartCoroutine(win());
        }
        else if (numResponseTries == numResponseTriesAllowed)
		{
			aboutToBlow = true;
			makeSpin ();
			musicConductor.AddMusicCallback(delegate() {
				StartCoroutine(coBomb());
			});
        }
        else
        {
            // try again
            StartCoroutine(sendPings());
        }
    }
	
	IEnumerator reveal()
	{
		revealed = true;

		// show true texture
		renderer.material.mainTexture = trueAppearances[strangerType];

		// unmute the shape's track
		musicConductor.unmuteTrack(strangerType+1);

		// play pulse sound
		PlayPulse(strangerType, 1.0f);

		// freeze spin and make upright
		rigidbody.angularVelocity = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		// pause
		yield return new WaitForSeconds(1f);

		// go in random direction
		addRandomForce();

		// stop conversation
		player.GetComponent<Player>().stopConversing();
	}
	
	public void follow()
	{
		following = true;
	}
	
	//for others to call
	public void HearPlayer(){
        player.GetComponent<Player>().startConversingWith(gameObject);
	}


    // BOMB

    AudioClip sndBombBlip;
    AudioClip sndBomb;
    public bool blowingUp = false;
	public bool aboutToBlow = false;
    IEnumerator coBomb()
    {
        //Blink Color 3 times
        Color clr = renderer.material.GetColor("_TintColor");
        clr.r = 1;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.25f);
        clr.r = 0;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.25f);
        clr.r = 1;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.25f);
        clr.r = 0;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.25f);
        clr.r = 1;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBomb);
		blowingUp = true;


        //Blow Up
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Entities");
        foreach (GameObject obj in objs)
        {
            //Boom Baby Boom
            obj.rigidbody.AddExplosionForce(100, transform.position, 100);
        }

        for (int i = 0; i < 10; i++)
        {
            //FadeOut Circle
            clr.a -= 0.1f;
            renderer.material.SetColor("_TintColor", clr);
            transform.localScale = transform.localScale * 1.5f;
            yield return new WaitForSeconds(0.05f);
        }
        clr.a = 0;
        renderer.material.SetColor("_TintColor", clr);
        transform.renderer.enabled = false;
		
		// stop the conversation
		player.GetComponent<Player>().stopConversing();

        Destroy(gameObject, 2.2f);

    }
}
