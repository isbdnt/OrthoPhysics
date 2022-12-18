using FixMathematics;
using System;
using System.Collections.Generic;

namespace OrthoPhysics
{
    public class Body
    {
        public BodyKind kind => _kind;
        public Collider collider => _collider;
        public Bounds bounds => _collider.bounds;
        public Transform transform => _collider.transform;
        public Fix64 mass
        {
            get => _mass;
            set
            {
                if (_kind != BodyKind.Rigid)
                {
                    return;
                }
                if (value > Fix64.Epsilon)
                {
                    _mass = value;
                    _inverseMass = Fix64.One / value;
                }
            }
        }
        public Fix64 inverseMass => _inverseMass;
        public Fix64 drag
        {
            get => _drag;
            set => _drag = value;
        }
        public Fix64 airDrag
        {
            get => _airDrag;
            set => _airDrag = value;
        }
        public Fix64 bounciness
        {
            get => _bounciness;
            set => _bounciness = value;
        }
        public FixVector3 velocity
        {
            get => _velocity;
            set => _velocity = value;
        }
        public bool isKinematic { get; set; }
        public object userData { get; set; }
        public BoundingVolumeHierarchy.Node bvhNode { get; set; }
        public FixVector2 collisionFreePosition => _collisionFreePosition;
        public int staticCollisionCount { get; set; }
        public event Action<Body> onTriggerEnter = delegate { };
        public event Action<Body> onTriggerStay = delegate { };
        public event Action<Body> onTriggerExit = delegate { };

        BodyKind _kind;
        Collider _collider;
        Fix64 _mass;
        Fix64 _inverseMass;
        Fix64 _drag;
        Fix64 _airDrag;
        Fix64 _bounciness;
        FixVector3 _force;
        FixVector3 _velocity;
        FixVector2 _collisionFreePosition;
        HashSet<Body> _enteredBodySet;
        HashSet<Body> _stayedBodySet;

        public Body(BodyKind kind, Collider collider)
        {
            _kind = kind;
            _collider = collider;
            switch (kind)
            {
                case BodyKind.Trigger:
                    _enteredBodySet = new HashSet<Body>();
                    _stayedBodySet = new HashSet<Body>();
                    break;
            }
        }

        public void AddForce(FixVector3 force)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            _force += force;
        }

        public void AddForce2D(FixVector2 force)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            _force += new FixVector3(force.x, Fix64.Zero, force.y);
        }

        public void AddImpulse(FixVector3 impulse)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            _velocity += impulse * _inverseMass;
        }

        public void AddImpulse2D(FixVector2 impulse)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            _velocity += new FixVector3(impulse.x, Fix64.Zero, impulse.y) * _inverseMass;
        }

        public void ClearForce()
        {
            _force = FixVector3.zero;
        }

        public void UpdateBounds()
        {
            collider.UpdateBounds();
            if (bvhNode != null)
            {
                bvhNode.hierarchy.AddBody(this);
            }
        }

        public void IntegrateMotion(Fix64 deltaTime, Fix64 groundHeight, Fix64 gravity)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            var position = _collider.transform.position;
            _collisionFreePosition = position.xz;
            var velocity = _velocity;
            Fix64 halfHeight = _collider.GetHalfHeight();
            if (_inverseMass != Fix64.Zero)
            {
                Fix64 velocitySqrMagnitude2D = velocity.xz.sqrMagnitude;
                if (position.y - halfHeight > groundHeight)
                {
                    if (velocitySqrMagnitude2D > Fix64.Epsilon)
                    {
                        velocity *= Fix64.Clamp(Fix64.One - _airDrag, Fix64.Zero, Fix64.One);
                    }
                    else if (velocitySqrMagnitude2D != Fix64.Zero)
                    {
                        velocity = FixVector3.zero;
                        velocity.y = _velocity.y;
                    }
                    velocity.y += gravity * deltaTime;
                }
                else
                {
                    if (velocitySqrMagnitude2D > Fix64.Epsilon)
                    {
                        velocity *= Fix64.Clamp(Fix64.One - _drag, Fix64.Zero, Fix64.One);
                    }
                    else if (velocitySqrMagnitude2D != Fix64.Zero)
                    {
                        velocity = FixVector3.zero;
                    }
                }
                if (_force != FixVector3.zero)
                {
                    velocity += _force * _inverseMass * deltaTime;
                    _force = FixVector3.zero;
                }
                position += velocity * deltaTime;
            }
            Fix64 minHeight = groundHeight + halfHeight;
            if (position.y < minHeight)
            {
                velocity.y = Fix64.Zero;
                position.y = minHeight;
            }
            _velocity = velocity;
            _collider.transform.position = position;
            UpdateBounds();
        }

        public void IntegratePosition2D(Fix64 deltaTime)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            _collider.transform.position = new FixVector3(_collisionFreePosition.x, _collider.transform.position.y, _collisionFreePosition.y) + _velocity * deltaTime;
        }

        public void AddTranslation2D(FixVector2 translation)
        {
            if (_kind != BodyKind.Rigid)
            {
                return;
            }
            _collider.transform.position += new FixVector3(translation.x, Fix64.Zero, translation.y);
        }

        public void Enter(Body other)
        {
            if (_kind != BodyKind.Trigger)
            {
                return;
            }
            _enteredBodySet.Add(other);
        }

        public void TriggerEvents()
        {
            if (_kind != BodyKind.Trigger || (_enteredBodySet.Count == 0 && _stayedBodySet.Count == 0))
            {
                return;
            }

            foreach (var body in _enteredBodySet)
            {
                if (!_stayedBodySet.Contains(body))
                {
                    _stayedBodySet.Add(body);
                    onTriggerEnter(body);
                }
            }
            foreach (var body in _stayedBodySet)
            {
                if (!_enteredBodySet.Contains(body))
                {
                    onTriggerExit(body);
                }
                else
                {
                    onTriggerStay(body);
                }
            }

            // Swap
            var bodySet = _stayedBodySet;
            _stayedBodySet = _enteredBodySet;
            bodySet.Clear();
            _enteredBodySet = bodySet;
        }
    }
}
