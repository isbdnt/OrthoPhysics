using UnityEngine;

namespace OrthoPhysics.UnityPlayer.Tests
{
    public class TestOverlap : MonoBehaviour
    {
        [SerializeField]
        OrthoCollider _collider1;
        [SerializeField]
        OrthoCollider _collider2;

        private void FixedUpdate()
        {
            bool isOverlapped = false;
            switch (_collider1.collider)
            {
                case BoxCollider box1:
                    switch (_collider2.collider)
                    {
                        case BoxCollider box2:
                            isOverlapped = IntersectUtility.IntersectBoxBox(box1, box2);
                            break;
                        case SphereCollider sphere2:
                            isOverlapped = IntersectUtility.IntersectBoxSphere(box1, sphere2);
                            break;
                    }
                    break;
                case SphereCollider sphere1:
                    switch (_collider2.collider)
                    {
                        case SphereCollider sphere2:
                            isOverlapped = IntersectUtility.IntersectSphereSphere(sphere1, sphere2);
                            break;
                        case BoxCollider box2:
                            isOverlapped = IntersectUtility.IntersectSphereBox(sphere1, box2);
                            break;
                    }
                    break;
            }
            if (isOverlapped)
            {
                Debug.Log("Overlapped");
            }
        }
    }
}