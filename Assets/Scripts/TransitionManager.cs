using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
	public static TransitionManager instance;


	public Animator switchSceneTransitionAnimator;
    public Animator loadSceneTransitionAnimator;

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
	

	public void PlayAnimSwitch(string name)
	{
		audio_Manager.instance.Play("loadTransition");
		audio_Manager.instance.Play("switchTransition");
		switchSceneTransitionAnimator.Play(name);
	}

	public void PlayANimLoad(string name)
	{
		audio_Manager.instance.Play("loadTransition");
		loadSceneTransitionAnimator.Play(name);
	}
}
