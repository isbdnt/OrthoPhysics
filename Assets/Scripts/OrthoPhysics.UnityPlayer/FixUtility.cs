using FixMathematics;
using UnityEngine;

namespace OrthoPhysics.UnityPlayer
{
    public static class FixUtility
    {
        public static FixVector2 ToFixVector2(Vector2 v)
        {
            return new FixVector2((Fix64)v.x, (Fix64)v.y);
        }

        public static FixVector3 ToFixVector3(Vector3 v)
        {
            return new FixVector3((Fix64)v.x, (Fix64)v.y, (Fix64)v.z);
        }

        public static FixVector4 ToFixVector4(Vector4 v)
        {
            return new FixVector4((Fix64)v.x, (Fix64)v.y, (Fix64)v.z, (Fix64)v.w);
        }

        public static FixQuaternion ToFixQuaternion(Quaternion v)
        {
            return new FixQuaternion((Fix64)v.x, (Fix64)v.y, (Fix64)v.z, (Fix64)v.w);
        }

        public static Vector2 ToVector2(FixVector2 v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static Vector3 ToVector3(FixVector3 v)
        {
            return new Vector3((float)v.x, (float)v.y, (float)v.z);
        }

        public static Vector4 ToFixVector4(FixVector4 v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }

        public static Quaternion ToQuaternion(FixQuaternion v)
        {
            return new Quaternion((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }
    }
}
