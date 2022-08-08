using MelonLoader;

[assembly: MelonInfo(typeof(unbalanced_random_rounds.unbalanced_random_rounds.Main), "Unbalanced Random Rounds", "1.3.0", "WarperSan")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace unbalanced_random_rounds
{
    using Assets.Main.Scenes;
    using Assets.Scripts.Models.Bloons.Behaviors;
    using Assets.Scripts.Models.Rounds;
    using Assets.Scripts.Unity;
    using Assets.Scripts.Unity.UI_New.InGame;
    using BTD_Mod_Helper;
    using BTD_Mod_Helper.Api.ModOptions;
    using Harmony;
    using System;
    using System.Collections.Generic;
    using System.IO;

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

            static Main()
            {
                ModSettingDouble modSettingInt1 = new ModSettingDouble(0.1f);
                modSettingInt1.displayName = "Cash Decrease Multiplier";
                modSettingInt1.minValue = 0;
                modSettingInt1.maxValue = 1000;
                Main.cashDecreaseMultiplier = modSettingInt1;
            }

            private static string path = "Mods/random_rounds/";
            private static string randomizationPath = path + "current_randomization.txt";
            private static readonly ModSettingDouble cashDecreaseMultiplier = 0.1f;

            [HarmonyPatch(typeof(TitleScreen), "Start")]
            public class GameModel_Patch
            {
                [HarmonyPostfix]
                public static void Postfix()
                {

                    foreach (var bloons in Game.instance.model.bloons)
                    {
                        if(bloons.tags.Contains("Boss"))
                        {
                            bloons.leakDamage = bloons.maxHealth;
                        }

                        allBloonsReference.Add(bloons.name);
                    }

                    foreach (var bloon in Game.instance.model.bloons)
                    {
                        bloon.behaviors[1].Cast<DistributeCashModel>().cash = (float)(bloon.behaviors[1].Cast<DistributeCashModel>().cash * cashDecreaseMultiplier);
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
                        RoundModel newRound = Game.instance.model.roundSets[1].rounds[roundIndex];

                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i];

                            if (!line.Contains("Round"))
                            {
                                if (line == "")
                                {
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
                                        BloonGroupModel bloonNew = newRound.groups[0];

                                        bloonNew.bloon = nameOfBloon;
                                        bloonNew.count = bloonCount;

                                        newRound.groups.Add(bloonNew);
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
                    int minRoundForBoss = 40;
                    int minRoundForMoabClass = 20;

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

                        int badIndex = Array.FindIndex(allBloons, item => "Bad" == item);
                        int moabIndex = Array.FindIndex(allBloons, item => "Moab" == item);

                        if (currentRoundIndex >= minRoundForBoss)
                        {
                            int currentBloonIndex = UnityEngine.Random.RandomRange(0, allBloons.Length - 1);
                            initalBloon = allBloons[currentBloonIndex];

                            if (currentBloonIndex > badIndex)
                                bloon.count = 1;
                        }
                        else if (currentRoundIndex >= minRoundForMoabClass)
                        {
                            initalBloon = allBloons[UnityEngine.Random.RandomRange(0, badIndex - 1)];
                        }
                        else
                        {
                            initalBloon = allBloons[UnityEngine.Random.RandomRange(0, moabIndex - 1)];
                        }
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
