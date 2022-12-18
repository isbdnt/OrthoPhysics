using FixMathematics;

namespace OrthoPhysics
{
    public class ContactConstraint
    {
        public Body body1;
        public Body body2;
        public Fix64 normalMass;
        public Fix64 normalImpulse;
        public Fix64 trapPenetrationDepth;
        public ContactManifold manifold;
    }
}
