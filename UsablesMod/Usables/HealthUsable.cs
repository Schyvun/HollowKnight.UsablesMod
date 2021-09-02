using UnityEngine;
using System.Collections;

namespace UsablesMod.Usables
{
    class HealthUsable : MonoBehaviour, IUsable, IRevertable
    {
        private readonly System.Random random;
        private bool running;
        private string displayName;

        public HealthUsable(int randomSeed)
        {
            random = new System.Random(randomSeed);
            displayName = "Health Supply";
        }

        public void Run()
        {
            running = true;

            int amount = random.Next(1, 3);
            if (amount == 1)
            {
                GameManager.instance.StartCoroutine(Regeneration());
                displayName = "Regeneration";
            }
            else
            {
                GameManager.instance.StartCoroutine(Poison());
                displayName = "Poison";
            }
        }

        public float GetDuration()
        {
            return 30f;
        }

        public void Revert()
        {
            running = false;
        }
        
        private IEnumerator Regeneration()
        {
            while (running)
            {
                HeroController.instance.AddHealth(1);
                yield return new WaitForSeconds(random.Next(2, 12));
            }
        }

        private IEnumerator Poison()
        {
            while (running)
            {
                GlobalEnums.CollisionSide ranHit = (GlobalEnums.CollisionSide)random.Next(5);
                HeroController.instance.TakeDamage(HeroController.instance.gameObject, ranHit, 1, 0);
                yield return new WaitForSeconds(random.Next(3, 12));
            }
        }

        public string GetName()
        {
            return "Health_Usable";
        }

        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetDescription()
        {
            return "Health supply has arrived! Wait, are you the supplier?";
        }

        public string GetItemSpriteKey()
        {
            return "ShopIcons.MaskShard";
        }
    }
}

