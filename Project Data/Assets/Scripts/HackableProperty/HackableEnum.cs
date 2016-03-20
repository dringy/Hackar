using UnityEngine;
using System.Collections;
//Represents the hackable property which works like an enum
public class HackableEnum : HackableProperty {
	//The enum is represented by an ArrayList and an index
	private ArrayList valueList;
	private int valueIndex;

	//Instanciated the object
	public HackableEnum()
	{
		valueList = new ArrayList ();
		valueIndex = 0;
	}

	//Adds a string to the enum
	public void addString(string value)
	{
		valueList.Add (value);
	}

	//Sets the enum index
	public void setIndex(int index)
	{
		valueIndex = index;
	}

	//Sets the value by searching the arraylist for that value
	public void setValue(string value)
	{
		valueIndex = valueList.LastIndexOf (value);
		if (valueIndex < 0)
		{
			valueIndex = 0;
		}
	}

	//Increments the index in the arraylist - Changing value
	public void incrementIndex()
	{
		if (++valueIndex >= valueList.Count)
		{
			valueIndex = 0;
		}
	}

	//Decrements the index in the arraylist - Changing value
	public void decrementIndex()
	{
		if (--valueIndex < 0)
		{
			valueIndex = valueList.Count - 1;
		}
	}

	//Gets the value that the index points too
	public string getValue()
	{
		return (string) valueList[valueIndex];
	}

	//Copies the contents of the property into a new property
	public void copy(HackableEnum property)
	{
		valueList = property.valueList;
		valueIndex = property.valueIndex;
		superCopy (property);
	}

	//Checks to see if this property has different contents to that of a given object of the same class
	public override bool isDifferent(HackableProperty property)
	{
		return (((HackableEnum)property).valueIndex != this.valueIndex);
	}
}
