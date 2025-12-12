using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastWhisper
{
    public class EscapeDeath : Mod
    {
        private static Texture2D BuffTextures;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += BuffTexture;

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.Update)),
                prefix: new HarmonyMethod(typeof(EscapeDeath), nameof(EscapeDeath.Prefix))
            );
        }
        private void BuffTexture(object sender, GameLaunchedEventArgs e)
        {
            BuffTextures = Game1.content.Load<Texture2D>("TileSheets/BuffsIcons");
        }
        private static Buff Defense()
        {
            Buff DefenseBuff = new Buff(
                id: "kt.defense",
                displayName: "Defense",
                duration: 7_000,
                iconTexture: BuffTextures,
                iconSheetIndex: 10,
                effects: new BuffEffects()
                {
                    Defense = { 10 }
                }
            );
            return DefenseBuff;
        }
        private static Buff Speed()
        {
            Buff Speed = new Buff(
                id: "kt.speed",
                displayName: "Speed",
                duration: 7_000,
                iconTexture: BuffTextures,
                iconSheetIndex: 9,
                effects: new BuffEffects()
                {
                    Speed = { 2 }
                }
            );
            return Speed;
        }
        public static bool Prefix(Farmer __instance)
        {
            if (__instance.health <= 0 && !Game1.killScreen && Game1.timeOfDay < 2600)
            {
                if (__instance.hasItemInInventoryNamed("LastWhisper"))
                {
                    __instance.health = 50;
                    __instance.applyBuff(Defense());
                    __instance.applyBuff(Speed());
                    __instance.removeFirstOfThisItemFromInventory("(O)KT_LastWhisper", 1);
                    Game1.playSound("healSound");
                    return false;
                }
            }
            return true;
        }
    }
}
