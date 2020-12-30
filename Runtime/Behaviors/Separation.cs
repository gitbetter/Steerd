using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Separation", menuName = "Steerd/Separation", order = 1)]
    public class Separation : Behavior{
        public override Flags flags { get { return Flags.MULTI_TARGET; } }
        public override Groups group { get { return Groups.Avoid; } }
        public float maxAcceleration;
        public float threshold;
        public float decayCoefficient;

        override public SteeringOutput GetSteering() {
            SteeringOutput output = new SteeringOutput();
            foreach (Kinematic t in targets) {
                Vector3 direction = character.position - t.position;
                float distance = direction.magnitude;
                if (distance < threshold) {
                    float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
                    output.linear += strength * direction.normalized;
                }
            }
            return output;
        }
    }
}
