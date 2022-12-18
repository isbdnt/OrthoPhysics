using FixMathematics;

namespace OrthoPhysics
{
    public static class IntersectUtility
    {
        public static bool IntersectAnyAny(Collider collider1, Collider collider2)
        {
            switch (collider1)
            {
                case BoxCollider box1:
                    switch (collider2)
                    {
                        case BoxCollider box2:
                            return IntersectBoxBox(box1, box2);
                        case SphereCollider sphere2:
                            return IntersectBoxSphere(box1, sphere2);
                    }
                    break;
                case SphereCollider sphere1:
                    switch (collider2)
                    {
                        case SphereCollider sphere2:
                            return IntersectSphereSphere(sphere1, sphere2);
                        case BoxCollider box2:
                            return IntersectSphereBox(sphere1, box2);
                    }
                    break;
            }
            return false;
        }

        public static bool IntersectBoxAny(BoxCollider box1, Collider collider2)
        {
            switch (collider2)
            {
                case BoxCollider box2:
                    return IntersectBoxBox(box1, box2);
                case SphereCollider sphere2:
                    return IntersectBoxSphere(box1, sphere2);
            }
            return false;
        }

        public static bool IntersectSphereAny(SphereCollider sphere1, Collider collider2)
        {
            switch (collider2)
            {
                case SphereCollider sphere2:
                    return IntersectSphereSphere(sphere1, sphere2);
                case BoxCollider box2:
                    return IntersectSphereBox(sphere1, box2);
            }
            return false;
        }

        public static bool IntersectBoxBox(BoxCollider box1, BoxCollider box2)
        {
            var bounds1 = box1.bounds;
            var bounds2 = box2.bounds;
            if (bounds1.min.y > bounds2.max.y || bounds2.min.y > bounds1.max.y)
            {
                return false;
            }

            if (box1.isAxisAligned && box2.isAxisAligned)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < 2; ++i)
                {
                    Fix64 overlap = Interval.GetOverlap(box1.intervals[i], box2.GetInterval(box1.normals[i]));
                    if (overlap < Fix64.Zero)
                    {
                        return false;
                    }
                }
                for (int i = 0; i < 2; ++i)
                {
                    Fix64 overlap = Interval.GetOverlap(box2.intervals[i], box1.GetInterval(box2.normals[i]));
                    if (overlap < Fix64.Zero)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static bool IntersectBoxSphere(BoxCollider box1, SphereCollider sphere2)
        {
            var bounds1 = box1.bounds;
            var bounds2 = sphere2.bounds;
            if (bounds1.min.y > bounds2.max.y || bounds2.min.y > bounds1.max.y)
            {
                return false;
            }

            FixVector3 position2 = sphere2.transform.position;
            FixVector2 closestPoint;
            if (box1.isAxisAligned)
            {
                closestPoint = new FixVector2(Fix64.Clamp(position2.x, bounds1.min.x, bounds1.max.x), Fix64.Clamp(position2.z, bounds1.min.z, bounds1.max.z));
            }
            else
            {
                closestPoint = box1.normals[0] * Fix64.Clamp(FixVector2.Dot(position2.xz, box1.normals[0]), box1.intervals[0].min, box1.intervals[0].max) + box1.normals[1] * Fix64.Clamp(FixVector2.Dot(position2.xz, box1.normals[1]), box1.intervals[1].min, box1.intervals[1].max);
            }
            Fix64 closestY = Fix64.Clamp(position2.y, bounds1.min.y, bounds1.max.y);
            return (position2 - new FixVector3(closestPoint.x, closestY, closestPoint.y)).sqrMagnitude < Fix64.Square(sphere2.shape.radius);
        }

        public static bool IntersectSphereSphere(SphereCollider sphere1, SphereCollider sphere2)
        {
            var bounds1 = sphere1.bounds;
            var bounds2 = sphere2.bounds;
            if (bounds1.min.y > bounds2.max.y || bounds2.min.y > bounds1.max.y)
            {
                return false;
            }

            return (sphere2.transform.position - sphere1.transform.position).sqrMagnitude < Fix64.Square(sphere1.shape.radius + sphere2.shape.radius);
        }

        public static bool IntersectSphereBox(SphereCollider sphere1, BoxCollider box2)
        {
            return IntersectBoxSphere(box2, sphere1);
        }
    }
}
