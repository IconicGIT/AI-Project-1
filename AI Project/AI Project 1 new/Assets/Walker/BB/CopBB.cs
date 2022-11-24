using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopBB : MonoBehaviour
{
    public List<GameObject> robbersToApproach;
    public GameObject nextRobber;

    public void NotifyRobber(GameObject robber)
    {
        //print("Cop robber: " + robber.name);
        nextRobber = robber;
        
        if (!robbersToApproach.Contains(robber))
            robbersToApproach.Add(robber);
    }

    public void ApproachNextRobber()
    {

    }

    public void ErradicateEntity(GameObject entity)
    {

        robbersToApproach.Remove(entity);
        entity.GetComponent<RobberBB>().Fly();
    }

}
