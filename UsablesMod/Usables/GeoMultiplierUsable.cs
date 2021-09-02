using System;

namespace UsablesMod.Usables
{
    class GeoMultiplierUsable : IUsable
    {
        private readonly bool isGoodMultiplier;

        private Random random;
        private string displayName;

        public GeoMultiplierUsable(int randomSeed)
        {
            random = new Random(randomSeed);
            isGoodMultiplier = random.Next(2) == 0;
            displayName = "Geo Multiplier";
        }

        public void Run()
        {
            if (isGoodMultiplier)
            {
                HeroController.instance.TakeGeo(PlayerData.instance.geo * random.Next(2, 5) / 10);
                displayName = "Thanks for the geo!";
            } 
            else
            {
                HeroController.instance.AddGeo(PlayerData.instance.geo * random.Next(6, 10) / 10);
                displayName = "Ad Revenue";
            }
        }

        public string GetName()
        {
            return "Geo_Multiplier_Usable";
        }
        public string GetDisplayName()
        {
            return displayName;
        }
        public string GetDescription()
        {
            return "It may not be safe, but it could be worth it...";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.Geo";
        }

    }
}
