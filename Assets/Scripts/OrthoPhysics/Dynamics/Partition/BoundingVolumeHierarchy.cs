using FixMathematics;
using System;
using System.Collections.Generic;

namespace OrthoPhysics
{
    public class BoundingVolumeHierarchy
    {
        public class Node
        {
            public BoundingVolumeHierarchy hierarchy;
            public Bounds2D bounds;
            public ulong key;
            public ulong minKey;
            public ulong maxKey;
            public int height;
            public Node parent;
            public Node leftChild;
            public Node rightChild;
            public BodyLinkedList bodies;
        }

        Node _rootNode;
        Stack<Node> _nodeStack = new Stack<Node>();

        public BoundingVolumeHierarchy()
        {
        }

        public void AddBody(Body body)
        {
            ulong key = GetKey(body.bounds.center.xz);
            if (body.bvhNode != null)
            {
                if (body.bvhNode.key == key)
                {
                    UpdateLeafNode(body.bvhNode);
                    return;
                }
                RemoveBodyOnLeafNode(body);
            }
            if (_rootNode == null)
            {
                _rootNode = CreateLeafNode(body, key);
            }
            else
            {
                var targetLeafNode = SearchClosestLeafNode(_rootNode, key);
                if (targetLeafNode.key == key)
                {
                    AddBodyToLeafNode(body, targetLeafNode);
                }
                else
                {
                    InsertLeafNodeAt(CreateLeafNode(body, key), targetLeafNode);
                }
            }
        }

        public bool RemoveBody(Body body)
        {
            if (body.bvhNode == null)
            {
                return false;
            }

            RemoveBodyOnLeafNode(body);
            return true;
        }

        public void TraverseBounds(Action<Bounds2D, int> action)
        {
            if (_rootNode == null)
            {
                return;
            }
            _nodeStack.Push(_rootNode);
            while (_nodeStack.Count > 0)
            {
                var currentNode = _nodeStack.Pop();
                action(currentNode.bounds, currentNode.height);
                if (currentNode.bodies == null)
                {
                    _nodeStack.Push(currentNode.leftChild);
                    _nodeStack.Push(currentNode.rightChild);
                }
            }
        }

        public void OverlapBounds(Bounds2D bounds, List<Body> results)
        {
            if (_rootNode == null)
            {
                return;
            }
            _nodeStack.Push(_rootNode);
            while (_nodeStack.Count > 0)
            {
                var currentNode = _nodeStack.Pop();
                if (currentNode.bodies == null)
                {
                    if (Bounds2D.Intersect(bounds, currentNode.leftChild.bounds))
                    {
                        _nodeStack.Push(currentNode.leftChild);
                    }
                    if (Bounds2D.Intersect(bounds, currentNode.rightChild.bounds))
                    {
                        _nodeStack.Push(currentNode.rightChild);
                    }
                }
                else
                {
                    BodyLinkedList bodies = currentNode.bodies;
                    while (bodies != null)
                    {
                        if (!bodies.current.isKinematic && Bounds.Intersect2D(bounds, bodies.current.bounds))
                        {
                            results.Add(bodies.current);
                        }
                        bodies = bodies.next;
                    }
                }
            }
        }

        ulong GetKey(FixVector2 position)
        {
            return ZOrderCurve.Encode(Fix64.RoundToInt(position.x), Fix64.RoundToInt(position.y));
        }

        Node SearchClosestLeafNode(Node searchNode, ulong key)
        {
            while (true)
            {
                if (key < searchNode.key)
                {
                    if (searchNode.leftChild == null)
                    {
                        return searchNode;
                    }
                    searchNode = searchNode.leftChild;
                }
                else
                {
                    if (searchNode.rightChild == null)
                    {
                        return searchNode;
                    }
                    searchNode = searchNode.rightChild;
                }
            }
        }

        Node CreateLeafNode(Body body, ulong key)
        {
            var leafNode = new Node();
            leafNode.hierarchy = this;
            leafNode.bounds = body.bounds.xz;
            leafNode.key = key;
            leafNode.minKey = key;
            leafNode.maxKey = key;
            leafNode.bodies = new BodyLinkedList(body, null);
            body.bvhNode = leafNode;
            return leafNode;
        }

        void AddBodyToLeafNode(Body body, Node leafNode)
        {
            leafNode.bounds = Bounds2D.Combine(leafNode.bounds, body.bounds.xz);
            leafNode.key = GetKey(leafNode.bounds.center);
            leafNode.minKey = leafNode.key;
            leafNode.maxKey = leafNode.key;
            leafNode.bodies = new BodyLinkedList(body, leafNode.bodies);
            body.bvhNode = leafNode;
            UpdateInteriorNodeUntilRootNode(leafNode.parent);
        }

        void UpdateLeafNode(Node leafNode)
        {
            leafNode.bounds = leafNode.bodies.current.bounds.xz;
            BodyLinkedList bodies = leafNode.bodies.next;
            while (bodies != null)
            {
                leafNode.bounds = Bounds2D.Combine(leafNode.bounds, bodies.current.bounds.xz);
                bodies = bodies.next;
            }
            leafNode.key = GetKey(leafNode.bounds.center);
            leafNode.minKey = leafNode.key;
            leafNode.maxKey = leafNode.key;
            UpdateInteriorNodeUntilRootNode(leafNode.parent);
        }

