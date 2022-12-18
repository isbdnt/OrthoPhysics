using FixMathematics;
using System.Collections.Generic;
using UnityEngine;
using UnityExtensions;

namespace OrthoPhysics.UnityPlayer
{
    public class OrthoDynamicsWorld : MonoBehaviour
    {
        public static OrthoDynamicsWorld instance { get; private set; }

        public DynamicsWorld dynamicsWorld => _dynamicsWorld;

        [SerializeField, DisableInPlay] bool _isPartitioningEnabled = true;
        [SerializeField] int _velocityIterationCount = 8;
        [SerializeField] int _positionIterationCount = 4;
        [SerializeField] int _contactConstraintBufferCount = 4;
        [SerializeField] Fix64 _maxPenetrationDepth = (Fix64)0.005f;
        [SerializeField] Fix64 _gravity;
        [SerializeField] Fix64 _groundHeight = Fix64.Zero;
        [SerializeField] Fix64 _deltaTime = Fix64.One / (Fix64)60;
#if UNITY_EDITOR
        [SerializeField] int _displayBvhHeight = -1;
#endif
        DynamicsWorld _dynamicsWorld;
        List<OrthoBody> _bodyProxies = new List<OrthoBody>();

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);

            _dynamicsWorld = new DynamicsWorld(_isPartitioningEnabled);
            ApplyChangesFromUnity();
        }

        void FixedUpdate()
        {
            ApplyChangesFromUnity();
            foreach (var bodyProxy in _bodyProxies)
            {
                bodyProxy.ApplyChangesFromUnity();
            }
            _dynamicsWorld.Simulate(_deltaTime);
            foreach (var bodyProxy in _bodyProxies)
            {
                bodyProxy.ApplyTransformToUnity();
            }
        }

        void ApplyChangesFromUnity()
        {
            _dynamicsWorld.velocityIterationCount = _velocityIterationCount;
            _dynamicsWorld.positionIterationCount = _positionIterationCount;
            _dynamicsWorld.contactConstraintBufferCount = _contactConstraintBufferCount;
            _dynamicsWorld.maxPenetrationDepth = _maxPenetrationDepth;
            _dynamicsWorld.groundHeight = _groundHeight;
            _dynamicsWorld.gravity = _gravity;
        }

        public void AddBody(OrthoBody bodyProxy)
        {
            _bodyProxies.Add(bodyProxy);
            _dynamicsWorld.AddBody(bodyProxy.body);
        }

        public void RemoveBody(OrthoBody bodyProxy)
        {
            _dynamicsWorld.RemoveBody(bodyProxy.body);
            _bodyProxies.Remove(bodyProxy);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_dynamicsWorld == null || !_dynamicsWorld.isPartitioningEnabled)
            {
                return;
            }
            _dynamicsWorld.boundingVolumeTree.TraverseBounds((Bounds2D bounds, int height) =>
            {
                if (height == _displayBvhHeight || _displayBvhHeight < 0)
                {
                    var center = bounds.center;
                    var size = bounds.size;
                    GizmoUtility.DrawBounds(new UnityEngine.Bounds(FixUtility.ToVector3(new FixVector3(center.x, Fix64.Zero, center.y)), FixUtility.ToVector3(new FixVector3(size.x, Fix64.Zero, size.y))), Color.gray);
                }
            });
        }
#endif
    }
}
