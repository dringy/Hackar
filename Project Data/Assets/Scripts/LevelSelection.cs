using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour {
	ArrayList levelName;
	ArrayList levelDesc;
	ArrayList levelFileName;

	bool showLevel;
	int levelNo;

	//Used for instanciation
	void Start()
	{
		levelName = new ArrayList ();
		levelDesc = new ArrayList ();
		levelFileName = new ArrayList ();

		levelName.Add ("The locked door");
		levelDesc.Add ("How do we get through a locked door?");
		levelFileName.Add ("Level1");

		levelName.Add ("Limited Resources");
		levelDesc.Add ("You're no superman, know your limits.");
		levelFileName.Add ("Level2");

		levelName.Add ("The Blockers");
		levelDesc.Add ("Blockers aren't good at maths but sure are good at blocking");
		levelFileName.Add ("Level3");

		levelName.Add ("The Wizard");
		levelDesc.Add ("Wizards like to teleport you but they're still human - they're just super mean.");
		levelFileName.Add ("Level4");

		levelName.Add ("The Mastermind");
		levelDesc.Add ("Changing how blockers and wizards behave is using your head");
		levelFileName.Add ("Level5");

		levelName.Add ("Keys");
		levelDesc.Add ("How do we get through a locked door? - Use a key!");
		levelFileName.Add ("Level6");

		levelName.Add ("Jail Break");
		levelDesc.Add ("These guards have got you on lock down, good thing they're stupid");
		levelFileName.Add ("Level7");


	}
	
	//The GUI elements are build here
	void OnGUI()
	{
		for (int i = 0; i < levelName.Count; i++)
		{
			if (GUI.Button (new Rect(10 + (100*(i%10)), 10 + (100*(i/10)), 100, 100), (i+1).ToString ()))
			{
				showLevel = true;
				levelNo = i;
			}
		}

		if (showLevel)
		{
			GUI.Box (new Rect(Screen.width - 200, 10, 200, Screen.height - 10), (string)levelName[levelNo]);
			GUI.Label (new Rect(Screen.width - 200, 50, 200, Screen.height - 50), (string)levelDesc[levelNo]);
			if (GUI.Button (new Rect(Screen.width - 100, Screen.height - 100, 50, 50), "Play"))
			{
				Application.LoadLevel((string)levelFileName[levelNo]);
			}
		}
	}
}
