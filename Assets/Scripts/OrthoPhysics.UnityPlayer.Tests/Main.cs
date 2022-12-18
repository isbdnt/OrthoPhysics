using UnityEngine;
using FixMathematics;
using System.Collections.Generic;
using System;

namespace OrthoPhysics.UnityPlayer.Tests
{
    public static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoad()
        {
            Setup();
        }

        static void Setup()
        {
            QualitySettings.vSyncCount = 1;
            Time.fixedDeltaTime = 1f / 60f;
            Application.targetFrameRate = 60;
        }
    }
}