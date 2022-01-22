// This script is a Manager that controls the UI HUD (deaths, time, and orbs) for the 
// project. All HUD UI commands are issued through the static methods of this class

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	//This class holds a static reference to itself to ensure that there will only be
	//one in existence. This is often referred to as a "singleton" design pattern. Other
	//scripts access this one through its public static methods
	public static UIManager instance;

	public TextMeshProUGUI normalKeyText;			//Text element showing number of normal keys
	public TextMeshProUGUI finalKeyText;			//Text element showing number of normal keys

	public TextMeshProUGUI switchWorldTimerText;		//Text element showing amount of time

	public TextMeshProUGUI gameOverText;    //Text element showing the Game Over message

	Image img;
	void Awake()
	{
		//If an UIManager exists and it is not this...
		if (instance != null && instance != this)
		{
			//...destroy this and exit. There can be only one UIManager
			Destroy(gameObject);
			return;
		}

		//This is the current UIManager and it should persist between scene loads
		instance = this;

		//DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if(normalKeyText != null)
			UpdateNormalKeyUI(14 - GameManager.instance.normalKeyCount);
		if (finalKeyText != null)
			UpdateFinalKeyUI(1 - GameManager.instance.FinalKeyCount);

	}
	public static void UpdateNormalKeyUI(int keyCount)
	{
		//If there is no current UIManager, exit
		if (instance == null)
			return;

		//Update the text orb element
		instance.normalKeyText.text = keyCount.ToString();
	}
	public static void UpdateFinalKeyUI(int keyCount)
	{
		//If there is no current UIManager, exit
		if (instance == null)
			return;

		//Update the text orb element
		instance.finalKeyText.text = keyCount.ToString();
	}

	public static void UpdateswitchWorldTimerUI(int time)
	{
		//If there is no current UIManager, exit
		if (instance == null)
			return;

		//Update the text orb element
		instance.switchWorldTimerText.text = time.ToString();

		if (time < 6)
		{
			instance.switchWorldTimerText.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 1);
			instance.switchWorldTimerText.color = new Color(1, 0, 0, 1);
		}
		else
		{
			instance.switchWorldTimerText.transform.parent.GetComponent<Image>().color = new Color(0.9f, 1f, 0.95f, 1f);
			instance.switchWorldTimerText.color = new Color(0, 0, 0, 1);
		}
	}

	
	public static void DisplayGameOverText()
	{
		//If there is no current UIManager, exit
		if (instance == null)
			return;

		//Show the game over text
		instance.gameOverText.enabled = true;
	}
}
