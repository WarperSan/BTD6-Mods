using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using UnityEngine;

namespace SolverSentry
{
    public class SolverSentryTower
    {
        public class SolverSentry : ModTower
        {
            public override string BaseTower => TowerType.TechBot;
            public override string Name => "Solver Sentry";
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

                towerModel.AddBehavior(Game.instance.model.GetTower("GlueGunner", 5, 2, 0).GetAttackModel(0));

                towerModel.GetWeapon(0).projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 5;
                towerModel.GetWeapon(0).rate = 0.15f;

                towerModel.range = Game.instance.model.GetTower("GlueGunner", 5, 2, 0).range;
                towerModel.GetAttackModel().range = Game.instance.model.GetTower("GlueGunner", 5, 2, 0).range;

                CreditPopsToParentTowerModel creditParent = new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_");
                towerModel.AddBehavior(creditParent);

                TowerExpireModel towerExpire = new TowerExpireModel("TowerExpireModel_", 25f, 0, false, false);

                if (!VillageParagon.VillageParagon.addSentriesInShop)
                    towerModel.AddBehavior(towerExpire);

                towerModel.isSubTower = true;

                towerModel.ApplyDisplay<SolverSentryDisplay>();

                towerModel.targetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).targetTypes;
                towerModel.TargetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).TargetTypes;
                towerModel.towerSelectionMenuThemeId = "Default";

                GetTowerModel<VillageParagon.VillageParagon.MonkeyVillageParagon>().GetWeapon(0).projectile.GetBehavior<CreateTypedTowerModel>().boomTower = towerModel;
                GetTowerModel<VillageParagon.VillageParagon.MonkeyVillageParagon>().GetWeapon(0).projectile.GetBehavior<CreateTypedTowerModel>().boomDisplay = towerModel.display;
            }
        }

        public class SolverSentryDisplay : ModDisplay
        {
            public override string BaseDisplay => GetDisplay(TowerType.TechBot);

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                GameObject sentry = node.gameObject;
                sentry.name = sentry.name.Replace("(Clone)(Clone)", "");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").gameObject.SetActive(true);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("RedEye").gameObject.SetActive(false);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("GreenEye").gameObject.SetActive(true);
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("GreenEye").GetComponent<SkinnedMeshRenderer>().material.mainTexture = GetTexture("SolverSentryDisplay");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("Head 2").GetComponent<SkinnedMeshRenderer>().material.mainTexture = GetTexture("SolverSentryDisplay");
                sentry.transform.FindChild("LOD_1").FindChild("TechBot").FindChild("Head 1").FindChild("Antenna").gameObject.SetActive(false);

                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("SolverSentryDisplay");
                }

                base.ModifyDisplayNode(node);
            }
        }
    }
}