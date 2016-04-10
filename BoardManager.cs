/*
 *  Title:       BoardManager.cs
 *  Author:      Steve Ross-Byers (Matthew Schell)
 *  Created:     10/21/2015
 *  Modified:    04/09/2016
 *  Resources:   Adapted from original boardmanager script for 2D Roguelike Tutorial by Matthew Schell (Unity Technologies) using the Unity API
 *  Description: Used to set up each random procedurally generated room and populate the room with items, walls, and enemies; also sets up the unique "secret room" when required
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	//A class called count to keep track of the number of random objects in each room
	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count(int min, int max){
			minimum = min;
			maximum = max;
		}
	}

	//columns and rows of gameboard
	public int columns = 8;
	public int rows = 8;

	//counts for walls and health in each room (min, max)
	public Count wallCount = new Count (5,9);
	public Count healthCount = new Count (0,3);

	//tiles to exit the room or floor (depending on current level)
	public GameObject roomExit;
	public GameObject floorExit;

	//game objects for the locked exit and the key to open it
	public GameObject lockedFloorExit;
	public GameObject Key;

	//arrays of game objects to pass in the options for floor, wall, health, enemy, and outer wall tiles
	public GameObject[] floorTiles;
	public GameObject[] secretFloorTiles;
	public GameObject[] secretWallTiles;
	public GameObject[] wallTilesF1;
	public GameObject[] wallTilesF2;
	public GameObject[] wallTilesF3;
	public GameObject[] wallTilesF4;
	public GameObject[] wallTilesF5;
	public GameObject[] allWallTiles;
	public GameObject[] healthTiles;
	public GameObject[] floorDanger;
	public GameObject[] enemyTiles;
	public GameObject[] bossTiles;
	public GameObject[] finalBosses;
	public GameObject[] outerWallTilesF1;
	public GameObject[] outerWallTilesF2;
	public GameObject[] outerWallTilesF3;
	public GameObject[] outerWallTilesF4;
	public GameObject[] outerWallTilesF5;
	public GameObject Torch;
	public GameObject[] allOuterWallTiles;

	//boardHolder is for organization purposes, make all new objects children of boardHolder so they can be collapsed
	private Transform boardHolder;

	//a list of all possible positions on the game grid for random objects (health, enemies, inner walls, etc)
	private List<Vector3> gridPositions = new List<Vector3>();

	//the location for the exit tile to be instantiated
	private float exitPosX;
	private float exitPosY;

	//function that clears and sets up the gridpositions list so we can add new randomly placed objects
	void InitializeList() {

		//clear the list
		gridPositions.Clear ();

		//add a vector3 to the list for each spot on the game board
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3(x,y,0f));
			}
		}
	}

	//function that sets up the board
	void BoardSetup(int floor) {

		//if the player is in the secret room
		if (GameManager.instance.secretRoomBool ()) {

			//set specific rows and columns for the secret room
			rows = 5;
			columns = 8;

			//set inner wall and health item counts to zero
			wallCount.minimum = 0;
			wallCount.maximum = 0;
			healthCount.minimum = 0;
			healthCount.maximum = 0;

		} else {

			//set the size of each room
			//rows = 8;
			//columns = 8;
			//Note: Trying some randomization based on floor level.  Column count is random dependent on floor number
			rows = 8;
			columns = Random.Range ((4 + floor), (8 + floor));

			//try to limit max column size
			//if column count is greater than 15
			if (columns > 15) {

				//set columns to 15
				columns = 15;
			}

			//set the inner walls depending on the newly formed column count
			wallCount.minimum = columns;
			wallCount.maximum = (columns + 3);
			healthCount.minimum = 0;
			healthCount.maximum = (columns - 2);
		}

		//we need to set the camera up for larger room sizes, so grab a reference to the camera game object
		GameObject mainCam = GameObject.Find ("Main Camera"); 

		//and set the camera's x axis position to half of the number of columns in the room
		mainCam.transform.position = new Vector3 ((columns / 2.0f), mainCam.transform.position.y, mainCam.transform.position.z);

		//if the player is in the secret room
		if (GameManager.instance.secretRoomBool ()) {

			//move the camera down on the y axis so the room is better centered
			mainCam.transform.position = new Vector3 (mainCam.transform.position.x, 2.0f, mainCam.transform.position.z);
		}

		//set boardHolder to a new object called board
		boardHolder = new GameObject ("Board").transform;

		//nested for loops to interact with each spot on the game board (X then Y)
		for (int x = - 1; x < columns + 1; x++) {
			for (int y =  -1; y < rows + 1; y++) {

				//set up the toInstantiate GameObject
				GameObject toInstantiate;

				//if the player is accessing the secret room
				if (GameManager.instance.secretRoomBool()) {

					//grab a random floor tile from the secret floor tiles array to instantiate at the current location
					toInstantiate = secretFloorTiles [Random.Range (0, secretFloorTiles.Length)];

				} else {

					//set a random floor tile to instantiate at the current grid position in our loop
					toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

				}
				//if we are outside the playable area of our game grid, set a random outer wall tile to instantiate instead
				if (x == -1 || x == columns || y == - 1 || y == rows) {

					//if we are at a corner of the room
					if ((x == -1 && y == -1) || (x == -1 && y == rows) || (x == columns && y == rows) || (x == columns && y == -1)) {

						//instantitate the torch tile
						toInstantiate = Torch;

					//otherwise, if the player is accessing the secret room
					}else if (GameManager.instance.secretRoomBool ()) {

						//if the current location is where we want the exit tile to be
						if (x == columns && y == 2) {

							//set a floor tile to be instantiated
							toInstantiate = secretFloorTiles [Random.Range (0, secretFloorTiles.Length)];

							//also, instantiate the tiles that make up the hallway leading to the exit
							Instantiate (secretFloorTiles[floor - 1], new Vector3 (columns + 1.0f, 2.0f), Quaternion.identity);
							Instantiate (secretFloorTiles[floor - 1], new Vector3 (columns + 2.0f, 2.0f), Quaternion.identity);
							Instantiate (roomExit, new Vector3 (columns + 3.0f, 2.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 4.0f, 2.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 4.0f, 3.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 4.0f, 1.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 3.0f, 3.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 3.0f, 1.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 2.0f, 3.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 2.0f, 1.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 1.0f, 3.0f), Quaternion.identity);
							Instantiate (secretWallTiles[floor - 1], new Vector3 (columns + 1.0f, 1.0f), Quaternion.identity);

							//instantiate two container objects, one above and one below the kindly man's spot
							Instantiate (healthTiles[Random.Range (6, healthTiles.Length)], new Vector3 (0.0f, 0.0f), Quaternion.identity);
							Instantiate (healthTiles[Random.Range (6, healthTiles.Length)], new Vector3 (0.0f, 4.0f), Quaternion.identity);

							//finally instantiate the kindly man at his proper spot in the room
							Instantiate (secretWallTiles[9], new Vector3 (0.0f, 2.0f), Quaternion.identity);

						//otherwise...
						} else {

							//instantiate a tile from the secret room wall tiles array
							toInstantiate = secretWallTiles [floor - 1];
						}

					//otherwise, if the floor is number 1
					} else if (floor == 1) {

						//instantiate tile from the F1 outer wall tiles array
						toInstantiate = outerWallTilesF1 [Random.Range (0, outerWallTilesF1.Length)];

					//if the floor is number 2
					} else if (floor == 2) {

						//get a random number between 0 and 1 to decide which array we will take a tile from
						switch (Random.Range (0, 2)) {

							//case 0 takes a tile from the F1 array
							case 0:
								toInstantiate = outerWallTilesF1 [Random.Range (0, outerWallTilesF1.Length)];
								break;

							//case 1 takes a tile from the F2 array
							case 1:
								toInstantiate = outerWallTilesF2 [Random.Range (0, outerWallTilesF2.Length)];
								break;
						}

					//if the floor is number 2
					} else if (floor == 3) {

						//instantiate tile from the F2 outer wall tiles array
						toInstantiate = outerWallTilesF2 [Random.Range (0, outerWallTilesF2.Length)];


					//if the floor is number 4
					} else if (floor == 4) {
						
						//get a random number between 0 and 1 to decide which array we will take a tile from
						switch (Random.Range (0, 2)) {
							
							//case 0 takes a tile from the F2 array
							case 0:
								toInstantiate = outerWallTilesF2 [Random.Range (0, outerWallTilesF2.Length)];
								break;
							
							//case 1 takes a tile from the F3 array
							case 1:
								toInstantiate = outerWallTilesF3 [Random.Range (0, outerWallTilesF3.Length)];
								break;
						}

					//if the floor is number 5
					} else if (floor == 5) {

						//instantiate tile from the F3 outer wall tiles array
						toInstantiate = outerWallTilesF3 [Random.Range (0, outerWallTilesF3.Length)];

					//if the floor is number 6
					} else if (floor == 6) {
						
						//get a random number between 0 and 1 to decide which array we will take a tile from
						switch (Random.Range (0, 2)) {
							
							//case 0 takes a tile from the F3 array
							case 0:
								toInstantiate = outerWallTilesF3 [Random.Range (0, outerWallTilesF3.Length)];
								break;
							
							//case 1 takes a tile from the F4 array
						case 1:
							toInstantiate = outerWallTilesF4 [Random.Range (0, outerWallTilesF4.Length)];
							break;
						}

					//if the floor number is 7
					} else if (floor == 7) {

						//instantiate tile from the F4 outer wall tiles array
						toInstantiate = outerWallTilesF4 [Random.Range (0, outerWallTilesF4.Length)];

					//if the floor is number 8
					} else if (floor == 8) {
						
						//get a random number between 0 and 1 to decide which array we will take a tile from
						switch (Random.Range (0, 2)) {
							
							//case 0 takes a tile from the F4 array
						case 0:
							toInstantiate = outerWallTilesF4 [Random.Range (0, outerWallTilesF4.Length)];
							break;
							
							//case 1 takes a tile from the F5 array
						case 1:
							toInstantiate = outerWallTilesF5 [Random.Range (0, outerWallTilesF5.Length)];
							break;
						}

					//if the floor number is 9
					} else if (floor == 9) {
					
						//instantiate tile from the F5 outer wall tiles array
						toInstantiate = outerWallTilesF5 [Random.Range (0, outerWallTilesF5.Length)];

					//if the floor number has outpaced the number of outer wall tile arrays
					} else {

						//instantiate a tile from the super outer wall tile array
						toInstantiate = allOuterWallTiles [Random.Range (0, allOuterWallTiles.Length)];
					}

				}

				//Instantiate the chosen floor or outer wall tile as a game object at the current loop position and set boardHolder as parent
				//NOTE: Quaternion.identity simply keeps the tile from being rotated
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	//function that selects a random index from the list of grid positions for random item placement
	Vector3 RandomPosition() {

		//create a new random integer with a range between zero and the number of grid positions left open
		int randomIndex = Random.Range(0, gridPositions.Count);

		//use the random integer to index a position on the list of grid positions
		Vector3 randomPosition = gridPositions[randomIndex];

		//remove this index from the list so it cannot be used for another random item
		gridPositions.RemoveAt(randomIndex);

		//return the chosen random position on the game grid
		return randomPosition;
	}

	//function that lays out a random object from an array of game objects at a random position chosen by the RandomPosition function
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {

		//randomly set the number of objects in the room within a range of the minimum and maximum parameters
		int objectCount = Random.Range (minimum, maximum);

		//for loop that keeps adding random objects at random positions until it reaches the object count from the previous step
		for (int i = 0; i < objectCount; i++) {

			//store a random position on the game grid
			Vector3 randomPosition = RandomPosition();

			//choose a random tile from the sprite array parameter
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

			//instantiate the random object at the random position
			//NOTE: Quaternion.identity simply keeps the tile from being rotated
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	//the single public function of this class, called by the game manager, to set up the current room
	public void SetupScene(int room, int floor) {

		//call our board setup function, which will lay out the floor and outer wall tiles
		BoardSetup (floor);

		//initialize our list of grid positions
		InitializeList ();

		//decide which array of wall tiles to use (dependent on floor number)
		//if the player is on floor 1
		if (floor == 1) {

			//lay out the inner wall tiles from the F1 array at random
			LayoutObjectAtRandom (wallTilesF1, wallCount.minimum, wallCount.maximum);
		
		//if the player is on floor 2
		}else if (floor == 2) {

			//if the player has not passed room 5
			if (room < 6) {

				//lay out the previous floor's walls
				LayoutObjectAtRandom (wallTilesF1, wallCount.minimum, wallCount.maximum);

			//otherwise, we are on room 6 or greater
			} else {

				//lay out the next floor's walls
				LayoutObjectAtRandom (wallTilesF2, wallCount.minimum, wallCount.maximum);
			}

		//if the player is on floor 3
		} else if (floor == 3) {

			//lay out the inner wall tiles from the F2 array at random
			LayoutObjectAtRandom (wallTilesF2, wallCount.minimum, wallCount.maximum);

		//if the player is on floor 4
		}else if (floor == 4) {
			
			//if the player has not passed room 5
			if (room < 6) {
				
				//lay out the previous floor's walls
				LayoutObjectAtRandom (wallTilesF2, wallCount.minimum, wallCount.maximum);
				
			//otherwise, we are on room 6 or greater
			} else {
				
				//lay out the next floor's walls
				LayoutObjectAtRandom (wallTilesF3, wallCount.minimum, wallCount.maximum);
			}

		//if the player is on floor 5
		} else if (floor == 5) {

			//lay out the inner wall tiles from the F3 array at random
			LayoutObjectAtRandom (wallTilesF3, wallCount.minimum, wallCount.maximum);

		//if the player is on floor 6
		}else if (floor == 6) {
			
			//if the player has not passed room 5
			if (room < 6) {
				
				//lay out the previous floor's walls
				LayoutObjectAtRandom (wallTilesF3, wallCount.minimum, wallCount.maximum);
				
			//otherwise, we are on room 6 or greater
			} else {
				
				//lay out the next floor's walls
				LayoutObjectAtRandom (wallTilesF4, wallCount.minimum, wallCount.maximum);
			}

		//if the player is on floor 7
		} else if (floor == 7) {

			//lay out the inner wall tiles from the F4 array at random
			LayoutObjectAtRandom (wallTilesF4, wallCount.minimum, wallCount.maximum);

			//if the player is on floor 8
		}else if (floor == 8) {
			
			//if the player has not passed room 5
			if (room < 6) {
				
				//lay out the previous floor's walls
				LayoutObjectAtRandom (wallTilesF4, wallCount.minimum, wallCount.maximum);
				
				//otherwise, we are on room 6 or greater
			} else {
				
				//lay out the next floor's walls
				LayoutObjectAtRandom (wallTilesF5, wallCount.minimum, wallCount.maximum);
			}

		//if the player is on floor 9
		} else if (floor == 9) {
		
			//lay out the inner wall tiles from the F5 array at Random
			LayoutObjectAtRandom (wallTilesF5, wallCount.minimum, wallCount.maximum);

		//if the floor level outpaces the number of wall tile arrays, use a super array containing all of the wall tile arrays
		} else {

			//lay out the inner wall tiles form the wall tile super array at random
			LayoutObjectAtRandom (allWallTiles, wallCount.minimum, wallCount.maximum);
		}

		//lay out the random objects at random
		LayoutObjectAtRandom (healthTiles, healthCount.minimum, healthCount.maximum);

		//set the number of enemies for the room
		int enemyCount = (int)Mathf.Log (room, 2f);

		//declare the current enemy and boss arrays
		GameObject[] currentEnemies;
		GameObject[] currentBoss;

		//if the player is in the last room of the last floor of the game
		if (floor == 9 && room == 10) {
			
			//reset the enemy count to 0
			enemyCount = 0;
		}

		//check to make sure the floor level has not outpaced the enemy tiles array
		if (floor > enemyTiles.Length) {

			//if it has, make a sub array that contains only one enemy
			currentEnemies = new GameObject[1];

			//this array will contain only the most powerful enemy in the enemy tiles array
			currentEnemies[0] = enemyTiles[enemyTiles.Length -1];

		//otherwise, if the floor level is 1 or 2
		} else if (floor < 3) {

			//make a sub-array from the enemies array that is appropirate for floors 1 and 2
			currentEnemies = new GameObject[floor];

			//this sub-array contains enemy objects equal to the current floor, starting with the first enemy in the array
			Array.ConstrainedCopy (enemyTiles, 0, currentEnemies, 0, floor);

		//otherwise, the floor level is at least 3
		} else {

			//make a sub-array from the enemies array that is appropriate for all floors greater than 2 and below total enemy types
			currentEnemies = new GameObject[2];

			//this sub-array contains two enemy objects, the enemy from the previous floor and the current floor's enemy
			Array.ConstrainedCopy (enemyTiles, (floor - 2), currentEnemies, 0, 2);
		}

		//if the player is in the last room of the last floor of the game
		if (floor == 9 && room == 10) {

			//set the current boss array to contain the final boss of the game
			currentBoss = new GameObject[1];
			currentBoss[0] = finalBosses[0];

		//make sure the floor level hasnt outpaced the boss tile array
		} else if (floor > bossTiles.Length) {

			//if it has, set the current boss array to contain the most powerful boss in the boss tiles array
			currentBoss = new GameObject[1];
			currentBoss[0] = bossTiles[bossTiles.Length - 1];

		//otherwise
		} else {

			//make a sub-array from the boss enemies array that holds only the boss for this floor
			currentBoss = new GameObject[1];
			currentBoss[0] = bossTiles[floor - 1];
		}

		//if the player is trying to access the secret room
		if (GameManager.instance.secretRoomBool ()) {

			//do not instantiate enemies or bosses

		//if the player is in the last room of the game
		} else if (floor == 9 && room == 10) {

			//spawn the specified number of enemies and one final Boss
			LayoutObjectAtRandom (currentEnemies, enemyCount, enemyCount);
			Instantiate (currentBoss[0], new Vector3(7.0f, 7.0f), Quaternion.identity);

		//if the room ends in 5 or 0
		} else if (room > 0 && room % 5 == 0) {

			//if the room ends in 0
			if (room > 0 && room % 10 == 0) {

				//spawn two less enemies and add two bosses
				LayoutObjectAtRandom (currentEnemies, ((enemyCount + (floor - 1)) - 2), ((enemyCount + (floor - 1)) - 2));
				LayoutObjectAtRandom (currentBoss, 2, 2);

			//otherwise, room must end in 5
			} else {

				//spawn one less enemy and add a boss
				LayoutObjectAtRandom (currentEnemies, ((enemyCount + (floor - 1)) - 1), ((enemyCount + (floor - 1)) - 1));
				LayoutObjectAtRandom (currentBoss, 1, 1);
			}

		//if the room does not end in 5 or 0
		} else {

			//spawn the required number of enemies
			LayoutObjectAtRandom (currentEnemies, (enemyCount + (floor - 1)), (enemyCount + (floor - 1)));

		}

		//spawn floor danger tiles
		//if the player is accessing the secret room
		if (GameManager.instance.secretRoomBool ()) {

			//do not add floor danger tiles

		//starting on floor 9
		} else if (floor > 8) {

			//randomly spawn ice and spike tiles based on room number
			LayoutObjectAtRandom (floorDanger, 0, room);

		//starting on floor 6
		} else if (floor > 5) {

			//create a new array containing only the ice tiles
			GameObject[] floorIce = {floorDanger[1]};

			//randomly spawn ice tiles based on room number
			LayoutObjectAtRandom (floorIce, 0, room);

		//starting on floor 3
		} else if (floor > 2) {

			//create a new array containing only the spike tiles
			GameObject[] floorSpikes = {floorDanger[0]};

			//randomly spawn spike tiles based on room number
			LayoutObjectAtRandom (floorSpikes, 0, room);
		}

		//if the player is in the secret room
		if (GameManager.instance.secretRoomBool ()) {

			//spawn the player at the correct location
			GameObject.Find ("Player").transform.position = new Vector3 (columns - 1.0f, 2f);

			//rotate the player sprite 90 on the X axis
			GameObject.Find ("Player").transform.rotation = new Quaternion (0, 90, 0, 0);

		//if the player is on the last floor of the last room, he has to spawn in the correct location
		} else if (floor == 9 && room == 10) {

			//spawn the player at the bottom middle of the room
			GameObject.Find ("Player").transform.position = new Vector3 (7.0f, 0.0f);

		//trying to change the position of the player to match the position of the exit in the previous room
		//if we are in the first room of the floor
		} else if (room == 1) {

			//we want to set the player's position to 0, 0, 0
			GameObject.Find ("Player").transform.position = new Vector3 (0.0f, 0.0f, 0.0f);

			//set up the exit tile in a random position opposite the player
			//get a random number, either 0 or 1, and if the number is 0
			if (Random.Range (0, 2) == 0) {
			
				//get another random number, either 0 or 1, and if the number is 0
				if (Random.Range (0, 2) == 0) {
			
					//set the exit tile's x coordinate to 0
					exitPosX = 0.0f;
			
					//set the exit tile's y coordinate to rows - 1
					exitPosY = rows - 1;
			
				//otherwise the number is 1
				} else {
			
					//set the exit tile's x coordinate to columns - 1
					exitPosX = columns - 1;
			
					//next, set the exit tile's y coordinate to a random number between 0 and rows - 1
					exitPosY = Random.Range (0, rows);
				}
			
			//otherwise the number is 1
			} else {
			
				//get another random number, either 0 or 1, and if the number is 0
				if (Random.Range (0, 2) == 0) {
			
					//set the exit tile's y coordinate to 0
					exitPosY = 0.0f;
			
					//set the exit tile's x coordinate to columns - 1
					exitPosX = columns - 1;
			
				//otherwise, the number is 1
				} else {
			
					//set the exit tiles y coordinate to rows - 1
					exitPosY = rows - 1;
			
					//next, set the exit tile's x coordinate to a random number between 0 and columns -1
					exitPosX = Random.Range (0, columns);
				}
			}

		//otherwise, we need to spawn the player at the player's final position from the previous room
		} else {

			//set up a couple of temporary variables
			int playerX = (int)GameManager.instance.PlayerPos().x;
			int playerY = (int)GameManager.instance.PlayerPos().y;

			//if the x coordinate is greater than the number of columns for the current room
			if (playerX > (columns - 1)) {

				//set the x coordinate to columns - 1
				playerX = columns - 1;
			}

			//set the player's new starting position
			GameObject.Find ("Player").transform.position = new Vector3 (playerX, playerY, 0);

			//set up the exit tile in a random position opposite the player
			//if the player spawned in the first column
			if (playerX == 0) {

				//set the exit tile x coordinate equal to columns - 1
				exitPosX = columns - 1;

			//otherwise, if the player spawned in the last column
			} else if (playerX == (columns - 1)) {

				//set the exit tile x coordinate to 0
				exitPosX = 0;

			//otherwise...
			} else {

				//set the exit tile x coordinate to a random column
				exitPosX = Random.Range (0, columns);
			}

			//if the player spawned in the first row
			if (playerY == 0) {

				//set the exit tile y coordinate equal to rows - 1
				exitPosY = rows - 1;

			//otherwise, if the player spawned in the last row
			} else if (playerY == (rows - 1)) {

				//set the exit tile y coordinate to 0
				exitPosY = 0;

			//otherwise...
			} else {

				//set the exit tile y coordinate to a random row
				exitPosY = Random.Range (0, rows);
			}
		}

		//trying to set up a random position for the exit tile opposite the player's starting position (commented out for new player spawn logic)
		//get a random number, either 0 or 1, and if the number is 0
		//if (Random.Range (0, 2) == 0) {

			//get another random number, either 0 or 1, and if the number is 0
			//if (Random.Range (0, 2) == 0) {

				//set the exit tile's x coordinate to 0
				//exitPosX = 0.0f;

				//set the exit tile's y coordinate to rows - 1
				//exitPosY = rows - 1;

			//otherwise the number is 1
			//} else {

				//set the exit tile's x coordinate to columns - 1
				//exitPosX = columns - 1;

				//next, set the exit tile's y coordinate to a random number between 0 and rows - 1
				//exitPosY = Random.Range (0, rows);
			//}

		//otherwise the number is 1
		//} else {

			//get another random number, either 0 or 1, and if the number is 0
			//if (Random.Range (0, 2) == 0) {

				//set the exit tile's y coordinate to 0
				//exitPosY = 0.0f;

				//set the exit tile's x coordinate to columns - 1
				//exitPosX = columns - 1;

			//otherwise, the number is 1
			//} else {

				//set the exit tiles y coordinate to rows - 1
				//exitPosY = rows - 1;

				//next, set the exit tile's x coordinate to a random number between 0 and columns -1
				//exitPosX = Random.Range (0, columns);
			//}
		//}

		//set up the room exit position based on where the player spawned


		//instantiate the appropriate exit tile
		//if the player is on the last room of the game
		if ((GameManager.instance.secretRoomBool ()) || (floor == 9 && room == 10)) {

			//do not spawn an exit tile

		//otherwise, decide if the current room number is 10
		} else if (room > 0 && room % 10 == 0) {

			//and set the locked floor exit tile on top
			Instantiate (lockedFloorExit, new Vector3 (exitPosX, exitPosY, 0f), Quaternion.identity);

		//otherwise
		} else {

			//use the room exit tile
			Instantiate (roomExit, new Vector3 (exitPosX, exitPosY, 0f), Quaternion.identity);
		}
	}

	//function that instantiates the floor exit tile (called by the player when the key is found)
	public void openFloorExit(Vector3 position) {

		//instantiate the floor exit tile at the given position
		Instantiate (floorExit, position, Quaternion.identity);

	}

	//function that instantiates the key object (called by the game manager when the last enemy is defeated)
	public void spawnKey () {

		//instantiate the key object near the center of the room
		Instantiate (Key, new Vector3 (columns/2, rows/2, 0f), Quaternion.identity);
	}
}