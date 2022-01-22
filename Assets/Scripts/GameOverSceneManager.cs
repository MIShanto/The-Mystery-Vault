using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        TransitionManager.instance.PlayANimLoad("transitionclose");

        yield return new WaitForSeconds(6f);

        TransitionManager.instance.PlayANimLoad("transition");

        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(0);
    }

   
}
