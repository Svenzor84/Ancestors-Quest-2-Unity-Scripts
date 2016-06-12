/*
 *  Title:       Descendant.cs
 *  Author:      Steve Ross-Byers
 *  Created:     01/31/2015
 *  Modified:    04/08/2016
 *  Resources:   Written using the Unity API
 *  Description: This script controls the object that represents the player in the Main Menu opening scene, triggers movements and Menu effects such as starting the game
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Descendant : MonoBehaviour {

	//stuff happens when the Descendant object touches a trigger
	public void OnTriggerEnter2D(Collider2D other) {

		//if the trigger is the start game trigger
		if (other.tag == "Start") {

			//deactivate the trigger
			other.gameObject.SetActive (false);

			//Begin the Fade Out with a timed delay
			StartCoroutine("DelayFade", 0.75f);
		}

	}

	//this function is used to wait for a fade out before loading the main game from the main menu
	protected IEnumerator DelayFade (float delayTime) {

		//grab a reference to the Scene Effects script on the Cloud game object
		SceneEffects Effects = GameObject.Find ("Cloud").GetComponent<SceneEffects> ();

		//set the fade speed
		Effects.fadeSpeed = 0.9f;

		//set the color of the fade to black
		Effects.screenColor = new Color (0.0f, 0.0f, 0.0f, 0.0f);

		//initiate the fade out
		Effects.BeginFade (1);

		//wait for the fade to end
		yield return new WaitForSeconds (delayTime);

		//detstroy the sound manager
		SoundManager.Destroy(SoundManager.instance.gameObject);
		
		//load the Main Scene to start the game
		SceneManager.LoadScene ("Main");
	}
}
