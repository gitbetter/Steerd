using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "LookWhereYoureGoing", menuName = "Steerd/LookWhereYoureGoing", order = 1)]
    public class LookWhereYoureGoing : Align {
        public override Groups group { get { return Groups.Approach; } }

        override public SteeringOutput GetSteering() {
            Vector3 velocity = character.velocity;
            if (velocity.magnitude == 0) {
                return null;
            }

            target.orientation = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

            return base.GetSteering();
        }
    }
}
