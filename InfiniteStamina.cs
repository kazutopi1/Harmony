using StardewValley;
using StardewModdingAPI;
using HarmonyLib;

namespace InfStam
{
    public class StamInf : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.useTool)),
                postfix: new HarmonyMethod(typeof(StamInf), nameof(StamInf.Postfix))
            );
        }
        private static void Postfix(Farmer who)
        {
            who.stamina = who.MaxStamina;
        }
    }
}
