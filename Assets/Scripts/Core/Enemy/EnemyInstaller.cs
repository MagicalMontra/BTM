using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.Player;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Enemy
{
    public abstract class EnemyInstaller : MonoInstaller<EnemyInstaller>
    {
        private CharacterData[] data;
        
        public override void InstallBindings()
        {
            BindSelector();
            Container.BindFactory<Object, Transform, ICharacter, ICharacter.Factory>().FromFactory<CharacterFactory>();
        }

        protected abstract void BindSelector();
    }
}
