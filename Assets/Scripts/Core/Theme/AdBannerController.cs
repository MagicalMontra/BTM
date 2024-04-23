using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class AdBannerController
    {
        [Inject(Id = "AdSpawnPoint")]
        private Transform spawnPoint;
        
        [Inject]
        private AdBanner.Factory factory;
        
        [Inject]
        private ThemeSelector themeSelector;

        private string currentId;
        
        private AdBanner current;

        public void Show()
        {
            Assert.IsNotNull(themeSelector.Current);
            
            if (currentId != themeSelector.Current.id)
            {
                if (current != null)
                {
                    current.Dispose();
                    current = null;
                }
            }

            if (current == null)
            {
                currentId = themeSelector.Current.id;
                current = factory.Create(themeSelector.Current.adBanner);
                current.Initialize(spawnPoint.position);
                return;
            }
            
            current.SetActive(true);
        }

        public void Hide()
        {
            if (current != null)
                current.SetActive(false);
        }
    }
}