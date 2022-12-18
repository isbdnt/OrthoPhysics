using FixMathematics;
using UnityEngine;

namespace OrthoPhysics.UnityPlayer
{
    public class OrthoBoxCollider : OrthoCollider
    {
        public override Collider collider => _boxCollider;
        protected override bool isDirty => _boxCollider.shape.halfExtents != _halfExtents;

        [SerializeField] FixVector3 _halfExtents;
        BoxCollider _boxCollider;

        void Awake()
        {
            _boxCollider = new BoxCollider(new BoxShape(_halfExtents));
        }

        public override bool ApplyChangesFromUnity()
        {
            if (_boxCollider.shape.halfExtents != _halfExtents)
            {
                _boxCollider.shape.halfExtents = _halfExtents;
                return true;
            }
            return false;
        }
    }
}
