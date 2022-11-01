# Progress Package
This package contains all scripting related to story and game progress. A general outline of the scripts is given below.
## Progress Master
This singleton object contains the current state of the game with a list of aliases for each state. The CheckState function allows a progress-dependent object to check is the game is in a current state.
## Progress Behavior Scripts
These scripts are used for interfacing the progress package with the behavior package.
*See Behavior package.