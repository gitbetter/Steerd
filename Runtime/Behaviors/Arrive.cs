using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Arrive", menuName = "Steerd/Arrive", order = 1)]
    public class Arrive : Behavior{
        public override Groups group { get { return Groups.Approach; } }
        public float maxAcceleration;
        public float maxSpeed;
        public float targetRadius;
        public float slowRadius;
        public float timeToTarget;

        override public SteeringOutput GetSteering() {
            SteeringOutput output = new SteeringOutput();
            Vector3 direction = target.position - character.position;
            float distance = direction.magnitude;
            float targetSpeed = 0;

            if (distance < targetRadius) {
                return null;
            }

            if (distance > slowRadius) {
                targetSpeed = maxSpeed;
            } else {
                targetSpeed = maxSpeed * distance / slowRadius;
            }
            
            Vector3 targetVelocity = direction.normalized * targetSpeed;

            output.linear = (targetVelocity - character.velocity) / timeToTarget;
            if (output.linear.magnitude > maxAcceleration) {
                output.linear = output.linear.normalized * maxAcceleration;
            }
            output.angular = 0;
            return output;
        }
    }
}
