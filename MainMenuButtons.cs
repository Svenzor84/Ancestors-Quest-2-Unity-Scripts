/*
 *  Title:       MainMenuButtons.cs
 *  Author:      Steve Ross-Byers
 *  Created:     01/30/2016
 *  Modified:    01/31/2016
 *  Resources:   Written with the Unity API
 *  Description: This script handles UI button presses for the main menu, such as opening the tutorial, starting the game, quitting the application, and skipping the into scene
 */

using UnityEngine;
using System.Collections;

public class MainMenuButtons : MonoBehaviour {

	//save a reference to the Tutorial Panels
	public GameObject tutPanel;

	//function that handles the behavior of the buttons on the Main Menu (Play and Tutorial)
	public void MenuButtons(int button) {

		//switch on the int argument
		switch (button) {

			//the first case is for the Play button
			case 1:

				//find and disable the Game Title game object
				GameObject.Find ("GameTitle").SetActive (false);

				//find and disable the Menu Buttons
				GameObject.Find ("Buttons").SetActive (false);

				//find the Descendant game object and trigger his final move animation
				GameObject.Find ("Descendant").GetComponent<Animator>().SetTrigger ("Final");
				break;

			//the second case is for the Tutorial button
			case 2:

				//activate the tutorial panels
				tutPanel.SetActive (true);
				break;

			//the third case is for the Skip Button
			case 3:

				//activate the triggers for each item in the scene, skipping all the way to the end of the scene
				//move the left gate half to the open position
				GameObject.Find ("GateLeft").GetComponent<Animator> ().SetTrigger ("Open");
			
				//move the right gate half to the open position
				GameObject.Find ("GateRight").GetComponent<Animator> ().SetTrigger ("Open");

				//skip the Descendant game object to its end position
				GameObject.Find ("Descendant").GetComponent<Animator> ().SetTrigger ("Skip");

				//skip the Cloud game objects to their end positions
				GameObject.Find ("Cloud").GetComponent<Animator> ().SetTrigger ("Skip");
				GameObject.Find ("Cloud (1)").GetComponent<Animator> ().SetTrigger ("Skip");
				GameObject.Find ("Cloud (2)").GetComponent<Animator> ().SetTrigger ("Skip");

				//skip the camera to its end position
				GameObject.Find ("Main Camera").GetComponent<Animator> ().SetTrigger ("Skip");

				//skip the map to it's final resting spot
				GameObject.FindGameObjectWithTag ("Map").GetComponent<Animator>().SetTrigger ("Skip");

				//activate the Main Menu Buttons
				GameObject.Find ("Cloud").GetComponent<SceneEffects> ().ActivateButtons ();

				//skip the Game Title past its fade in
				GameObject.Find ("GameTitle").GetComponent<Animator> ().SetTrigger ("Skip");

				//deactivate the skip button
				GameObject.Find ("SkipButton").SetActive (false);

				break;

			//the fourth case is for the quit button
			case 4:

				//quit the application
				Application.Quit ();

				break;

			//the default case does nothing
			default:
				break;

		}
	}
}
