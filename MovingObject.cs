/*
 *  Title:       MovingObject.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/25/2015
 *  Modified:    02/15/2016
 *  Resources:   Adapted from original movingobject script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: Base class for moving objects in the game (player and enemies); handles raycast for object interactions, movement success/failure reporting, and animation time sync
 */

using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	//the time it will take an object to move in seconds
	public float moveTime = 0.15f;

	//the layer on which we check collision to determine if a space is open
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;

	//stores a component reference to the rigidbody 2d component of unit being moved
	private Rigidbody2D rb2d;

	//used to make movement calculations more efficient
	private float inverseMoveTime;

	//keep count of how many times the object has tried to move unsuccessfully
	private int moveTries = 0;

	protected virtual void Start () {
	
		//get component references to the 2D box collider and rigidbody
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();

		//storing the inverse of move time allows us to use * instead of / in our computations, which is more efficient computationally
		inverseMoveTime = 1 / moveTime;
	}
	
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit) {

		//store the current transform position
		Vector2 start = transform.position;

		//calculate the end position based on parameters
		Vector2 end = start + new Vector2 (xDir, yDir);

		//disable the box collider so that when we cast our ray we wont hit our own collider
		boxCollider.enabled = false;

		//cast a line from start to end, checking collisions on the blocking layer, store this value in hit
		hit = Physics2D.Linecast (start, end, blockingLayer);

		//reenable the box collider
		boxCollider.enabled = true;

		//see if anything was hit by the line we cast
		if (hit.transform == null) {

			//if nothing was hit then start the coroutine smooth movement and report true for a successful movement
			StartCoroutine (SmoothMovement (end));
			return true;
		}

		//if something was hit return false to report an unsuccessful move
		return false;
	}

	//co-routine that moves units smoothly from one space to the next, using the parameter end as the end point
	protected IEnumerator SmoothMovement (Vector3 end) {

		//determine the distance left to move; square magnitude is computationally cheaper than magnitude
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {

			//find a new position that is proportionally closer to the end position, based on move time
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, end, inverseMoveTime * Time.deltaTime);

			//move the unit to the new position
			rb2d.MovePosition (newPosition);

			//recalculate the remaining distance after the move
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			//wait for a frame before reevaluating the loop condition
			yield return null;
		}
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir)
		where T : Component
	{
		RaycastHit2D hit;

		//increment the move tries variable
		moveTries++;

		//call move function and store the return value (successful or unsuccessful move) to a new boolean called canMove
		bool canMove = Move (xDir, yDir, out hit);

		//if nothing was hit in the move attempt, then return now and do not run further code
		if (hit.transform == null) {

			//reset the move tries variable to zero
			moveTries = 0;

			return;
		}

		//if this is the 5th time the moving object has tried to move unsuccessfully
		if (moveTries > 4) {

			//reset themove tries variable to zero
			moveTries = 0;

			//return without running any more code
			return;

		//otherwise, the object has collided with an object it can either interact with, or will try to maneuver around
		} else {

			//get a component reference of type T attatched to the object that was hit
			T hitComponent = hit.transform.GetComponent<T> ();

			//if the moving object failed to move and is blocked by an object it can interact with
			if (!canMove && hitComponent != null) {

				//pass the hit component and the hit component's transform position as parameters into the OnCantMove function
				OnCantMove (hitComponent, hitComponent.transform.position);

			//otherwise, if the moving object failed to move and is blocked by an inner destructable wall or enemy object (this code should only affect enemies)
			//Note: this code is to improve the AI, allowing enemies to move around inner walls and more realistically track the player
			} else if (!canMove && (hit.collider.gameObject.tag == "destructWall" || hit.collider.gameObject.tag == "Enemy")) {

				//if the collided object is below the moving object (the moving object was moving down [-Y])
				if (hit.transform.position.y < transform.position.y) {

					//recursively call the arrempt move function for a move to the right
					AttemptMove<Player> (1, 0);

					//if the collided object is to the right of the moving object (the moving object was moving to the right [+X])
				} else if (hit.transform.position.x > transform.position.x) {

					//recursively call the attempt move function for a move upward
					AttemptMove<Player> (0, 1);
			
					//if the collided object is above the moving object (the moving object was moving upward [+Y]
				} else if (hit.transform.position.y > transform.position.y) {

					//recursively call the attempt move function for a move to the left
					AttemptMove<Player> (-1, 0);

					//if the collided object is to the left of the moving object (the moving object was moving to the left [-X])
				} else if (hit.transform.position.x < transform.position.x) {

					//recursively call the attempt move function for a move downward
					AttemptMove<Player> (0, -1);
				}
			}
		}
	}
	
	protected abstract IEnumerator OnCantMove <T> (T component, Vector3 position)
		where T : Component;
}
