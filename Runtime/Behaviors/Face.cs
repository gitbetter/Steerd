using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "Face", menuName = "Steerd/Face", order = 1)]
    public class Face : Align {
        public override Kinematic target { get; set; }

        override public SteeringOutput GetSteering() {
            Vector3 direction = target.position - character.position;
            if (direction.magnitude == 0) {
                return null;
            }

            target.orientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            return base.GetSteering();
        }
    }
}
