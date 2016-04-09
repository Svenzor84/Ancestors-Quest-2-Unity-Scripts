/*
 *  Title:       Floor.cs
 *  Author:      Steve Ross-Byers
 *  Created:     03/19/2016
 *  Modified:    03/20/2016
 *  Resources:   Written with the unity API
 *  Description: This script detects double clicks (also defines the amount of time allowed between clicks) and estimates the position of the double click (for determining which floor tile the user clicked on)
 */

using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	//variables used for determining if a click happened on this floor tile
	private float mouseX;
	private float mouseY;
	private bool firstClick = false;
	private float doubleClickTimer;
	private float doubleClickDelay = 0.25f;
	private Camera mainCam;

	// Use this for initialization
	void Start () {
	
		//initialize the mainCam variable
		mainCam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		//track the movement of the mouse
		mouseX = Mathf.Abs (Input.mousePosition.x - mainCam.WorldToScreenPoint(this.transform.position).x);
		mouseY = Mathf.Abs (Input.mousePosition.y - mainCam.WorldToScreenPoint(this.transform.position).y);

		//if the mouse is hovering over this floor tile
		if ((mouseX < 45) && (mouseY < 45)) {

			//if the player clicks the left mouse button
			if (Input.GetMouseButtonDown (0) && GameManager.instance.playersTurn && !GameObject.Find ("Player").GetComponent<Player>().playerSlowBool ()) {

				//if there has not been a first click
				if (!firstClick) {

					//acknowledge the first click
					firstClick = true;

					//save the time of the first click
					doubleClickTimer = Time.time;

					//otherwise, there has already been one click
				} else {

					//reset the double click check
					firstClick = false;

					//call the doubleClick function from the player script
					GameObject.Find ("Player").GetComponent<Player>().doubleClick (this.transform.position);
				}
			}
		}

		//check the double click timer, and if it has been too long
		if ((Time.time - doubleClickTimer) > doubleClickDelay) {

			//reset the current double click cycle
			firstClick = false;
		}
	}	
}
