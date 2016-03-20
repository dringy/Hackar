using UnityEngine;
using System.Collections;
/// <summary>
/// Represents a key.
/// </summary>
public class Key : Enviroment {
	//Contains hackable part
	public HackableKey hackablePart;

	private bool isCollected;

	public void Start()
	{
		base.Start ();
		isCollected = false;
		//keys are shrunk smaller
		transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
	}

	//Keys rotate slowly
	void Update()
	{
		transform.Rotate (new Vector3(0f, 45 * Time.deltaTime, 0f));
	}

	//They can be walked to
	public override bool canWalkTo(MobileEnviroment enviroment)
	{
		return true;
	}

	//They disapear when walked on
	public override void preWalk(MobileEnviroment enviroment)
	{
		gameObject.SetActive (false);
	}

	//The key is added to the player
	public override void postWalk(MobileEnviroment enviroment)
	{
		if (!isCollected)
		{
			isCollected = true;
			PlayerController.addKey (hackablePart.getKeyID ());
		}
	}
	
	public override bool canWalkOnTopOf()
	{
		return false;
	}
	
	public override bool walkOnTopOf()
	{
		return false;
	}
	
	public override bool doesBlock()
	{
		return false;
	}

}
