using System.Collections.Generic;
using HotPlay.BoosterMath.Core.Character;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace HotPlay.BoosterMath.Core.Player
{
    public class PlayerInstaller : MonoInstaller<PlayerInstaller>
    {
        [Inject]
        private ShopDatabase shopDatabase;

        private CharacterData[] characterData;

        public override void InstallBindings()
        {
            Container.Bind<PlayerSpawner>().AsSingle();

            characterData = new CharacterData[shopDatabase.CharacterData.Length];
            
            for (int i = 0; i < shopDatabase.CharacterData.Length; i++)
            {
                characterData[i] = shopDatabase.CharacterData[i].characterData;
            }
            
            Container.Bind<IEnumerable<CharacterData>>().FromInstance(characterData).AsSingle();
            Container.BindFactory<Object, Transform, ICharacter, ICharacter.Factory>().FromFactory<CharacterFactory>();
        }
    }
}