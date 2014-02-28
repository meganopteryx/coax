using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicConductor : MonoBehaviour {

	public AudioClip[] clips;
	AudioSource[] sources;
	OmniPulse omniPulse;

	const int numMeasures = 12;
	const int secondsPerMeasure = 2;
	const int samplesPerMeasure = 44100 * secondsPerMeasure;

	public delegate void MusicCallback();
	List<MusicCallback> musicCallbacks;

	// Schedules a callback to be executed at the start of the next measure.
	public void AddMusicCallback(MusicCallback callback) {
		musicCallbacks.Add(callback);
	}

	// Use this for initialization
	void Start () {

		musicCallbacks = new List<MusicCallback>();

		omniPulse = GameObject.Find("Player").GetComponent<OmniPulse>();

		// Create an AudioSource for every AudioClip we added in the editor (under LevelController)
		int numClips = clips.Length;
		sources = new AudioSource[numClips];
		for (int i=0; i<numClips; i++) {
			AudioSource src = gameObject.AddComponent<AudioSource>();
			src.clip = clips[i];
			src.mute = true;
			src.volume = 0.5f;
			sources[i] = src;
		}

		// Unmute the main background track
		unmuteTrack(0);

		// Start the music synchronizer
		StartCoroutine(PlayInSync());
	}

	public void unmuteTrack(int i)
	{
		AudioSource src = sources[i];

		src.mute = false;
		float targetVolume = src.volume;
		src.volume = 0;

		// fade in the track
		StartCoroutine(fadeInSource(src, targetVolume, 1.0f));
	}

	IEnumerator fadeInSource(AudioSource src, float volume, float time) {
		int frames = 30;
		for (int i=0; i<=frames; i++) {
			src.volume = volume * i / frames;
			yield return new WaitForSeconds(time/frames);
		}
	}
	
	IEnumerator PlayInSync() {
		// add an initial delay
		yield return new WaitForSeconds(1);

		// infinite loop that iterates "i" for every measure in the song
		for (int i=0; true; i = (i+1)%numMeasures) {

			// call all the music callbacks that were scheduled for the next measure
			foreach(MusicCallback callback in musicCallbacks) {
				callback();
			}
			musicCallbacks.Clear();
			
			// sync and restart the tracks at the start of every loop
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
