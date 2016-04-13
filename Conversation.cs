/*
 *  Title:       Conversation.cs
 *  Author:      Steve Ross-Byers
 *  Created:     04/06/2016
 *  Modified:    04/10/2016
 *  Resources:   Written using the Unity API
 *  Description: This script handles player interaction with the "kindly man" NPC, mostly concerning dialogue text
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {

	//a reference to the kindly text component element
	private Text kindlyText;

	//keep track of the current potion chest offer from the kindly man
	private int offer;

	//an array of sprites to set the kindly man's item with
	public Sprite[] items;

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

			//deactivate the ok, accept shave, and accept offer buttons
			transform.FindChild("okButton").gameObject.SetActive (false);
			transform.FindChild("acceptShave").gameObject.SetActive (false);
			transform.FindChild ("acceptOffer").gameObject.SetActive (false);

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

				//enable the accept offer button
				transform.FindChild ("acceptOffer").gameObject.SetActive (true);

				//set the starter text for the kindly conversation
				kindlyText.text = "You seem to have found some Special Equipment...\n";

				//switch on the value in the special equipment inventory slot
				switch (GameObject.Find ("Player").GetComponent<Player>().specialItem ()) {

				//if the player has the fire orb
				case 1:

					//add the rest of the conversation text
					kindlyText.text += "You possess the Fire Orb:\nAn impressive relic indeed;\nwhen equipped it will allow you to cast Fireball, a powerful ranged spell that can burn your enemies (AND you) dealing damage based on your intelligence.\n(double click/tap to cast)\n";

					//set the exchange offer
					offer = 2;

					break;

				//if the player has the ice orb
				case 2:
					
					//add the rest of the conversation text
					kindlyText.text += "You possess the Ice Orb:\nA quite powerful item;\nwhen equipped it will allow you to cast Ice Shards, a ranged spell that summons frost spikes that can deal damage to your enemies (AND you) based on your intelligence.\n(double click/tap to cast)\n";
					
					//set the exchange offer
					offer = 1;
					
					break;

				//if the player has the robes
				case 3:
					
					//add the rest of the conversation text
					kindlyText.text += "You possess the Robes:\nA wonderous relic;\nwhen equipped they will allow you to Teleport at a whim, blinking across the battlefield before your enemies can react.\n(double click/tap to cast)\n";
					
					//set the exchange offer
					offer = 2;
					
					break;

				//if the player has the cloak
				case 4:
					
					//add the rest of the conversation text
					kindlyText.text += "You possess the Cloak:\nIt seems a simple garment;\nhowever, when equipped it will allow you to move with magical speed, granting you an extra action each turn before your enemies can act.\n";
					
					//set the exchange offer
					offer = 1;
					
					break;

				//default case does nothing
				default:
					break;

				}

				if (offer == 1) {

					//add text that offers the player potions in exchange for the current special equipment item
					kindlyText.text += "If you care to be rid of this Special Item, I can offer you " + offer + " Potion Chest";

				} else {

					//add text that offers the player potions in exchange for the current special equipment item
					kindlyText.text += "If you care to be rid of this Special Item, I can offer you " + offer + " Potion Chests";
				}
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

		//case 4 is for accepting the kindly man's special equipment trade offer
		case 4:

			//disable the ok and accept offer buttons
			transform.FindChild ("okButton").gameObject.SetActive (false);
			transform.FindChild ("acceptOffer").gameObject.SetActive (false);

			//set the kindly conversation text to thank the player
			kindlyText.text = "Thank you very much, Descendant.\nThis item will do much to further my research.";

			//initiate item swap coroutine
			StartCoroutine (itemSwap());

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

	//coroutine that handles animation and scren fade timing as well as taking away the player's current special equipment and spawning the proper number of potion chests
	IEnumerator itemSwap() {

		//grab a temporary reference to the player script on the player gameobject
		Player tempPlayer = GameObject.Find ("Player").GetComponent<Player> ();

		//if the player has the special item currently equipped, unequip it prior to swap
		if (tempPlayer.currentEquip ("armor") == 9) {

			tempPlayer.equipArmor (9);

		} else if (tempPlayer.currentEquip ("armor") == 10) {

			tempPlayer.equipArmor (10);

		} else if (tempPlayer.currentEquip ("weapon") == 9) {

			tempPlayer.equipWeap (9);

		} else if (tempPlayer.currentEquip ("weapon") == 10) {

			tempPlayer.equipWeap (10);

		}

		//grab a temporary reference to the kindly man
		GameObject tempKindly = GameObject.FindWithTag ("Kindly");

		//initiate the kindly man's item swap animation and change the sprite renderer for the Item gameObject
		tempKindly.GetComponent<Animator> ().SetTrigger ("Swap");

		//set the kindly man's item to the player's special equipment item
		tempKindly.gameObject.transform.FindChild ("Item").GetComponent<SpriteRenderer>().sprite = items[tempPlayer.specialItem ()];

		//wait for the animation to finish
		yield return new WaitForSeconds(1.5f);

		//unset the kindly man's item
		tempKindly.transform.FindChild ("Item").GetComponent<SpriteRenderer> ().sprite = null;

		//fade the screen out and wait for the fade to end
		yield return new WaitForSeconds(GameManager.instance.GetComponent<ScreenFade>().BeginFade (1) + 0.5f);

		//remove the player's special equipment
		tempPlayer.specialItem (0);

		//set the kindly text
		kindlyText.text = "You are now free of your burden and also free to pick up a new Special Equipment item should you chance upon one in your travels.  Do not forget to pick up your payment on your way out.";

		//spawn the proper number of potion chests as per the kindly man's offer
		for (int i = 0; i < offer; i++) {

			Instantiate (tempPlayer.itemDrops[4], new Vector3 (3.0f + i, 2.0f), Quaternion.identity);
		}

		//reset the ok button text
		transform.FindChild ("okButton").FindChild ("Text").GetComponent<Text> ().text = "Ok...";
		
		//activate the ok button
		transform.FindChild ("okButton").gameObject.SetActive (true);
		
		//return the screen from black
		GameManager.instance.GetComponent<ScreenFade> ().BeginFade (-1);
	}
}
