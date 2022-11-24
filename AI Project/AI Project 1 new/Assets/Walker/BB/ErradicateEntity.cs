using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
using System.Linq;

[Action("MyActions/Erradicate Entity")]
[Help("Get the Closest Free Cop.")]
public class ErradicateEntity : BasePrimitiveAction
{
    [InParam("self")]
    [Help("Nearest free cop.")]
    public GameObject self;


    [InParam("entity")]
    [Help("Nearest free cop.")]
    public GameObject entity;

    public override TaskStatus OnUpdate()
    {
       if (entity)
            self.GetComponent<CopBB>().ErradicateEntity(entity);

        return TaskStatus.COMPLETED;
    }
}