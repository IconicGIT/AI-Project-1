using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlockEntities : MonoBehaviour
{
    public FlockingManager myManager;

    public float speed;
    private Vector3 direction;

    private float timePassed;
    private float seconds;

    [SerializeField]
    private float maxChasingTime;
    [SerializeField]
    private float currentChasingTime;

    public GameObject target;
    Vector3 randomVector;

    
    private GameObject[] beesPosibleTargets;
    private GameObject Nest;
    [SerializeField]
    float MinDangerDistance;
    //private bool turning;

    void Start()
    {
        seconds = 0.25f;
        timePassed = seconds;
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
        Nest = myManager.entitiesTarget;
        randomVector.x = Random.Range(-0.15f, 0.15f);
        randomVector.y = Random.Range(-0.15f, 0.15f);
        randomVector.z = Random.Range(-0.15f, 0.15f);

        beesPosibleTargets = GameObject.FindGameObjectsWithTag("beeTarget");
        target = Nest;
    }

    void Update()
    { 
        if (timePassed >= seconds)
        {
            if (Random.Range(0.0f, 100.0f) < 75.0f)
            {
                FlockingRules();
            }

            if (Random.Range(0.0f, 100.0f) < 10.0f)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }

            timePassed = 0.0f;
        }
        timePassed += Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        transform.Translate(0.0f, 0.0f, Time.deltaTime * speed);

        if (currentChasingTime > 0.0f)
        {
            currentChasingTime -= Time.deltaTime;
        }
        else
        {
            if (target.GetComponent<AgentBehavior>() != null)
            {
                target.GetComponent<AgentBehavior>().isBeingChased = false;
                target.GetComponent<AgentBehavior>().target = null;
            }
            target = Nest;
            
        }
       
    }

    void FlockingRules() // posicion del leader - posicion del flocking guy, que el lider se espere
    {
        //Debug.Log("Rules created");
        Vector3 cohesion = Vector3.zero;
        Vector3 align = Vector3.zero;
        Vector3 separation = Vector3.zero;
        Vector3 leader = Vector3.zero;
        GameObject beeTarget = Nest;

        if (target != Nest) beeTarget = target;
        
        int num = 0;
        float distanceToBeetarget = Mathf.Infinity;

        if (target == Nest)
        {
            foreach (GameObject agent in beesPosibleTargets)
            {
                float currentDistance = Vector3.Distance(agent.transform.position, Nest.transform.position);
                //print("current distance: " + distanceToBeetarget);
                if (currentDistance < distanceToBeetarget)
                {

                    distanceToBeetarget = currentDistance;

                    if (distanceToBeetarget < MinDangerDistance)
                    {
                        beeTarget = agent;
                        currentChasingTime = maxChasingTime;
                        if (Vector3.Distance(beeTarget.transform.position, Nest.transform.position) < MinDangerDistance)
                        {


                            
                        }
                    }
                }
            }
        }

        if(beeTarget == Nest)
        {
            //print("target is Nest");
            WaitingMode();
        }
        else
        {
            //print("target is guy " + target.gameObject);

            ChasingMode(beeTarget);
        }
        
        foreach (GameObject go in myManager.allFlockingEntities)
        {
            if (go != this.gameObject)
            {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance <= myManager.neighbourDistance)
                {
                    // Cohesion
                    cohesion += go.transform.position;

                    // Align
                    align += go.GetComponent<FlockEntities>().direction;
                    num++;

                    // Separation
                    separation -= (transform.position - go.transform.position) / (distance * 0.1f );

                    // Follow the leader
                    Vector3 position;
                    position.x = target.transform.position.x;
                    position.y = target.transform.position.y + 2.5f;
                    position.z = target.transform.position.z;

                    leader = position - transform.position;
                    
                }
            }
        }

        if (num > 0)
        {
            // Cohesion
            cohesion = (cohesion / num - transform.position).normalized * speed;

            // Align
            align /= num;
            speed = Mathf.Clamp(align.magnitude, myManager.minSpeed, myManager.maxSpeed);
        }

        //Debug.Log("" + leader);
        // Final computation
        Vector3 positionCorrection = Vector3.zero;
        positionCorrection = target.transform.position - transform.position;

        float instanceDistance = Vector3.Distance(target.transform.position, transform.position);

        if (instanceDistance > 15.0f)
        {
            direction = (positionCorrection + randomVector).normalized * speed;
        }
        else
        {
            direction = ((cohesion + align + separation /*+ leader*/).normalized + (leader.normalized * 4) + randomVector) * speed;
        }
        

       //Debug.DrawRay(transform.position, direction, Color.white);
       //Debug.DrawRay(transform.position, cohesion, Color.red);
       //Debug.DrawRay(transform.position, align, Color.green);
       //Debug.DrawRay(transform.position, separation, Color.blue);
    }

    void ChasingMode(GameObject guy)
    {
        seconds = 0.8f;
        target = guy;
        
        randomVector.y = Random.Range(-0.15f, 0.15f);

        if (guy.GetComponent<AgentBehavior>() != null)
        {
            guy.GetComponent<AgentBehavior>().isBeingChased = true;
            //Debug.Log(guy.gameObject + " traget is: " + guy.GetComponent<AgentBehavior>().target);
            guy.GetComponent<AgentBehavior>().target = gameObject;
            //Debug.Log(guy.gameObject + " traget is: " + guy.GetComponent<AgentBehavior>().target);

        }

    }

    void WaitingMode() 
    {
        seconds = 0.25f;
        target = Nest;
        randomVector.y = Random.Range(-1.15f, 1.15f);
    }
}
