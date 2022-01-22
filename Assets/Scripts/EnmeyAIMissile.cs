using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmeyAIMissile : MonoBehaviour
{
    public GameObject missile;
    public Transform shootPos;
    AIPath aiPath;          //aidestination setter script
    bool callActive ;

    public Animator anim;

    private void Start()
    {
        callActive = true;
        aiPath = GetComponent<AIPath>();
    }
    // Update is called once per frame
    void Update()
    {
        //TakeDamage();
        if (aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
        {
            // play idle animation;
            anim.Play("attackAnimation");
            TakeDamage();
        }
        else
        {
            anim.Play("walking enemy");
            //play running animation;
        }
    }

    int i = 1;
    float time = 0;
    void TakeDamage()
    {
        /*time += Time.fixedDeltaTime;

        // use corouting...
        if (time >= 1f)
        {
            i++;
            time = 0;
        }
        if (i % 5 == 0)
        {
            FireMissile();
            // play fire animation
            
            i++;
        }
        if (i >= 10)
        {
            i = 1;
        }*/

        if (callActive)
        {
            StartCoroutine(FireMissile());
        }
    }

    IEnumerator FireMissile()
    {
        Instantiate(missile, shootPos.position, Quaternion.identity);
        callActive = false;
        yield return new WaitForSeconds(1f);
        callActive = true;
    }
}
