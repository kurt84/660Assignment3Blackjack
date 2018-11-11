# 660Assignment3Blackjack
RAD Blackjack Game


TODO:
Persistence
-Create database  and use entity framework to update total money for player
--Possibly store game state for resuming if session terminates mid-game, but this is not required

Class to manage game state
-This will work internally to control the game logic
-Start with simply giving cards to player
-Add simple game logic to determine score
-Add handling for betting and player money total
-Add logic for dealer
-Add logic for advanced rules

User interface to display cards based on the state of the game class
-Queries the game class to determine what needs to be drawn and where
-Can rearrange the card layout if it needs to (such as in the case of splitting)
-Possibly make cards overlap if the field is too crowded

Card images, sounds, animations
-Noah has added card images
-Need a sound for card flipping and maybe card sliding
-Animation for cards moving around


Research the game and think about how to implement the rules
