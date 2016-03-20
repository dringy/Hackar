using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the hack and step statistics of a level to be held by the level controller.
/// </summary>
public class LevelStats : MonoBehaviour {
	//Booleans represent if a limit is present and the number is the limit
	public bool isHackLimit;
	public int hackLimit;
	public bool isStepLimit;
	public int stepLimit;

	//The current steps and hacks
	private int steps;
	private int hacks;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		steps = 0;
		hacks = 0;
	}

	/// <summary>
	/// Checks to see if there are steps left.
	/// </summary>
	/// <returns><c>true</c>, If the player can step <c>false</c> otherwise.</returns>
	public bool canStep()
	{
		if (!isStepLimit)
		{
			return true;
		}
		else if (steps + 1 > stepLimit)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	/// <summary>
	/// Increments the step counter.
	/// </summary>
	public void step()
	{
		steps++;
	}

	/// <summary>
	/// Attempts the hack, this fails if there are no hacks lef.
	/// </summary>
	/// <returns><c>true</c>, if hacks was used up correctly <c>false</c> otherwise.</returns>
	/// <param name="numberOfHacks">Number of hacks the user wishes to use.</param>
	public bool attemptHacks(short numberOfHacks)
	{
		//Checks if there is a hack limit and then if there is if the number of hacks breaks the limit
		//If it does not the hack counter is increased
		if (!isHackLimit)
		{
			return true;
		}
		else if (hacks + numberOfHacks > hackLimit)
		{
			return false;
		}
		else
		{
			hacks += numberOfHacks;
			return true;
		}
	}

	/// <summary>
	/// Gets the hack string for use with the GUI
	/// </summary>
	/// <returns>The hack string.</returns>
	public string getHackString()
	{
		//In the form Hacks: x or Hacks: Unlimited where x is the hacks left
		string hackString = "Hacks: ";
		if (isHackLimit)
		{
			hackString += (hackLimit - hacks);
		}
		else
		{
			hackString += "Unlimited";
		}
		return hackString;
	}

	/// <summary>
	/// Gets the step string for use with the GUI
	/// </summary>
	/// <returns>The step string.</returns>
	public string getStepString()
	{
		//In the form Steps: x or Steps: Unlimited where x is the steps left
		string stepString = "Steps: ";
		if (isStepLimit)
		{
			stepString += (stepLimit - steps);
		}
		else
		{
			stepString += "Unlimited";
		}
		return stepString;
	}
}
