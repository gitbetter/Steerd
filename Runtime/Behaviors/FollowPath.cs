using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    [CreateAssetMenu(fileName = "FollowPath", menuName = "Steerd/FollowPath", order = 1)]
    public class FollowPath : Seek {
        public override Flags flags { get { return Flags.PATH_FOLLOWER; } }
        public override Groups group { get { return Groups.Approach; } }
        public float pathOffset;
        public float currentParam = 0;
        public float predictTime;

        override public SteeringOutput GetSteering() {
            Vector3 futurePos = character.position + character.velocity * predictTime;
            currentParam = path.GetParam(futurePos, currentParam);
            float targetParam = currentParam + pathOffset;
            target.position = path.GetPosition(targetParam);
            return base.GetSteering();
        }
    }
}
