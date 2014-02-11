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
	
	// Use this for initialization
    void Start()
    {
        sndBombBlip = Resources.Load("bombBlip") as AudioClip;
        sndBomb = Resources.Load("bomb") as AudioClip;
		if(rigidbody){
			rigidbody.AddForce(new Vector3(Random.Range (-maxForce,maxForce),Random.Range (-maxForce,maxForce),0));
		}
		player = GameObject.Find("Player");
		strangerType = Random.Range(0,trueAppearances.Length);
	}
	
	void FixedUpdate () {
        //Keep Moving
        if (!player.GetComponent<Player>().isConversing)
            rigidbody.velocity = rigidbody.velocity * 1.01f;


		//if not in quicktime event
		if(following && player != null && !player.GetComponent<Player>().isConversing)
        {
            if(Vector3.Distance(player.transform.position, transform.position) > maxDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
			}
			else if(Vector3.Distance(player.transform.position, transform.position) > minDistance){
      			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*Random.Range(.2f,.6f));
			}
		}
	}
	
	void OnCollisionEnter(Collision c){
        if(following)
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
        if (following)
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

                blowingUp = true;
                player.GetComponent<Player>().stopConversing();
                StartCoroutine(coBomb());
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

        rigidbody.velocity = Vector3.zero;

		StartCoroutine(sendPings());
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
        yield return new WaitForSeconds(1f);
        follow();
        reveal();
        player.GetComponent<Player>().stopConversing();
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
            // failure
            player.GetComponent<Player>().stopConversing();

            //Boom
            blowingUp = true;
            StartCoroutine(coBomb());
        }
        else
        {
            // try again
            StartCoroutine(sendPings());
        }
    }
	
	public void reveal()
	{
		renderer.material.mainTexture = trueAppearances[strangerType]; //instant switch
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
    IEnumerator coBomb()
    {
        //Blink Color 3 times
        Color clr = renderer.material.GetColor("_TintColor");
        clr.r = 1;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.2f);
        clr.r = 0;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.2f);
        clr.r = 1;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.2f);
        clr.r = 0;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBombBlip);
        yield return new WaitForSeconds(0.2f);
        clr.r = 1;
        renderer.material.SetColor("_TintColor", clr);
        audio.PlayOneShot(sndBomb);


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
        Destroy(gameObject, 2.2f);
    }
}
