﻿/*
 *  Title:       Damagable.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     11/19/2015
 *  Modified:    04/09/2016
 *  Resources:   Adapted from original Wall script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: Originally used to allow the player to attack walls, this script is now attached to all enemies, walls, and any other object the player can damage
 */

using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {

	//track the current hp of the object
	public int hp;
	private int totalHp;

	//variable that keeps track of the current phase (only used for bosses)
	private int phase = 0;

	//the xp value gained for killing the object
	public int expValue;

	//save a reference to the sprite for a damaged non-enemy object
	public Sprite dmgSprite;

	//saves a reference to the sprite renderer
	private SpriteRenderer spriteRenderer;

	//saves a reference to the animator of enemy objects
	private Animator animator;

	//a list of animator references for Boss enemies
	private Animator[] animators;

	//an audio clip for when an enemy dies
	public AudioClip enemyDeath;

	//a reference to the health bar game object for each enemy
	private GameObject healthBar;

	void Awake () {
	
		//set the enemy's total hp
		totalHp = hp;

		//if the object is a boss
		if (tag == "Boss") {

			//grab the animator component connected to the body child
			animators = GetComponentsInChildren<Animator> ();

			//save a reference to the health bar gameObject
			healthBar = transform.FindChild ("healthBar").gameObject;
			
			//set the color of the health bar
			healthBar.GetComponent<SpriteRenderer> ().color = new Color (0.0f, 1.0f, 0.0f);

		//if the object is a container
		} else if (tag == "Container") {
			
			//get the sprite renderer AND animator
			spriteRenderer = GetComponent<SpriteRenderer> ();
			animator = GetComponent<Animator> ();

			//disable the animator for the time being
			animator.enabled = false;

		//otherwise, if the object is not an enemy
		} else if (tag != "Enemy") {

			//for non enemies get a component reference of the sprite renderer
			spriteRenderer = GetComponent<SpriteRenderer> ();

		//otherwise
		} else {

			//for enemies get a compnent reference of the animator
			animator = GetComponent<Animator> ();

			//save a reference to the health bar gameObject
			healthBar = transform.FindChild ("healthBar").gameObject;
			
			//set the color of the health bar
			healthBar.GetComponent<SpriteRenderer>().color = new Color (0.0f, 1.0f, 0.0f);
		}
	}

	//a function that causes the object to take damage
	public void takeDamage(int damage) {

		//decrement the object's health points
		hp -= damage;

		//if the object is a boss
		if (tag == "Boss") {

			//loop through the animators list
			foreach(Animator animator in animators) {

				//and set all triggers to boss hit
				animator.SetTrigger("bossHit");
			}

			//set up the percent float
			float percent;

			//if the enemy is not dead
			if (hp > 0) {

				//get the percentage of enemy health remaining
				percent = (float)hp / (float)totalHp;

				//spawn enemy adds if the boss' health drops below certain thresholds
				if (percent < 0.26 && phase < 3) {

					GameManager.instance.boardScript.spawnAdds (6);
					phase++;

				} else if (percent < 0.51 && phase < 2) {

					GameManager.instance.boardScript.spawnAdds (5);
					phase++;

				} else if (percent < 0.76 && phase < 1) {

					GameManager.instance.boardScript.spawnAdds (2);
					phase++;
				}

			//otherwise
			} else {

				//set the percentage to zero
				percent = 0.0f;
			}
			
			//set the health bar x scale and color depending on how much hp the enemy has left
			healthBar.transform.localScale = new Vector2 (percent, 1.0f);
			
			//change the color of the health bar depending on how much hp the enemy has left
			if (percent < 0.33f) {
				
				healthBar.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.0f, 0.0f);
				
			} else if (percent < 0.66f) {
				
				healthBar.GetComponent<SpriteRenderer>().color = new Color (1.0f, 1.0f, 0.0f);
			}

		//otherwise, if the object is not an enemy
		} else if (tag != "Enemy") {

			//update the sprite to the damaged verion (if applicable)
			spriteRenderer.sprite = dmgSprite;

		//otherwise
		} else {

			//set the animator trigger for enemy hit
			animator.SetTrigger("enemyHit");

			//set up the percent float
			float percent;
			
			//if the enemy is not dead
			if (hp > 0) {
				
				//get the percentage of enemy health remaining
				percent = (float)hp / (float)totalHp;
				
				//otherwise
			} else {
				
				//set the percentage to zero
				percent = 0.0f;
			}
			
			//set the health bar x scale and color depending on how much hp the enemy has left
			healthBar.transform.localScale = new Vector2 (percent, 1.0f);
			
			//change the color of the health bar depending on how much hp the enemy has left
			if (percent < 0.33f) {
				
				healthBar.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.0f, 0.0f);
				
			} else if (percent < 0.66f) {
				
				healthBar.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 0.0f);
			}

		}

		//check to see if the object dies
		if (hp <= 0) {

			//if the object is not an enemy
			if (tag != "Enemy" && tag != "Boss" && tag != "Container") {

				//check to see if this is the last wall in the room
				if (GameManager.instance.GetComponent<BoardManager>().getCurrentWalls() != 1) {

					//decrement the current wall count
					GameManager.instance.GetComponent<BoardManager>().decrementCurrentWalls ();

					//deactivate the object
					gameObject.SetActive (false);

				} else {

					//if the player is in room 5
					if (GameManager.instance.GetRoom() == 5) {

						//spawn a crate object below the wall object
						GameObject tempBarrel = Instantiate (GameManager.instance.GetComponent<BoardManager>().healthTiles[7], transform.position, Quaternion.identity) as GameObject;

						//kill the temp barrel
						tempBarrel.GetComponent<Damagable>().killObject();

						//deactivate the wall
						gameObject.SetActive (false);

						//spawn a secret exit in the same position
						GameManager.instance.GetComponent<BoardManager>().openFloorExit (transform.position, true);

					//otherwise
					} else {

						//deactivate the object
						gameObject.SetActive (false);
					}
				}

			//otherwise, the object is an enemy
			} else {

				//start the coroutine that initiates the enemy death animation and deactivates the object after a delay
				StartCoroutine (EnemyDeath());
			}
		}
	}

	//a couroutine that activates the enemy death animation and deactivates the object after a delay
	IEnumerator EnemyDeath() {

		//if the enemy is a boss
		if (tag == "Boss") {

			//set the room over bool to true so that enemies wont attack the player after killing the beholder
			GameObject.FindWithTag("Player").GetComponent<Player>().roomOverSet(true);

			//loop through the animators list
			foreach(Animator animator in animators) {

				//and set all triggers to boss death
				animator.SetTrigger("bossDeath");
			}

			//set the game win boolean to true
			GameManager.instance.setGameWin(true);

			//stop the music from playing
			SoundManager.instance.musicSource.Stop ();

			//grab a temporary player script reference
			Player tempPlayer = GameObject.FindWithTag ("Player").GetComponent<Player>();

			//update the status display to tell the player they have won
			tempPlayer.statusUpdate ("You defeated the Beholder!!!", new Color (0.0f, 1.0f, 0.0f));

			//call the trigger animation function and pass in the trigger "Win"
			tempPlayer.triggerAnimation ("Win");

		} else {

			//make sure the animator is enabled
			animator.enabled = true;

			//initiate the enemy death animation
			animator.SetTrigger ("death");
		}

		//play the enemy death sound (currently the game over sound)
		SoundManager.instance.RandomizeSfx (enemyDeath);

		if (!GameManager.instance.getGameWin ()) {

			//wait for a delay to allow the animation to finish
			yield return new WaitForSeconds (1.5f);

		} else {

			//give the player a little extra time to enjoy his victory
			yield return new WaitForSeconds (3.0f);
		}

		//deactivate the enemy object
		gameObject.SetActive (false);

		//if the boss was just killed
		if (GameManager.instance.getGameWin ()) {

			//start the end game function
			GameManager.instance.GameOver ();
		}
	}

	//handle what happens when a trigger collider enters the damagable object
	private void OnTriggerEnter2D (Collider2D other) {

		//if the object is a fireball
		if (other.tag == "Fireball") {

			//have the object take damage
			takeDamage (GameObject.Find ("Player").GetComponent<Player>().getInt () * 10);
		}

		//if the object is an iceshard
		if (other.tag == "IceShards") {

			//have the object take damage
			takeDamage (GameObject.Find ("Player").GetComponent<Player>().getInt () * 5);
		}
	}

	//public function that allows other scripts to start the co-routine enemy death
	public void killObject() {

		//start the enemy death coroutine
		StartCoroutine (EnemyDeath ());
	}
}
