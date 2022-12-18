using System;
using UnityEngine;

namespace OrthoPhysics.UnityPlayer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DisableInPlayAttribute : PropertyAttribute
    {
        public DisableInPlayAttribute()
        {

        }
    }
}
