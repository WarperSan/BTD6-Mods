using MelonLoader;

[assembly: MelonInfo(typeof(balanced_random_rounds.balanced_random_rounds.Main), "Balanced Random Rounds", "1.2.0", "WarperSan")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace balanced_random_rounds
{
    using Assets.Main.Scenes;
    using Assets.Scripts.Models.Bloons.Behaviors;
    using Assets.Scripts.Models.Rounds;
    using Assets.Scripts.Unity;
    using BTD_Mod_Helper;
    using BTD_Mod_Helper.Api.ModOptions;
    using Harmony;
    using System;
    using System.Collections.Generic;
    using System.IO;

    namespace balanced_random_rounds
    {
        // Token: 0x02000003 RID: 3
        public class Main : BloonsTD6Mod
        {
            // Token: 0x06000003 RID: 3 RVA: 0x0000205C File Offset: 0x0000025C
            public override void OnApplicationStart()
            {
                base.OnApplicationStart();
                Console.WriteLine("Balanced Random Rounds Loaded.");
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
                "TestBloon"
            };

            public static List<string> allBloonsReference = new List<string>();
            public static string[] bossSpawner = new string[]
            {
                "Bad"
            };

            public static string[] allBosses = new string[]
            {
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
                "LychElite1",
                "LychElite2",
                "LychElite3",
                "LychElite4",
                "LychElite5"
            };

            private static float cashDecreaseMultiplier = 0.1f;
            private static string path = "Mods/random_rounds/";
            private static string randomizationPath = path + "current_randomization.txt";

            [HarmonyPatch(typeof(TitleScreen), "Start")]
            public class GameModel_Patch
            {
                [HarmonyPostfix]
                public static void Postfix()
                {
                    foreach (var bloons in Game.instance.model.bloons)
                    {
                        if (bloons.tags.Contains("Boss"))
                        {
                            bloons.leakDamage = bloons.maxHealth;
                        }

                        allBloonsReference.Add(bloons.name);
                        bloons.behaviors[1].Cast<DistributeCashModel>().cash = bloons.behaviors[1].Cast<DistributeCashModel>().cash * cashDecreaseMultiplier;
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string content = "";

                    if (!File.Exists(randomizationPath))
                    {
                        File.Create(randomizationPath);

                        randomizeRounds(content);
                    }
                    else
                    {
                        string[] lines = File.ReadAllLines(randomizationPath);
                        int roundIndex = 0;

                        Console.WriteLine("Starting Reading File");
                        RoundModel newRound = Game.instance.model.roundSets[1].rounds[roundIndex].Clone().Cast<RoundModel>();

                        int groupIndex = 0;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i];

                            if (!line.Contains("Round"))
                            {
                                if (line == "")
                                {
                                    groupIndex = 0;

                                    Game.instance.model.roundSets[1].rounds[roundIndex] = newRound;

                                    roundIndex++;

                                    if (Game.instance.model.roundSets[1].rounds.Length > roundIndex)
                                        newRound = Game.instance.model.roundSets[1].rounds[roundIndex];
                                    else
                                        i = lines.Length;
                                }
                                else
                                {
                                    int indexOfSpace = line.IndexOf(" ");
                                    string nameOfBloon = line.Substring(0, indexOfSpace);

                                    Int32.TryParse(line.Substring(indexOfSpace + 2, line.Length - indexOfSpace - 2), out int bloonCount);

                                    if (allBloonsReference.FindIndex(item => nameOfBloon == item) == -1)
                                    {
                                        Console.WriteLine(nameOfBloon);
                                        i = lines.Length;
                                        Console.WriteLine("Error at line " + (i + 1));
                                        return;
                                    }
                                    else
                                    {
                                        BloonGroupModel bloonNew = newRound.groups[groupIndex];

                                        bloonNew.bloon = nameOfBloon;
                                        bloonNew.count = bloonCount;

                                        newRound.groups.Add(bloonNew);

                                        groupIndex++;
                                    }
                                }
                            }
                        }

                        Console.WriteLine("Reading File Successfully");
                    }
                }

                private static void randomizeRounds(string content)
                {
                    Console.WriteLine("Randomizing Starting");
                    RoundSetModel roundSet = Game.instance.model.roundSets[1];
                    for (int i = 0; i < roundSet.rounds.Count; i++)
                    {
                        content = content + "Round " + (i + 1) + "\n";

                        RoundModel newRound = roundSet.rounds[i];

                        foreach (var bloon in newRound.groups)
                        {
                            string bloonName = bloon.bloon;
                            BloonGroupModel bloonNew = bloon;
                            bloonNew.bloon = bloonName.Replace("Camo", "").Replace("Fortified", "").Replace("Regrow", "");

                            bloonNew.bloon = randomizedBloon(bloonNew.bloon, i, bloonNew);

                            newRound.groups.Add(bloonNew);

                            content = content + bloonNew.bloon + " x" + bloonNew.count + "\n";
                        }

                        roundSet.rounds[i] = newRound;
                        content = content + "\n";
                    }

                    File.WriteAllText(randomizationPath, content);

                    Console.WriteLine("Randomizing Ended");
                }

                public static string randomizedBloon(string initalBloon, int currentRoundIndex, BloonGroupModel bloon)
                {
                    float randomNumber = UnityEngine.Random.RandomRange(0f, 1f);

                    // Do nothing 0.1%
                    if (randomNumber > 0.99f)
                        return initalBloon;

                    // Upgrade 20%
                    if (randomNumber > 0.8f)
                    {
                        int index = Array.FindIndex(allBloons, item => initalBloon == item);

                        if (Array.FindIndex(bossSpawner, item => initalBloon == item) != -1)
                        {
                            // Test Bloon 0.01%
                            if (randomNumber > 0.9999f)
                                index = allBloons.Length - 1;
                            else
                                index = UnityEngine.Random.RandomRange(0, allBosses.Length - 1);
                        }
                        else
                            index++;

                        initalBloon = allBloons[index];
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
