using FixMathematics;
using System;

namespace OrthoPhysics
{
    public class BoxShape : Shape
    {
        public FixVector3 halfExtents
        {
            get => _halfExtents;
            set => _halfExtents = value;
        }

        FixVector3 _halfExtents;

        public BoxShape(FixVector3 halfExtents)
        {
            _halfExtents = halfExtents;
        }
    }
}
