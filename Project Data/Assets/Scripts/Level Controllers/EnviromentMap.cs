using UnityEngine;
using System.Collections;

/// <summary>
/// A mainly static class for keeping track of enviromental objects in a level. The map has limits in all directions and provides functions to
/// move enviromental object around.
/// </summary>
public class EnviromentMap : MonoBehaviour {
	//These are the upper and lower limits in all three directions
	public int xUpperLimit;
	public int yUpperLimit;
	public int zUpperLimit;
	public int xLowerLimit;
	public int yLowerLimit;
	public int zLowerLimit;

	//Static versions of all the lower limits
	private static int xStaticLowerLimit;
	private static int yStaticLowerLimit;
	private static int zStaticLowerLimit;

	//The actual map is a 3d array of Enviroment objects
	private static Enviroment[ , , ] map;

	private static ArrayList frozenMobiles;

	/// <summary>
	/// The instanciation of the object, Awake() is used instead of Start() to ensure it goes before all the enviromental versions of Start()
	/// </summary>
	void Awake () {
		xLowerLimit++;
		yLowerLimit++;
		zLowerLimit++;
		xUpperLimit++;
		yUpperLimit++;
		zUpperLimit++;
		map = new Enviroment[xLowerLimit + xUpperLimit + 1, yLowerLimit + yUpperLimit + 1, zLowerLimit + zUpperLimit + 1];
		xStaticLowerLimit = xLowerLimit;
		yStaticLowerLimit = yLowerLimit;
		zStaticLowerLimit = zLowerLimit;
		frozenMobiles = new ArrayList();
	}

	/// <summary>
	/// Adds an enviromental object to the enviroment map.
	/// </summary>
	/// <param name="xPos">X position. The x position of the enviroment</param>
	/// <param name="yPos">Y position. The y position of the enviroment</param>
	/// <param name="zPos">Z position. The z position of the enviroment</param>
	/// <param name="xScale">X scale. The x scale of the enviroment</param>
	/// <param name="yScale">Y scale. The y scale of the enviromen</param>
	/// <param name="zScale">Z scale. The z scale of the enviromen</param>
	/// <param name="enviroment">Enviroment. The enviroment to be added.</param>
	public static void addToMap(float xPos, float yPos, float zPos, float xScale, float yScale, float zScale, Enviroment enviroment)
	{
		//Scales are halved to get the scale on each side
		xScale /= 2f;
		yScale /= 2f;
		zScale /= 2f;
		//Scales are shortened to adjust for rounding
		xScale -= 0.5f;
		yScale -= 0.5f;
		zScale -= 0.5f;

		//enviroment is added to map in all positions it spans
		for (float y = yPos - yScale; y <= (yPos + yScale); y++)
		{
			for (float x = xPos - xScale; x <= (xPos + xScale); x++)
			{
				for (float z = zPos - zScale; z <= (zPos + zScale); z++)
				{
					//As map starts at 0 we have to adjust the positon by adding the lower limts of the map
					setEnviroment (x, y, z, enviroment);
				}
			}
		}


	}

	/// <summary>
	/// Moves the movile enviroment to the given coordinate. Coordinates are in space form not in map reference form.
	/// </summary>
	/// <param name="x">The x coordinate. The new x coordinate of the mobile enviromet</param>
	/// <param name="y">The y coordinate. The new y coordinate of the mobile enviromet</param>
	/// <param name="z">The z coordinate. The new z coordinate of the mobile enviromet</param>
	/// <param name="mobile">Mobile. The mobile enviroment to be moved.</param>
	public static bool moveTo(float x, float y, float z, MobileEnviroment mobile)
	{
		//The enviroment that the mobile will move into
		Enviroment enviroment = getEnviromentFromCords(x, y, z);
		//The enviroment bellow the enviroment that the mobile will move into
		Enviroment floorEnviroment = getEnviromentFromCords(x, y - 1, z);

		//We first check if the enviroment can be moved into
		if (canWalkTo(enviroment, mobile))
		{
			//If we can we then check if the floor enviroment can be walked ontop of.
			if (canWalkOnTopOf(floorEnviroment))
			{
				//If the attempt works we then attempt to walk ontop of the floor enviroment
				if (walkOnTopOf(floorEnviroment))
				{
					//We then move the mobile into the new location
					mobile.updateMovement(x - mobile.transform.position.x, y - mobile.transform.position.y,
					                           z - mobile.transform.position.z, enviroment);

					return true;
				}
			}

		}
		return false;
	}

