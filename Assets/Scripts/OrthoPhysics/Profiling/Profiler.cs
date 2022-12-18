
namespace OrthoPhysics
{
    public static class Profiler
    {
        public static void BeginSample(string name)
        {
            UnityEngine.Profiling.Profiler.BeginSample(name);
        }

        public static void EndSample()
        {
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}