        void RemoveBodyOnLeafNode(Body body)
        {
            Node leafNode = body.bvhNode;
            body.bvhNode = null;
            if (leafNode.bodies.next == null)
            {
                // Only one body, just remove the leaf node.
                RemoveLeafNode(leafNode);
            }
            else
            {
                BodyLinkedList bodies = leafNode.bodies;
                BodyLinkedList previousBodies = null;
                leafNode.bounds = Bounds2D.invalid;
                while (bodies != null)
                {
                    if (bodies.current == body)
                    {
                        if (previousBodies == null)
                        {
                            leafNode.bodies = bodies.next;
                        }
                        else
                        {
                            previousBodies.next = bodies.next;
                        }
                    }
                    else
                    {
                        leafNode.bounds = Bounds2D.SafeCombine(leafNode.bounds, bodies.current.bounds.xz);
                    }
                    previousBodies = bodies;
                    bodies = bodies.next;
                }
                leafNode.key = GetKey(leafNode.bounds.center);
                leafNode.minKey = leafNode.key;
                leafNode.maxKey = leafNode.key;
                UpdateInteriorNodeUntilRootNode(leafNode.parent);
            }
        }

        void RemoveLeafNode(Node leafNode)
        {
            var interiorNode = leafNode.parent;
            if (interiorNode == null)
            {
                _rootNode = null;
                return;
            }
            var siblingNode = interiorNode.leftChild == leafNode ? interiorNode.rightChild : interiorNode.leftChild;
            ReplaceChildNode(interiorNode.parent, interiorNode, siblingNode);
            BalanceInteriorNodeUntilRootNode(interiorNode.parent);
        }

        void InsertLeafNodeAt(Node insertNode, Node targetNode)
        {
            var interiorNode = new Node();
            ReplaceChildNode(targetNode.parent, targetNode, interiorNode);
            insertNode.parent = interiorNode;
            targetNode.parent = interiorNode;
            if (insertNode.key < targetNode.key)
            {
                interiorNode.leftChild = insertNode;
                interiorNode.rightChild = targetNode;
            }
            else
            {
                interiorNode.leftChild = targetNode;
                interiorNode.rightChild = insertNode;
            }
            UpdateInteriorNode(interiorNode);
            BalanceInteriorNodeUntilRootNode(interiorNode.parent);
        }

        void ReplaceChildNode(Node parentNode, Node oldChildNode, Node newChildNode)
        {
            if (parentNode == null)
            {
                _rootNode = newChildNode;
                newChildNode.parent = null;
            }
            else
            {
                if (parentNode.leftChild == oldChildNode)
                {
                    parentNode.leftChild = newChildNode;
                }
                else
                {
                    parentNode.rightChild = newChildNode;
                }
                newChildNode.parent = parentNode;
            }
        }

        void UpdateInteriorNodeUntilRootNode(Node node)
        {
            if (node == null)
            {
                return;
            }
            do
            {
                UpdateInteriorNode(node);
                node = node.parent;
            } while (node != null);
        }

        void BalanceInteriorNodeUntilRootNode(Node node)
        {
            if (node == null)
            {
                return;
            }
            do
            {
                node = BalanceInteriorNode(node);
                node = node.parent;
            } while (node != null);
        }

        void UpdateInteriorNode(Node interiorNode)
        {
            interiorNode.height = 1 + Math.Max(interiorNode.leftChild.height, interiorNode.rightChild.height);
            interiorNode.bounds = Bounds2D.Combine(interiorNode.leftChild.bounds, interiorNode.rightChild.bounds);
            interiorNode.key = Math.Clamp(GetKey(interiorNode.bounds.center), interiorNode.leftChild.maxKey + 1, interiorNode.rightChild.minKey);
            interiorNode.minKey = interiorNode.leftChild.minKey;
            interiorNode.maxKey = interiorNode.rightChild.maxKey;
        }

        Node BalanceInteriorNode(Node node)
        {
            int balanceFactor = node.leftChild.height - node.rightChild.height;
            if (balanceFactor > 1)
            {
                return LeftBalance(node);
            }
            else if (balanceFactor < -1)
            {
                return RightBalance(node);
            }
            else
            {
                UpdateInteriorNode(node);
                return node;
            }
        }

        Node LeftBalance(Node node)
        {
            if (node.leftChild.height > node.rightChild.height)
            {
                // LL Balance
                return RightRotate(node);
            }
            else
            {
                // LR Balance
                LeftRotate(node.leftChild);
                return RightRotate(node);
            }
        }

        Node RightBalance(Node node)
        {
            if (node.leftChild.height < node.rightChild.height)
            {
                // RR Balance
                return LeftRotate(node);
            }
            else
            {
                // RL Balance
                RightRotate(node.rightChild);
                return LeftRotate(node);
            }
        }

        Node RightRotate(Node rotateNode)
        {
            var midNode = rotateNode.leftChild;
            ReplaceChildNode(rotateNode.parent, rotateNode, midNode);
            rotateNode.parent = midNode;
            rotateNode.leftChild = midNode.rightChild;
            rotateNode.leftChild.parent = rotateNode;
            UpdateInteriorNode(rotateNode);
            midNode.rightChild = rotateNode;
            UpdateInteriorNode(midNode);
            return midNode;
        }

        Node LeftRotate(Node rotateNode)
        {
            var midNode = rotateNode.rightChild;
            ReplaceChildNode(rotateNode.parent, rotateNode, midNode);
            rotateNode.parent = midNode;
            rotateNode.rightChild = midNode.leftChild;
            rotateNode.rightChild.parent = rotateNode;
            UpdateInteriorNode(rotateNode);
            midNode.leftChild = rotateNode;
            UpdateInteriorNode(midNode);
            return midNode;
        }
    }
}
