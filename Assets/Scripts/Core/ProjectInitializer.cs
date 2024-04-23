using UnityEngine;
using Zenject;
using Rewired;
using UnityEngine.EventSystems;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Core/Create ProjectInitializer", fileName = "ProjectInitializer", order = 0)]
    public class ProjectInitializer : ScriptableObjectInstaller<ProjectInitializer>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}