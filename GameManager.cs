/*
 *  Title:       GameManager.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/21/2015
 *  Modified:    04/04/2016
 *  Resources:   Adapted from original gamemanager script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: This script handles transitions from one room to another (during which all other objects are destroyed) and calls functions to set up the room, display room details, and initiate player/enemy turns
 */

using UnityEngine;
using System.Collections;

//allows us to manage the UI
using UnityEngine.UI;

//required for using lists
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


	public BoardManager boardScript;

	//the time to wait before starting the next room
	public float roomStartDelay = 2f;

	//track the time delay between turns
	public float turnDelay = 0.15f;

	//allows us to access public functions and variables from any script in the game
	public static GameManager instance = null;

	//track the player's xp
	public int playerExpPoints = 0;
	public int playerExpSpent = 0;

	//track the player's equipped weapon and armor
	public int playerWeap = 0;
	public int playerArmor = 0;

	//track the player's str, dex, int, and con
	public int playerStr = 1;
	public int playerDex = 1;
	public int playerInt = 1;
	public int playerCon = 1;

	//to track the player's hp
	public int playerHealthPoints = 50;

	//track the player's inventory : Drinks, weapons, strength potions, armor, experience potions, health + potions, health++ potions, and special items
	private int[] playerInv = new int[] {0, 1, 0, 0, 0, 0, 0, 0};

	//track if the player is advancing in the game, or attempting to retry from the beginning
	public bool retry = false;

	//track if the player is continuing the game after beating the final boss
	public bool continueGame = false;

	//track if it is the player's turn or not (hidden in the editor, despite being public)
	public bool playersTurn = true;

	//track if the current floor has dropped a weapon or armor set already
	public bool weaponDrop = true;
	public bool armorDrop = false;

	public bool menu_open = true;

	public Texture potion_image;
	public Texture str_potion_image;
	public Texture exp_potion_image;

	public GUISkin hotbar;

	//UI REFERENCES
	//text to display for the current room in the UI
	private Text roomText;

	//text to display for the current floor in the UI
	private Text floorText;

	//text to display the time taken for this run
	private Text timeText;

	//stores a reference to the room image UI panel so we can enable and disable on room transition
	private GameObject roomImage;

	//stores a reference to the health text UI panel so we can enable and disable on room transition
	private GameObject healthText;

	//store a reference to the exp text UI panel so we can enable and disable on room transition
	private GameObject expText;

	//store a reference to the Status UI button so we can enable and disable on room transition
	private GameObject statusPanel;

	//stores a reference to the replay or quit UI panel so we can enable and disable on game over
	private GameObject retryOrQuit;

	//stores as reference to the continue button so we can enable it only when the player has beaten the final boss
	private GameObject continueButton;

	//stores references to the status bottom/top background panels so we can enable and disable on room/floor change
	private GameObject statusBottomBackPanel;
	private GameObject statusTopBackPanel;

	//the starting room of the game
	private int room = 0;

	//the starting floor of the game
	private int floor = 1;

	//declare a list of enemy objects to keep track of enemies and send them orders to move
	private List <Enemy> enemies;

	//tracks whether or not the enemies should be moving
	public bool enemiesMoving;
	
	//tracks if the board is being set up (in order to prevent the player from moving during setup)
	public bool doingSetup;

	//bool that determines if the key has spawned for this room
	private bool keySpawn = false;

	//bool that keeps track of if the player is accessing the secret room
	private bool secretRoom = false;

	//acknowledge if the player has beaten the game or not
	private bool gameWin = false;

	//keep track of the player's final position in the room (for spawning the player in the next room)
	private Vector3 finalPlayerPos = new Vector3 (0.0f, 0.0f, 0.0f);

	//keep track of the time
	private float startTime;
	private Text currentTime;
	private int minutes = 0;
	private int seconds = 0;
	private int finalMins;
	private int finalSecs;

	//awake function will start with a singleton pattern
	void Awake () {

		//set the start time for the game
		startTime = Time.time;

		//check to see if there is an instance of GameManager present
		if (instance == null) {

			//if not, create one
			instance = this;

		//check to make sure we only have one instance of GameManager at a time
		} else if (instance != this) {

			//destroy any instance of GameManager that isnt this one
			Destroy (gameObject);
		}

		//keeps the game from destroying this instance of GameManager on scene change
		//allows GameManager to keep track of score across all scenes
		DontDestroyOnLoad (gameObject);

		//set enemies to equal a new list of type enemy
		enemies = new List <Enemy>();

		//get and store component reference to boardManager script
		boardScript = GetComponent<BoardManager>();
	}

	//called every time a scene is loaded, used to add to room number and call the init game function
	private void OnLevelWasLoaded (int index){

		//change the key spawn bool to false
		keySpawn = false;

		//fade the game back in from black
		this.GetComponent<ScreenFade> ().BeginFade (-1);

		//if the continue game boolean is true
		if (continueGame) {

			//start the music back on the Warlock track
			SoundManager.instance.musicSource.clip = SoundManager.instance.Warlock;
			SoundManager.instance.musicSource.Play ();

			//set the floor and room values
			floor = 10;
			room = 1;

			//reset the game win booleans and continue game booleans
			gameWin = false;
			continueGame = false;

			//reset the armor and weapon drop bools
			weaponDrop = false;
			armorDrop = false;

			//set the current time to the final minutes and seconds of the run so the player doesnt lose time
			minutes = finalMins;
			seconds = finalSecs;

		//if the retry bool is false
		} else if (!retry) {

			//if you have reached the end of the current floor
			if (room == 10) {

				//reset the room number to 1
				room = 1;

				//increment the floor number
				floor++;

				//reset the armor and weapon drop bools
				weaponDrop = false;
				armorDrop = false;

			//otherwise
			} else {

				//increment the room number
				room++;

				//for quick testing of new floors
				//floor++;
			}

		//otherwise, retry is true
		} else {

			//reset the game timer
			minutes = 0;
			seconds = 0;
			startTime = Time.time;

			//set the room to 1
			room = 1;

			//set the floor to 1
			floor = 1;

			//set game win boolean to false
			gameWin = false;

			//set player exp to 0
			playerExpPoints = 0;
			playerExpSpent = 0;

			//set player equipped weapon and armor to 1 and 0
			playerWeap = 0;
			playerArmor = 0;

			//set player strength, dexterity, intelligence and constitution to 1
			playerStr = 1;
			playerDex = 1;
			playerInt = 1;
			playerCon = 1;

			//set player health to 50
			playerHealthPoints = playerCon * 50;

			//set the player's inventory to the initial default
			playerInv = new int[] {0, 1, 0, 0, 0, 0, 0, 0};

			//set player's turn to be true
			playersTurn = true;

			//set retry to false
			retry = false;

			//enable the game manager
			enabled = true;

			//set the armor and weapon drop bools to default
			weaponDrop = true;
			armorDrop = false;

			//set the music to Warlock and start playing the room music again
			SoundManager.instance.musicSource.clip = SoundManager.instance.Warlock;
			SoundManager.instance.musicSource.Play ();
		}

		//call the initialize game function
		InitGame ();
	}

	//sets up the game at the desired room
	void InitGame(){

		//tell the game manager that setup is being performed
		doingSetup = true;

		//find the timer text UI text element
		currentTime = GameObject.Find ("TimerText").GetComponent<Text> ();

		//find the room image UI object and save a reference
		roomImage = GameObject.Find ("roomImage");

		//find the health text UI object and save a reference
		healthText = GameObject.Find ("healthText");

		//find the exp text UI object and save a reference
		expText = GameObject.Find ("expText");

		//find the status UI button object and save a reference
		statusPanel = GameObject.Find ("statusPanel");

		//find the retry or quit UI object and save a reference
		retryOrQuit = GameObject.Find ("retryOrQuitPanel");

		//find the continue button
		continueButton = GameObject.Find ("continueButton");

		//find the options UI button object and save a reference
		statusBottomBackPanel = GameObject.Find ("BottomStatusBackPanel");
		statusTopBackPanel = GameObject.Find ("TopStatusBackPanel");

		//find the room text game object, save a reference, and get the text component
		roomText = GameObject.Find ("roomText").GetComponent<Text>();

		//set the room text UI object to display the current room
		//if the player is in the secret Room
		if (GameManager.instance.secretRoomBool ()) {

			//set the specific room text
			roomText.text = "Secret Room";

		//if the room is 0
		} else if (room < 1) {

			//set the room text to say Room 1
			roomText.text = "Room 1";

			//otherwise, the room number should be correct
		} else {

			//set the room text to use the current value of the room variable
			roomText.text = "Room " + room;
		}

		//find the floor text game object, save a reference and get the text component
		floorText = GameObject.Find ("floorText").GetComponent<Text>();

		//set the floor text UI object to display the current floor
		floorText.text = "Floor " + floor;

		//find the time text game object and save a reference to the text component
		timeText = GameObject.Find ("timeText").GetComponent<Text> ();

		//disable the continue button
		continueButton.SetActive (false);

		//disable the retry or quit UI panel
		retryOrQuit.SetActive (false);

		//set the room image to active
		roomImage.SetActive (true);

		//diable the health text
		healthText.SetActive (false);

		//disable the exp text
		expText.SetActive (false);

		//disable the status button
		statusPanel.SetActive (false);

		//disable the status background panels
		statusBottomBackPanel.SetActive (false);
		statusTopBackPanel.SetActive (false);

		//invoke the hide room image function (passing in the room start delay for delay time [default 2 seconds])
		Invoke ("HideRoomImage", roomStartDelay);

		//clear out the list of enemies from the last room
		enemies.Clear ();

		//set up the current scene
		boardScript.SetupScene(room, floor);
	}

	//function to switch the room image and health text after the room has been set up (called from within init game)
	private void HideRoomImage() {

		//disable the room Image
		roomImage.SetActive (false);

		//enable the health text
		healthText.SetActive (true);

		//enable the exp text
		expText.SetActive (true);

		//enable the status button
		statusPanel.SetActive (true);

		//enable the status background panels
		statusBottomBackPanel.SetActive (true);
		statusTopBackPanel.SetActive (true);

		//tell the game manager that we are no longer doing setup (allowing the player to move)
		doingSetup = false;

		menu_open = false;
	}

	//function for ending the game
	public void GameOver() {

		//log the final times for this run
		finalMins = minutes;
		finalSecs = seconds;

		//if the player has not beaten the beholder
		if (!gameWin) {

			//play the retired sheriff's theme
			SoundManager.instance.musicSource.clip = SoundManager.instance.Sheriff;
			SoundManager.instance.musicSource.Play ();

		} else {

			//play narrow escape
			SoundManager.instance.musicSource.clip = SoundManager.instance.Escape;
			SoundManager.instance.musicSource.Play ();
		}

		//set the floor text to tell the player how far they got
		floorText.text = "Your quest lasted for"; 

		//if the player finished more or less than 1 floor
		if ((floor - 1) != 1) {
			roomText.text = (floor - 1) + " floors, and ";

		//otherwise
		} else {
			roomText.text = (floor - 1) + " floor, and ";
		}
	
		//if the player got past the first room, modify the room text
		if (room > 1) {
			roomText.text += " " + room + " rooms.";

		//otherwise
		} else {
			roomText.text += " " + room + " room.";
		}

		//if the player has won the game, the game over message has to change
		if (!gameWin) {

			if (finalMins == 1) {

				//set the time text to tell the player how long it took them
				timeText.text = "You took " + finalMins + " minute and " + finalSecs + " seconds\nto meet your doom.";

			} else {

				//set the time text to tell the player how long it took them
				timeText.text = "You took " + finalMins + " minutes and " + finalSecs + " seconds\nto meet your doom.";
			}

			//change the color of the room image
			roomImage.GetComponent<Image>().color = new Color (0.318f, 0, 0);

		} else {

			if (finalMins == 1) {

				//set the time text to tell the player how long it took them
				timeText.text = "You took " + finalMins + " minute and " + finalSecs + " seconds\nto fulfill your destiny!";

			} else {

				//set the time text to tell the player how long it took them
				timeText.text = "You took " + finalMins + " minutes and " + finalSecs + " seconds\nto fulfill your destiny!";
			}

			//change the color of the room image
			roomImage.GetComponent<Image>().color = new Color (0, 0.318f, 0);
		}

		//enable the room image
		roomImage.SetActive (true);

		//enable the retry or quit panel
		retryOrQuit.SetActive (true);

		//if the player has beaten the final boss
		if (gameWin) {

			//enable the continue button
			continueButton.SetActive (true);
		}

		//disable the health text
		healthText.SetActive (false);

		//disable the exp text
		expText.SetActive (false);

		//disable the status button
		statusPanel.SetActive (false);

		//disable the status background panels
		statusBottomBackPanel.SetActive (false);
		statusTopBackPanel.SetActive (false);

		//if the player has not defeated the final boss
		if (!gameWin) {

			//disable the game manager
			enabled = false;
		}
	}

	void Update () {

		//if the timer is active
		if (currentTime != null && !roomImage.activeInHierarchy) {

			//set the seconds eqal to the difference between the current time and start time (converted to an int)
			seconds = (int)(Time.time - startTime);

			//if the seconds have reached 60
			if (seconds >= 60) {

				//reset the seconds to 0
				seconds = 0;

				//increment the minutes
				minutes++;

				//reset the starting time to the current time
				startTime = Time.time;
			}

			//set the current time in minutes
			currentTime.text = minutes + ":";

			//and add in the seconds in appropriate format
			if (seconds < 10) {

				currentTime.text += "0" + seconds;

			} else {

				currentTime.text += seconds;
			}
		}

		//check to see if the enemies are moving, or if it is the player's turn, or if we are doing setup
		if (playersTurn || enemiesMoving || doingSetup) {

			//if any are true, then return now and do not run any of the following code
			return;
		}

		//if enemies are not moving, we are not doing setup, and it is not the player's turn then start the enemies moving coroutine
		StartCoroutine (MoveEnemies ());
	}

	void OnGUI () {

		GUI.skin = hotbar;
		GUI.color = Color.white;

		if (roomImage != null && !roomImage.activeInHierarchy && !menu_open) {

			GUI.Box (new Rect (Screen.width - Screen.width, Screen.height / 2 - Screen.height / 11, Screen.width / 24, Screen.height / 12), "1");

			GUI.DrawTexture (new Rect (Screen.width - Screen.width, Screen.height / 2 - Screen.height / 11, Screen.width / 24, Screen.height / 12), potion_image);

			if (GUI.Button(new Rect(Screen.width - Screen.width, Screen.height / 2 - Screen.height / 11, Screen.width / 24, Screen.height / 12), "" + getPlayerInv()[0], new GUIStyle("label"))) {

				GameObject.Find ("Player").GetComponent<Player> ().usePlayerItem (0);
			}

			GUI.Box (new Rect (Screen.width - Screen.width, Screen.height / 2, Screen.width / 24, Screen.height / 12), "2");

			GUI.DrawTexture (new Rect (Screen.width - Screen.width, Screen.height / 2, Screen.width / 24, Screen.height / 12), str_potion_image);

			if (GUI.Button(new Rect(Screen.width - Screen.width, Screen.height / 2, Screen.width / 24, Screen.height / 12), "" + getPlayerInv()[2], new GUIStyle("label"))) {

				GameObject.Find ("Player").GetComponent<Player> ().usePlayerItem (2);
			}

			GUI.Box (new Rect (Screen.width - Screen.width, Screen.height / 2 + Screen.height / 11, Screen.width / 24, Screen.height / 12), "3");

			GUI.DrawTexture (new Rect (Screen.width - Screen.width, Screen.height / 2 + Screen.height / 11, Screen.width / 24, Screen.height / 12), exp_potion_image);

			if (GUI.Button(new Rect(Screen.width - Screen.width, Screen.height / 2 + Screen.height / 11, Screen.width / 24, Screen.height / 12), "" + getPlayerInv()[4], new GUIStyle("label"))) {

				GameObject.Find ("Player").GetComponent<Player> ().usePlayerItem (4);
			}
		}
	}

	//function that adds enemies to the enemy list
	public void AddEnemyToList (Enemy script){

		//use the add function of our enemy list to add an enemy, and pass in the parameter script
		enemies.Add (script);
	}

	//function to get the player's inventory from the GameManager at Room start
	public int[] getPlayerInv() {

		//return the player inventory from the game manager
		return playerInv;
	}

	//function to set the player's inventory in the GameManager at Room end
	public void setPlayerInv(int[] newPlayerInv) {

		//set the player inventory in the game manager
		playerInv = newPlayerInv;
	}

	//a coroutine to move the enemies in sequence
	IEnumerator MoveEnemies() {

		//keep track of how many enemies are dead
		int deadEnemies = 0;

		//set the enemies moving bool to true (since we are about to move the enemies)
		enemiesMoving = true;

		//let the player finish moving before the enemies try to move
		yield return new WaitForSeconds (turnDelay);

		//loop through the list of enemies and order them each move commands in sequence
		for (int i = 0; i < enemies.Count; i++) {

			//if the current enemy has more than 0 health points
			if (enemies[i].gameObject.GetComponent<Damagable>().hp > 0) {

				//move each enemy in the list
				enemies[i].MoveEnemy();

				//wait to move the next enemy for the amount of time it takes an enemy to move
				yield return new WaitForSeconds(enemies[i].moveTime);

			//if the current enemy is disabled
			} else {

				//increment the dead enemy count
				deadEnemies++;
			}
		}

		//if there aren't any enemies present
		if (deadEnemies == enemies.Count){

			//if the room number is divisible by 10
			if ((room > 0 && room % 10 == 0) && !keySpawn && floor != 9) {

				//call the spawn key function on the board manager script
				this.boardScript.spawnKey ();

				//set the key spawn bool to true
				keySpawn = true;
			}

			//keep the player from moving too quickly
			yield return new WaitForSeconds (turnDelay);
		}

		//if the player cast a spell last turn
		if (GameObject.Find ("Player").GetComponent<Player> ().playerSlowBool ()) {

			//reset the player slow bool so that the player can move on their next turn
			GameObject.Find ("Player").GetComponent<Player>().playerSlowBool(false);

		//otherwise, the player did not cast a spell last turn
		} else {

			//tell the game manager that it is the player's turn now
			playersTurn = true;
		}

		//tell the game Manager that the enemies are no longer moving
		enemiesMoving = false;

	}

	//function that returns the current floor number
	public int GetFloor() {

		//return the floor number
		return floor;
	}

	//function that returns the current room number
	public int GetRoom() {

		//return the room number
		return room;
	}

	//function that grabs the player's final position from the previous room, allowing us to spawn the player at the corresponding spot in the new room
	public void PlayerPos (Vector3 position) {

		//set the player's final position from the argument position
		finalPlayerPos = position;

	}

	//overloaded function that returns the player's stored final position from the previous room
	public Vector3 PlayerPos () {

		//return the saved final player position
		return finalPlayerPos;
	}

	//function that gives access to the secret room private member
	public bool secretRoomBool() {

		return secretRoom;
	}

	//overloaded function that lets other scripts set the secret room private member
	public void secretRoomBool(bool value) {

		secretRoom = value;
	}

	//function that allows access to the private member game win
	public bool getGameWin() {

		return gameWin;
	}

	//function that allows other scripts to set the game win boolean
	public void setGameWin(bool gameWon) {

		gameWin = gameWon;
	}
}
