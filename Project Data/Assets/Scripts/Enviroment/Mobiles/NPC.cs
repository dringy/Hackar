using UnityEngine;
using System.Collections;
/// <summary>
/// Represents a standard npc.
/// </summary>
public class NPC : MobileEnviroment {
	//What the NPC does when it sees a player
	public enum playerAction {Kill, Teleport, Ignore};
	public playerAction action;
	//How far it can see - 0 means it is blind
	public int sightLength;
	//How far it can hear - 0 means it is deaf
	public int soundLength;
	//It's teleporting cords
	public float teleX;
	public float teleY;
	public float teleZ;

	//The advance turn function called by the turn based controller
	public virtual void advanceTurn()
	{
		return;
	}

	public void checkForPlayer()
	{
		//The npc only does something if it doesn't ignore the player
		if (action != playerAction.Ignore)
		{
			PlayerController player;
			//It then attempts to spot a player given it's direction, sightlength and spotlength
			//It calls EnviromentMap functions for this
			switch (directionFacing)
			{
			case direction.posotiveX:
			{
				player = EnviromentMap.findPlayerLookingPosotiveX(transform.position.x, transform.position.y,
				                                                  transform.position.z, sightLength, soundLength);
				break;
			}
			case direction.negativeX:
			{
				player = EnviromentMap.findPlayerLookingNegativeX(transform.position.x, transform.position.y,
				                                                  transform.position.z, sightLength, soundLength);
				break;
			}
			case direction.posotiveZ:
			{
				player = EnviromentMap.findPlayerLookingPosotiveZ(transform.position.x, transform.position.y,
				                                                  transform.position.z, sightLength, soundLength);
				break;
			}
			default:
			{
				player = EnviromentMap.findPlayerLookingNegativeZ(transform.position.x, transform.position.y,
				                                                  transform.position.z, sightLength, soundLength);
				break;
			}
			}
			
			//If a player was found we can then act
			if(player != null)
			{
				//If the kill action is present then we reset the level
				if (action == playerAction.Kill)
				{
					Application.LoadLevel (Application.loadedLevelName);
				}
				//If the teleport ation is present we display a times message and teleport the player
				else
				{
					LevelGUI.displayTimedMessage(this.name + " spotted you and teleported you.");
					//player.triggerWalkAwayDueToTeleport ();
					EnviromentMap.moveTo (teleX, teleY, teleZ, player);
				}
			}
		}
	}
}
