using UnityEngine;
using System.Collections;

/// <summary>
/// The enviroment object for the door.
/// </summary>
public class Door : Enviroment {
	//The hackable part
	public HackableDoor hackablePart;

	public override void preWalk(MobileEnviroment enviroment)
	{
		//If it isn't locked then the door needs to be opened first onces it is then the mob can move through it
		if (hackablePart.locked)
		{
			hackablePart.unlock();
		}
		if (!hackablePart.isDoorClosing())
		{
			hackablePart.open ();
		}
	}

	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		//Mobs can only walk to the object if it isn't locked
		return (!hackablePart.locked || ((enviroment is PlayerController) && (PlayerController.hasKey (hackablePart.key))));
	}

	//Doors cannot be walked ontop of
	public override bool canWalkOnTopOf()
	{
		return false;
	}
	
	public override bool walkOnTopOf()
	{
		return false;
	}

	//If the mob walks away from the door then it closes
	public override void walkAwayFrom()
	{
		hackablePart.close ();
	}

	//The door blocks sight if it is closed or closing
	public override bool doesBlock ()
	{
		return (!hackablePart.isDoorOpening () || hackablePart.isDoorClosing ());
	}


}
