using System;

namespace UsablesMod.Usables
{
    class ZoomUsable : IUsable, IRevertable
    {
        private static readonly string[] POSSIBLE_NAMES_IN = { "Enhanced Vision", 
            "I didn't like 4K anyway", "Look at the details on your skin!", "Look me in the eyes", 
            "20-20 Vision"};
        private static readonly string[] POSSIBLE_NAMES_OUT = { "Where's the Knight?", 
            "I can't see anything", "What's over there?", "I can see my house from here!", 
            "Smallow Knight Time!" };

        private readonly Random random;
        private readonly float multiplier;
        private readonly float duration;
        private string displayName;

        public ZoomUsable(int randomSeed) 
        {
            random = new Random(randomSeed);
            if (random.Next(2) == 1)
                multiplier = random.Next(6, 8) / 10f;
            else
                multiplier = random.Next(13, 20) / 10f;
            duration = random.Next(120, 240);

            displayName = "Zoomed Camera";
        }

        public void Run()
        {
            GameCameras.instance.tk2dCam.ZoomFactor *= multiplier;
            string[] possible_names = multiplier > 1 ? POSSIBLE_NAMES_IN : POSSIBLE_NAMES_OUT;
            displayName = possible_names[random.Next(possible_names.Length)];
        }

        public float GetDuration()
        {
            return duration;
        }

        public void Revert()
        {
            GameCameras.instance.tk2dCam.ZoomFactor /= multiplier;
        }

        public string GetName()
        {
            return "Zoom_Usable";
        }
        public string GetDisplayName()
        {
            return displayName;
        }
        public string GetDescription()
        {
            return "Enhance. Enhance. Still can't see anything, ENHANCE!";
        }
        public string GetItemSpriteKey()
        {
            return "ShopIcons.Focus";
        }
    }
}