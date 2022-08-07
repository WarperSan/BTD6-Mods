using MelonLoader;

[assembly: MelonInfo(typeof(unbalanced_random_rounds.unbalanced_random_rounds.Main), "Unbalanced Random Rounds", "1.1.0", "WarperSan")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace unbalanced_random_rounds
{
    using Assets.Main.Scenes;
    using Assets.Scripts.Models.Rounds;
    using Assets.Scripts.Unity;
    using Assets.Scripts.Unity.UI_New.InGame;
    using BTD_Mod_Helper;
    using Harmony;
    using System;
    using System.Collections.Generic;

    namespace unbalanced_random_rounds
    {
        public class Main : BloonsTD6Mod
        {
            public override void OnApplicationStart()
            {
                base.OnApplicationStart();
                Console.WriteLine("Unbalanced Random Rounds Loaded.");
            }

            public static string[] allBloons = new string[]
            {
                "Red",
                "Blue",
                "Green",
                "Yellow",
                "Pink",
                "Black",
                "White",
                "Purple",
                "Lead",
                "Zebra",
                "Rainbow",
                "Ceramic",
                "Moab",
                "Bfb",
                "Zomg",
                "Ddt",
                "Bad",
                "Vortex1",
                "Vortex2",
                "Vortex3",
                "Vortex4",
                "Vortex5",
                "VortexElite1",
                "VortexElite2",
                "VortexElite3",
                "VortexElite4",
                "VortexElite5",
                "Bloonarius1",
                "Bloonarius2",
                "Bloonarius3",
                "Bloonarius4",
                "Bloonarius5",
                "BloonariusElite1",
                "BloonariusElite2",
                "BloonariusElite3",
                "BloonariusElite4",
                "BloonariusElite5",
                "Lych1",
                "Lych2",
                "Lych3",
                "Lych4",
                "Lych5",
                "MiniLych1",
                "MiniLych2",
                "MiniLych3",
                "MiniLych4",
                "MiniLych5",
                "LychElite1",
                "LychElite2",
                "LychElite3",
                "LychElite4",
                "LychElite5",
                "MiniLychElite1",
                "MiniLychElite2",
                "MiniLychElite3",
                "MiniLychElite4",
                "MiniLychElite5",
                "TestBloon"
            };

            public static List<string> allBloonsReference = new List<string>();

            [HarmonyPatch(typeof(TitleScreen), "Start")]
            public class GameModel_Patch
            {
                [HarmonyPostfix]
                public static void Postfix()
                {
                    foreach (var bloons in Game.instance.model.bloons)
                    {
                        allBloonsReference.Add(bloons.name.Replace(" ", ""));
                    }

                    Console.WriteLine("Randomizing Starting");
                    RoundSetModel roundSet = Game.instance.model.roundSets[1];
                    for (int i = 0; i < roundSet.rounds.Count; i++)
                    {
                        RoundModel newRound = roundSet.rounds[i];

                        foreach (var bloon in newRound.groups)
                        {
                            string bloonName = bloon.bloon;
                            BloonGroupModel bloonNew = bloon;
                            bloonNew.bloon = bloonName.Replace("Camo", "").Replace("Fortified", "").Replace("Regrow", "");

                            bloonNew.bloon = randomizedBloon(bloonNew.bloon);

                            newRound.groups.Add(bloonNew);
                        }

                        roundSet.rounds[i] = newRound;
                    }

                    Console.WriteLine("Randomizing Ended");
                }

                public static string randomizedBloon(string initalBloon)
                {
                    float randomNumber = UnityEngine.Random.RandomRange(0f, 1f);

                    // Do nothing 0.1%
                    if (randomNumber > 0.99f)
                        return initalBloon;

                    // Upgrade everytime
                    if (randomNumber > -1f)
                    {
                        int index = Array.FindIndex(allBloons, item => initalBloon == item);

                        if (index >= allBloons.Length)
                        {
                            index = 0;
                        }

                        initalBloon = allBloons[UnityEngine.Random.RandomRange(0, allBloons.Length - 1)];
                    }

                    string[] allStates = new string[] { "Regrow", "Fortified", "Camo" };

                    foreach (var state in allStates)
                    {
                        randomNumber = UnityEngine.Random.Range(0f, 1f);
                        if (randomNumber > 0.7f)
                        {
                            if (allBloonsReference.Contains(initalBloon + state))
                                initalBloon += state;
                        }
                    }
                    return initalBloon;
                }
            }
        }
    }

}
