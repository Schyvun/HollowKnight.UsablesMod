using System.Collections.Generic;
using UsablesMod.Usables;
using UnityEngine;

namespace UsablesMod
{
    internal class UsablesManager
    {
        private static readonly int MIN_USABLES = 15, MAX_USABLES = 25;
        private static readonly int MAX_SWAPPED_GEO_ROCKS = 20, MAX_SWAPPED_SOUL_REFILLS = 6;

        private readonly UsablesSlots usablesSlots;
        private UsablesFactory factory;

        private Dictionary<string, IUsable> usables;

        public UsablesManager() 
        {
            usablesSlots = new UsablesSlots();
            usablesSlots.Hook();
            CreateUsablesReqDefs();
        }

        internal void AddUsableItemsToSet(HashSet<string> items)
        {
            usables = new Dictionary<string, IUsable>();
            factory = new UsablesFactory();

            int amount = new System.Random(RandomizerMod.RandomizerMod.Instance.Settings.Seed).Next(MIN_USABLES, MAX_USABLES + 1);

            for (int i = 0; i < amount; i++)
            {
                IUsable usable = factory.GetRandomUsable(randomSeed: RandomizerMod.RandomizerMod.Instance.Settings.Seed + i);

                string usableItemName = NameFormatter.AddIdToName(usable.GetName(), i);

                SetUsableItemData(usableItemName, usable);

                usables.Add(usableItemName, usable);
                items.Add(usableItemName);
            }

            SwapJunkPlacementsForUsables(items, amount);
        }

        private void SwapJunkPlacementsForUsables(HashSet<string> items, int indexOffset)
        {
            if (!RandomizerMod.RandomizerMod.Instance.Settings.RandomizeRocks &&
                !RandomizerMod.RandomizerMod.Instance.Settings.RandomizeSoulTotems) return;

            int swappedGeoRocks = 0, swappedSoulRefills = 0;

            Dictionary<string, string> itemsToSwap = new Dictionary<string, string>();

            foreach (string item in items)
            {
                if (IsASmallGeoRock(item) && swappedGeoRocks < MAX_SWAPPED_GEO_ROCKS)
                {

                    IUsable usable = factory.GetRandomUsable(randomSeed: RandomizerMod.RandomizerMod.Instance.Settings.Seed + indexOffset);
                    string usableItemName = NameFormatter.AddIdToName(usable.GetName(), indexOffset);
                    indexOffset++;

                    SetUsableItemData(usableItemName, usable);

                    usables.Add(usableItemName, usable);
                    itemsToSwap.Add(item, usableItemName);
                    swappedGeoRocks++;
                }

                if (IsASoulTotem(item) && swappedSoulRefills < MAX_SWAPPED_SOUL_REFILLS)
                {

                    IUsable usable = factory.GetRandomUsable(randomSeed: RandomizerMod.RandomizerMod.Instance.Settings.Seed + indexOffset);
                    string usableItemName = NameFormatter.AddIdToName(usable.GetName(), indexOffset);
                    indexOffset++;

                    SetUsableItemData(usableItemName, usable);

                    usables.Add(usableItemName, usable);
                    itemsToSwap.Add(item, usableItemName);
                    swappedSoulRefills++;
                }

                if (swappedGeoRocks == MAX_SWAPPED_GEO_ROCKS && swappedSoulRefills == MAX_SWAPPED_SOUL_REFILLS) break;
            }

            foreach (KeyValuePair<string, string> swapPair in itemsToSwap)
            {
                items.Remove(swapPair.Key);
                items.Add(swapPair.Value);
            }
        }

        private bool IsASmallGeoRock(string item)
        {
            return item.StartsWith("Geo_Rock") && RandomizerMod.Randomization.LogicManager.GetItemDef(item).geo < 20;
        }

        private bool IsASoulTotem(string item)
        {
            return item.StartsWith("Soul_Totem");
        }

        internal void LoadMissingItems(RandomizerMod.SaveSettings settings)
        {
            usables = new Dictionary<string, IUsable>();
            foreach ((string item, string _) in settings.ItemPlacements)
            {
                if (UsablesFactory.TryCreateUsable(item, out IUsable usable))
                {
                    RegisterUsable(item, usable);
                }
            }
        }
        internal void RegisterUsable(string usableItemName, IUsable usable)
        {
            SetUsableItemData(usableItemName, usable);
            usables.Add(usableItemName, usable);
        }

        internal static void SetUsableItemData(string usableItemName, IUsable usable)
        {
            RandomizerMod.LanguageStringManager.SetString("UI", usableItemName, usable.GetDisplayName());

            RandomizerMod.Randomization.LogicManager.EditItemDef(usableItemName,
                RandomizerMod.Randomization.LogicManager.GetItemDef(
                    usableItemName.Substring(0, usableItemName.LastIndexOf("_"))));
        }

        private static void CreateUsablesReqDefs()
        {
            foreach (string usableName in UsablesFactory.USABLE_NAMES)
            {
                UsablesFactory.TryCreateUsable(NameFormatter.AddIdToName(usableName, 0), out IUsable usable);
                RandomizerMod.LanguageStringManager.SetString("UI", usableName, usable.GetDisplayName());

                string shopDescKey = usableName + "_SHOP_DESC";
                RandomizerMod.LanguageStringManager.SetString("UI", shopDescKey, usable.GetDescription());


                RandomizerMod.Randomization.ReqDef usableDef = new RandomizerMod.Randomization.ReqDef()
                {
                    action = RandomizerMod.GiveItemActions.GiveAction.None,
                    pool = "Usables",
                    type = RandomizerMod.Randomization.ItemType.Trinket,
                    nameKey = usableName,
                    shopDescKey = shopDescKey,
                    shopSpriteKey = usable.GetItemSpriteKey(),
                };
                if (usable is GeoMultiplierUsable)
                {
                    usableDef.geo = 1;
                }
                RandomizerMod.Randomization.LogicManager.EditItemDef(usableName, usableDef);
            }
        }

        internal void AddToSlots(string itemName)
        {
            IUsable usable = usables[itemName];
            usablesSlots.Add(itemName, usable);
        }

        internal bool IsAUsable(string item)
        {
            foreach (string usableName in UsablesFactory.USABLE_NAMES)
                if (item.StartsWith(usableName))
                    return true;
            return false;
        }

        ~UsablesManager()
        {
            usablesSlots.Destroy();
            usablesSlots.UnHook();
        }
    }
}
