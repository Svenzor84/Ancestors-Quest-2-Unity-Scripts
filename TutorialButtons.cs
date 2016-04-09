/*
 *  Title:       TutorialButtons.cs
 *  Author:      Steve Ross-Byers
 *  Created:     01/31/2016
 *  Modified:    01/31/2016
 *  Resources:   Written with the Unity API
 *  Description: This script handles the activation of the tutorial panels game object, as well as cycling through the set of tutorial panel children within
 */

using UnityEngine;
using System.Collections;

public class TutorialButtons : MonoBehaviour {

	//reference to the Tutorial Panels game objects
	public GameObject tutorialPanels;
	public GameObject Panel2;
	public GameObject Panel3;
	public GameObject Panel4;
	public GameObject Panel5;
	public GameObject Panel6;

	//function that activates or deactivates the tutorial panels
	public void activatePanels(int activate) {

		//if activate is 0
		if (activate == 0) {

			//deactivate the panels
			tutorialPanels.SetActive (false);

			//if activate is 1
		} else if (activate == 1) {

			//activate the panels
			tutorialPanels.SetActive (true);
		
		//otherwise
		} else {

			//do nothing
		}
	}

	//function that controls the tutorial buttons
	public void UseButton(int button) {

		//switch on the argument
		switch (button) {

			//first case is for panel number 2
			case 1:

				//if panel 2 is active
				if (Panel2.activeInHierarchy) {
					
					//deactivate it
					Panel2.SetActive (false);

				//otherwise
				} else {
					
					//activate panel 2
					Panel2.SetActive (true);
				}
			break;

			//second case is for panel number 3
			case 2:
			
				//if panel 3 is active
				if (Panel3.activeInHierarchy) {
				
					//deactivate it
					Panel3.SetActive (false);
				
				//otherwise
				} else {
				
					//activate panel 3
					Panel3.SetActive (true);
				}
				break;

			//third case is for panel number 4
			case 3:
			
				//if panel 4 is active
				if (Panel4.activeInHierarchy) {
				
					//deactivate it
					Panel4.SetActive (false);
				
				//otherwise
				} else {
				
					//activate panel 4
					Panel4.SetActive (true);
				}
				break;

			//fourth case is for panel number 5
			case 4:
			
				//if panel 5 is active
				if (Panel5.activeInHierarchy) {
				
					//deactivate it
					Panel5.SetActive (false);
				
				//otherwise
				} else {
				
					//activate panel 5
					Panel5.SetActive (true);
				}
				break;

			//fifth case is for panel number 6
			case 5:
			
				//if panel 6 is active
				if (Panel6.activeInHierarchy) {
				
					//deactivate it
					Panel6.SetActive (false);
				
				//otherwise
				} else {
				
					//activate panel 6
					Panel6.SetActive (true);
				}
				break;
		}

	}

}
