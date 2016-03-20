using UnityEngine;
using System.Collections;
//Represents a standard Turn based npc
public class TurnBasedNPC : TurnBasedElement {
	public NPC npc; //NPC enviroment class
	
	public override void advanceTurn ()
	{
		//Control is simply passed into enviroment class
		npc.advanceTurn ();
	}

	public override void takeEndOfTurnObservation()
	{
		npc.checkForPlayer ();
	}
}
