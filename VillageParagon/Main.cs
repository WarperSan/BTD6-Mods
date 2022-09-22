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
        public override string MelonInfoCsURL => "https://raw.githubusercontent.com/WarperSan/BTD6-Mods/main/VillageParagon/Main.cs";
        public override string LatestURL => "https://github.com/WarperSan/BTD6-Mods/blob/main/VillageParagon/VillageParagon.dll?raw=true";
    }
}
