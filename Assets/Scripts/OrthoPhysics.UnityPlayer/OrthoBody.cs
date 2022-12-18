using FixMathematics;
using System;
using UnityEngine;
using UnityExtensions;

namespace OrthoPhysics.UnityPlayer
{
    public class OrthoBody : MonoBehaviour
    {
        public Body body => _body;
        public event Action<OrthoBody> onTriggerEnter = delegate { };
        public event Action<OrthoBody> onTriggerStay = delegate { };
        public event Action<OrthoBody> onTriggerExit = delegate { };

        [SerializeField, DisableInPlay] BodyKind _kind;
        [SerializeField] Fix64 _mass;
        [SerializeField] Fix64 _drag;
        [SerializeField] Fix64 _airDrag;
        [SerializeField] bool _isKinematic;
        OrthoCollider _colliderProxy;
        Body _body;
        Vector3 _position;
        Quaternion _rotation = Quaternion.identity;

        private void Awake()
        {
            _colliderProxy = GetComponent<OrthoCollider>();
        }

        private void Start()
        {
            var collider = _colliderProxy.collider;
            _body = new Body(_kind, collider);
            _body.mass = _mass;
            _body.isKinematic = _isKinematic;
            _body.userData = this;
            if (_kind == BodyKind.Trigger)
            {
                _body.onTriggerEnter += OnBodyTriggerEnter;
                _body.onTriggerStay += OnBodyTriggerStay;
                _body.onTriggerExit += OnBodyTriggerExit;
            }
            ApplyChangesFromUnity();
            OrthoDynamicsWorld.instance.AddBody(this);
        }

        private void OnEnable()
        {
            if (_body != null)
            {
                OrthoDynamicsWorld.instance.AddBody(this);
            }
        }

        private void OnDisable()
        {
            if (_body != null)
            {
                OrthoDynamicsWorld.instance.RemoveBody(this);
            }
        }

        private void OnDestroy()
        {
            if (OrthoDynamicsWorld.instance != null)
            {
                OrthoDynamicsWorld.instance.RemoveBody(this);
                _body.onTriggerEnter -= OnBodyTriggerEnter;
                _body.onTriggerStay -= OnBodyTriggerStay;
                _body.onTriggerExit -= OnBodyTriggerExit;
            }
        }

        void OnBodyTriggerEnter(Body body)
        {
            onTriggerEnter(body.userData as OrthoBody);
        }

        void OnBodyTriggerStay(Body body)
        {
            onTriggerStay(body.userData as OrthoBody);
        }

        void OnBodyTriggerExit(Body body)
        {
            onTriggerExit(body.userData as OrthoBody);
        }

        public void ApplyChangesFromUnity()
        {
            _body.mass = _mass;
            _body.isKinematic = _isKinematic;
            _body.drag = _drag;
            _body.airDrag = _airDrag;
            if ((_position - transform.position).sqrMagnitude > 0.000001f || (1f - Quaternion.Dot(_rotation, transform.rotation)) > 0.000001f || _colliderProxy.ApplyChangesFromUnity())
            {
                _position = transform.position;
                _rotation = transform.rotation;
                _body.transform.position = FixUtility.ToFixVector3(_position);
                var direction = FixUtility.ToFixQuaternion(_rotation) * FixVector3.right;
                direction.y = Fix64.Zero;
                var directionOnPlane = direction.normalized;
                _body.transform.rotation = new FixComplex(directionOnPlane.x, directionOnPlane.z);
                _body.UpdateBounds();
            }
        }

        public void ApplyTransformToUnity()
        {
            transform.position = FixUtility.ToVector3(_body.transform.position);
            var direction = FixUtility.ToVector2(_body.transform.rotation.direction);
            transform.rotation = Quaternion.LookRotation(new Vector3(-direction.y, 0f, direction.x), Vector3.up);
        }

        private void OnDrawGizmos()
        {
            if (_body == null)
            {
                return;
            }
            Color color = Color.white;
            if (_body.isKinematic)
            {
                color = Color.blue;
            }
            else
            {
                switch (_body.kind)
                {
                    case BodyKind.Static:
                        color = Color.white;
                        break;
                    case BodyKind.Rigid:
                        color = Color.yellow;
                        break;
                    case BodyKind.Trigger:
                        color = Color.green;
                        break;
                }
            }
            GizmoUtility.DrawBounds(new UnityEngine.Bounds(transform.position, FixUtility.ToVector3(_body.collider.bounds.max - _body.collider.bounds.min)), color);
        }
    }
}
