using System;
using System.Linq;
using System.Reflection;

using BannerlordRestartPlus.Actions;
using BannerlordRestartPlus.UI;

using HarmonyLib;

using SandBox.GauntletUI.Encyclopedia;
using SandBox.View.Map;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace BannerlordRestartPlus
{
    public class PlayAsBehaviour : CampaignBehaviorBase
    {
        public static PlayAsBehaviour? Instance = null;

        public Hero? HeroToPossess = null;

        private EncyclopediaPlayAsVM? playAsVM;

        private EncyclopediaHeroPageVM? selectedHeroPage = null;

        private Hero? selectedHero = null;

        private ScreenBase? gauntletLayerTopScreen;

        private GauntletLayer? gauntletLayer;

        private IGauntletMovie? gauntletMovie;

        static Color Error = new(178 * 255, 34 * 255, 34 * 255);
        static Color Warn = new(189 * 255, 38 * 255, 0);

        public PlayAsBehaviour(): base()
        {
            Instance = this;
        }

        #region Overrides
        public override void RegisterEvents()
        {
            Game.Current.EventManager.RegisterEvent<EncyclopediaPageChangedEvent>((EncyclopediaPageChangedEvent e) => this.AddPlayAsLayer(e));
            CampaignEvents.TickEvent.AddNonSerializedListener(this, new Action<float>(this.Tick));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
        #endregion

        #region Event Handlers

        public void Tick(float delta)
        {
            if (Main.Settings != null && Main.Settings.PlayAsExisting && this.HeroToPossess != null)
            {
                var currentHero = this.HeroToPossess;
                this.HeroToPossess = null;
                try
                {
                    InformationManager.HideInquiry();
                    PlayAsCharacterAction.Apply(currentHero);
                }
                catch (Exception e)
                {
                    Debug.WriteDebugLineOnScreen(e.ToString());
                }
            }
        }

        private void AddPlayAsLayer(EncyclopediaPageChangedEvent evt)
        {
            try
            {
                if (!(Main.Settings?.PlayAsExisting ?? false))
                {
                    return;
                }

                EncyclopediaPages newPage = evt.NewPage;
                this.selectedHeroPage = null;
                this.selectedHero = null;
                if (this.gauntletLayerTopScreen != null && this.gauntletLayer != null)
                {
                    this.gauntletLayerTopScreen.RemoveLayer(this.gauntletLayer);
                    if (this.gauntletMovie != null)
                    {
                        this.gauntletLayer.ReleaseMovie(this.gauntletMovie);
                    }
                    this.gauntletLayerTopScreen = null;
                    this.gauntletMovie = null;
                }
                if (newPage == EncyclopediaPages.Hero)
                {
                    GauntletMapEncyclopediaView encyclopediaScreenManager = MapScreen.Instance.EncyclopediaScreenManager as GauntletMapEncyclopediaView;
                    if (encyclopediaScreenManager != null)
                    {
                        var encyclopediaData = AccessTools.Field(encyclopediaScreenManager.GetType(), "_encyclopediaData").GetValue(encyclopediaScreenManager) as EncyclopediaData;
                        if (encyclopediaData == null)
                        {
                            return;
                        }
                        this.selectedHeroPage = AccessTools.Field(encyclopediaData.GetType(), "_activeDatasource").GetValue(encyclopediaData) as EncyclopediaHeroPageVM;
                        if (this.selectedHeroPage != null)
                        {
                            this.selectedHero = this.selectedHeroPage.Obj as Hero;
                            if (this.selectedHero == null)
                            {
                                return;
                            }
                            this.gauntletLayer = new GauntletLayer(716, "GauntletLayer", false);
                            this.playAsVM = new EncyclopediaPlayAsVM(this.selectedHero, encyclopediaScreenManager);

                            this.gauntletMovie = this.gauntletLayer.LoadMovie("EncyclopediaHeroPagePlayAs", this.playAsVM);
                            this.gauntletLayerTopScreen = ScreenManager.TopScreen;
                            this.gauntletLayerTopScreen.AddLayer(this.gauntletLayer);
                            this.gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.MouseButtons);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
                InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Error));
            }
        }

        public void QueuePossession(Hero hero)
        {
            this.HeroToPossess = hero;
        }
        #endregion
    }
}