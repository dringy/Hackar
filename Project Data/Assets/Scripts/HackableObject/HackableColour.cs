using UnityEngine;
using System.Collections;
//Represents a hackable object that has a hackable colour option
public abstract class HackableColour : HackableObject {
	//Colour is a hackable enum
	protected HackableEnum colourProperty;
	//The colour index for the array in the enum
	public int colourIndex;

	protected void setColourProperty()
	{
		//Colour property is set up
		colourProperty = new HackableEnum ();
		colourProperty.addString ("Red");
		colourProperty.addString ("Blue");
		colourProperty.addString ("Green");
		colourProperty.addString ("Yellow");
		colourProperty.addString ("Purple");
		colourProperty.addString ("Pink");
		colourProperty.addString ("Orange");
		colourProperty.setIndex (colourIndex);
		colourProperty.name = "Colour";
	}

	//Updates the colour from the new colour property
	protected void updateColour()
	{
		Color colour;
		//Uses a switch statement from the output from the HackableEnum
		switch (colourProperty.getValue())
		{
		case ("Red"):
		{
			colour = Color.red;
			break;
		}
		case ("Blue"):
		{
			colour = Color.blue;
			break;
		}
		case ("Green"):
		{
			colour = Color.green;
			break;
		}
		case ("Yellow"):
		{
			colour = Color.yellow;
			break;
		}
		case ("Purple"):
		{
			colour = new Color(0.153f, 0.034f, 0.166f);
			break;
		}
		case ("Pink"):
		{
			colour = new Color(245f/100, 39f/100, 191f/100);
			break;
		}
		default:
		{
			colour = new Color(185f/100, 72f/100, 3f/100);
			break;
		}
		}
		if(renderer != null)
		{
			renderer.material.color = colour;
		}
		foreach(Transform child in transform)
		{
			child.renderer.material.color = colour;
		}
	}
}
