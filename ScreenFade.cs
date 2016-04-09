/*
 *  Title:       ScreenFade.cs
 *  Author:      Steve Ross-Byers
 *  Created:     02/20/2016
 *  Modified:    02/21/2016
 *  Resources:   Written with the Unity API
 *  Description: This script handles fading the screen in and out (used in room transition and for screen effects like lightning)
 */

using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	//texture that overlays the screen
	public Texture2D fullScreen;
	
	//the speed at which the screen overlay will fade
	private float fadeSpeed = 1.0f;

	//the screen overlay's order in the heirarchy (Small number ensures that the overlay is rendered last, on top)
	private int drawDepth = -1000;
	
	//the alpha value for the screen overlay
	private float alpha = 0.0f;
	
	//the direction to fade (either 1 or -1)
	private int fadeDir = -1;
	
	//the color of the screen overlay
	private Color screenColor = new Color (0.0f, 0.0f, 0.0f, 0.0f);

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

	//overloaded function that begins the screen change (and allows you to set the fade speed)
	public float BeginFade (int direction, float newSpeed) {

		//set the new fadeSpeed
		fadeSpeed = newSpeed;

		//set the fade direction variable equal to the direction paramter (1 to fade out, -1 to fade in)
		fadeDir = direction;

		//return a float containing the fade speed, making it easier to sync with other events
		return (fadeSpeed);
	}

	//function that causes screen change after a delay
	protected IEnumerator DelayFade (float delayTime) {

		//initiate the fade out
		BeginFade (1);
		
		//wait for the fade to end
		yield return new WaitForSeconds (delayTime);

	}

	//overloaded function that causes screen change after a delay (and allows you to set the fade speed)
	protected IEnumerator DelayFade (float delayTime, float newSpeed) {

		//set the new fade speed
		fadeSpeed = newSpeed;

		//initiate the fade out
		BeginFade (1);
		
		//wait for the fade to end
		yield return new WaitForSeconds (delayTime);
		
	}
}
