using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Whatever
{
    public class ThisModIs : Mod
    {
        private BankBalance bal;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += StashOrWithdraw;
        }
        private void StashOrWithdraw(object sender, ButtonPressedEventArgs e)
        {
            bal = Helper.Data.ReadJsonFile<BankBalance>("Bank/balance.json") ?? new BankBalance();

            if (e.Button == SButton.F1 && Context.IsWorldReady && Context.IsPlayerFree)
            {
                if (Game1.player.Money >= 1000)
                {
                    Game1.player.Money -= 1000;
                    bal.balance += 1000;
                    Helper.Data.WriteJsonFile("Bank/balance.json", bal);
                    Game1.drawObjectDialogue($"Cash Deposited.^^Bank Balance: {bal.balance}g");
                }
                else if (Game1.player.Money < 1000)
                {
                    Game1.drawObjectDialogue("Minimum cash deposit is 1000g");
                }
            }
            else if (e.Button == SButton.F2 && Context.IsWorldReady && Context.IsPlayerFree)
            {
                if (bal.balance >= 1000)
                {
                    Game1.player.Money += 1000;
                    bal.balance -= 1000;
                    Helper.Data.WriteJsonFile("Bank/balance.json", bal);
                    Game1.drawObjectDialogue($"Cash Withdrawn.^^Bank Balance: {bal.balance}g");
                }
                else if (bal.balance == 0)
                {
                    Game1.drawObjectDialogue("Insufficient Balance!");
                }
            }
            else if (e.Button == SButton.F3 && Context.IsWorldReady && Context.IsPlayerFree)
            {
                Game1.drawObjectDialogue($"Current Bank balance: {bal.balance}g");
            }
        }
    }
    public class BankBalance
    {
        public int balance { get; set; } = 0;
    }
}
