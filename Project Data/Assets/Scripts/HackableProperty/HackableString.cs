using UnityEngine;
using System.Collections;
//Represents a hackable string
public class HackableString : HackableProperty {
	//The string property
	public string value;

	//Copies the property into this property
	public void copy(HackableString property)
	{
		value = property.value;
		superCopy (property);
	}

	//Checks to see if the properties are different
	public override bool isDifferent(HackableProperty property)
	{
		return (!((HackableString)property).value.Equals(this.value));
	}
}
