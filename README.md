# BattleshipWebApi

This is an Asp.Net Core 2.2 Webapi for Battleship. The API is meant to track the state of a typical battleship game from only 1 player's perspective. *Please note this is not the complete game but only an API to track the state of the game.*


### Scope
The project is meant to track the state of the Battleship game for 1 player. The project does not maintain a persistance layer for storing the game data between multiple sessions of game play. This is because there is no database configured to capture the state of the game. The project however retains the game state for a given game session. This information is stored in memory of the API. The API provides end points for:
* Creating a board.
* Adding a ship.
* Attacking a ship.

*There are underlying business logic implemented to handling various typical and edge case scenarios that are detailed below.*

### Tech Stack
For API development: .Net Core 2.2
For Unit Testing: NUnit, Moq
For Logging: NLog
For API XML Documentation: Swagger/Swashbuckle Asp.Net Core
For API Simulation: Postman


### Project URL:
The API has been deployed to a t2.micro EC2 server. The project URL is: 
<ec2-13-211-139-250.ap-southeast-2.compute.amazonaws.com/api/battleship>

### Swagger Documentation
<http://ec2-13-211-139-250.ap-southeast-2.compute.amazonaws.com/swagger/index.html>

### Postman Collection for Typical Requests
A request collection has been included in the git repo. 

### API Endpoints
* CreateBoard()<br/>
 This creates a [10x10] with grid cell indexing starting from 0 to 9. Therefore the 1st grid cell is positioned at `{verticalPos:0,     horizontalPos:0}` <br/>
 
 ##### MethodVerb: `Post` <br/>
 
 ##### Sample Request: `{}`
 
 * AddShip() <br/>
  This method adds a new ship with given dimension at the given grid location identified by *verticalHeadPosition* and *horizontalHeadPosition*. As before, the vertical/horizontal coordinates are between 0-9. The length of the ship could be between 1-10. If the addition is successful the response returns 200OK with appropriate message. If the addition fails the response returns BadRequest status with appropriate message. An addition of ship could fail because:<br/>
  1) The ship's position overlaps with another ship.<br/>
  2) The ship's dimension is out of board area. <br/>
 
 ##### MethodVerb: `Post`<br/>
 
 ##### Sample Request:
  ```
  {
    "verticalHeadPosition":6,
    "horizontalHeadPosition": 6,
    "size": 2, //ranges between 1-10.
    "orientation": "vertical"
  }
````

* AttackShip() <br/>
This method allows a user to attack a ship by sending the coordinates on the grid `{verticalPosition, horizontalPosition}`. As before, the vertical/horizontal coordinates are between 0-9. If the attack is successful the response returned is 200OK along with the appropriate message. If the attak missed a ship, the response returned is 200OK with appropriate message. If the attack sinks a ship, the response returned is 200OK with appropriate message.<br/>

 ##### MethodVerb: `Post`<br/>

 ##### Sample Request:
  ```
  {
	  "verticalPosition": 1,
	  "horizontalPosition" : 2
  }
  ````
  
### Assumptions and Considerations.
1. The grid coordinates start from (0,0) and end at (9,9). So the user inputs have to be within this range. The validation and checks have been put in place for edge cases.
2. _The ship can added only vertically and horizontally_. The coordinates for adding the ship is only for **ship's head and not it's tail**. Edge cases have been take into consideration and coded in the validation logic.
3. The attack on the ship can **Miss/Hit/Sink** a ship. _If the attack sinks a ship, the sunk ship is removed from memory and the grid cells occupied by the ship are reset. Therefore, if the next attack is on the same coordinate, the attack will result in a miss._ __After the ship has been sunk, a new ship can be added to the same location.__
4. All the exceptions and debug information are stored in Nlog logs, as configured in the project. The logs of the project are currently stored at:`c:\temp\battleship\`. This can be modified in nlog.config for the new location. The logs capture all the major milestones of the project.
5. Unit testing takes care of all the possible scenarios I could think of in the given time. I potentially could have written more tests for testing out of the box scenarios and others I couldn't think of.

