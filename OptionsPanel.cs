/*
 *  Title:       OptionsPanel.cs
 *  Author:      Steve Ross-Byers
 *  Created:     01/03/2016
 *  Modified:    03/27/2016
 *  Resources:   Written with the Unity API
 *  Description: This script handles UI button presses to open and close the options panel, mute and unmute sound effects and music, and change the sound track; also sets visual display of sound effects and music volume
 */

using UnityEngine;
using System.Collections;
//so we can access the UI
using UnityEngine.UI;
public class OptionsPanel : MonoBehaviour {

	//store a reference to the options menu so we can enable and disable it when the options button is clicked
	private GameObject optionsPanel;

	//text to display for the song text, music and sound effects volume
	public Text musicVolume;
	public Text efxVolume;
	public Text songText;

	void Start () {
	
		//get the reference to the options panel
		optionsPanel = GameObject.Find ("options");

		//set the options panel to inactive until the button gets clicked
		optionsPanel.SetActive (false);
	}

	void Update () {

		//if the user presses the escape button
		if (Input.GetKeyDown (KeyCode.Escape)) {

			//open or close the options panel
			openOptionsPanel ();
		}
	}

	//function that opens or closes the options panel when the button is clicked
	public void openOptionsPanel () {

		//if the panel is already active
		if (optionsPanel.activeInHierarchy) {
			
			//deactivate it
			optionsPanel.SetActive (false);

			//otherwise
		} else {
			
			//activate it
			optionsPanel.SetActive (true);

			//set the music UI text object to and empty string
			musicVolume.text = "";

			//if the music source is muted
			if (SoundManager.instance.musicSource.mute) {

				//do nothing

			//otherwise set the music UI text object according to the music volume
			} else if (SoundManager.instance.musicSource.volume > 0.9) {
				
				musicVolume.text = "||||||||||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.8) {
				
				musicVolume.text = "|||||||||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.7) {
				
				musicVolume.text = "||||||||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.6) {
				
				musicVolume.text = "|||||||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.5) {
				
				musicVolume.text = "||||||";
				
			}  else if (SoundManager.instance.musicSource.volume > 0.4) {
				
				musicVolume.text = "|||||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.3) {
				
				musicVolume.text = "||||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.2) {
				
				musicVolume.text = "|||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.1) {
				
				musicVolume.text = "||";
				
			} else if (SoundManager.instance.musicSource.volume > 0.0) {
				
				musicVolume.text = "|";
				
			}
			
			//set the sound effects UI text object to an empty string
			efxVolume.text = "";

			//if the sound effects source is muted
			if (SoundManager.instance.efxSource.mute) {

				//do nothing

			//otherwise set the sound effect UI text object according to the sound effects volume
			} else if (SoundManager.instance.efxSource.volume > 0.9) {
				
				efxVolume.text = "||||||||||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.8) {
				
				efxVolume.text = "|||||||||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.7) {
				
				efxVolume.text = "||||||||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.6) {
				
				efxVolume.text = "|||||||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.5) {
				
				efxVolume.text = "||||||";
				
			}  else if (SoundManager.instance.efxSource.volume > 0.4) {
				
				efxVolume.text = "|||||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.3) {
				
				efxVolume.text = "||||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.2) {
				
				efxVolume.text = "|||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.1) {
				
				efxVolume.text = "||";
				
			} else if (SoundManager.instance.efxSource.volume > 0.0) {
				
				efxVolume.text = "|";
				
			}

			//set the text for the song title
			if (SoundManager.instance.musicSource.clip == SoundManager.instance.Warlock) {
				
				//set the song tesxt to warlock
				songText.text = "Warlock";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Morbid) {
				
				//set the song text to morbid
				songText.text = "Morbid";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Pyramid) {
				
				//set the song text to morbid
				songText.text = "Pyramid";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Intense) {
				
				//set the song text to morbid
				songText.text = "Intense";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Sheriff) {
				
				//set the song text to morbid
				songText.text = "Sheriff";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Gavotte) {
				
				//set the song text to morbid
				songText.text = "Gavotte";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Apoc) {
				
				//set the song text to morbid
				songText.text = "Apoc";
				
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Metal) {
				
				//set the song text to morbid
				songText.text = "Metal";
			}
		}
	}
}
