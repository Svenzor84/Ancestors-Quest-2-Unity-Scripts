/*
 *  Title:       Conversation.cs
 *  Author:      Steve Ross-Byers
 *  Created:     04/06/2016
 *  Modified:    04/08/2016
 *  Resources:   Written using the Unity API
 *  Description: This script handles player interaction with the "kindly man" NPC, mostly concerning dialogue text
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {

	//a reference to the kindly text component element
	private Text kindlyText;

	//function that handles button presses within the kindly conversation
	public void Converse (int choice) {

		//switch on the argument choice
		switch (choice) {

		//case 0 is for trying to get a shave
		case 0:

			//deactivate the shave and special buttons
			transform.FindChild ("shaveButton").gameObject.SetActive (false);
			transform.FindChild ("specialButton").gameObject.SetActive (false);


			//if the player has spent enough xp to grow a beard
			if (GameObject.Find ("Player").GetComponent<Player>().playerExpSpent () > 20) {



			//otherwise...
			} else {

				//let the player know he cannot have a shave yet
				kindlyText.text = "You do not seem to need a shave as of yet.  Only when you have had more experience and seen greater hardship will your chin require the blade."; 

				//activate the ok button
				transform.FindChild ("okButton").gameObject.SetActive(true);
			}

			break;

		//case 1 is for the ok button, which resets the kind converstaion text and buttons to default
		case 1:

			//reset the kindly text to default
			kindlyText.text = "Hello Descendant, I am the steward of your ancient house and have long awaited your arrival.  Congratulations on finding my secret sanctuary.  I have been hiding here for years, carefully avoiding the dangerous denizens of this tomb and protecting the secrets hidden within. There is precious little I can offer in the way of help in your quest, but in whatever small way, I am yours to command.";

			//deactivate the ok button
			transform.FindChild("okButton").gameObject.SetActive (false);

			//activate the shave and special buttons
			transform.FindChild ("shaveButton").gameObject.SetActive (true);
			transform.FindChild ("specialButton").gameObject.SetActive (true);

			break;

		//case 2 is for the special equipment button, which gives the player the option to give up their current special equipment item
		case 2:

			//deactivate the shave and special buttons
			transform.FindChild ("shaveButton").gameObject.SetActive (false);
			transform.FindChild ("specialButton").gameObject.SetActive (false);

			//activate the ok button
			transform.FindChild ("okButton").gameObject.SetActive(true);

			if (GameObject.Find ("Player").GetComponent<Player>().specialItem () == 0) {

				//let the player know that he does not have a special equipment item to trade
				kindlyText.text = "I do not see anything Special about your equipment.  When you find something interesting be sure to bring it to me and I will share any information at my disposal.  If you wish to part with a Special item I can take it off your hands in exchange for useful potions.";

			} else {

				//switch on the value in the special equipment inventory slot
				switch (GameObject.Find ("Player").GetComponent<Player>().specialItem ()) {

				case 1:
					break;

				default:
					break;

				}
			}
			break;

		default:
			break;

		}
	}

	// Use this for initialization
	void Start () {
	
		kindlyText = transform.FindChild ("kindlyText").GetComponent<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
