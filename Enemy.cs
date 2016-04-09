/*
 *  Title:       Enemy.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/30/2015
 *  Modified:    04/02/2016
 *  Resources:   Adapted from original enemy script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: This script contains enemy and boss AI logic, allows enemies to move, locate and follow the player, interact with the player by attacking, and triggers enemy animations
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MovingObject {

	//track the amount of damage that the enemy does to the player
	public int playerDamage;

	//stores a reference to the animator
	private Animator animator;

	//track the player's position so the enemy knows where to move toward
	private Transform target;

	//make sure the enemy only moves once every two turns
	private bool skipMove = true;

	//store the clips we want to play for when the enemies attack
	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

	//a reference to the who text (turn allocation) UI element
	private Text whoText;

	//keep track of the turn count (necessary to allow the player an extra turn when wearing the cloak)
	private int turnCount = 0;

	//keep track of wether or not the enemy is casting a spell
	private bool casting = false;

	//for when a boss is casting a spell, save the position of the spell target and the spell choice
	private Vector3 castTarget;
	private int spellChoice;

	//gameobjects used for Boss spell casting
	public GameObject FireBall;
	public GameObject IceShards;

	//use protected and override because we will have a different implementation in the enemy class vs the movingObject class
	protected override void Start () {

		//have the enemy script add itself to the game manager's enemy list (allows game manager to call the move enemy function)
		GameManager.instance.AddEnemyToList (this);

		//if the enemy is a boss
		if (tag == "Boss") {

			animator = transform.FindChild ("body").transform.FindChild ("face").GetComponent <Animator> ();

		//otherwise
		} else {

			//get and store a component reference to the animator
			animator = GetComponent<Animator> ();
		}

		//store the player's position in the target variable
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		//call the start function from our base class (MovingObject)
		base.Start ();
	}

	//this is the enemy's own special implementation of the attempt move function
	protected override void AttemptMove <T> (int xDir, int yDir){

		//if the enemy is a displacer or a flayer
		if (ToString ().Substring (0, (ToString ().Length - 15)) == "Flayer" ||
			ToString ().Substring (0, (ToString ().Length - 15)) == "Displacer") {

			//avoid the standard skipMove cycle
			skipMove = false;
		}

		//if the playerQuick bool is true and the player has not yet had 2 quick turns
		if (GameObject.Find ("Player").GetComponent<Player> ().playerQuickBool () && turnCount < 2) {

			//set the skip move bool to true
			skipMove = true;
		}

		//if the skip move bool is true or the player has reached the end of the room
		if (skipMove || GameObject.Find ("Player").GetComponent<Player>().roomOverCheck()) {

			//increment the turn count
			turnCount++;

			//change skip move bool to false and do not run any more code
			skipMove = false;

			return;
		}

		if (GameObject.Find ("WhoText") != null) {
		
			//find the who text UI element
			whoText = GameObject.Find ("WhoText").GetComponent<Text>();

			//set the turn allocation text UI value and color
			whoText.color = new Color (1.0f, 0.0f, 0.0f);
			whoText.text = ToString ().Substring (0, (ToString ().Length - 15));
		}

		//call the attempt move function from the base class (Moving Object)
		base.AttemptMove<T> (xDir, yDir);

		//since the enemy just moved set skip move to true
		skipMove = true;

		//reset the turn count
		turnCount = 0;
	}

	//function that enables the enemies to try and move to the player's position (called by GameManager)
	public void MoveEnemy() {

		//declare variables and set x and y coords to zero
		int xDir = 0;
		int yDir = 0;

		//if the enemy is a boss and is either casting or the player is in range to lock on for casting next turn
		if (tag == "Boss" && (casting || (Mathf.Abs (target.transform.position.x - transform.position.x) < 2 && 
		                                  Mathf.Abs (target.transform.position.y - transform.position.y) == 2) || 
		                      			 (Mathf.Abs (target.transform.position.x - transform.position.x) == 2 && 
		                                  Mathf.Abs (target.transform.position.y - transform.position.y) < 2))) {

			//if the playerQuick bool is true and the player has not yet had 2 quick turns
			if (GameObject.Find ("Player").GetComponent<Player> ().playerQuickBool () && turnCount < 2) {
				
				//set the skip move bool to true
				skipMove = true;

			//if the skip move bool is true or the player has reached the end of the room
			} else if (skipMove || GameObject.Find ("Player").GetComponent<Player>().roomOverCheck()) {
				
				//increment the turn count
				turnCount++;
				
				//change skip move bool to false and do not run any more code
				skipMove = false;

			//otherwise, if the boss is not yet casting
			} else if (!casting) {

				//save the taget's current location as the casting target
				castTarget = target.transform.position;

				//decide which spell to cast
				spellChoice = Random.Range (0, 2);

				//enable the proper casting animation dependent on which spell was chosen
				switch (spellChoice) {

					//case 0 is for ice shards
					case 0:
						
						//make the left tentacles of the beholder glow blue until he casts next turn
						transform.FindChild ("body").transform.FindChild("upperLeft").GetComponent<Animator>().SetTrigger ("Cast");

						break;

					//case 1 is for fireball
					case 1:

						//make the right tentacles of the beholder glow red until he casts next turn
						transform.FindChild("body").transform.FindChild("upperRight").GetComponent<Animator>().SetTrigger ("Cast");

						break;

					//default case does nothing
					default:

						break;
				}

				//acknowledge for next turn that the Boss is casting a spell
				casting = true;
				skipMove = true;

			//otherwise the boss can cast the spell
			} else {

				//cast the proper spell at the target location dependent on which one was chosen
				switch (spellChoice) {

					//case 0 is for ice shards
					case 0:

						//spawn ice shards at the target position and four other adjacent positions and set them to be destroyed after a delay
						Destroy(Instantiate (IceShards, castTarget, Quaternion.identity), 0.75f);
						Destroy(Instantiate (IceShards, new Vector3 (castTarget.x + 1, castTarget.y, castTarget.z), Quaternion.identity), 0.75f);
						Destroy(Instantiate (IceShards, new Vector3 (castTarget.x, castTarget.y + 1, castTarget.z), Quaternion.identity), 0.75f);
						Destroy(Instantiate (IceShards, new Vector3 (castTarget.x - 1, castTarget.y, castTarget.z), Quaternion.identity), 0.75f);
						Destroy(Instantiate (IceShards, new Vector3 (castTarget.x, castTarget.y - 1, castTarget.z), Quaternion.identity), 0.75f);

						//stop the left tentacles of the beholder from glowing blue
						transform.FindChild ("body").transform.FindChild("upperLeft").GetComponent<Animator>().SetTrigger ("Cast");

						break;

					//case 1 is for fireball
					case 1:

						//spawn a fireball at position, as well as four other adjacent positions, and set them to destroy after a delay
						Destroy(Instantiate (FireBall, castTarget, Quaternion.identity), 0.75f);
						Destroy(Instantiate (FireBall, new Vector3 (castTarget.x + 1, castTarget.y + 1, castTarget.z), Quaternion.identity), 0.75f);
						Destroy(Instantiate (FireBall, new Vector3 (castTarget.x + 1, castTarget.y - 1, castTarget.z), Quaternion.identity), 0.75f);
						Destroy(Instantiate (FireBall, new Vector3 (castTarget.x - 1, castTarget.y + 1, castTarget.z), Quaternion.identity), 0.75f);
						Destroy(Instantiate (FireBall, new Vector3 (castTarget.x - 1, castTarget.y - 1, castTarget.z), Quaternion.identity), 0.75f);

						//stop the right tentacles of the beholder from glowing red
						transform.FindChild ("body").transform.FindChild("upperRight").GetComponent<Animator>().SetTrigger ("Cast");

						break;

					//default case does nothing
					default:
						break;
				}

				//reset the cast bool
				casting = false;
				skipMove = true;
			}

		//otherwise, let the enemy attempt to track down the player and melee attack as normal
		} else {

			//check if the difference between the target and the enemy x coordinates are less than Epsilon (aka almost the same)
			if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {

				//if we are in the same column (player and enemy x coords are almost the same) move the enemy either up or down
				//depending on in which direction the player's position lies
				yDir = target.position.y > transform.position.y ? 1 : -1;

			//if the player and enemy are in the same row
			} else if (Mathf.Abs (target.position.y - transform.position.y) < float.Epsilon) {

				//move the enemy to the left or right depending on in which direction the player's position lies
				xDir = target.position.x > transform.position.x ? 1 : -1;

			//if the player and enemy are not in the same column or row
			} else {

				//get a random number (either 0 or 1) and store it in an int variable called axis
				int axis = Random.Range (0,2);

				//decide on either the y or x axis depending on which random number came up
				//0 is for x axis
				if (axis == 0) {

					//move the enemy to the left or right depending on in which direction the player's position lies
					xDir = target.position.x > transform.position.x ? 1 : -1;
				}

				//1 is for y axis
				if (axis == 1) {

					//move the enemy up or down depending on in which direction the player's position lies
					yDir = target.position.y > transform.position.y ? 1 : -1;
				}

			}

			//if the enemy attempts to move to the right
			if (xDir == 1) {
			
				//rotate the enemy sprite on the X axis
				gameObject.transform.rotation = new Quaternion (0, 90, 0, 0);
			}
		
			//if the enemy attempts to move to the left
			if (xDir == -1) {
			
				//set the enemy sprite to the default rotation
				gameObject.transform.rotation = new Quaternion (0, 0, 0, 0);
			}

			//now that we have the direction for the enemy to move, call the attmept move function, and pass in the player as the possible interacting object
			AttemptMove <Player> (xDir, yDir);
		}
	}


	//enemy specific inplementation of OnCantMove (enemy expected to interact with the player)
	protected override IEnumerator OnCantMove <T> (T component, Vector3 position) {

		//if the object that was hit is the player
		if (component.gameObject.tag == "Player") {

			//create a player object using the component that was passed into the OnCantMove function (cast as a Player)
			Player hitPlayer = component as Player;

			//if the enemy is a boos
			if(tag == "Boss") {

				animator.SetTrigger ("bossAttack");

			//otherwise...
			} else {

				//set the enemy attack trigger
				animator.SetTrigger ("enemyAttack");

			}

			//play a randomized version of one of the two enemy attack sounds
			SoundManager.instance.RandomizeSfx (enemyAttack1, enemyAttack2);

			//call the lose health function on our player object and pass in the amount of damage that the enemy does (minus the player's current armor bonus)
			hitPlayer.LoseHealth ((playerDamage - hitPlayer.damReduct()), ToString ().Substring (0, (ToString ().Length - 15)));
		}

		return null;
	}
}
