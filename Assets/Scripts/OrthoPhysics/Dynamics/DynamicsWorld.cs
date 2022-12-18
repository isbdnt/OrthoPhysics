using CSharpExtensions;
using FixMathematics;
using System;
using System.Collections.Generic;

namespace OrthoPhysics
{
    public class DynamicsWorld
    {
        public int velocityIterationCount
        {
            get => _contactConstraintSolver.velocityIterationCount;
            set => _contactConstraintSolver.velocityIterationCount = value;
        }
        public int positionIterationCount
        {
            get => _contactConstraintSolver.positionIterationCount;
            set => _contactConstraintSolver.positionIterationCount = value;
        }
        public int contactConstraintBufferCount
        {
            get => _contactConstraintBufferCount;
            set
            {
                if (_contactConstraintBufferCount != value)
                {
                    _contactConstraintBufferCount = value;
                    _contactConstraintBufferIndex %= value;
                    InitializeContactConstraintBuffers();
                }
            }
        }
        public Fix64 maxPenetrationDepth
        {
            get => _contactConstraintSolver.maxPenetrationDepth;
            set => _contactConstraintSolver.maxPenetrationDepth = value;
        }
        public Fix64 gravity
        {
            get => _gravity;
            set => _gravity = value;
        }
        public Fix64 groundHeight
        {
            get => _groundHeight;
            set => _groundHeight = value;
        }
        public bool isPartitioningEnabled => _isPartitioningEnabled;
        public BoundingVolumeHierarchy boundingVolumeTree => _boundingVolumeTree;

        Fix64 _gravity;
        Fix64 _groundHeight;
        List<Body> _bodies = new List<Body>();
        List<Body> _staticBodies = new List<Body>();
        List<Body> _rigidBodies = new List<Body>();
        List<Body> _triggerBodies = new List<Body>();
        List<Body> _collisionBodies = new List<Body>();
        List<ContactConstraint>[] _contactConstraintBuffers;
        int _contactConstraintBufferCount = 4;
        int _contactConstraintBufferIndex;
        Stack<ContactConstraint> _contactConstraintPool = new Stack<ContactConstraint>();
        ContactConstraintSolver _contactConstraintSolver = new ContactConstraintSolver();
        BoxCollider _overlapBox;
        SphereCollider _overlapSphere;
        bool _isPartitioningEnabled;
        BoundingVolumeHierarchy _boundingVolumeTree;

        public DynamicsWorld(bool isPartitioningEnabled)
        {
            _overlapBox = new BoxCollider(new BoxShape(FixVector3.zero));
            _overlapSphere = new SphereCollider(new SphereShape(Fix64.Zero));
            if (isPartitioningEnabled)
            {
                _isPartitioningEnabled = true;
                _boundingVolumeTree = new BoundingVolumeHierarchy();
            }
            InitializeContactConstraintBuffers();
        }

        void InitializeContactConstraintBuffers()
        {
            _contactConstraintBuffers = new List<ContactConstraint>[_contactConstraintBufferCount];
            for (int i = 0; i < _contactConstraintBufferCount; i++)
            {
                _contactConstraintBuffers[i] = new List<ContactConstraint>();
            }
            _contactConstraintSolver.trapStaticCollisionCount = _contactConstraintBufferCount + 1;
        }

        public void AddBody(Body body)
        {
            using (new ProfileScope("AddBody"))
            {
                _bodies.Add(body);
                if (_isPartitioningEnabled)
                {
                    switch (body.kind)
                    {
                        case BodyKind.Static:
                            _staticBodies.Add(body);
                            _boundingVolumeTree.AddBody(body);
                            break;
                        case BodyKind.Rigid:
                            _rigidBodies.Add(body);
                            break;
                        case BodyKind.Trigger:
                            _triggerBodies.Add(body);
                            _boundingVolumeTree.AddBody(body);
                            break;
                    }
                }
                else
                {
                    switch (body.kind)
                    {
                        case BodyKind.Static:
                            _staticBodies.Add(body);
                            break;
                        case BodyKind.Rigid:
                            _rigidBodies.Add(body);
                            break;
                        case BodyKind.Trigger:
                            _triggerBodies.Add(body);
                            break;
                    }
                }
            }
        }