	/// <summary>
	/// Freezes any movile enviroment above the given coordinates
	/// </summary>
	/// <param name="x">The x coordinate</param>
	/// <param name="y">The y coordinate</param>
	/// <param name="z">The z coordinate</param>
	public static void freezeMobileOntop(float x, float y, float z)
	{
		Enviroment enviromentOnTop = getEnviromentFromCords (x, y + 1, z);
		//Enviroment is not guaranteed to exist or be a mobile enviroment
		if (enviromentOnTop != null)
		{
			if (enviromentOnTop is MobileEnviroment)
			{
				((MobileEnviroment) enviromentOnTop).freezeMob ();
				//frozen mobs are kept track of
				frozenMobiles.Add (enviromentOnTop);
			}
		}
	}

	/// <summary>
	/// Kills all frozen mobile enviroments on this level.
	/// </summary>
	public static void killFrozenMobiles()
	{
		//Calls kill function on each recorded frozen mob
		foreach(Enviroment mobile in frozenMobiles)
		{
			((MobileEnviroment)mobile).killMob();
		}
		//Frozen list is reset
		frozenMobiles = new ArrayList();
	}

	/// <summary>
	/// Finds a player enviroment given a spotter is faccing in the posotive x direction.
	/// </summary>
	/// <returns>The player spotted</returns>
	/// <param name="xCord">X cord of the spotter</param>
	/// <param name="yCord">Y cord of the spotter</param>
	/// <param name="zCord">Z cord of the spotter</param>
	/// <param name="sightLength">Sight length - how far the spotter can see</param>
	/// <param name="soundLength">Sound length - how far the spotter can hear</param>
	public static PlayerController findPlayerLookingPosotiveX(float xCord, float yCord, float zCord, int sightLength, int soundLength)
	{
		//Firstly cords are converted to map indexes
		int xStart = (int)(xCord + xStaticLowerLimit) + 1;
		int y = (int)(yCord + yStaticLowerLimit);
		int z = (int)(zCord + zStaticLowerLimit);
		//We start at the spotter's x cord and move in the posotive x direction until we reach the sight length
		for(int x = xStart; x <= xStart + sightLength; x++)
		{
			try
			{
				//If we find a player then we return it
				if (map[x, y, z] is PlayerController)
				{
					return (PlayerController) map[x, y, z];
				}
				else if (doesBlock(map[x, y, z]))
				{
					break;
				}
			}
			//If the spotter can see past the boundaries then this catch statement will trigger
			catch (System.IndexOutOfRangeException e){}
		}
		//If the spotter cannot see the player we check if it can hear the player
		return findPlayerBySound (xCord, yCord, zCord, soundLength);
	}

	/// <summary>
	/// Finds a player enviroment given a spotter is faccing in the negative x direction.
	/// </summary>
	/// <returns>The spotted playerController</returns>
	/// <param name="xCord">X cord of the spotter</param>
	/// <param name="yCord">Y cord of the spotter</param>
	/// <param name="zCord">Z cord of the spotter</param>
	/// <param name="sightLength">Sight length - how far the spotter can see</param>
	/// <param name="soundLength">Sound length - how far the spotter can hear</param>
	public static PlayerController findPlayerLookingNegativeX(float xCord, float yCord, float zCord, int sightLength, int soundLength)
	{
		//Firstly cords are converted to map indexes
		int xStart = (int)(xCord + xStaticLowerLimit) - 1;
		int y = (int)(yCord + yStaticLowerLimit);
		int z = (int)(zCord + zStaticLowerLimit);
		//We start at the spotter's x cord and move in the negative x direction until we reach the sight length
		for(int x = xStart; x >= xStart - sightLength; x--)
		{
			try
			{
				//If we find a player then we return it
				if (map[x, y, z] is PlayerController)
				{
					return (PlayerController) map[x, y, z];
				}
				else if (doesBlock(map[x, y, z]))
				{
					break;
				}
			}
			//If the spotter can see past the boundaries then this catch statement will trigger
			catch (System.IndexOutOfRangeException e){}
		}
		//If the spotter cannot see the player we check if it can hear the player
		return findPlayerBySound (xCord, yCord, zCord, soundLength);
	}

