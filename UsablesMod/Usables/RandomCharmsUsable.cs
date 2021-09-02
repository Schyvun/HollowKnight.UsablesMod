using System;
using System.Collections.Generic;
namespace UsablesMod.Usables
{
    class RandomCharmsUsable : IUsable
    {
        private readonly Random random;
        
        public RandomCharmsUsable(int randomSeed) 
        {
            random = new Random(randomSeed);
        }

        public void Run()
        {
            List<int> ownedCharms = new List<int>(); 
            for (int i = 1; i < 41; i++)
            {
                if (PlayerData.instance.GetBool("gotCharm_" + i))
                {
                    ownedCharms.Add(i);
                }
                if (PlayerData.instance.GetBool("equippedCharm_" + i))
                {
                    PlayerData.instance.SetBool("equippedCharm_" + i, false);
                    GameManager.instance.UnequipCharm(i);
                }
            }

            if (ownedCharms.Count == 0) return;

            int equippedCharmsCount = random.Next(1, ownedCharms.Count + 1);
            for (int i = 0; i < equippedCharmsCount; i++)
            {
                int charmIndex = random.Next(ownedCharms.Count);
                PlayerData.instance.SetBool("equippedCharm_" + ownedCharms[charmIndex], true);
                GameManager.instance.EquipCharm(ownedCharms[charmIndex]);
                ownedCharms.RemoveAt(charmIndex);
            }

            HeroController.instance.CharmUpdate();
            GameManager.instance.RefreshOvercharm();
        }

        public string GetName()
        {
            return "Randomize_Charms_Build_Usable";
        }

        public string GetDisplayName()
        {
            return "New Charms Build";
        }

        public string GetDescription()
        {
            return "Not to be rude, but your current charms build is not that good. Lemme show you how it's done.";
        }

        public string GetItemSpriteKey()
        {
            return "Charms.2";
        }
    }
}

