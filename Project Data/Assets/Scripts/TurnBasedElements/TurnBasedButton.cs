using UnityEngine;
using System.Collections;
//Represents the turn based functionality of the button in case it has a depression timer involved
public class TurnBasedButton : TurnBasedElement {
	public Button button; //The envriomental part of the object

	public override void advanceTurn ()
	{
		//Advance turn simply calls the button advanceTurn() function
		button.advanceTurn ();
	}
}