	/// <summary>
	/// Finds a player enviroment given a spotter is faccing in the posotive z direction.
	/// </summary>
	/// <returns>The spotted playerController</returns>
	/// <param name="xCord">X cord of the spotter</param>
	/// <param name="yCord">Y cord of the spotter</param>
	/// <param name="zCord">Z cord of the spotter</param>
	/// <param name="sightLength">Sight length - how far the spotter can see</param>
	/// <param name="soundLength">Sound length - how far the spotter can hear</param>
	public static PlayerController findPlayerLookingPosotiveZ(float xCord, float yCord, float zCord, int sightLength, int soundLength)
	{
		//Firstly cords are converted to map indexes
		int zStart = (int)(zCord + zStaticLowerLimit) + 1;
		int y = (int)(yCord + yStaticLowerLimit);
		int x = (int)(xCord + xStaticLowerLimit);
		//We start at the spotter's z cord and move in the posotive z direction until we reach the sight length
		for(int z = zStart; z <= zStart + sightLength; z++)
		{
			try
			{
				//If we find a player then we return it
				if (map[x, y, z] is PlayerController)
				{
					return (PlayerController) map[x, y, z];
				}
				else if (doesBlock(map[x, y, z]))
				{
					break;
				}
			}
			//If the spotter can see past the boundaries then this catch statement will trigger
			catch (System.IndexOutOfRangeException e){}
		}
		//If the spotter cannot see the player we check if it can hear the player
		return findPlayerBySound (xCord, yCord, zCord, soundLength);
	}

	/// <summary>
	/// Finds a player enviroment given a spotter is faccing in the negative z direction.
	/// </summary>
	/// <returns>The spotted playerController</returns>
	/// <param name="xCord">X cord of the spotter</param>
	/// <param name="yCord">Y cord of the spotter</param>
	/// <param name="zCord">Z cord of the spotter</param>
	/// <param name="sightLength">Sight length - how far the spotter can see</param>
	/// <param name="soundLength">Sound length - how far the spotter can hear</param>
	public static PlayerController findPlayerLookingNegativeZ(float xCord, float yCord, float zCord, int sightLength, int soundLength)
	{
		//Firstly cords are converted to map indexes
		int zStart = (int)(zCord + zStaticLowerLimit) - 1;
		int y = (int)(yCord + yStaticLowerLimit);
		int x = (int)(xCord + xStaticLowerLimit);
		//We start at the spotter's z cord and move in the negative z direction until we reach the sight length
		for(int z = zStart; z >= zStart - sightLength; z--)
		{
			try
			{
				//If we find a player then we return it
				if (map[x, y, z] is PlayerController)
				{
					return (PlayerController) map[x, y, z];
				}
				else if (doesBlock(map[x, y, z]))
				{
					break;
				}
			}
			//If the spotter can see past the boundaries then this catch statement will trigger
			catch (System.IndexOutOfRangeException e){}
		}
		//If the spotter cannot see the player we check if it can hear the player
		return findPlayerBySound (xCord, yCord, zCord, soundLength);
	}

