using UnityEngine;
using System.Collections;

/// <summary>
/// The enviromental object for the trap door
/// </summary>
public class TrapDoor : Enviroment {
	public HackableTrapDoor hackablePart; //The hackable part of the object
	private float originalY; //The original Y position before repositioning

	//Instanciates the object
	public void Start()
	{
		//Calls the enviroment superclass
		base.Start ();
		//originalY value is saved
		originalY = transform.position.y;
		//The cube is squashed
		transform.localScale = new Vector3(transform.localScale.x - 0.01f, 0.199f, transform.localScale.z - 0.01f);
		//The positioned is moved up
		transform.position = new Vector3 (transform.position.x, transform.position.y + 0.4f, transform.position.z);
	}

	//Accessor for the original y value
	public float getOriginalY()
	{
		return originalY;
	}

	//The door cannot be walked to as it acts like a wall
	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		return false;
	}

	//It can only be walked on top of if it isn't open
	public override bool canWalkOnTopOf()
	{
		return !hackablePart.isDoorOpen();
	}

	//Walking on top of it only workds if the trap door isn't open, or operning or closing
	public override bool walkOnTopOf()
	{
		return !(hackablePart.isDoorOpen() || hackablePart.isDoorOpening() || hackablePart.isDoorClosing());
	}

}
