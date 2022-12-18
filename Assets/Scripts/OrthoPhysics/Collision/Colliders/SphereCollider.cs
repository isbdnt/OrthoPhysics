using FixMathematics;

namespace OrthoPhysics
{
    public class SphereCollider : Collider
    {
        public override Bounds bounds => _bounds;
        public SphereShape shape => _shape;

        SphereShape _shape;
        Bounds _bounds;

        public SphereCollider(SphereShape shape)
        {
            _shape = shape;
        }

        public override Fix64 GetHalfHeight()
        {
            return _shape.radius;
        }

        public override void UpdateBounds()
        {
            FixVector3 halfExtents = FixVector3.one * _shape.radius;
            _bounds = new Bounds(_transform.position - halfExtents, _transform.position + halfExtents);
        }
    }
}
