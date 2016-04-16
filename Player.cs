/*
 *  Title:       Player.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/30/2015
 *  Modified:    04/10/2016
 *  Resources:   Adapted from original player script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: Attached to the player game object, this script handles the player's inventory, stat tracking, stat increases, current and max health, interactions with other items, and UI status updates
 */

using UnityEngine;
using System.Collections;

//allows us to access unity UI features
using UnityEngine.UI;

//the player class inherits from MovingObject instead of from the default monobehaviour
public class Player : MovingObject {

	//track the player's current damage, find, xpMod, and maxHp
	private int damage = 0;
	private int find = 0;
	private float xpMod = 0.0f;
	private int maxHp = 0;

	//track how much hp the player receives for health items
	public int pointsPerFood = 5;
	public int pointsPerDrink = 10;

	//track the delay for room restart
	public float restartRoomDelay = 1f;

	//a reference to the health UI text element
	public Text healthText;

	//a reference to the experience UI text element
	public Text expText;

	//a reference to the UI text element that gives the player status updates (pickups)
	public Text statusText;

	//store the value of the player's strength, dexterity, intelligence, constitution and damage to update the UI
	public Text strText;
	public Text dexText;
	public Text intText;
	public Text conText;

	//store the value of the player's sub-stats, damage, find, xpMod and maxHp for UI text elements
	public Text damText;
	public Text finText;
	public Text xpModText;
	public Text maxHpText;

	//store the count for the player's inventory items to update to the UI
	public Text drinkCount;
	public Text strPotCount;
	public Text expPotCount;
	public Text healthPCount;
	public Text healthPPCount;

	//store references to the buff text, buff marks, and who text UI text elements
	public Text buffText;
	public Text buffMarks;
	public Text whoText;

	//store a reference to the item help text element on the help panel
	public Text itemHelp;

	//these variables hold the audio clips for the game, most of which have two variations
	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip attackSound1;
	public AudioClip attackSound2;
	public AudioClip gameOverSound;
	public AudioClip levelUpSound;
	public AudioClip levelUpChime;
	public AudioClip explosion;

	//used to store a reference to the animator element
	private Animator animator;
	
	//used to store a reference to the weapons' animator elements
	private Animator knifeAnimator;
	private Animator staffAnimator;
	private Animator daggerAnimator;
	private Animator maceAnimator;
	private Animator flailAnimator;
	private Animator broadAnimator;
	private Animator greatAnimator;
	private Animator hammerAnimator;

	//used to store a reference to the armor sets' animator elements
	private Animator paddedAnimator;
	private Animator leatherAnimator;
	private Animator studdedAnimator;
	private Animator hideAnimator;
	private Animator scaleAnimator;
	private Animator chainAnimator;
	private Animator plateAnimator;
	private Animator dragonAnimator;

	//used to store a reference to the special equipment animator elements
	private Animator fireOrbAnimator;
	private Animator iceOrbAnimator;
	private Animator robesAnimator;
	private Animator cloakAnimator;

	//store reference to the beard animator elements
	private Animator beard1Animator;
	private Animator beard2Animator;
	private Animator beard3Animator;
	private Animator beard4Animator;

	//store references to the level up UI buttons so we can enable and disable when the player has enough xp
	public GameObject strUp;
	public GameObject dexUp;
	public GameObject intUp;
	public GameObject conUp;

	//store references to the Armor Equip UI buttons, animation renderers, and selectors so we can enable and disable when needed
	public GameObject paddedUIButton;
	public GameObject paddedSelector;
	public GameObject leatherUIButton;
	public GameObject leatherSelector;
	public GameObject studdedUIButton;
	public GameObject studdedSelector;
	public GameObject hideUIButton;
	public GameObject hideSelector;
	public GameObject scaleUIButton;
	public GameObject scaleSelector;
	public GameObject chainUIButton;
	public GameObject chainSelector;
	public GameObject plateUIButton;
	public GameObject plateSelector;
	public GameObject dragonUIButton;
	public GameObject dragonSelector;

	//and for weapons as well
	public GameObject knifeUIButton;
	public GameObject knifeSelector;
	public GameObject staffUIButton;
	public GameObject staffSelector;
	public GameObject daggerUIButton;
	public GameObject daggerSelector;
	public GameObject maceUIButton;
	public GameObject maceSelector;
	public GameObject flailUIButton;
	public GameObject flailSelector;
	public GameObject broadUIButton;
	public GameObject broadSelector;
	public GameObject greatUIButton;
	public GameObject greatSelector;
	public GameObject hammerUIButton;
	public GameObject hammerSelector;

	//and finally special equipment
	public GameObject fireOrbUIButton;
	public GameObject fireOrbSelector;
	public GameObject FireBall;
	public GameObject iceOrbUIButton;
	public GameObject iceOrbSelector;
	public GameObject IceShards;
	public GameObject robesUIButton;
	public GameObject robesSelector;
	public GameObject Teleport;
	public GameObject cloakUIButton;
	public GameObject cloakSelector;

	//a reference to the conversation with the kindly man
	public GameObject kindlyConversation;

	//store reference to the level up indicator and weapons, armor, special objects, and beard renderers
	private Renderer levelUpIndicator;
	private Renderer padded;
	private Renderer leather;
	private Renderer studded;
	private Renderer hide;
	private Renderer scale;
	private Renderer chain;
	private Renderer plate;
	private Renderer dragon;
	private Renderer knife;
	private Renderer staff;
	private Renderer dagger;
	private Renderer mace;
	private Renderer flail;
	private Renderer broad;
	private Renderer great;
	private Renderer hammer;
	private Renderer fireOrb;
	private Renderer iceOrb;
	private Renderer robes;
	private Renderer cloak;
	private Renderer beard1;
	private Renderer beard2;
	private Renderer beard3;
	private Renderer beard4;

	//stores the player's health during the room (passed to the GameManager in between rooms)
	private int health;

	//stores the player's experience and experience spent during the room (passed to the GameManager in between rooms)
	private int exp;
	private int expSpent;

	//stores the player's currently equipped weapon and armor (passed to the GameManager in between rooms)
	private int currentWeap = 0;
	private int currentArmor = 0;

	//stores the player's currently equipped weapon damage and armor bonus
	private int weapDam = 0;
	private int armorBonus = 0;

	//stores the player's current strength, dexterity, intelligence, and constitution
	private int strength = 1;
	private int dexterity = 1;
	private int intelligence = 1;
	private int constitution = 1;

	//keep track of the player's temporary strength points (from strength potions)
	private int tempStr;

	//keep track of the player's inventory: Drinks, Weapons, Strength Potions, Armor, Experience Potions
	private int[] inventory;

	//array that holds possible gameObjects to drop from enemies
	public GameObject[] itemDrops;

	//a bool that tells us if the current room is over
	private bool roomOver = false;

	//a bool that tells us if the player cast a spell last turn, or has haste
	private bool playerSlow = false;
	private bool playerQuick = false;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

//we only need the touch origin variable if we are including touch controls
#else

	//a variable that indicates the first position touched, initialized to a position off the screen
	private Vector2 touchOrigin = -Vector2.one;

#endif

