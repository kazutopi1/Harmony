using StardewValley;
using StardewModdingAPI;
using HarmonyLib;

namespace EscapeFast
{
    public class JustRun : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.useTool)),
                postfix: new HarmonyMethod(typeof(JustRun), nameof(JustRun.Postfix))
            );
        }
        private static void Postfix(Farmer who)
        {
            int maxStamina = who.MaxStamina;

            Game1.player.stamina = maxStamina;
        }
    }
}
