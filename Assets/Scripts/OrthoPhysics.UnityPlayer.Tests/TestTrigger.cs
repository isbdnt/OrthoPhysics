using UnityEngine;

namespace OrthoPhysics.UnityPlayer.Tests
{
    public class TestTrigger : MonoBehaviour
    {
        OrthoBody _bodyProxy;

        private void Awake()
        {
            _bodyProxy = GetComponent<OrthoBody>();
            _bodyProxy.onTriggerEnter += OnBodyTriggerEnter;
            _bodyProxy.onTriggerStay += OnBodyTriggerStay;
            _bodyProxy.onTriggerExit += OnBodyTriggerExit;
        }

        void OnBodyTriggerEnter(OrthoBody body)
        {
            //Debug.Log("OnBodyTriggerEnter: " + body.name);
        }

        void OnBodyTriggerStay(OrthoBody body)
        {
            //Debug.Log("OnBodyTriggerStay: " + body.name);
        }

        void OnBodyTriggerExit(OrthoBody body)
        {
            //Debug.Log("OnBodyTriggerExit: " + body.name);
        }
    }
}