	//use protected and override because we will have a different implementation in the player class vs the movingObject class
	protected override void Start () {

		//get component reference for the animator
		animator = GetComponent<Animator> ();

		//get component reference for the weapons' animators
		knifeAnimator = GameObject.Find ("Knife").GetComponent<Animator> ();
		staffAnimator = GameObject.Find ("Staff").GetComponent<Animator> ();
		daggerAnimator = GameObject.Find ("Dagger").GetComponent<Animator> ();
		maceAnimator = GameObject.Find ("Mace").GetComponent<Animator> ();
		flailAnimator = GameObject.Find ("Flail").GetComponent<Animator> ();
		broadAnimator = GameObject.Find ("Broad").GetComponent<Animator> ();
		greatAnimator = GameObject.Find ("Great").GetComponent<Animator> ();
		hammerAnimator = GameObject.Find ("Hammer").GetComponent<Animator> ();

		//get component reference for the armors' animator
		paddedAnimator = GameObject.Find ("Padded").GetComponent<Animator> ();
		leatherAnimator = GameObject.Find ("Leather").GetComponent<Animator> ();
		studdedAnimator = GameObject.Find ("Studded").GetComponent<Animator> ();
		hideAnimator = GameObject.Find ("Hide").GetComponent<Animator> ();
		scaleAnimator = GameObject.Find ("Scale").GetComponent<Animator> ();
		chainAnimator = GameObject.Find ("Chain").GetComponent<Animator> ();
		plateAnimator = GameObject.Find ("Plate").GetComponent<Animator> ();
		dragonAnimator = GameObject.Find ("Dragon").GetComponent<Animator> ();

		//get component reference for the special equipment's animator
		fireOrbAnimator = GameObject.Find ("Fire Orb").GetComponent<Animator> ();
		iceOrbAnimator = GameObject.Find ("Ice Orb").GetComponent<Animator> ();
		robesAnimator = GameObject.Find ("Robes").GetComponent<Animator> ();
		cloakAnimator = GameObject.Find ("Cloak").GetComponent<Animator> ();

		//get component reference for the beard animators
		beard1Animator = GameObject.Find ("Beard1").GetComponent<Animator> ();
		beard2Animator = GameObject.Find ("Beard2").GetComponent<Animator> ();
		beard3Animator = GameObject.Find ("Beard3").GetComponent<Animator> ();
		beard4Animator = GameObject.Find ("Beard4").GetComponent<Animator> ();

		//set the player's health from the value saved in the GameManager
		health = GameManager.instance.playerHealthPoints;

		//set the player's exp from the value saved in the GameManager
		exp = GameManager.instance.playerExpPoints;
		expSpent = GameManager.instance.playerExpSpent;
		
		//set the player's str, dex, int, and con from the value saved in the GameManager
		strength = GameManager.instance.playerStr;
		dexterity = GameManager.instance.playerDex;
		intelligence = GameManager.instance.playerInt;
		constitution = GameManager.instance.playerCon;

		//set the player's inventory from the value saved in the GameManager
		inventory = GameManager.instance.getPlayerInv ();

		//update the player's health points in the UI
		healthText.text = "HP: " + health;

		//update the player's exp points in the UI
		expText.text = "XP: " + exp;
 
		//update the player's inventory items in the UI
		drinkCount.text = "x" + inventory [0];
		strPotCount.text = "x" + inventory [2];
		expPotCount.text = "x" + inventory [4];
		healthPCount.text = "x" + inventory [5];
		healthPPCount.text = "x" + inventory [6];

		//save a reference to all of the required renderer components (weapons, armor, special, beards, and level up indicator)
		levelUpIndicator = GameObject.Find ("LevelUpIndicator").GetComponent<Renderer> ();
		knife = GameObject.Find ("Knife").GetComponent<Renderer> ();
		staff = GameObject.Find ("Staff").GetComponent<Renderer> ();
		dagger = GameObject.Find ("Dagger").GetComponent<Renderer> ();
		mace = GameObject.Find ("Mace").GetComponent<Renderer> ();
		flail = GameObject.Find ("Flail").GetComponent<Renderer> ();
		broad = GameObject.Find ("Broad").GetComponent<Renderer> ();
		great = GameObject.Find ("Great").GetComponent<Renderer> ();
		hammer = GameObject.Find ("Hammer").GetComponent<Renderer> ();
		padded = GameObject.Find ("Padded").GetComponent<Renderer> ();
		leather = GameObject.Find ("Leather").GetComponent<Renderer> ();
		studded = GameObject.Find ("Studded").GetComponent<Renderer> ();
		hide = GameObject.Find ("Hide").GetComponent<Renderer> ();
		scale = GameObject.Find ("Scale").GetComponent<Renderer> ();
		chain = GameObject.Find ("Chain").GetComponent<Renderer> ();
		plate = GameObject.Find ("Plate").GetComponent<Renderer> ();
		dragon = GameObject.Find ("Dragon").GetComponent<Renderer> ();
		fireOrb = GameObject.Find ("Fire Orb").GetComponent<Renderer> ();
		iceOrb = GameObject.Find ("Ice Orb").GetComponent<Renderer> ();
		robes = GameObject.Find ("Robes").GetComponent<Renderer> ();
		cloak = GameObject.Find ("Cloak").GetComponent<Renderer> ();
		beard1 = GameObject.Find ("Beard1").GetComponent<Renderer> ();
		beard2 = GameObject.Find ("Beard2").GetComponent<Renderer> ();
		beard3 = GameObject.Find ("Beard3").GetComponent<Renderer> ();
		beard4 = GameObject.Find ("Beard4").GetComponent<Renderer> ();

		//set up the player's equipped armor and weapons in the UI
		equipWeap (GameManager.instance.playerWeap);
		equipArmor (GameManager.instance.playerArmor);

		//call the start function of our base class (MovingObject)
		base.Start();
	}

	//function that runs when the player object is disabled
	private void OnDisable() {

		//store the player's current health in the GameManager so it can be used in the next room
		GameManager.instance.playerHealthPoints = health;

		//store the player's current exp in the GameManager so it can be used in the next room
		GameManager.instance.playerExpPoints = exp;
		GameManager.instance.playerExpSpent = expSpent;

		//store the player's currently equipped weapon and armor in the GameManager so it can be used in the next room
		GameManager.instance.playerWeap = currentWeap;
		GameManager.instance.playerArmor = currentArmor;

		//if the player object is still active
		if (gameObject.activeInHierarchy) {

			//unequip the player's weapon and armor, so they do not gain cumulative armor bonuses for advancing rooms
			Invoke ("unequip", 1.0f);
		}

		//store the player's current str, dex, int, and con in the GameManager so it can be used in the next room
		GameManager.instance.playerStr = strength;
		GameManager.instance.playerDex = dexterity;
		GameManager.instance.playerInt = intelligence;
		GameManager.instance.playerCon = constitution;

		//store the player's current inventory in the GameManager so it can be used in the next room
		GameManager.instance.setPlayerInv (inventory);
	}

	// Update is called once per frame
	void Update () {

		//enable or disable the level up UI buttons depending on the player's current xp
		showStatInvButtons ();

		//if it is not the player's turn, or the player cast a spell last turn, simply run no other code
		if (!GameManager.instance.playersTurn || playerSlow) {

			return;
		}

		//set the who text value and color to indicate that it is the player's turn
		whoText.color = new Color (0.0f, 1.0f, 0.0f);
		whoText.text = "Go!";

		//check for the user pressing key macros
		//if the user presses the alpha 1 key
		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {

			//consume a health potion
			usePlayerItem (0);
		}

		//if the user presses the alpha 2 key
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {

			//consume a strength potion
			usePlayerItem (2);
		}

		//if the user presses the alpha 3 key
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {

			//consume an experience potion
			usePlayerItem (4);
		}

		//variables to store the direction that the player is moving (either as 1 or -1) on each axis
		int horizontal = 0;
		int vertical = 0;

//check to see what platform the game is running on in order to use the proper input method
//unity standalone and webplayer platforms get keyboard controls
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

		//get the direction to move the player object from the user
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		//prevent the player from moving diagonally
		if (horizontal != 0) {
			vertical = 0;
		}

//every other platform (Android, IOS, Windows Phone) get touch screen controls
#else

		//if there are more than zero touches detected
		if (Input.touchCount > 0) {

			//store the first touch (ignoring all other touches)
			Touch myTouch = Input.touches[0];

			//make sure that this is the beginning of a touch phase, and not the middle or end
			if (myTouch.phase == TouchPhase.Began) {

				//if so set the variable touch origin to the position of the current touch
				touchOrigin = myTouch.position;

			//otherwise, if the touch is ending and the origin of the touch was different than our pre-set value (-1)
			} else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {

				//declare a variable and set this position as the end of the touch
				Vector2 touchEnd = myTouch.position;

				//get the difference between the origin and end touch phases on the x axis
				float x = touchEnd.x - touchOrigin.x;

				//and do the same for the y axis
				float y = touchEnd.y - touchOrigin.y;

				//reset touch origin x to -1 so our conditional doesn't repeatedly evaluate to true
				touchOrigin.x = -1;

				//determine which axis had the greatest absolute change over the swipe
				if (Mathf.Abs (x) > Mathf.Abs (y)) {

					//if x had the greatest absolute change, determine if x was greater than (set to 1) or less than zero (set to -1)
					horizontal = x > 0 ? 1 : -1;

				//if the y axis had the greatest absolute change
				} else {

					//determine if y is greater or less than zero and set y to -1 or 1
					vertical = y > 0 ? 1 : -1;
				}
			}
		}

#endif

		//if the user is has attempted to move the player
		if (horizontal != 0 || vertical != 0) {

			//reset the text on the status text UI text element
			statusText.text = "";

			//if the player attempts to move to the left
			if (horizontal == -1) {

				//rotate the player sprite 90 on the X axis
				gameObject.transform.rotation = new Quaternion (0, 90, 0, 0);
			}

			//if the player attempts to move to the right
			if (horizontal == 1) {

				//set the player sprite to the default rotation
				gameObject.transform.rotation = new Quaternion (0, 0, 0, 0);
			}
			
			//call the AttemptMove function and pass in the desired movement, assuming the player may interact with a damagable object
			AttemptMove<Damagable> (horizontal, vertical);
		}
	}

