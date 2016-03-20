using UnityEngine;
using System.Collections;
//Represents a property that can be changed using the IN-GAME Hacking
public abstract class HackableProperty {
	public string name; //The name of the property

	//Coppies data from a property into this object
	protected void superCopy(HackableProperty property)
	{
		this.name = property.name;
	}

	//Checks if the hackable property is different to this one
	public abstract bool isDifferent(HackableProperty property);


}
