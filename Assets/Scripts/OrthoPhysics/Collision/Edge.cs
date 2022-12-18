using FixMathematics;

namespace OrthoPhysics
{
    public struct Edge
    {
        public FixVector2 point1;
        public FixVector2 point2;
        public FixVector2 direction;

        public Edge(FixVector2 point1, FixVector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.direction = (point2 - point1).normalized;
        }

        public override string ToString()
        {
            return $"{{point1: {point1}, point2: {point2}}}";
        }
    }
}
