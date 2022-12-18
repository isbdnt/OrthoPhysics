using FixMathematics;
using System.Transactions;
using UnityEngine;

namespace OrthoPhysics.UnityPlayer.Tests
{
    public class TestCamera : MonoBehaviour
    {
        [SerializeField] bool _follow = true;
        [SerializeField] Vector3 _offset;
        [SerializeField] GameObject _followTarget;

        private void Awake()
        {
        }

        private void LateUpdate()
        {
            if (_follow)
            {
                transform.position = _followTarget.transform.position + _offset;
            }
        }
    }
}