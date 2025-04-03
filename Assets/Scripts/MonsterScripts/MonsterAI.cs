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
    public CapsuleCollider Player;
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

    [SerializeField] Transform[] WayPoints;
    private Vector3 nearestWp;
    private float nearest = 100000;
    private float distance;
    private int pointIndex = 0;

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
                Debug.Log("Wonder");
                FollowWP();
                if (seePlayer)
                    state = MonsterSates.PERSU;

                break;
            case MonsterSates.PERSU: // Persu state
                Debug.Log("Persu");
                if (seePlayer)
                    SetDestinationAgent(Player.transform.position);
                else if (!seePlayer)
                {
                    SetDestinationAgent(lastPosition);
                    state = MonsterSates.WONDER;
                }
                
                if (atkDistance <= lastDistance) 
                    state = MonsterSates.ATTACK;

                break;
            case MonsterSates.ATTACK: // Attack state
                Debug.Log("Attack");
                if (seePlayer)
                attackPlayer();

                if(!seePlayer)
                    state = MonsterSates.PERSU;

                break;
            default:
                break;
        }
    }

    //Sets the destination for the monster agent 
    public void SetDestinationAgent(Vector3 location)
    {
        agent.SetDestination(location);
    }



    /*Todo List 
     * 1) Make list of waypoints 
     * 2) sicle trough them and check which is the closes to the player 
     * 3) set the distanation to that waypoint
     * 4) make the mosnter follow the next waypoitn 
     * 5) make the monster follow the waypoints back      
     */
    public void GetNearestWp()
    {
        pointIndex = 0;
        foreach (var Wp in WayPoints)
        {
            distance = Vector3.Distance(transform.position, Wp.position);
            if (distance < nearest)
            {
                nearestWp = Wp.position;
                nearest = distance;
            }
            pointIndex++;
        }
        SetDestinationAgent(nearestWp);
    }


    //Waypoints 
    public void FollowWP()
    {
        if (pointIndex <= WayPoints.Length -1)
        {
            agent.SetDestination(WayPoints[pointIndex].transform.position);

            if (transform.position == WayPoints[pointIndex].transform.position)
            {
                pointIndex++;
                Debug.Log(pointIndex);
            }
        }
    }

    //attack fucntion
    private void attackPlayer()
    {
       //Applicable for animations or states only
    }

    void Start()
    {
        lastPosition = new Vector3();
        playerMask = LayerMask.GetMask("Player");
        obstaclesMask = LayerMask.GetMask("Obstacles");
        agent = this.GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
    }

    //Coroutine that updates every 0.2 seconds an executes the check for the line of sight
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f); //<------- Change if needed

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
