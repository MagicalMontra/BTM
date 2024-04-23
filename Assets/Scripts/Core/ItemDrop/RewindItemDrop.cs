using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class RewindItemDrop : ItemDropBase
    {
        [InjectLocal]
        private ITimer timer;
        
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
        private readonly GameSessionController gameSessionController;

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
            itemDropController.OnBoosterActivated(ItemDropTypeEnum.Rewind, true);
            await ActivatePickupVfx();
            timer.Start();
        }
        
        private void Awake()
        {
            timer.OnStart += OnActivate;
            timer.OnTicked += OnTicked;
            timer.OnStop += OnDeactivate;
            gameSessionController.OnScoreIncreased += OnScoreIncreased;
        }
        
        private void OnDestroy()
        {
            timer.OnStart -= OnActivate;
            timer.OnTicked -= OnTicked;
            timer.OnStop -= OnDeactivate;
            gameSessionController.OnScoreIncreased -= OnScoreIncreased;
        }
        
        internal override void OnReinitialize()
        {
            tutorialController.BoosterTutorialPass(ItemDropTypeEnum.Rewind);
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
        
        private void OnScoreIncreased()
        {
            boosterPanel.OnBoosterEffectApplied(type);
        }
    }
}