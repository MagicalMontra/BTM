using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace HotPlay.BoosterMath.Core.Character
{
    public interface ICharacter
    {
        AttackTypeEnum AttackType { get; }
        
        bool IsFloating { get; }
        
        string Id { get; }
        
        int MaxHealth { get; }
        
        int AttackPower { get; }
        
        int CurrentHealth { get; }
        
        Transform transform { get; }
        
        MeshRenderer Renderer { get; }
        
        WorldSpaceVFXBase AttackVfx { get; }
        
        WorldSpaceVFXBase GetHitVfx { get; }

        void Reinitialize();

        void Dispose();
        
        void Idle(CancellationToken cancellationToken);

        void Walk(CancellationToken cancellationToken);

        void Heal(int amount);

        UniTask Attack(CancellationToken cancellationToken);

        UniTask GetHit(int damage, CancellationToken cancellationToken);

        UniTask Death(CancellationToken cancellationToken);

        public class Factory : PlaceholderFactory<Object, Transform, ICharacter>
        {
            
        }

        [Serializable]
        public enum AttackTypeEnum
        {
            Melee,
            Ranged
        }
    }

    public class CharacterFactory : IFactory<Object, Transform, ICharacter>
    {
        private DiContainer container;

        public CharacterFactory(DiContainer container)
        {
            this.container = container;
        }

        public ICharacter Create(Object prefab, Transform parent)
        {
            return container.InstantiatePrefabForComponent<ICharacter>(prefab, parent);
        }
    }
}