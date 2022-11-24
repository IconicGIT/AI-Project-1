using UnityEngine;
 
using Pada1.BBCore;
using Pada1.BBCore.Framework; 

[Condition("MyConditions/Check Robber Victims")]
[Help("Checks whether Cop is near the Treasure.")]
public class CheckRobberVictims : ConditionBase
{
    [InParam("self")]
    [Help("Self")]
    public GameObject self;
    [InParam("NextRobber")]
    [Help("Self")]
    public GameObject nextRobber;

    public override bool Check()
    {
        Debug.Log("copBB robber count: " + self.GetComponent<CopBB>().robbersToApproach.Count);

        bool ret = false;
        if (self.GetComponent<CopBB>().robbersToApproach.Count > 0)
        {
            ret = true;
            nextRobber = self.GetComponent<CopBB>().robbersToApproach[0];
            Debug.Log("copBB next robber: " + nextRobber.name);
        }
        return ret;
    }
} 
