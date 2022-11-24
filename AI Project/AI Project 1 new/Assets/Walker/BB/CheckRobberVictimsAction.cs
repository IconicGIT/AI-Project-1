using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
using System.Linq;

[Action("MyActions/Check Robber victims")]
[Help("Get the Closest Free Cop.")]
public class CheckRobberVictimsAction : BasePrimitiveAction
{
    [InParam("self")]
    [Help("Nearest free cop.")]
    public GameObject self;

    [OutParam("game object")]
    [Help("Nearest free cop.")]
    public GameObject go;

    public override TaskStatus OnUpdate()
    {
        var l = self.GetComponent<CopBB>().robbersToApproach;
        if (l.Count() == 0)
            return TaskStatus.FAILED;
        go = l.First();
        return TaskStatus.COMPLETED;
    }
}