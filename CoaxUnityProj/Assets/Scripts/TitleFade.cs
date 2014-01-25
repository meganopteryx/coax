using UnityEngine;
using System.Collections;

public class TitleFade : MonoBehaviour {

	// Use this for initialization
    private float time;
    public const float durationWait = 2.0f;
    public const float durationFadeIn = 2.0f;
    public const float durationSustain = 2.0f;
    public const float durationFadeOut = 2.0f;
    public float timeWait, timeFadeIn, timeSustain, timeFadeOut;

    void setAlpha(float alpha)
    {
        Color oldColor = renderer.material.GetColor("_TintColor");
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        renderer.material.SetColor("_TintColor", newColor);
    }

    void initTimes()
    {
        time = 0;
        timeWait = durationWait;
        timeFadeIn = timeWait + durationFadeIn;
        timeSustain = timeFadeIn + durationSustain;
        timeFadeOut = timeSustain + durationFadeOut;
    }

	void Start () {
        setAlpha(0.0f);
        initTimes();
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;
        float t;

        if (time < timeWait)
        {
            setAlpha(0);
        }
        else if (time < timeFadeIn)
        {
            t = time - timeWait;
            setAlpha(t / durationFadeIn);
        }
        else if (time < timeSustain)
        {
            setAlpha(1);
        }
        else if (time < timeFadeOut)
        {
            t = time-timeSustain;
            setAlpha(1 - t / durationFadeOut);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
