using StardewValley;
using StardewValley.Mobile;
using StardewModdingAPI;
using HarmonyLib;
using System;

namespace BlahBlah
{
    public class Shop : Mod
    {
        private static Response[] shops;

        private static StardewValley.GameLocation.afterQuestionBehavior optionLogic;

        private static bool wasBTapped = false;

        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(VirtualJoypad), nameof(VirtualJoypad.ButtonBPressed)),
                postfix: new HarmonyMethod(typeof(Shop), nameof(Shop.Postfix))
            );
        }
        private static void Postfix(ref bool __result)
        {
            if (Context.IsPlayerFree && !wasBTapped && __result)
            {
                if (Game1.player.CurrentItem is StardewValley.Item item)
                {
                    if (item.QualifiedItemId.Equals("(O)388"))
                    {
                        shops = new Response[]
                        {
                            new Response("openCarpenter", "Robin's Shop"),
                            new Response("openSeedShop", "Pierre's General Store"),
                            new Response("openWeaponShop", "Adventurer's Guild Shop"),
                            new Response("nextPage", "Next Page"),
                            new Response("doNothing", "Close")
                        };
                        optionLogic = (Farmer who, string whichAnswer) =>
                        {
                            if (whichAnswer == "openCarpenter")
                            {
                                Utility.TryOpenShopMenu(
                                    shopId: "Carpenter",
                                    ownerName: null
                                );
                            }
                            else if (whichAnswer == "openSeedShop")
                            {
                                Utility.TryOpenShopMenu(
                                    shopId: "SeedShop",
                                    ownerName: null
                                );
                            }
                            else if (whichAnswer == "openWeaponShop")
                            {
                                Utility.TryOpenShopMenu(
                                    shopId: "AdventureShop",
                                    ownerName: null
                                );
                            }
                            else if (whichAnswer == "nextPage")
                            {
                                nextPage();
                            }
                        };

                        Game1.currentLocation.createQuestionDialogue(
                            question: "Open which shop?",
                            answerChoices: shops,
                            afterDialogueBehavior: optionLogic,
                            speaker: null
                        );
                    }
                }
            }
            wasBTapped = __result;
        }
        private static void nextPage()
        {
            Response[] page2 = new Response[]
            {
            new Response("openBlacksmith", "Blacksmith"),
            new Response("openFishShop", "Willy's Shop"),
            new Response("openSaloon", "Saloon"),
            new Response("prevPage", "Previous Page"),
            new Response("doNothing2", "Close")
            };
            StardewValley.GameLocation.afterQuestionBehavior nextPage = (Farmer who, string nextPageAnswers) =>
            {
                if (nextPageAnswers == "openBlacksmith")
                {
                    Utility.TryOpenShopMenu(
                        shopId: "Blacksmith",
                        ownerName: null
                    );
                }
                else if (nextPageAnswers == "openFishShop")
                {
                    Utility.TryOpenShopMenu(
                        shopId: "FishShop",
                        ownerName: null
                    );
                }
                else if (nextPageAnswers == "openSaloon")
                {
                    Utility.TryOpenShopMenu(
                        shopId: "Saloon",
                        ownerName: null
                    );
                }
                else if (nextPageAnswers == "prevPage")
                {
                    Game1.currentLocation.createQuestionDialogue(
                        question: "Open which shop?",
                        answerChoices: shops,
                        afterDialogueBehavior: optionLogic,
                        speaker: null
                    );
                }
            };
            Game1.currentLocation.createQuestionDialogue(
                question: "Open which shop?",
                answerChoices: page2,
                afterDialogueBehavior: nextPage,
                speaker: null
            );
        }
    }
}
