using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Whatever
{
    public class ThisModIs : Mod
    {
        public static int Bank = 0;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += StashOrWithdraw;
        }
        private void StashOrWithdraw(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F1)
            {
                if (Game1.player.Money >= 1000)
                {
                    Game1.player.Money -= 1000;
                    Bank += 1000;
                    Game1.drawObjectDialogue($"Bank Balance: {Bank}");
                }
            }
            else if (e.Button == SButton.F2)
            {
                if (Bank >= 1000)
                {
                    Game1.player.Money += 1000;
                    Bank -= 1000;
                    Game1.drawObjectDialogue($"Bank Balance: {Bank}");
                }
                else if (Bank is 0)
                {
                    Game1.drawObjectDialogue("Insufficient Balance!");
                }
            }
        }
    }
}
