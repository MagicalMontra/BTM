using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class LifeItemDrop : ItemDropBase
    {
        [InjectOptional]
        private int healAmount = 1;
        
        [Inject]
        private HeartPanel heartPanel;
        
        [Inject]
        private PlayerSpawner playerSpawner;

        [Inject]
        private readonly SoundData soundData;
        
        [Inject]
        private readonly PecanServices services;
        
        public override async UniTask Activate()
        {
            playerSpawner.Current.Heal(healAmount);
            isDespawning = true;
            await UniTask.WaitUntil(() => !isDespawning);
            await UniTask.Delay(Random.Range(50, 100));
            heartPanel.UpdateHeart(playerSpawner.Current.CurrentHealth);
            services.SoundManager.SoundPlayer.Play(soundData.HeartCollect, false);
            transform.DOMoveY(transform.position.y + Random.Range(3, 5f), 0.45f).SetEase(Ease.OutBack);
            renderer.DOFade(0f, 0.4f);
            await ActivatePickupVfx();
        }
    }
}