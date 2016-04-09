/*
 *  Title:       Loader.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/25/2015
 *  Modified:    10/25/2015
 *  Resources:   Adapted from original loader script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: Simple script that creates an instance of the game manager prefab if there is not already one
 */

using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
	
	void Awake () {

		//check to see if there is an instance of GameManager already
		if(GameManager.instance == null) {

			//if not, create one
			Instantiate(gameManager);
		}
	}
}