
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
#if MCM_v5
using MCM.Abstractions.Base.Global;
#else
using MCM.Abstractions.Settings.Base.Global;
#endif

namespace BannerlordRestartPlus
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => $"{Main.Name}_v1";
        public override string DisplayName => Main.DisplayName;
        public override string FolderName => Main.Name;
        public override string FormatType => "json";

        private const string Enabled_Hint = "Enables Restart+ mod and adds the option to the in game pause menu.  [ Default: ON ]";

        [SettingPropertyBool("Enabled", HintText = Enabled_Hint, RequireRestart = false, Order = 0, IsToggle = true)]
        [SettingPropertyGroup("Restart+", GroupOrder = 0)]
        public bool Enabled { get; set; } = true;

        private const string CharacterCreation_Hint = "Uses full character creation for new player (recommended). Otherwise will generate a random character to play. [ Default: ON ]";

        [SettingPropertyBool("Character Creation", HintText = CharacterCreation_Hint, RequireRestart = false, Order = 1, IsToggle = false)]
        [SettingPropertyGroup("Restart+", GroupOrder = 0)]
        public bool CharacterCreation { get; set; } = true;

        private const string EditLooks_Hint = "When generating a random character to play (See 'Character Creation' option), will open character editor. Otherwise will use random features. [ Default: ON ]";

        [SettingPropertyBool("Edit Looks", HintText = EditLooks_Hint, RequireRestart = false, Order = 2, IsToggle = false)]
        [SettingPropertyGroup("Restart+", GroupOrder = 0)]
        public bool EditLooks { get; set; } = true;

        private const string SeedXP_Hint = "When generating a random character to play (See 'Character Creation' option), will prepopulate skills and xp. Otherwise will start with no skills and traits. [ Default: OFF ]";

        [SettingPropertyBool("Randomize XP", HintText = SeedXP_Hint, RequireRestart = false, Order = 3, IsToggle = false)]
        [SettingPropertyGroup("Restart+", GroupOrder = 0)]
        public bool SeedXP { get; set; } = false;

        private const string PlaceAtOldPosition_Hint = "Starts the new character in the same place on the map as where the previous character is. Otherwise will either start at default location for selected culture if Character Creation is enable, or at a random place in the world, close to a settlement. [ Default: OFF ]";

        [SettingPropertyBool("Start at same position", HintText = PlaceAtOldPosition_Hint, RequireRestart = false, Order = 4, IsToggle = false)]
        [SettingPropertyGroup("Restart+", GroupOrder = 0)]
        public bool PlaceAtOldPosition { get; set; } = false;

        private const string AllowCampaign_Hint = "Allows Restart+ for the Story mode campaign. (EXPERIMENTAL: Stability not guaranteed!) [ Default: OFF ]";

        [SettingPropertyBool("Allow for Story Campaign", HintText = AllowCampaign_Hint, RequireRestart = false, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Experimental", GroupOrder = 1)]
        public bool AllowCampaign { get; set; } = false;

        private const string PlayAsExisting_Hint = "Enable playing as any existing character. (EXPERIMENTAL: Stability not guaranteed!) [ Default: ON ]";

        [SettingPropertyBool("Allow play as existing characters", HintText = PlayAsExisting_Hint, RequireRestart = false, Order = 1, IsToggle = false)]
        [SettingPropertyGroup("Experimental")]
        public bool PlayAsExisting { get; set; } = true;
    }
}
