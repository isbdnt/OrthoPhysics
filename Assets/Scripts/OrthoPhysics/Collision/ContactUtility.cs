using FixMathematics;

namespace OrthoPhysics
{
    public static class ContactUtility
    {
        public static ContactManifold ContactAnyAny2D(Collider collider1, Collider collider2)
        {
            switch (collider1)
            {
                case BoxCollider box1:
                    switch (collider2)
                    {
                        case BoxCollider box2:
                            return ContactBoxBox2D(box1, box2);
                        case SphereCollider sphere2:
                            return ContactBoxSphere2D(box1, sphere2);
                    }
                    break;
                case SphereCollider sphere1:
                    switch (collider2)
                    {
                        case SphereCollider sphere2:
                            return ContactSphereSphere2D(sphere1, sphere2);
                        case BoxCollider box2:
                            return ContactSphereBox2D(sphere1, box2);
                    }
                    break;
            }
            return ContactManifold.empty;
        }

        public static ContactManifold ContactBoxAny2D(BoxCollider box1, Collider collider2)
        {
            switch (collider2)
            {
                case BoxCollider box2:
                    return ContactBoxBox2D(box1, box2);
                case SphereCollider sphere2:
                    return ContactBoxSphere2D(box1, sphere2);
            }
            return ContactManifold.empty;
        }

        public static ContactManifold ContactSphereAny2D(SphereCollider sphere1, Collider collider2)
        {
            switch (collider2)
            {
                case BoxCollider box2:
                    return ContactSphereBox2D(sphere1, box2);
                case SphereCollider sphere2:
                    return ContactSphereSphere2D(sphere1, sphere2);
            }
            return ContactManifold.empty;
        }

        public static ContactManifold ContactBoxBox2D(BoxCollider box1, BoxCollider box2)
        {
            FixVector2 position1 = box1.transform.position.xz;
            FixVector2 position2 = box2.transform.position.xz;
            if (box1.isAxisAligned && box2.isAxisAligned)
            {
                Bounds bounds1 = box1.bounds;
                Bounds bounds2 = box2.bounds;
                Fix64 overlap1 = Fix64.Min(bounds1.max.x - bounds2.min.x, bounds2.max.x - bounds1.min.x);
                if (overlap1 < Fix64.Zero)
                {
                    return ContactManifold.empty;
                }
                Fix64 overlap2 = Fix64.Min(bounds1.max.z - bounds2.min.z, bounds2.max.z - bounds1.min.z);
                if (overlap2 < Fix64.Zero)
                {
                    return ContactManifold.empty;
                }
                var manifold = ContactManifold.empty;
                Fix64 contactPoint1X;
                Fix64 contactPoint1Z;
                Fix64 contactPoint2X;
                Fix64 contactPoint2Z;
                if (overlap1 < overlap2)
                {
                    if (position1.x < position2.x)
                    {
                        manifold.normal = FixVector2.right;
                        contactPoint1X = bounds1.max.x;
                        contactPoint2X = bounds2.min.x;
                    }
                    else
                    {
                        manifold.normal = FixVector2.left;
                        contactPoint1X = bounds1.min.x;
                        contactPoint2X = bounds2.max.x;
                    }
                    contactPoint1Z = bounds1.min.z < bounds2.min.z ? bounds2.min.z : bounds2.max.z;
                    contactPoint2Z = contactPoint1Z;
                }
                else
                {
                    if (position1.y < position2.y)
                    {
                        manifold.normal = FixVector2.up;
                        contactPoint1Z = bounds1.max.z;
                        contactPoint2Z = bounds2.min.z;
                    }
                    else
                    {
                        manifold.normal = FixVector2.down;
                        contactPoint1Z = bounds1.min.z;
                        contactPoint2Z = bounds2.max.z;
                    }
                    contactPoint1X = bounds1.min.x < bounds2.min.x ? bounds2.min.x : bounds2.max.x;
                    contactPoint2X = contactPoint1X;
                }
                manifold.point1 = box1.transform.ToLocalSpace2D(new FixVector2(contactPoint1X, contactPoint1Z));
                manifold.point2 = box2.transform.ToLocalSpace2D(new FixVector2(contactPoint2X, contactPoint2Z));
                return manifold;
            }
            else
            {
                var manifold = ContactManifold.empty;
                Fix64 minOverlap = Fix64.MaxValue;
                for (int i = 0; i < 2; ++i)
                {
                    Fix64 overlap = Interval.GetOverlap(box1.intervals[i], box2.GetInterval(box1.normals[i]));
                    if (overlap < Fix64.Zero)
                    {
                        return ContactManifold.empty;
                    }
                    if (overlap < minOverlap)
                    {
                        minOverlap = overlap;
                        manifold.normal = box1.normals[i];
                    }
                }
                for (int i = 0; i < 2; ++i)
                {
                    Fix64 overlap = Interval.GetOverlap(box2.intervals[i], box1.GetInterval(box2.normals[i]));
                    if (overlap < Fix64.Zero)
                    {
                        return ContactManifold.empty;
                    }
                    if (overlap < minOverlap)
                    {
                        minOverlap = overlap;
                        manifold.normal = box2.normals[i];
                    }
                }
                if (FixVector2.Dot(manifold.normal, position2 - position1) < Fix64.Zero)
                {
                    manifold.normal = -manifold.normal;
                }
                Edge edge1 = box1.GetIncidentEdge(manifold.normal);
                Edge edge2 = box2.GetIncidentEdge(-manifold.normal);
                manifold.point1 = box1.transform.ToLocalSpace2D(GetEdgeContactPoint(ref edge1, ref edge2));
                manifold.point2 = box2.transform.ToLocalSpace2D(GetEdgeContactPoint(ref edge2, ref edge1));
                return manifold;
            }
        }

