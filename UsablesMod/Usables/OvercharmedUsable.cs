using System.Threading;

namespace UsablesMod.Usables
{
    class OvercharmedUsable : IUsable, IRevertable
    {
        private static object s_lock = new object();
        private static OvercharmedUsable s_activeInstance = null;
        private static float unifiedDuration = 0;

        public OvercharmedUsable() {}

        public void Run()
        {
            Monitor.Enter(s_lock);
            try
            {
                unifiedDuration += 90f;
            }
            finally
            {
                Monitor.Exit(s_lock);
            }

            if (IsActiveInstance())
            {
                SetOvercharm();
            }
        }

        private static void SetOvercharm()
        {
            PlayerData.instance.overcharmed = true;
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
                    GameManager.instance.RefreshOvercharm();
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

        public string GetName()
        {
            return "Overcharmed_Usable";
        }
        public string GetDisplayName()
        {
            return "Overcharmed Time";
        }
        public string GetDescription()
        {
            return "Cursing your notches has proven to improve Flukemarm RNG.";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.CharmNotch";
        }
    }
}
