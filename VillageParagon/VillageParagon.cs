using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;

namespace VillageParagon
{
    public class VillageParagon
    {
        public class MonkeyVillageParagon : ModVanillaParagon
        {
            public override string BaseTower => "MonkeyVillage-140";
            public override string Name => "MonkeyVillage";
        }

        static public bool addSentriesInShop = false;

        public class MonkeyVillageParagonUpgrade : ModParagonUpgrade<MonkeyVillageParagon>
        {
            public override int Cost => 650000;
            public override string Description => "Throws specialized Bots on the field. Tier 5s sacrifices enchance the paragon's tower of the same type but will consume everything that isn't a village. Also gives mega buffs.";
            public override string DisplayName => "Monkey Industry";

            public override string Icon => "VillageParagon-Icon";
            public override string Portrait => "VillageParagon-Portrait";

            public override void OnDegreeSet(Tower tower, int degree)
            {
                int[] tier5Counts = new int[4] { 0, 0, 0, 0 }; // Primary, Military, Magic, Support

                const int max5Tier = 5;

                for (int i = 0; i < InGame.Bridge.GetAllTowers().Count; i++)
                {
                    Tower towerToSell = InGame.Bridge.GetAllTowers()[i].tower;
                    if (!towerToSell.towerModel.name.Contains("Village") && !towerToSell.towerModel.IsHero() && !towerToSell.towerModel.isParagon && !towerToSell.towerModel.isPowerTower)
                    {
                        Vector2 paragonPos = tower.transform.position.ToVector2();
                        Vector2 towerPos = InGame.Bridge.GetAllTowers()[i].tower.transform.position.ToVector2();

                        float distance = Math.Sqrt(Math.Pow2(paragonPos.x - towerPos.x) + Math.Pow2(paragonPos.y - towerPos.y));

                        if (distance <= tower.towerModel.range)
                        {
                            if (InGame.Bridge.GetAllTowers()[i].tower.towerModel.tier == 5)
                            {
                                switch (InGame.Bridge.GetAllTowers()[i].tower.towerModel.towerSet)
                                {
                                    case "Primary":
                                        if (tier5Counts[0] + 1 <= max5Tier)
                                            tier5Counts[0]++;
                                        break;
                                    case "Military":
                                        if (tier5Counts[1] + 1 <= max5Tier)
                                            tier5Counts[1]++;
                                        break;
                                    case "Magic":
                                        if (tier5Counts[2] + 1 <= max5Tier)
                                            tier5Counts[2]++;
                                        break;
                                    case "Support":
                                        if (tier5Counts[3] + 1 <= max5Tier)
                                            tier5Counts[3]++;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            InGame.Bridge.GetAllTowers()[i].tower.worth = 0;
                            InGame.Bridge.GetAllTowers()[i].tower.SellTower();
                        }
                    }
                }

                // Degree ranking up
                // Start Cripple Moab
                TowerModel crippleMoab = tower.towerModel.GetWeapon(3).projectile.GetBehavior<CreateTowerModel>().tower;
                tower.towerModel.GetWeapon(3).rate = 30;

                crippleMoab.GetWeapon(0).rate = 0.15f - degree / 1000f;
                crippleMoab.GetWeapon(0).projectile.GetDamageModel().damage = 240 + Math.FloorToInt(960f * degree / 1000f) * 10 * tier5Counts[1] / max5Tier;
                // End Cripple Moab ---------


                // Start Banana Central
                TowerModel bananaCentral = tower.towerModel.GetWeapon(0).projectile.GetBehavior<CreateTowerModel>().tower;
                tower.towerModel.GetWeapon(0).rate = 60;

                bananaCentral.GetWeapon(0).projectile.GetBehavior<CashModel>().minimum = 625 + 625 * degree / 100 * tier5Counts[3] / max5Tier;
                bananaCentral.GetWeapon(0).projectile.GetBehavior<CashModel>().maximum = 625 + 625 * degree / 100 * tier5Counts[3] / max5Tier;
                // End Banana Central ------------


                // Start Dark Champ
                TowerModel darkChamp = tower.towerModel.GetWeapon(2).projectile.GetBehavior<CreateTowerModel>().tower;
                tower.towerModel.GetWeapon(2).rate = 60;

                darkChamp.GetAbility(0).cooldown = 15 - 10 * degree / 100 * tier5Counts[2] / max5Tier;

                darkChamp.GetWeapon(0).projectile.GetDamageModel().damage = 4 + 12 * degree / 100 * tier5Counts[2] / max5Tier;
                darkChamp.GetWeapon(0).projectile.pierce = 18 + 54 * degree / 100 * tier5Counts[2] / max5Tier;
                // End Dark Champ -----------

                // Start Bloon Solver
                TowerModel bloonSolver = tower.towerModel.GetWeapon(1).projectile.GetBehavior<CreateTowerModel>().tower;
                tower.towerModel.GetWeapon(1).rate = 60;

                bloonSolver.GetAttackModel(0).weapons[0].projectile.GetBehavior<SlowModifierForTagModel>().slowMultiplier = 4  + 2 * degree / 100 * tier5Counts[3] / max5Tier;
                bloonSolver.GetAttackModel(0).weapons[0].projectile.GetBehaviors<SlowForBloonModel>()[0].multiplier = 0.2f + 0.4f * degree / 100 * tier5Counts[3] / max5Tier;
                bloonSolver.GetAttackModel(0).weapons[0].projectile.GetBehaviors<SlowForBloonModel>()[1].multiplier = 0.4f + 0.8f * degree / 100 * tier5Counts[3] / max5Tier;
                bloonSolver.GetAttackModel(0).weapons[0].projectile.GetBehaviors<SlowModifierForTagModel>()[1].slowMultiplier = 3f + 3f * degree / 100 * tier5Counts[3] / max5Tier;

                bloonSolver.GetWeapon(0).rate = 0.15f - 0.1f * degree / 100 * tier5Counts[0] / max5Tier;
                // End Bloon Solver -----------

                System.Console.WriteLine($"{tier5Counts[0]};{tier5Counts[1]};{tier5Counts[2]};{tier5Counts[3]}");

                base.OnDegreeSet(tower, degree);
            }

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.RemoveBehavior<AttackModel>();

                // 400 Engi Sentry Behavior
                //towerModel.AddBehavior(Game.instance.model.GetTower(TowerType.EngineerMonkey, 4, 0, 0).GetAttackModel(0));
                for (int i = 0; i < 4; i++)
                {
                    towerModel.AddBehavior(Game.instance.model.GetTower(TowerType.EngineerMonkey, 1, 0, 0).GetAttackModel(0).Duplicate());
                    towerModel.GetAttackModel(i).weapons[0].Rate = 10f;

                    towerModel.GetAttackModel(i).GetBehavior<RandomPositionModel>().maxDistance = towerModel.range;
                }

                towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                towerModel.range = 87;

                // Crushing = Banana Central
                // Boom = Bloon Solver
                // Cold = Dark Champion
                // Energy = Cripple Moab

                towerModel.targetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).targetTypes;
                towerModel.TargetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).TargetTypes;

