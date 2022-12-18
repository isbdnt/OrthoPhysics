using FixMathematics;

namespace OrthoPhysics
{
    public class Transform
    {
        public FixVector3 position
        {
            get => _position;
            set => _position = value;
        }
        public FixComplex rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        FixVector3 _position;
        FixComplex _rotation = FixComplex.identify;

        public Transform()
        {
        }

        public FixVector2 ToWorldSpace2D(FixVector2 point)
        {
            return _position.xz + _rotation * point;
        }

        public FixVector2 ToLocalSpace2D(FixVector2 point)
        {
            return _rotation.inverse * (point - _position.xz);
        }
    }
}
