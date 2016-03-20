using UnityEngine;
using System.Collections;
//Represents a trigger with two parts an On and Off
public abstract class TwoStateTrigger : MonoBehaviour {
	//The on trigger function
	public abstract void triggerOn();

	//The off trigger function
	public abstract void triggerOff();
}
