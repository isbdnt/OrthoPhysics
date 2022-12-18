using System;

namespace FixMathematics
{
    [Serializable]
    // x + yi
    public struct FixComplex
    {
        public static FixComplex identify => _identify;
        static readonly FixComplex _identify = new FixComplex(Fix64.One, Fix64.Zero);

        public Fix64 x;
        public Fix64 y;

        public Fix64 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid vector index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid vector index!");
                }
            }
        }
        public Fix64 sqrMagnitude => Dot(this, this);
        public Fix64 magnitude => Fix64.Sqrt(sqrMagnitude);
        public FixComplex normalzed => Normalize(this);
        public FixComplex abs => new FixComplex(Fix64.Abs(x), Fix64.Abs(y));
        public Fix64 eulerAngle => Fix64.Atan2(y, x);
        public FixVector2 direction => new FixVector2(x, y);
        public FixComplex inverse => new FixComplex(x, -y);

        public FixComplex(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        public static FixComplex operator +(FixComplex a, FixComplex b)
        {
            return new FixComplex(
                a.x + b.x,
                a.y + b.y);
        }

        public static FixComplex operator -(FixComplex a, FixComplex b)
        {
            return new FixComplex(
                a.x - b.x,
                a.y - b.y);
        }

        public static FixComplex operator *(FixComplex v, Fix64 d)
        {
            return new FixComplex(
                v.x * d,
                v.y * d);
        }

        public static FixComplex operator /(FixComplex v, Fix64 d)
        {
            return new FixComplex(
                v.x / d,
                v.y / d);
        }

        public static FixComplex operator +(FixComplex v)
        {
            return new FixComplex(v.x, v.y);
        }

        public static FixComplex operator -(FixComplex v)
        {
            return new FixComplex(-v.x, -v.y);
        }

        public static bool operator ==(FixComplex a, FixComplex b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(FixComplex a, FixComplex b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static FixComplex operator *(FixComplex a, FixComplex b)
        {
            return new FixComplex(
                a.x * b.x - a.y * b.y,
                a.x * b.y + a.y * b.x);
        }

        public static FixVector2 operator *(FixComplex a, FixVector2 b)
        {
            return new FixVector2(
                a.x * b.x - a.y * b.y,
                a.x * b.y + a.y * b.x);
        }

        public static FixComplex Normalize(FixComplex v)
        {
            Fix64 sqrMagnitude = v.sqrMagnitude;
            if (sqrMagnitude < Fix64.Epsilon)
                return v;

            return v / Fix64.Sqrt(sqrMagnitude);
        }

        public static Fix64 Dot(FixComplex a, FixComplex b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static FixComplex Euler(Fix64 angleInRadian)
        {
            return new FixComplex(Fix64.Cos(angleInRadian), Fix64.Sin(angleInRadian));
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FixComplex)) return false;

            FixComplex v = (FixComplex)obj;
            return x.Equals(v.x) && y.Equals(v.y);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
    }
}
