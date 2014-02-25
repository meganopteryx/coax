using UnityEngine;
using System.Collections;

public class HelpMessenger : MonoBehaviour {

	public GameObject blackoutType;
	public GameObject instructionType;

	public bool lockLook;
	public bool lockMove;
	public bool lockShoot;

	GameObject blackoutObject, instructionObject;

	bool taughtConversation = false;

	public Texture2D[] instructionTextures;

	// This function sets the state of a message transition
	// with (0 <= k <= 1)
	// start = 0, end = 1
	void setTransition(float k)
	{
		setBlackoutAlpha(Mathf.Clamp (k,0,0.25f));
		//setInstructionAlpha(k);
		setInstructionScale(Mathf.Clamp(k*4, 0, 1));
	}

	void setBlackoutAlpha(float alpha)
	{
		blackoutObject.renderer.material.SetColor("_TintColor", new Color(0, 0, 0, alpha));
	}

	void setInstructionAlpha(float alpha)
	{
		Color oldColor = instructionObject.renderer.material.GetColor("_TintColor");
		Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
		instructionObject.renderer.material.SetColor("_TintColor", newColor);
	}

	void setInstructionScale(float scale)
	{
		instructionObject.transform.localScale = new Vector3(scale, scale, 1);
	}

	void setInstructionTexture(int index)
	{
		instructionObject.renderer.material.mainTexture = instructionTextures[index];
	}

	void createObjects()
	{
		// instantiate objects
		blackoutObject = Instantiate(blackoutType) as GameObject;
		instructionObject = Instantiate(instructionType) as GameObject;

		// attach to camera
		blackoutObject.transform.parent = GameObject.Find("Main Camera").transform;
		instructionObject.transform.parent = GameObject.Find("Main Camera").transform;

		// set positions (relative to camera)
		blackoutObject.transform.localPosition = new Vector3(0, 0, 1.01f);
		instructionObject.transform.localPosition = new Vector3(0,0,1);
		blackoutObject.transform.localRotation = Quaternion.identity;
		instructionObject.transform.localRotation = Quaternion.identity;

		// initialize transition state
		setTransition(0);
	}

	void destroyObjects()
	{
		Destroy(blackoutObject);
		Destroy(instructionObject);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(script());
	}

	// Coroutine for transitioning
	IEnumerator transition(float startK, float endK, float totalTime)
	{
		float time = 0;
		float k;
		float deltaK = endK - startK;
		float minK = startK;
		float maxK = endK;
		if (startK > endK) {
			minK = endK;
			maxK = startK;
		}
		while (true) {
			k = startK + deltaK*time/totalTime;
			k = Mathf.Clamp (k, minK, maxK);
			setTransition(k);
			if (k == endK) {
				break;
			}
			else {
				yield return null;
			}
			time += Time.deltaTime;
		}
	}

	// Wrapper coroutines for transitioning in and out of a message
	IEnumerator transitionIn(float totalTime)
	{
		yield return StartCoroutine(transition(0,1,totalTime));
	}
	IEnumerator transitionOut(float totalTime)
	{
		yield return StartCoroutine(transition(1,0,totalTime));
	}

	public IEnumerator teachConversation()
	{
		if (!taughtConversation) {
			createObjects();
			setInstructionTexture(5);
			yield return StartCoroutine(transitionIn(1f));
			yield return new WaitForSeconds(5.0f);
			yield return StartCoroutine(transitionOut(1f));
			destroyObjects();
		}
		taughtConversation = true;
	}

	IEnumerator script()
	{
		// lock controls in the beginning
		lockLook = lockMove = lockShoot = true;

		// wait for title screen to fade out
		yield return new WaitForSeconds(6);

		// fade in/out "you see things as yourself"
		createObjects();
		setInstructionTexture(0);
		yield return StartCoroutine(transitionIn(1f));
		yield return new WaitForSeconds(3.0f);
		yield return StartCoroutine(transitionOut(1f));
		destroyObjects();

		yield return new WaitForSeconds(3.0f);

		// show move instructions
		lockMove = false;
		createObjects();
		setInstructionTexture(1);
		yield return StartCoroutine(transitionIn(1f));
		for (int i=0; i<3; i++) {
			setInstructionTexture(1 + i);
			yield return new WaitForSeconds(1);
		}
		yield return StartCoroutine(transitionOut(1f));
		destroyObjects();

		yield return new WaitForSeconds(6.0f);

		// show look/shoot instructions
		createObjects();
		setInstructionTexture(4);
		yield return StartCoroutine(transitionIn(1f));
		yield return new WaitForSeconds(3.0f);
		yield return StartCoroutine(transitionOut(1f));
		destroyObjects();
		lockLook = lockShoot = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
