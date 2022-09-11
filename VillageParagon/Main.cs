using Assets.Scripts.Models;
using Assets.Scripts.Simulation.Towers;
using BTD_Mod_Helper;
using MelonLoader;
using ModHelperData = VillageParagon.ModHelperData;

[assembly: MelonInfo(typeof(VillageParagon.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace VillageParagon
{
    public class Main : BloonsTD6Mod
    {
        public override string GithubReleaseURL => "https://api.github.com/WarperSan/BTD6-Mods/releases/tag/SubParagon";
    }
}