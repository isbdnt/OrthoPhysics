using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtensions;

namespace OrthoPhysics.UnityPlayer.Tests
{
    public class TestZOrderCurve : MonoBehaviour
    {
        [Serializable]
        class Node
        {
            public ulong code;
            public Vector2 position;
            public Node(Vector2 position)
            {
                this.position = position;
            }
        }

        [SerializeField] List<Node> _nodes = new List<Node>();

        private void Awake()
        {
            var a = ZOrderCurve.Encode(2, 2);
            var b = ZOrderCurve.Encode(6, 6);
            var c = ZOrderCurve.Encode(10, 10);
        }

        private void FixedUpdate()
        {
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _nodes.Count - 1; i++)
            {
                Gizmos.DrawLine(_nodes[i].position, _nodes[i + 1].position);
            }
            foreach (var node in _nodes)
            {
                GizmoUtility.DrawSphere(node.position, Color.red, 0.5f);
            }
        }
    }
}