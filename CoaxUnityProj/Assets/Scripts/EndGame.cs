using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

    public float endGameTime = 120;
	// Use this for initialization
	void Start () 
    {
        StartCoroutine(coEndGame());	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    IEnumerator coEndGame()
    {
        //Fixed Time for endgame
        yield return new WaitForSeconds(endGameTime);

        //Get Rid of Stuff
        GameObject bounds = GameObject.Find("Boundaries");
        bounds.transform.localScale = bounds.transform.localScale * 20000;
        GameObject player = GameObject.Find("Player");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Entities");
        foreach(GameObject obj in objs)
            obj.rigidbody.AddExplosionForce(200, player.transform.position, 10000);
        //Destroy(player);
        player.renderer.enabled = false;

        GameObject endTitle = GameObject.Find("EndCredits");
        Color clr = endTitle.renderer.material.GetColor("_TintColor");
        //Fade In EndCredits
        for (int i = 0; i < 200; i++)
        {
            clr.a += 0.005f;
            endTitle.renderer.material.SetColor("_TintColor", clr);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
