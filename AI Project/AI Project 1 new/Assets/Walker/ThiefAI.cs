using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefAI : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    enum States
    {
        Seek = 0,
        Flee = 1,
        Wander = 2,
        Pursue = 3,
        None = 4
    }

    [SerializeField]
    States IASatate;

    [SerializeField]
    GameObject target;

    [SerializeField]
    float maxVelocity;

    [SerializeField]
    float minDistance;

    [SerializeField]
    float turnSpeed;

    bool NewWanderPoint;
    float distance;

    void SeekTarget(GameObject point)
    {
        agent.destination = point.transform.position;
    }

    void SeekPoint(Vector3 point)
    {
        agent.destination = point;
    }

    void FleeTarget(GameObject point)
    {
       // Vector3 fleeVector = point - transform.position;
       // agent.destination = transform.position- fleeVector;
    }

    void FleePoint(Vector3 point)
    {
        Vector3 fleeVector = point - transform.position;
        agent.destination = transform.position - fleeVector;
    }

    void Wander()
    {

        SeekPoint(RandomPoint(transform.position, 100));

        NewWanderPoint = false;
    }

    void Wander2()
    {
        float radius = Random.Range(0f, 10f);
        float offset = Random.Range(8f, 16f);

        Vector3 localTarget = Random.insideUnitCircle * radius;
        localTarget += new Vector3(0, 0, offset);

        Vector3 worldTarget = transform.TransformPoint(localTarget);
        worldTarget.y = 0f;

        // agent.destination = worldTarget;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(worldTarget, out hit, radius, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
        else
        {
            worldTarget = transform.TransformPoint(-localTarget);

            if (NavMesh.SamplePosition(worldTarget, out hit, radius, NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }
        }
    }

    void Pursue()
    {
        if (distance < minDistance + 2)
        {
            SeekTarget(target);
        }
        else
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float lookAhead = targetDir.magnitude / agent.speed;
            SeekPoint(target.transform.position + target.transform.forward * lookAhead);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        NewWanderPoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        //IASatate = States.Flee; // for test purposes

        //Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;
        float angle = 0;
        distance = Vector3.Distance(agent.destination, transform.position);
        
        switch (IASatate)
        {
            case States.Seek:
                {
                    SeekTarget(target);
                }
                break;
            case States.Flee:
                {
                   // FleeTarget(target);
                    FleePoint(target.transform.position);
                }
                break;
            case States.Wander:
                {
                    if (NewWanderPoint == true)
                    {
                        Wander2();
                    }
                }
                break;
            case States.Pursue:
                {
                    Pursue();
                }
                break;
            case States.None:
                {
                    
                }
                break;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        if(IASatate != States.Flee)
        {
            if ((distance > minDistance))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
                transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;
            }
            else
            {
                NewWanderPoint = true;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
            transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;
        }
       
    }

    // Other Usefull Functions

    Vector3 RandomPoint(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                Debug.DrawRay(hit.position, Vector3.up, Color.blue, 1.0f);
                return hit.position;
            }
        }

        return Vector3.zero;

    }
}
