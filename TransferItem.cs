using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Transfer
{
    public class IT : Mod
    {
        ItemStorage i;

        public override void Entry(IModHelper helper)
        {
            i = new ItemStorage();

            helper.Events.Input.ButtonPressed += StoreOrTake;
        }
        private void StoreOrTake(object sender, ButtonPressedEventArgs e)
        {
            i = Helper.Data.ReadJsonFile<ItemStorage>("data/StoredItem.json") ?? new ItemStorage();

            var f = Game1.player;

            if (e.Button == SButton.F1 && Context.IsWorldReady && Context.IsPlayerFree)
            {
                if (f.CurrentItem != null)
                {
                    i.item = f.CurrentItem.QualifiedItemId;
                    Helper.Data.WriteJsonFile("data/StoredItem.json", i);
                    f.removeFirstOfThisItemFromInventory(f.CurrentItem.QualifiedItemId, 1);
                    Game1.drawObjectDialogue($"{i.item} stored.");
                }
            }
            else if (e.Button == SButton.F2 && Context.IsWorldReady && Context.IsPlayerFree)
            {
                if (i.item == null)
                {
                    Game1.drawObjectDialogue("No item stored!");
                    return;
                }
                if (f.isInventoryFull())
                {
                    Game1.drawObjectDialogue("Inventory is full!");
                    return;
                }
                else
                {
                    var it = ItemRegistry.Create(i.item);
                    f.addItemToInventory(it);
                    i.item = null;
                    Helper.Data.WriteJsonFile("data/StoredItem.json", i);
                }
            }
            else if (e.Button == SButton.F3 && Context.IsWorldReady && Context.IsPlayerFree)
            {
                Game1.drawObjectDialogue($"Stored item: {i.item}");
            }
        }
    }
    public class ItemStorage
    {
        public string item { get; set; }
    }
}
