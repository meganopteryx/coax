using UnityEngine;
using System.Collections;

public class OmniPulse : MonoBehaviour {

    public GameObject omniPrefab;
    float pulseFrequency = 2.0f;
    float pulseScale = 1.1f;
    float pulseFadeTime = 0.01f;
    Transform player;
    GameObject pulse;
    AudioClip sndPulse;


	// Use this for initialization
	void Start () 
    {
        player = GameObject.Find("Player").transform;
        sndPulse = Resources.Load("sndPulse") as AudioClip;
        StartCoroutine(coPulse());
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    IEnumerator coPulse()
    {
        while (1 != 0) //ForEvaAndaDay
        {
            //Make A Pulse
            pulse = Instantiate(omniPrefab) as GameObject;
            pulse.transform.position = player.position;
            pulse.transform.rotation = player.rotation;

            //Set Initial Color
            Color color = pulse.renderer.material.GetColor("_TintColor");
            color.r = 0.5f; color.g = 0.5f; color.b = 0.5f; color.a = 0.5f;

            //PlaySound
            audio.PlayOneShot(sndPulse, 0.5f);

            //Scale-Fade
            for (int i = 0; i < 100; i++)
            {
                //Scale
                pulse.transform.localScale = pulse.transform.localScale * pulseScale;
                //Fade
                color.g += 0.01f;
                color.a -= 0.01f;
                pulse.renderer.material.SetColor("_TintColor", color);
                yield return new WaitForSeconds(pulseFadeTime);
            }
            Destroy(pulse, 0);

            yield return new WaitForSeconds(pulseFrequency);
        }
    }
}
