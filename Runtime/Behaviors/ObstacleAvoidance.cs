using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd {
    public class Collision {
        public Vector3 position;
        public Vector3 normal;
    }

    public class CollisionDetector {
        public Collision GetCollision(Vector3 position, Vector3 direction, float moveAmount) {
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
            Vector3 whisker1 = Quaternion.Euler(0, -20, 0) * direction;
            Vector3 whisker2 = Quaternion.Euler(0, 20, 0) * direction;
            RaycastHit hit;
            Collision collision = null;
            if (Physics.Raycast(position, whisker1, out hit, moveAmount * 0.5f, layerMask)) {
                Debug.DrawRay(position, whisker1 * moveAmount * 0.5f, Color.yellow);
                collision = new Collision();
                collision.position = hit.point;
                collision.normal = hit.normal;
            } else if (Physics.Raycast(position, whisker2, out hit, moveAmount * 0.5f, layerMask)) {
                Debug.DrawRay(position, whisker2 * moveAmount * 0.5f, Color.yellow);
                collision = new Collision();
                collision.position = hit.point;
                collision.normal = hit.normal;
            } else if (Physics.Raycast(position, direction, out hit, moveAmount, layerMask)) {
                Debug.DrawRay(position, direction * moveAmount, Color.yellow);
                collision = new Collision();
                collision.position = hit.point;
                collision.normal = hit.normal;
            }
            return collision;
        }
    }

    [CreateAssetMenu(fileName = "ObstacleAvoidance", menuName = "Steerd/ObstacleAvoidance", order = 1)]
    public class ObstacleAvoidance : Seek {
        public override Groups group { get { return Groups.Collide; } }
        public float avoidDistance;
        public float lookAhead;

        private CollisionDetector collisionDetector = new CollisionDetector();

        override public SteeringOutput GetSteering() {
            Collision collision = collisionDetector.GetCollision(character.position, character.velocity.normalized, lookAhead);
            if (collision == null) {
                return null;
            }
            target.position = collision.position + collision.normal * avoidDistance;
            return base.GetSteering();
        }
    }
}
