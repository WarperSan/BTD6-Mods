using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using MelonLoader;
using ModHelperData = SubParagon.ModHelperData;

[assembly: MelonInfo(typeof(SubParagon.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace SubParagon
{
    public class Main : BloonsTD6Mod
    {
        [HarmonyPatch(typeof(TowerManager), "UpgradeTower")]
        internal class Tower_Initialise
        {
            [HarmonyPrefix]
            internal static bool Prefix(int inputIndex,
      Tower tower,
      TowerModel def,
      int pathIndex,
      float upgradeCost,
      float costMultiplier,
      bool triggerOnUpgraded = true,
      bool triggerOnUpgrade = true,
      bool playUpgradeEffect = true,
      bool isParagon = false,
      bool leveledFromEndOfRoundXp = false)
            {
                if(def.name == "MonkeySub-Paragon")
                {
                    if(tower.towerModel.tiers[0] == 5)
                    {
                        tower.SetTargetType(def.targetTypes[0]);
                    }
                    else if(tower.towerModel.tiers[2] == 5)
                    {
                        tower.towerModel = Game.instance.model.GetTowerFromId("MonkeySub-050");
                    }
                }
                return true;
            }
        }

        public override void OnTowerUpgraded(Tower tower, string upgradeName, Assets.Scripts.Models.Towers.TowerModel newBaseTowerModel)
        {
            base.OnTowerUpgraded(tower, upgradeName, newBaseTowerModel);

            if (tower.towerModel.name == "MonkeySub-Paragon")
            {
                var degree = tower.GetTowerBehavior<ParagonTower>().GetCurrentDegree();

               tower.transform.position = new Vector3Boxed(
               tower.transform.position.X,
               tower.transform.position.Y,
               tower.transform.position.Z + 12f);

                tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetDamageModel().damage = degree / 10f + 16f;
                tower.towerModel.GetAttackModel(0).weapons[0].rate = -0.00003f * (degree - 1) + 0.03f;
                tower.towerModel.GetWeapons()[1].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 20000f;
                tower.towerModel.GetWeapons()[1].rate = 5f / 7f * degree + 10f;
                tower.towerModel.GetWeapons()[2].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 2000f * (int)(degree / 10 + 1);

                if (degree >= 25)
                {
                    tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = degree / 8f + 8f;

                    tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().count = 9;
                    tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().GetBehavior<EmissionRotationOffProjectileDirectionModel>().startingOffset = -20f;
                }

                if (degree >= 50)
                {
                    tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<TravelStraitModel>().speed = 6.5f * degree;

                    tower.towerModel.GetWeapons()[1].projectile.GetBehaviors<CreateProjectileOnExpireModel>()[1].projectile.GetBehavior<ClearHitBloonsModel>().interval = 0.05f;
                    tower.towerModel.GetWeapons()[1].projectile.GetBehaviors<CreateProjectileOnExpireModel>()[1].projectile.GetBehavior<AgeModel>().lifespan = 0.4f * degree + 20f;
                }

                if (degree >= 75)
                {
                    tower.towerModel.GetWeapons()[1].rate = 60f;

                    tower.towerModel.GetBehavior<PreEmptiveStrikeLauncherModel>().projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<DamageModel>().damage = 1500f;

                    tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().count = 13;
                    tower.towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().GetBehavior<EmissionRotationOffProjectileDirectionModel>().startingOffset = -30f;
                }

                if (degree == 100)
                {
                    tower.towerModel.GetWeapons()[1].rate = 20f;
                }
            }
        }
    }
}