using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;

namespace SubParagon.Displays.Projectiles
{
    public class RadioactiveZone : ModDisplay
    {
        public override string BaseDisplay => "7da7168251b270846a5b60a6b29fd85c";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "7da7168251b270846a5b60a6b29fd85c");
        }
    }
}