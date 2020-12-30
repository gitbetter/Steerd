using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Wander", menuName = "Steerd/Wander", order = 1)]
    public class Wander : Face {
        public override Groups group { get { return Groups.Amble; } }
        public float wanderOffset;
        public float wanderRadius;
        public float wanderRate;
        public float maxAcceleration;

        private float wanderOrientation = 0;

        override public SteeringOutput GetSteering() {
            wanderOrientation += (Random.value - Random.value) * wanderRate;
            float targetOrientation = wanderOrientation + character.orientation;
            Vector3 characterOrientationVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * character.orientation), 0.0f, Mathf.Sin(Mathf.Deg2Rad * character.orientation));
            Vector3 targetOrientationVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * targetOrientation), 0.0f, Mathf.Sin(Mathf.Deg2Rad * targetOrientation));

            target.position = character.position + wanderOffset * characterOrientationVector;
            target.position += wanderRadius * targetOrientationVector;

            SteeringOutput output = base.GetSteering();
            if (output != null) {
                output.linear = maxAcceleration * characterOrientationVector;
            }
            return output;
        }
    }
}
