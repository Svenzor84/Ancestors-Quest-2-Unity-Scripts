/*
 *  Title:       SoundManager.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/31/2015
 *  Modified:    02/20/2016
 *  Resources:   Adapted from original soundmanager script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: This script handles playing and randomization of sound effects (slight changes in pitch)
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
	
	//variables for the audio sources (sound effect and music)
	public AudioSource efxSource;
	public AudioSource musicSource;

	//variables that hold the choices for game music
	public AudioClip Gavotte;
	public AudioClip Apoc;
	public AudioClip Metal;
	public AudioClip Warlock;
	public AudioClip Intense;
	public AudioClip Pyramid;
	public AudioClip Morbid;
	public AudioClip Sheriff;
	public AudioClip Escape;

	//allows us to access all public functions and variables from any script in the game
	public static SoundManager instance = null;

	//variables used to add random variation to the pitch of sound effects
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	//awake function will use a singleton pattern, as with the gameManager
	void Awake () {

		//check to see is there is an instance of Sound Manager
		if (instance == null) {

			//if not create one
			instance = this;

		//if there is an instance of Sound Manager that isnt this one
		} else if (instance != this){

			//destroy it
			Destroy (gameObject);
		}

		//keep the Sound Manager alive between rooms so the music will keep playing
		DontDestroyOnLoad(gameObject);

		//if the current loaded level is the Main scene
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Main") {

			//set the volume of the game music clip to 0.2
			musicSource.volume = 0.5f;

			//set the volume of the sound effect clip to 0.2
			efxSource.volume = 0.5f;

			//set the game music clip to the Warlock music clip and play it
			musicSource.clip = Warlock;

			if(musicSource.isActiveAndEnabled) {
				musicSource.Play ();
			}
		}
	}

	//function that plays different audio clips
	public void PlaySingle (AudioClip clip) {

		//set our sound effects source to the clip passed into the function
		efxSource.clip = clip;

		//play the audio clip associated with our sound effect source
		efxSource.Play ();
	}

	//function to slightly randomize the sound effects (takes an array of audio clips as parameter)
	public void RandomizeSfx (params AudioClip [] clips){

		//get and retain a random number between zero and the length of our clips array
		int randomIndex = Random.Range (0, clips.Length);

		//get and retain another random number between our low and high pitch range variables
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		//set the pits of our sound effect to the randomly chosen pitch
		efxSource.pitch = randomPitch;

		//add the random clip to our array of clips at the random index
		efxSource.clip = clips [randomIndex];

		//play the clip
		efxSource.Play ();
	}

	void Update() {

		//adding music loop
		if (musicSource.time >= musicSource.clip.length - 0.5f) {

			CycleMusic ();
		}
	}

	public void CycleMusic() {

		//if the music track is currently playing Gavotte
		if (SoundManager.instance.musicSource.clip == SoundManager.instance.Gavotte) {

			//switch to the Metal tack
			SoundManager.instance.musicSource.clip = SoundManager.instance.Metal;

			//start the song
			SoundManager.instance.musicSource.Play ();

			//if the music track is currently playing Metal
		} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Metal) {

			//switch to the Apocaclypse track
			SoundManager.instance.musicSource.clip = SoundManager.instance.Apoc;

			//start the song
			SoundManager.instance.musicSource.Play ();

			//if the music track is currently playing Apoc
		} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Apoc) {

			//switch to the Warlock track
			SoundManager.instance.musicSource.clip = SoundManager.instance.Warlock;

			//start the song
			SoundManager.instance.musicSource.Play ();

			//if the music track is currently playing Warlock
		} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Warlock) {

			//switch to the Morbid track
			SoundManager.instance.musicSource.clip = SoundManager.instance.Morbid;

			//start the song
			SoundManager.instance.musicSource.Play ();

		} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Morbid) {

			//switch to the Pyramid track
			SoundManager.instance.musicSource.clip = SoundManager.instance.Pyramid;

			//start the song
			SoundManager.instance.musicSource.Play ();

		} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Pyramid) {

			//switch to the Intense track
			SoundManager.instance.musicSource.clip = SoundManager.instance.Intense;

			//start the song
			SoundManager.instance.musicSource.Play ();

		} else {

			//switch the track back to Gavotte
			SoundManager.instance.musicSource.clip = SoundManager.instance.Gavotte;

			//start the song
			SoundManager.instance.musicSource.Play ();

		}
	}
}
