using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerd
{
    public class PathEdge {
        public Vector3 point1;
        public Vector3 point2;
        
        public PathEdge(Vector3 a, Vector3 b) {
            this.point1 = a;
            this.point2 = b;
        }
    }

    public class Path : MonoBehaviour {
        private List<PathEdge> edges;

        void Start() {
            for (int i = 0; i < transform.childCount - 1; i++) {
                AddEdge(new PathEdge(transform.GetChild(i).position, transform.GetChild(i+1).position));
            }
        }

        public float GetParam(Vector3 position, float lastParam) {
            float param = 0;
            float pathDistance = 0;
            PathEdge currentEdge = null;
            foreach (PathEdge edge in edges) {
                pathDistance += Vector3.Distance(edge.point1, edge.point2);
                if (lastParam <= pathDistance) {
                    currentEdge = edge;
                    break;
                }
            }
            if (currentEdge == null) {
                return param;
            }
            Vector3 currentPosition = position - currentEdge.point1;
            Vector3 edgeDirection = (currentEdge.point2 - currentEdge.point1).normalized;
            Vector3 pointInEdge = Vector3.Project(currentPosition, edgeDirection);
            param = pathDistance - Vector3.Distance(currentEdge.point1, currentEdge.point2);
            param += pointInEdge.magnitude;
            return param;
        }

        public Vector3 GetPosition(float param) {
            Vector3 position = Vector3.zero;
            PathEdge currentEdge = null;
            float pathDistance = 0;
            foreach (PathEdge edge in edges) {
                pathDistance += Vector3.Distance(edge.point1, edge.point2);
                if (param <= pathDistance) {
                    currentEdge = edge;
                    break;
                }
            }
            if (currentEdge == null) {
                return position;
            }
            Vector3 edgeDirection = (currentEdge.point2 - currentEdge.point1).normalized;
            pathDistance -= Vector3.Distance(currentEdge.point1, currentEdge.point2);
            pathDistance = param - pathDistance;
            position = currentEdge.point1 + edgeDirection * pathDistance;
            return position;
        }

        public void AddEdge(PathEdge edge) {
            if (edges == null) {
                edges = new List<PathEdge>();
            }
            edges.Add(edge);
        }
    }   
}