        static FixVector2 GetEdgeContactPoint(ref Edge incidentEdge, ref Edge referenceEdge)
        {
            FixVector2 incidentEdgeNormal = FixVector2.Perpendicular(incidentEdge.direction);
            FixVector2 referencePoint = FixVector2.Dot(referenceEdge.point1, incidentEdgeNormal) < FixVector2.Dot(referenceEdge.point2, incidentEdgeNormal) ? referenceEdge.point1 : referenceEdge.point2;
            Fix64 minIncidentEdgeProjection = FixVector2.Dot(incidentEdge.point1, incidentEdge.direction);
            Fix64 maxIncidentEdgeProjection = FixVector2.Dot(incidentEdge.point2, incidentEdge.direction);
            Fix64 referencePointProjection = Fix64.Clamp(FixVector2.Dot(referencePoint, incidentEdge.direction), minIncidentEdgeProjection, maxIncidentEdgeProjection);
            return incidentEdge.point1 + incidentEdge.direction * (referencePointProjection - minIncidentEdgeProjection);
        }

        public static ContactManifold ContactBoxSphere2D(BoxCollider box1, SphereCollider sphere2)
        {
            FixVector2 position1 = box1.transform.position.xz;
            FixVector2 position2 = sphere2.transform.position.xz;
            FixVector2 closestPoint;
            Fix64 sphereProjectionX;
            Fix64 sphereProjectionZ;
            if (box1.isAxisAligned)
            {
                sphereProjectionX = position2.x;
                sphereProjectionZ = position2.y;
                closestPoint = new FixVector2(Fix64.Clamp(sphereProjectionX, box1.bounds.min.x, box1.bounds.max.x), Fix64.Clamp(sphereProjectionZ, box1.bounds.min.z, box1.bounds.max.z));
            }
            else
            {
                sphereProjectionX = FixVector2.Dot(position2, box1.normals[0]);
                sphereProjectionZ = FixVector2.Dot(position2, box1.normals[1]);
                closestPoint = box1.normals[0] * Fix64.Clamp(sphereProjectionX, box1.intervals[0].min, box1.intervals[0].max) + box1.normals[1] * Fix64.Clamp(sphereProjectionZ, box1.intervals[1].min, box1.intervals[1].max);
            }
            var direction = position2 - closestPoint;
            Fix64 closestPointSqrDistance = direction.sqrMagnitude;
            if (closestPointSqrDistance > Fix64.Square(sphere2.shape.radius))
            {
                return ContactManifold.empty;
            }

            var manifold = ContactManifold.empty;
            if (closestPointSqrDistance == Fix64.Zero)
            {
                if (box1.isAxisAligned)
                {
                    Fix64 overlap1 = Fix64.Min(box1.bounds.max.x - sphere2.bounds.min.x, sphere2.bounds.max.x - box1.bounds.min.x);
                    if (overlap1 < Fix64.Zero)
                    {
                        return ContactManifold.empty;
                    }
                    Fix64 overlap2 = Fix64.Min(box1.bounds.max.z - sphere2.bounds.min.z, sphere2.bounds.max.z - box1.bounds.min.z);
                    if (overlap2 < Fix64.Zero)
                    {
                        return ContactManifold.empty;
                    }
                    if (overlap1 < overlap2)
                    {
                        manifold.normal = position1.x < position2.x ? FixVector2.right : FixVector2.left;
                    }
                    else
                    {
                        manifold.normal = position1.y < position2.y ? FixVector2.up : FixVector2.down;
                    }
                }
                else
                {
                    Fix64 overlap1 = Interval.GetOverlap(box1.intervals[0], new Interval(sphereProjectionX - sphere2.shape.radius, sphereProjectionX + sphere2.shape.radius));
                    if (overlap1 < Fix64.Zero)
                    {
                        return ContactManifold.empty;
                    }
                    Fix64 overlap2 = Interval.GetOverlap(box1.intervals[1], new Interval(sphereProjectionZ - sphere2.shape.radius, sphereProjectionZ + sphere2.shape.radius));
                    if (overlap2 < Fix64.Zero)
                    {
                        return ContactManifold.empty;
                    }
                    if (overlap1 < overlap2)
                    {
                        manifold.normal = box1.normals[0];
                    }
                    else
                    {
                        manifold.normal = box1.normals[1];
                    }
                    if (FixVector2.Dot(manifold.normal, position2 - position1) < Fix64.Zero)
                    {
                        manifold.normal = -manifold.normal;
                    }
                }
            }
            else
            {
                Fix64 closestPointDistance = Fix64.Sqrt(closestPointSqrDistance);
                manifold.normal = direction / closestPointDistance;
            }
            manifold.point1 = box1.transform.ToLocalSpace2D(closestPoint);
            manifold.point2 = sphere2.transform.ToLocalSpace2D(position2 - manifold.normal * sphere2.shape.radius);
            return manifold;
        }