                // Dummy attack
                towerModel.AddBehavior(Game.instance.model.GetTower("SniperMonkey").GetAttackModel(0).Duplicate());
                towerModel.GetAttackModel(4).weapons[0].rate = 999999;
                towerModel.GetAttackModel(4).GetBehavior<RotateToTargetModel>().rotateTower = false;
                towerModel.GetAttackModel(4).weapons[0].projectile.display.guidRef = null;
                towerModel.GetAttackModel(4).weapons[0].RemoveBehavior<EjectEffectModel>();
                towerModel.GetAttackModel(4).weapons[0].projectile.GetBehavior<DisplayModel>().display.guidRef = null;
                towerModel.GetAttackModel(4).weapons[0].projectile.GetDamageModel().damage = 0;

                // Mega discount
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 0, 0, 2).GetBehavior<DiscountZoneModel>());
                towerModel.GetBehavior<DiscountZoneModel>().discountMultiplier = 0.30f;
                towerModel.GetBehavior<DiscountZoneModel>().tierCap = 5;
                towerModel.GetBehavior<DiscountZoneModel>().buffLocsName = "MonkeyBusinessBuff";
                towerModel.GetBehavior<DiscountZoneModel>().stackName = "MonkeyIndustryDiscount";
                towerModel.GetBehavior<DiscountZoneModel>().name = "MonkeyIndustryDiscount";
                towerModel.GetBehavior<DiscountZoneModel>().buffIconName = "BuffIconVillagexx1";

                // Extra range
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 5, 0, 0).GetBehavior<RangeSupportModel>());
                towerModel.GetBehavior<RangeSupportModel>().multiplier = 0.5f;

                // Boosted Drums
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 5, 0, 0).GetBehavior<RateSupportModel>());
                towerModel.GetBehavior<RateSupportModel>().multiplier = 1.7f;
            }
        }

        public class VillageParagonDisplay : ModTowerDisplay<MonkeyVillageParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.MonkeyVillage, 1, 4, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("VillageParagonDisplay");
                    //node.SaveMeshTexture();
                }

                base.ModifyDisplayNode(node);
            }
        }
    }
}