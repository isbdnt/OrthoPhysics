using FixMathematics;

namespace OrthoPhysics
{
    public struct Bounds2D
    {
        public static Bounds2D invalid => _invalid;
        static Bounds2D _invalid = new Bounds2D(FixVector2.one, -FixVector2.one);

        public FixVector2 min;
        public FixVector2 max;
        public FixVector2 center => (min + max) / Fix64.Two;
        public FixVector2 size => max - min;

        public Bounds2D(FixVector2 min, FixVector2 max)
        {
            this.min = min;
            this.max = max;
        }

        public static bool Intersect(Bounds2D a, Bounds2D b)
        {
            return
                a.min.x < b.max.x && b.min.x < a.max.x &&
                a.min.y < b.max.y && b.min.y < a.max.y;
        }

        public static Bounds2D Combine(Bounds2D a, Bounds2D b)
        {
            return new Bounds2D(new FixVector2(Fix64.Min(a.min.x, b.min.x), Fix64.Min(a.min.y, b.min.y)), new FixVector2(Fix64.Max(a.max.x, b.max.x), Fix64.Max(a.max.y, b.max.y)));
        }

        public static Bounds2D SafeCombine(Bounds2D a, Bounds2D b)
        {
            if (a == _invalid)
            {
                return b;
            }
            if (b == _invalid)
            {
                return a;
            }
            return new Bounds2D(new FixVector2(Fix64.Min(a.min.x, b.min.x), Fix64.Min(a.min.y, b.min.y)), new FixVector2(Fix64.Max(a.max.x, b.max.x), Fix64.Max(a.max.y, b.max.y)));
        }

        public static bool operator ==(Bounds2D a, Bounds2D b)
        {
            return a.min == b.min && a.max == b.max;
        }

        public static bool operator !=(Bounds2D a, Bounds2D b)
        {
            return a.min != b.min || a.max != b.max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Bounds2D)) return false;

            Bounds2D v = (Bounds2D)obj;
            return min.Equals(v.min) && max.Equals(v.max);
        }

        public override int GetHashCode()
        {
            return min.GetHashCode() ^ (max.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return $"{{min: {min}, max: {max}}}";
        }
    }
}
