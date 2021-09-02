using System;

namespace UsablesMod.Usables
{
    class LifebloodUsable : IUsable
    {
        private readonly int amount;

        public LifebloodUsable(int randomSeed)
        {
            amount = new Random(randomSeed).Next(2, 10);
        }

        public void Run()
        {
            for (int i = 0; i < amount; i++)
            {
                EventRegister.SendEvent("ADD BLUE HEALTH");
            }
        }

        public string GetName()
        {
            return "Lifeblood_Usable";
        }
        public string GetDisplayName()
        {
            return "Some Lifeblood Masks";
        }
        public string GetDescription()
        {
            return "Limited edition Lifeblood masks - get them while they last!";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.Lifeblood";
        }
    }
}
