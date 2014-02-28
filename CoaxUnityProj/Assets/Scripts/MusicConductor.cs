using UnityEngine;
using System.Collections;

public class MusicConductor : MonoBehaviour {

	public AudioClip[] clips;
	AudioSource[] sources;
	OmniPulse omniPulse;

	const int numMeasures = 12;
	const int secondsPerMeasure = 2;
	const int samplesPerMeasure = 44100 * secondsPerMeasure;

	// Use this for initialization
	void Start () {

		omniPulse = GameObject.Find("Player").GetComponent<OmniPulse>();

		// Create an AudioSource for every AudioClip we added in the editor (under LevelController)
		int numClips = clips.Length;
		sources = new AudioSource[numClips];
		for (int i=0; i<numClips; i++) {
			AudioSource src = gameObject.AddComponent<AudioSource>();
			src.clip = clips[i];
			src.volume = 0.5f;
			sources[i] = src;
		}

		StartCoroutine(PlayInSync());
	}

	IEnumerator PlayInSync() {
		yield return new WaitForSeconds(1);

		// infinite loop that iterates "i" for every measure in the song
		for (int i=0; true; i = (i+1)%numMeasures) {
			
			// sync the tracks at the start of every loop
			if (i == 0) {
				foreach(AudioSource src in sources) {
					src.timeSamples = i * samplesPerMeasure;
					src.Play();
				}
			}

			// create an omni-pulse every two measures
			if (i % 2 == 0) {
				StartCoroutine(omniPulse.coNewPulse());
			}
			yield return new WaitForSeconds(secondsPerMeasure);
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
