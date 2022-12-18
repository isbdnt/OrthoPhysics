using System;

namespace FixMathematics
{
    [Serializable]
    public struct FixVector2
    {
        public static FixVector2 zero => _zero;
        static readonly FixVector2 _zero = new FixVector2();
        public static FixVector2 one => _one;
        static readonly FixVector2 _one = new FixVector2(Fix64.One, Fix64.One);
        public static FixVector2 left => _left;
        static readonly FixVector2 _left = new FixVector2(-Fix64.One, Fix64.Zero);
        public static FixVector2 right => _right;
        static readonly FixVector2 _right = new FixVector2(Fix64.One, Fix64.Zero);
        public static FixVector2 down => _down;
        static readonly FixVector2 _down = new FixVector2(Fix64.Zero, -Fix64.One);
        public static FixVector2 up => _up;
        static readonly FixVector2 _up = new FixVector2(Fix64.Zero, Fix64.One);

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
        public FixVector2 normalized => Normalize(this);
        public FixVector2 abs => new FixVector2(Fix64.Abs(x), Fix64.Abs(y));

        public FixVector2(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        public static FixVector2 operator +(FixVector2 a, FixVector2 b)
        {
            return new FixVector2(
                a.x + b.x,
                a.y + b.y);
        }

        public static FixVector2 operator -(FixVector2 a, FixVector2 b)
        {
            return new FixVector2(
                a.x - b.x,
                a.y - b.y);
        }

        public static FixVector2 operator *(FixVector2 v, Fix64 d)
        {
            return new FixVector2(
                v.x * d,
                v.y * d);
        }

        public static FixVector2 operator /(FixVector2 v, Fix64 d)
        {
            return new FixVector2(
                v.x / d,
                v.y / d);
        }

        public static FixVector2 operator +(FixVector2 v)
        {
            return new FixVector2(v.x, v.y);
        }

        public static FixVector2 operator -(FixVector2 v)
        {
            return new FixVector2(-v.x, -v.y);
        }

        public static bool operator ==(FixVector2 a, FixVector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(FixVector2 a, FixVector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static FixVector2 Normalize(FixVector2 v)
        {
            Fix64 sqrMagnitude = v.sqrMagnitude;
            if (sqrMagnitude < Fix64.Epsilon)
                return v;

            return v / Fix64.Sqrt(sqrMagnitude);
        }

        public static Fix64 Dot(FixVector2 a, FixVector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        // |axb|=|a||b|sinθ
        public static Fix64 Cross(FixVector2 a, FixVector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static FixVector2 Perpendicular(FixVector2 a)
        {
            return new FixVector2(-a.y, a.x);
        }

        public static FixVector2 Lerp(FixVector2 a, FixVector2 b, Fix64 t)
        {
            return new FixVector2(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FixVector2)) return false;

            FixVector2 v = (FixVector2)obj;
            return x.Equals(v.x) && y.Equals(v.y);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
    }
}
