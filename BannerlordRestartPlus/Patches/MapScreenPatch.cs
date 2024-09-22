using System;
using System.Collections.Generic;
using System.Reflection;

using BannerlordRestartPlus.Actions;

using HarmonyLib;

using SandBox.View.Map;

using StoryMode;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

namespace BannerlordRestartPlus.Patches
{
    [HarmonyPatch(typeof(MapScreen))]
    public static class MapScreenPatch
    {
        static MethodInfo _onEscapeScreenToggle = AccessTools.Method(typeof(MapScreen), nameof(OnEscapeMenuToggled));

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GetEscapeMenuItems))]

        public static void GetEscapeMenuItems(ref List<EscapeMenuItemVM> __result, ref MapScreen __instance)
        {
            if (Main.Settings != null && Main.Settings.Enabled)
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
                var confirm = new TextObject("{=restart_plus_02}Are you sure you want to start over with a new character in this game world?");

                MapScreen inst = __instance;
                __result.Insert(1, new EscapeMenuItemVM(restartPlusTitle, (t) =>
                {
                    inst.OnEscapeMenuToggled(false);
                    InformationManager.ShowInquiry(new InquiryData(restartPlusTitle.ToString(), confirm.ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(),
                    () =>
                    {
                        try
                        {
                            InformationManager.HideInquiry();
                            RestartPlusAction.Apply();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteDebugLineOnScreen(e.ToString());
                        }
                    },
                    () =>
                    {
                        // Cancelled. Do nothing.
                        InformationManager.HideInquiry();
                    }), true, false);

                }, null, () => new Tuple<bool, TextObject>(!enabled, disableReason), true));
            }
        }

        public static void OnEscapeMenuToggled(this MapScreen instance, bool isOpened = false)
        {
            _onEscapeScreenToggle.Invoke(instance, new object[] { isOpened });
        }
    }
}