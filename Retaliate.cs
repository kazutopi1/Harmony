using StardewValley;
using StardewValley.Tools;
using StardewModdingAPI;
using HarmonyLib;
using StardewValley.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace Reta
{
    public class Liate : Mod
    {
        private const double playerIFrames = 1200.0;

        private static double lastDamaged = -1.0;

        private static int damageTaken = 0;

        private const int Threshold = 7;

        public override void Entry(IModHelper helper)
        {
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
        private static Buff Retaliate()
        {
            Buff Retaliate = new Buff(
                id: "df.retaliate",
                displayName: "Retaliate",
                iconTexture: Game1.content.Load<Texture2D>("TileSheets/BuffsIcons"),
                iconSheetIndex: 11,
                duration: Buff.ENDLESS,
                effects: new BuffEffects()
                {
                    Attack = { 50 },
                    WeaponSpeedMultiplier = { 10 }
                }
            );
            return Retaliate;
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
                    Game1.player.applyBuff(Retaliate());
                    damageTaken = 0;
                }
            }
        }
        private static void Postfix2()
        {
            Game1.player.buffs.Remove("df.retaliate");
        }
    }
}
