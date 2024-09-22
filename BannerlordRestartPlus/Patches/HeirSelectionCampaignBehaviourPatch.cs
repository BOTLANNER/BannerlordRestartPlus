
using System.Collections.Generic;

using HarmonyLib;

using SandBox.CampaignBehaviors;
using SandBox.View.Map;

using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using System;
using Helpers;
using StoryMode;
using TaleWorlds.Library;
using BannerlordRestartPlus.Actions;

namespace BannerlordRestartPlus.Patches
{

    [HarmonyPatch]
    public static class HeirSelectionCampaignBehaviourReversePatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(HeirSelectionCampaignBehavior), nameof(HeirSelectionCampaignBehaviourPatch.OnBeforeMainCharacterDied))]
        public static void OnBeforeMainCharacterDied(HeirSelectionCampaignBehavior instance, Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true) =>
            // its a stub so it has no initial content
            throw new NotImplementedException("It's a stub");
    }

    [HarmonyPatch(typeof(HeirSelectionCampaignBehavior))] 
    public static class HeirSelectionCampaignBehaviourPatch
    {


        [HarmonyPrefix]
        [HarmonyPatch(nameof(OnBeforeMainCharacterDied))]
        public static bool OnBeforeMainCharacterDied(ref HeirSelectionCampaignBehavior __instance, Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
        {
            if (Main.Settings != null && Main.Settings.Enabled && Main.Settings.PromptOnDeath)
            {
                bool enabled = true;
                TextObject disableReason = TextObject.Empty;

                if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                {
                    enabled = false;
                    disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                }

                try
                {
                    if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                    {
                        enabled = false;
                        disableReason = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                    }
                    else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                    {
                        enabled = false;
                        disableReason = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                    }
                }
                catch (Exception e)
                {
                    Debug.PrintError(e.Message, e.StackTrace);
                    Debug.WriteDebugLineOnScreen(e.ToString());
                    Debug.SetCrashReportCustomString(e.Message);
                    Debug.SetCrashReportCustomStack(e.StackTrace);

                    enabled = false;
                    disableReason = new TextObject(e.Message);
                }

                var restartPlusTitle = new TextObject("{=restart_plus_n_01}Restart+");
                var confirm = new TextObject("{=restart_plus_09}{CHARACTER} has died! Do you want to start over with a new character in this game world?");
                confirm.SetTextVariable("CHARACTER", victim.Name);

                HeirSelectionCampaignBehavior instance = __instance;
                if (enabled)
                {
                    InformationManager.ShowInquiry(new InquiryData(restartPlusTitle.ToString(), confirm.ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(),
                    () =>
                    {
                        try
                        {
                            InformationManager.HideInquiry();
                            if (PlayerEncounter.Current != null && (PlayerEncounter.Battle == null || !PlayerEncounter.Battle.IsFinalized))
                            {
                                PlayerEncounter.Finish(true);
                            }
                            RestartPlusAction.PostApply = (oldHero, newHero) =>
                            {
                                if (oldHero.IsPrisoner)
                                {
                                    EndCaptivityAction.ApplyByDeath(oldHero);
                                }
                                var oldParty = oldHero.PartyBelongedTo;
                                KillCharacterAction.ApplyByDeathMarkForced(oldHero, true);
                            };
                            RestartPlusAction.Apply();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteDebugLineOnScreen(e.ToString());
                        }
                    },
                    () =>
                    {
                        // Cancelled. Do default.
                        //InformationManager.HideInquiry();
                        HeirSelectionCampaignBehaviourReversePatch.OnBeforeMainCharacterDied(instance, victim, killer, detail, showNotification);
                    }), true, false);

                    return false;
                }
            }
            return true;
        }
    }
}