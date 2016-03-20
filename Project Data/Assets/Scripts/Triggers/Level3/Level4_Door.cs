using UnityEngine;
using System.Collections;

public class Level4_Door : TwoStateTrigger {
	public HackableDoor door;
	public NPC spotter;

	//The on trigger function
	public override void triggerOn()
	{
		door.open ();
	}
	
	//The off trigger function
	public override void triggerOff()
	{
		door.close ();
	}
}
