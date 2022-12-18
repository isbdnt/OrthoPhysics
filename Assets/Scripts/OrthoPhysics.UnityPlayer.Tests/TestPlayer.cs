using FixMathematics;
using UnityEngine;

namespace OrthoPhysics.UnityPlayer.Tests
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] bool _moveByTranslate;
        [SerializeField] Fix64 _velocityMoveSpeed;
        [SerializeField] Fix64 _translateMoveSpeed;
        [SerializeField] FixVector3 _impulse;
        OrthoBody _bodyProxy;

        private void Awake()
        {
            _bodyProxy = GetComponent<OrthoBody>();
        }

        private void FixedUpdate()
        {
            if (_bodyProxy == null)
            {
                return;
            }

            FixVector2 moveDirection = FixVector2.zero;
            if (Input.GetKey(KeyCode.W))
            //if (true)
            {
                moveDirection += FixVector2.up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDirection += FixVector2.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += FixVector2.right;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDirection += FixVector2.left;
            }
            if (_moveByTranslate)
            {
                moveDirection = moveDirection.normalized * _translateMoveSpeed;
                _bodyProxy.body.transform.position += new FixVector3(moveDirection.x, Fix64.Zero, moveDirection.y);
            }
            else
            {
                moveDirection = moveDirection.normalized * _velocityMoveSpeed;
                if (moveDirection != FixVector2.zero)
                {
                    var vel = new FixVector3(moveDirection.x, Fix64.Zero, moveDirection.y);
                    vel.y = _bodyProxy.body.velocity.y;
                    _bodyProxy.body.velocity = vel;
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    _bodyProxy.body.AddImpulse(_impulse);
                }
            }
        }
    }
}