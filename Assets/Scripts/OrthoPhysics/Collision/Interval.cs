using FixMathematics;
using System.Collections;
using System.Collections.Generic;

namespace OrthoPhysics
{
    public struct Interval
    {
        public Fix64 min;
        public Fix64 max;

        public Interval(Fix64 min, Fix64 max)
        {
            this.min = min;
            this.max = max;
        }

        public static Fix64 GetOverlap(Interval a, Interval b)
        {
            return Fix64.Min(a.max - b.min, b.max - a.min);
        }

        public override string ToString()
        {
            return $"{{min: {min}, max: {max}}}";
        }
    }
}
