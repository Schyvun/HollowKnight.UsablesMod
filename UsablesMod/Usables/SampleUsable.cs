namespace UsablesMod.Usables
{
    class SampleUsable : IUsable, IRevertable
    {
        private readonly System.Random random;

        public SampleUsable(int randomSeed) 
        {
            random = new System.Random(randomSeed);
        }

        public void Run() 
        {
            if (random.Next(2) == 0) { }
            else { }
        }

        public float GetDuration()
        {
            return 12f;
        }

        public void Revert() {}

        public string GetName()
        {
            return "Sample_Usable";
        }
        public string GetDisplayName()
        {
            return "Sample Usable";
        }
        public string GetDescription()
        {
            return "Shop description here.";
        }
        public string GetItemSpriteKey()
        {
            return "UI.Shop.Shitpost";
        }
    }
}
