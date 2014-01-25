using UnityEngine;
using System.Collections;

public class TitleFade : MonoBehaviour {

	// Use this for initialization

    void setAlpha(float alpha)
    {
        Color oldColor = renderer.material.GetColor("_TintColor");
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        renderer.material.SetColor("_TintColor", newColor);
    }

	void Start () {
        setAlpha(0.2f);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
