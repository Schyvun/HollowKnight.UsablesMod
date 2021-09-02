using HutongGames.PlayMaker;
using System.Collections;
using UnityEngine;

namespace UsablesMod.Usables
{
    class MPCostUsable : IUsable, IRevertable
    {
        private bool running = false;
        private readonly int multiplier;
        private FsmInt mpCost;
        private string displayName;

        public MPCostUsable(int randomSeed)
        {
            multiplier = new System.Random(randomSeed).Next(0, 2) * 2;
            displayName = "Stell Twisper";
        }

        public void Run()
        {
            running = true;
            mpCost = HeroController.instance.gameObject.LocateMyFSM("Spell Control").FsmVariables.FindFsmInt("MP Cost");
            mpCost.Value *= multiplier;
            if (multiplier > 1)
            {
                displayName = "Soul Eater";
            }
            else
            {
                displayName = "Stell Twisperer";
            }

            GameManager.instance.StartCoroutine(WaitingForBench());
        }

        private IEnumerator WaitingForBench()
        {
            while (running)
            {
                if (mpCost.Value == 24 || mpCost.Value == 33)
                {
                    while (!PlayerData.instance.atBench)
                    {
                        yield return new WaitForSeconds(2f);
                    }
                    mpCost.Value *= multiplier;
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(2f);
            }
        }

        public float GetDuration()
        {
            return 120;
        }

        public void Revert()
        {
            running = false;
            mpCost.Value = PlayerData.instance.GetBool("equippedCharm_33") ? 24 : 33;
        }

        public string GetName()
        {
            return "MP_Cost_Usable";
        }
        public string GetDisplayName()
        {
            return displayName;
        }
        public string GetDescription()
        {
            return "Comes with a cup, lid, and sippy straw. No additional refills.";
        }
        public string GetItemSpriteKey()
        {
            return "Charms.33";
        }
    }
}