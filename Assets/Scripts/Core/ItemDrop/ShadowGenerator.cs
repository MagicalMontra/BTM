using System.Collections.Generic;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ShadowGenerator : IInitializable, ILateDisposable
    {
        [Inject]
        private Shadow.Pool pool;

        [Inject]
        private PecanServices pecanServices;

        private List<Shadow> actives = new List<Shadow>();

        public void Initialize()
        {
            pecanServices.Events.GameplayEventsHandler.Restart += OnGameFinished;
            pecanServices.Events.GameplayEventsHandler.BackToMainMenu += OnGameFinished;
        }

        public void LateDispose()
        {
            pecanServices.Events.GameplayEventsHandler.Restart -= OnGameFinished;
            pecanServices.Events.GameplayEventsHandler.BackToMainMenu -= OnGameFinished;
        }
        
        public Shadow Spawn(Transform reference, Vector3 position)
        {
            var instance = pool.Spawn(reference, position);
            actives.Add(instance);
            return instance;
        }

        public void Despawn(Shadow shadow)
        {
            if (shadow == null)
                return;

            if (!actives.Contains(shadow)) 
                return;
            
            actives.Remove(shadow);
            pool.Despawn(shadow);
        }

        private void OnGameFinished()
        {
            foreach (var shadow in actives)
            {
                pool.Despawn(shadow);
            }
            
            actives.Clear();
        }
    }
}