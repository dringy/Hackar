using UnityEngine;
using System.Collections;
//Represents a turn based element
public abstract class TurnBasedElement : MonoBehaviour {
	void Start()
	{
		//It registers itself in the turn based controller
		TurnBasedController.addTurnBasedElement (this);
	}
	//Advances the turn of the element in the enviroment
	public abstract void advanceTurn ();

	public virtual void takeEndOfTurnObservation()
	{
		return;
	}
}
