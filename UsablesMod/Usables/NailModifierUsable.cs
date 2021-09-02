using System;

namespace UsablesMod.Usables
{
    class NailModifierUsable : IUsable, IRevertable
    {
        private readonly Random random;
        private int damageBuffAmount;
        private float nailScaleMultiplier;
        private string displayName;

        public NailModifierUsable(int randomSeed)
        {
            random = new Random(randomSeed);
            damageBuffAmount = 0;
            displayName = "Nail Modifier";
        }

        public void Run()
        {
            int damageBuffMultiplier;
            if (random.Next(1, 3) == 1)
            {
                damageBuffMultiplier = 1;
                displayName = "Mighty ";
            }
            else
            {
                damageBuffMultiplier = -1;
                displayName = "Broken ";
            }

            damageBuffAmount = random.Next(Math.Min(PlayerData.instance.nailDamage, 3), PlayerData.instance.nailDamage) * damageBuffMultiplier;
            if (damageBuffMultiplier < 0 && PlayerData.instance.nailDamage == 1) return;

            PlayerData.instance.nailDamage += damageBuffAmount;
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");

            if (random.Next(2) == 0)
            {
                nailScaleMultiplier = random.Next(15, 25) / 10f;
                displayName += "Longsword";
            }
            else
            {
                nailScaleMultiplier = random.Next(2, 6) / 10f;
                displayName += "Dagger";
            }
            On.NailSlash.StartSlash += ChangeNailScale;
        }

        private void ChangeNailScale(On.NailSlash.orig_StartSlash orig, NailSlash self)
        {
            orig(self);

            self.transform.localScale *= nailScaleMultiplier;
        }

        public float GetDuration()
        {
            return 90;
        }

        public void Revert()
        {
            On.NailSlash.StartSlash -= ChangeNailScale;

            PlayerData.instance.nailDamage -= damageBuffAmount;
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
        }

        public string GetName()
        {
            return "Nail_Modifier_Usable";
        }
        public string GetDisplayName()
        {
            return displayName;
        }
        public string GetDescription()
        {
            return "Feel like your nail is capable of more! Or perhaps you seek for a weaker one.";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.Upslash";
        }
    }
}
