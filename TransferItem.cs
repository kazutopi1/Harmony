using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System.Collections.Generic;
using StardewValley.Objects;

namespace GlobalChest
{
    public class GC : Mod
    {
        public Chest chest;
        public ChestItemData chestItemData;
        private string path = "data/ChestItemData.json";

        public override void Entry(IModHelper helper)
        {
            if (Constants.TargetPlatform != GamePlatform.Android) { return; }

            helper.Events.Input.ButtonPressed += StoreOrTake;
            helper.Events.GameLoop.GameLaunched += ChestInstance;
        }
        private void ChestInstance(object sender, GameLaunchedEventArgs e)
        {
            chestItemData = Helper.Data.ReadJsonFile<ChestItemData>(path) ?? new ChestItemData();
            chest = new Chest();
            LoadData();
        }
        private void StoreOrTake(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F1 && Context.IsPlayerFree && Context.IsWorldReady)
            {
                OpenChest();
            }
        }
        private void OpenChest()
        {
            LoadData();
            Game1.activeClickableMenu = new ItemGrabMenu(
                inventory: chest.Items,
                reverseGrab: false,
                showReceivingMenu: true,
                highlightFunction: null,
                behaviorOnItemSelectFunction: chest.grabItemFromInventory,
                message: null,
                behaviorOnItemGrab: chest.grabItemFromChest,
                snapToBottom: false,
                canBeExitedWithKey: true,
                playRightClickSound: true,
                allowRightClick: true,
                showOrganizeButton: true,
                storageCapacity: 36,
                numRows: 3,
                source: 1,
                sourceItem: chest,
                behaviorOnTapClose: (item, farmer) =>
                {
                    StoreData();
                }
            );
        }
        private void StoreData()
        {
            if (!Context.IsWorldReady || Game1.player == null) { return; }

            ChestList chestList = new ChestList();

            foreach (Item item in chest.Items)
            {
                if (item is null) { continue; }

                ChestItemData itemData = new ChestItemData
                {
                    QualifiedId = item.QualifiedItemId,
                    Stack = item.Stack,
                    Quality = item.Quality
                };
                chestList.Items.Add(itemData);
            }
            Helper.Data.WriteJsonFile<ChestList>(path, chestList);
        }
        private void LoadData()
        {
            ChestList savedData = Helper.Data.ReadJsonFile<ChestList>(path);

            if (savedData == null || savedData.Items == null) return;

            chest.Items.Clear();

            foreach (var itemData in savedData.Items)
            {
                Item item = ItemRegistry.Create(itemData.QualifiedId, itemData.Stack, itemData.Quality);

                if (item != null)
                {
                    chest.Items.Add(item);
                }
            }
        }
    }
    public class ChestList
    {
        public List<ChestItemData> Items { get; set; } = new List<ChestItemData>();
    }
    public class ChestItemData
    {
        public string QualifiedId { get; set; }
        public int Stack { get; set; }
        public int Quality { get; set; }
    }
}
