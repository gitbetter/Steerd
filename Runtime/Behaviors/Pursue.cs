using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Pursue", menuName = "Steerd/Pursue", order = 1)]
    public class Pursue : Arrive {
        public override Groups group { get { return Groups.Approach; } }
        
        public float maxPredictionTime;

        public override Kinematic target { get; set; }

        override public SteeringOutput GetSteering() {
            Vector3 direction = target.position - character.position;
            float distance = direction.magnitude;

            float speed = character.velocity.magnitude;
            float predictionTime = 0;
            if (speed <= (distance / maxPredictionTime)) {
                predictionTime = maxPredictionTime;
            } else {
                predictionTime = distance / speed;
            }

            Vector3 targetVelocity = target.velocity;
            target.position += targetVelocity * predictionTime;

            return base.GetSteering();
        }
    }
}
