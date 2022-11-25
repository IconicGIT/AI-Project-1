using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    /// <summary>
    /// It is a perception condition to check if the objective is close depending on a given distance.
    /// </summary>
    [Condition("Perception/IsFlying")]
    [Help("Check if robber is flying")]
    public class IsFlying: GOCondition
    {
        ///<value>Input Target Parameter to to check the distance.</value>
        [InParam("robber")]
        [Help("Robber self")]
        public GameObject self;

        /// <summary>
        /// Checks whether a target is close depending on a given distance,
        /// calculates the magnitude between the gameobject and the target and then compares with the given distance.
        /// </summary>
        /// <returns>True if the magnitude between the gameobject and de target is lower that the given distance.</returns>
        public override bool Check()
        {
            return self.GetComponent<RobberBB>().fly;
        }
    }
}