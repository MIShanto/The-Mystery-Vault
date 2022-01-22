// This script is a Manager that controls the the flow and control of the game. It keeps
// track of player data (orb count, death count, total game time) and interfaces with
// the UI Manager. All game commands are issued through the static methods of this class

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

[DefaultExecutionOrder(-101)]
public class GameManager : MonoBehaviour
{
	//This class holds a static reference to itself to ensure that there will only be
	//one in existence. This is often referred to as a "singleton" design pattern. Other
	//scripts access this one through its public static methods
	public static GameManager instance;

	public bool isGameOver;                            //Is the game currently over?
	public bool isSwitchingScene;                            //Is switching scene..?

	public int normalKeyCount = 0;
	public int FinalKeyCount = 0;

	public int totalKeys;
	public List<Vector3> normalKeyPositions = new List<Vector3>();

	public float goChaoticTimerDuration;
	float goChaoticTimer;
	bool goChaoticTimerIsRunning = true;

	public float goNormalTimerDuration;
	float goNormalTimer;
	bool goNormalTimerIsRunning = false;

	public Vector3 normalPlayerPosition;

	bool isItNewGameManager = true;

	public int enemyCount = 0;

	int c = 0;
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

		//Create out collection to hold the orbs
		//orbs = new List<Orb>();

		//Persis this object between scene reloads
		//2,5,8,11,14 = normal world
		//3,6,9,12,15 = chaotic world
		//4,7,10,13,16 = special chaotic world

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		goChaoticTimer = goChaoticTimerDuration;
		goNormalTimer = goNormalTimerDuration;

		if(isItNewGameManager)
		{
			isItNewGameManager = false;

			TransitionManager.instance.PlayANimLoad("transitionclose");
		}
	}

	IEnumerator HandleDeath()
	{
		TransitionManager.instance.PlayANimLoad("transition");

		yield return new WaitForSeconds(0.7f);
		//as this script will only remain on normal level so this will reload normal level only.. 
		//destroy existing game manager..

		Destroy(GameManager.instance.gameObject, 0.1f);

		SceneManager.LoadScene(1);



	}
	void Update()
	{
		//if dialouge not complete hold everything
		if (PlayerPrefs.GetInt("Dialouge", 0) != 3)
			return;

		//If the game is over, exit
		if (isGameOver)
		{
			audio_Manager.instance.Stop("timer");

			/*if (SceneManager.GetActiveScene().buildIndex == 2)
			{
				//StartCoroutine(HandleDeath());
			}*/
			return;
		}

		if (SceneManager.GetActiveScene().buildIndex == 3)
			return;

		//Update the total game time and tell the UI Manager to update
		if (goChaoticTimerIsRunning)
		{

			if (goChaoticTimer < 6 && goChaoticTimer > 0)
			{
				if (c == 0)
				{
					c = 1;
					audio_Manager.instance.Play("timer");
				}
			}
			else
			{
				c = 0;
				audio_Manager.instance.Stop("timer");
			}

			//Timer section...........
			if (goChaoticTimer > 0)
			{
				goChaoticTimer -= Time.deltaTime;

				UIManager.UpdateswitchWorldTimerUI((int)goChaoticTimer);
			}
			else
			{
				goChaoticTimerIsRunning = false;
				goChaoticTimer = goChaoticTimerDuration;

				//save player position..
				normalPlayerPosition =  KeyManager.instance.normalPlayer.transform.position;

				//load chaotic scene..
				StartCoroutine(SwitchScene(true));
			}

			
		}

		else if (goNormalTimerIsRunning)
		{

			if (goNormalTimer < 6 && goNormalTimer > 0)
			{
				if (c == 0)
				{
					c = 1;
					audio_Manager.instance.Play("timer");
				}
			}
			else
			{
				c = 0;
				audio_Manager.instance.Stop("timer");
			}

			//Timer section...........
			if (goNormalTimer > 0)
			{
				goNormalTimer -= Time.deltaTime;

				UIManager.UpdateswitchWorldTimerUI((int)goNormalTimer);
			}
			else
			{
				goNormalTimerIsRunning = false;
				goNormalTimer = goNormalTimerDuration;

				//load normal scene..
				StartCoroutine(SwitchScene(false));

			}

		}

		

		//check for keys..
		if (normalKeyCount == totalKeys - 1 && goChaoticTimerIsRunning)
		{
			//turn off all timer..
			goChaoticTimerIsRunning = false;
			goNormalTimerIsRunning = false;

			//open gateway..
			DoorManager.instance.OpenDoor();

			//load final chaotic world..
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
		}
	}


	IEnumerator SwitchScene(bool switchToChaotic)
	{
		isSwitchingScene = true;

		//do transition anim..
		TransitionManager.instance.PlayAnimSwitch("transition");

		yield return new WaitForSeconds(2f);

		if(switchToChaotic)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

			TransitionManager.instance.PlayAnimSwitch("transitionclose");

			isSwitchingScene = false;

			// activate chatic timer.. 
			goNormalTimerIsRunning = true;
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

			TransitionManager.instance.PlayAnimSwitch("transitionclose");

			isSwitchingScene = false;

			// activate chatic timer.. 
			goChaoticTimerIsRunning = true;
		}
	}
	public static bool IsGameOver()
	{
		//If there is no current Game Manager, return false
		if (instance == null)
			return false;

		//Return the state of the game
		return instance.isGameOver;
	}

	public static bool IsSwitchingScene()
	{
		//If there is no current Game Manager, return false
		if (instance == null)
			return false;

		//Return the state of the game
		return instance.isSwitchingScene;
	}
}