        public static ContactManifold ContactSphereSphere2D(SphereCollider sphere1, SphereCollider sphere2)
        {
            FixVector2 position1 = sphere1.transform.position.xz;
            FixVector2 position2 = sphere2.transform.position.xz;
            Fix64 radius1 = sphere1.shape.radius;
            Fix64 radius2 = sphere2.shape.radius;
            Fix64 combinedRadius = radius1 + radius2;
            FixVector2 direction = position2 - position1 + FixVector2.one * Fix64.Epsilon;
            Fix64 sqrDistance = direction.sqrMagnitude;
            if (sqrDistance > Fix64.Square(combinedRadius))
            {
                return ContactManifold.empty;
            }

            var manifold = ContactManifold.empty;
            if (sqrDistance < Fix64.Epsilon)
            {
                manifold.normal = FixVector2.right;
            }
            else
            {
                manifold.normal = direction / Fix64.Sqrt(sqrDistance);
            }
            manifold.point1 = sphere1.transform.ToLocalSpace2D(position1 + manifold.normal * radius1);
            manifold.point2 = sphere2.transform.ToLocalSpace2D(position2 - manifold.normal * radius2);
            return manifold;
        }

        public static ContactManifold ContactSphereBox2D(SphereCollider sphere1, BoxCollider box2)
        {
            var manifold = ContactBoxSphere2D(box2, sphere1);
            manifold.normal = -manifold.normal;
            var tempPoint = manifold.point1;
            manifold.point1 = manifold.point2;
            manifold.point2 = tempPoint;
            return manifold;
        }
    }
}
