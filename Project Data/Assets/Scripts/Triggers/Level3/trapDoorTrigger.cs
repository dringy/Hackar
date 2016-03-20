using UnityEngine;
using System.Collections;
//Represents the Trap door switch on Level 3
public class trapDoorTrigger : TwoStateTrigger {
	//The trap door part
	public HackableTrapDoor trapDoor;

	//When the button is pressed the trap door should open
	public override void triggerOn()
	{
		trapDoor.open ();
	}

	//When the button is depressed the trap door should close
	public override void triggerOff()
	{
		trapDoor.close ();
	}
}
