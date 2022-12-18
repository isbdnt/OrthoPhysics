using System;

namespace FixMathematics
{
    [Serializable]
    public struct FixVector3
    {
        public static FixVector3 zero => _zero;
        static readonly FixVector3 _zero = new FixVector3();
        public static FixVector3 one => _one;
        static readonly FixVector3 _one = new FixVector3(Fix64.One, Fix64.One, Fix64.One);
        public static FixVector3 left => _left;
        static readonly FixVector3 _left = new FixVector3(-Fix64.One, Fix64.Zero, Fix64.Zero);
        public static FixVector3 right => _right;
        static readonly FixVector3 _right = new FixVector3(Fix64.One, Fix64.Zero, Fix64.Zero);
        public static FixVector3 back => _back;
        static readonly FixVector3 _back = new FixVector3(Fix64.Zero, Fix64.Zero, -Fix64.One);
        public static FixVector3 forward => _forward;
        static readonly FixVector3 _forward = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.One);
        public static FixVector3 down => _down;
        static readonly FixVector3 _down = new FixVector3(Fix64.Zero, -Fix64.One, Fix64.Zero);
        public static FixVector3 up => _up;
        static readonly FixVector3 _up = new FixVector3(Fix64.Zero, Fix64.One, Fix64.Zero);

        public Fix64 x;
        public Fix64 y;
        public Fix64 z;

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
                    case 2:
                        return z;
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
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid vector index!");
                }
            }
        }
        public Fix64 sqrMagnitude => Dot(this, this);
        public Fix64 magnitude => Fix64.Sqrt(sqrMagnitude);
        public FixVector3 normalized => Normalize(this);
        public FixVector3 abs => new FixVector3(Fix64.Abs(x), Fix64.Abs(y), Fix64.Abs(z));
        public FixVector2 xz => new FixVector2(x, z);

        public FixVector3(Fix64 x, Fix64 y, Fix64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static FixVector3 operator +(FixVector3 a, FixVector3 b)
        {
            return new FixVector3(
                a.x + b.x,
                a.y + b.y,
                a.z + b.z);
        }

        public static FixVector3 operator -(FixVector3 a, FixVector3 b)
        {
            return new FixVector3(
                a.x - b.x,
                a.y - b.y,
                a.z - b.z);
        }

        public static FixVector3 operator *(FixVector3 v, Fix64 d)
        {
            return new FixVector3(
                v.x * d,
                v.y * d,
                v.z * d);
        }

        public static FixVector3 operator /(FixVector3 v, Fix64 d)
        {
            return new FixVector3(
                v.x / d,
                v.y / d,
                v.z / d);
        }

        public static FixVector3 operator +(FixVector3 v)
        {
            return new FixVector3(v.x, v.y, v.z);
        }

        public static FixVector3 operator -(FixVector3 v)
        {
            return new FixVector3(-v.x, -v.y, -v.z);
        }

        public static bool operator ==(FixVector3 a, FixVector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(FixVector3 a, FixVector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static FixVector3 Normalize(FixVector3 v)
        {
            Fix64 sqrMagnitude = v.sqrMagnitude;
            if (sqrMagnitude < Fix64.Epsilon)
                return v;

            return v / Fix64.Sqrt(sqrMagnitude);
        }

        public static Fix64 Dot(FixVector3 a, FixVector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static FixVector3 Cross(FixVector3 a, FixVector3 b)
        {
            return new FixVector3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x);
        }

        public static FixVector3 Lerp(FixVector3 a, FixVector3 b, Fix64 t)
        {
            return new FixVector3(
                a.x + (b.x - a.x) * t, 
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FixVector3)) return false;

            FixVector3 v = (FixVector3)obj;
            return x.Equals(v.x) && y.Equals(v.y) && z.Equals(v.z);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }
    }
}
