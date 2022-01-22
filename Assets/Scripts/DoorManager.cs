using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
	public static DoorManager instance;

	public GameObject DoorOpenVFX;
	void Awake()
	{
		//If a Game Manager exists and this isn't it...
		if (instance != null && instance != this)
		{
			//...destroy this and exit. There can only be one Game Manager
			Destroy(gameObject);
			return;
		}

		//Set this as the current game manager
		instance = this;

	}

	public void OpenDoor()
	{
		GameObject vfx =  Instantiate(DoorOpenVFX, transform.position, Quaternion.identity);
		Destroy(vfx, 5f);

		gameObject.SetActive(false);
	}
}
