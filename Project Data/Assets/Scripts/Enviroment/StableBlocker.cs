using UnityEngine;
using System.Collections;
//Represents the Stable Blocker enviroment
public class StableBlocker : Enviroment {
	//Stable blocker cannot be walked into
	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		return false;
	}

	//It can be walked ontop of
	public override bool canWalkOnTopOf()
	{
		return true;
	}
	
	public override bool walkOnTopOf()
	{
		return true;
	}

}
