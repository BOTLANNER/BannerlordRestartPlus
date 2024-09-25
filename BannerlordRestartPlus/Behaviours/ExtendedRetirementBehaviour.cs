
using System;
using System.Collections.Generic;
using System.Linq;

using BannerlordRestartPlus.Actions;

using HarmonyLib;

using SandBox.CampaignBehaviors;

using StoryMode;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace BannerlordRestartPlus.Behaviours
{
    public class ExtendedRetirementBehaviour : CampaignBehaviorBase
    {
        private bool _restartPlus = false;
        private bool _isRetire = false;

        private Settlement? _retirementSettlement;

        private RetirementCampaignBehavior? retirementCampaignBehavior;
        public RetirementCampaignBehavior? RetirementCampaignBehavior
        {
            get
            {
                return (RetirementCampaignBehavior?) (retirementCampaignBehavior ??= SandBoxManager.Instance?.GameStarter?.CampaignBehaviors?.FirstOrDefault(b => b is RetirementCampaignBehavior) as RetirementCampaignBehavior);
            }
        }

        public static ExtendedRetirementBehaviour? Instance = null;

        public ExtendedRetirementBehaviour()
        {
            Instance = this;
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            this._retirementSettlement = Settlement.Find("retirement_retreat");
            this.SetupGameMenus(starter);
            this.SetupConversationDialogues(starter);
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.GameMenuOpened));
        }

        private void GameMenuOpened(MenuCallbackArgs args)
        {
            if (args.MenuContext.GameMenu.StringId == "retirement_place")
            {
                if (this._restartPlus)
                {
                    this._restartPlus = false;
                    retirement_menu_on_restart_plus(args, this._isRetire);
                    _isRetire = false;
                }
            }
        }

        private void SetupConversationDialogues(CampaignGameStarter starter)
        {
            starter.AddPlayerLine("player_accept_or_decline_3", "player_accept_or_decline", "close_window", "{=restart_plus_06}It might be time for others to have their own stories now. (Restart+)", new ConversationSentence.OnConditionDelegate(this.conv_restart_plus_condition), new ConversationSentence.OnConsequenceDelegate(() => this.conv_restart_plus_consequence(false)), 100, new ConversationSentence.OnClickableConditionDelegate(this.conv_restart_plus_clickable), null);
            starter.AddPlayerLine("player_accept_or_decline_4", "player_accept_or_decline", "close_window", "{=restart_plus_07}My story is over.It might be time for others to have their own stories now. (Retire and Restart+)", new ConversationSentence.OnConditionDelegate(this.conv_restart_plus_condition), new ConversationSentence.OnConsequenceDelegate(() => this.conv_restart_plus_consequence(true)), 100, new ConversationSentence.OnClickableConditionDelegate(this.conv_restart_plus_clickable), null);
        }

        private bool conv_restart_plus_condition()
        {
            return Main.Settings != null && Main.Settings.Enabled && Main.Settings.AddOptionsOnRetire;
        }

        private void conv_restart_plus_consequence(bool isRetire)
        {
            this._isRetire = isRetire;
            try
            {
                typeof(RetirementCampaignBehavior).Field("_selectedHeir").SetValue(RetirementCampaignBehavior, null);
                typeof(RetirementCampaignBehavior).Field("_playerEndedGame").SetValue(RetirementCampaignBehavior, false);
                typeof(RetirementCampaignBehavior).Field("_hasTalkedWithHermitBefore").SetValue(RetirementCampaignBehavior, false);
            }
            catch (Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
            }
            this._restartPlus = true;
            Mission.Current.EndMission();
        }

        private bool conv_restart_plus_clickable(out TextObject explanation)
        {
            if (Main.Settings != null && Main.Settings.Enabled)
            {
                bool enabled = true;
                explanation = new TextObject("{=restart_plus_n_01}Restart+");

                if (!Main.Settings.AddOptionsOnRetire)
                {
                    enabled = false;
                    explanation = new TextObject("{=restart_plus_n_19}RestartPlus: Support for retirement is disabled.");
                }
                else if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                {
                    enabled = false;
                    explanation = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                }

                try
                {
                    if (enabled && Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                    {
                        enabled = false;
                        explanation = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                    }
                    else if (enabled && Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                    {
                        enabled = false;
                        explanation = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                    }
                }
                catch (Exception e)
                {
                    Debug.PrintError(e.Message, e.StackTrace);
                    Debug.WriteDebugLineOnScreen(e.ToString());
                    Debug.SetCrashReportCustomString(e.Message);
                    Debug.SetCrashReportCustomStack(e.StackTrace);

                    enabled = false;
                    explanation = new TextObject(e.Message);
                }

                return enabled;
            }
            explanation = new TextObject("{=restart_plus_n_08}Restart+ not enabled!");
            return false;
        }

        public void SetupGameMenus(CampaignGameStarter starter)
        {
            try
            {
                var presumed = starter.GetPresumedGameMenu("retirement_place");
                var presumedAfterKnocout = starter.GetPresumedGameMenu("retirement_after_player_knockedout");

                if (presumed.MenuOptions is List<GameMenuOption> menuOptions)
                {
                    var retirement_place_restart_plus = menuOptions.FirstOrDefault(o => o.IdString == "retirement_place_restart_plus");
                    var retirement_place_restart_plus_retire = menuOptions.FirstOrDefault(o => o.IdString == "retirement_place_restart_plus_retire");
                    if (retirement_place_restart_plus != null)
                    {
                        menuOptions.Remove(retirement_place_restart_plus);
                    }
                    if (retirement_place_restart_plus_retire != null)
                    {
                        menuOptions.Remove(retirement_place_restart_plus_retire);
                    }
                }
                if (presumedAfterKnocout.MenuOptions is List<GameMenuOption> menuOptions2)
                {
                    var retirement_place_restart_plus = menuOptions2.FirstOrDefault(o => o.IdString == "retirement_place_restart_plus");
                    var retirement_place_restart_plus_retire = menuOptions2.FirstOrDefault(o => o.IdString == "retirement_place_restart_plus_retire");
                    if (retirement_place_restart_plus != null)
                    {
                        menuOptions2.Remove(retirement_place_restart_plus);
                    }
                    if (retirement_place_restart_plus_retire != null)
                    {
                        menuOptions2.Remove(retirement_place_restart_plus_retire);
                    }
                }


                if (Main.Settings != null && Main.Settings.Enabled && Main.Settings.AddOptionsOnRetire)
                {
                    starter.AddGameMenuOption("retirement_place", "retirement_place_restart_plus", "{=restart_plus_n_01}Restart+", new GameMenuOption.OnConditionDelegate((args) => this.leave_on_condition(args, false)), new GameMenuOption.OnConsequenceDelegate((args) => this.retirement_menu_on_restart_plus(args, false)), true, -1, false, null);
                    starter.AddGameMenuOption("retirement_place", "retirement_place_restart_plus_retire", "{=restart_plus_n_08}Retire (Restart+)", new GameMenuOption.OnConditionDelegate((args) => this.leave_on_condition(args, true)), new GameMenuOption.OnConsequenceDelegate((args) => this.retirement_menu_on_restart_plus(args, true)), true, -1, false, null);
                    starter.AddGameMenuOption("retirement_after_player_knockedout", "retirement_place_restart_plus", "{=restart_plus_n_01}Restart+", new GameMenuOption.OnConditionDelegate((args) => this.leave_on_condition(args, false)), new GameMenuOption.OnConsequenceDelegate((args) => this.retirement_menu_on_restart_plus(args, false)), true, -1, false, null);
                    starter.AddGameMenuOption("retirement_after_player_knockedout", "retirement_place_restart_plus_retire", "{=restart_plus_n_08}Retire (Restart+)", new GameMenuOption.OnConditionDelegate((args) => this.leave_on_condition(args, true)), new GameMenuOption.OnConsequenceDelegate((args) => this.retirement_menu_on_restart_plus(args, true)), true, -1, false, null);
                }
            }
            catch (Exception e)
            {
                // Ignore
            }

        }

        private void retirement_menu_on_restart_plus(MenuCallbackArgs args, bool isRetire)
        {
            PlayerEncounter.LeaveSettlement();
            PlayerEncounter.Finish(true);

            if (Main.Settings != null && Main.Settings.Enabled && Main.Settings.AddOptionsOnRetire)
            {
                var restartPlusTitle = new TextObject("{=restart_plus_n_01}Restart+");
                var confirm = new TextObject("{=restart_plus_02}Are you sure you want to start over with a new character in this game world?");

                InformationManager.ShowInquiry(new InquiryData(restartPlusTitle.ToString(), confirm.ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(),
                () =>
                {
                    try
                    {
                        InformationManager.HideInquiry();
                        if (isRetire)
                        {
                            RestartPlusAction.PostApply = (Hero oldHero, Hero newHero) =>
                            {
                                if (oldHero != null && !oldHero.IsDead && oldHero.IsClanLeader)
                                {
                                    ChangeClanLeaderAction.ApplyWithoutSelectedNewLeader(oldHero.Clan);
                                    DisableHeroAction.Apply(oldHero);
                                    LogEntry.AddLogEntry(new PlayerRetiredLogEntry(oldHero));
                                    TextObject textObject = new TextObject("{=0MTzaxau}{?CHARACTER.GENDER}She{?}He{\\?} retired from adventuring, and was last seen with a group of mountain hermits living a life of quiet contemplation.", null);
                                    textObject.SetCharacterProperties("CHARACTER", oldHero.CharacterObject, false);
                                    oldHero.EncyclopediaText = textObject;
                                }
                            };
                        }
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
            }
        }

        private bool leave_on_condition(MenuCallbackArgs args, bool isRetire)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            if (Main.Settings != null && Main.Settings.Enabled)
            {
                bool enabled = true;
                TextObject? disableReason = null;

                if (!Main.Settings.AddOptionsOnRetire)
                {
                    enabled = false;
                    disableReason = new TextObject("{=restart_plus_n_19}RestartPlus: Support for retirement is disabled.");
                }
                else if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
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

                args.Tooltip = disableReason ?? new TextObject(isRetire ? "{=restart_plus_h_05}RestartPlus: Current character retires and start with new character in same world (Current character is gone)." : "{=restart_plus_h_04}RestartPlus: Start with new character in same world (Current character is still around).");
                return enabled;
            }
            return false;
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}