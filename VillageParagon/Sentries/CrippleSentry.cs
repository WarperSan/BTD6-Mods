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

namespace CrippleSentry
{
    public class CrippleSentryTower
    {
        public class CrippleSentry : ModTower
        {
            public override string BaseTower => TowerType.TechBot;
            public override string Name => "Big Boi Sentry";
            public override int Cost => 0;
            public override string TowerSet => "Support";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;

            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.ignoreBlockers = false;
                towerModel.RemoveBehaviors<AbilityModel>();
                towerModel.RemoveBehavior<IgnoreAllMutatorsTowerModel>();

                towerModel.icon = Game.instance.model.GetTowerFromId("TechBot").portrait;
                towerModel.portrait = Game.instance.model.GetTowerFromId("TechBot").portrait;
                towerModel.dontDisplayUpgrades = true;

                towerModel.AddBehavior(Game.instance.model.GetTower("SniperMonkey", 5, 0, 2).GetAttackModel(0));

                towerModel.isGlobalRange = true;

                towerModel.GetWeapon(0).rate = 0.15f;
                towerModel.GetWeapon(0).projectile.GetDamageModel().damage = 240;

                // Modifiers
                // Ceramics
                towerModel.GetWeapon(0).projectile.GetBehavior<DamageModifierForTagModel>().damageMultiplier = 1.5f;
                towerModel.GetWeapon(0).projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 45;

                // Moab
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().moabMutator.bloonPerHitDamageAddition = 60;
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().moabMutator.multiplier = 1;

                // BFB
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().bfbMutator.bloonPerHitDamageAddition = 120;
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().bfbMutator.multiplier = 1;

                // ZOMG
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().zomgMutator.bloonPerHitDamageAddition = 180;
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().zomgMutator.multiplier = 1;

                // DDT
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().ddtMutator.bloonPerHitDamageAddition = 180;
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().ddtMutator.multiplier = 1;

                // BAD
                towerModel.GetWeapon(0).projectile.GetBehavior<SlowMaimMoabModel>().badMutator.bloonPerHitDamageAddition = 480;

                CreditPopsToParentTowerModel creditParent = new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_");
                towerModel.AddBehavior(creditParent);

                TowerExpireModel towerExpire = new TowerExpireModel("TowerExpireModel_", 25f, 0, false, false);

                towerModel.AddBehavior(towerExpire);

                towerModel.isSubTower = true;

                towerModel.ApplyDisplay<CrippleSentryDisplay>();

                towerModel.targetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).targetTypes;
                towerModel.TargetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).TargetTypes;
                towerModel.towerSelectionMenuThemeId = "Default";

                GetTowerModel<VillageParagon.VillageParagon.MonkeyVillageParagon>().GetWeapon(0).projectile.GetBehavior<CreateTypedTowerModel>().energyTower = towerModel;
                GetTowerModel<VillageParagon.VillageParagon.MonkeyVillageParagon>().GetWeapon(0).projectile.GetBehavior<CreateTypedTowerModel>().energyDisplay = towerModel.display;
            }
        }

        public class CrippleSentryDisplay : ModDisplay
        {
            public override string BaseDisplay => GetDisplay(TowerType.TechBot);

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                GameObject sentry = node.gameObject;
                sentry.name = sentry.name.Replace("(Clone)(Clone)", "");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").gameObject.SetActive(true);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("RedEye").gameObject.SetActive(false);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("GreenEye").gameObject.SetActive(true);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("GreenEye").GetComponent<SkinnedMeshRenderer>().material.mainTexture = GetTexture("CrippleSentryDisplay");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("Head 2").GetComponent<SkinnedMeshRenderer>().material.mainTexture = GetTexture("CrippleSentryDisplay");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("Antenna").gameObject.SetActive(false);

                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("CrippleSentryDisplay");
                }

                base.ModifyDisplayNode(node);
            }
        }
    }
}