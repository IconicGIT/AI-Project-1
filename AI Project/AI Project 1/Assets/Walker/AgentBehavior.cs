using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

public class AgentBehavior : MonoBehaviour
{

    [SerializeField]
    NavMeshAgent me;

    public GameObject target;
    GameObject target_ref;

    public bool isBeingChased;
    float speed_ref;

    [SerializeField]
    GameObject pointToFollowInPath;

    [SerializeField]
    float timerRef;
    [SerializeField]
    float timer = 0;
    float timerDecrease = 1;

    [SerializeField]
    float fleeDistance;

    [SerializeField]
    float walkDistance;

    [SerializeField]
    [Range(0, 50)]
    float maxWalkDistance;

    [SerializeField]
    float walkRadius;

    [SerializeField]
    GameObject[] hidingSpots;

    [SerializeField]
    bool debugPointWander;

    [SerializeField]
    bool debugHide;

    [SerializeField]
    bool fleeNearTarget;

    [SerializeField]
    Vector3 st, end;

    enum MovementMode
    {
        NONE,
        PURSUE,
        FLEE,
        WANDER,
        PATROL,
        HIDE,
    }

    [SerializeField]
    MovementMode movMode;
    MovementMode prev_movMode;

    bool Timer()
    {
        if (timer > 0)
        {

            timer -= timerDecrease;
            return false;
        }
        else
        {
            timer = (int)UnityEngine.Random.Range(timerRef - timerRef/2, timerRef + timerRef/2);
           
            return true;
        }


    }

    void Seek(Vector3 destination)
    {
        me.destination = destination;
    }

    void Wander()
    {
        float radius = walkRadius;

        Vector3 localTarget = UnityEngine.Random.insideUnitCircle * radius;
        localTarget += new Vector3(0, 0, walkDistance);


        Vector3 worldTarget = transform.TransformPoint(localTarget);

        worldTarget.y = 0f;

        st = transform.position;
        end = worldTarget;

        me.destination = worldTarget;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(worldTarget, out  hit, radius, NavMesh.AllAreas))
        {
            Seek(hit.position);
        }
        else
        {
            worldTarget = transform.TransformPoint(-localTarget);
            worldTarget.y = 0f;
            if (NavMesh.SamplePosition(worldTarget, out hit, radius, NavMesh.AllAreas))
            {
                Seek(hit.position);
            }
        }


        //Debug.Log("hit: " + me.destination);

    }

    void FleeFrom(Vector3 position)
    {
        Vector3 fleeVector = position - transform.position;
        Seek(transform.position - fleeVector);
    }

    void Hide()
    {
        Func<GameObject, float> distance = (hs) => Vector3.Distance(target.transform.position, hs.transform.position);
        //Func<GameObject, bool> avaiableSpot = (s) => Vector3.Distance(target.transform.position, s.transform.position) < fleeDistance;

        GameObject hidingSpot = hidingSpots.Select(ho => (distance(ho), ho)).Min()/*ElementAt(UnityEngine.Random.Range(0, hidingSpots.Length - 1))*/.Item2;


        Vector3 dir = hidingSpot.transform.position - target.transform.position * 2;

        //print("target.transform.position: " + target.transform.position * 2);

        Ray backRay = new Ray(hidingSpot.transform.position, -dir.normalized);

        st = hidingSpot.transform.position;
        end = -dir;

        RaycastHit info;
        hidingSpot.GetComponent<Collider>().Raycast(backRay, out info, 50f);

        Vector3 destination = hidingSpot.transform.position + info.point + dir.normalized * 5;
        destination.y = 0;
        Seek(destination);
        //print("me.destination: " + me.destination);
        //print("destination: " + destination);
        //print("hidingSpot: " + hidingSpot);
        //print("info.point + dir.normalized: " + " " + info.point + " " + dir.normalized + " " + (info.point + dir.normalized));

    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
        speed_ref = me.speed;
        target_ref = target;
        prev_movMode = movMode;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir;

        float lookAhead;

        if ((movMode == MovementMode.HIDE || movMode == MovementMode.WANDER || movMode == MovementMode.FLEE) && fleeNearTarget && !isBeingChased)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < fleeDistance)
            {
                if (movMode != MovementMode.FLEE) prev_movMode = movMode;
                movMode = MovementMode.FLEE;
                me.speed = speed_ref * 3;
                print("should flee");
            }
            else
            {
                me.speed = speed_ref;

                movMode = prev_movMode;
                print("should hide / wander");

            }
        }

        if (isBeingChased)
        {
            if (movMode != MovementMode.FLEE)
            {
                prev_movMode = movMode;
            }
            me.speed = speed_ref * 3;

            movMode = MovementMode.FLEE;
            
        }
        else
        {
            movMode = prev_movMode;
            me.speed = speed_ref;
            target = target_ref;
        }


        switch (movMode)
        {
            case MovementMode.NONE:
                break;

            case MovementMode.PURSUE:

                targetDir = target.transform.position - transform.position;
                lookAhead = targetDir.magnitude / me.speed;

                Seek(target.transform.position + target.transform.forward * lookAhead);
                break;

            case MovementMode.FLEE:

                FleeFrom(target.transform.position);
                break;

            case MovementMode.WANDER:

               
                if (Timer())
                {
                    Wander();
                    walkDistance = (int)UnityEngine.Random.Range(maxWalkDistance - maxWalkDistance / 2, maxWalkDistance + maxWalkDistance / 2);
                }

               if (debugPointWander) Debug.DrawRay(st, end, Color.red);


                break;

            case MovementMode.HIDE:

                Hide();
                if (debugHide)
                {
                    Debug.DrawLine(st, end, Color.red);
                }

                break;

            case MovementMode.PATROL:

                
                if (Vector3.Distance(transform.position,pointToFollowInPath.transform.position) > 5)
                {
                    me.speed = Mathf.Abs(pointToFollowInPath.GetComponent<PathPointController>().speed) * 3;
                }
                else
                {
                    me.speed = Mathf.Abs(pointToFollowInPath.GetComponent<PathPointController>().speed) - 0.1f;
                }

                Seek(pointToFollowInPath.transform.position);

                break;
        }

    }
}
