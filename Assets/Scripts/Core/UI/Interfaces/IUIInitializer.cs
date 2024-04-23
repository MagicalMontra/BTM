using System;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public interface IUIInitializer : IInitializable, IDisposable
    {
        void Setup();

        void Terminate();
    }
}
