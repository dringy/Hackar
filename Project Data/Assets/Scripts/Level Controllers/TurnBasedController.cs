using UnityEngine;
using System.Collections;
//A controller that controls turn based elements
public class TurnBasedController : MonoBehaviour {
	//Contains one arraylists one for obstacles and one for mobiles
	private static ArrayList turnBasedElements;
	private static bool takeTurn; //Take turn represents if a turn request has been sent

	private static int index;

	//Instanciates the class
	void Awake () {
		turnBasedElements = new ArrayList ();
		takeTurn = false;
		index = 0;
	}

	//Adds a turn based obstacle
	public static void addTurnBasedElement(TurnBasedElement element)
	{
		turnBasedElements.Add (element);
	}

	//Removal function for a turn based mob
	public static void removeTurnBasedElement(TurnBasedElement element)
	{
		turnBasedElements.Remove (element);
	}

	//Called every fram
	public void Update()
	{
		//If there is a request to advance a turn and the player isn't being held then we advance the turns of all the turn based elements
		//If the player is being held the request remains until the player is released
		if (takeTurn)
		{
			if (index < turnBasedElements.Count)
			{
				((TurnBasedElement)turnBasedElements[index]).advanceTurn ();
				index++;
			}
			else
			{
				//Afterwards the request is retracted
				takeTurn = false;
				index = 0;
				//All turn based elements can observe the enviroment
				foreach(TurnBasedElement element in turnBasedElements)
				{
					element.takeEndOfTurnObservation();
				}
			}
		}
	}

	//Updates the request boolean
	public static void advanceTurn()
	{
		takeTurn = true;
	}
}
