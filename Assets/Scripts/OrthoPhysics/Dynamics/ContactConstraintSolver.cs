using FixMathematics;
using System;
using System.Collections.Generic;

namespace OrthoPhysics
{
    public class ContactConstraintSolver
    {
        public Fix64 maxPenetrationDepth
        {
            get => _maxPenetrationDepth;
            set => _maxPenetrationDepth = Fix64.Max(Fix64.Epsilon, value);
        }
        public int velocityIterationCount
        {
            get => _velocityIterationCount;
            set => _velocityIterationCount = Math.Max(0, value);
        }
        public int positionIterationCount
        {
            get => _positionIterationCount;
            set => _positionIterationCount = Math.Max(0, value);
        }
        public int trapStaticCollisionCount
        {
            get => _trapStaticCollisionCount;
            set => _trapStaticCollisionCount = Math.Max(0, value);
        }

        int _velocityIterationCount = 8;
        int _positionIterationCount = 8;
        int _trapStaticCollisionCount;
        Fix64 _maxPenetrationDepth = (Fix64)0.005f;
        Fix64 _correctionScale = (Fix64)0.2f;
        Fix64 _correctionSlop = (Fix64)0.005f;
        Fix64 _maxCorrection = (Fix64)0.1f;

        public ContactConstraintSolver()
        {
        }

        public void SolveVelocity(List<ContactConstraint> contactContraints)
        {
            using (new ProfileScope("SolveVelocity"))
            {
                for (int i = 0; i < _velocityIterationCount; ++i)
                {
                    foreach (var contactContraint in contactContraints)
                    {
                        SolveVelocity(contactContraint);
                    }
                }
            }
        }

        public void SolvePosition(List<ContactConstraint> contactContraints)
        {
            using (new ProfileScope("SolvePosition"))
            {
                for (int i = 0; i < _positionIterationCount; ++i)
                {
                    foreach (var contactContraint in contactContraints)
                    {
                        SolvePosition(contactContraint);
                    }
                }
            }
        }

        void SolveVelocity(ContactConstraint contactConstraint)
        {
            FixVector2 relativeVelocity = (contactConstraint.body1.velocity - contactConstraint.body2.velocity).xz;
            Fix64 normalImpulse = FixVector2.Dot(relativeVelocity, contactConstraint.manifold.normal);
            if (Fix64.Abs(normalImpulse) < Fix64.Epsilon)
            {
                return;
            }

            Fix64 newImpulse = Fix64.Max(Fix64.Zero, contactConstraint.normalImpulse + normalImpulse);
            normalImpulse = newImpulse - contactConstraint.normalImpulse;
            contactConstraint.normalImpulse = newImpulse;
            Fix64 restitution = contactConstraint.body1.bounciness * contactConstraint.body2.bounciness;
            FixVector2 impulse = contactConstraint.manifold.normal * (Fix64.One + restitution) * normalImpulse * contactConstraint.normalMass;
            contactConstraint.body1.AddImpulse2D(-impulse);
            contactConstraint.body2.AddImpulse2D(impulse);
        }

        void SolvePosition(ContactConstraint contactConstraint)
        {
            FixVector2 contactPoint1 = contactConstraint.body1.transform.ToWorldSpace2D(contactConstraint.manifold.point1);
            FixVector2 contactPoint2 = contactConstraint.body2.transform.ToWorldSpace2D(contactConstraint.manifold.point2);
            Fix64 penetrationDepth = FixVector2.Dot(contactPoint1 - contactPoint2, contactConstraint.manifold.normal);
            if (penetrationDepth < Fix64.Epsilon)
            {
                return;
            }

            if (contactConstraint.body2.kind == BodyKind.Static && contactConstraint.body1.staticCollisionCount >= _trapStaticCollisionCount)
            {
                if (contactConstraint.trapPenetrationDepth > penetrationDepth)
                {
                    contactConstraint.trapPenetrationDepth = penetrationDepth;
                    Fix64 normalCorrection = Fix64.Min(Fix64.Min(_maxCorrection, penetrationDepth), _correctionScale * (penetrationDepth + _correctionSlop));
                    FixVector2 translation = contactConstraint.manifold.normal * normalCorrection;
                    contactConstraint.body1.AddTranslation2D(-translation);
                }
                else
                {
                    FixVector2 movement = contactConstraint.body1.transform.position.xz - contactConstraint.body1.collisionFreePosition;
                    Fix64 movementSqrMagnitude = movement.sqrMagnitude;
                    if (movementSqrMagnitude > Fix64.Epsilon)
                    {
                        Fix64 movementMagnitude = Fix64.Sqrt(movementSqrMagnitude);
                        Fix64 movementCorrection = Fix64.Min(Fix64.Min(_maxCorrection, movementMagnitude), (Fix64.One - _correctionScale) * (movementMagnitude + _correctionSlop));
                        contactConstraint.body1.AddTranslation2D(movement / movementMagnitude * -movementCorrection);
                    }
                }
            }
            else
            {
                Fix64 normalCorrection = Fix64.Min(Fix64.Min(_maxCorrection, penetrationDepth), _correctionScale * (penetrationDepth + _correctionSlop));
                FixVector2 translation = contactConstraint.manifold.normal * normalCorrection * contactConstraint.normalMass;
                contactConstraint.body1.AddTranslation2D(-translation * contactConstraint.body1.inverseMass);
                contactConstraint.body2.AddTranslation2D(translation * contactConstraint.body2.inverseMass);
            }
        }
    }
}
