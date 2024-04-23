using System.Collections.Generic;

namespace HotPlay.BoosterMath.Core
{
    public class ThemeSelector
    {
        public ThemeData Current { get; private set; }

        private readonly string defaultKey = "Theme01";

        private readonly ParallaxController parallaxController;
        
        private readonly Dictionary<string, ThemeData> themeDictionary;

        public ThemeSelector(ParallaxController parallaxController, IEnumerable<ThemeData> themeList)
        {
            this.parallaxController = parallaxController;
            themeDictionary = new Dictionary<string, ThemeData>();
            
            foreach (var theme in themeList)
            {
                themeDictionary.Add(theme.id, theme);
            }
        }
        
        public void Select(string id)
        {
            if (Current != null)
                Current = null;

            var hasKey = themeDictionary.ContainsKey(id);
            Current = hasKey ? themeDictionary[id] : themeDictionary[defaultKey];
            parallaxController.Select(Current.id);
        }
    }
}
