using StardewValley;
using StardewModdingAPI;
using HarmonyLib;

namespace PermaSpeed
{
    public class hsuwiiwhwb : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.getMovementSpeed)),
                postfix: new HarmonyMethod(typeof(hsuwiiwhwb), nameof(hsuwiiwhwb.Postfix))
            );
        }
        private static void Postfix(ref float __result)
        {
            __result += 1;
        }
    }
}
