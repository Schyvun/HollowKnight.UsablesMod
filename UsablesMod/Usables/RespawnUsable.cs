namespace UsablesMod.Usables
{
    class RespawnUsable : IUsable
    {
        public RespawnUsable() {}

        public void Run()
        {
            GameManager.instance.HazardRespawn();
        }

        public string GetName()
        {
            return "Hazard_Respawn_Usable";
        }
        public string GetDisplayName()
        {
            return "Hazard Respawn";
        }
        public string GetDescription()
        {
            return "How did I get here?";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.CityKey";
        }
    }
}