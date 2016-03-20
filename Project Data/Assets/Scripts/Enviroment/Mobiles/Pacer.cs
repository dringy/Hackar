using UnityEngine;
using System.Collections;

public class Pacer : NPC {
	public bool moveInX; //if true the mob moves in x direction else it moves in z direction
	public bool movePosotive; //If true it moves in posotive direction else it moves in negative direction
	public int lowerLimit; //The lower limit of the path
	public int upperLimit; //The upper limit of the path
	
	//Advances the turn, it figures out where to move next
	public override void advanceTurn()
	{
		if (movePosotive)
		{
			//We first check that the position is at the limit if it isn't then the mob attempts to move
			//If the mob can't move past or if they have reached their limit then it revereses direction
			if (getPosition() >= upperLimit || !move (1))
			{
				movePosotive = false;
				invertDirection();
			}
		}
		else
		{
			//We first check that the position is at the limit if it isn't then the mob attempts to move
			//If the mob can't move past or if they have reached their limit then it revereses direction
			if (getPosition() <= lowerLimit || !move (-1))
			{
				movePosotive = true;
				invertDirection();
			}
		}
		base.advanceTurn ();
	}
	
	//Moves the mob
	private bool move(short factor)
	{
		if (moveInX)
		{
			return movePosotiveX(factor);
		}
		else
		{
			return movePosotiveZ(factor);
		}
	}
	
	//Gets the position based on the mobs directinal movement
	private float getPosition()
	{
		if (moveInX)
		{
			return transform.position.x;
		}
		else
		{
			return transform.position.z;
		}
	}

	//Changes the pacer's direction by 180 degrees
	private void invertDirection()
	{
		//Uses a simple switch statement and calls the update function
		switch (directionFacing)
		{
		case direction.posotiveX:
		{
			updateDirectionFacing(direction.negativeX);
			break;
		}
		case direction.posotiveZ:
		{
			updateDirectionFacing(direction.negativeZ);
			break;
		}
		case direction.negativeX:
		{
			updateDirectionFacing(direction.posotiveX);
			break;
		}
		default:
		{
			updateDirectionFacing(direction.posotiveZ);
			break;
		}
		}
	}
}
