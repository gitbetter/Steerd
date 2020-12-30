using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Align", menuName = "Steerd/Align", order = 1)]
    public class Align : Behavior {
        public override Groups group { get { return Groups.Approach; } }
        public float maxAngularAcceleration;
        public float maxRotation;
        public float targetRadius;
        public float slowRadius;
        public float timeToTarget;

        override public SteeringOutput GetSteering() {
            SteeringOutput output = new SteeringOutput();
            float rotation = target.orientation - character.orientation;
            rotation = MapTo180Range(rotation);
            float rotationSize = Mathf.Abs(rotation);

            if (rotationSize <= targetRadius) {
                return null;
            }

            float targetRotation = 0;
            if (rotationSize > slowRadius) {
                targetRotation = maxRotation;
            } else {
                targetRotation = maxRotation * rotationSize / slowRadius;
            }
            targetRotation *= rotation / rotationSize;

            output.angular = (targetRotation - character.rotation * Mathf.Rad2Deg) / timeToTarget;
            float angularAcceleration = Mathf.Abs(output.angular);
            if (angularAcceleration > maxAngularAcceleration) {
                output.angular /= angularAcceleration;
                output.angular *= maxAngularAcceleration;
            }
            output.linear = new Vector3(0, 0, 0);
            return output;
        }

        private float MapTo180Range(float angle) {
            float newAngle = angle % 360;
            newAngle = (newAngle + 360) % 360;
            if (newAngle > 180) {
                newAngle -= 360;
            }
            return newAngle;
        }
    }
}
