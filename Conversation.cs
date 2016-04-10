/*
 *  Title:       Conversation.cs
 *  Author:      Steve Ross-Byers
 *  Created:     04/06/2016
 *  Modified:    04/09/2016
 *  Resources:   Written using the Unity API
 *  Description: This script handles player interaction with the "kindly man" NPC, mostly concerning dialogue text
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {

	//a reference to the kindly text component element
	private Text kindlyText;

	//a reference to the scissors prefab gameobject
	public GameObject Scissors;

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

				//have the kindly man draw his scissors
				GameObject.FindWithTag ("Kindly").GetComponent<Animator>().SetTrigger ("Draw");

				//let the player know that the kindly man will gladly shave his beard
				kindlyText.text = "Yes, it looks like you have spent some time down here and could use a shave.  A massive mane seems quite majestic, but it is not a look for everyone.  Would you like me to remove your facial hair?";

				//reuse the ok button to say no to the kindly man's offer
				transform.FindChild ("okButton").FindChild ("Text").GetComponent<Text>().text = "No thanks.";
				transform.FindChild("okButton").gameObject.SetActive(true);

				//enable the accept shave button
				transform.FindChild ("acceptShave").gameObject.SetActive (true);

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

			//reset the kindly man's animation
			GameObject.FindWithTag ("Kindly").GetComponent<Animator>().SetTrigger ("Reset");

			//reset the kindly text to default
			kindlyText.text = "Hello Descendant, I am the steward of your ancient house and have long awaited your arrival.  Congratulations on finding my secret sanctuary.  I have been hiding here for years, carefully avoiding the dangerous denizens of this tomb and protecting the secrets hidden within. There is precious little I can offer in the way of help in your quest, but in whatever small way, I am yours to command.";

			//reset the ok button text
			transform.FindChild("okButton").FindChild("Text").gameObject.GetComponent<Text>().text = "Ok...";

			//deactivate the ok and accept shave buttons
			transform.FindChild("okButton").gameObject.SetActive (false);
			transform.FindChild("acceptShave").gameObject.SetActive (false);

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

			//if the player has no special items
			if (GameObject.Find ("Player").GetComponent<Player>().specialItem () == 0) {

				//let the player know that he does not have a special equipment item to trade
				kindlyText.text = "I do not see anything Special about your equipment.  When you find something interesting be sure to bring it to me and I will share any information at my disposal.  If you wish to part with a Special item I can take it off your hands in exchange for useful potions.";

			} else {

				//change the text of the ok button
				transform.FindChild("okButton").FindChild("Text").gameObject.GetComponent<Text>().text = "No Thanks.";

				//set the starter text for the kindly conversation
				kindlyText.text = "You seem to have found some Special Equipment...\n";

				//switch on the value in the special equipment inventory slot
				switch (GameObject.Find ("Player").GetComponent<Player>().specialItem ()) {

				//if the player has the fire orb
				case 1:

					//add the rest of the conversation text
					kindlyText.text += "You possess the Fire Orb:\nAn impressive relic indeed;\nwhen equipped it will allow you to cast Fireball, a powerful ranged spell that can burn your enemies (AND you) dealing damage based on your intelligence.\n(double click/tap to cast)\n";

					break;


					//FINISH UNIQUE SPECIAL ITEM TEXT AND IMPLEMENT SPECIAL ITEM REMOVAL BUTTON

				//default case does nothing
				default:
					break;

				}

				//add text that offers the player potions in exchange for the current special equipment item
				kindlyText.text += "If you care to be rid of this Special Item, I can offer you 5 health potions.";
			}
			break;

		//case 3 is for accepting the kindly man's shave offer
		case 3:

			//disable the ok and accept shave buttons
			transform.FindChild ("okButton").gameObject.SetActive (false);
			transform.FindChild ("acceptShave").gameObject.SetActive (false);

			//set the kindly conversation text to let the player know they are about to lose their beard
			kindlyText.text = "Alright, here we go!";

			//initiate the beard removal coroutine
			StartCoroutine (beardRemoval());

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

	//coroutine that handles animation and screen fade timing as well as actual removal of the player's beard
	IEnumerator beardRemoval() {

		//intiate the kindly man's cut animation
		GameObject.FindWithTag ("Kindly").GetComponent<Animator> ().SetTrigger ("Cut");

		//wait for the animation to finish
		yield return new WaitForSeconds(1.5f);

		//fade the screen out and wait for the fade to end
		yield return new WaitForSeconds(GameManager.instance.GetComponent<ScreenFade> ().BeginFade (1) + 0.5f);

		//grab a temporary reference to the player script component on the player game object
		Player tempPlayer = GameObject.Find ("Player").GetComponent<Player> ();

		//reset the player's exp spent variable
		tempPlayer.playerExpSpent (0);

		//call the level up player function with the argument 99 to reset the beard
		tempPlayer.levelUp (99);

		//set the kindly text
		kindlyText.text = "It is done.  Now you must face the terrors of this under-dark smooth faced and afraid.  I do not envy you.";

		//reset the ok button text
		transform.FindChild ("okButton").FindChild ("Text").GetComponent<Text> ().text = "Ok...";

		//activate the ok button
		transform.FindChild ("okButton").gameObject.SetActive (true);

		//return the screen from black
		GameManager.instance.GetComponent<ScreenFade> ().BeginFade (-1);
	}
}
