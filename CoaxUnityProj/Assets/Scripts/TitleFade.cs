using UnityEngine;
using System.Collections;

public class TitleFade : MonoBehaviour {

    void setAlpha(float alpha)
    {
        Color oldColor = renderer.material.GetColor("_TintColor");
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        renderer.material.SetColor("_TintColor", newColor);
	}

	// Coroutine for fading from one alpha to another over a given time
	IEnumerator fade(float startAlpha, float endAlpha, float totalTime)
	{
		float time = 0;
		float alpha;
		float deltaAlpha = endAlpha - startAlpha;
		float minAlpha = startAlpha;
		float maxAlpha = endAlpha;
		if (startAlpha > endAlpha) {
			minAlpha = endAlpha;
			maxAlpha = startAlpha;
		}
		while (true) {
			alpha = startAlpha + deltaAlpha*time/totalTime;
			alpha = Mathf.Clamp (alpha, minAlpha, maxAlpha);
			setAlpha(alpha);
			if (alpha == endAlpha) {
				break;
			}
			else {
				yield return null;
			}
			time += Time.deltaTime;
		}
	}
	
	// Wrapper coroutines for fading in and out
	IEnumerator fadeIn(float totalTime)
	{
		yield return StartCoroutine(fade(0,1,totalTime));
	}
	IEnumerator fadeOut(float totalTime)
	{
		yield return StartCoroutine(fade(1,0,totalTime));
	}

	void Start ()
    {
		StartCoroutine (script ());
	}

	IEnumerator script() {
		setAlpha(0);
		yield return new WaitForSeconds(1);
		yield return StartCoroutine (fadeIn (1));
		yield return new WaitForSeconds(2);
		yield return StartCoroutine (fadeOut (1));
		Destroy(gameObject);
	}

	
	// Update is called once per frame
	void Update () {
	}
}
