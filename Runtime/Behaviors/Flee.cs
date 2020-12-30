using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Flee", menuName = "Steerd/Flee", order = 1)]
    public class Flee : Behavior {
        public override Groups group { get { return Groups.Avoid; } }

        public float maxAcceleration;

        override public SteeringOutput GetSteering() {
            SteeringOutput output = new SteeringOutput();
            output.linear = character.position - target.position;
            output.linear = output.linear.normalized * maxAcceleration;
            output.angular = 0;
            return output;
        }
    }
}
