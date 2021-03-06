﻿/*
 *  Title:       LoadOnClick.cs
 *  Author:      Steve Ross-Byers
 *  Created:     11/01/2015
 *  Modified:    04/08/2016
 *  Resources:   Written with the Unity API
 *  Description: This script handles UI button presses that restart, quit the program, quit to the main menu, and change sound options (such as sound track and volume)
 */

using UnityEngine;
using System.Collections;

//allows us to access unity UI features and scene management
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

	//variable that holds the Song UI text, for switching the current music track that is playing
	public Text songText;

	//text to display for the music and sound effects volume
	public Text musicVolume;
	public Text efxVolume;

	private bool music_done = false;
	private float music_end = 0.0f;

	//called once per frame
	void Update () {

	}

	//function that loads the main scene of quits the application
	public void LoadScene (int retry){

		//check to see if zero was passed into the funtion as a parameter
		if (retry == 0) {

			//if so, set the game managers retry bool to true
			GameManager.instance.retry = true;

			//and load the main scene
			SceneManager.LoadScene ("Main");
		}

		//check to see if 1 was passed into the function as a parameter
		if (retry == 1) {

			//if so, quit the application
			Application.Quit ();
		}

		//check to see if 2 was passed into the function as a parameter
		if (retry == 2) {

			//if so, kill the SoundManager
			SoundManager.Destroy (SoundManager.instance.gameObject);

			//and the GameManager
			GameManager.Destroy (GameManager.instance.gameObject);

			//and then load the menu scene
			SceneManager.LoadScene ("Menu");
		}

		//check to see if 3 was passed into the function as a parameter
		if (retry == 3) {

			//if so set the game manager continue bool to true
			GameManager.instance.continueGame = true;

			//and load the main scene
			SceneManager.LoadScene ("Main");
		}
	}

	//function that changes the in game music and toggles muting for music and sound effects
	public void ChangeMusic(int track) {

		//switch on track to decide which button was pressed
		switch (track) {

		//case 0 is for muting the sound effects
		case 0:

				//if the sound effects source is already muted
			if (SoundManager.instance.efxSource.mute) {

				//unmute the sound effects source
				SoundManager.instance.efxSource.mute = false;

				if (SoundManager.instance.efxSource.volume > 0.91) {

					efxVolume.text = "||||||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.81) {

					efxVolume.text = "|||||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.71) {

					efxVolume.text = "||||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.61) {

					efxVolume.text = "|||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.51) {

					efxVolume.text = "||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.41) {

					efxVolume.text = "|||||";

				} else if (SoundManager.instance.efxSource.volume > 0.31) {

					efxVolume.text = "||||";

				} else if (SoundManager.instance.efxSource.volume > 0.21) {

					efxVolume.text = "|||";

				} else if (SoundManager.instance.efxSource.volume > 0.11) {

					efxVolume.text = "||";

				} else if (SoundManager.instance.efxSource.volume > 0.01) {

					efxVolume.text = "|";

				}

				//otherwise the sound effects source is un-muted
			} else {

				//so mute the sound effects source
				SoundManager.instance.efxSource.mute = true;

				efxVolume.text = "";
			}
			break;

		//case 1 is for muting the game music
		case 1:

				//if the music source is already muted
			if (SoundManager.instance.musicSource.mute) {

				//umute the music source
				SoundManager.instance.musicSource.mute = false;

				if (SoundManager.instance.musicSource.volume > 0.91) {

					musicVolume.text = "||||||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.81) {

					musicVolume.text = "|||||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.71) {

					musicVolume.text = "||||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.61) {

					musicVolume.text = "|||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.51) {

					musicVolume.text = "||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.41) {

					musicVolume.text = "|||||";

				} else if (SoundManager.instance.musicSource.volume > 0.31) {

					musicVolume.text = "||||";

				} else if (SoundManager.instance.musicSource.volume > 0.21) {

					musicVolume.text = "|||";

				} else if (SoundManager.instance.musicSource.volume > 0.11) {

					musicVolume.text = "||";

				} else if (SoundManager.instance.musicSource.volume > 0.01) {

					musicVolume.text = "|";

				}

				//otherwise the music source is un-muted
			} else {

				//so mute the music source
				SoundManager.instance.musicSource.mute = true;

				//set the music volume UI text to display an empty string
				musicVolume.text = "";
			}

			break;

		//case 2 is for muting all game sounds
		case 2:

				//if both music and sound effects are already muted
			if (SoundManager.instance.musicSource.mute && SoundManager.instance.efxSource.mute) {

				//unmute both audio sources
				SoundManager.instance.musicSource.mute = false;
				SoundManager.instance.efxSource.mute = false;

				if (SoundManager.instance.musicSource.volume > 0.91) {

					musicVolume.text = "||||||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.81) {

					musicVolume.text = "|||||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.71) {

					musicVolume.text = "||||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.61) {

					musicVolume.text = "|||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.51) {

					musicVolume.text = "||||||";

				} else if (SoundManager.instance.musicSource.volume > 0.41) {

					musicVolume.text = "|||||";

				} else if (SoundManager.instance.musicSource.volume > 0.31) {

					musicVolume.text = "||||";

				} else if (SoundManager.instance.musicSource.volume > 0.21) {

					musicVolume.text = "|||";

				} else if (SoundManager.instance.musicSource.volume > 0.11) {

					musicVolume.text = "||";

				} else if (SoundManager.instance.musicSource.volume > 0.01) {

					musicVolume.text = "|";

				}

				if (SoundManager.instance.efxSource.volume > 0.91) {

					efxVolume.text = "||||||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.81) {

					efxVolume.text = "|||||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.71) {

					efxVolume.text = "||||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.61) {

					efxVolume.text = "|||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.51) {

					efxVolume.text = "||||||";

				} else if (SoundManager.instance.efxSource.volume > 0.41) {

					efxVolume.text = "|||||";

				} else if (SoundManager.instance.efxSource.volume > 0.31) {

					efxVolume.text = "||||";

				} else if (SoundManager.instance.efxSource.volume > 0.21) {

					efxVolume.text = "|||";

				} else if (SoundManager.instance.efxSource.volume > 0.11) {

					efxVolume.text = "||";

				} else if (SoundManager.instance.efxSource.volume > 0.01) {

					efxVolume.text = "|";

				}

				//otherwise, at least one audio source is unmuted
			} else {

				//so just mute them both
				SoundManager.instance.musicSource.mute = true;
				SoundManager.instance.efxSource.mute = true;

				efxVolume.text = "";
				musicVolume.text = "";
			}
			break;

		//case 3 is for cycling through the choices of music
		case 3:

				//if the music track is currently playing Gavotte
			if (SoundManager.instance.musicSource.clip == SoundManager.instance.Gavotte) {
					
				//switch to the Metal tack
				SoundManager.instance.musicSource.clip = SoundManager.instance.Metal;

				//switch the song text
				songText.text = "Metal";

				//start the song
				SoundManager.instance.musicSource.Play ();

				//if the music track is currently playing Metal
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Metal) {

				//switch to the Apocaclypse track
				SoundManager.instance.musicSource.clip = SoundManager.instance.Apoc;

				//switch the song text
				songText.text = "Apoc";

				//start the song
				SoundManager.instance.musicSource.Play ();

				//if the music track is currently playing Apoc
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Apoc) {

				//switch to the Warlock track
				SoundManager.instance.musicSource.clip = SoundManager.instance.Warlock;

				//switch the song text
				songText.text = "Warlock";

				//start the song
				SoundManager.instance.musicSource.Play ();

				//if the music track is currently playing Warlock
			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Warlock) {

				//switch to the Morbid track
				SoundManager.instance.musicSource.clip = SoundManager.instance.Morbid;

				//switch the song text
				songText.text = "Morbid";

				//start the song
				SoundManager.instance.musicSource.Play ();

			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Morbid) {

				//switch to the Pyramid track
				SoundManager.instance.musicSource.clip = SoundManager.instance.Pyramid;

				//switch the song text
				songText.text = "Pyramid";

				//start the song
				SoundManager.instance.musicSource.Play ();

			} else if (SoundManager.instance.musicSource.clip == SoundManager.instance.Pyramid) {

				//switch to the Intense track
				SoundManager.instance.musicSource.clip = SoundManager.instance.Intense;

				//switch the song text
				songText.text = "Intense";

				//start the song
				SoundManager.instance.musicSource.Play ();

			} else {

				//switch the track back to Gavotte
				SoundManager.instance.musicSource.clip = SoundManager.instance.Gavotte;

				//switch the song text
				songText.text = "Gavotte";

				//start the song
				SoundManager.instance.musicSource.Play ();

			}

			break;

		//case 4 is for increasing the music volume
		case 4:
				
			//add 0.20 to the music source volume
			SoundManager.instance.musicSource.volume += 0.10f;

			if (SoundManager.instance.musicSource.volume > 0.91) {

				musicVolume.text = "||||||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.81) {

				musicVolume.text = "|||||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.71) {

				musicVolume.text = "||||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.61) {

				musicVolume.text = "|||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.51) {

				musicVolume.text = "||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.41) {

				musicVolume.text = "|||||";

			} else if (SoundManager.instance.musicSource.volume > 0.31) {

				musicVolume.text = "||||";

			} else if (SoundManager.instance.musicSource.volume > 0.21) {

				musicVolume.text = "|||";

			} else if (SoundManager.instance.musicSource.volume > 0.11) {

				musicVolume.text = "||";

			} else if (SoundManager.instance.musicSource.volume > 0.01) {

				musicVolume.text = "|";

			}

			break;

		//case 5 is for decreasing the music volume
		case 5:
				
			//subtract 0.20 from the music source volume
			SoundManager.instance.musicSource.volume -= 0.10f;

			if (SoundManager.instance.musicSource.volume > 0.91) {

				musicVolume.text = "||||||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.81) {

				musicVolume.text = "|||||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.71) {

				musicVolume.text = "||||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.61) {

				musicVolume.text = "|||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.51) {

				musicVolume.text = "||||||";

			} else if (SoundManager.instance.musicSource.volume > 0.41) {

				musicVolume.text = "|||||";

			} else if (SoundManager.instance.musicSource.volume > 0.31) {

				musicVolume.text = "||||";

			} else if (SoundManager.instance.musicSource.volume > 0.21) {

				musicVolume.text = "|||";

			} else if (SoundManager.instance.musicSource.volume > 0.11) {

				musicVolume.text = "||";

			} else if (SoundManager.instance.musicSource.volume > 0.01) {

				musicVolume.text = "|";

			}

			break;

		//case 4 is for increasing the sound effects volume
		case 6:
			
			//add 0.10 to the efx source volume
			SoundManager.instance.efxSource.volume += 0.10f;

			if (SoundManager.instance.efxSource.volume > 0.91) {

				efxVolume.text = "||||||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.81) {

				efxVolume.text = "|||||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.71) {

				efxVolume.text = "||||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.61) {

				efxVolume.text = "|||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.51) {

				efxVolume.text = "||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.41) {

				efxVolume.text = "|||||";

			} else if (SoundManager.instance.efxSource.volume > 0.31) {

				efxVolume.text = "||||";

			} else if (SoundManager.instance.efxSource.volume > 0.21) {

				efxVolume.text = "|||";

			} else if (SoundManager.instance.efxSource.volume > 0.11) {

				efxVolume.text = "||";

			} else if (SoundManager.instance.efxSource.volume > 0.01) {

				efxVolume.text = "|";

			}
			
			break;
			
		//case 5 is for decreasing the sound effects volume
		case 7:
			
			//subtract 0.10 from the efx source volume
			SoundManager.instance.efxSource.volume -= 0.10f;

			if (SoundManager.instance.efxSource.volume > 0.91) {

				efxVolume.text = "||||||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.81) {

				efxVolume.text = "|||||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.71) {

				efxVolume.text = "||||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.61) {

				efxVolume.text = "|||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.51) {

				efxVolume.text = "||||||";

			} else if (SoundManager.instance.efxSource.volume > 0.41) {

				efxVolume.text = "|||||";

			} else if (SoundManager.instance.efxSource.volume > 0.31) {

				efxVolume.text = "||||";

			} else if (SoundManager.instance.efxSource.volume > 0.21) {

				efxVolume.text = "|||";

			} else if (SoundManager.instance.efxSource.volume > 0.11) {

				efxVolume.text = "||";

			} else if (SoundManager.instance.efxSource.volume > 0.01) {

				efxVolume.text = "|";

			}

			break;

		//the default case does nothing at this point
		default:
			break;
		}
	}
}