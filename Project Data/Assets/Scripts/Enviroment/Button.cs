using UnityEngine;
using System.Collections;

/// <summary>
/// The enviromental object for the button
/// </summary>
public class Button : Enviroment {
	public bool isTemporary; //If it is false then the button stays down once it is stood on
	public int turnTimer; //The turn timer is how many turns until the button rises up
	public TwoStateTrigger trigger; //The trigger has two pars, one part when the button is pressed and one when it is depressed

	//The counter to count the turns after the button is pressed
	private int turnCounter;
	private bool isStoodOn; //represents if a mob is ontop of them
	private bool isDown; //represents if the button is depressed

	//Instanciated the objext
	public void Start()
	{
		//Enviroment super-function
		base.Start ();
		//values are preset
		turnCounter = 0;
		isStoodOn = false;
		isDown = false;
		//It set in it's upright position
		setLookToUp ();
		//It is repositioned to look mre like a button
		transform.position = new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z);
	}

	//Can be walked on always
	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		return true;
	}

	//The walk to function
	public override void postWalk(MobileEnviroment enviroment)
	{
		//Resets the turn counter and marks the button as stood on
		turnCounter = 0;
		isStoodOn = true;
		//If the button isn't already depressed then it's variables and images is updated
		if (!isDown)
		{
			isDown = true;
			setLookToDown ();
			//Trigger function is called
			trigger.triggerOn ();
		}
	}

	//Button cannot be walked ontop of
	public override bool canWalkOnTopOf()
	{
		return false;
	}
	
	public override bool walkOnTopOf()
	{
		return false;
	}
	
	public override void walkAwayFrom()
	{
		//when walked away from the stood on variable is updated
		isStoodOn = false;
	}

	//Advance turn increments turn counter
	public void advanceTurn()
	{
		if (isTemporary && !isStoodOn && isDown)
		{
			if (turnCounter++ == turnTimer)
			{
				//If the turn counter reaches that if the timer then the button is made to be depressed and the depression trigger is set off
				setLookToUp();
				trigger.triggerOff();
				isDown = false;
			}
		}
	}

	//Makes the button appear depressed
	private void setLookToUp()
	{
		transform.localScale = new Vector3 (0.5f, 0.4f, 0.5f);
	}

	//Makes the button appear pressed
	private void setLookToDown()
	{
		transform.localScale = new Vector3 (0.5f, 0.05f, 0.5f);
	}

	//Buttons do not block sight
	public override bool doesBlock ()
	{
		return false;
	}
}
