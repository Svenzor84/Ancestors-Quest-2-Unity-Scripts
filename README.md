 https://drive.google.com/drive/folders/0B-LOwNKFbxZKZ0hscXBGXzRCc3c
 
-# -Release Notes-
+# Release Notes
 
 ***Minor Update***
 GAMEPLAY ADDITIONS/ENHANCEMENTS IN V1.2.2
-~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 
 -Rebalanced hit points and damage for Flayers (90hp 65dam -> 55hp 55dam) and Displacer Beasts (150hp 100dam -> 70hp 70dam) due to their increased speed
+
 -Added visual reference to potion quantity and associated hotkey to main HUD at top right (NOT a button)
+
 -Added status update for when the player tries to heal when at full health
+
 -Added a new Panel to the Tutorial section that explains the available hotkeys (tab, escape, 1, 2, 3)
 
 
 ***Minor Update**
 GAMEPLAY ADDITIONS IN V1.2.1
-~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 
 -Final boss battle (Beholder) now includes phases where increasingly difficult mini bosses are teleported into the room
 [defeating the Beholder itself is still the only requirement to completion of the final room]
@@ -60,66 +61,110 @@ BUGS/GLITCHES FOUND IN V1.2 (FIXED IN V1.2.1)
 ***Modifications leading toward Ancestor's Quest 2 Beta V1.2 Release***
 
 -Armor now reduces damage (from enemy attacks only) instead of granting a max hp bonus (armor bonus was reduced, as well [5->10->15->20])
+
 -Equipment chests now have a 50/50 chance to drop either a weapon or an armor set first
   [if the chosen item has already dropped this floor the other item can drop; if both items have already dropped potions drop instead]
+
 -the exit on floor 10 is now locked until the player collects the key item [key spawns in the middle of the room when all enemies have been killed]
+
 -items are no longer hidden if they land on a wall tile [item Layer Order is 1]
+
 -added four new weapons [Staff, Mace, Flail, and War Hammer] and mixed them into the weapon drop sequence
+
 -added four new armor sets [Padded, Hide, Scale, and Chain] and mixed them into the weapon drop sequence
+
 -fixed idle weapon animations [sync with player idle animation]
+
 -fixed armor attack sprite alignment [moved attack animation sprite editor boxes to the left one pixel]
+
 -reduced armor bonus further to match weapon damage bonus [1->3->6->10->15->21->28->36]
+
 -enemies now have a specific death sound
+
 -introduced a delay when enemies die that prevents the player from immediately occupying the enemy's now vacant square
+
 -items now drop directly where the enemy was standing when it died
+
 -enemies now have specific death animations (splatter)
+
 -added player/equipment death animations and delay before end of game summary display
+
 -player now spawns at the position of the previous room's exit and exit always spawns opposite the player (player always spawns at origin in room 1)
+
 -health potion drops now upgrade to higher tiers when the player's maxHp reaches certain levels [Health+ potions at maxHP 100, Health++ potions at maxHP 150]
+
 -bug fixed that allowed the player to continue attacking a dying enemy for extra xp and item drops [player can still continuously attack, but items and xp only apply once]
+
+
 -implemented new class of Equipment: Special; Player can only have one item of this type at a time, it is the highest tier drop (findCheck > 124 required)
+
 -added new controls: Double Click, works with Special Equipment [currently]
+
 -added the first Special Equipment Item: Fire Orb; player double clicks to cast a ranged area-effect fireball [deals (int * 10) damage, requires full turn]
+
 -added the second Special Equipment Item: Ice Orb; player double clicks to cast ranged area-effect ice shards [deals (int * 5) damage, requires half turn]
+
 -added the third Special Equipment Item: Robes; player double clicks to cast teleport, instantly moves the player to the desired space [requires full turn]
+
 -added the fourth Special Equipment Item: Cloak; player is granted an extra move per turn (for a total of 3 moves)
