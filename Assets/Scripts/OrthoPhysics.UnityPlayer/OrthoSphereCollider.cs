using FixMathematics;
using UnityEngine;

namespace OrthoPhysics.UnityPlayer
{
    public class OrthoSphereCollider : OrthoCollider
    {
        public override Collider collider => _sphereCollider;
        protected override bool isDirty => _sphereCollider.shape.radius != _radius;

        [SerializeField] Fix64 _radius;
        SphereCollider _sphereCollider;

        void Awake()
        {
            _sphereCollider = new SphereCollider(new SphereShape(_radius));
        }

        public override bool ApplyChangesFromUnity()
        {
            if (_sphereCollider.shape.radius != _radius)
            {
                _sphereCollider.shape.radius = _radius;
                return true;
            }
            return false;
        }
    }
}
