using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    public enum Types {
        Seek,
        Flee,
        Arrive,
        Align,
        VelocityMatch,
        Pursue,
        Evade,
        Face,
        LookWhereYoureGoing,
        Wander,
        FollowPath,
        Separation,
        CollisionAvoidance,
        ObstacleAvoidance
    }

    public enum Groups {
        Collide,
        Approach,
        Avoid,
        Amble,
        TotalGroups
    }

    public enum Flags {
        NONE = 0,
        SINGLE_TARGET = 1,
        MULTI_TARGET = 2,
        PATH_FOLLOWER = 4
    }

    public class Kinematic {
        public Vector3 position;
        public Vector3 velocity;
        public float orientation;
        public float rotation;
    }

    public class SteeringOutput
    {
        public Vector3 linear;
        public float angular;

        public static SteeringOutput operator +(SteeringOutput lhs, SteeringOutput rhs) {
            SteeringOutput result = new SteeringOutput();
            result.linear = lhs.linear + rhs.linear;
            result.angular = lhs.angular + rhs.angular;
            return result;
        }

        public static SteeringOutput operator *(SteeringOutput lhs, float rhs) {
            SteeringOutput result = new SteeringOutput();
            result.linear = lhs.linear * rhs;
            result.angular = lhs.angular * rhs;
            return result;
        }

        public void Clear() {
            this.linear = Vector3.zero;
            this.angular = 0;
        }
    }

    public abstract class Behavior : ScriptableObject {
        public virtual Flags flags { get { return Flags.SINGLE_TARGET; } }
        public virtual Groups group { get { return Groups.Amble; } }
        public virtual Kinematic character { get; set; }
        public virtual Kinematic target { get; set; }
        public virtual List<Kinematic> targets { get; set; }
        public virtual Path path { get; set; }
        public abstract SteeringOutput GetSteering();

        public virtual void Initialize(Kinematic target, Kinematic character) {
            this.target = target;
            this.character = character;
        }

        public static Kinematic RigidbodyToKinematic(Rigidbody rigidbody) {
            Kinematic kinematic = new Kinematic();
            kinematic.position = rigidbody.position;
            kinematic.orientation = rigidbody.rotation.eulerAngles.y;
            kinematic.velocity = rigidbody.velocity;
            kinematic.rotation = rigidbody.angularVelocity.y;
            return kinematic;
        }
    }
}
