using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        audio_Manager.instance.Play("MainMenu");

        TransitionManager.instance.PlayANimLoad("transitionclose");
    }
    public void PlayButtonPressed()
    {
        audio_Manager.instance.Play("button");


        TransitionManager.instance.PlayANimLoad("transition");

        Invoke("Load", 0.7f);
    }

    public void CreditButtonPressed()
    {
        audio_Manager.instance.Play("button");

    }
    public void ExitButtonPressed()
    {
        audio_Manager.instance.Play("button");

        Application.Quit();
    }

    void Load()
    {
        SceneManager.LoadScene(1);
    }
}
