using UnityEngine;
using System.Collections;
/// <summary>
/// Represents a hackable part of a key
/// </summary>
public class HackableKey : HackableColour {
	//The key name for use with doors
	public string keyID;
	private HackableString keyIDProperty;

	// Use this for initialization
	void Start () {
		keyIDProperty = new HackableString ();
		keyIDProperty.name = "Key Name";
		keyIDProperty.value = keyID;

		setColourProperty ();
		updateColour ();
	}

	//Properties are name a colour
	public override HackableProperty[] getPropeties()
	{
		HackableProperty[] hackableProperties = new HackableProperty[1];
		hackableProperties [0] = keyIDProperty;
		hackableProperties [1] = colourProperty;
		return hackableProperties;
	}
	
	public override void updateProperties(HackableProperty[] properties)
	{
		keyIDProperty = (HackableString)properties [0];
		colourProperty = (HackableEnum)properties [1];
		updateColour ();
	}

	public string getKeyID()
	{
		return keyIDProperty.value;
	}
}
