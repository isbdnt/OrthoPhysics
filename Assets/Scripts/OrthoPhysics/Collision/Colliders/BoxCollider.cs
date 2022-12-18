using FixMathematics;

namespace OrthoPhysics
{
    public class BoxCollider : Collider
    {
        public override Bounds bounds => _bounds;
        public BoxShape shape => _shape;
        public bool isAxisAligned => _isAxisAligned;
        public Interval[] intervals
        {
            get
            {
                UpdateCache();
                return _intervals;
            }
        }
        public FixVector2[] normals
        {
            get
            {
                UpdateCache();
                return _normals;
            }
        }

        Bounds _bounds;
        BoxShape _shape;
        bool _isAxisAligned;
        FixVector2[] _points = new FixVector2[4];
        FixVector2[] _normals = new FixVector2[2];
        Interval[] _intervals = new Interval[2];
        bool _isCacheValid;

        public BoxCollider(BoxShape shape)
        {
            _shape = shape;
        }

        public override Fix64 GetHalfHeight()
        {
            return _shape.halfExtents.y;
        }

        public override void UpdateBounds()
        {
            Fix64 result = Fix64.Abs(FixComplex.Dot(transform.rotation, FixComplex.identify));
            _isAxisAligned = Fix64.Approximately(result, Fix64.One) || Fix64.Approximately(result, Fix64.Zero);
            FixVector3 halfExtents = _shape.halfExtents;
            if (!_isAxisAligned)
            {
                FixVector2 x = _transform.rotation * new FixVector2(halfExtents.x, Fix64.Zero);
                FixVector2 z = _transform.rotation * new FixVector2(Fix64.Zero, halfExtents.z);
                FixVector2 abs = x.abs + z.abs;
                halfExtents = new FixVector3(abs.x, halfExtents.y, abs.y);
            }
            _bounds = new Bounds(_transform.position - halfExtents, _transform.position + halfExtents);
            _isCacheValid = false;
        }

        public Interval GetInterval(FixVector2 normal)
        {
            UpdateCache();
            Fix64 min = FixVector2.Dot(normal, _points[0]);
            Fix64 max = min;
            for (int i = 1; i < 4; ++i)
            {
                Fix64 projection = FixVector2.Dot(normal, _points[i]);
                if (projection < min)
                {
                    min = projection;
                }
                else if (projection > max)
                {
                    max = projection;
                }
            }
            return new Interval(min, max);
        }

        public Edge GetIncidentEdge(FixVector2 normal)
        {
            UpdateCache();
            Fix64 projection1 = FixVector2.Dot(normal, _normals[0]);
            Fix64 projection2 = FixVector2.Dot(normal, _normals[1]);
            if (Fix64.Abs(projection1) < Fix64.Abs(projection2))
            {
                if (projection2 < Fix64.Zero)
                {
                    return new Edge(_points[0], _points[3]);
                }
                else
                {
                    return new Edge(_points[2], _points[1]);
                }
            }
            else
            {
                if (projection1 < Fix64.Zero)
                {
                    return new Edge(_points[3], _points[2]);
                }
                else
                {
                    return new Edge(_points[1], _points[0]);
                }
            }
        }

        void UpdateCache()
        {
            if (_isCacheValid)
            {
                return;
            }
            _isCacheValid = true;
            if (_isAxisAligned)
            {
                _points[0] = new FixVector2(_bounds.max.x, _bounds.min.z);
                _points[1] = new FixVector2(_bounds.max.x, _bounds.max.z);
                _points[2] = new FixVector2(_bounds.min.x, _bounds.max.z);
                _points[3] = new FixVector2(_bounds.min.x, _bounds.min.z);
                _normals[0] = FixVector2.right;
                _normals[1] = FixVector2.up;
                _intervals[0] = new Interval(_bounds.min.x, _bounds.max.x);
                _intervals[1] = new Interval(_bounds.min.z, _bounds.max.z);
            }
            else
            {
                _points[0] = _transform.ToWorldSpace2D(new FixVector2(_shape.halfExtents.x, -_shape.halfExtents.z));
                _points[1] = _transform.ToWorldSpace2D(new FixVector2(_shape.halfExtents.x, _shape.halfExtents.z));
                _points[2] = _transform.ToWorldSpace2D(new FixVector2(-_shape.halfExtents.x, _shape.halfExtents.z));
                _points[3] = _transform.ToWorldSpace2D(new FixVector2(-_shape.halfExtents.x, -_shape.halfExtents.z));
                _normals[0] = _transform.rotation.direction;
                _normals[1] = FixVector2.Perpendicular(_normals[0]);
                _intervals[0] = GetOrthogonalInterval(_normals[0]);
                _intervals[1] = GetOrthogonalInterval(_normals[1]);
            }
        }

        Interval GetOrthogonalInterval(FixVector2 normal)
        {
            Fix64 projection1 = FixVector2.Dot(normal, _points[0]);
            Fix64 projection2 = FixVector2.Dot(normal, _points[2]);
            if (projection1 < projection2)
            {
                return new Interval(projection1, projection2);
            }
            else
            {
                return new Interval(projection2, projection1);
            }
        }
    }
}
