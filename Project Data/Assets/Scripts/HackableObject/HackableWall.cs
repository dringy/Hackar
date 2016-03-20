using UnityEngine;
using System.Collections;
//The hackable part of a wall objecy
public class HackableWall : HackableColour {

	// Used for initialization
	void Start () {
		setColourProperty ();
		updateColour ();
	}

	//The only property is the colour property
	public override HackableProperty[] getPropeties ()
	{
		HackableProperty[] properties = new HackableProperty[1];
		properties [0] = colourProperty;
		return properties;
	}

	//Colour property is not updated
	public override void updateProperties (HackableProperty[] properties)
	{
		colourProperty = (HackableEnum)properties [0];
		updateColour ();
	}

}
