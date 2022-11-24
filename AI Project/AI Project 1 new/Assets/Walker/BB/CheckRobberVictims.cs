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

    public override bool Check()
    {
        return self.GetComponent<CopBB>().robbersToApproach.Count > 1;
    }
} 
