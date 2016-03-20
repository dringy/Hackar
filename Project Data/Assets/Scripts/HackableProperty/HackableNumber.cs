using UnityEngine;
using System.Collections;
//Represents a hackable property that is a number
public class HackableNumber : HackableProperty {
	//The number value
	public int value;

	//Converts the number value to a string
	public string getString()
	{
		return value.ToString();
	}

	//Copies the value of this object into another
	public void copy(HackableNumber property)
	{
		value = property.value;
		superCopy (property);
	}

	//Checks to see if a given object has the same value as this one
	public override bool isDifferent(HackableProperty property)
	{
		return (((HackableNumber)property).value != this.value);
	}
}