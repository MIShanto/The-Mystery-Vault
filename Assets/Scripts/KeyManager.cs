using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public GameObject[] normalKeys;
    public GameObject finalKey;

    public GameObject normalPlayer;

    private void Awake()
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
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < normalKeys.Length; i++)
        {
            for (int j = 0; j < GameManager.instance.normalKeyPositions.Count; j++)
            {

                if (normalKeys[i].transform.position == GameManager.instance.normalKeyPositions[j])
                {
                    normalKeys[i].SetActive(false);
                    
                }

            }
            
        }

        normalPlayer.transform.position = GameManager.instance.normalPlayerPosition;

    }

}
