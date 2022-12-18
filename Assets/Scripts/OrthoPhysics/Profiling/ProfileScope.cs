using System;

namespace OrthoPhysics
{
    public struct ProfileScope : IDisposable
    {
        bool _isDisposed;

        public ProfileScope(string name)
        {
            _isDisposed = false;
            Profiler.BeginSample(name);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Profiler.EndSample();
            }
        }
    }
}
