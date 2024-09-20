using System;

using BannerlordRestartPlus.Behaviours;

using SandBox.GauntletUI.Encyclopedia;

using StoryMode;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordRestartPlus.UI
{
    public class EncyclopediaPlayAsVM : ViewModel
    {
        private Hero currentHero;
        private readonly GauntletMapEncyclopediaView encyclopediaManager;
        private bool _isPlayAsEnabled = true;

        private HintViewModel? _disableReasonHint;

        public EncyclopediaPlayAsVM(Hero hero, GauntletMapEncyclopediaView encyclopediaManager)
        {
            this.currentHero = hero;
            this.encyclopediaManager = encyclopediaManager;
            CalculateEnabled();
        }


        [DataSourceProperty]
        public bool IsPlayAsAllowed
        {
            get
            {
                return this._isPlayAsEnabled;
            }
            set
            {
                if (value != this._isPlayAsEnabled)
                {
                    this._isPlayAsEnabled = value;
                    this?.OnPropertyChangedWithValue(value, "IsPlayAsAllowed");
                }
            }
        }

        [DataSourceProperty]
        public string PlayAsText
        {
            get
            {
                return new TextObject("{=restart_plus_04}Play As").ToString();
            }
            //set
            //{
            //    if (value != this._playAsText)
            //    {
            //        this._playAsText = value;
            //        this?.OnPropertyChangedWithValue(value, "PlayAsText");
            //    }
            //}
        }

        [DataSourceProperty]
        public HintViewModel? DisableHint
        {
            get
            {
                return this._disableReasonHint;
            }
            set
            {
                if (value != this._disableReasonHint)
                {
                    this._disableReasonHint = value;
                    this?.OnPropertyChangedWithValue(value, "DisableHint");
                }
            }
        }


        public override void RefreshValues()
        {
            base.RefreshValues();

            CalculateEnabled();
        }

        private void CalculateEnabled()
        {
            TextObject? disableReason = null;

            if (PlayAsBehaviour.Instance?.HeroToPossess == this.currentHero)
            {
                disableReason ??= new TextObject("{=restart_plus_n_15}RestartPlus: Already queued to play as this character.");
            }

            if (Main.Settings != null && Main.Settings.Enabled)
            {
                if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                {
                    disableReason ??= new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                }
                else
                {
                    if (!Main.Settings.PlayAsExisting)
                    {
                        disableReason ??= new TextObject("{=restart_plus_n_18}RestartPlus: Playing as existing characters is disabled.");
                    }

                    if (this.currentHero == Hero.MainHero)
                    {
                        disableReason ??= new TextObject("{=restart_plus_n_13}RestartPlus: Already playing as {CHARACTER}");
                        disableReason.SetTextVariable("CHARACTER", this.currentHero?.Name ?? new TextObject("{=restart_plus_n_10}Character"));
                    }

                    try
                    {
                        if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                        {
                            disableReason ??= new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                        }
                        else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                        {
                            disableReason ??= new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.PrintError(e.Message, e.StackTrace);
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        Debug.SetCrashReportCustomString(e.Message);
                        Debug.SetCrashReportCustomStack(e.StackTrace);

                        disableReason = new TextObject(e.Message);
                    }

                    if (Hero.MainHero.CurrentSettlement != null || MobileParty.MainParty.MapEvent != null || Hero.MainHero.IsPrisoner)
                    {
                        disableReason ??= new TextObject("{=restart_plus_n_05}RestartPlus: Not available. Current main character is busy (either in a settlement, in an encounter, or is a prisoner).");
                    }

                    if (this.currentHero != null && this.currentHero.Age < (Campaign.Current?.Models?.AgeModel?.HeroComesOfAge ?? 18))
                    {
                        disableReason ??= new TextObject("{=restart_plus_n_17}RestartPlus: {CHARACTER} is too young.");
                        disableReason.SetTextVariable("CHARACTER", this.currentHero?.Name ?? new TextObject("{=restart_plus_n_10}Character"));
                    }

                    if (this.currentHero != null && this.currentHero.IsDead)
                    {
                        disableReason ??= new TextObject("{=restart_plus_n_16}RestartPlus: {CHARACTER} is already dead.");
                        disableReason.SetTextVariable("CHARACTER", this.currentHero?.Name ?? new TextObject("{=restart_plus_n_10}Character"));
                    }

                    if (this.currentHero == null || this.currentHero.CurrentSettlement != null || this.currentHero.PartyBelongedTo?.MapEvent != null || this.currentHero.IsPrisoner)
                    {
                        disableReason ??= new TextObject("{=restart_plus_n_09}RestartPlus: Not available. {CHARACTER} is busy (either in a settlement, in an encounter, or is a prisoner).");
                        disableReason.SetTextVariable("CHARACTER", this.currentHero?.Name ?? new TextObject("{=restart_plus_n_10}Character"));
                    }
                }
            }
            else
            {
                disableReason = new TextObject("{=restart_plus_n_08}Restart+ not enabled!");
            }

            if (disableReason == null)
            {
                disableReason = TextObject.Empty;
                IsPlayAsAllowed = true;
            }
            else
            {
                IsPlayAsAllowed = false;
            }


            if (!IsPlayAsAllowed)
            {
                this.DisableHint = new HintViewModel(disableReason, null);
            }
            else
            {
                this.DisableHint = null;
            }
        }

        public override void OnFinalize()
        {
            base.OnFinalize();
        }


        public void ExecutePossessCharacter()
        {
            if (this.currentHero == null)
            {
                var disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!").ToString();
                Debug.PrintError(disableReason);
                Debug.WriteDebugLineOnScreen(disableReason);
                Debug.SetCrashReportCustomString(disableReason);
                Debug.SetCrashReportCustomStack(disableReason);
                InformationManager.DisplayMessage(new InformationMessage(disableReason));
            }
            else
            {
                var restartPlusTitle = new TextObject("{=restart_plus_n_01}Restart+");
                var confirm = new TextObject("{=restart_plus_05}Are you sure you want to play as {CHARACTER} in this game world?");
                confirm.SetTextVariable("CHARACTER", currentHero.Name);
                InformationManager.ShowInquiry(new InquiryData(restartPlusTitle.ToString(), confirm.ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(),
                    () =>
                    {
#if !POP_STATE_TO_MAP
                        if (!(Game.Current.GameStateManager.ActiveState is MapState))
                        {
                            var info = new TextObject("{=restart_plus_n_14}RestartPlus: Close all open windows and resume on the world map for changes to take effect.");
                            InformationManager.DisplayMessage(new InformationMessage(info.ToString()));

                            MBInformationManager.AddQuickInformation(info, 0, null, "");                         
                        }
#endif

                        if (PlayAsBehaviour.Instance != null)
                        {
                            PlayAsBehaviour.Instance.QueuePossession(this.currentHero);

                            IsPlayAsAllowed = false;
                            RefreshValues();
#if POP_STATE_TO_MAP
                            if (encyclopediaManager != null)
                            {
                                try
                                {
                                    encyclopediaManager.CloseEncyclopedia();
                                }
                                catch (Exception err)
                                {
                                    // Ignore
                                }
                            }

                            while (!(Game.Current.GameStateManager.ActiveState is MapState))
                            {
                                Game.Current.GameStateManager.PopState();
                            }
                            Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
#endif
                        }
                    },
                    () =>
                    {
                        // Cancelled. Do nothing.
                        InformationManager.HideInquiry();
                    }), true, false);
            }
        }
    }
}
