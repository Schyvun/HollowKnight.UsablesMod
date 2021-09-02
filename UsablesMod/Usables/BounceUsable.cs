using UnityEngine;
using System.Collections;
using System.Threading;

namespace UsablesMod.Usables
{
    class BounceUsable : IUsable, IRevertable
    {
        private static object s_lock = new object();
        private static BounceUsable s_activeInstance = null;
        private static float unifiedDuration = 0;

        private readonly System.Random random;
        private bool bouncing = false;

        public BounceUsable(int randomSeed)
        {
            random = new System.Random(randomSeed);
        }

        public void Run()
        {
            Monitor.Enter(s_lock);
            try
            {
                unifiedDuration += 45f;
            }
            finally
            {
                Monitor.Exit(s_lock);
            }

            if (IsActiveInstance())
            {
                bouncing = true;
                GameManager.instance.StartCoroutine(Bouncing());
            }
        }

        public float GetDuration()
        {
            // Case for 2+ instances of usable running before 1st was reverted
            if (!IsActiveInstance()) return 0;

            return unifiedDuration;
        }

        public void Revert()
        {
            if (IsActiveInstance())
            {
                Monitor.Enter(s_lock);
                try
                {
                    bouncing = false;
                    unifiedDuration = 0;
                    s_activeInstance = null;
                }
                finally
                {
                    Monitor.Exit(s_lock);
                }
            }
        }

        private bool IsActiveInstance()
        {
            bool res;
            Monitor.Enter(s_lock);

            if (s_activeInstance == null)
            {
                s_activeInstance = this;
                res = true;
            }
            else
            {
                res = s_activeInstance == this;
            }

            Monitor.Exit(s_lock);
            return res;
        }

        private IEnumerator Bouncing()
        {
            while (bouncing)
            {
                if (HeroController.instance.CheckTouchingGround())
                {
                    HeroController.instance.ShroomBounce();
                }
                yield return new WaitForSeconds(random.Next(3));
            }
            yield return null;
        }

        public string GetName()
        {
            return "Bounce_Usable";
        }
        public string GetDisplayName()
        {
            return "Spring Shoes";
        }
        public string GetDescription()
        {
            return "I swear, your jump key is broken.";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.Wings";
        }
    }
}

