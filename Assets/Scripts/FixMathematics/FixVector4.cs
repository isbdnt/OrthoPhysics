using System;

namespace FixMathematics
{
    [Serializable]
    public struct FixVector4
    {
        public static FixVector4 zero => _zero;
        static readonly FixVector4 _zero = new FixVector4();
        public static FixVector4 one => _one;
        static readonly FixVector4 _one = new FixVector4(Fix64.One, Fix64.One, Fix64.One, Fix64.One);

        public Fix64 x;
        public Fix64 y;
        public Fix64 z;
        public Fix64 w;

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
                    case 3:
                        return w;
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
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid vector index!");
                }
            }
        }
        public Fix64 sqrMagnitude => Dot(this, this);
        public Fix64 magnitude => Fix64.Sqrt(sqrMagnitude);
        public FixVector4 normalized => Normalize(this);
        public FixVector4 abs => new FixVector4(Fix64.Abs(x), Fix64.Abs(y), Fix64.Abs(z), Fix64.Abs(w));

        public FixVector4(Fix64 x, Fix64 y, Fix64 z, Fix64 w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static FixVector4 operator +(FixVector4 a, FixVector4 b)
        {
            return new FixVector4(
                a.x + b.x,
                a.y + b.y,
                a.z + b.z,
                a.w + b.w);
        }

        public static FixVector4 operator -(FixVector4 a, FixVector4 b)
        {
            return new FixVector4(
                a.x - b.x,
                a.y - b.y,
                a.z - b.z,
                a.w - b.w);
        }

        public static FixVector4 operator *(FixVector4 v, Fix64 d)
        {
            return new FixVector4(
                v.x * d,
                v.y * d,
                v.z * d,
                v.w * d);
        }

        public static FixVector4 operator /(FixVector4 v, Fix64 d)
        {
            return new FixVector4(
                v.x / d,
                v.y / d,
                v.z / d,
                v.w / d);
        }

        public static FixVector4 operator +(FixVector4 v)
        {
            return new FixVector4(v.x, v.y, v.z, v.w);
        }

        public static FixVector4 operator -(FixVector4 v)
        {
            return new FixVector4(-v.x, -v.y, -v.z, -v.w);
        }

        public static bool operator ==(FixVector4 a, FixVector4 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(FixVector4 a, FixVector4 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public static FixVector4 Normalize(FixVector4 v)
        {
            Fix64 sqrMagnitude = v.sqrMagnitude;
            if (sqrMagnitude < Fix64.Epsilon)
                return v;

            return v / Fix64.Sqrt(sqrMagnitude);
        }

        public static Fix64 Dot(FixVector4 a, FixVector4 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static FixVector4 Lerp(FixVector4 a, FixVector4 b, Fix64 t)
        {
            return new FixVector4(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t,
                a.w + (b.w - a.w) * t);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FixVector4)) return false;

            FixVector4 v = (FixVector4)obj;
            return x.Equals(v.x) && y.Equals(v.y) && z.Equals(v.z) && z.Equals(v.w);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }
    }
}
