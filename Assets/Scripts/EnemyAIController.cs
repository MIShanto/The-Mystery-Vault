using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public LayerMask PlayerMask;
    bool touchedPlayer;
    GameObject player;
    PlayerHealth playerhealth;
    bool availableDamage;
    public Animator anim;
    AIPath aiPath;          //aidestination setter script
    // Start is called before the first frame update
    void Start()
    {
        availableDamage = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerhealth = player.GetComponent<PlayerHealth>();
        aiPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        if (aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
        {
            // play attack Animation;
            anim.Play("attackAnimation");
        }
        else
        {
            // play running animation;
            anim.Play("walking enemy");
        }
    }

}
