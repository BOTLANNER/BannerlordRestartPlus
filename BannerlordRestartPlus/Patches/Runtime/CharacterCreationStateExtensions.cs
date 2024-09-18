using System;
using System.Linq;
using System.Reflection;

using BannerlordRestartPlus.Actions;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordRestartPlus.Patches.Runtime
{
    public static class CharacterCreationStateExtensions
    {

        static FieldInfo ActiveSaveSlotNameField = AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName");
        static MethodInfo GetNextAvailableSaveNameMethod = AccessTools.Method(typeof(MBSaveLoad), "GetNextAvailableSaveName");

        public static CharacterCreationState? CharacterCreationState = null;
        public static MapState? MapState = null;
        public static Vec2? Position = null;

        public static void FinalizeCharacterCreationInGame(this CharacterCreationState __instance)
        {
            __instance.CharacterCreation.ApplyFinalEffects();
            Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(__instance);
            if (Game.Current.GameStateManager.ActiveState == __instance)
            {
                Game.Current.GameStateManager.PopState();
            }
            PartyBase.MainParty.SetVisualAsDirty();
            ICharacterCreationStateHandler characterCreationStateHandler = __instance.Handler;
            if (characterCreationStateHandler != null)
            {
                characterCreationStateHandler.OnCharacterCreationFinalized();
            }
            else
            {
            }
            __instance.CurrentCharacterCreationContent.OnCharacterCreationFinalized();
            CampaignEventDispatcher.Instance.OnCharacterCreationIsOver();

            PartyTemplateObject oldTemplate = Campaign.Current.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template");
            if (oldTemplate.Stacks.Where(s => s.Character == CharacterObject.PlayerCharacter).Count() == 0)
            {
                oldTemplate.Stacks.Clear();
                oldTemplate.Stacks.Add(new PartyTemplateStack
                {
                    Character = CharacterObject.PlayerCharacter,
                    MaxValue = 1,
                    MinValue = 1
                });
            }

            var pos = MobileParty.MainParty.Position2D;
            Campaign.Current.InitializeMainParty();

            MobileParty.MainParty.Position2D = pos;
            if ((Main.Settings!.PlaceAtOldPosition || pos == Campaign.Current.DefaultStartingPosition || pos == default) && Position != null && Position != default(Vec2))
            {
                MobileParty.MainParty.Position2D = Position.Value;

            }

            Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;

            CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(RestartPlusAction.Instance, new Action<bool, string>(RestartPlusAction.Instance.LoadInternal));

            string saveName = (string) ActiveSaveSlotNameField.GetValue(null);
            if (saveName == null)
            {
                saveName = (string) GetNextAvailableSaveNameMethod.Invoke(null, new object[] { });
                ActiveSaveSlotNameField.SetValue(null, saveName);
            }

            saveName = saveName.Replace(new TextObject("{=restart_plus_n_02} (auto)").ToString(), new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString());
            if (!saveName.EndsWith(new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString()))
            {
                saveName += new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString();
            }

            Campaign.Current.SaveHandler.SaveAs(saveName);
        }
    }
}
