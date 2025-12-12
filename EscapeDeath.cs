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
        private static Item ReaperScroll;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += BuffTexture;

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.Update)),
                prefix: new HarmonyMethod(typeof(EscapeDeath), nameof(EscapeDeath.Prefix))
            );
            System.Type[] prefix2Params = new System.Type[]
            {
                typeof(Item),
                typeof(int),
                typeof(bool)
            };
            harmony.Patch(
                original: AccessTools.Method(
                    typeof(Farmer),
                    nameof(Farmer.holdUpItemThenMessage),
                    prefix2Params
                ),
                prefix: new HarmonyMethod(typeof(EscapeDeath), nameof(EscapeDeath.Prefix2))
            );
        }
        private void BuffTexture(object sender, GameLaunchedEventArgs e)
        {
            BuffTextures = Game1.content.Load<Texture2D>("TileSheets/BuffsIcons");
            ReaperScroll = ItemRegistry.Create("(O)KT_ReaperScroll");
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
                if (__instance.hasItemInInventoryNamed("ReaperScroll"))
                {
                    __instance.health = 50;
                    __instance.applyBuff(Defense());
                    __instance.applyBuff(Speed());
                    __instance.holdUpItemThenMessage(ReaperScroll, false);
                    __instance.removeFirstOfThisItemFromInventory("(O)KT_ReaperScroll", 1);
                    Game1.playSound("healSound");
                    return false;
                }
            }
            return true;
        }
        public static bool Prefix2(Farmer __instance)
        {
            if (__instance.hasItemInInventoryNamed("ReaperScroll"))
            {
                __instance.freezePause = 2000;
            }
            return true;
        }
    }
}
