using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    public HealthManager healthManager;
    [SerializeField]
    public CapsuleCollider Player;
    [SerializeField]
    public float atkDistance;
    [SerializeField]
    public Collider atkColider;



    public bool seePlayer = false;
    public float angle;
    public float radius;

    public float rotationSpeed;
    public float rotationAngle;


    NavMeshAgent agent;
    LayerMask playerMask;
    LayerMask obstaclesMask;
    private float lastDistance;
    private Vector3 lastPosition;

    //Navigation System
    [SerializeField] Transform[] WayPoints;
    private Vector3 nearestWp;
    private float nearest = 100000;
    private float distance;
    private int pointIndex = 0;
    private bool finishedWp;
    private bool followingWp = true;

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
                {
                    state = MonsterSates.PERSU;
                    followingWp = false;
                }                   
                break;

            case MonsterSates.PERSU: // Persu state
                Debug.Log("Persu");
                if (seePlayer)
                {
                    SetDestinationAgent(Player.transform.position);
                    Debug.Log(Vector3.Distance(transform.position, Player.transform.position));
                    if (Vector3.Distance(transform.position, Player.transform.position) <= atkDistance) 
                        state = MonsterSates.ATTACK;
                }
                else if (!seePlayer)
                {
                    SetDestinationAgent(lastPosition);
                    state = MonsterSates.WONDER;
                    Debug.Log("Yes it did");

                }
                break;
            case MonsterSates.ATTACK: // Attack state
                Debug.Log("Attack");
                if(!seePlayer || Vector3.Distance(Player.transform.position, transform.position) >= atkDistance)
                {
                    state = MonsterSates.PERSU;
                    break;
                }
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

   
    public void GetNearestWp()
    {
        pointIndex = 0;
        foreach (var Wp in WayPoints)
        {
            distance = Vector3.Distance(transform.position, Wp.position);
            if (distance < nearest)
            {
                nearestWp = Wp.position;
            }
            pointIndex++;
        }
        SetDestinationAgent(nearestWp);
        followingWp = true;
    }


    //Waypoints 
    public void FollowWP()
    {
        /*if(transform.position == WayPoints[pointIndex].transform.position)
        {
            MoveToWp = false;
            StartCoroutine(LookAround());
        }
        */
        if (followingWp)
        {
            if (pointIndex <= WayPoints.Length - 1)
            {
                agent.updateRotation = true;
                agent.SetDestination(WayPoints[pointIndex].transform.position);

                if (transform.position == WayPoints[pointIndex].transform.position && finishedWp == false)
                {
                    pointIndex++;
                    if (pointIndex == WayPoints.Length - 1)
                    {
                        finishedWp = true;
                    }
                }
                else if (transform.position == WayPoints[pointIndex].transform.position && finishedWp == true)
                {
                    pointIndex--;
                    if (pointIndex == 0)
                    {
                        finishedWp = false;
                    }
                }

            }
            Debug.Log(pointIndex);
        }
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

    //Looks around from right to left and goes back to original rotation
    private IEnumerator LookAround()
    {
        Quaternion InitialRotation = transform.rotation;
        float startRotation = transform.rotation.eulerAngles.y;
        float targetRotation = startRotation + rotationAngle; // Rotate right 
            
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * rotationSpeed / 360f; // Adjust for smooth rotation
            float currentAngle = Mathf.LerpAngle(startRotation, targetRotation, timeElapsed);
            transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            Debug.Log(currentAngle);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        startRotation = transform.rotation.eulerAngles.y;
        targetRotation = startRotation - rotationAngle; // rotates left 

        timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * rotationSpeed / 360f; 
            float currentAngle2 = Mathf.LerpAngle(startRotation, targetRotation, timeElapsed);
            transform.rotation = Quaternion.Euler(0f, currentAngle2, 0f);
            Debug.Log(currentAngle2);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        //Not working yet need to set the rotation back to normal and make it so that it only rotates with the path given 
        transform.LookAt(WayPoints[pointIndex +1], Vector3.up);


        yield return new WaitForSeconds(1f);

        followingWp = true;
    }
}
 