using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChaoticWorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.isGameOver = false;

        TransitionManager.instance.PlayANimLoad("transitionclose");

        audio_Manager.instance.Play("FinalChaos");
        audio_Manager.instance.Stop("timer");
    }


}
