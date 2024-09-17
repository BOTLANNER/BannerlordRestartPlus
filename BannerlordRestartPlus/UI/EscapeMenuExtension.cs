using System.Collections.Generic;

using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

namespace BannerlordRestartPlus.UI
{
    [PrefabExtension("EscapeMenu", "/Prefab/Constants/Constant[@Name='EscapeMenu.Background.Height']")]

    public class EscapeMenuExtension : PrefabExtensionSetAttributePatch
    {
        private List<Attribute> attributes = new List<Attribute>()
        {
            new Attribute("BrushName","EscapeMenuStretched.Background") // Use EscapeMenuStretched brush provided with this mod
        };

        private readonly List<Attribute> empty = new();

        public override List<Attribute> Attributes => Main.Settings != null && Main.Settings.Enabled ? attributes : empty;
    }
}
