using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    GameObject player;
    public GameObject[] cards;
    public GameObject[] spawnPoints;
    public GameObject[] spawnVFX;
    bool available = true , active;
    gunMove gunmove;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //gunmove = player.GetComponent<gunMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position;

        if (available)
        {
            int rand = Random.Range(0, 3);
            StartCoroutine(ActiveCard(rand));
        }
    }

    IEnumerator ActiveCard(int number)
    {
        available = false;

        int random = Random.Range(0,5);

        yield return new WaitForSeconds(3f);

        GameObject vfx = Instantiate(spawnVFX[number], spawnPoints[random].transform.position, Quaternion.identity);

        GameObject card = Instantiate(cards[number], spawnPoints[random].transform.position, Quaternion.identity);

        StartCoroutine(VanishCard(card));
        
    }

    IEnumerator VanishCard(GameObject card)
    {
        yield return new WaitForSeconds(4f);
        {
            Destroy(card);
            available = true;
        }
    }
}
