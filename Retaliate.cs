using StardewValley;
using StardewValley.Tools;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace Reta
{
    public class Liate : Mod
    {
        private static Buff Retaliate;

        private const double playerIFrames = 1200.0;

        private static double lastDamaged = -1.0;

        private static int damageTaken = 0;

        private const int Threshold = 7;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += SetupBuff;

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.takeDamage)),
                postfix: new HarmonyMethod(typeof(Liate), nameof(Liate.Postfix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(MeleeWeapon), nameof(MeleeWeapon.doSwipe)),
                postfix: new HarmonyMethod(typeof(Liate), nameof(Liate.Postfix2))
            );
        }
        private void SetupBuff(object sender, GameLaunchedEventArgs e)
        {
            Retaliate = new Buff(
                id: "df.retaliate",
                displayName: "Retaliate",
                iconTexture: Game1.content.Load<Texture2D>("TileSheets/BuffsIcons"),
                iconSheetIndex: 11,
                duration: Buff.ENDLESS,
                effects: new BuffEffects()
                {
                    Attack = { 50 },
                    CriticalChanceMultiplier = { 100000 }
                }
            );
        }
        private static void Postfix()
        {
            double timeNow = Game1.currentGameTime.TotalGameTime.TotalMilliseconds;

            if (timeNow - lastDamaged >= playerIFrames)
            {
                lastDamaged = timeNow;

                damageTaken += 1;

                if (damageTaken >= Threshold
                    && !Game1.player.hasBuff("df.retaliate"))
                {
                    Game1.player.applyBuff(Retaliate);
                    damageTaken = 0;
                }
            }
        }
        private static void Postfix2()
        {
            DelayedAction.functionAfterDelay(() =>
            {
                var f = Game1.player;

                if (f.hasBuff("df.retaliate"))
                {
                    f.buffs.Remove("df.retaliate");
                }
            }, 500);
        }
    }
}
