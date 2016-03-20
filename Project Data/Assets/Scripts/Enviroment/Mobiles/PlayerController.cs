using UnityEngine;
using System.Collections;

/// <summary>
/// The class that supplies the functionality for the player and the controls.
/// </summary>
public class PlayerController : MobileEnviroment {
	//The Camera is needed to fix it's position relative to the player
	public GameObject camera;
	//Offset is used to keep track of the the rotation of the camera so that the movement keys are relative
	//It can range from 0 to 3
	public int offSet;
	//The stats of the level is needed to check if the user has steps remaining to move with
	public LevelStats levelStats;
	//A hold is when something in the game wants everyone to wait until it is finished
	private static int holds;
	//A list of the players keys
	private static ArrayList keys;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		base.Start ();
		camera.transform.LookAt(this.transform.position);
		holds = 0;
		for (int rotates = 0; rotates < offSet; rotates++)
		{
			rotateClockwise ();
		}
		keys = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		//We first call super class function
		base.Update ();

		bool moved = false; //moved indicates that the user has attempted to move
		//We are only allowed to move if there are no holds on the player
		if (!isHeld())
		{
			moved = true;
			//Here we check for the arrow keys inputs and WASD inputs for movement, the turn based controller is then prompted after the movement
			if ((Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.W)))
			{
				moveSightWise (1);
				TurnBasedController.advanceTurn();
			}
			else if ((Input.GetKeyDown(KeyCode.DownArrow)) || (Input.GetKeyDown(KeyCode.S)))
			{
				moveSightWise (-1);
				TurnBasedController.advanceTurn();
			}
			else if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.GetKeyDown(KeyCode.A)))
			{
				moveSideWays(1);
				TurnBasedController.advanceTurn();
			}
			else if ((Input.GetKeyDown(KeyCode.RightArrow)) || (Input.GetKeyDown(KeyCode.D)))
			{
				moveSideWays(-1);
				TurnBasedController.advanceTurn();
			}
			//If the user preses space it indicates that they are waiting and the enviroment will advance a turn
			else if (Input.GetKeyDown (KeyCode.Space))
			{
				TurnBasedController.advanceTurn();
			}
			//If no movement commands are issues then we mark it as false
			else
			{
				moved = false;
			}
		}
		//Q and E rotate the camera, Q by 90 degrees and E by 270 degrees (equal to -90 degrees)
		//The camera offset which is used to ensure the movement keys are relative also have to be changed here
		if (Input.GetKeyDown (KeyCode.Q))
		{
			rotateClockwise();
			if (++offSet > 7)
			{
				offSet -= 8;
			}
		}
		else if (Input.GetKeyDown (KeyCode.E))
		{
			rotateAntiClockwise();
			if (--offSet < 0)
			{
				offSet += 8;
			}
		}
		//Empty else-if statement for tab is used here so the disableGUI() function is called which is the only function needed for Tab
		else if (Input.GetKeyDown (KeyCode.Tab));
		//Escape loads the main menu level
		else if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.LoadLevel("MainMenu");
		}
		//R loads the same level we're on 
		else if (Input.GetKeyDown (KeyCode.R))
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
		//If we haven't moved we return to avoid calling disableGUI()
		else if (!moved)
		{
			return;
		}
		//If any control has been pressed the GUI is disabled to prevent positioning issues and inconsistency issues
		LevelGUI.disableGUI ();
	}

	/// <summary>
	/// Moves in the x direction
	/// </summary>
	/// <param name="x">The number of spaces to move</param>
	private void moveX(int x)
	{
		movePosition (x, 0, 0);
		if (x == 1)
		{
			updateDirectionFacing(direction.negativeX);
		}
		else
		{
			updateDirectionFacing(direction.posotiveX);
		}
	}

	/// <summary>
	/// Moves in the z direction
	/// </summary>
	/// <param name="z">The number of spaces to move</param>
	private void moveZ(int z)
	{
		movePosition (0, 0, z);
		if (z == 1)
		{
			updateDirectionFacing(direction.negativeZ);
		}
		else
		{
			updateDirectionFacing(direction.posotiveZ);
		}
	}

	/// <summary>
	/// Moves the position of the player.
	/// </summary>
	/// <param name="x">The number of the spaces to move in the x direction</param>
	/// <param name="y">The number of the spaces to move in the y direction</param>
	/// <param name="z">The number of the spaces to move in the z direction</param>
	private void movePosition(int x, int y, int z)
	{
		//Movements are only accepted if there are steps remaining
		if (levelStats.canStep())
		{
			//The movement method is then called in the Enviroment Map, if it's successful the step counter is incremented
			if(EnviromentMap.moveTo(transform.position.x + x, transform.position.y + y, transform.position.z + z, this))
			{
				levelStats.step();
			}
		}
		else
		{
			//If there are no steps remaining a message is displayed
			displayStepLimitMessage();
		}
	}

	/// <summary>
	/// Displays the step limit message.
	/// </summary>
	private void displayStepLimitMessage()
	{
		//If we run out of steps we display a timed message
		LevelGUI.displayTimedMessage("Exceeded Step Limit");
	}

	/// <summary>
	/// Moves front and back by the factor.
	/// </summary>
	/// <param name="factor">Steps to move.</param>
	private void moveSightWise(short factor)
	{
		//It uses the offset to decide which direction you can move in
		if (offSet == 0 || offSet == 1)
		{
			moveZ(factor);
		}
		else if (offSet == 2 || offSet == 3)
		{
			moveX(factor);
		}
		else if (offSet == 4 || offSet == 5)
		{
			moveZ(-factor);
		}
		else
		{
			moveX(-factor); 
		}
	}

	/// <summary>
	/// Moves side ways by the factor.
	/// </summary>
	/// <param name="factor">The steps to move.</param>
	private void moveSideWays(short factor)
	{
		//It uses the offset to decide which direction you can move in
		if (offSet == 0 || offSet == 1)
		{
			moveX(-factor);
		}
		else if (offSet == 2 || offSet == 3)
		{
			moveZ(factor);
		}
		else if (offSet == 4 || offSet == 5)
		{
			moveX(factor);
		}
		else
		{
			moveZ(-factor);
		}
	}

	//Registers a hold onto the player
	public static void hold()
	{
		holds++;
	}

	//Releases a hold onto the player
	public static void release()
	{
		if(isHeld())
		{
			holds--;
		}
	}

	//Checks to see if a hold is placed on the player
	public static bool isHeld()
	{
		return (holds > 0);
	}

	//Rotates the player, camera and model clockwise
	private void rotateClockwise()
	{
		camera.transform.Rotate(new Vector3(0, 45, 0));
	}

	//rotates the player, camera and model anticlockwise
	private void rotateAntiClockwise()
	{
		camera.transform.Rotate(new Vector3(0, 315, 0));
	}

	//Adds a key to the players keys
	public static void addKey(string key)
	{
		keys.Add (key);
	}

	//Checks to see if a player has a key
	public static bool hasKey (string key)
	{
		string lowerKey = key.ToLower ();
		foreach (string ownedKey in keys)
		{
			if (ownedKey.ToLower().Equals (lowerKey))
			{
				return true;
			}
		}
		return false;
	}

	//Creates the key display message for the GUI
	public static string getKeysString()
	{
		string keyString = "Keys: ";
		if (keys.Count>0)
		{
			keyString += keys[0];
			if (keys.Count>1)
			{
				for(int i = 1; i < keys.Count; i++)
				{
					keyString += ", " + keys[i];
				}
			}
		}
		else
		{
			keyString += "NONE";
		}
		return keyString;

	}

	public void triggerWalkAwayDueToTeleport()
	{
		if (currentEnviroment != null)
		{
			currentEnviroment.walkAwayFrom();
		}
	}
}
