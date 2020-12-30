using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "VelocityMatch", menuName = "Steerd/VelocityMatch", order = 1)]
    public class VelocityMatch : Behavior{
        public override Groups group { get { return Groups.Approach; } }
        public float maxAcceleration;
        public float timeToTarget;

        override public SteeringOutput GetSteering() {
            SteeringOutput output = new SteeringOutput();
            output.linear = (target.velocity - character.velocity) / timeToTarget;
            if (output.linear.magnitude > maxAcceleration) {
                output.linear = output.linear.normalized * maxAcceleration;
            }
            output.angular = 0;
            return output;
        }
    }
}
