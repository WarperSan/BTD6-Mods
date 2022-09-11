using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using UnityEngine;

namespace FarmingSentry
{
    public class FarmingSentryTower
    {
        public class FarmingSentry : ModTower
        {
            public override string BaseTower => TowerType.TechBot;
            public override string Name => "Farming Sentry";
            public override int Cost => 0;
            public override string TowerSet => "Support";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override bool DontAddToShop => !VillageParagon.VillageParagon.addSentriesInShop;

            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.ignoreBlockers = false;
                towerModel.RemoveBehaviors<AbilityModel>();
                towerModel.RemoveBehavior<IgnoreAllMutatorsTowerModel>();

                towerModel.icon = Game.instance.model.GetTowerFromId("TechBot").portrait;
                towerModel.portrait = Game.instance.model.GetTowerFromId("TechBot").portrait;
                towerModel.dontDisplayUpgrades = true;

                towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                towerModel.AddBehavior(Game.instance.model.GetTower(TowerType.BananaFarm, 5, 2, 0).GetAttackModel(0));

                towerModel.GetWeapon(0).GetBehavior<EmissionsPerRoundFilterModel>().count = 10;
                towerModel.GetWeapon(0).GetBehavior<WeaponRateMinModel>().min = 0;

                towerModel.GetWeapon(0).projectile.GetBehavior<CashModel>().minimum = 625;
                towerModel.GetWeapon(0).projectile.GetBehavior<CashModel>().maximum = 625;
                towerModel.GetWeapon(0).projectile.GetBehavior<CashModel>().bonusMultiplier = 0;

                towerModel.targetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).targetTypes;
                towerModel.TargetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).TargetTypes;

                CreditPopsToParentTowerModel creditParent = new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_");
                towerModel.AddBehavior(creditParent);

                TowerExpireModel towerExpire = new TowerExpireModel("TowerExpireModel_", 25f, 0, false, false);

                if (!VillageParagon.VillageParagon.addSentriesInShop)
                    towerModel.AddBehavior(towerExpire);

                towerModel.isSubTower = true;

                towerModel.ApplyDisplay<FarmSentryDisplay>();

                GetTowerModel<VillageParagon.VillageParagon.MonkeyVillageParagon>().GetWeapon(0).projectile.GetBehavior<CreateTypedTowerModel>().crushingTower = towerModel;
                GetTowerModel<VillageParagon.VillageParagon.MonkeyVillageParagon>().GetWeapon(0).projectile.GetBehavior<CreateTypedTowerModel>().crushingDisplay = towerModel.display;
            }
        }

        public class FarmSentryDisplay : ModDisplay
        {
            public override string BaseDisplay => GetDisplay(TowerType.TechBot);

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                GameObject sentry = node.gameObject;
                sentry.name = sentry.name.Replace("(Clone)(Clone)", "");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").gameObject.SetActive(true);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("RedEye").gameObject.SetActive(false);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("GreenEye").gameObject.SetActive(true);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("GreenEye").GetComponent<SkinnedMeshRenderer>().material.mainTexture = GetTexture("FarmSentryDisplay");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("Head 2").GetComponent<SkinnedMeshRenderer>().material.mainTexture = GetTexture("FarmSentryDisplay");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("Antenna").gameObject.SetActive(false);

                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("FarmSentryDisplay");
                }

                base.ModifyDisplayNode(node);
            }
        }
    }
}