using FixMathematics;

namespace OrthoPhysics
{
    public struct Bounds
    {
        public static Bounds invalid => _invalid;
        static Bounds _invalid = new Bounds(FixVector3.one, -FixVector3.one);

        public FixVector3 min;
        public FixVector3 max;
        public FixVector3 center => (min + max) / Fix64.Two;
        public FixVector3 size => max - min;
        public Bounds2D xz => new Bounds2D(min.xz, max.xz);

        public Bounds(FixVector3 min, FixVector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public static bool Intersect(Bounds a, Bounds b)
        {
            return
                a.min.x < b.max.x && b.min.x < a.max.x &&
                a.min.y < b.max.y && b.min.y < a.max.y &&
                a.min.z < b.max.z && b.min.z < a.max.z;
        }

        public static bool Intersect2D(Bounds a, Bounds b)
        {
            return
                a.min.x < b.max.x && b.min.x < a.max.x &&
                a.min.z < b.max.z && b.min.z < a.max.z;
        }

        public static bool Intersect2D(Bounds a, Bounds2D b)
        {
            return
                a.min.x < b.max.x && b.min.x < a.max.x &&
                a.min.z < b.max.y && b.min.y < a.max.z;
        }

        public static bool Intersect2D(Bounds2D a, Bounds b)
        {
            return
                a.min.x < b.max.x && b.min.x < a.max.x &&
                a.min.y < b.max.z && b.min.z < a.max.y;
        }

        public static Bounds Combine(Bounds a, Bounds b)
        {
            return new Bounds(new FixVector3(Fix64.Min(a.min.x, b.min.x), Fix64.Min(a.min.y, b.min.y), Fix64.Min(a.min.z, b.min.z)), new FixVector3(Fix64.Max(a.max.x, b.max.x), Fix64.Max(a.max.y, b.max.y), Fix64.Max(a.max.z, b.max.z)));
        }

        public static Bounds SafeCombine(Bounds a, Bounds b)
        {
            if (a == _invalid)
            {
                return b;
            }
            if (b == _invalid)
            {
                return a;
            }
            return new Bounds(new FixVector3(Fix64.Min(a.min.x, b.min.x), Fix64.Min(a.min.y, b.min.y), Fix64.Min(a.min.z, b.min.z)), new FixVector3(Fix64.Max(a.max.x, b.max.x), Fix64.Max(a.max.y, b.max.y), Fix64.Max(a.max.z, b.max.z)));
        }


        public static bool operator ==(Bounds a, Bounds b)
        {
            return a.min == b.min && a.max == b.max;
        }

        public static bool operator !=(Bounds a, Bounds b)
        {
            return a.min != b.min || a.max != b.max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Bounds)) return false;

            Bounds v = (Bounds)obj;
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
