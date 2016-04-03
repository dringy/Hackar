
using UnityEngine;
using System.Collections;
/// <summary>
/// The enviromental object for the goal
/// </summary>
public class Goal : Enviroment {

	//Instanciates the class
	void Start()
	{
		//Calls enviroment superfunction
		base.Start ();
		//Adjusts the scale and colour to make it look cool
		transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		gameObject.GetComponent<Renderer>().material.color = Color.yellow;
	}

	//Calls every frame
	void Update()
	{
		//The goal has to rotate the cube
		transform.Rotate (new Vector3 (0, 50 * Time.deltaTime, 0));
	}


	//Goal defaults is that it can be walked to but can't be walked on top of
	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		return true;
	}
	
	public override void preWalk(MobileEnviroment enviroment)
	{
		//When you walk to the goal the main menu is loaded
		Application.LoadLevel ("MainMenu");
	}
	
	public override bool canWalkOnTopOf()
	{
		return false;
	}
	
	public override bool walkOnTopOf()
	{
		return false;
	}
}
