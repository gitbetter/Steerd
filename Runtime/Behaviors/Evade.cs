using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Evade", menuName = "Steerd/Evade", order = 1)]
    public class Evade : Flee {
        public override Groups group { get { return Groups.Avoid; } }

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

            base.target = new Kinematic();
            base.target.position += target.velocity * predictionTime;

            return base.GetSteering();
        }
    }
}
