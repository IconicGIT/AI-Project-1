using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
using System.Linq;

[Action("MyActions/Update pig current apple")]
[Help("Get the Closest Free Cop.")]
public class UpdatePigCurrentApple : BasePrimitiveAction
{
    [InParam("self")]
    [Help("pig :)")]
    public GameObject self;

    [OutParam("currentApple")]
    [Help("apple >:)")]
    public GameObject apple;

    public override TaskStatus OnUpdate()
    {
        self.GetComponent<PigBB>().GetNearestApple();
        apple = self.GetComponent<PigBB>().currentApple;
        return TaskStatus.COMPLETED;
    }
}