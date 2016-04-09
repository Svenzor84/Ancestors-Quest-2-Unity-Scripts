/*
 *  Title:       Damagable.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     11/19/2015
 *  Modified:    04/03/2016
 *  Resources:   Adapted from original Wall script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: Originally used to allow the player to attack walls, this script is now attached to all enemies, walls, and any other object the player can damage
 */

using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {

	//track the current hp of the object
	public int hp;
	private int totalHp;

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
			healthBar.GetComponent<SpriteRenderer>().color = new Color (0.0f, 1.0f, 0.0f);

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

			//get the percentage of enemy health remaining
			float percent = (float)hp / (float)totalHp;
			
			//set the health bar x scale and color depending on how much hp the enemy has left
			healthBar.transform.localScale = new Vector2 (percent, 1.0f);
			
			//change the color of the health bar depending on how much hp the enemy has left
			if (percent < 0.66f) {
				
				healthBar.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 0.0f);
				
			} else if (percent < 0.33f) {
				
				healthBar.GetComponent<SpriteRenderer>().color = new Color (1.0f, 0.0f, 0.0f);
			}

		//otherwise, if the object is not an enemy
		} else if (tag != "Enemy") {

			//update the sprite to the damaged verion (if applicable)
			spriteRenderer.sprite = dmgSprite;

		//otherwise
		} else {

			//set the animator trigger for enemy hit
			animator.SetTrigger("enemyHit");

			//get the percentage of enemy health remaining
			float percent = (float)hp / (float)totalHp;
			
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
			if (tag != "Enemy" && tag!= "Boss") {

				//deactivate the object
				gameObject.SetActive (false);

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

			//loop through the animators list
			foreach(Animator animator in animators) {
				
				//and set all triggers to boss death
				animator.SetTrigger("bossDeath");
			}

		} else {

			//initiate the enemy death animation
			animator.SetTrigger ("death");
		}

		//wait for a delay to allow the animation to finish
		yield return new WaitForSeconds (1.5f);

		//play the enemy death sound (currently the game over sound)
		SoundManager.instance.RandomizeSfx (enemyDeath);

		//deactivate the enemy object
		gameObject.SetActive (false);
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
}
