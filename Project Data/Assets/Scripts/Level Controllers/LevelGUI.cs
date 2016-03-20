using UnityEngine;
using System.Collections;

/// <summary>
/// The Level GUI class that displays the Level stats and provides functions for hackable onjects to display their hack menu.
/// </summary>
public class LevelGUI : MonoBehaviour {
	//The level's levelStats objecy
	public LevelStats levelStats;

	//Boolean indicates if the hack gui should display
	private static bool displayHackGUI;
	//The hack object fot the hack gui
	private static HackableObject hackObject;

	//Various traits for the hack gui to use
	private static float hackGUIx;
	private static float nameX;
	private static float valueX;
	private static float hackGUIy;
	private static float hackGUIHeight;
	private static float hackGUIWidth = 200;
	private static float indvidualHeight = 20;

	//A copy of the original properties to be updated
	private static HackableProperty[] properties;
	//A referebce to the original properties to be used to count changes
	private static HackableProperty[] originalProperties;

	//Timed message strings and a countdown variable
	private static short timedMessageCountdown;
	private static string timedMessage;

	/// <summary>
	/// Start class used as a constructor.
	/// </summary>
	void Start () {
		displayHackGUI = false;
		timedMessageCountdown = 0;
		timedMessage = "";
	}

	/// <summary>
	/// The class that builds the gui
	/// </summary>
	void OnGUI()
	{
		//We create two labels for the hack and steps counters
		var topleftStyle = GUI.skin.GetStyle("Label");
		topleftStyle.alignment = TextAnchor.UpperLeft;
		GUI.Label (new Rect (0, 0, Screen.width, 30), levelStats.getHackString (), topleftStyle);
		GUI.Label (new Rect (0, 20, Screen.width, 30), levelStats.getStepString (), topleftStyle);
		GUI.Label (new Rect (0, 40, Screen.width, 30), PlayerController.getKeysString (), topleftStyle);

		//If the hack gui is on we display
		if (displayHackGUI)
		{
			//Here is the box
			GUI.Box (new Rect(hackGUIx, hackGUIy, hackGUIWidth, hackGUIHeight), hackObject.name);

			//Then we display each property one ontop of another
			int index = 0;
			foreach (HackableProperty property in properties)
			{
				//Height is calculated for all the property elements
				float propertyHeight = hackGUIy + (indvidualHeight * (index+1));
				//A label is created for the property name
				GUI.Label (new Rect(nameX, propertyHeight, 65, 20), property.name);

				//Then we get the property changing part which varrys depending on the hackable property
				Rect valueRectangle = new Rect(valueX, propertyHeight, 65, 20);
				if (property is HackableBool)
				{
					//HackableBools are represented as checkboxes
					((HackableBool) property).value = GUI.Toggle(valueRectangle, ((HackableBool) property).value, "");
				}
				else if (property is HackableString)
				{
					//HackableString are represented as text fields
					((HackableString) property).value = GUI.TextField(valueRectangle, ((HackableString) property).value);
				}
				else if (property is HackableNumber)
				{
					//HackableNumber are represented as text fields but it should only accept numbers
					string numberString = GUI.TextField(valueRectangle, ((HackableNumber) property).getString());
					int.TryParse(numberString, out ((HackableNumber) property).value); 
				}
				else if (property is HackableEnum)
				{
					//HackableEnums are buttons with content linked to that of the current value
					//Pressing the button changes the index thus changing the button content next frame
					if (GUI.Button(valueRectangle, ((HackableEnum) property).getValue()))
					{
						((HackableEnum) property).incrementIndex();
					}
				}

				index++;
			}

			//Next confirm and cancel buttons are added

			float buttonHeights = hackGUIy + (indvidualHeight * (properties.Length + 1));
			if (GUI.Button (new Rect(nameX, buttonHeights, 70, 20), "Confirm"))
			{
				//Confirm button checks differences but only changes them if there are hacks available
				if (levelStats.attemptHacks(calcuateDifferences()))
				{
					hackObject.updateProperties(properties);
				}
				else
				{
					displayTimedMessage("Exceeded Hack Limit");
				}
				//Hack menu closes afterwards
				displayHackGUI = false;
			}
			if (GUI.Button (new Rect(valueX, buttonHeights, 65, 20), "Cancel"))
			{
				//Hack menu closes when cancel is pressed
				displayHackGUI = false;
			}

		}

		if (timedMessageCountdown > 0)
		{
			//Timed message is just a label in the center it decrements the counter until the this code no longer runs
			var centralStyle = GUI.skin.GetStyle("Label");
			centralStyle.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height/2), timedMessage, centralStyle);
			timedMessageCountdown --;
		}
	}

	/// <summary>
	/// Sets us the data for the hack gui for the hack objext supplied.
	/// </summary>
	/// <param name="hackObject">Hack object.</param>
	public static void displayGUI(HackableObject hackObject)
	{
		//New positions are taken
		float newHackGUIx = Input.mousePosition.x;
		float newHackGUIy = Screen.height - Input.mousePosition.y;

		//On Click command should be ignored if done inside the currently displayed area
		//Function just returns leaving the gui to not be updated
		if (LevelGUI.displayHackGUI)
		{

			if (((newHackGUIx >= hackGUIx) && (newHackGUIx <= (hackGUIx + hackGUIWidth))) &&
			    ((newHackGUIy >= hackGUIy) && (newHackGUIy <= (hackGUIy + hackGUIHeight))))
			{
				return;
			}
		}

		//GUI is disabled for a bit
		displayHackGUI = false;

		//Data is transfered across into the object attributes
		LevelGUI.hackObject = hackObject;
		hackGUIx = newHackGUIx;
		hackGUIy = newHackGUIy;

		//Properties are stored
		originalProperties = hackObject.getPropeties ();
		//Copy properties array is created
		properties = new HackableProperty[originalProperties.Length];

		//Creates a property copy for each type of hackable property
		int index = 0;
		foreach(HackableProperty property in originalProperties)
		{
			if (property is HackableBool)
			{
				HackableBool copyProperty = new HackableBool();
				copyProperty.copy ((HackableBool) property);
				properties[index] = copyProperty;
			}
			else if (property is HackableString)
			{
				HackableString copyProperty = new HackableString();
				copyProperty.copy ((HackableString) property);
				properties[index] = copyProperty;
			}
			else if (property is HackableNumber)
			{
				HackableNumber copyProperty = new HackableNumber();
				copyProperty.copy ((HackableNumber) property);
				properties[index] = copyProperty;
			}
			else if (property is HackableEnum)
			{
				HackableEnum copyProperty = new HackableEnum();
				copyProperty.copy ((HackableEnum) property);
				properties[index] = copyProperty;
			}
			index++;
		}

		//Height is caluclated and stored
		hackGUIHeight = 10 + indvidualHeight + ((properties.Length + 1) * indvidualHeight);

		//Coordinates are moved if the menu appears off the screen

		if ((hackGUIx + hackGUIWidth) >= Screen.width)
		{
			hackGUIx = Screen.width - hackGUIWidth;
		}
		if ((hackGUIy + hackGUIHeight) >= Screen.height)
		{
			hackGUIy = Screen.height - hackGUIHeight;
		}

		//precalcuated values are stored
		nameX = hackGUIx + 10;
		valueX = hackGUIx + 100;

		//GUI is now set to be displayed
		displayHackGUI = true;
	}

	/// <summary>
	/// Calcuates the differences between the updated properties and the original properties
	/// </summary>
	/// <returns>The number of differences.</returns>
	private short calcuateDifferences()
	{
		short numberOfDifferences = 0;
		//Goes through each property and checks to see if they're different
		for (int index = 0; index < properties.Length; index++)
		{
			if(originalProperties[index].isDifferent(properties[index]))
			{
				numberOfDifferences++;
			}
		}
		return numberOfDifferences;
	}

	/// <summary>
	/// Displays a temporary message that will dissapear in 175 frames.
	/// </summary>
	/// <param name="message">Message to be displayed</param>
	public static void displayTimedMessage(string message)
	{
		//Message is set and countdown time change triggers code in onGUI() to run
		timedMessage = message;
		timedMessageCountdown = 225;
	}

	/// <summary>
	/// Hides the hack menu.
	/// </summary>
	public static void disableGUI()
	{
		displayHackGUI = false;
	}
	
}