        public void RemoveBody(Body body)
        {
            using (new ProfileScope("RemoveBody"))
            {
                if (_isPartitioningEnabled)
                {
                    switch (body.kind)
                    {
                        case BodyKind.Static:
                            _staticBodies.RemoveSwapBack(body);
                            _boundingVolumeTree.RemoveBody(body);
                            break;
                        case BodyKind.Rigid:
                            _rigidBodies.RemoveSwapBack(body);
                            break;
                        case BodyKind.Trigger:
                            _triggerBodies.RemoveSwapBack(body);
                            _boundingVolumeTree.RemoveBody(body);
                            break;
                    }
                }
                else
                {
                    switch (body.kind)
                    {
                        case BodyKind.Static:
                            _staticBodies.RemoveSwapBack(body);
                            break;
                        case BodyKind.Rigid:
                            _rigidBodies.RemoveSwapBack(body);
                            break;
                        case BodyKind.Trigger:
                            _triggerBodies.RemoveSwapBack(body);
                            break;
                    }
                }
                _bodies.RemoveSwapBack(body);
            }
        }

        public void OverlapBox(BoxCollider box, List<Body> results)
        {
            using (new ProfileScope("OverlapBox"))
            {
                CollectBroadPhaseCollision(box.bounds.xz);
                OverlapNarrowPhaseCollision(box, results);
            }
        }

        public void OverlapBox(FixVector3 position, FixComplex rotation, FixVector3 halfExtents, List<Body> results)
        {
            _overlapBox.transform.position = position;
            _overlapBox.transform.rotation = rotation;
            _overlapBox.shape.halfExtents = halfExtents;
            _overlapBox.UpdateBounds();
            OverlapBox(_overlapBox, results);
        }

        public void OverlapSphere(SphereCollider sphere, List<Body> results)
        {
            using (new ProfileScope("OverlapSphere"))
            {
                CollectBroadPhaseCollision(sphere.bounds.xz);
                OverlapNarrowPhaseCollision(sphere, results);
            }
        }

        public void OverlapSphere(FixVector3 position, Fix64 radius, List<Body> results)
        {
            _overlapSphere.transform.position = position;
            _overlapSphere.shape.radius = radius;
            _overlapSphere.UpdateBounds();
            OverlapSphere(_overlapSphere, results);
        }

        public void Simulate(Fix64 deltaTime)
        {
            using (new ProfileScope("Simulate"))
            {
                SolveMotion(deltaTime);
                SolveCollision(deltaTime);
                SolveMisc();
            }
        }

        void SolveMotion(Fix64 deltaTime)
        {
            using (new ProfileScope("SolveMotion"))
            {
                foreach (var body in _rigidBodies)
                {
                    if (body.isKinematic)
                    {
                        continue;
                    }
                    body.staticCollisionCount = 0;
                    body.IntegrateMotion(deltaTime, _groundHeight, _gravity);
                }
            }
        }

        void SolveCollision(Fix64 deltaTime)
        {
            using (new ProfileScope("SolveCollision"))
            {
                FreeAllContactConstraints();
                for (int i = 0, length = _rigidBodies.Count; i < length; ++i)
                {
                    var body = _rigidBodies[i];
                    if (body.isKinematic)
                    {
                        continue;
                    }
                    CollectBroadPhaseCollision(body.collider.bounds.xz, i + 1);
                    CollectNarrowPhaseCollision(body);
                }
                _contactConstraintSolver.SolveVelocity(_contactConstraintBuffers[_contactConstraintBufferIndex]);
                IntegratePosition(deltaTime);
                for (int i = 0; i < _contactConstraintBufferCount; ++i)
                {
                    foreach (var contactConstraint in _contactConstraintBuffers[(_contactConstraintBufferIndex - i + _contactConstraintBufferCount) % _contactConstraintBufferCount])
                    {
                        if (contactConstraint.body2.kind == BodyKind.Static)
                        {
                            ++contactConstraint.body1.staticCollisionCount;
                        }
                    }
                }
                for (int i = 0; i < _contactConstraintBufferCount; ++i)
                {
                    _contactConstraintSolver.SolvePosition(_contactConstraintBuffers[(_contactConstraintBufferIndex - i + _contactConstraintBufferCount) % _contactConstraintBufferCount]);
                }
                _contactConstraintBufferIndex = (_contactConstraintBufferIndex + 1) % _contactConstraintBufferCount;
            }
        }

