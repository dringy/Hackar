using UnityEngine;
using System.Collections;
//Represents the Main Menu of the Game
public class MainMenuGUI : MonoBehaviour {
	private bool displayHowTo; //Indicates if the how to play screen should appear
	private string howTo; //The string for how to play

	//Used for instanciation
	void Start()
	{
		displayHowTo = false;
		howTo = "Welcome To Hackar! A puzzle adventure game where you must reach the goal (yellow spinng cube) of each level to complete it." +
						" In the game you can hack into the enviroment to change the properties of things in the world to make level completable however" +
						" some levels have a hack limit or a step limit or both.\n\nYou can use WASD or the arrow keys to move arround. You can use Q and E" +
						" to rotate the camera. To hack into the enviroment Left Click on the item you wish to hack to bring up the hack menu. " +
						"Pressing Tab will dismiss the hack menu, pressing R will reset the level and U will undo your last action (Undo to be implemented). " +
						"This game is mainly turn based so everytime you move other mobs will move, you may press space to wait (skip your turn). Finally you can press Escape to return to the main menu.";
	}

	//The GUI elements are build here
	void OnGUI()
	{
		//Rectangle is used the same size as the screen
		Rect box = new Rect (0, 0, Screen.width, Screen.height);
		//if the how-to-play screen is being displayed then a button containing the text is places and a reutrn button to the main menu
		if (displayHowTo)
		{
			var middleStyle = GUI.skin.GetStyle("Label");
			middleStyle.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 2), howTo, middleStyle);

			if (GUI.Button (new Rect(Screen.width/2 - 35, Screen.height/2, 70, 70), "Return"))
			{
				displayHowTo = false;
			}
		}
		else //if the main menu should display
		{
			//We have 4 buttons, 3 levels and a how-to menu
			Rect buttonBox1 = new Rect ((Screen.width / 2) - 150, (Screen.height / 8), 300, 80);
			Rect buttonBox2 = new Rect ((Screen.width / 2) - 150, (Screen.height / 8) + 160, 300, 80);

			//Uses LoadLevel to simply load in the new levels

			GUI.Box (box, "Hackar");
			if (GUI.Button (buttonBox1, "Play"))
			{
				Application.LoadLevel("LevelSelect");
			}
			if (GUI.Button (buttonBox2, "How To Play"))
			{
				displayHowTo = true;
			}
		}
	}
}
