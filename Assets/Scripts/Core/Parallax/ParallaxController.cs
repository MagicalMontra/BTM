using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HotPlay.BoosterMath.Core
{
    public class ParallaxController
    {
        public ParallaxThemeController Current { get; private set; }

        private const string defaultKey = "theme01";
        
        private readonly ParallaxThemeController.Factory factory;

        private readonly Dictionary<string, ParallaxThemeController> parallaxDict = new Dictionary<string, ParallaxThemeController>();

        public ParallaxController(ParallaxThemeController.Factory factory, IEnumerable<ParallaxThemeController> parallaxList)
        {
            this.factory = factory;
            
            foreach (var parallax in parallaxList)
            {
                parallaxDict.Add(parallax.Id, parallax);  
            }
        }

        public void Select(string id)
        {
            if (Current != null)
            {
                Object.Destroy(Current.gameObject);
                Current = null;
            }

            if (!parallaxDict.ContainsKey(id))
            {
                id = defaultKey;
                Assert.IsTrue(!string.IsNullOrEmpty(id));
            }

            Current = factory.Create(parallaxDict[id]);
        }
    }
}