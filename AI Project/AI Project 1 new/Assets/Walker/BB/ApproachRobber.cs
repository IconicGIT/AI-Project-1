using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Set Robber")]
[Help("Call for GetRobbed() in Target.")]
public class SetRobber : BasePrimitiveAction
{
    [InParam("game object")]
    [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
    public GameObject targetGameobject;
    public GameObject self;

    //[OutParam("target position")]
    //[Help("Vector3 for target's position.")]
    //public Vector3 targetPos;

    public override TaskStatus OnUpdate()
    {
        Debug.Log("next robber :" + self.GetComponent<CopBB>().nextRobber);
        self.GetComponent<CopBB>().nextRobber = self.GetComponent<CopBB>().robbersToApproach[0];
        targetGameobject = self.GetComponent<CopBB>().nextRobber;
        return TaskStatus.COMPLETED;
    }
}

