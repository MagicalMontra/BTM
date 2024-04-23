using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.PecanUI;
using Zenject;
using Random = UnityEngine.Random;

namespace HotPlay.BoosterMath.Core
{
    public class SlowItemDrop : ItemDropBase
    {
        [InjectLocal]
        private readonly ITimer timer;

        [Inject]
        private readonly SoundData soundData;

        [Inject]
        private readonly PecanServices services;

        [Inject]
        private readonly ItemDropTypeEnum type;
            
        [Inject]
        private readonly BoosterPanel boosterPanel;

        [Inject]
        private readonly TutorialController tutorialController;

        [Inject]
        private readonly ItemDropController itemDropController;

        public override async UniTask Activate()
        {
            isDespawning = true;
            await UniTask.WaitUntil(() => !isDespawning);
            await UniTask.Delay(Random.Range(50, 100));
            services.SoundManager.SoundPlayer.Play(soundData.BoosterCollect, false);
            transform.DOMoveY(transform.position.y + Random.Range(3, 5f), 0.45f).SetEase(Ease.OutBack);
            renderer.DOFade(0f, 0.4f);
            itemDropController.OnBoosterActivated(ItemDropTypeEnum.Slow, true);
            await ActivatePickupVfx();
            timer.Start();
        }
        
        private void Awake()
        {
            timer.OnStart += OnActivate;
            timer.OnTicked += OnTicked;
            timer.OnStop += OnDeactivate;
        }
        
        private void OnDestroy()
        {
            timer.OnStart -= OnActivate;
            timer.OnTicked -= OnTicked;
            timer.OnStop -= OnDeactivate;
        }
        
        internal override void OnReinitialize()
        {
            tutorialController.BoosterTutorialPass(ItemDropTypeEnum.Slow);
        }

        private void OnActivate()
        {
            boosterPanel.OnBoosterActivate(type);
        }
        
        private void OnTicked()
        {
            boosterPanel.OnBoosterTicked(type, timer);
        }

        private void OnDeactivate()
        {
            boosterPanel.OnBoosterDeactivate(type);
            itemDropController.OnBoosterActivated(type, false);
        }
    }
}