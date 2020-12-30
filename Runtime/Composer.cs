using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd
{
    [System.Serializable]
    public class BehaviorInfo {
        [SerializeField]
        public Behavior behavior;

        [SerializeField]
        public Rigidbody target;

        [SerializeField]
        public Path path;

        [SerializeField]
        public List<Rigidbody> targets;

        [SerializeField]
        public float blendingWeight;
    }

    public class Composer : MonoBehaviour
    {
        [SerializeField]
        public BehaviorInfo[] BehaviorInfos;

        [SerializeField]
        public Vector3 maxAcceleration;

        [SerializeField]
        public float maxRotation;

        private List<List<BehaviorInfo>> behaviorGroups = new List<List<BehaviorInfo>>();

        private Rigidbody rigidBody;

        void Start() {
            for (int i = 0; i < (int)Groups.TotalGroups; i++) {
                behaviorGroups.Add(new List<BehaviorInfo>());
            }
            
            rigidBody = GetComponent<Rigidbody>();
            InitializeSteeringBehaviors();
        }

        void FixedUpdate() {
            UpdateSteeringBehaviors();
        }

        public SteeringOutput GetSteering() {
            SteeringOutput steering = new SteeringOutput();
            for (int i = 0; i < behaviorGroups.Count; i++) {
                steering.Clear();
                for (int j = 0; j < behaviorGroups[i].Count; j++) {
                    SteeringOutput behaviorSteering = behaviorGroups[i][j].behavior.GetSteering();
                    if (behaviorSteering != null) {
                        steering += behaviorSteering * behaviorGroups[i][j].blendingWeight;
                    }
                }
                if (steering.linear.magnitude > Mathf.Epsilon || Mathf.Abs(steering.angular) > Mathf.Epsilon) {
                    return ClampSteering(steering);
                }
            }
            return ClampSteering(steering);
        }

        void InitializeSteeringBehaviors() {
            foreach (BehaviorInfo info in BehaviorInfos) {
                info.behavior.character = Behavior.RigidbodyToKinematic(rigidBody);
                if ((info.behavior.flags & Flags.SINGLE_TARGET) != Flags.NONE) {
                    info.behavior.target = Behavior.RigidbodyToKinematic(info.target);
                }
                if ((info.behavior.flags & Flags.PATH_FOLLOWER) != Flags.NONE) {
                    info.behavior.path = info.path;
                }
                if ((info.behavior.flags & Flags.MULTI_TARGET) != Flags.NONE) {
                    info.behavior.targets = new List<Kinematic>();
                    foreach (Rigidbody rb in info.targets) {
                        info.behavior.targets.Add(Behavior.RigidbodyToKinematic(rb));
                    }
                }
                behaviorGroups[(int)info.behavior.group].Add(info);
            }
        }

        void UpdateSteeringBehaviors() {
            for (int i = 0; i < behaviorGroups.Count; i++) {
                for (int j = 0; j < behaviorGroups[i].Count; j++) {
                    BehaviorInfo info = behaviorGroups[i][j];
                    info.behavior.character = Behavior.RigidbodyToKinematic(rigidBody);
                    if ((info.behavior.flags & Flags.SINGLE_TARGET) != Flags.NONE
                        && info.target != null) {
                        info.behavior.target = Behavior.RigidbodyToKinematic(info.target);
                    }
                    if ((info.behavior.flags & Flags.PATH_FOLLOWER) != Flags.NONE
                        && info.path != null) {
                        info.behavior.path = info.path;
                    }
                    if ((info.behavior.flags & Flags.MULTI_TARGET) != Flags.NONE) {
                        for (int k = 0; k < info.targets.Count; k++) {
                            info.behavior.targets[k] = Behavior.RigidbodyToKinematic(info.targets[k]);
                        }
                    }
                }
            }
        }

        SteeringOutput ClampSteering(SteeringOutput steering) {
            steering.linear = new Vector3(Mathf.Clamp(steering.linear.x, -maxAcceleration.x, maxAcceleration.x),
                                            Mathf.Clamp(steering.linear.y, -maxAcceleration.y, maxAcceleration.y),
                                            Mathf.Clamp(steering.linear.z, -maxAcceleration.z, maxAcceleration.z));
            steering.angular = Mathf.Clamp(steering.angular, -maxRotation, maxRotation);
            return steering;
        }
    }   
}