	protected override void AttemptMove <T> (int xDir, int yDir){

		//update the player's health points in the UI
		healthText.text = "HP: " + health;

		//update the player's exp points in the UI
		expText.text = "XP: " + exp;
		
		//call the AttemptMove function from our base class (MovingObJect) and pass it our generic parameter T and our direction integers
		base.AttemptMove <T> (xDir, yDir);

		//allows us to reference the result of the line cast done in Move
		RaycastHit2D hit;

		//check to see if the move was successful
		if (Move (xDir, yDir, out hit)) {

			//if the Move function returns true, play one of the two move sounds, randomized by our randomize sfx function
			SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
		}

		//logic for granting an extra move with the cloak equipped
		//if the player has the cloak equipped and the playerQuick bool is false
		if (currentArmor == 10) {

			//set the playerQuick bool to true
			playerQuick = true;

		} else {

			//reset the playerQuick bool to false
			playerQuick = false;
		}

		//tell the GameManager that the player's turn is over
		GameManager.instance.playersTurn = false;

		//New code for leaking one temporary strength point per turn, instead of all temp strength points being lost after one turn
		//if the player has any temporary strength points
		if (tempStr > 0) {

			//decrement one point of strength
			strength--;

			//decrement one temporary strength counter
			tempStr--;

			//recalculate damage sub-skill
			damage = strength + weapDam;
			
			//update UI values for strength and damage
			strText.text = "STR: " + strength;
			damText.text = "DAM: " + damage;

			//update the UI value and color for buff text and buff marks
			buffText.color = new Color (0.0f, 1.0f, 0.0f);
			buffText.text = "STR";
			buffMarks.color = new Color (0.0f, 1.0f, 0.0f);
			buffMarks.text = "";
			for (int i = 0; i < tempStr; i++) {

				buffMarks.text += "+";
			}

			//reset the buff text UI element if the player has run out of temporary strength
			if (tempStr < 1) {
				buffText.text = "";
			}
		}
	}

	//check to see what (if anything) the player is currently interacting with, and take action
	private void OnTriggerEnter2D (Collider2D other){

		//if the player is interacting with the kindly man
		if (other.tag == "Conversation") {

			//activate the kindly converstaion
			kindlyConversation.SetActive (true);

			//if the object the player interacts with is the secret exit
		} else if (other.tag == "Secret") {

			//secret exit is exactly like normal exit except that we set the secret room boolean (in the game manager script) to true
			GameManager.instance.secretRoomBool(true);

			//so leak ALL temporary strength points by looping through the tempStr variable
			for (int i = 0; i < tempStr;) {
				
				//decrement strength by one
				strength--;
				
				//decrement temporary strength by one
				tempStr--;
			}
			
			//recalculate damage sub-skill
			damage = strength + weapDam;
			
			//update UI values for strength and damage
			strText.text = "STR: " + strength;
			damText.text = "DAM: " + damage;
			
			//set the roomOver bool to true
			roomOver = true;
			
			//invoke the restart function, after a delay equal to the returned value of the beginFade function
			Invoke ("Restart", GameManager.instance.GetComponent<ScreenFade> ().BeginFade (1));
			
			//disable the player object since the room is over
			enabled = false;

			//if the object the player interacts with is the exit
		} else if (other.tag == "Exit") {

			//so leak ALL temporary strength points by looping through the tempStr variable
			for (int i = 0; i < tempStr;) {
				
				//decrement strength by one
				strength--;
				
				//decrement temporary strength by one
				tempStr--;
			}
			
			//recalculate damage sub-skill
			damage = strength + weapDam;
			
			//update UI values for strength and damage
			strText.text = "STR: " + strength;
			damText.text = "DAM: " + damage;

			//set the roomOver bool to true
			roomOver = true;

			//if the secret room bool is true
			if (GameManager.instance.secretRoomBool ()) {

				//reset the secret room bool
				GameManager.instance.secretRoomBool (false);
			}

			//invoke the restart function, after a delay equal to the value of restartRoomDelay
			Invoke ("Restart", GameManager.instance.GetComponent<ScreenFade> ().BeginFade (1));

			//disable the player object since the room is over
			enabled = false;

			//if the object the player interacts with is food
		} else if (other.tag == "Food") {

			//if the player has less than full health
			if (health < maxHp) {

				//have the player gain health points euqal to the value of pointsPerFood
				GainHealth (pointsPerFood);

				//update the player's health points in the UI
				healthText.text = "HP: " + health;

				//play one of the two eat sounds randomized by our randomize sfx function
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);

				//disable the food object that the player touched
				other.gameObject.SetActive (false);
			}
		
			//if the object is a mushroom
		} else if (other.tag == "Mushroom") {

			//have the player lose health points euqal to the value of pointsPerFood
			LoseHealth (pointsPerFood, "Mushroom");
				
			//update the player's health points in the UI
			healthText.text = "HP: " + health;
				
			//play one of the two eat sounds randomized by our randomize sfx function
			SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
				
			//disable the food object that the player touched
			other.gameObject.SetActive (false);
			
			//if the object is a health drink
		} else if (other.tag == "Soda") {

			//increment the number of usable drink items in the player's inventory
			inventory [0]++;

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);

			//disable the soda object
			other.gameObject.SetActive (false);

