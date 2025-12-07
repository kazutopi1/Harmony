using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley.Tools;
using StardewValley.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace PressTheAttack
{
    public class PTA : Mod
    {
        private static Buff PierceBuff;

        private static int useToolCount = 0;

        private const int Threshold = 3;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += Pierce;

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(MeleeWeapon), nameof(MeleeWeapon.doSwipe)),
                prefix: new HarmonyMethod(typeof(PTA), nameof(PTA.Prefix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(MeleeWeapon), nameof(MeleeWeapon.doSwipe)),
                postfix: new HarmonyMethod(typeof(PTA), nameof(PTA.Postfix))
            );
        }
        private void Pierce(object sender, GameLaunchedEventArgs e)
        {
            PierceBuff = new Buff(
                id: "df.pierce",
                displayName: "Pierce",
                iconTexture: Game1.content.Load<Texture2D>("TileSheets/BuffsIcons"),
                iconSheetIndex: 11,
                duration: Buff.ENDLESS,
                effects: new BuffEffects()
                {
                    CriticalPowerMultiplier = { 2 },
                    CriticalChanceMultiplier = { 1000 },
                    WeaponSpeedMultiplier = { 3 },
                }
            );
        }
        private static void Prefix()
        {
            var player = Game1.player;

            if (player.professions.Contains(Farmer.desperado))
            {
                useToolCount += 1;

                if (useToolCount == Threshold
                && !player.hasBuff("df.pierce"))
                {
                    player.applyBuff(PierceBuff);
                    useToolCount = 0;
                }
            }
        }
        private static void Postfix()
        {
            DelayedAction.functionAfterDelay(() =>
            {
                var f = Game1.player;

                if (f.hasBuff("df.pierce"))
                {
                    f.buffs.Remove("df.pierce");
                }
            }, 500);
        }
    }
}
