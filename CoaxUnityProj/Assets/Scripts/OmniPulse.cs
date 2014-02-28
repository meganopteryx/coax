using UnityEngine;
using System.Collections;

public class OmniPulse : MonoBehaviour {

    public GameObject omniPrefab;
    float pulseScale = 1.1f;
    float pulseFadeTime = 0.01f;
    GameObject player;
    GameObject pulse;
    AudioClip sndPulse;
	
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        sndPulse = Resources.Load("sndPulse") as AudioClip;

		// NOTE: the omni pulse is now fired by the MusicConductor script in LevelController
		//	(to sync with the music)
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

	public IEnumerator coNewPulse()
	{
		if(player.GetComponent<Player>().isConversing) {
			yield break;
		}

		ArrayList swapped = new ArrayList();

		//Make A Pulse
		pulse = Instantiate(omniPrefab) as GameObject;
		pulse.transform.position = player.transform.position;
		pulse.transform.rotation = player.transform.rotation;
		
		//Set Initial Color
		Color color = pulse.renderer.material.GetColor("_TintColor");
		color.r = 0.5f; color.g = 0.5f; color.b = 0.5f; color.a = 0.5f;
		
		//PlaySound
		audio.PlayOneShot(sndPulse, 1f);
		
		//Scale-Fade
		for (int i = 0; i < 100; i++)
		{
			//Scale
			pulse.transform.localScale = pulse.transform.localScale * pulseScale;
			//Fade
			color.g += 0.01f;
			color.a -= 0.01f;
			pulse.renderer.material.SetColor("_TintColor", color);
			
			//Loop Through All Entities
			GameObject[] objs = GameObject.FindGameObjectsWithTag("Entities");
			float dist;
			foreach (GameObject obj in objs)
			{
				//CheckDist from Player
				dist = Vector3.Distance(player.transform.position, obj.transform.position);
				float scale = i;
				if (dist < scale/4 && swapped.Contains(obj) == false)
				{
					swapped.Add(obj);
					StartCoroutine(coSwapEntity(obj));
				}	
				
			}
			yield return new WaitForSeconds(pulseFadeTime);
			if(player.GetComponent<Player>().isConversing) {
				break;
			}
		}
		Destroy(pulse, 0);
	}

    IEnumerator coSwapEntity(GameObject obj)
    {
		StrangerBehavior stranger = obj.GetComponent<StrangerBehavior>();
		
		// Play random shape sound
		stranger.PlayPulse(Random.Range(0,3), 0.12f);

		// Flash the triangle over the circle
		stranger.SetTriangleAlpha(1);
        yield return new WaitForSeconds(0.1f);
		stranger.SetTriangleAlpha(0);
    }
}