        ContactConstraint CreateContactConstraint()
        {
            if (_contactConstraintPool.Count > 0)
            {
                return _contactConstraintPool.Pop();
            }
            return new ContactConstraint();
        }

        void FreeAllContactConstraints()
        {
            foreach (var contactConstraint in _contactConstraintBuffers[_contactConstraintBufferIndex])
            {
                _contactConstraintPool.Push(contactConstraint);
            }
            _contactConstraintBuffers[_contactConstraintBufferIndex].Clear();
        }

        void IntegratePosition(Fix64 deltaTime)
        {
            using (new ProfileScope("IntegratePosition"))
            {
                foreach (var body in _rigidBodies)
                {
                    if (body.isKinematic)
                    {
                        continue;
                    }
                    body.IntegratePosition2D(deltaTime);
                }
            }
        }

        void SolveMisc()
        {
            using (new ProfileScope("SolveMisc"))
            {
                foreach (var body in _rigidBodies)
                {
                    if (body.isKinematic)
                    {
                        continue;
                    }
                    body.UpdateBounds();
                }
                foreach (var body in _triggerBodies)
                {
                    if (body.isKinematic)
                    {
                        continue;
                    }
                    body.TriggerEvents();
                }
            }
        }

        void CollectBroadPhaseCollision(Bounds2D bounds, int bodyStart = 0)
        {
            using (new ProfileScope("CollectBroadPhaseCollision"))
            {
                _collisionBodies.Clear();
                if (_isPartitioningEnabled)
                {
                    _boundingVolumeTree.OverlapBounds(bounds, _collisionBodies);
                    for (int i = bodyStart, length = _rigidBodies.Count; i < length; i++)
                    {
                        var body = _rigidBodies[i];
                        if (!body.isKinematic && Bounds.Intersect2D(bounds, body.collider.bounds))
                        {
                            _collisionBodies.Add(body);
                        }
                    }
                }
                else
                {
                    for (int i = bodyStart, length = _rigidBodies.Count; i < length; i++)
                    {
                        var body = _rigidBodies[i];
                        if (!body.isKinematic && Bounds.Intersect2D(bounds, body.collider.bounds))
                        {
                            _collisionBodies.Add(body);
                        }
                    }
                }
            }
        }

        void CollectNarrowPhaseCollision(Body body)
        {
            using (new ProfileScope("CollectNarrowPhaseCollision"))
            {
                foreach (var otherBody in _collisionBodies)
                {
                    switch (otherBody.kind)
                    {
                        case BodyKind.Static:
                        case BodyKind.Rigid:
                            {
                                var manifold = ContactUtility.ContactAnyAny2D(body.collider, otherBody.collider);
                                if (manifold.normal != FixVector2.zero)
                                {
                                    var contactConstraint = CreateContactConstraint();
                                    contactConstraint.body1 = body;
                                    contactConstraint.body2 = otherBody;
                                    if (otherBody.kind == BodyKind.Static)
                                    {
                                        contactConstraint.normalMass = body.mass;
                                    }
                                    else
                                    {
                                        contactConstraint.normalMass = Fix64.One / (body.inverseMass + otherBody.inverseMass);
                                    }
                                    contactConstraint.normalImpulse = Fix64.Zero;
                                    contactConstraint.trapPenetrationDepth = Fix64.MaxValue;
                                    contactConstraint.manifold = manifold;
                                    _contactConstraintBuffers[_contactConstraintBufferIndex].Add(contactConstraint);
                                }
                            }
                            break;
                        case BodyKind.Trigger:
                            {
                                if (IntersectUtility.IntersectAnyAny(body.collider, otherBody.collider))
                                {
                                    otherBody.Enter(body);
                                }
                            }
                            break;
                    }
                }
            }
        }

        void OverlapNarrowPhaseCollision(Collider collider, List<Body> results)
        {
            using (new ProfileScope("OverlapNarrowPhaseCollision"))
            {
                foreach (var body in _collisionBodies)
                {
                    if (collider == body.collider)
                    {
                        continue;
                    }
                    if (IntersectUtility.IntersectAnyAny(collider, body.collider))
                    {
                        results.Add(body);
                    }
                }
            }
        }
    }
}
