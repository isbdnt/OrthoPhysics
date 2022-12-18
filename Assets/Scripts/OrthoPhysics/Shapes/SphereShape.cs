using FixMathematics;

namespace OrthoPhysics
{
    public class SphereShape : Shape
    {
        public Fix64 radius
        {
            get => _radius;
            set => _radius = value;
        }

        Fix64 _radius;

        public SphereShape(Fix64 radius)
        {
            _radius = radius;
        }
    }
}
