using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Seek", menuName = "Steerd/Seek", order = 1)]
    public class Seek : Behavior {
        public override Groups group { get { return Groups.Approach; } }
        
        public float maxAcceleration;

        override public SteeringOutput GetSteering() {
            SteeringOutput output = new SteeringOutput();
            output.linear = target.position - character.position;
            output.linear = output.linear.normalized * maxAcceleration;
            output.angular = 0;
            return output;
        }
    }
}
