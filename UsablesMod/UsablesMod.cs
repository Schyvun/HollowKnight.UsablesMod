using Modding;

namespace UsablesMod
{
    public class UsablesMod : Mod, ITogglableMod
    {
        public static UsablesMod Instance { get; private set; }

        internal UsablesManager usablesManager;

        public override void Initialize()
        {
            if (Instance != null)
            {
                LogWarn("Initialized twice... Stop that.");
                return;
            }

            Instance = this;
            usablesManager = new UsablesManager();

            RandomizerMod.Randomization.ItemManager.AddItemsToRandomizedItemsSet += usablesManager.AddUsableItemsToSet;
            RandomizerMod.GiveItemActions.ExternItemHandlers.Add(TriggerUsable);
            RandomizerMod.SaveSettings.PreAfterDeserialize += usablesManager.LoadMissingItems;
        }

        private bool TriggerUsable(RandomizerMod.GiveItemActions.GiveAction action,
			string item, string location, int geo)
        {
            if (!usablesManager.IsAUsable(item)) return false;

            usablesManager.AddToSlots(item);

            RandomizerMod.RandoLogger.LogItemToTracker(item, location);
            RandomizerMod.RandomizerMod.Instance.Settings.MarkItemFound(item);
            RandomizerMod.RandomizerMod.Instance.Settings.MarkLocationFound(location);
            RandomizerMod.RandoLogger.UpdateHelperLog();
            
            return true;
        }

        public override string GetVersion()
        {
            string ver = "0.0.3";
            return ver;
        }

        public void Unload()
        {
			Instance = null;
            RandomizerMod.Randomization.ItemManager.AddItemsToRandomizedItemsSet -= usablesManager.AddUsableItemsToSet;
            RandomizerMod.GiveItemActions.ExternItemHandlers.Remove(TriggerUsable);
            RandomizerMod.SaveSettings.PreAfterDeserialize -= usablesManager.LoadMissingItems;
            usablesManager = null;
        }
    }
}
