using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeManager : MonoBehaviour
{
    public GameObject dialougePanel; // 3 total

    int index = 0;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Dialouge", 0) != 3)
        {
            audio_Manager.instance.Play("dead");
        }
    }
    private void Update()
    {
        if(PlayerPrefs.GetInt("Dialouge", 0) != 3)
        {
            dialougePanel.SetActive(true);
            if(index < 3)
                dialougePanel.transform.GetChild(index).gameObject.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            audio_Manager.instance.Play("dead");

            index++;
            dialougePanel.transform.GetChild(index - 1).gameObject.SetActive(false);
        }
        if(index > 2)
        {
            PlayerPrefs.SetInt("Dialouge", 3);
            dialougePanel.SetActive(false);
        }
    }
}
