using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    /// <summary>
    /// It is a perception condition to check if the objective is close depending on a given distance.
    /// </summary>
    [Condition("Perception/Is Target Active")]
    [Help("Check if robber is flying")]
    public class IsTargetActive : GOCondition
    {
        ///<value>Input Target Parameter to to check the distance.</value>
        [InParam("target")]
        public GameObject target;

        /// <summary>
        /// Checks whether a target is close depending on a given distance,
        /// calculates the magnitude between the gameobject and the target and then compares with the given distance.
        /// </summary>
        /// <returns>True if the magnitude between the gameobject and de target is lower that the given distance.</returns>
        public override bool Check()
        {
            return target && target.activeSelf;
        }
    }
}