using UnityEngine;
using System.Collections;
//Represents the hackable part of a button
public class HackableButton :  HackableColour {

	//Used for instanciation
	void Start () {
		setColourProperty ();
		updateColour ();
	}

	//Only the colour is hackable
	public override HackableProperty[] getPropeties()
	{
		HackableProperty[] properties = new HackableProperty[1];
		properties [0] = colourProperty;
		return properties;
	}

	//Updates the colour propery
	public override void updateProperties(HackableProperty[] properties)
	{
		colourProperty = (HackableEnum) properties [0];
		updateColour ();
	}
}
