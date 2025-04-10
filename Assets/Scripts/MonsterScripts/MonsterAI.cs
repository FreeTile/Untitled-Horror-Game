using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("Player Info")]
    public CapsuleCollider Player;
    public bool playerDamaged = false;


    [Header("FOV Values")]
    public float angle;
    public float radius;
    public bool seePlayer = false;

    [Header("Look around Values")]
    public float rotationSpeed;
    public float rotationAngle;

    [Space]
    [SerializeField] 
    Transform[] WayPoints;

    NavMeshAgent agent;
    LayerMask playerMask;
    LayerMask obstaclesMask;
    private Vector3 lastPosition;

    //Navigation System
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
        LOOK,

        NUM_STATES
    }
    MonsterSates state = 0;

    void Start()
    {
        lastPosition = new Vector3();
        playerMask = LayerMask.GetMask("Player");
        obstaclesMask = LayerMask.GetMask("Obstacles");
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
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
                case MonsterSates.WONDER: // Wonder state
                    FollowWP();
                    if (seePlayer)
                    {
                        state = MonsterSates.PERSU;
                        followingWp = false;
                    }
                    break;
                case MonsterSates.PERSU: // Persu state
                    if (seePlayer)
                    {
                        Vector3 ddajkfe = new Vector3(0.2f, 0, 0.2f);
                        SetDestinationAgent(Player.transform.position - ddajkfe);
                    }
                    else if (!seePlayer)
                    {
                        if (Vector3.Distance(transform.position, lastPosition) <= 1)
                        {
                            StartCoroutine(LookAround());
                        }
                    }
                    break;
                case MonsterSates.LOOK:
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

   //Gets the nearest waypoint from the monster and makes it follow the waypoint path from that point 
    public void GetNearestWp()
    {
        //Checks for the closes waypoint 
        int tempIndex = 0;
        nearest = 100000;
        foreach (var Wp in WayPoints)
        {
            distance = Vector3.Distance(transform.position, Wp.position);
            if (distance < nearest && tempIndex != pointIndex)
            {
                nearest = distance;
                nearestWp = Wp.position;
            }
            tempIndex++;
        }

        //adds to the pointIndex to set the next waypoint 
        pointIndex = 0;
        foreach (var Wp in WayPoints)
        {
            distance = Vector3.Distance(transform.position, Wp.position);
            Debug.Log(distance);
            if (Wp.position == nearestWp)
            {
                break;
            }
            pointIndex++;
        }
        SetDestinationAgent(nearestWp);
        followingWp = true;
    }


    //Monster Followes the Waypoints from one end to another and then the other way around 
    public void FollowWP()
    {
        //sets a random range to make the monster LookAround for the player 
        if(Random.Range(0,4) == 1 && transform.position == WayPoints[pointIndex].transform.position)
        {
            if (state != MonsterSates.LOOK)
            {
                state = MonsterSates.LOOK;
                StartCoroutine(LookAround());
            }
        }
        else if(followingWp)
        {
             if (pointIndex <= WayPoints.Length - 1)
             {
                agent.updateRotation = true;
                agent.SetDestination(WayPoints[pointIndex].transform.position);

                if (transform.position == WayPoints[pointIndex].transform.position && finishedWp == false)
                {
                    pointIndex++;
                    if (pointIndex >= WayPoints.Length - 1)
                    {
                        pointIndex = WayPoints.Length - 1;
                        finishedWp = true;
                    }
                }
                else if (transform.position == WayPoints[pointIndex].transform.position && finishedWp == true)
                {
                    pointIndex--;
                    if (pointIndex <= 0)
                    {
                        pointIndex = 0;
                        finishedWp = false;
                    }
                }

             }
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
                    float lastDistance = distanceToPLayer;
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
    //Coroutine that looks around from right to left and goes back to a wonder or persu state 
    private IEnumerator LookAround()
    {
        if (!seePlayer)
        {
            float startRotation = transform.rotation.eulerAngles.y;
            float targetRotation = startRotation + rotationAngle; // Rotate right 

            float timeElapsed = 0f;

            while (timeElapsed < 1f)
            {
                timeElapsed += Time.deltaTime * rotationSpeed / 360f; // Adjust for smooth rotation
                float currentAngle = Mathf.LerpAngle(startRotation, targetRotation, timeElapsed);
                transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
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
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }

        fieldOfView();

        if (seePlayer)
        {
            state = MonsterSates.PERSU;
            followingWp = false;
        }
        else if (!followingWp)
        {
            GetNearestWp();
            followingWp = true;
            state = MonsterSates.WONDER;
        }
        else if (followingWp)
        {
            state = MonsterSates.WONDER;
        }
        yield return new WaitForSeconds(1f);
    }
}
 