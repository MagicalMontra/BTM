using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class CoinItemDrop : ItemDropBase
    {
        [Inject]
        private readonly SoundData soundData;

        [Inject]
        private readonly PecanServices services;

        [Inject]
        private GameSessionController sessionController;

        public override async UniTask Activate()
        {
            isDespawning = true;
            await UniTask.WaitUntil(() => !isDespawning);
            await UniTask.Delay(Random.Range(50, 100));
            sessionController.IncreaseCoin();
            services.SoundManager.SoundPlayer.Play(soundData.CoinCollect, false);
            transform.DOMoveY(transform.position.y + Random.Range(3, 5f), 0.45f).SetEase(Ease.OutBack);
            renderer.DOFade(0f, 0.4f);
            await ActivatePickupVfx();
        }
    }
}