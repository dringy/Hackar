using UnityEngine;
using System.Collections;

/// <summary>
/// representes an enviroment that moves around
/// </summary>
public abstract class MobileEnviroment : Enviroment {
	public TurnBasedElement turnBasedElement; //The turn based element of the mobile which is used for freezing
	public enum direction {posotiveX, posotiveZ, negativeX, negativeZ};
	public direction directionFacing;

	private short move;
	private Enviroment previousEnviroment;
	protected Enviroment currentEnviroment;
	private Vector3 newLocation;

	//The mobs model
	public GameObject model;

	private bool isDying; //Represents if the mobile is dying from a trap
	private float limit; //This is a float limit used if the mobile is falling

	//Used for instanciation
	public void Start()
	{
		//Calls the superfunction
		base.Start();
		isDying = false;
		limit = 0f;
		move = 0;
		updateDirectionFacing (directionFacing);
	}

	//Gets the enviroment that the mob posses
	public Enviroment getEnviroment()
	{
		return currentEnviroment;
	}

	//Update calls every frame
	public void Update()
	{
		//If the mob is dying they will move down for a bit
		if (isDying)
		{
			transform.Translate (0, -2.5f * Time.deltaTime, 0);
			//If we've reached the limit then we stop the mob dying, we release the player and deactiviate the game object
			if (transform.position.y <= limit)
			{
				isDying = false;
				PlayerController.release();
				if (this is PlayerController)
				{
					Application.LoadLevel (Application.loadedLevelName);
				}
				gameObject.SetActive (false);
			}
		}
		//If there is a location set and the player isn't being held then we can move
		if((move == 1) && (!PlayerController.isHeld ()))
		{
			performPreWalk ();
		}
		if((move == 2) && (!PlayerController.isHeld ()))
		{
			performPostWalk ();
		}
	}

	//Starts the death of the mov
	public void killMob()
	{
		//Firstly while the mob dies we should hold the player
		PlayerController.hold ();
		//We then remove the player from the enviroment
		EnviromentMap.removeEnviroment (transform.position.x, transform.position.y, transform.position.z);
		//The death animation is then triggered
		isDying = true;
	}

	//Frezes the mob from moving
	public void freezeMob()
	{
		//To freeze the mob we simply remove the mob from the turn based controller
		TurnBasedController.removeTurnBasedElement (turnBasedElement);
	}

	//Moves the mob in the x direction if possible
	protected virtual bool movePosotiveX(int x)
	{
		return EnviromentMap.moveTo(transform.position.x + x, transform.position.y, transform.position.z, this);
	}

	//Moves the mob in the z direction if possible
	protected virtual bool movePosotiveZ(int z)
	{
		return EnviromentMap.moveTo(transform.position.x, transform.position.y, transform.position.z + z, this);
	}

	//Moves the mob in the negative x direction if possible
	protected void moveNegativeX(int x)
	{
		movePosotiveX (-x);
	}

	//Moves the mob in the negative z direction if possible
	protected void moveNegativeZ(int z)
	{
		movePosotiveZ (-z);
	}

	//The enviromental boolean functions are pre-set for all mobs

	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		return false;
	}
	
	public override bool canWalkOnTopOf()
	{
		return false;
	}
	
	public override bool walkOnTopOf()
	{
		return false;
	}

	/// <summary>
	/// Sets up the player to move to a new location.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	/// <param name="enviroment">Enviroment - the enviroment the player is moving into</param>
	public void updateMovement(float x, float y, float z, Enviroment enviroment)
	{
		PlayerController.hold ();
		if(move==1)
		{
			performPreWalk ();
			performPostWalk ();
		}
		else if(move==2)
		{
			performPostWalk ();
		}
		PlayerController.release ();
		//the variables are set up for the Update() function to move the player
		newLocation = new Vector3(x, y, z);
		currentEnviroment = enviroment;
		move = 1;
	}

	public void performPreWalk()
	{
		if (currentEnviroment != null)
		{
			currentEnviroment.preWalk (this);
		}
		move = 2;
	}

	public void performPostWalk()
	{
		EnviromentMap.setEnviroment (transform.position.x, transform.position.y, transform.position.z, previousEnviroment);
		transform.Translate (newLocation);
		EnviromentMap.setEnviroment (transform.position.x, transform.position.y, transform.position.z, this);
		if (previousEnviroment != null)
		{
			previousEnviroment.walkAwayFrom();
		}
		previousEnviroment = currentEnviroment;
		move = 0;
		if (currentEnviroment != null)
		{
			currentEnviroment.postWalk (this);
		}
	}

	/// <summary>
	/// Updates the direction the mobile is facing and rotates the model respectivley.
	/// </summary>
	/// <param name="directionFacing">The new direction of the mobile.</param>
	protected void updateDirectionFacing(direction directionFacing)
	{
		this.directionFacing = directionFacing;
		switch (this.directionFacing)
		{
		case (direction.negativeX):
		{
			model.transform.rotation = Quaternion.Euler (0, 90, 0);
			break;
		}
		case (direction.posotiveX):
		{
			model.transform.rotation = Quaternion.Euler (0, 270, 0);
			break;
		}
		case (direction.negativeZ):
		{
			model.transform.rotation = Quaternion.Euler (0, 0, 0);
			break;
		}
		default:
		{
			model.transform.rotation = Quaternion.Euler (0, 180, 0);
			break;
		}
		}
	}
}
