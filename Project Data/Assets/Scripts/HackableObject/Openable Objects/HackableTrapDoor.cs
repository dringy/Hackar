using UnityEngine;
using System.Collections;
/// <summary>
/// Represents a hackable trap door.
/// </summary>
public class HackableTrapDoor : HackableOpenableObject {
	//A reference to the enviroment part
	public TrapDoor enviromentPart;

	//instanciates the objecy
	void Start () {
		isOpening = false;
		isOpen = false;
		
		setColourProperty ();
		updateColour ();
	}

	//gets the hackable properties for the hack menu
	public override HackableProperty[] getPropeties()
	{
		//Only colour can be changed
		HackableProperty[] properties = new HackableProperty[1];
		properties [0] = colourProperty;
		return properties;
	}

	//Updates the properties from the hack menu
	public override void updateProperties(HackableProperty[] properties)
	{
		colourProperty = (HackableEnum) properties [0];
		updateColour ();
	}

	//When the trap door starts opening, any mob on top is frozen
	protected override void openingTrigger()
	{
		EnviromentMap.freezeMobileOntop (transform.position.x, enviromentPart.getOriginalY (), transform.position.z);
	}

	//When a trap door opens the mob ontop is killed
	protected override void openTrigger()
	{
		EnviromentMap.killFrozenMobiles();
	}
}
