using FixMathematics;
using UnityEngine;
using UnityExtensions;

namespace OrthoPhysics.UnityPlayer
{
    public abstract class OrthoCollider : MonoBehaviour
    {
        public new abstract Collider collider { get; }
        protected abstract bool isDirty { get; }

        public abstract bool ApplyChangesFromUnity();
    }
}
