using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Set Robber")]
[Help("Call for GetRobbed() in Target.")]
public class SetRobber : BasePrimitiveAction
{
    [InParam("target")]
    [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
    public GameObject targetGameobject;
    [InParam("self ")]
    public GameObject self;

    [InParam("target pos ")]
    public Vector3 targetPos;


    //[OutParam("target position")]
    //[Help("Vector3 for target's position.")]
    //public Vector3 targetPos;

    public override TaskStatus OnUpdate()
    {
        self.GetComponent<CopBB>().nextRobber = self.GetComponent<CopBB>().robbersToApproach[0];
        targetGameobject = self.GetComponent<CopBB>().nextRobber;
        targetPos = targetGameobject.transform.position;
        Debug.Log("next robber :" + self.GetComponent<CopBB>().nextRobber);

        return TaskStatus.COMPLETED;
    }
}

