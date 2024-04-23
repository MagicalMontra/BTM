using System;
using Eflatun.SceneReference;

namespace HotPlay.PecanUI.SceneLoader
{
    public class SceneLoadSignalData
    {
        public SceneReference Scene { get; private set; }

        public Action Callback { get; private set; }

        public SceneLoadSignalData(SceneReference scene, Action callback)
        {
            Scene = scene;
            Callback = callback;
        }
    }
}