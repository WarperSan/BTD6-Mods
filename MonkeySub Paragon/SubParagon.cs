using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using SubParagon.Displays.Projectiles;

namespace SubParagon
{
    public class ParagonSub
    {
        public class MonkeySubParagon : ModVanillaParagon
        {
            public override string BaseTower => "MonkeySub-140";
            public override string Name => "MonkeySub";
        }

        public class USSSeawolf : ModParagonUpgrade<MonkeySubParagon>
        {
            public override int Cost => 450000;
            public override string Description => "\"After YEARS of making, I finally succeeded to create the perfect radioactive destroyer. It can destroy anything! Even... Oh no... I forgot about DDTs...\"\n- Dr. Monkey";
            public override string DisplayName => "USS Seawolf-575";

            public override string Icon => "USS-Seawolf";
            public override string Portrait => "SubParagon-Portrait";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                CreateProjectileOnExpireModel radioactiveMissile = null;
                ClearHitBloonsModel clearHit = null;

                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeySub-140").GetAttackModel(1));

                for (int i = 0; i < Game.instance.model.towers.Count; i++)
                {
                    if (Game.instance.model.towers[i].name.Contains("MonkeySub") && Game.instance.model.towers[i].name.Contains("5"))
                    {
                        Game.instance.model.towers[i].paragonUpgrade = Game.instance.model.GetTowerFromId("DartMonkey-050").paragonUpgrade;
                        Game.instance.model.towers[i].paragonUpgrade.tower = "MonkeySub-Paragon";
                        Game.instance.model.towers[i].paragonUpgrade.upgrade = "USS Seawolf-575 Paragon";
                    }

                    // Allows the radioation to hit multiple times
                    if (Game.instance.model.towers[i].name == "SpikeFactory")
                    {
                        clearHit = Game.instance.model.towers[i].GetWeapons()[0].projectile.GetBehavior<ClearHitBloonsModel>().Duplicate();
                        clearHit.interval = 0.1f;
                    }

                    // Takes the darts from the 025 and modifies them
                    if (Game.instance.model.towers[i].name == "MonkeySub-025")
                    {
                        TowerModel tower = Game.instance.model.towers[i];

                        towerModel.GetAttackModel(0).weapons[0].projectile = tower.GetAttackModel(0).weapons[0].projectile.Duplicate();

                        // Can see camo
                        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive = false;

                        // Change the amount of darts
                        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().count = 7;
                        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().GetBehavior<EmissionRotationOffProjectileDirectionModel>().startingOffset = -15f;
                        towerModel.GetBehaviors<AttackModel>()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().GetBehavior<EmissionRotationOffProjectileDirectionModel>().angleInBetween = 5f;

                        // Base projectile stats
                        towerModel.GetAttackModel(0).weapons[0].rate = 0.03f;
                        towerModel.GetAttackModel(0).weapons[0].projectile.GetDamageModel().damage = 16f;
                        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<TravelStraitModel>().speed = 300f;

                        // Second projectile stats
                        towerModel.GetBehaviors<AttackModel>()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = 8f;
                        towerModel.GetBehaviors<AttackModel>()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<TravelStraitModel>().speed = 900f;
                    }

                    // Take the radiation from the 520
                    if (Game.instance.model.towers[i].name == "MonkeySub-520")
                    {
                        TowerModel tower = Game.instance.model.towers[i];

                        radioactiveMissile = towerModel.GetWeapons()[1].projectile.GetBehavior<CreateProjectileOnExpireModel>().Duplicate();

                        radioactiveMissile.projectile = tower.GetBehavior<SubmergeEffectModel>().projectileModel.Duplicate();
                        radioactiveMissile.projectile.GetBehavior<DisplayModel>().ignoreRotation = true;
                        radioactiveMissile.projectile.GetBehavior<DamageModel>().damage = 500f;
                        radioactiveMissile.projectile.scale = 1f / 2f;
                        radioactiveMissile.projectile.radius = 24f;

                        radioactiveMissile.projectile.ApplyDisplay<RadioactiveZone>();
                        radioactiveMissile.projectile.GetBehavior<AgeModel>().lifespan = 20f;

                        towerModel.GetWeapons()[1].projectile.GetBehavior<CreateSoundOnProjectileExpireModel>().sound1 = tower.GetBehavior<SubmergeModel>().submergeSound;
                        towerModel.GetWeapons()[1].projectile.GetBehavior<CreateSoundOnProjectileExpireModel>().sound2 = tower.GetBehavior<SubmergeModel>().submergeSound;
                        towerModel.GetWeapons()[1].projectile.GetBehavior<CreateSoundOnProjectileExpireModel>().sound3 = tower.GetBehavior<SubmergeModel>().submergeSound;
                        towerModel.GetWeapons()[1].projectile.GetBehavior<CreateSoundOnProjectileExpireModel>().sound4 = tower.GetBehavior<SubmergeModel>().submergeSound;
                        towerModel.GetWeapons()[1].projectile.GetBehavior<CreateSoundOnProjectileExpireModel>().sound5 = tower.GetBehavior<SubmergeModel>().submergeSound;
                    }

                    // Pre-Emptive Strike
                    if (Game.instance.model.towers[i].name == "MonkeySub-050")
                    {
                        TowerModel tower = Game.instance.model.towers[i];

                        TowerModelBehaviorExt.RemoveBehavior<AbilityModel>(towerModel);
                        towerModel.AddBehavior(tower.GetBehavior<PreEmptiveStrikeLauncherModel>());

                        towerModel.GetBehavior<PreEmptiveStrikeLauncherModel>().projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<DamageModel>().damage = 1500f;
                    }
                }
                radioactiveMissile.projectile.AddBehavior(clearHit);

                //Normal Missile
                towerModel.GetWeapons()[2].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 2000f;
                towerModel.GetWeapons()[2].rate = 0.5f;

                // Radioactive Missile Attack
                towerModel.GetWeapons()[1].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 20000f;
                towerModel.GetWeapons()[1].rate = 10f;
                towerModel.GetWeapons()[1].projectile.AddBehavior(radioactiveMissile);

                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                towerModel.range = 90f;

                for (int i = 0; i < towerModel.GetBehaviors<AttackModel>().Count; i++)
                {
                    towerModel.GetBehaviors<AttackModel>()[i].range = towerModel.range;
                }
            }
        }


        public class SubParagonBaseDisplay : ModTowerDisplay<MonkeySubParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.MonkeySub, 1, 4, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("SubParagonBaseDisplay");
                }

                base.ModifyDisplayNode(node);
            }
        }
    }
}