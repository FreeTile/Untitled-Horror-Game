using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;
    [SerializeField]
    public float atkDistance;
    [SerializeField]
    public Collider atkColider;


    public bool seePlayer = false;
    public float angle;
    public float radius; 


    NavMeshAgent agent;
    LayerMask playerMask;
    LayerMask obstaclesMask;
    private float lastDistance;
    private Vector3 lastPosition;

    //Monster Ai machine states 
    enum MonsterSates
    {
        WONDER,
        PERSU,
        ATTACK
    }
    MonsterSates state = 0;

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void Update()
    {
        //Switch that changes beteween the monsster states
        switch (state)
        {
            case MonsterSates.WONDER: // Wonder state
                if(seePlayer)
                    state = MonsterSates.PERSU;

                break;
            case MonsterSates.PERSU: // Persu state
                if (seePlayer)
                    PersuPlayer(Player.transform.position);
                else if (!seePlayer)
                {
                    PersuPlayer(lastPosition);
                    state = MonsterSates.WONDER;
                }
                
                if (atkDistance <= lastDistance) 
                    state = MonsterSates.ATTACK;

                break;
            case MonsterSates.ATTACK: // Attack state
                attackPlayer();

                if(!seePlayer)
                    state = MonsterSates.PERSU;

                break;
            default:
                break;
        }
    }

    //Sets the destination for the monster agent 
    void PersuPlayer(Vector3 location)
    {
        agent.SetDestination(location);
    }

    //attack fucntion
    private void attackPlayer()
    {
        if (atkColider.gameObject.tag == "Player")
        {
            //The attack that the monster does goes here
        }
    }

    void Start()
    {
        lastPosition = new Vector3();
        playerMask = LayerMask.GetMask("Character");
        obstaclesMask = LayerMask.GetMask("Obstacles");
        agent = this.GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
    }

    //Coroutine that updates every 0.2 seconds an executes the check for the line of sight
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); //<------- Change if needed

        while (true)
        {
            yield return wait;
            fieldOfView();
        }
    }

    //Checks if the player is in line of sight of the monster 
    private void fieldOfView()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, radius, playerMask);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToPlayer = (target.position - transform.position).normalized;

            float angleoffser = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleoffser < angle / 2)
            {
                float distanceToPLayer = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPLayer, obstaclesMask))
                {
                    lastDistance = distanceToPLayer;
                    lastPosition = rangeCheck[0].transform.position;
                    seePlayer = true;
                }
                else
                    seePlayer = false;
            }
            else 
                seePlayer = false;
        }
        else if (seePlayer)
            seePlayer = false;
    }
}
