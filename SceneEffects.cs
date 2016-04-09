/*
 *  Title:       SceneEffects.cs
 *  Author:      Steve Ross-Byers
 *  Created:     01/25/2016
 *  Modified:    03/30/2016
 *  Resources:   Written with the Unity API
 *  Description: This script handles the effects for the main menu scene, including thunder, lightning, and animations for the camera, clouds, Descendant, and other objects
 */

using UnityEngine;
using System.Collections;

public class SceneEffects : MonoBehaviour {

	//texture that overlays the screen
	public Texture2D fullScreen;

	//the speed at which the screen overlay will fade
	public float fadeSpeed = 0.1f;

	//an audio clip for thunder
	public AudioClip Thunder;

	//a reference to the Play, Quit and Tutorial UI buttons
	public GameObject PlayButton;
	public GameObject QuitButton;
	public GameObject TutorialButton;

	//the screen overlay's order in the heirarchy (Small number ensures that the overlay is rendered last, on top)
	private int drawDepth = -1000;

	//the alpha value for the screen overlay
	public float alpha = 1.0f;

	//the direction to fade (either 1 or -1)
	private int fadeDir = -1;

	//the color of the screen overlay
	public Color screenColor = new Color (0.0f, 0.0f, 0.0f, 0.0f);

	void OnGUI () {

		//these are the algorithms that actually cause the screen overlay to change transparency and color
		//multiplying by Time.deltaTime converts the operation to seconds, making it easier to sync with other operations
		alpha += fadeDir * fadeSpeed * Time.deltaTime;

		//make sure that alpha is clamped to either 1 or 0 for GUI.color
		alpha = Mathf.Clamp01 (alpha);

		//alter the alpha value of the screenColor
		screenColor.a = alpha;

		//set the color for the fade
		GUI.color = screenColor;

		//set the screen overlay to be rendered on top
		GUI.depth = drawDepth;

		//draw the screen overlay to fit the entire screen
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fullScreen);
	}

	//function that begins the screen change
	public float BeginFade (int direction) {

		//set the fade direction variable equal to the direction paramter (1 to fade out, -1 to fade in)
		fadeDir = direction;

		//return a float containing the fade speed, making it easier to sync with other events
		return (fadeSpeed);
	}

	//this function is called whenever a level is successfully loaded
	void OnLevelWasLoaded () {

		//set alpha to 1 if alpha was not initialized to 1 as default
		//alpha = 1;

		//call the begin fade function (and pass in -1) to have the screen fade in
		BeginFade (-1);
	}

	//check to see what (if anything) the object is currently interacting with, and take action
	private void OnTriggerEnter2D (Collider2D other) {

		//if the object is a lightning trigger
		if (other.tag == "Lightning") {

			//deactivate the trigger object
			other.gameObject.SetActive (false);

			//reset the alpha for the screen overlay to 1
			alpha = 1;

			//ramp up the fade speed so that we get a flash instead of a slow fade
			fadeSpeed = 0.9f;

			//change the screen color to a bright yellow
			screenColor = new Color (255.0f, 252.0f, 0.0f, 0.0f);

			//call the begin fade function (and pass in -1) to have the screen flash yellow and quickly fade back in
			BeginFade (-1);

			//play the Thunder sound
			SoundManager.instance.PlaySingle (Thunder);

			//if the object is a camera move trigger
		} else if (other.tag == "CameraMove1") {

			//deactivate the trigger object
			other.gameObject.SetActive (false);

			//make the camera's first move of the Main Menu Scene (downward to view the descendant's entrance)
			GameObject.Find ("Main Camera").GetComponent<Animator> ().SetTrigger ("Move1");

			//if the object is a descendant move trigger
		} else if (other.tag == "DescendantMove1") {

			//deactivate the trigger object
			other.gameObject.SetActive (false);

			//make the descendant's first move of the Main Menu Scene (onto the left side of the scene, stopping in camera view)
			GameObject.Find ("Descendant").GetComponent<Animator> ().SetTrigger ("Move1");

			//have the descendant throw the map into the air in excitement
			GameObject.FindGameObjectWithTag ("Map").GetComponent<Animator>().SetTrigger ("Throw");

			//if the object is a descendant move trigger
		} else if (other.tag == "DescendantMove2") {
			
			//deactivate the trigger object
			other.gameObject.SetActive (false);
			
			//make the descendant's second move of the Main Menu Scene (to the right, up to the catacomb gate)
			GameObject.Find ("Descendant").GetComponent<Animator> ().SetTrigger ("Move2");

			//make the camera''s second move of the Main Menu Scene (following the descendant to the right)
			GameObject.Find ("Main Camera").GetComponent<Animator> ().SetTrigger ("Move2");

			//if the object is a Gate Open trigger
		} else if (other.tag == "OpenGate") {
			
			//deactivate the trigger object
			other.gameObject.SetActive (false);

			//deactivate the skip button
			GameObject.Find ("SkipButton").SetActive (false);

			//move the left gate half to the open position
			GameObject.Find ("GateLeft").GetComponent<Animator> ().SetTrigger ("Open");
			
			//move the right gate half to the open position
			GameObject.Find ("GateRight").GetComponent<Animator> ().SetTrigger ("Open");

			//pan the camera out (increase field of view) and move into place for end of Menu Scene
			GameObject.Find ("Main Camera").GetComponent<Animator> ().SetTrigger ("Move3");

			//fade in the Game Title to signify the end of the Main Menu Scene
			GameObject.Find ("GameTitle").GetComponent<Animator>().SetTrigger ("FadeIn");

		//if the object is the Game Title trigger
		} else if (other.tag == "ButtonsAppear") {

			//deactivate the trigger object
			other.gameObject.SetActive (false);

			//call the activate buttons function
			ActivateButtons ();

		}
	}

	//a public function that activates the buttons on the Main Menu
	public void ActivateButtons () {

		//activate the Play and Tutorial Buttons
		PlayButton.SetActive (true);
		QuitButton.SetActive (true);
		TutorialButton.SetActive (true);
	}
}