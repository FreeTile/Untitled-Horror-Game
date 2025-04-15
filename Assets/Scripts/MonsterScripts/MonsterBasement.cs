using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBasement : MonoBehaviour
{
    [Header("Player Info")]
    public CapsuleCollider Player;
    public bool playerDamaged = false;

    [Header("Player Info")]
    [SerializeField]
    private Animator animator;
    NavMeshAgent agent;


    //Monster Ai machine states 
    enum MonsterSates
    {
        PERSU = 1,

        NUM_STATES
    }
    MonsterSates state = 0;

    void Start()
    {
        state = MonsterSates.PERSU;
        agent = GetComponent<NavMeshAgent>();
    }

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void Update()
    {
        //checks if player was damaged if so it stops the Monster for a few seconds before resuming 
        if (!playerDamaged)
        {
            //Switch that changes beteween the monsster states
            switch (state)
            {
                case MonsterSates.PERSU: // Persu state
                    animator.Play("Idle_001");
                    Vector3 ofset = new Vector3(0.3f, 0, 0.3f);
                    SetDestinationAgent(Player.transform.position + ofset);
                    break;
                default:
                    break;
            }
        }
    }

    //Sets the destination for the monster agent 
    public void SetDestinationAgent(Vector3 location)
    {
        agent.SetDestination(location);
    }
}