	/// <summary>
	/// Checks to see if a spotter can hear a player, if it can it returns the player's PlayerController class
	/// </summary>
	/// <returns>The playerController class of the spotted player</returns>
	/// <param name="xCord">X cord of the spotter</param>
	/// <param name="yCord">Y cord of the spotter</param>
	/// <param name="zCord">Z cord of the spotter</param>
	/// <param name="soundLength">Sound length - how far the spotter can hear</param>
	private static PlayerController findPlayerBySound(float xCord, float yCord, float zCord, int soundLength)
	{
		//Firstly cords are converted to map indexes
		int xStart = (int)(xCord + xStaticLowerLimit);
		int zStart = (int)(zCord + zStaticLowerLimit);
		int y = (int)(yCord + yStaticLowerLimit);
		//We use the sound length to determine the lower and upper limit of x and z
		for (int x = xStart - soundLength; x <= xStart + soundLength; x++)
		{
			for (int z = zStart - soundLength; z <= zStart + soundLength; z++)
			{
				try{
					//If we find a player then we return it
					if (map[x, y, z] is PlayerController)
					{
						return (PlayerController) map[x, y, z];
					}
				}
				//If the spotter can hear past the boundaries then this catch statement will trigger
				catch (System.IndexOutOfRangeException e){};
			}
		}
		//If the spotter cannot hear the player we return null
		return null;
	}

	//removes an enviroment given indexes
	public static void removeEnviroment(float x, float y, float z)
	{
		setEnviroment (x, y, z, null);
	}

	//Sets the enviroment given cords and the enviroment
	//To be used when moving a 1x1 mobile
	public static void setEnviroment(float x, float y, float z, Enviroment enviroment)
	{
		map [(int)(x + xStaticLowerLimit), (int)(y + yStaticLowerLimit), (int)(z + zStaticLowerLimit)] = enviroment;
	}

	/// <summary>
	/// Gets an enviroment from the map using real world coordinates
	/// </summary>
	/// <returns>The enviroment in the map.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	private static Enviroment getEnviromentFromCords(float x, float y, float z)
	{
		return map [(int)(x + xStaticLowerLimit), (int)(y + yStaticLowerLimit), (int)(z + zStaticLowerLimit)];
	}

	/// <summary>
	/// Sets the enviroment in the map using real world coordinates and a supplied enviroment.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	/// <param name="enviroment">Enviroment to be placed in the map.</param>
	private static void setEnviromentFromCords(float x, float y, float z, Enviroment enviroment)
	{
		map [(int)(x + xStaticLowerLimit), (int)(y + yStaticLowerLimit), (int)(z + zStaticLowerLimit)] = enviroment;
	}

	/// <summary>
	/// Checks if the mobile can walk into the enviroment
	/// </summary>
	/// <returns><c>true</c>, if the mobile can walk to it <c>false</c> otherwise.</returns>
	/// <param name="enviroment">Enviroment.</param>
	private static bool canWalkTo(Enviroment enviroment, MobileEnviroment mobile)
	{
		if (enviroment == null)
		{
			//If there is nothing there then you can walk to it
			return true;
		}
		else
		{
			return enviroment.canWalkTo(mobile);
		}
	}

	/// <summary>
	/// Checks if the mobile can walk ontop of the enviroment
	/// </summary>
	/// <returns><c>true</c>, if the mobile can walk on top of it <c>false</c> otherwise.</returns>
	/// <param name="enviroment">Enviroment.</param>
	private static bool canWalkOnTopOf(Enviroment enviroment)
	{
		if (enviroment == null)
		{
			//If there is nothing there then you can't walk ontop of it
			return false;
		}
		else
		{
			return enviroment.canWalkOnTopOf();
		}
	}

	/// <summary>
	/// Attempts to walk ontop of enviroment
	/// </summary>
	/// <returns><c>true</c> If the attempt was successful <c>false</c> otherwise.</returns>
	/// <param name="enviroment">Enviroment.</param>
	private static bool walkOnTopOf(Enviroment enviroment)
	{
		if (enviroment == null)
		{
			//If there is nothing there then you can't walk ontop of it
			return false;
		}
		else
		{
			return enviroment.walkOnTopOf();
		}
	}

	/// <summary>
	/// Walks away from an eviroment
	/// </summary>
	/// <param name="enviroment">Enviroment.</param>
	private static void walkAwayFrom(Enviroment enviroment)
	{
		if (enviroment != null)
		{
			enviroment.walkAwayFrom();
		}
	}

	//Checks if an enviroment blocks sight
	private static bool doesBlock(Enviroment enviroment)
	{
		//If there is nothing there then spotter can see throught it
		if (enviroment == null)
		{
			return false;
		}
		else
		{
			return enviroment.doesBlock ();
		}
	}


}
