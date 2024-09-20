using System;
using System.Linq;
using System.Reflection;

using BannerlordRestartPlus.Patches.Runtime;

using HarmonyLib;

using BannerlordRestartPlus.UI;

using SandBox;

using StoryMode;
using StoryMode.CharacterCreationContent;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.Quests.PlayerClanQuests;
using StoryMode.Quests.SecondPhase;
using StoryMode.Quests.ThirdPhase;
using StoryMode.StoryModeObjects;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.ScreenSystem;

using FaceGen = TaleWorlds.Core.FaceGen;
using BannerlordRestartPlus.Behaviours;

namespace BannerlordRestartPlus.Actions
{

    public class RestartPlusAction
    {
        static RestartPlusAction? _instance = null;
        internal static RestartPlusAction Instance => (_instance ??= new RestartPlusAction());

        static FieldInfo ActiveSaveSlotNameField = AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName");
        static MethodInfo GetNextAvailableSaveNameMethod = AccessTools.Method(typeof(MBSaveLoad), "GetNextAvailableSaveName");


        public static void Apply()
        {
            CharacterCreationStateExtensions.CharacterCreationState = null;
            CharacterCreationStateExtensions.MapState = null;
            CharacterCreationStateExtensions.Position = null;

            CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(Instance, new Action<bool, string>(Instance.ApplyInternal));

            string saveName = (string) ActiveSaveSlotNameField.GetValue(null);
            if (saveName == null)
            {
                saveName = (string) GetNextAvailableSaveNameMethod.Invoke(null, new object[] { });
                ActiveSaveSlotNameField.SetValue(null, saveName);
            }
            Campaign.Current.SaveHandler.SaveAs(saveName + new TextObject("{=restart_plus_n_02} (auto)").ToString());
        }

        private RestartPlusAction() { }
        private void ApplyInternal(bool isSaveSuccessful, string newSaveGameName)
        {
            CampaignEvents.OnSaveOverEvent.ClearListeners(this);

            if (!isSaveSuccessful)
            {
                Debug.WriteDebugLineOnScreen(new TextObject("{=restart_plus_n_04}RestartPlus: Could not save").ToString());
                return;
            }

            bool isStoryMode = Game.Current?.GameType is CampaignStoryMode;
            if (isStoryMode)
            {
                var campaignGameStarter = SandBoxManager.Instance?.GameStarter;
                if (StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted && campaignGameStarter != null)
                {

                    // Prevent new character from being spawned at training field
                    var trainingFieldBehaviour = campaignGameStarter.CampaignBehaviors.FirstOrDefault(b => b is TrainingFieldCampaignBehavior) as TrainingFieldCampaignBehavior;
                    CampaignEvents.OnSessionLaunchedEvent.ClearListeners(trainingFieldBehaviour);
                    CampaignEvents.OnMissionEndedEvent.ClearListeners(trainingFieldBehaviour);
                    CampaignEvents.OnCharacterCreationIsOverEvent.ClearListeners(trainingFieldBehaviour);

                    StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();

                    campaignGameStarter.RemoveBehaviors<LordConversationsStoryModeBehavior>();
                    campaignGameStarter.RemoveBehaviors<MainStorylineCampaignBehavior>();
                    campaignGameStarter.RemoveBehaviors<TutorialPhaseCampaignBehavior>();
                    campaignGameStarter.RemoveBehaviors<FirstPhaseCampaignBehavior>();
                    campaignGameStarter.RemoveBehaviors<SecondPhaseCampaignBehavior>();
                    campaignGameStarter.RemoveBehaviors<ThirdPhaseCampaignBehavior>();
                    campaignGameStarter.RemoveBehaviors<TrainingFieldCampaignBehavior>();
                    campaignGameStarter.RemoveBehaviors<StoryModeTutorialBoxCampaignBehavior>();

                    campaignGameStarter.RemoveBehaviors<WeakenEmpireQuestBehavior>();
                    campaignGameStarter.RemoveBehaviors<AssembleEmpireQuestBehavior>();

                    campaignGameStarter.RemoveBehaviors<DefeatTheConspiracyQuestBehavior>();

                    campaignGameStarter.RemoveBehaviors<RescueFamilyQuestBehavior>();
                } else
                {
                    return;
                }
            }


            Hero tempMain = CreateHeroAction.Apply(requireFamily: isStoryMode);

            if (isStoryMode)
            {
                //var _elderBrotherField = AccessTools.Field(typeof(StoryModeHeroes), "_elderBrother");
                //var _littleBrotherField = AccessTools.Field(typeof(StoryModeHeroes), "_littleBrother");
                //var _littleSisterField = AccessTools.Field(typeof(StoryModeHeroes), "_littleSister");
                //var inst = StoryModeManager.Current.StoryModeHeroes;
                //var elderBro = tempMain.Siblings.FirstOrDefault();
                //var previousElderBro = StoryModeHeroes.ElderBrother;
                //var previousId = previousElderBro.StringId;
                //previousElderBro.StringId = elderBro.StringId ?? "bro_previous";
                var existingElderBrother = StoryModeHeroes.ElderBrother;  //(Hero) _elderBrotherField.GetValue(inst);
                if (existingElderBrother != null)
                {
                    tempMain.Father = existingElderBrother.Father;
                    tempMain.Mother = existingElderBrother.Mother;
                }
                //var createdBro = elderBro ?? StoryModeHeroes.ElderBrother;
                //createdBro.StringId = previousId;
                //createdBro.Father = tempMain.Father;
                //createdBro.Mother = tempMain.Mother;
                //createdBro.Clan = tempMain.Clan;
                //_elderBrotherField.SetValue(inst,createdBro);

                //if (_littleBrotherField.GetValue(inst) is Hero littleBro)
                //{
                //    littleBro.Father = tempMain.Father;
                //    littleBro.Mother = tempMain.Mother;
                //    littleBro.Clan = tempMain.Clan;
                //}

                //if (_littleSisterField.GetValue(inst) is Hero littleSis)
                //{
                //    littleSis.Father = tempMain.Father;
                //    littleSis.Mother = tempMain.Mother;
                //    littleSis.Clan = tempMain.Clan;
                //}
            }


            var visualTrackingBehaviour = SandBoxManager.Instance?.GameStarter?.CampaignBehaviors?.FirstOrDefault(b => b is ViewDataTrackerCampaignBehavior) as ViewDataTrackerCampaignBehavior;


            while (Campaign.Current.QuestManager.Quests.Count > 0)
            {
                var quest = Campaign.Current.QuestManager.Quests.FirstOrDefault();


                quest.CompleteQuestWithCancel(null);

                if (visualTrackingBehaviour != null)
                {
                    quest.JournalEntries?.ForEach(l => visualTrackingBehaviour.OnQuestLogExamined(l));
                }
                quest.JournalEntries?.RemoveAll(j => true);
            }

            Campaign.Current.LogEntryHistory.DeleteOutdatedLogs();
            Campaign.Current.LogEntryHistory.GameActionLogs.Clear();

            var position = ChangePlayerCharacterInGameAction.Apply(tempMain);
            Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;

            //while (Campaign.Current.IssueManager.Issues.Count > 0)
            //{
            //    var issue = Campaign.Current.IssueManager.Issues.FirstOrDefault();
            //    Campaign.Current.IssueManager.DeactivateIssue(issue.Value);
            //}

            if (Main.Settings!.CharacterCreation)
            {
                var cccb = CharacterCreationContentBase.Instance;
                if (cccb == null)
                {
                    cccb = GetCharacterCreationContent();
                }

                var active = Game.Current!.GameStateManager.ActiveState;
                CharacterCreationStateExtensions.Position = position;
                if (active is MapState ms)
                {
                    CharacterCreationStateExtensions.MapState = ms;
                }

                CharacterCreationState gameState = Game.Current.GameStateManager.CreateState<CharacterCreationState>(new object[] { cccb });
                CharacterCreationStateExtensions.CharacterCreationState = gameState;
                Game.Current!.GameStateManager.PushState(gameState, 0);
            }
            else if (Main.Settings.EditLooks)
            {
                FaceGen.ShowDebugValues = true;
                ScreenManager.PushScreen(new InGameBodyGeneratorScreen(tempMain.CharacterObject, false, null, () =>
                {
                    CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool, string>(this.LoadInternal));

                    Campaign.Current.SaveHandler.SaveAs(newSaveGameName.Replace(new TextObject("{=restart_plus_n_02} (auto)").ToString(), new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString()));
                }));

            }
            else
            {
                CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool, string>(this.LoadInternal));