			//update the Drink count in the UI
			drinkCount.text = "x" + inventory [0];

			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);

			//change the text on the status text UI text element
			statusText.text = "Health Potion Found!";

			//if the object is a weapon chest
		} else if (other.tag == "Strength Potion") {

			//increment the number of strength potions in the player's inventory
			inventory [2]++;

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			
			//disable the strength potion object
			other.gameObject.SetActive (false);

			//update the Strength Potion count in the UI
			strPotCount.text = "x" + inventory [2];
		
			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);

			//change the text on the status text UI text element
			statusText.text = "Strength Potion Found!";

			//if the object is an armor chest
		} else if (other.tag == "Equipment") {

			//get and save a random number, either 0 or 1
			int rand = Random.Range (0, 2);

			//and the player has not found a weapon on this floor
			//and does not have all the weapons
			if ((rand == 0 || GameManager.instance.armorDrop) && !(GameManager.instance.weaponDrop) && inventory [1] < 8) {

				//increment the number of weapons in the player's inventory
				inventory [1]++;

				//set the color of the status text UI element
				statusText.color = new Color (0.75f, 0.75f, 0.75f);
				
				//change the text on the status text UI text element
				statusText.text = "Weapon Found!";

				//set the weapon drop bool to true so that no more weapons will drop this floor
				GameManager.instance.weaponDrop = true;

				//otherwise if the player has not found an armor set on this floor
				//and does not have all the armor sets
			} else if ((rand == 1 || GameManager.instance.weaponDrop) && !(GameManager.instance.armorDrop) && inventory [3] < 8) {

				//increment the number of armor sets in the player's inventory
				inventory [3]++;

				//set the color of the status text UI element
				statusText.color = new Color (0.75f, 0.75f, 0.75f);

				//change the text on the status text UI text element
				statusText.text = "Armor Found!";

				//set the armor drop bool to true so that no more armor sets will drop this floor
				GameManager.instance.armorDrop = true;

				//otherwise
			} else {

				//if the player has high enough max hp
				if (maxHp > 149) {

					//grant the player a health++ potion
					inventory [6]++;

					//update the Drink count in the UI
					healthPPCount.text = "x" + inventory [6];
					
					//set the color of the status text UI element
					statusText.color = new Color (0.75f, 0.75f, 0.75f);
					
					//change the text on the status text UI text element
					statusText.text = "Health++ Potion Found!";

				} else if (maxHp > 99) {

					//grant the player a health+ potion
					inventory [5]++;

					//update the Drink count in the UI
					healthPCount.text = "x" + inventory [5];
					
					//set the color of the status text UI element
					statusText.color = new Color (0.75f, 0.75f, 0.75f);
					
					//change the text on the status text UI text element
					statusText.text = "Health+ Potion Found!";

				} else {

					//grant the player two health potions
					inventory [0] += 2;

					//update the Drink count in the UI
					drinkCount.text = "x" + inventory [0];

					//set the color of the status text UI element
					statusText.color = new Color (0.75f, 0.75f, 0.75f);
					
					//change the text on the status text UI text element
					statusText.text = "Health Potions Found!";
				}
			}

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			
			//disable the armor chest object
			other.gameObject.SetActive (false);

			//if the object is an experience potion
		} else if (other.tag == "Experience Potion") {

			//increment the number of experience potions in the player's inventory
			inventory [4]++;

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			
			//disable the experience potion object
			other.gameObject.SetActive (false);

			//update the Experience Potion count in the UI
			expPotCount.text = "x" + inventory [4];

			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);

			//change the text on the status text UI text element
			statusText.text = "XP Potion Found!";

			//if the object is a potion chest
		} else if (other.tag == "Potion Chest") {

			//if the player has high enough max hp
			if (maxHp > 149) {
				
				//grant the player a health++ potion
				inventory [6]++;
				
				//update the Drink count in the UI
				healthPPCount.text = "x" + inventory [6];
				
				//set the color of the status text UI element
				statusText.color = new Color (0.75f, 0.75f, 0.75f);
				
				//change the text on the status text UI text element
				statusText.text = "HP++ ";
				
			} else if (maxHp > 99) {
				
				//grant the player a health+ potion
				inventory [5]++;
				
				//update the Drink count in the UI
				healthPCount.text = "x" + inventory [5];
				
				//set the color of the status text UI element
				statusText.color = new Color (0.75f, 0.75f, 0.75f);
				
				//change the text on the status text UI text element
				statusText.text = "HP+ ";
				
			} else {
				
				//grant the player two health potions
				inventory [0] += 2;
				
				//update the Drink count in the UI
				drinkCount.text = "x" + inventory [0];
				
				//set the color of the status text UI element
				statusText.color = new Color (0.75f, 0.75f, 0.75f);
				
				//change the text on the status text UI text element
				statusText.text = "HP ";
			}

			//switch on a random number (0 or 1)
			switch (Random.Range (0, 2)) {

			//case 0 gives the player strength potion
			case 0:

				//increment the number of strength potions in the player's inventory
				inventory [2]++;

				//update the Strength Potion count in the UI
				strPotCount.text = "x" + inventory [2];

				//change the text on the status text UI text element
				statusText.text += "and STR Potions Found!";
				break;
			//case 1 gives the player experience potion
			case 1:

				//increment the number of experience potions in the player's inventory
				inventory [4]++;

				//update the Experience Potion count in the UI
				expPotCount.text = "x" + inventory [4];

				//change the text on the status text UI text element
				statusText.text += "and XP Potions Found!";
				break;
			}

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);

			//disable the potion chest object
			other.gameObject.SetActive (false);

			//if the object is a health+ potion
		} else if (other.tag == "Health+") {

			//increment the number of health+ potions in the player's inventory
			inventory [5]++;

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			
			//disable the experience potion object
			other.gameObject.SetActive (false);
			
			//update the Health+ Potion count in the UI
			healthPCount.text = "x" + inventory [5];
			
			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);
			
			//change the text on the status text UI text element
			statusText.text = "Health+ Potion Found!";

			//if the object is a health ++ potion
		} else if (other.tag == "Health++") {
			
			//increment the number of health++ potions in the player's inventory
			inventory [6]++;
			
			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			
			//disable the experience potion object
			other.gameObject.SetActive (false);
			
			//update the Health++ Potion count in the UI
			healthPPCount.text = "x" + inventory [6];
			
			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);
			
			//change the text on the status text UI text element
			statusText.text = "Health++ Potion Found!";

			//if the object is a key
		} else if (other.tag == "Key") {

			//find the locked exit floor tile and save a Vector3 of its transform position
			Vector3 exitPos = GameObject.FindGameObjectWithTag ("Lock").transform.position;

			//call the open floor exit function (on the BoardManager script) and pass in the locked floor exit's transform position
			GameManager.instance.boardScript.openFloorExit (exitPos);

			//play othe level up sound randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (levelUpSound);

			//disable the key object
			other.gameObject.SetActive (false);

			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);
			
			//change the text on the status text UI text element
			statusText.text = "The Door has Opened!";

			//if the object is a special chest
		} else if (other.tag == "Special Chest") {

			//play one of the pickup sounds (that is currently a drink sound) randomized by our sfx funciton
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			
			//disable the fireOrb object
			other.gameObject.SetActive (false);

			//decide which special item the player will get
			int special = Random.Range (1, 5);
			inventory [7] = special;

			//set the color of the status text UI element
			statusText.color = new Color (0.75f, 0.75f, 0.75f);

			//display the proper message for each item
			if (special == 1) {

				statusText.text = "Fire Orb Found!";
			}
			if (special == 2) {

				statusText.text = "Ice Orb Found!";
			}
			if (special == 3) {

				statusText.text = "Robes Found!";
			}
			if (special == 4) {

				statusText.text = "Cloak Found!";
			}

			//if the object is a Fireball
		} else if (other.tag == "Fireball") {

			//have the player take damage
			LoseHealth ((150 - (intelligence * 10)), "Fireball");

			//if the object is Ice Shards
		} else if (other.tag == "IceShards") {

			//have the player take damage
			LoseHealth ((100 - (intelligence * 10)), "Ice Shards");

			//if the object is floor spikes
		} else if (other.tag == "Spikes") {

			//have the player take damage
			LoseHealth (25, "Spike Pit");
		
			//if the object is floor ice
		} else if (other.tag == "Ice") {

			//update the status display
			statusUpdate ("You Have Been Slowed by Ice!", new Color (0.0f, 255.0f, 244.0f));

			//set the player to slow
			playerSlow = true;
		}
	}

	//specific things happen when you leave certain trigger colliders
	private void OnTriggerExit2D(Collider2D other) {

		//if the player is moving away from the kindly man
		if (other.tag == "Conversation") {

			//disable the kindly converstaion
			kindlyConversation.SetActive (false);

			//call the reset function (belongs to the ok button) on the kindly conversation script
			kindlyConversation.transform.FindChild ("kindlyPanel").GetComponent<Conversation>().Converse (1);
		}

	}

	//certain things happen when a player cant move, depending on what the player is interacting with
	protected override IEnumerator OnCantMove <T> (T component, Vector3 position) {

		//create an object based on the component that was passed into the function
		Damagable hitObject = component as Damagable;

		//call the animation trigger function and pass in playerAttack
		triggerAnimation ("playerAttack");
		
		//play one of two attack sounds after randomizing it with the randomize sound clip function
		SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);

		//only run the following code if the object being attacked currently has hp to lose
		if (hitObject.GetComponent<Damagable> ().hp > 0) {

			//call the function to cause the object to take damage
			hitObject.takeDamage(damage);

			//if the Object was disabled as a result of the interaction
			if (hitObject.gameObject.GetComponent<Damagable> ().hp < 1) {

				//randomly drop a health potion if the object is a container
				if (hitObject.gameObject.tag == "Container") {

					//potions from containers have a 50% drop rate (no find skill check)
					if (Random.Range (0, 2) == 1) {

						//variable that holds the item to be dropped, initially set to lowest tier health potion
						GameObject toDrop = itemDrops[0];
						
						//if the player has gained a high enough max hp
						if (maxHp > 149) {
							
							//upgrade the health potion
							toDrop = itemDrops[6];
							
						} else if (maxHp > 99) {
							
							//upgrade the health potion
							toDrop = itemDrops[5];
						}

						//instantiate the chosen item at the pre-defined destination
						Instantiate (toDrop, position, Quaternion.identity);
					}

				//make sure we are only dropping items, playing death sound, and gaining xp when enemies die (as opposed to walls)
				} else if (hitObject.gameObject.tag == "Enemy" || hitObject.gameObject.tag == "Boss") {

					//gain experience equal to the object's exp value
					GainExp (hitObject.expValue);

					//create a variable to hold the result of the find check
					int findCheck = 0;

					//set the find check to a random number between zero and 50 + the player's find skill
					findCheck = Random.Range (0, (51 + find));

					//if the find check is sufficiently high, proceed to decide what item to drop, otherwise drop no item
					if (findCheck > 30) {

						//old code for keeping item drops inside of the play area (not necessary now that items drop where the enemy was standing)
						//Vector3 dropPos = position;

						//check to make sure the item will not be dropped on the right most outer wall tile column
						//if ((dropPos.x + 1) == GameManager.instance.GetComponent<BoardManager>().columns) {

							//decrement the x value of dropPos
							//dropPos.x--;

						//otherwise, the item will either drop inside of the play area, or on the left most outer wall tile column
						//} else {

							//increment the x value of dropPos
							//dropPos.x++;
						//}

						//check to make sure the item will not be dropped on the top most outer wall tile row
						//if ((dropPos.y + 1) == GameManager.instance.GetComponent<BoardManager>().rows) {

							//decrement the y value of dropPos
							//dropPos.y--;

						//otherwise, the item will either drop inside the play area, or on the bottom most outer wall tile row
						//} else {

							//increment the y value of dropPos
							//dropPos.y++;
						//}

						//variable that holds the item to be dropped
						GameObject toDrop = itemDrops[0];

						//if the player has gained a high enough max hp
						if (maxHp > 149) {

							//upgrade the health potion
							toDrop = itemDrops[6];
									
						} else if (maxHp > 99) {

							//upgrade the health potion
							toDrop = itemDrops[5];
						}

						//cascade through the possible item drops checking the find result at each step
						//only upgrade the drop to a special equipment piece if the player does not currently have one
						if (findCheck > 124 && inventory[7] == 0) {

							//upgrade the item drop
							toDrop = itemDrops[7];

						} else if (findCheck > 100) {

							//upgrade the item drop
							toDrop = itemDrops[4];

						} else if(findCheck > 90) {
								
							//upgrade the item drop
							toDrop = itemDrops[3];

						} else if (findCheck > 75) {

							//upgrade the item drop
							toDrop = itemDrops[2];

						} else if (findCheck > 60) {

							//upgrade the item drop
							toDrop = itemDrops[1];
						}

						//instantiate the chosen item at the pre-defined destination
						Instantiate (toDrop, position, Quaternion.identity);
					}
				}
			}
		}

		return null;
	}

	//this function is run when the player reaches the exit of the current room (thus generating a new room)
	private void Restart() {

		//save the player's final position in  the gameManager for spawning the player in the next room
		GameManager.instance.PlayerPos (this.transform.position);

		//call the load room function and pass in the currently loaded room as the parameter
		Application.LoadLevel (Application.loadedLevel);
	}

	//funciton that causes the player to lose health
	public void LoseHealth (int loss, string cause) {

		//call the trigger animation function and pass in playerHit
		triggerAnimation ("playerHit");

		//set the special animation trigger to playerHit
		fireOrbAnimator.SetTrigger ("playerHit");
		iceOrbAnimator.SetTrigger ("playerHit");

		//if the player is about to take negative damage
		if (loss < 0) {

			//set loss variable to zero
			loss = 0;
		}

		//decrement the player's health by the value passed into the function
		health -= loss;

		//update the player's health points in the UI
		healthText.text = "HP: " + health;

		//set the color of the status text UI element
		statusText.color = new Color (1.0f, 0.0f, 0.0f);
		
		//change the text on the status text UI text element
		statusText.text = cause + " dealt " + loss + " damage!";

		//since the player has lost health, check to see if the game is over
		CheckIfGameOver ();
	}

	//function that causes the player to gain health
	public void GainHealth (int gain) {

		//if the health gain would go over the player's max
		if (health + gain > maxHp) {

			//simply heal the player to max
			health = maxHp;

		} else {

			//increment the player's health by the value passed into the function
			health += gain;
		}

		//update the player's health points in the UI
		healthText.text = "HP: " + health;

		//set the color of the status text UI element
		statusText.color = new Color (0.0f, 1.0f, 0.0f);
		
		//change the text on the status text UI text element
		statusText.text = "Gained " + gain + " health points!";
	}

	//function that causes the player to gain exp
	public void GainExp (int gain) {

		//increment the player's exp by the amount passed
		//Note: must round and cast the exp gain to an int due to xpMod's type of float; Also, added 0.01 before rounding to get 0.5 to round up
		exp += (int)(Mathf.Round (gain * xpMod + 0.01f));

		//update the player's exp in the UI
		expText.text = "XP: " + exp;

		//set the color of the status text UI element
		statusText.color = new Color (0.9f, 1.0f, 0.0f);
		
		//change the text on the status text UI text element
		statusText.text = "Gained " + (Mathf.Round (gain * xpMod + 0.01f)) + " experience!";
	}

	//function that uses an item from the player's inventory
	public void usePlayerItem(int index) {

		//check to make sure there are any items at the requested index
		if (inventory [index] < 1) {

			//if not, do nothing
			return;

		//otherwise
		} else {

			//switch on the requested index
			switch (index) {

			//if the index calls for a Drink
			case 0:

				//if the player's health is less than max
				if (health < maxHp) {

					//call the gain health function and pass in the points per drink variable
					GainHealth (pointsPerDrink);

					//play one of the two drink sounds randomized by our randomize sfx function
					SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);

					//decrement the Drink count
					inventory [0]--;

					//update the Drink count in the UI
					drinkCount.text = "x" + inventory[0];

				}
				break;

			//if the index calls for a strength potion
			case 2:

				//add one point to the player's strength
				strength++;

				//and increment the temporary strength points variable
				tempStr++;

				//play one of the two drink sounds randomized by our randomize sfx function
				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				//decrement the strength potion count
				inventory [2]--;

				//update player's current damage
				damage = strength + weapDam;

				//update the strength potion count, strength, and damage values in the UI
				strPotCount.text = "x" + inventory[2];
				strText.text = "STR: " + strength;
				damText.text = "DAM: " + damage;

				//set the color of the status text UI element
				statusText.color = new Color (0.2f, 0.2f, 1.0f);
				
				//change the text on the status text UI text element
				statusText.text = "Gained " + tempStr + " temporary Strength!";

				//update the UI value and color for buff text and buff marks
				buffText.color = new Color (0.0f, 1.0f, 0.0f);
				buffText.text = "STR";
				buffMarks.color = new Color (0.0f, 1.0f, 0.0f);
				buffMarks.text = "";
				for (int i = 0; i < tempStr; i++) {
					
					buffMarks.text += "+";
				}

				break;

			//if the index calls for an experience potion
			case 4:
				
				//call the function to have the player gain 100 experience
				GainExp ((int)(100/xpMod));
				
				//play one of the two drink sounds randomized by our randomize sfx function
				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				//decrement the strength potion count
				inventory [4]--;
				
				//update the strength potion count in the UI
				expPotCount.text = "x" + inventory[4];

				break;

			//if the index calls for a health+ potion
			case 5:

				//if the player has less than full health
				if (health < maxHp) {

					//call the gain health function and pass in 25
					GainHealth (25);

					//play one of the two drink sounds randomized by our randomize sfx function
					SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
					
					//decrement the Drink count
					inventory [5]--;
					
					//update the Drink count in the UI
					healthPCount.text = "x" + inventory[5];

				}
				break;

			//if the index calls for a health++ potion
			case 6:
				
				//if the player has less than full health
				if (health < maxHp) {
					
					//call the gain health function and pass in 50
					GainHealth (50);
					
					//play one of the two drink sounds randomized by our randomize sfx function
					SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
					
					//decrement the Drink count
					inventory [6]--;
					
					//update the Drink count in the UI
					healthPCount.text = "x" + inventory[6];
					
				}
				break;
			}
		}
	}

	//function that equips and unequips weapons
	public void equipWeap(int weapon) {

		//displace all weapons and disable weapon selector GameObjects
		knife.sortingLayerName = "Default";
		knifeSelector.SetActive (false);
		staff.sortingLayerName = "Default";
		staffSelector.SetActive (false);
		dagger.sortingLayerName = "Default";
		daggerSelector.SetActive (false);
		mace.sortingLayerName = "Default";
		maceSelector.SetActive (false);
		flail.sortingLayerName = "Default";
		flailSelector.SetActive (false);
		broad.sortingLayerName = "Default";
		broadSelector.SetActive (false);
		great.sortingLayerName = "Default";
		greatSelector.SetActive (false);
		hammer.sortingLayerName = "Default";
		hammerSelector.SetActive (false);

		//displace all special equipment and disable special selector GameObjects
		fireOrb.sortingLayerName = "Default";
		fireOrbSelector.SetActive (false);
		iceOrb.sortingLayerName = "Default";
		iceOrbSelector.SetActive (false);

		//set weapon damage to zero
		weapDam = 0;

		//if the weapon number passed into the function is the same as the weapon currently equipped
		if (currentWeap == weapon) {

			//unequip all weapons
			currentWeap = 0;

		//otherwise
		} else {

			//switch on the weapon being clicked on
			switch(weapon) {

				//case 1 is for the knife
				case 1:
					
					//equip the weapon
					currentWeap = 1;

					//set the weapon damage
					weapDam = 1;

					//replace the knife and enable the knife selector
					knife.sortingLayerName = "Units";
					knifeSelector.SetActive (true);
					break;

				//case 2 is for the staff
				case 2:
				
					//equip the weapon
					currentWeap = 2;
				
					//set the weapon damage
					weapDam = 3;
				
					//replace the staff and enable staff selector
					staff.sortingLayerName = "Units";
					staffSelector.SetActive (true);
					break;


				//case 3 is for the dagger
				case 3:

					//equip the weapon
					currentWeap = 3;

					//set the weapon damage
					weapDam = 6;

					//replace the dagger and enable dagger selector
					dagger.sortingLayerName = "Units";
					daggerSelector.SetActive (true);
					break;

				//case 4 is for the mace
				case 4:
				
					//equip the weapon
					currentWeap = 4;
				
					//set the weapon damage
					weapDam = 10;
				
					//replace the mace and enable the maceselector
					mace.sortingLayerName = "Units";
					maceSelector.SetActive (true);
					break;

				//case 5 is for the flail
				case 5:
				
					//equip the weapon
					currentWeap = 5;
				
					//set the weapon damage
					weapDam = 15;
				
					//replace the flail and enable the flail selector
					flail.sortingLayerName = "Units";
					flailSelector.SetActive (true);
					break;

				//case 6 is for the broad sword
				case 6:

					//equip the weapon
					currentWeap = 6;

					//set the weapon damage
					weapDam = 21;

					//replace the broad sword and enable the broad sword selector
					broad.sortingLayerName = "Units";
					broadSelector.SetActive (true);
					break;

				//case 7 is for the great sword
				case 7:

					//equip the weapon
					currentWeap = 7;

					//set the weapon damage
					weapDam = 28;

					//replace the great sword and enalbe the great sword selector
					great.sortingLayerName = "Units";
					greatSelector.SetActive (true);
					break;

				//case 8 is for the war hammer
				case 8:
				
					//equip the weapon
					currentWeap = 8;
				
					//set the weapon damage
					weapDam = 36;
				
					//replace the war hammer and enable the war hammer selector
					hammer.sortingLayerName = "Units";
					hammerSelector.SetActive (true);
					break;

				//case 9 is for the fireOrb
				case 9:
				
					//equip the special equipment
					currentWeap = 9;
				
					//set the weapon damage
					weapDam = 0;
				
					//replace the fireOrb and enable the fireOrb selector
					fireOrb.sortingLayerName = "Units";
					fireOrbSelector.SetActive (true);
					break;

				//case 10 is for the iceOrb
				case 10:
				
					//equip the special equipment
					currentWeap = 10;
				
					//set the weapon damage
					weapDam = 0;
				
					//replace the iceOrb and enable the iceOrb selector
					iceOrb.sortingLayerName = "Units";
					iceOrbSelector.SetActive (true);
					break;

				//default case
				default:

					//does nothing
					break;
			}
		}

		//call the levelUp function (using the default case) to update all sub stats and UI elements
		levelUp (99);

		//play a sound to signal successful equip/unequip (currently using the attack sound)
		SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
	}

	//function that equips and unequips armor
	public void equipArmor(int armor) {

		//displace all armor objects and disable all armor selectors
		padded.sortingLayerName = "Default";
		paddedSelector.SetActive (false);
		leather.sortingLayerName = "Default";
		leatherSelector.SetActive (false);
		studded.sortingLayerName = "Default";
		studdedSelector.SetActive (false);
		hide.sortingLayerName = "Default";
		hideSelector.SetActive (false);
		scale.sortingLayerName = "Default";
		scaleSelector.SetActive (false);
		chain.sortingLayerName = "Default";
		chainSelector.SetActive (false);
		plate.sortingLayerName = "Default";
		plateSelector.SetActive (false);
		dragon.sortingLayerName = "Default";
		dragonSelector.SetActive (false);
		robes.sortingLayerName = "Default";
		robesSelector.SetActive (false);
		cloak.sortingLayerName = "Default";
		cloakSelector.SetActive (false);

		//set the armor bonus to zero
		armorBonus = 0;

		//if the currently equipped armor is the armor being clicked on
		if (currentArmor == armor) {
			
			//unequip the armor
			currentArmor = 0;

		//otherwise
		} else {
			
			//switch on the armor being clicked on
			switch(armor) {

				//case 1 is for the padded armor
				case 1:
				
					//equip the armor
					currentArmor = 1;
				
					//set the armor bonus to 1
					armorBonus = 1;
					
					//replace the padded armor and enable the padded armor selector
					padded.sortingLayerName = "Units";
					paddedSelector.SetActive (true);
				
					break;

				//case 2 is for the leather armor
				case 2:

					//equip the armor
					currentArmor = 2;

					//set the armor bonus to 3
					armorBonus = 3;

					//replace the leather armor and enable the leather armor selector
					leather.sortingLayerName = "Units";
					leatherSelector.SetActive (true);

					break;
				
				//case 3 is for the studded armor
				case 3:

					//equip the armor
					currentArmor = 3;

					//set the armor bonus to 6
					armorBonus = 6;

					//replace the studded armor and enable the studded armor selector
					studded.sortingLayerName = "Units";
					studdedSelector.SetActive (true);
				
					break;

				//case 4 is for the hide armor
				case 4:
				
					//equip the armor
					currentArmor = 4;
				
					//set the armor bonus to 10
					armorBonus = 10;
				
					//replace the hide armor and enable the hide armor selector
					hide.sortingLayerName = "Units";
					hideSelector.SetActive (true);
				
					break;

				//case 5 is for the scale armor
				case 5:
				
					//equip the armor
					currentArmor = 5;
				
					//set the armor bonus to 15
					armorBonus = 15;
				
					//replace the scale armor and enable the scale armor selector
					scale.sortingLayerName = "Units";
					scaleSelector.SetActive (true);
				
					break;

				//case 6 is for the chain armor
				case 6:
				
					//equip the armor
					currentArmor = 6;
				
					//set the armor bonus to 21
					armorBonus = 21;
					
					//replace the chain armor and enable the chain armor selector
					chain.sortingLayerName = "Units";
					chainSelector.SetActive (true);
				
					break;

				//case 7 is for the plate armor
				case 7:

					//equip the armor
					currentArmor = 7;

					//set the armor bonus to 28
					armorBonus = 28;

					//replace the plate armor and enable the plate armor selector
					plate.sortingLayerName = "Units";
					plateSelector.SetActive (true);
				
					break;
				
				//case 8 is for the dragon armor
				case 8:

					//equip the armor
					currentArmor = 8;

					//set the armor bonus to 36
					armorBonus = 36;

					//replace the dragon armor and enable the dragon armor selector
					dragon.sortingLayerName = "Units";
					dragonSelector.SetActive (true);
				
					break;

				//case 9 is for the robes
				case 9:
				
					//equip the robes
					currentArmor = 9;
				
					//set the armor bonus to 0
					armorBonus = 0;
				
					//replace the robes and enable the robes selector
					robes.sortingLayerName = "Units";
					robesSelector.SetActive (true);
				
					break;

				//case 10 is for the cloak
				case 10:
				
					//equip the cloak
					currentArmor = 10;

					//set the armor bonus to 0
					armorBonus = 0;
				
					//replace the cloak and enable the cloak selector
					cloak.sortingLayerName = "Units";
					cloakSelector.SetActive (true);
				
					break;

				//default case
				default:
				
					//does nothing
					break;
			}
		}

		//call the levelUp function (using the default case) to update stats and all UI elements
		levelUp (99);

		//play a sound to signal successful equip/unequip (currently using the move sound)
		SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);

		//logic for granting an extra move with the cloak equipped
		//if the player has the cloak equipped and the playerQuick bool is false
		if (currentArmor == 10) {
			
			//set the playerQuick bool to true
			playerQuick = true;
			
		} else {
			
			//reset the playerQuick bool to false
			playerQuick = false;
		}

		//end the player's current turn
		GameManager.instance.playersTurn = false;
	}

	//function that allows the player to spend experience to level up character stats
	public void levelUp (int stat) {

		//switch on the integer passed into the function to determine which stat will be increased
		switch (stat) {

		//first case is for the strength stat
		case 0:

			//if the player does not have enough exp to increase the requested stat
			//Note: the int casts were left in place when the int substat was changed to XpMod in case I decide to change it back
			if (exp < (int)(strength * 100)) {

				//simply play the gameover sound
				SoundManager.instance.RandomizeSfx (gameOverSound);

			//otherwise
			} else {

				//subtract the appropriate amount of experience calculated from the current stat value
				exp -= (int)(strength * 100); 

				//increase the requested stat
				strength++;

				//play the level up sound
				SoundManager.instance.RandomizeSfx (levelUpSound);

				//increment the expSpent count
				expSpent++;

			}
			//break out of the switch
			break;

		//second case is for the dexterity stat
		case 1:

			//if the player does not have enough exp to increase the requested stat
			if (exp < (int)(dexterity * 100)) {

				//simplly play the gameover sound
				SoundManager.instance.RandomizeSfx (gameOverSound);

			//otherwise
			} else {

				//subtract the appropriate amount of experience calculated from the current stat value
				exp -= (int)(dexterity * 100); 
			
				//increase the requested stat
				dexterity++;

				//play the level up sound
				SoundManager.instance.RandomizeSfx (levelUpSound);

				//increment the expSpent count
				expSpent++;
			}

			//break out of the switch
			break;

		//third case is for the intelligence stat
		case 2:

			//if the player does not have enough exp to increase the requested stat
			if (exp < (int)(intelligence * 100)) {
				
				//simplly play the gameover sound
				SoundManager.instance.RandomizeSfx (gameOverSound);
				
				//otherwise
			} else {

				//subtract the appropriate amount of experience calculated from the current stat value 
				exp -= (int)(intelligence * 100); 
			
				//increase the requested stat
				intelligence++;

				//play the level up sound
				SoundManager.instance.RandomizeSfx (levelUpSound);

				//increment the expSpent count
				expSpent++;
			}

			//break out of the switch
			break;

		//last case is for the constitution stat
		case 3:

			//if the player does not have enough exp to increase the requested stat
			if (exp < (int)(constitution * 100)) {
				
				//simplly play the gameover sound
				SoundManager.instance.RandomizeSfx (gameOverSound);
				
				//otherwise
			} else {

				//subtract the appropriate amount of experience calculated from the current stat value
				exp -= (int)(constitution * 100); 
			
				//increase the requested stat
				constitution++;

				//play the level up sound
				SoundManager.instance.RandomizeSfx (levelUpSound);

				//increment the expSpent count
				expSpent++;

				//recalculate max hp
				//set the player's max Hp depending on current constitution starting by setting maxHP to 50
				maxHp = 50;
				
				//then loop through iterations equal to one less the player's constitution, adding 10 maxHP each time
				for (int i = 1; i < constitution; i++) {
					
					//this adds 10 maxHP for each point in constitution
					maxHp += 10;
				}

				//heal the player for 20 health points
				GainHealth (20);
			}

			//break out of the switch
			break;

		//the default case can be used to bypass any stat increase and simply update all sub-stats and UI text elements
		default:
			break;

		}

		//next, refresh all sub-stats and UI elements to reflect any change

		//update the player's exp points in the UI
		expText.text = "XP: " + exp;
		
		//set the player's damage depending on current strength and equipped weapon damage
		damage = strength + weapDam;
		
		//set the player's find skill depending on current dexterity
		find = dexterity * 10;
		
		//set the player's xpMod depending on current intelligence starting by setting xpMod to 1
		xpMod = 1f;

		//loop through iterations equal to one less the player's intelligence, adding 0.01 to xpMod each time
		for(int i = 1; i < intelligence; i++) {

			//this gives a bonus to xp gained from  all sources
			xpMod += 0.01f;
		}
		
		//set the player's max Hp depending on current constitution starting by setting maxHP to 50
		maxHp = 50;
		
		//then loop through iterations equal to one less the player's constitution, adding 10 maxHP each time
		for (int i = 1; i < constitution; i++) {
			
			//this adds 10 maxHP for each point in constitution
			maxHp += 10;
		}

		//add the armor bonus given by the player's current equipment
		//maxHp += armorBonus;

		//update the player's str, dex, int, con and dam in the UI
		strText.text = "STR: " + strength;
		dexText.text = "DEX: " + dexterity;
		intText.text = "INT: " + intelligence;
		conText.text = "CON: " + constitution;
		
		//update the player's dam, find, xpMod, and maxhp in the UI
		damText.text = "DAM: " + damage;
		finText.text = "FIND: " + find;
		xpModText.text = "XpMOD:" + xpMod;
		maxHpText.text = "MxHp: " + maxHp;

		//run the set beard function to show the appropriate beard
		setBeard ();

		//finally, run the showStatInvButtons function to see if the player can level up any more
		showStatInvButtons ();
	}

	//function that runs when the status panel is opened (and after spending exp) to see if the player has enough exp to level any stats
	public void showStatInvButtons () {

		//first, disable all the level up buttons and displace the level up indicator
		strUp.SetActive (false);
		dexUp.SetActive (false);
		intUp.SetActive (false);
		conUp.SetActive (false);
		levelUpIndicator.sortingLayerName = "Default";

		//second, disable all armor equip buttons
		paddedUIButton.SetActive (false);
		leatherUIButton.SetActive (false);
		studdedUIButton.SetActive (false);
		hideUIButton.SetActive (false);
		scaleUIButton.SetActive (false);
		chainUIButton.SetActive (false);
		plateUIButton.SetActive (false);
		dragonUIButton.SetActive (false);

		//and weapon equip buttons
		knifeUIButton.SetActive (false);
		staffUIButton.SetActive (false);
		daggerUIButton.SetActive (false);
		maceUIButton.SetActive (false);
		flailUIButton.SetActive (false);
		broadUIButton.SetActive (false);
		greatUIButton.SetActive (false);
		hammerUIButton.SetActive (false);

		//and special equip buttons and item help text
		fireOrbUIButton.SetActive (false);
		iceOrbUIButton.SetActive (false);
		robesUIButton.SetActive (false);
		cloakUIButton.SetActive (false);
		itemHelp.text = "You do not currently possess a Special Equipment Item.";

		//next, check each stat vs player experience to see if the button should be activated
		//Note: int cast was left in place after changing the int sub stat to XpMod in case I decide to change it back
		if (exp >= (int)(strength * 100)) {

			//activate the button if the player has enough exp
			strUp.SetActive (true);

			//replace the level up indicator
			levelUpIndicator.sortingLayerName = "Units";
		}

		if (exp >= (int)(dexterity * 100)) {

			//activate the button if the player has enough exp
			dexUp.SetActive (true);

			//replace the level up indicator
			levelUpIndicator.sortingLayerName = "Units";
		}

		if (exp >= (int)(intelligence * 100)) {

			//activate the button if the player has enough exp
			intUp.SetActive (true);

			//replace the level up indicator
			levelUpIndicator.sortingLayerName = "Units";
		}

		if (exp >= (int)(constitution * 100)) {

			//activate the button if the player has enough exp
			conUp.SetActive (true);

			//replace the level up indicator
			levelUpIndicator.sortingLayerName = "Units";
		}

		//finally, check each armor inventory requirement against the player's current inventory and activate any appropriate buttons
		//if the player has found more than 7 armor sets
		if (inventory [3] > 7) {

			//enable the dragon armor button
			dragonUIButton.SetActive (true);

		}

		//if the player has found more than 6 armor sets
		if (inventory [3] > 6) {

			//enable the plate armor button
			plateUIButton.SetActive (true);
		}

		//if the player has found more than 5 armor sets
		if (inventory [3] > 5) {
			
			//enable the chain armor button
			chainUIButton.SetActive(true);
		}

		//if the player has found more than 4 armor sets
		if (inventory [3] > 4) {
			
			//enable the scale armor button
			scaleUIButton.SetActive(true);
		}

		//if the player has found more than 3 armor sets
		if (inventory [3] > 3) {
			
			//enable the hide armor button
			hideUIButton.SetActive(true);
		}

		//if the player has found more than 2 armor set
		if (inventory [3] > 2) {

			//enable the studded armor button
			studdedUIButton.SetActive(true);
		}

		//if the player has found more than 1 armor sets
		if (inventory [3] > 1) {

			//enable the leather armor button
			leatherUIButton.SetActive(true);
		}

		//if the player has found more than 0 armor sets
		if (inventory [3] > 0) {
			
			//enable the padded armor button
			paddedUIButton.SetActive(true);
		}

		//and for weapons as well
		//if the player has found more than 7 weapons
		if (inventory [1] > 7) {
			
			//enable the war hammer button
			hammerUIButton.SetActive(true);
		}

		//if the player has found more than 6 weapons
		if (inventory [1] > 6) {

			//enable the great sword button
			greatUIButton.SetActive (true);
		}

		//if the player has found more than 5 weapons
		if (inventory [1] > 5) {

			//enable the broad sword button
			broadUIButton.SetActive(true);
		}

		//if the player has found more than 4 weapons
		if (inventory [1] > 4) {
			
			//enable the flail button
			flailUIButton.SetActive(true);
		}

		//if the player has found more than 3 weapons
		if (inventory [1] > 3) {
			
			//enable the mace button
			maceUIButton.SetActive(true);
		}

		//if the player has found more than 2 weapon
		if (inventory [1] > 2) {

			//enable the dagger button
			daggerUIButton.SetActive (true);
		}

		//if the player has found more than 1 weapon
		if (inventory [1] > 1) {
			
			//enable the staff button
			staffUIButton.SetActive (true);
		}

		//if the player has found more than 0 weapons
		if (inventory [1] > 0) {

			//enable the knife button
			knifeUIButton.SetActive (true);
		}

		//finally, check to see which special item (if any) to enable
		//if the special item in the player's inventory is the fireOrb
		if (inventory [7] == 1) {

			//enable the fireOrb button
			fireOrbUIButton.SetActive (true);

			//set the help item text element to the fire Orb help text
			itemHelp.text = "Fire Orb - equips as weapon, double click/tap any location to cast Fireball: deals damage based on intelligence, requires two actions";
		}

		//if the special item in the player's inventory is the iceOrb
		if (inventory [7] == 2) {

			//enable the iceOrb button
			iceOrbUIButton.SetActive (true);

			//set the help item text element to the fire Orb help text
			itemHelp.text = "Ice Orb - equips as weapon, double click/tap any location to cast Ice Shards: deals damage based on intelligence, requires one action";
		}

		//if the special item in the player's inventorty is the robes
		if (inventory [7] == 3) {

			//enable the robes button
			robesUIButton.SetActive (true);

			//set the help item text element to the fire Orb help text
			itemHelp.text = "Robes - equips as armor, double click/tap any location to cast Teleport: instantly arrive at the chosen location, requires two actions";
		}

		//if the special item in the player's inventorty is the cloak
		if (inventory [7] == 4) {
			
			//enable the cloak button
			cloakUIButton.SetActive (true);

			//set the help item text element to the fire Orb help text
			itemHelp.text = "Cloak - equips as armor, grants one additional action per player turn";
		}
	}

	//check to see if the game should end
	private void CheckIfGameOver() {

		//if the player's health reaches zero
		if (health <= 0) {

			GameManager.instance.setGameWin (false);

			StartCoroutine (PlayerDeath ());

		}
	}

	//a coroutine that initiates the player's death animation and calls the game manager's game over function after a delay
	IEnumerator PlayerDeath () {

		//play the gameover sound
		SoundManager.instance.PlaySingle (gameOverSound);
		
		//stop the background  music
		SoundManager.instance.musicSource.Stop ();

		//call the trigger animation function to start the player and equipment death animations
		triggerAnimation ("death");

		//trigger the special equipment death animations as well
		fireOrbAnimator.SetTrigger ("death");
		iceOrbAnimator.SetTrigger ("death");

		//displace the levelup indicator
		levelUpIndicator.sortingLayerName = "Default";

		//set the color of the status text UI element
		statusText.color = new Color (1.0f, 0.0f, 0.0f);
		
		//change the text on the status text UI text element
		statusText.text = "You Have Died!!!";

		//delay to allow the animation to finish before calling the game manager's game over function
		yield return new WaitForSeconds (3.0f);

		//call the gameover function in game manager
		GameManager.instance.GameOver();
	}

	//a function that unequips the player's weapon and armor (used for exiting a room without seeing the player's equipment dissappear)
	private void unequip () {

		//call the equip weap and equip armor functions and pass in the currently equipped weapon and armor
		equipWeap(currentWeap);
		equipArmor (currentArmor);
	}

	//function that relays the status of the roomOver bool
	public bool roomOverCheck() {

		//return the variable roomOver
		return roomOver;
	}

	//a function that returns the player's current armor bonus (for damage reduction on enemy attacks)
	public int damReduct() {

		//return the player's current armor bonus
		return armorBonus;
	}

	//a function that allows other scripts to access the status display area
	public void statusUpdate(string message, Color color) {

		//set the color of the status text UI element
		statusText.color = color;
		
		//change the text on the status text UI text element
		statusText.text = message;
	}

	//decide what to do when the player double clicks on a tile
	public void doubleClick (Vector3 position) {

		//if the player has the fireOrb equipped
		if (currentWeap == 9) {

			//play the randomized explosion sound effect
			SoundManager.instance.RandomizeSfx (explosion);

			//start the fireOrb attack animation
			fireOrbAnimator.SetTrigger ("playerAttack");

			//call the animation trigger function and pass in playerAttack
			triggerAnimation ("playerAttack");

			//spawn a fireball at position, as well as four other adjacent positions, and set them to destroy after a delay
			Destroy(Instantiate (FireBall, position, Quaternion.identity), 0.75f);
			Destroy(Instantiate (FireBall, new Vector3 (position.x + 1, position.y + 1, position.z), Quaternion.identity), 0.75f);
			Destroy(Instantiate (FireBall, new Vector3 (position.x + 1, position.y - 1, position.z), Quaternion.identity), 0.75f);
			Destroy(Instantiate (FireBall, new Vector3 (position.x - 1, position.y + 1, position.z), Quaternion.identity), 0.75f);
			Destroy(Instantiate (FireBall, new Vector3 (position.x - 1, position.y - 1, position.z), Quaternion.identity), 0.75f);

			//acknowledge the fact that the player cast a spell this turn
			playerSlow = true;

			//call the end turn function to end the player's turn after a delay
			StartCoroutine ("endTurn", 0.75f);
		}

		//if the player has the iceOrb equipped
		if (currentWeap == 10) {
			
			//play the randomized explosion sound effect
			SoundManager.instance.RandomizeSfx (explosion);
			
			//start the iceOrb attack animation
			iceOrbAnimator.SetTrigger ("playerAttack");

			//call the trigger animation function and pass in playerAttack
			triggerAnimation ("playerAttack");
			
			//spawn IceShards at position, as well as four other adjacent positions, and set them to destroy after a delay
			Destroy(Instantiate (IceShards, position, Quaternion.identity), 0.75f);
			Destroy(Instantiate (IceShards, new Vector3 (position.x + 1, position.y, position.z), Quaternion.identity), 0.75f);
			Destroy(Instantiate (IceShards, new Vector3 (position.x, position.y + 1, position.z), Quaternion.identity), 0.75f);
			Destroy(Instantiate (IceShards, new Vector3 (position.x - 1, position.y, position.z), Quaternion.identity), 0.75f);
			Destroy(Instantiate (IceShards, new Vector3 (position.x, position.y - 1, position.z), Quaternion.identity), 0.75f);
			
			//call the end turn function to end the player's turn after a delay
			StartCoroutine ("endTurn", 0.75f);
		}

		//if the player has the robes equipped
		if (currentArmor == 9) {

			//play the teleport effect (currently the explosion effect)
			SoundManager.instance.RandomizeSfx (explosion);

			//call the overloaded trigger animation function with player attack and player hit
			triggerAnimation ("playerAttack", "playerHit");

			//spawn a teleport at the double click position and also at the players current position, and set them to destroy after a delay
			Destroy (Instantiate (Teleport, position, Quaternion.identity), 0.75f);
			Destroy (Instantiate (Teleport, transform.position, Quaternion.identity), 0.75f);

			//move the player to the double click position
			transform.position = position;

			//acknowledge the fact that the player cast a spell this turn
			playerSlow = true;

			//call the end turn function to end the player's turn after a delay
			StartCoroutine ("endTurn", 0.75f);
		}
	}

	//function that returns the player's current intelligence
	public int getInt () {

		//return the player's current intelligence
		return intelligence;
	}

	//a function that ends the player's turn after a delay
	IEnumerator endTurn (float delay) {

		//wait for the requested delay time
		yield return new WaitForSeconds (delay);

		//logic for granting an extra move with the cloak equipped
		//if the player has the cloak equipped and the playerQuick bool is false
		if (currentArmor == 10) {
			
			//set the playerQuick bool to true
			playerQuick = true;
			
		} else {
			
			//reset the playerQuick bool to false
			playerQuick = false;
		}

		//tell the game manager that the player's turn is over
		GameManager.instance.playersTurn = false;
	}

	//function that allows other scripts to access the playerSlow bool
	public bool playerSlowBool() {

		//return the playerSlow bool
		return playerSlow;
	}

	//overloaded function that allows other scripts to change the value of the playerSlow bool
	public void playerSlowBool(bool newValue) {

		//set the playerSlow bool value equal to the argument
		playerSlow = newValue;
	}

	//function that allows other scripts to access the playerQuick bool
	public bool playerQuickBool() {

		//return the playerQuick bool
		return playerQuick;
	}

	//overloaded function that allows other scripts to change the value of the playerQuick bool
	public void playerQuickBool(bool newValue) {

		//set the playerQuick bool value with the argument
		playerQuick = newValue;
	}

	//function that sets the player and all armor, weapon, and beard animators triggers
	public void triggerAnimation (string trigger) {

		//set the player animator trigger to the string passed into the function
		animator.SetTrigger (trigger);
		
		//do the same with the weapon animator triggers
		knifeAnimator.SetTrigger (trigger);
		staffAnimator.SetTrigger (trigger);
		daggerAnimator.SetTrigger (trigger);
		maceAnimator.SetTrigger (trigger);
		flailAnimator.SetTrigger (trigger);
		broadAnimator.SetTrigger (trigger);
		greatAnimator.SetTrigger (trigger);
		hammerAnimator.SetTrigger (trigger);
		
		//as well as the armor animation triggers
		paddedAnimator.SetTrigger (trigger);
		leatherAnimator.SetTrigger (trigger);
		studdedAnimator.SetTrigger (trigger);
		hideAnimator.SetTrigger (trigger);
		scaleAnimator.SetTrigger (trigger);
		chainAnimator.SetTrigger (trigger);
		plateAnimator.SetTrigger (trigger);
		dragonAnimator.SetTrigger (trigger);
		robesAnimator.SetTrigger (trigger);
		cloakAnimator.SetTrigger (trigger);

		//and finally beards
		beard1Animator.SetTrigger (trigger);
		beard2Animator.SetTrigger (trigger);
		beard3Animator.SetTrigger (trigger);
		beard4Animator.SetTrigger (trigger);
	}

	//overloaded function that sets the player and armor animations to one trigger, and the weapon animations to another
	public void triggerAnimation (string trigger, string trigger2) {

		//set the player and armor animations to the first argument
		animator.SetTrigger (trigger);
		paddedAnimator.SetTrigger (trigger);
		leatherAnimator.SetTrigger (trigger);
		studdedAnimator.SetTrigger (trigger);
		hideAnimator.SetTrigger (trigger);
		scaleAnimator.SetTrigger (trigger);
		chainAnimator.SetTrigger (trigger);
		plateAnimator.SetTrigger (trigger);
		dragonAnimator.SetTrigger (trigger);
		robesAnimator.SetTrigger (trigger);
		cloakAnimator.SetTrigger (trigger);

		//and set the weapon animatios to the second argument
		knifeAnimator.SetTrigger (trigger2);
		staffAnimator.SetTrigger (trigger2);
		daggerAnimator.SetTrigger (trigger2);
		maceAnimator.SetTrigger (trigger2);
		flailAnimator.SetTrigger (trigger2);
		broadAnimator.SetTrigger (trigger2);
		greatAnimator.SetTrigger (trigger2);
		hammerAnimator.SetTrigger (trigger2);

		//and finally beards
		beard1Animator.SetTrigger (trigger);
		beard2Animator.SetTrigger (trigger);
		beard3Animator.SetTrigger (trigger);
		beard4Animator.SetTrigger (trigger);
	}

	//function that sets the character's beard depending on the exp spent variable's current value
	private void setBeard() {

		//displace all beards before setting the current beard
		beard1.sortingLayerName = "Default";
		beard2.sortingLayerName = "Default";
		beard3.sortingLayerName = "Default";
		beard4.sortingLayerName = "Default";

		//set the current beard depending on how many times the player has spent experience
		if (expSpent > 20) {

			beard4.sortingLayerName = "Units";

		} else if (expSpent > 15) {

			beard3.sortingLayerName = "Units";

		} else if (expSpent > 10) {

			beard2.sortingLayerName = "Units";

		} else if (expSpent > 5) {

			beard1.sortingLayerName = "Units";
		}
	}

	//function that gives access to the private member exp sent
	public int playerExpSpent () {

		return expSpent;
	}

	//overloaded function that allows other scripts to set the private member exp sent
	public void playerExpSpent(int xpShave) {

		expSpent = xpShave;

	}

	//function that gives access to the private member inventory[7] (the special item inventory slot)
	public int specialItem() {

		return inventory [7];
	}

	//overloaded function that allows other scripts to set the special item inventory slot (inventory[7])
	public void specialItem(int value) {

		inventory [7] = value;
	}

	//function that gives access to the private members currentArmor or currentWeap depending on the argument passed
	public int currentEquip(string category) {

		if (category == "armor") {

			return currentArmor;

		} else if (category == "weapon") {

			return currentWeap;

		} else {

			return 0;
		}
	}
}
