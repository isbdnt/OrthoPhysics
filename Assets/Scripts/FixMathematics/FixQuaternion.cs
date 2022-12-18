using System;

namespace FixMathematics
{
    [Serializable]
    // xi + yj + zk + w
    public struct FixQuaternion
    {
        public static FixQuaternion identity => _identity;
        static readonly FixQuaternion _identity = new FixQuaternion(Fix64.Zero, Fix64.Zero, Fix64.Zero, Fix64.One);

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
        public Fix64 magnitude => Fix64.Sqrt(Dot(this, this));
        public FixQuaternion normalized => Normalize(this);

        public FixQuaternion(Fix64 x, Fix64 y, Fix64 z, Fix64 w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static FixQuaternion operator +(FixQuaternion a, FixQuaternion b)
        {
            return new FixQuaternion(
                a.x + b.x,
                a.y + b.y,
                a.z + b.z,
                a.w + b.w);
        }

        public static FixQuaternion operator -(FixQuaternion a, FixQuaternion b)
        {
            return new FixQuaternion(
                a.x - b.x,
                a.y - b.y,
                a.z - b.z,
                a.w - b.w);
        }

        public static FixQuaternion operator *(FixQuaternion v, Fix64 d)
        {
            return new FixQuaternion(
                v.x * d,
                v.y * d,
                v.z * d,
                v.w * d);
        }

        public static FixQuaternion operator /(FixQuaternion v, Fix64 d)
        {
            return new FixQuaternion(
                v.x / d,
                v.y / d,
                v.z / d,
                v.w / d);
        }

        public static bool operator ==(FixQuaternion a, FixQuaternion b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(FixQuaternion a, FixQuaternion b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public static FixQuaternion operator *(FixQuaternion a, FixQuaternion b)
        {
            return new FixQuaternion(
                a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z,
                a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x,
                a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
        }

        public static FixVector3 operator *(FixQuaternion a, FixVector3 b)
        {
            Fix64 x = a.x * Fix64.Two;
            Fix64 y = a.y * Fix64.Two;
            Fix64 z = a.z * Fix64.Two;
            Fix64 xx = a.x * x;
            Fix64 yy = a.y * y;
            Fix64 zz = a.z * z;
            Fix64 xy = a.x * y;
            Fix64 xz = a.x * z;
            Fix64 yz = a.y * z;
            Fix64 wx = a.w * x;
            Fix64 wy = a.w * y;
            Fix64 wz = a.w * z;

            FixVector3 r;
            r.x = (Fix64.One - (yy + zz)) * b.x + (xy - wz) * b.y + (xz + wy) * b.z;
            r.y = (xy + wz) * b.x + (Fix64.One - (xx + zz)) * b.y + (yz - wx) * b.z;
            r.z = (xz - wy) * b.x + (yz + wx) * b.y + (Fix64.One - (xx + yy)) * b.z;
            return r;
        }

        public static FixQuaternion Normalize(FixQuaternion v)
        {
            Fix64 sqrMagnitude = v.sqrMagnitude;
            if (sqrMagnitude < Fix64.Epsilon)
                return v;

            return v / Fix64.Sqrt(sqrMagnitude);
        }

        public static Fix64 Dot(FixQuaternion a, FixQuaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FixQuaternion)) return false;

            FixQuaternion v = (FixQuaternion)obj;
            return x.Equals(v.x) && y.Equals(v.y) && z.Equals(v.z) && w.Equals(v.w);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }
    }
}
