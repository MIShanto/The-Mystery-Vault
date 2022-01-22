// This script handles inputs for the player. It serves two main purposes: 1) wrap up
// inputs so swapping between mobile and standalone is simpler and 2) keeping inputs
// from Update() in sync with FixedUpdate()

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
	[HideInInspector] public float horizontal;		//Float that stores horizontal input
	[HideInInspector] public bool jumpHeld;			//Bool that stores jump pressed
	[HideInInspector] public bool jumpPressed;		//Bool that stores jump held

	public GameObject playerDeathVFX;
	public GameObject coinPickupVFX;

	bool readyToClear;                              //Bool used to keep input in sync

	[SerializeField] float shakeIntensity;
	[SerializeField] float shakeTime;

	private void Start()
	{
		GameManager.instance.isGameOver = false;
		TransitionManager.instance.PlayANimLoad("transitionclose");
		audio_Manager.instance.Play("bg_sound");
	}
	void Update()
	{
/*		//for debug
		if (Input.GetKeyDown(KeyCode.R))
			PlayerPrefs.DeleteAll();
*/
		//Clear out existing input values
		ClearInput();

		//if dialouge not complete hold everything
		if (PlayerPrefs.GetInt("Dialouge", 0) != 3)
			return;

		//If the Game Manager says the game is over, exit
		if (GameManager.IsGameOver())
			return;
		//If the Game Manager says the it is switch time, exit
		if (GameManager.IsSwitchingScene())
			return;


		//Process keyboard, mouse, gamepad (etc) inputs
		ProcessInputs();

		//Clamp the horizontal input to be between -1 and 1
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
	}

	void FixedUpdate()
	{
		//In FixedUpdate() we set a flag that lets inputs to be cleared out during the 
		//next Update(). This ensures that all code gets to use the current inputs
		readyToClear = true;
	}

	void ClearInput()
	{
		//If we're not ready to clear input, exit
		if (!readyToClear)
			return;

		//Reset all inputs
		horizontal		= 0f;
		jumpPressed		= false;
		jumpHeld		= false;

		readyToClear	= false;
	}

	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal		+= Input.GetAxis("Horizontal");

		//Accumulate button inputs
		jumpPressed		= jumpPressed || Input.GetButtonDown("Jump");
		jumpHeld		= jumpHeld || Input.GetButton("Jump");

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "NormalKey")
		{
			audio_Manager.instance.Play("key");

			Destroy(collision.transform.parent.gameObject);

			Instantiate(coinPickupVFX, collision.transform.position, Quaternion.identity);

			GameManager.instance.normalKeyCount++;

			GameManager.instance.normalKeyPositions.Add(collision.gameObject.transform.parent.position);

			UIManager.UpdateNormalKeyUI(14 - GameManager.instance.normalKeyCount);

		}

		if (collision.tag == "FinalKey")
		{
			audio_Manager.instance.Play("key");

			Destroy(collision.gameObject);

			Instantiate(coinPickupVFX, collision.transform.position, Quaternion.identity);

			GameManager.instance.FinalKeyCount++;

			UIManager.UpdateFinalKeyUI(1 - GameManager.instance.FinalKeyCount);

		}

		if (collision.tag == "Obstacle")
		{
			audio_Manager.instance.Play("dead");

			StartCoroutine(HandleDeath());

			CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);
		}

		if (collision.tag == "GoToFinalChaoticWorld")
		{
			TransitionManager.instance.PlayANimLoad("transition");

			Invoke("LoadFinalChaoticScene", 0.7f);

		}
		/*
		if (collision.tag == "Level 2 End")
		{

			Invoke("LoadScene0", 0.7f);
		}*/
	}

	IEnumerator HandleDeath()
	{
		GameManager.instance.isGameOver = true;

		GameObject playerVFX = Instantiate(playerDeathVFX, transform.position, Quaternion.identity);
		Destroy(playerVFX, 4f);
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<Collider2D>().enabled = false;

		yield return new WaitForSeconds(1.1f);

		TransitionManager.instance.PlayANimLoad("transition");

		yield return new WaitForSeconds(0.7f);
		//as this script will only remain on normal level so this will reload normal level only.. 
		LoadCurrentScene();

		//gameOverPanel.SetActive(true);

		/*totalPointText.text = "Total point: " + point;

		audioSource.PlayOneShot(gameOverClip, 1f);*/

	}

	void LoadFinalChaoticScene()
	{
		//load final chaotic world..
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
	}

	void LoadCurrentScene()
	{
		//destroy existing game manager..
		Destroy(GameManager.instance.gameObject);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
