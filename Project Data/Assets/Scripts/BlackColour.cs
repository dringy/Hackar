using UnityEngine;
using System.Collections;
/// <summary>
///sets the colour to black.
/// </summary>
public class BlackColour : MonoBehaviour {
	// Use this for initialization
	void Start () {
		renderer.material.color = Color.black;
	}
}
