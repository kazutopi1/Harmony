using StardewValley;
using StardewValley.Mobile;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;

namespace blahblah
{
    public class idcifthisdoebntwork : Mod
    {
        private const int tickThreshhold = 2;

        private static int tickCounter = 0;

        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(VirtualJoypad), nameof(VirtualJoypad.CheckForTapJoystickAndButtons)),
                postfix: new HarmonyMethod(typeof(idcifthisdoebntwork), nameof(idcifthisdoebntwork.Postfix))
            );
        }
        private static void Postfix(VirtualJoypad __instance)
        {
            if (!Context.IsWorldReady || !Context.IsPlayerFree)
            {
                tickCounter = 0;
                return;
            }
            if (__instance.buttonBHeld)
            {
                tickCounter++;

                if (tickCounter >= tickThreshhold)
                {
                    Game1.currentLocation.tapToMove.mobileKeyStates.actionButtonPressed = true;

                    tickCounter = 0;
                }
            }
            else
            {
                tickCounter = 0;
            }
        }
    }
}