                Campaign.Current.SaveHandler.SaveAs(newSaveGameName.Replace(new TextObject("{=restart_plus_n_02} (auto)").ToString(), new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString()));
            }
        }


        internal void LoadInternal(bool isSaveSuccessful, string newSaveGameName)
        {
            CampaignEvents.OnSaveOverEvent.ClearListeners(this);

            RestartPlusBehaviour.SuggestGameRestart = true;
            RestartPlusBehaviour.CheckRestart(onCancel: () =>
            {

                if (!isSaveSuccessful)
                {
                    Debug.WriteDebugLineOnScreen(new TextObject("{=restart_plus_n_04}RestartPlus: Could not save").ToString());
                    return;
                }
                SaveGameFileInfo saveFileWithName = MBSaveLoad.GetSaveFileWithName(newSaveGameName);
                if (saveFileWithName != null && !saveFileWithName.IsCorrupted)
                {
                    SandBoxSaveHelper.TryLoadSave(saveFileWithName, new Action<LoadResult>(this.StartGame), null);
                    return;
                }
                InformationManager.ShowInquiry(new InquiryData((new TextObject("{=oZrVNUOk}Error", null)).ToString(), (new TextObject("{=t6W3UjG0}Save game file appear to be corrupted. Try starting a new campaign or load another one from Saved Games menu.", null)).ToString(), true, false, (new TextObject("{=yS7PvrTD}OK", null)).ToString(), null, null, null, "", 0f, null, null, null), false, false);
            });
        }

        public void StartGame(LoadResult loadResult)
        {
            if (Game.Current != null)
            {
                ScreenManager.PopScreen();
                GameStateManager.Current.CleanStates(0);
                GameStateManager.Current = TaleWorlds.MountAndBlade.Module.CurrentModule.GlobalGameStateManager;
            }
            MBSaveLoad.OnStartGame(loadResult);
            MBGameManager.StartNewGame(new SandBoxGameManager(loadResult));
        }

        private static CharacterCreationContentBase GetCharacterCreationContent()
        {
            var baseType = typeof(CharacterCreationContentBase);
            var excludedTypes = new[] { typeof(SandboxCharacterCreationContent), typeof(StoryModeCharacterCreationContent) };

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    try
                    {
                        if (type.IsSubclassOf(baseType))
                        {

                            var cccbType = type;
                            if (cccbType != null && !excludedTypes.Contains(cccbType))
                            {
                                var cccb = cccbType.CreateInstance();
                                if (cccb is CharacterCreationContentBase ccc)
                                {
                                    return ccc;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteDebugLineOnScreen(e.ToString());
                    }
                }
            }

            if (Game.Current!.GameType is CampaignStoryMode)
            {
                return new StoryModeCharacterCreationContent();
            }

            return new SandboxCharacterCreationContent();
        }
    }
}