+
 -inner wall hp now increases in tiers (each successive color has greater hp [green:4-8-12-16-20:black]
+
 -equipping or unequipping armor now takes a move (half a normal turn) [this is more realistic and also corrects potential abuse of the Cloak]
+
 -added new controls: Keyboard Macros are as follows
  [Tab] - opens and closes the status panels
  [Esc] - opens and closes the options panel
  [1] - uses a health potion
  [2] - uses a strength potion
  [3] - uses an experience potion
+
 -added a Beard level progression indicator; beard grows when the player gains sufficient levels [5->10->15->20]
+
 -added final enemy/miniboss, Displacer Beast [first double wide sprite]
+
 -added more details to opening scene; Map leading the Descendant to his ancestral tomb and a banner displaying the Descendant's ancestral Sigil)
+
 -Beholder final boss created, spawns on 9x10 and has normal enemy AI but also casts Fireball or Ice Shards (same as orb versions, randomly chosen) when player is 2 spaces away 
+
 -magic objects can now deal damage to the player as well as enemies and inner walls [Fireball (150 hp) and Ice Shards (100 hp); Player's intelligence * 10 is subtracted for magic defense]
+
 -Flayers and Displacers are now as quick as the player [Cloak is not affected]
+
 -added persistant ground dangers: floor 3 (spikes - 50 hp), floor 6 (ice - causes slow), and floor 9 (ice and spikes) [random density potential increases with room number]
+
 -added help button/panel for special item explanation (explains special items in general and gives specific info for the player's current special item)
+
 -added a timer that keeps track of the time spent for each run and is incorporated into the game summary display screen
+
 -added health bars for all enemies, minibosses, and bosses [bars change length and color to indicate enemy hp]
+
 -fixed flickering line glitch at lower resolutions by creating a material with a default sprite shader, checking the pixel snap checkbox, and using the material to render sprites]
+
 -added new object Barrels (tagged Container); spawn randomly from "health items" array, and drop appropriate health items with a 50% drop rate (no find check)
+
 -added new objects Crates and Crate & Barrel (tagged Container); act exactly like Barrels object, but with different sprites and death animations
+
 -added secret room with kindly man; accessible via secret exit in room 5 of each floor (destroy all inner walls to uncover exit)
+
 -added npc kindly man; found in the secret room, he offers to shave your beard or tell you about your special equipment, and offers a trade if you want to get rid of your special item.
+
 -added changes to game over when the player kills the final boss (Beholder) including animations, status display, quest summary, and a continue button that allows the player to keep going.
+
 -fixed options panel bug where song text would be incorrect
+
 -added original music scores by Tony Lux; 
  -Retired Sheriff's Theme used for Game Over screen
  -All other tracks were added to the music cycle in the options panel ([ESC] or click/tap the Gear icon)
    -Pyramid
    -Intense Suspence
    -Morbid Flight of the Pirhana Fish Heads
    -Warlock
+
 -fixed player and exit spawn bug that caused one or both to spawn inside the inner play area [added a check to the player position logic]
 
 ***Minor Update***
 BUGS/GLITCHES FOUND IN V1.1 (FIXED IN V1.1.1)
-~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 
 -temporary strength buff caused permanent bonus if active when the player exits the current room
 [all temporary strength points are completely leaked when the player exits a room]
@@ -142,94 +187,171 @@ BUGS/GLITCHES FOUND IN V1.1 (FIXED IN V1.1.1)
 ***Modifications leading toward Ancestor's Quest 2 Beta v1.1 Release***
 
 -added floor 3 enemy Jelly and boss Slime
+
 -restructured enemy progression so that bosses spawn as normal enemies on later floors (floor 4 Ghoul Elder, floor 5 Orc Chieftan, and floor 6 Slime)
+
 -added floor 4 boss (and floor 7 enemy) Flayer
+
 -changed enemy progression logic so that when the floor number outpaces the number of distinct enemy/boss types, only the most powerful enemy/boss will spawn
+
 -fixed xp potion glitch (potions could not be picked up)
+
 -modified the main menu scene sprite to have more scenery (added tree and title embelishments)
+
 -modified item drop logic so that find skill is a part of the random number to be generated instead of added on (maintains the potential for getting tier 1 drops with high find skill)
+
 -resized the Status Text UI element and added background panel so it is much more visible
+
 -added more input for the Status Text UI element, including Experience Gained, Health Gained, and Damage Taken (with Source Name)
+
 -reorganized entire HUD to be more user friendly and central (options button moved to margin, status button down near xp and hp, and status UI text alone at the top)
+
 -added checks for item drops so that items cannot drop outside the play area, where they cannot be picked up
+
 -created new drop Equipment chest that combines weapon and armor chests.  The first chest of a floor will drop a weapon, the second a set of armor, and 2x health potions thereafter
   (Only one armor set/weapon can drop in each floor, and unlimited health potions; the knife in the player's starting inventory counts as the weapon drop for floor 1)
+
 -changed drop order to give more access to potions at each item tier [health potion->strength potion->Equipment Chest->xp potion]
+
 -Int sub-stat is now XpMOD (changed from XpCOST).  It now gives a 1% (0.01 added multiplier) bonus to xp gained (from any source) for each point in Intelligence beyond 1
+
 -added blue "Gained # temporary Strength!" to status UI text
+
 -strength potions now leak one point of strength per turn, instead of losing all of your temporary strength at once (makes strength potions more useful when using more than one)
+
 -added buff text and buff mark UI text elements to display any buff the player currently possesses (only buff at this point is temporary strength)
+
 -implemented Turn UI text element that displays if it is the player's turn (Go![Green]) or an enemy's turn (EnemyName[Red])
+
 -added AI logic to moving object script that allows enemies to move around inner walls/eachother, and slightly more random axis choice when not in the same column/row as the player
+
 -changed enemy AI logic so that all enemies start on their skip turn [meaning the player cannot be caught taking unavoidable damage at the beginning of a room]
+
 -added new item drop: Potion Chest (gives the player two health potions and one of either an xp or a strength potion)
+
 -room width is now randomized; min and max columns increase with floor number, but row number is fixed; inner-wall min and max count logic was also modified to be dependent on column count
+
 -rooms now fade out on exit, and fade back in from to the room/floor title screen
+
 -added a function that unequips the player after a delay (allows the room to fade out, keeping the user from seeing the character being unequipped)
+
 -inner and outer wall tiles are now mixed (previous and next floor tiles) on even floor numbers extending down through floor 8 (floor 10 has all tiles mixed)
+
 -the exit tile now spawns randomly on the outside track of the room (cannot spawn at player location)
+
 -added animated torch object that spawns at the corners (outer wall track) of each room
 -player now recieves 20 health points when experience is spent on the constitution skill
 
 ~Temp/Trial Changes made permanent [from v1.0]~
+
 -each room now has (floor - 1) extra enemies and rooms that end in 0 now spawn 2 mini-bosses
+
 -each floor can now spawn only two enemy types, the previous floor's enemy and the current floor's enemy (save for floor one, which only has one enemy type)
 
 ~Temp/Trial Changes for Beta v1.1~
+
 -the column size (width) of any individual room is randomized between the minimum (4 + floor) and maximum (8 + floor)
   Note: camera now centers on room width; current boundary for room size is 20 columns
+
 -increased the difficulty curve by adding bonuses to enemy hp and player damage stats
 
 ***Modifications from vanilla 2D Roguelike tutorial to SRB Modified Version***
 
 -Minor text and color changes
+
 -Added quit/retry option after player death
+
 -Movement/turn allocation algorithms updated to prevent enemies and player from occupying the same space
 
 ***Further Modifications leading toward Ancestor's Quest 2 Release***
 
 -enemy and player sprites now turn (left/right) in the direction they are moving
+
 -player no longer takes damage for moving
+
 -player damage output is now dependant on player Strength and Weapon Damage
+
 -made some game balance changes, including enemy XP values, damage, hp, and lowered Wall hit points
+
 -fixed possibility for enemies to move into the same space and become stuck (more enemies increases refractory period for player movement)
+
+
 -enemies can now be killed, and wall destruction is now handled by the same script (player movement refractory period restored when enemies are removed)
+
 -player now gains experience for "killing" damagable objects, added XP to the UI
+
 -the two current vampire enemies are now more distinct (Ghoul and Ghoul Elder), the Ghoul Elder prefab becoming a sort of mini-boss
+
 -added the status button to the UI that opens a stats and an inventory panel
+
 -health drinks are now pickups that accumulate in the player's inventory and can be used by clicking on them (health items also heal less and are less frequent)
+
 -health is now governed by a maximum health score, health items are safe from being picked up/used if the character is at full health
+
 -player find skill is dependant on player Dexterity and influences the quality of items that drop from enemies
+
 -mini-boss enemies spawn every 5 rooms
+
 -game progression now consists of 10 rooms per floor of catacombs, enemy and boss sub-arrays are set to advance with floor increment, and game over text reads appropriately
+
 -added mushroom floor item that deals food damage when consumed
+
 -changed/added sprites to change the feel of the game from post apocalyptic to fantasy (including new exit tiles for room and floor transition and no aluminum cans on potion sprites)
+
 -slight tweaks to UI placement [may have to alter, test for different resolutions]
+
 -implemented xpCost reduction sub-stat for intelligence
+
 -implemented exp system that allows the player to spend XP to increase stats: buttons and levelUpIndicator activate when the player has enough exp to spend
+
 -enemy kills can now grant random drops if a find skill check is high enough
+
 -new item drops from enemies after sufficient find check, strength potions!  Gain one strength point for each strength potion you drink (lasts for one turn)
+
 -updated inventory panel to show more info about usable items
+
 -new item drops from enemies after sufficient find check, experience potions!  The most powerful potion in the game, earn 100 experience points for each potion you drink
+
 -new item drops from enemies after sufficient find check, armor and weapon chests!  These increment the value in the player's inventory for armor and weapons, respectively
+
 -added a new menu, opened by a new UI button, that gives the player the option to restart, quit to main menu [when it exists], or quit the game
+
 -created new sprites for equipable weapons and armor [prefabs finished for all 4 weapons and armor sets]
+
 -recolored wall tiles for less dreary and more varied look to levels [more wall tiles to come]
+
 -fully implemented floor 2 enemy Orc and boss Orc Chieftan
+
 -slowed down player hit and attack animations so the sprites can be seen more clearly
+
 -enemies now have hit animations and player and enemy objects flash red when they take damage
+
 -weapon and armor chests grant 2x health or 2x strength potions (respectively) if the player's weapon/armor inventory values are not less than the current floor level
+
 -weapon and armor inventory is fully implemented and functional
+
 -cleaned up sprite sheets to remove extra pixels and extraneous sprites
+
 -options menu now has audio options to toggle mute on sound effects and music (with visual UI selectors)
+
 -options menu now has audio option to cycle through three game music options (Gavotte, Metal, and Apocalypse)
+
 -armor now provides an armor bonus to max hp instead of incrementing the constitution score itself
+
 -options menu now has music and sound effects volume buttons (and tick marks) that allow the player to customize the game's sound mix
+
 -added a sound effect (bell chime) when using experience points
+
 -implemented Main Menu scene that includes an intro animation and buttons to start the game [functional] and to start a tutorial [not functional]
+
 -made changes to the room increment logic so that rooms, enemies, and exit tiles load in proper order [Main scene was not set up to be the second Level Loaded]
+
 -changed the music and sound effects volume when the Main scene loads to make the transition less jarring [could still be better]
+
 -added a skip function to the main menu (works with a click or tap anywhere on the screen)
+
 -completed comprehensive tutorial panels (6 in all) explaining how to play the game and what to expect
+
 -changed armor alternate drop to 2 health potions and weapon alternate drop to 3 health potions
+
 -switched armor and weapon chest in drop order (new order: health potion, armor chest, strength potion, weapon chest, experience potion)
