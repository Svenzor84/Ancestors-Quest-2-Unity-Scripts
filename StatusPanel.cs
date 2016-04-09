/*
 *  Title:       StatusPanel.cs
 *  Author:      Steve Ross-Byers
 *  Created:     11/20/2015
 *  Modified:    04/06/2016
 *  Resources:   Written using the Unity  API
 *  Description: This script simply handles UI button presses to open status/inventory and help panels
 */

using UnityEngine;
using System.Collections;

public class StatusPanel : MonoBehaviour {

	//store a reference to the Stats and Inventory UI panels so we can enable and disable on level transition
	private GameObject statusPanel;
	public GameObject helpPanel;

	void Start () {

		//get the reference to the inventory/status panel
		statusPanel = GameObject.Find ("panels");
	
		//set the panel to inactive until the button gets clicked
		statusPanel.SetActive (false);
	}

	void Update () {

		//if the user presses the spacebar
		if (Input.GetKeyDown (KeyCode.Tab)) {

			//open or close the status panel
			openStatusPanel ();
		}
	}

	//function that opens or closes the panel on click or tab
	public void openStatusPanel() {

		//if the panel is already active
		if (statusPanel.activeInHierarchy) {

			//deactivate it
			statusPanel.SetActive (false);

			//deactivate the help panel as well
			helpPanel.SetActive (false);

			//otherwise
		} else {

			//activate it
			statusPanel.SetActive (true);
		}

	}

	//function that opens or closes the help panel on click
	public void openHelpPanel() {

		//if the help panel is already active
		if (helpPanel.activeInHierarchy) {

			//deactivate it
			helpPanel.SetActive (false);

			//otherwise
		} else {

			//activate it
			helpPanel.SetActive (true);
		}
	}
}
