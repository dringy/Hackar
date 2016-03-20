using UnityEngine;
using System.Collections;
//Represent a hackable propert which is true or false
public class HackableBool : HackableProperty {
	//The value of the property
	public bool value;

	//Inverts the property
	public void invert()
	{
		value = !value;
	}

	//Populates a given HackableBool with a copy of the data
	public void copy(HackableBool property)
	{
		value = property.value;
		superCopy (property);
	}

	//Checks if two hackablePropeties contain different data
	//True represents that the two items are different
	public override bool isDifferent(HackableProperty property)
	{
		return (((HackableBool)property).value != this.value);
	}

}
