using FixMathematics;

namespace OrthoPhysics
{
    public struct ContactManifold
    {
        public static ContactManifold empty => _empty;
        static readonly ContactManifold _empty = new ContactManifold();

        public FixVector2 normal;
        public FixVector2 point1;
        public FixVector2 point2;

        public override string ToString()
        {
            return $"{{normal: {normal}, point1: {point1}, point2: {point2}}}";
        }
    }
}
