﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

public class AgentBehavior : MonoBehaviour
{
    TextMesh infoText;

    GameObject camObject;

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
    bool DebugPatrol;

    [SerializeField]
    bool DebugFlee;

    [SerializeField]
    bool DebugPursue;

    [SerializeField]
    bool fleeNearTarget;



    Vector3 st, end;
    Vector3 st1, end1;

    public enum AgentType
    {
        NONE,
        ROBBER,
        WALKER,
        COP
    }

    public AgentType agentType;

    public enum MovementMode
    {
        NONE,
        PURSUE,
        FLEE,
        WANDER,
        PATROL,
        HIDE,
    }

    [SerializeField]
    public MovementMode movMode;
    MovementMode prev_movMode;

    public void SetState(MovementMode movementMode)
    {
        movMode = movementMode;
    }


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

    public void Seek(Vector3 destination)
    {
        me.destination = destination;
    }

    public  void Wander()
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

    public void FleeFrom(Vector3 position)
    {
        Vector3 fleeVector = position - transform.position;
        Seek(transform.position - fleeVector);
    }

    public void Hide()
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

        print("hidingSpot: " + hidingSpot.transform.position);
        print("info: " + info.point);
        print("dir: " + dir.normalized * 5);
        print("destination: " + destination);
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

        camObject = GameObject.FindGameObjectWithTag("MainCamera");
        infoText = gameObject.GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

        infoText.transform.rotation = Quaternion.LookRotation(camObject.transform.position);


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
                infoText.text = "None";
                infoText.color = Color.gray;
                break;

            case MovementMode.PURSUE:
                infoText.text = "PURSUING";
                infoText.color = Color.red;

                targetDir = target.transform.position - transform.position;
                lookAhead = targetDir.magnitude / me.speed;
                if (DebugPursue)
                    Debug.DrawLine(transform.position, target.transform.position, Color.red);

                Seek(target.transform.position + target.transform.forward * lookAhead);
                break;

            case MovementMode.FLEE:
                infoText.text = "FLEEING";
                infoText.color = Color.blue;


                if (DebugFlee)
                    Debug.DrawLine(transform.position, target.transform.position, Color.blue);

                FleeFrom(target.transform.position);
                break;

            case MovementMode.WANDER:
                infoText.text = "WANDERING";
                infoText.color = Color.green;


                if (Timer())
                {
                    Wander();
                    walkDistance = (int)UnityEngine.Random.Range(maxWalkDistance - maxWalkDistance / 2, maxWalkDistance + maxWalkDistance / 2);
                }

               if (debugPointWander) 
                    Debug.DrawLine(transform.position, me.destination, Color.green);


                break;

            case MovementMode.HIDE:
                infoText.text = "HIDING";
                infoText.color = Color.magenta;


                if (Timer())
                {
                    Hide();

                }

                if (debugHide)
                {
                    Debug.DrawLine(st, end, Color.magenta);
                    Debug.DrawLine(transform.position, target.transform.position, Color.red);
                }

                break;

            case MovementMode.PATROL:

                infoText.text = "PATROLING";
                infoText.color = Color.cyan;

                float distanceToFollower = Vector3.Distance(transform.position, pointToFollowInPath.transform.position);

                if (distanceToFollower > 15)
                {
                    me.speed = Mathf.Abs(pointToFollowInPath.GetComponent<PathPointController>().speed) * 2;
                }
                else if (distanceToFollower > 30)
                {
                    me.speed = Mathf.Abs(pointToFollowInPath.GetComponent<PathPointController>().speed) * 3;
                }
                else
                {
                    me.speed = Mathf.Abs(pointToFollowInPath.GetComponent<PathPointController>().speed) - 0.1f;
                }


                Seek(pointToFollowInPath.transform.position);

                if (DebugPatrol)
                {
                    Debug.DrawLine(transform.position, pointToFollowInPath.transform.position, Color.magenta);
                    pointToFollowInPath.GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    pointToFollowInPath.GetComponent<MeshRenderer>().enabled = false;
                }

                break;
        }

    }
}
