using UnityEngine;
using System.Collections;

public class OmniPulse : MonoBehaviour {

    public GameObject omniPrefab;
    public float pulseFrequency = 1.5f;
    public float pulseScale = 1.2f;
    public float pulseFadeTime = 0.01f;
    Transform player;
    GameObject pulse;


	// Use this for initialization
	void Start () 
    {
        player = GameObject.Find("Player").transform;
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
            pulse = Instantiate(omniPrefab) as GameObject;
            pulse.transform.position = player.position;
            pulse.transform.rotation = player.rotation;
            Color color = pulse.renderer.material.GetColor("_TintColor");
            color.r = 1; color.g = 1; color.b = 1; color.a = 1;

            //Scale-Fade
            for (int i = 0; i < 10; i++)
            {
                //Scale
                pulse.transform.localScale = pulse.transform.localScale * pulseScale;
                //Fade
                color.g += 0.1f;
                color.a -= 0.1f;
                pulse.renderer.material.SetColor("_TintColor", color);
                yield return new WaitForSeconds(pulseFadeTime);
            }
            Destroy(pulse, 0);

            yield return new WaitForSeconds(pulseFrequency);
        }
    }
}
