using UnityEngine;
using System.Collections;
/// <summary>
/// Represents a hackable object that can be opened
/// </summary>
public abstract class HackableOpenableObject : HackableColour {
	//Lists all the directions the door can open - vanish means the door just disapears when it is opened
	public enum movement {posotiveX, posotiveY, posotiveZ, negativeX, negativeY, negativeZ, vanish};
	public movement openMovement;

	//the speed the door opens and closes
	private float speed = 2.5f;

	//these values control the opening and closing of the door
	protected bool isOpening;
	protected bool isClosing;
	protected bool isOpen;
	protected float limit;

	//Called every frame
	void Update () {
		//if the door is opening we then open it some more
		if(isOpening)
		{
			//if it has finished opening we update the variables, trigger any event and release the player
			if(openDoor ())
			{
				isOpening = false;
				isOpen = true;
				openTrigger();
				PlayerController.release ();
			}
		}
		//if the door is closing we then close it some more
		else if(isClosing)
		{
			//if it has finished closing we update the variables, trigger any event and release the player
			if(closeDoor ())
			{
				isClosing = false;
				isOpen = false;
				PlayerController.release ();
			}
		}
	}

	//triggered every frame when the door is opening
	private bool openDoor()
	{
		//For each direction we translate the object a small amount
		//We then check to see if we've reached the limit
		//If it has we force the object to the position of the limit to avoid tiny differences in the position
		//We then return true to stop the opening
		switch (openMovement)
		{
		case movement.posotiveX:
		{
			transform.Translate (speed * Time.deltaTime, 0, 0);
			if (transform.position.x >= limit)
			{
				transform.position = (new Vector3(limit, transform.position.y, transform.position.z));
				return true;
			}
			break;
		}
		case movement.negativeX:
		{
			transform.Translate (-speed * Time.deltaTime, 0, 0);
			if (transform.position.x <= limit)
			{
				transform.position = (new Vector3(limit, transform.position.y, transform.position.z));
				return true;
			}
			break;
		}
		case movement.posotiveY:
		{
			transform.Translate (0, speed * Time.deltaTime, 0);
			if (transform.position.y >= limit)
			{
				transform.position = (new Vector3(transform.position.x, limit, transform.position.z)); 
				return true;
			}
			break;
		}
		case movement.negativeY:
		{
			transform.Translate (0, -speed * Time.deltaTime, 0);
			if (transform.position.y <= limit)
			{
				transform.position = (new Vector3(transform.position.x, limit, transform.position.z));
				return true;
			}
			break;
		}
		case movement.posotiveZ:
		{
			transform.Translate (0, 0, speed * Time.deltaTime);
			if (transform.position.z >= limit)
			{
				transform.position = (new Vector3(transform.position.x, transform.position.y, limit));
				return true;
			}
			break;
		}
		case movement.negativeZ:
		{
			transform.Translate (0, 0, -speed * Time.deltaTime);
			if (transform.position.z <= limit)
			{
				transform.position = (new Vector3(transform.position.x, transform.position.y, limit));
				return true;
			}
			break;
		}
		default:
		{
			//if we are vanishing we just deactivate the object
			gameObject.SetActive(false);
			return true;
		}
		}
		return false;
	}

	//triggered every frame when the door is closing
	private bool closeDoor()
	{
		//For each direction we translate the object a small amount
		//We then check to see if we've reached the limit
		//If it has we force the object to the position of the limit to avoid tiny differences in the position
		//We then return true to stop the closing
		switch (openMovement)
		{
		case movement.posotiveX:
		{
			transform.Translate (-speed * Time.deltaTime, 0, 0);
			if (transform.position.x <= limit)
			{
				transform.position = (new Vector3(limit, transform.position.y, transform.position.z));
				return true;
			}
			break;
		}
		case movement.negativeX:
		{
			transform.Translate (speed * Time.deltaTime, 0, 0);
			if (transform.position.x >= limit)
			{
				transform.position = (new Vector3(limit, transform.position.y, transform.position.z));
				return true;
			}
			break;
		}
		case movement.posotiveY:
		{
			transform.Translate (0, -speed * Time.deltaTime, 0);
			if (transform.position.y <= limit)
			{
				transform.position = (new Vector3(transform.position.x, limit, transform.position.z));
				return true;
			}
			break;
		}
		case movement.negativeY:
		{
			transform.Translate (0, speed * Time.deltaTime, 0);
			if (transform.position.y >= limit)
			{
				transform.position = (new Vector3(transform.position.x, limit, transform.position.z));
				return true;
			}
			break;
		}
		case movement.posotiveZ:
		{
			transform.Translate (0, 0, -speed * Time.deltaTime);
			if (transform.position.z <= limit)
			{
				transform.position =(new Vector3(transform.position.x, transform.position.y, limit));
				return true;
			}
			break;
		}
		case movement.negativeZ:
		{
			transform.Translate (0, 0, speed * Time.deltaTime);
			if (transform.position.z >= limit)
			{
				transform.position = (new Vector3(transform.position.x, transform.position.y, limit));
				return true;
			}
			break;
		}
		default:
		{
			//if we are vanishing we just deactivate the object
			gameObject.SetActive(true);
			return true;
		}
		}
		return false;
	}

	//Starts the opening of the door
	public void open()
	{
		//holds the player
		PlayerController.hold ();
		//updates the variables
		setLimit (1);
		isOpening = true;
		//triggers an event
		openingTrigger ();
	}

	//Starts the closing of the door
	public void close()
	{
		//holds the player
		PlayerController.hold ();
		//updates the variables
		isOpen = false;
		setLimit (-1);
		//triggers an event
		isClosing = true;
	}

	//sets the limit to when the opening object stops opening or closing
	private void setLimit(short factor)
	{
		switch (openMovement) {
		case movement.posotiveX:
		{
			limit = transform.position.x + (factor * transform.localScale.x);
			break;
		}
		case movement.negativeX:
		{
			limit = transform.position.x - (factor * transform.localScale.x);
			break;
		}
		case movement.posotiveY:
		{
			limit = transform.position.y + (factor * transform.localScale.y);
			break;
		}
		case movement.negativeY:
		{
			limit = transform.position.y - (factor * transform.localScale.y);
			break;
		}
		case movement.posotiveZ:
		{
			limit = transform.position.z + (factor * transform.localScale.z);
			break;
		}
		case movement.negativeZ:
		{
			limit = transform.position.z - (factor * transform.localScale.z);
			break;
		}
		}
	}

	//An open trigegr called whenever a door opens
	protected virtual void openTrigger()
	{
		return;
	}

	//An opening trigger called whenever a door starts opening
	protected virtual void openingTrigger()
	{
		return;
	}

	//Used in various circulstances to decide how mobs interact with the openable objecy
	public bool isDoorOpen()
	{
		return isOpen;
	}
	
	public bool isDoorOpening()
	{
		return isOpening;
	}
	
	public bool isDoorClosing()
	{
		return isClosing;
	}

}
