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

namespace BannerlordRestartPlus.Actions
{
    public class PlayAsCharacterAction
    {
        static PlayAsCharacterAction? _instance = null;
        internal static PlayAsCharacterAction Instance => (_instance ??= new PlayAsCharacterAction());

        static FieldInfo ActiveSaveSlotNameField = AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName");
        static MethodInfo GetNextAvailableSaveNameMethod = AccessTools.Method(typeof(MBSaveLoad), "GetNextAvailableSaveName");


        public static void Apply(Hero character)
        {
            if (character == Hero.MainHero)
            {
                TextObject textObject = new TextObject("{=restart_plus_n_13}RestartPlus: Already playing as {CHARACTER}");
                textObject.SetTextVariable("CHARACTER", character.Name);
                InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
                return;
            }

            CharacterCreationStateExtensions.CharacterCreationState = null;
            CharacterCreationStateExtensions.MapState = null;
            CharacterCreationStateExtensions.Position = null;

            CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(Instance, (bool isSaveSuccessful, string newSaveGameName) =>
            {
                Instance.ApplyInternal(character, isSaveSuccessful, newSaveGameName);
            });

            string saveName = (string) ActiveSaveSlotNameField.GetValue(null);
            if (saveName == null)
            {
                saveName = (string) GetNextAvailableSaveNameMethod.Invoke(null, new object[] { });
                ActiveSaveSlotNameField.SetValue(null, saveName);
            }
            Campaign.Current.SaveHandler.SaveAs(saveName + new TextObject("{=restart_plus_n_02} (auto)").ToString());
        }

        private PlayAsCharacterAction() { }
        private void ApplyInternal(Hero tempMain, bool isSaveSuccessful, string newSaveGameName)
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

            var mainPos = Hero.MainHero.GetPosition().AsVec2;

            var position = ChangePlayerCharacterInGameAction.Apply(tempMain);
            Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;

            if (Main.Settings!.PlaceExistingAtOldPosition && tempMain.PartyBelongedTo != null)
            {
                tempMain.PartyBelongedTo.Position2D = mainPos;
            }


            if (Main.Settings!.EditLooks)
            {
                FaceGen.ShowDebugValues = true;
                ScreenManager.PushScreen(new InGameBodyGeneratorScreen(tempMain.CharacterObject, false, null, () =>
                {
                    CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(RestartPlusAction.Instance, new Action<bool, string>(RestartPlusAction.Instance.LoadInternal));

                    Campaign.Current.SaveHandler.SaveAs(newSaveGameName.Replace(new TextObject("{=restart_plus_n_02} (auto)").ToString(), new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString()));
                }));

            }
            else
            {
                CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(RestartPlusAction.Instance, new Action<bool, string>(RestartPlusAction.Instance.LoadInternal));

                Campaign.Current.SaveHandler.SaveAs(newSaveGameName.Replace(new TextObject("{=restart_plus_n_02} (auto)").ToString(), new TextObject("{=restart_plus_n_03} (RestartPlus)").ToString()));
            }
        }
    }
}
