
using System;

using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace BannerlordRestartPlus.UI
{
    [OverrideView(typeof(FaceGeneratorScreen))]
    public class InGameBodyGeneratorScreen : ScreenBase, IFaceGeneratorScreen
    {
        private const int ViewOrderPriority = 15;

        private readonly BodyGeneratorView _facegenLayer;
        private readonly Action? onComplete;

        public IFaceGeneratorHandler Handler
        {
            get
            {
                return this._facegenLayer;
            }
        }

        public InGameBodyGeneratorScreen(BasicCharacterObject character, bool openedFromMultiplayer, IFaceGeneratorCustomFilter? filter, Action? onComplete = null)
        {
            this._facegenLayer = new BodyGeneratorView(new ControlCharacterCreationStage(this.OnExit), GameTexts.FindText("str_done", null), new ControlCharacterCreationStage(this.OnExit), GameTexts.FindText("str_cancel", null), character, openedFromMultiplayer, filter, null, null, null, null, null);
            this.onComplete = onComplete;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            base.AddLayer(this._facegenLayer.SceneLayer);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            this._facegenLayer.OnFinalize();
            LoadingWindow.EnableGlobalLoadingWindow();
            MBInformationManager.HideInformations();
            Mission current = Mission.Current;
            if (current != null)
            {
                foreach (Agent agent in current.Agents)
                {
                    agent.EquipItemsFromSpawnEquipment(false);
                    agent.UpdateAgentProperties();
                }
            }
        }

        public void OnExit()
        {
            ScreenManager.PopScreen();
            this.onComplete?.Invoke();
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            if (LoadingWindow.GetGlobalLoadingWindowState())
            {
                LoadingWindow.DisableGlobalLoadingWindow();
            }
            Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            this._facegenLayer.OnTick(dt);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
            base.AddLayer(this._facegenLayer.GauntletLayer);
        }
    }
}
