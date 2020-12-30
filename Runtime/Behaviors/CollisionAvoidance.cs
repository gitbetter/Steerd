using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "CollisionAvoidance", menuName = "Steerd/CollisionAvoidance", order = 1)]
    public class CollisionAvoidance : Behavior{
        public override Flags flags { get { return Flags.MULTI_TARGET; } }
        public override Groups group { get { return Groups.Collide; } }
        public float maxAcceleration;

        public float radius;

        override public SteeringOutput GetSteering() {
            float shortestTime = Mathf.Infinity;
            Kinematic firstTarget = null;
            float firstMinSeparation = 0;
            float firstDistance = 0;
            Vector3 firstRelativePosition = Vector3.zero;
            Vector3 firstRelativeVelocity = Vector3.zero;

            foreach (Kinematic t in targets) {
                Vector3 relativePosition = character.position - t.position;
                Vector3 relativeVelocity = t.velocity - character.velocity;
                float relativeSpeed = relativeVelocity.magnitude;
                float timeToCollision = Vector3.Dot(relativePosition, relativeVelocity) / (relativeSpeed * relativeSpeed);

                float distance = relativePosition.magnitude;
                float minSeparation = distance - relativeSpeed * timeToCollision;
                if (minSeparation > 2 * radius) {
                    continue;
                }

                if (timeToCollision > 0 && timeToCollision < shortestTime) {
                    shortestTime = timeToCollision;
                    firstTarget = t;
                    firstMinSeparation = minSeparation;
                    firstDistance = distance;
                    firstRelativePosition = relativePosition;
                    firstRelativeVelocity = relativeVelocity;
                }
            }

            if (firstTarget == null) {
                return null;
            }

            Vector3 position;
            if (firstMinSeparation <= 0 || firstDistance < 2 * radius) {
                position = character.position - firstTarget.position;
            } else {
                position = firstRelativePosition + firstRelativeVelocity * shortestTime;
            }

            SteeringOutput output = new SteeringOutput();
            output.linear = position.normalized * maxAcceleration;
            output.angular = 0;
            return output;
        }
    }
}
