using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/RobTarget")]
[Help("Call for GetRobbed() in Target.")]
public class RobTarget : BasePrimitiveAction
{
    [InParam("game object")]
    [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
    public GameObject targetGameobject;
    public GameObject me;

    //[OutParam("target position")]
    //[Help("Vector3 for target's position.")]
    //public Vector3 targetPos;

    public override TaskStatus OnUpdate()
    {
        targetGameobject.GetComponent<WalkerBB>().GetRobbed(me);
        return TaskStatus.COMPLETED;
    }
}

