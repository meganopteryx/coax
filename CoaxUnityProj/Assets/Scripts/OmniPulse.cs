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
        ArrayList swapped = new ArrayList();

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
            audio.PlayOneShot(sndPulse, 1.0f);

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
                    dist = Vector3.Distance(player.position, obj.transform.position);
                    float scale = i;
                    if (dist < scale/7 && swapped.Contains(obj) == false)
                    {
                        swapped.Add(obj);
                        StartCoroutine(coSwapEntity(obj));
                    }

                }
                yield return new WaitForSeconds(pulseFadeTime);
            }
            Destroy(pulse, 0);
            swapped.Clear();

            yield return new WaitForSeconds(pulseFrequency);
        }
    }

    IEnumerator coSwapEntity(GameObject obj)
    {
        int i = Random.Range(1, 4);
        obj.renderer.material = obj.renderer.materials[i];
        Color clr = obj.renderer.materials[i].GetColor("_TintColor");
        
        //SetAllTranslucent
        clr.a = 0;
        obj.renderer.materials[0].SetColor("_TintColor", clr);
        obj.renderer.materials[1].SetColor("_TintColor", clr);
        obj.renderer.materials[2].SetColor("_TintColor", clr);
        obj.renderer.materials[3].SetColor("_TintColor", clr);

        //Set THE one alpha
        clr.a = 1;
        obj.renderer.materials[i].SetColor("_TintColor", clr);
        yield return new WaitForSeconds(0.02f);

        //Set Back to Circle
        clr.a = 0;
        obj.renderer.materials[1].SetColor("_TintColor", clr);
        obj.renderer.materials[2].SetColor("_TintColor", clr);
        obj.renderer.materials[3].SetColor("_TintColor", clr);
        
        clr.a = 1;
        obj.renderer.material = obj.renderer.materials[0];
        //obj.renderer.materials[0].SetColor("_TintColor", clr);

    }
}
