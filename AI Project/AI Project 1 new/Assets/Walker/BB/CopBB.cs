using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopBB : MonoBehaviour
{
    public List<GameObject> robbersToApproach;
    public GameObject nextRobber;

    public void NotifyRobber(GameObject robber)
    {
        robbersToApproach.Add(robber);
    }

    public void ApproachNextRobber()
    {

    }

    public void ErradicateRobber()
    {

        robbersToApproach.Remove(nextRobber);
    }

}
