using FixMathematics;
using NUnit.Framework;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrthoPhysics.UnityEditor.Tests
{
    public class TestFix64
    {
        public const float epsilon = 0.0001f;

        static void AssertApproximately(float a, float b, float epsilon = epsilon)
        {
            try
            {
                Assert.True(Approximately(a, b, epsilon));
            }
            catch (Exception)
            {
                Debug.LogError($"Actual {a}, Expected: {b}");
                throw;
            }
        }

        static void AssertApproximately(Fix64 a, Fix64 b)
        {
            try
            {
                Assert.True(Fix64.Approximately(a, b));
            }
            catch (Exception)
            {
                Debug.LogError($"Actual {a}, Expected: {b}");
                throw;
            }
        }

        public static bool Approximately(float a, float b, float epsilon)
        {
            return Mathf.Abs(b - a) < Mathf.Max(epsilon * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), epsilon);
        }

        [Test]
        public void Sign()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                int fTarget = (int)Mathf.Sign(f1);
                Fix64 fix1 = (Fix64)f1;
                Assert.AreEqual(Fix64.Sign(fix1), fTarget);
            }
        }

        [Test]
        public void Abs()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = Mathf.Abs(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Abs(fix1), fTarget);
                AssertApproximately(Fix64.Abs(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void FastAbs()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = Mathf.Abs(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.FastAbs(fix1), fTarget);
                AssertApproximately(Fix64.FastAbs(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Floor()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = Mathf.Floor(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Floor(fix1), fTarget);
                AssertApproximately(Fix64.Floor(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Ceiling()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = Mathf.Ceil(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Ceiling(fix1), fTarget);
                AssertApproximately(Fix64.Ceiling(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Round()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = Mathf.Round(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Round(fix1), fTarget);
                AssertApproximately(Fix64.Round(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Add()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 + f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)(fix1 + fix2), fTarget);
                AssertApproximately(fix1 + fix2, (Fix64)fTarget);
            }
        }

        [Test]
        public void FastAdd()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 + f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)Fix64.FastAdd(fix1, fix2), fTarget);
                AssertApproximately(Fix64.FastAdd(fix1, fix2), (Fix64)fTarget);
            }
        }

        [Test]
        public void Sub()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 - f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)(fix1 - fix2), fTarget);
                AssertApproximately(fix1 - fix2, (Fix64)fTarget);
            }
        }

        [Test]
        public void FastSub()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 - f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)Fix64.FastSub(fix1, fix2), fTarget);
                AssertApproximately(Fix64.FastSub(fix1, fix2), (Fix64)fTarget);
            }
        }

        [Test]
        public void Mul()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 * f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)(fix1 * fix2), fTarget);
                AssertApproximately(fix1 * fix2, (Fix64)fTarget);
            }
        }

        [Test]
        public void FastMul()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 * f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)Fix64.FastMul(fix1, fix2), fTarget);
                AssertApproximately(Fix64.FastMul(fix1, fix2), (Fix64)fTarget);
            }
        }

        [Test]
        public void Div()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 / f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)(fix1 / fix2), fTarget);
                AssertApproximately(fix1 / fix2, (Fix64)fTarget);
            }
        }

        [Test]
        public void Mod()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 % f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)(fix1 % fix2), fTarget);
                AssertApproximately(fix1 % fix2, (Fix64)fTarget);
            }
        }

        [Test]
        public void FastMod()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                float fTarget = f1 % f2;
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)Fix64.FastMod(fix1, fix2), fTarget);
                AssertApproximately(Fix64.FastMod(fix1, fix2), (Fix64)fTarget);
            }
        }

        [Test]
        public void Negative()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = -f1;
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)-fix1, fTarget);
                AssertApproximately(-fix1, (Fix64)fTarget);
            }
        }

        [Test]
        public void Equal()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f1;
                Assert.True(fix1 == fix2);
            }
        }

        [Test]
        public void NotEqual()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = f1;
                while (f2 == f1)
                {
                    f2 = Random.Range(-10000f, 10000f);
                }
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                Assert.True(fix1 != fix2);
            }
        }

        [Test]
        public void Greater()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                if (f1 > f2)
                {
                    Assert.True(fix1 > fix2);
                }
            }
        }

        [Test]
        public void Less()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                if (f1 < f2)
                {
                    Assert.True(fix1 < fix2);
                }
            }
        }

        [Test]
        public void GreaterOrEqual()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                Fix64 fix3 = (Fix64)f1;
                if (f1 >= f2)
                {
                    Assert.True(fix1 >= fix2);
                }
                Assert.True(fix1 >= fix3);
            }
        }

        [Test]
        public void LessOrEqual()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float f2 = Random.Range(-10000f, 10000f);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                Fix64 fix3 = (Fix64)f1;
                if (f1 <= f2)
                {
                    Assert.True(fix1 <= fix2);
                }
                Assert.True(fix1 <= fix3);
            }
        }

        [Test]
        public void Pow2()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-5f, 5f);
                float fTarget = Mathf.Pow(2f, f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Pow2(fix1), fTarget);
                AssertApproximately(Fix64.Pow2(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Log2()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(0f, 10000f);
                float fTarget = Mathf.Log(f1, 2f);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Log2(fix1), fTarget);
                AssertApproximately(Fix64.Log2(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Ln()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(0f, 10000f);
                float fTarget = Mathf.Log(f1, (float)Math.E);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Ln(fix1), fTarget);
                AssertApproximately(Fix64.Ln(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Pow()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(0f, 2f);
                float f2 = Random.Range(0f, 5f);
                float fTarget = Mathf.Pow(f1, f2);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)Fix64.Pow(fix1, fix2), fTarget);
                AssertApproximately(Fix64.Pow(fix1, fix2), (Fix64)fTarget);
            }
        }

        [Test]
        public void Sqrt()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(0f, 10000f);
                float fTarget = Mathf.Sqrt(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Sqrt(fix1), fTarget);
                AssertApproximately(Fix64.Sqrt(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Sin()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-Mathf.Deg2Rad * 360f, Mathf.Deg2Rad * 360f);
                float fTarget = Mathf.Sin(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Sin(fix1), fTarget);
                AssertApproximately(Fix64.Sin(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void FastSin()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-Mathf.Deg2Rad * 360f, Mathf.Deg2Rad * 360f);
                float fTarget = Mathf.Sin(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.FastSin(fix1), fTarget);
                AssertApproximately(Fix64.FastSin(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Cos()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-Mathf.Deg2Rad * 360f, Mathf.Deg2Rad * 360f);
                float fTarget = Mathf.Cos(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Cos(fix1), fTarget);
                AssertApproximately(Fix64.Cos(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void FastCos()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-Mathf.Deg2Rad * 360f, Mathf.Deg2Rad * 360f);
                float fTarget = Mathf.Cos(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.FastCos(fix1), fTarget);
                AssertApproximately(Fix64.FastCos(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Tan()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1;
                float fTarget;
                do
                {
                    f1 = Random.Range(-Mathf.Deg2Rad * 360f, Mathf.Deg2Rad * 360f);
                    fTarget = Mathf.Tan(f1);

                } while (MathF.Abs(fTarget) > 10f);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Tan(fix1), fTarget);
                AssertApproximately(Fix64.Tan(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Acos()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-1f, 1f);
                float fTarget = Mathf.Acos(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Acos(fix1), fTarget);
                AssertApproximately(Fix64.Acos(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Atan()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-10000f, 10000f);
                float fTarget = Mathf.Atan(f1);
                Fix64 fix1 = (Fix64)f1;
                AssertApproximately((float)Fix64.Atan(fix1), fTarget);
                AssertApproximately(Fix64.Atan(fix1), (Fix64)fTarget);
            }
        }

        [Test]
        public void Atan2()
        {
            for (int i = 0; i < 10000; i++)
            {
                float f1 = Random.Range(-1f, 1f);
                float f2 = Random.Range(-1f, 1f);
                float fTarget = Mathf.Atan2(f1, f2);
                Fix64 fix1 = (Fix64)f1;
                Fix64 fix2 = (Fix64)f2;
                AssertApproximately((float)Fix64.Atan2(fix1, fix2), fTarget, 0.01f);
            }
        }
    }
}
