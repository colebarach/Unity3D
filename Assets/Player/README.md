# Player
This package contains the code and prefabs for a first-person player object. The purpose and use of the individual scripts are outlined below.
## Player Script
Contains the code for moving and rotating the player object. This should be attached to a CharacterController which has a Camera child. The horizontal and vertical input axes are used for movement, and the mouse axes are used for rotation.
## Player Interaction
The main script for interaction. This is called when the left mouse button is pressed, and stores a reference to the object directly in front of the camera. The length of this raycast is determined by the MaxDistance variable, and the reference to this object is stored in the Interactee variable. The function CheckInteractee() is used to check if an object has been interacted with, in which case the function will return true.
## Player Inventory
Stores the data for anything inventory related. Object quantities are stored in the itemQuantities array and item names are stored in the itemNames array. To make an item known to the inventory system, add an appropriate entry to both arrays under the same index.
## Interaction Behavior Scripts
Used to interface player interaction with behavior objects
*See the Behavior package for reference
## Player Debug Script
Used for debugging of the Player Script, prints requested internal information to the console.