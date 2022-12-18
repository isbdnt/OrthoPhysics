using FixMathematics;
using System;

namespace OrthoPhysics
{
    public abstract class Collider
    {
        public abstract Bounds bounds { get; }
        public Transform transform => _transform;

        protected Transform _transform = new Transform();

        public Collider()
        {

        }
        public abstract Fix64 GetHalfHeight();
        public abstract void UpdateBounds();
    }
}
