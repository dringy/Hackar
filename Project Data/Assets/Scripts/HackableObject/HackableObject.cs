using UnityEngine;
using System.Collections;

//Represents an object that can be hacked
public abstract class HackableObject : MonoBehaviour {
	public bool isHackLocked;

	//When the mouse if over the object and the user clicks then the Hack GUI is updated
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			if (isHackLocked)
			{
				LevelGUI.displayTimedMessage(name + " is hack locked.");
			}
			else
			{
				LevelGUI.displayGUI(this);
			}
		}
	}

	//The properties for the object
	public abstract HackableProperty[] getPropeties();

	//Updates the properties given a similar array that can be given
	public abstract void updateProperties(HackableProperty[] properties);

}
