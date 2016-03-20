using UnityEngine;
using System.Collections;

/// <summary>
/// Represent a door that can be chacked.
/// </summary>
public class HackableDoor : HackableOpenableObject {
	public bool xOrientated;
	public bool locked;
	public string key;

	private HackableBool isLocked;
	private HackableString keyCode;

	// Use this for initialization
	void Start () {
		//Just sets up the basic information
		isOpening = false;
		isOpen = false;

		isLocked = new HackableBool ();
		isLocked.value = locked;
		isLocked.name = "Locked";

		keyCode = new HackableString ();
		keyCode.value = key;
		keyCode.name = "Key Name";

		setColourProperty ();
		updateColour ();

		//Squashes the cube to look like a door relative to it's orientation
		if(xOrientated)
		{
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.1f, 0.3f);
		}
		else
		{
			transform.localScale = new Vector3(0.3f, transform.localScale.y - 0.1f, transform.localScale.z);
		}

	}

	//Removes the locked status of the door
	public void unlock()
	{
		locked = false;
	}

	//Gets the the hack properties for the hack menu
	public override HackableProperty[] getPropeties()
	{
		HackableProperty[] properties = new HackableProperty[3];
		properties [0] = colourProperty;
		properties [1] = isLocked;
		properties [2] = keyCode;
		return properties;
	}

	//Pushes hack menu changes to the object
	public override void updateProperties(HackableProperty[] properties)
	{
		colourProperty = (HackableEnum) properties [0];
		isLocked = (HackableBool) properties [1];
		keyCode = (HackableString)properties [2];

		updateColour ();
		locked = isLocked.value;
		key = keyCode.value;
	}
}
