using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using UnityEngine;

namespace HotPlay.BoosterMath.Core.Player
{
    public class PlayerSpawner
    {
        public ICharacter Current { get; private set; }

        private readonly string defaultKey = "Character01";
        
        private readonly SoundData soundData;
        
        private readonly PecanServices services;

        private readonly ICharacter.Factory factory;
        
        private readonly IGameplayPanel gameplayPanel;

        private readonly Dictionary<string, Object> playableDictionary;

        public PlayerSpawner(SoundData soundData, PecanServices services, IGameplayPanel gameplayPanel, ICharacter.Factory factory, IEnumerable<CharacterData> playableList)
        {
            this.factory = factory;
            this.services = services;
            this.soundData = soundData;
            this.gameplayPanel = gameplayPanel;
            playableDictionary = new Dictionary<string, Object>();
            
            foreach (var playable in playableList)
            {
                playableDictionary.Add(playable.id, playable.prefab);
            }
        }
        
        public async UniTask Spawn(string id, CancellationToken cancellationToken)
        {
            if (Current?.Id == id)
            {
                await UniTask.Yield();
                return;
            }
            
            if (Current != null)
            {
                Current.Dispose();
                Current = null;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            var hasKey = playableDictionary.ContainsKey(id);
            var selectedPrefab = hasKey ? playableDictionary[id] : playableDictionary[defaultKey];
            Current = factory.Create(selectedPrefab, gameplayPanel.PlayerPivot);
            var position = Current.transform.position;
            var originalPos = position;
            position = new Vector3(position.x - 30f, position.y, position.z);
            Current.transform.position = position;
            Current.Reinitialize();
            Current.Walk(cancellationToken);
            var soundId = -1;
            if (!Current.IsFloating)
                soundId = services.SoundManager.SoundPlayer.Play(soundData.PlayerStep, true);
            
            await Current.transform.DOMoveX(originalPos.x, 1f).WithCancellation(cancellationToken);
            services.SoundManager.SoundPlayer.Stop(soundId);
            await UniTask.Delay(250, DelayType.Realtime, PlayerLoopTiming.Update, cancellationToken);
        }

        public void Despawn()
        {
            Current?.Dispose();
            Current = null;
        }
    }
}