using UnityEngine;
using System.Collections;

public class GoalReplica : MonoBehaviour {

	//Instanciates the class
	void Start()
	{
		//Adjusts the scale and colour to make it look cool
		transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		gameObject.renderer.material.color = Color.yellow;
	}
	
	//Calls every frame
	void Update()
	{
		//The goal has to rotate the cube
		transform.Rotate (new Vector3 (0, 50 * Time.deltaTime, 0));
	}
}
