using UnityEngine;
using System.Collections;
//Represents an enviroment object to be used on the map
public abstract class Enviroment : MonoBehaviour {
	//Instanciates the class
	protected void Start () {
		//This enviroment object is added to the map
		EnviromentMap.addToMap (transform.position.x, transform.position.y, transform.position.z, transform.localScale.x, transform.localScale.y, transform.localScale.z, this);
	}

	//Abstract methods needed form mobiles using the Enviroment Map
	public abstract bool canWalkTo(MobileEnviroment enviroment);

	public virtual void preWalk(MobileEnviroment mobile)
	{

	}

	public virtual void postWalk(MobileEnviroment mobile)
	{

	}

	public abstract bool canWalkOnTopOf();

	public abstract bool walkOnTopOf();

	public virtual bool doesBlock()
	{
		return true;
	}

	public virtual void walkAwayFrom()
	{
		//This is the default behaviour that most objects won't change
		return;
	}

}
