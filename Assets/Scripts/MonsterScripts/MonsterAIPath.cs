using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MonsterAIPath : MonoBehaviour
{
    [SerializeField] Transform[] WayPoints;
    [SerializeField] private MonsterAI monster;
    private Vector3 nearestWp;
    private float nearest = 100000;
    private float distance;
    

    /*Todo List 
     * 1) Make list of waypoints 
     * 2) sicle trough them and check which is the closes to the player 
     * 3) set the distanation to that waypoint
     * 4) make the mosnter follow the next waypoitn 
     * 5) make the monster follow the waypoints back      
     */

    private int pointIndex = 0;

    public void GetNearestWp()
    {
        foreach (var Wp in WayPoints)
        {
            distance = Vector3.Distance(monster.transform.position, Wp.position);
            if (distance < nearest)
            {
                nearestWp = Wp.position;
                nearest = distance;
            }
        }
        monster.SetDestinationAgent(nearestWp); 
    }


    public void FollowWP()
    {
        if (pointIndex  <= WayPoints.Length )
        {
            monster.SetDestinationAgent(WayPoints[pointIndex].transform.position);

            if (monster.transform.position == WayPoints[pointIndex].transform.position)
            {
                pointIndex++;
                Debug.Log(pointIndex);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        monster.SetDestinationAgent(WayPoints[pointIndex].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        FollowWP();
    }
}
