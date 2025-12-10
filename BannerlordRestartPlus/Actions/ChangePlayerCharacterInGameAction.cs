using System.Reflection;

using BannerlordRestartPlus.Patches;
using BannerlordRestartPlus.Saves;

using HarmonyLib;

using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

#if REQUIRE_RECREATE_OLD_PARTY
using TaleWorlds.CampaignSystem.Roster; 
#endif

namespace BannerlordRestartPlus.Actions
{
    public static class ChangePlayerCharacterInGameAction
    {

        static MethodInfo StartUpMethod = AccessTools.Method(typeof(MobileParty), "StartUp");
        static void StartUp(this MobileParty current) => StartUpMethod.Invoke(current, new object[] { });

        static MethodInfo SetMainPartyMethod = AccessTools.Property(typeof(Campaign), "MainParty").GetSetMethod(true);

        static void SetMainParty(this Campaign current, MobileParty party) => SetMainPartyMethod.Invoke(current, new object[] { party });


        static MethodInfo SetPlayerTraitDeveloperMethod = AccessTools.Property(typeof(Campaign), "PlayerTraitDeveloper").GetSetMethod(true);

        static void SetPlayerTraitDeveloper(this Campaign current, PropertyOwner<PropertyObject> value) => SetPlayerTraitDeveloperMethod.Invoke(current, new object[] { value });

        //static PropertyOwner<PropertyObject> CreateHeroTraitDeveloper(Hero? hero = null) => new PropertyOwner<PropertyObject>(); //(PropertyOwner<PropertyObject>) AccessTools.DeclaredConstructor(typeof(PropertyOwner<PropertyObject>), new[] { typeof(Hero) }).Invoke(new object[] { hero });
        //static LordPartyComponent CreateLordPartyComponent(Hero hero, Hero hero2) => (LordPartyComponent) AccessTools.DeclaredConstructor(typeof(LordPartyComponent), new[] { typeof(Hero), typeof(Hero) }).Invoke(new object[] { hero, hero2 });


        public static CampaignVec2 Apply(Hero newHero)
        {
            var oldHero = Hero.MainHero;
            var oldMainHero = Hero.FindFirst(h => h.StringId == "main_hero");

            CampaignVec2 position = oldHero.PartyBelongedTo.Position;
            var id = oldMainHero.StringId;
            var charId = oldMainHero.CharacterObject.StringId;
            var clanId = oldHero.Clan.StringId;
            var oldPartyId = oldHero.PartyBelongedTo.StringId;
            var oldCharacterSkills = HeroCreatorPatch.DefaultCharacterSkills.GetValue(oldHero.CharacterObject) as MBCharacterSkills;
            if (oldCharacterSkills == null)
            {
                oldCharacterSkills = MBObjectManager.Instance.CreateObject<MBCharacterSkills>((oldHero.CharacterObject as BasicCharacterObject).StringId);
                HeroCreatorPatch.DefaultCharacterSkills.SetValue(oldHero.CharacterObject, oldCharacterSkills);
            }
            var oldCharacterSkillsId = oldCharacterSkills.StringId;


            //Hero idGenerated = CreateHeroAction.Apply();

            //Campaign.Current.ObjectManager.UnregisterObject(oldHero.PartyBelongedTo);
            //oldHero.PartyBelongedTo.StringId = string.Concat("player_party_is_now_ai_", oldHero.CharacterObject.StringId, newHero.CharacterObject.StringId);
            //Campaign.Current.ObjectManager.UnregisterObject(oldHero.PartyBelongedTo);
            var replacementPartyId = string.Concat("player_party_is_now_ai_", oldHero.CharacterObject.StringId, newHero.CharacterObject.StringId);
            Campaign.Current.ObjectManager.UpdateRegistration(oldHero.PartyBelongedTo, replacementPartyId);
            if (oldMainHero != oldHero && oldMainHero.PartyBelongedTo != null)
            {
                Campaign.Current.ObjectManager.UpdateRegistration(oldMainHero.PartyBelongedTo, string.Concat(replacementPartyId, oldMainHero.CharacterObject.StringId));

            }

            var replacementHeroId = string.Concat("player_is_now_ai_", oldHero.StringId, newHero.StringId);
            Campaign.Current.ObjectManager.UpdateRegistration(oldHero, replacementHeroId);
            if (oldMainHero != oldHero)
            {
                Campaign.Current.ObjectManager.UpdateRegistration(oldMainHero, string.Concat(replacementHeroId, oldMainHero.StringId));
            }
            //oldHero.StringId = string.Concat("player_is_now_ai_", oldHero.StringId, newHero.StringId);
            //newHero.StringId = id;
            Campaign.Current.ObjectManager.UpdateRegistration(newHero, id);

            //Campaign.Current.ObjectManager.UnregisterObject(oldHero.CharacterObject);
            //oldHero.CharacterObject.StringId = string.Concat("player_char_is_now_ai_", oldHero.CharacterObject.StringId, newHero.CharacterObject.StringId);
            //Campaign.Current.ObjectManager.UnregisterObject(oldHero.CharacterObject);
            var replacementCharId = string.Concat("player_char_is_now_ai_", oldHero.CharacterObject.StringId, newHero.CharacterObject.StringId);
            Campaign.Current.ObjectManager.UpdateRegistration(oldHero.CharacterObject, replacementCharId);
            if (oldMainHero != oldHero)
            {
                Campaign.Current.ObjectManager.UpdateRegistration(oldMainHero.CharacterObject, string.Concat(replacementCharId, oldMainHero.CharacterObject.StringId));
            }

            //Campaign.Current.ObjectManager.UnregisterObject(newHero.CharacterObject);
            //newHero.CharacterObject.StringId = charId;
            //Campaign.Current.ObjectManager.RegisterObject<CharacterObject>(newHero.CharacterObject);
            Campaign.Current.ObjectManager.UpdateRegistration(newHero.CharacterObject, charId);


            //Campaign.Current.ObjectManager.RegisterObject<CharacterObject>(oldHero.CharacterObject);

            //Campaign.Current.ObjectManager.RegisterObject(oldHero.PartyBelongedTo);

            //oldHero.Clan.StringId = string.Concat("player_clan_now_ai_", oldHero.StringId, newHero.Clan.StringId);
            //newHero.Clan.StringId = clanId;
            var replacementClanId = string.Concat("player_clan_now_ai_", oldHero.StringId, newHero.Clan.StringId);
            Campaign.Current.ObjectManager.UpdateRegistration(oldHero.Clan, replacementClanId);
            if (oldMainHero != oldHero && oldMainHero.Clan != oldHero.Clan)
            {
                Campaign.Current.ObjectManager.UpdateRegistration(oldMainHero.Clan, string.Concat(replacementClanId, oldMainHero.Clan.StringId));
            }

            Campaign.Current.ObjectManager.UpdateRegistration(newHero.Clan, clanId);

            if (Hero.MainHero.CurrentSettlement != null && !Hero.MainHero.IsPrisoner)
            {
                if (Hero.MainHero.PartyBelongedTo != null)
                {
                    LeaveSettlementAction.ApplyForParty(Hero.MainHero.PartyBelongedTo);
                }
                else
                {
                    LeaveSettlementAction.ApplyForCharacterOnly(Hero.MainHero);
                }
            }

#if REQUIRE_DISBAND_ARMY
            if (MobileParty.MainParty.Army != null)
            {
                DisbandArmyAction.ApplyByUnknownReason(MobileParty.MainParty.Army);
            } 
#endif


            MobileParty oldMainParty = MobileParty.MainParty;

            // Actual change
            Game.Current.PlayerTroop = newHero.CharacterObject;

            Campaign.Current.InitializeSinglePlayerReferences();

            // TODO: Check main hero and player clan
            if (Game.Current.PlayerTroop != newHero.CharacterObject || Hero.MainHero != newHero || Clan.PlayerClan != newHero.Clan)
            {
                // TODO: Fix?
                Game.Current.PlayerTroop = newHero.CharacterObject;
            }

            // Update stuff because of change
            var newMainParty = newHero.PartyBelongedTo;

            var currentCampaign = Campaign.Current;

            Vec3 position3;
            bool isMainPartyChanged = false;
            if (oldMainParty != newMainParty)
            {
                isMainPartyChanged = true;
            }
            currentCampaign.SetMainParty(newMainParty);

            if (newHero.IsFugitive)
            {
                newHero.ChangeState(Hero.CharacterStates.Active);
            }

            PropertyOwner<PropertyObject> developer = new PropertyOwner<PropertyObject>();
            currentCampaign.SetPlayerTraitDeveloper(developer);
            if (currentCampaign.MainParty == null)
            {
                MobileParty party = MobileParty.CreateParty(string.Concat("player_party_", newHero.StringId), null);
                LordPartyComponent.ConvertPartyToLordParty(party, newHero, newHero);
                party.ActualClan = Clan.PlayerClan;
                currentCampaign.SetMainParty(party);
                isMainPartyChanged = true;
                if (!newHero.IsPrisoner)
                {
                    if (newHero.GetCampaignPosition().IsValid() && newHero.GetCampaignPosition().ToVec2() != Vec2.Zero)
                    {
                        position3 = newHero.GetPositionAsVec3();
                        position = newHero.GetCampaignPosition();
                    }
                    else
                    {
                        position = SettlementHelper.FindRandomSettlement((Settlement s) =>
                        {
                            if (s.OwnerClan == null)
                            {
                                return false;
                            }
                            return !s.OwnerClan.IsAtWarWith(Clan.PlayerClan);
                        }).Position;
                    }
                    Vec2 vec2 = position.ToVec2();

                    if (Main.Settings!.PlaceAtOldPosition)
                    {
                        position = oldMainParty.Position;
                        vec2 = position.ToVec2();
                    }

                    //PartyTemplateObject pt = currentCampaign.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template");
                    //var oldHeroStack = pt.Stacks.Find(s => s.Character == oldHero.CharacterObject)
                    //    old
                    currentCampaign.MainParty!.InitializeMobilePartyAtPosition(position);
                    currentCampaign.MainParty.IsActive = true;
                    currentCampaign.MainParty.MemberRoster.AddToCounts(Hero.MainHero.CharacterObject, 1, true, 0, 0, true, -1);
                }
                else
                {
                    MobileParty mainParty = currentCampaign.MainParty!;
                    //PartyTemplateObject obj = currentCampaign.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template");
                    position3 = Hero.MainHero.GetPositionAsVec3();
                    position = Hero.MainHero.GetCampaignPosition();
                    mainParty.InitializeMobilePartyAtPosition(position);
                    currentCampaign.MainParty!.IsActive = false;
                }
            }

            // Why?
            //Campaign.Current.MainParty.Ai.SetAsMainParty();


            PartyBase.MainParty.ItemRoster.UpdateVersion();
            PartyBase.MainParty.MemberRoster.UpdateVersion();
            if (MobileParty.MainParty.IsActive)
            {
                PartyBase.MainParty.SetAsCameraFollowParty();
            }
            PartyBase.MainParty.UpdateVisibilityAndInspected(position, 0f);
            if (Hero.MainHero.Mother != null)
            {
                Hero.MainHero.Mother.SetHasMet();
            }
            if (Hero.MainHero.Father != null)
            {
                Hero.MainHero.Father.SetHasMet();
            }
            currentCampaign.MainParty.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit);

            //newHero.PartyBelongedTo.StringId = oldPartyId;
            //Campaign.Current.ObjectManager.RegisterObject(newHero.PartyBelongedTo);
            Campaign.Current.ObjectManager.UpdateRegistration(newHero.PartyBelongedTo, oldPartyId);


            if (isMainPartyChanged)
            {
                oldMainParty.Ai.SetInitiative(1f, 1f, 24f);
                oldMainParty.Ai.EnableAi();
                oldMainParty.StartUp();
            }

#if REQUIRE_RECREATE_OLD_PARTY
            if (oldMainParty != MobileParty.MainParty && oldMainParty.IsActive)
            {
                var vec2 = oldMainParty.Position2D;

                oldMainParty.MemberRoster.RemoveTroop(oldHero.CharacterObject, 1, new UniqueTroopDescriptor(), 0);

                LordPartyComponent component = CreateLordPartyComponent(oldHero, oldHero);
                MobileParty newPartyForOldHero = MobileParty.CreateParty(string.Concat("player_party_now_ai_", oldHero.StringId), component, (MobileParty mobileParty) =>
                {
                    mobileParty.ActualClan = oldHero.Clan;
                });
                newPartyForOldHero!.InitializeMobilePartyAtPosition(TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), vec2);
                newPartyForOldHero.IsActive = true;
                newPartyForOldHero.MemberRoster.AddToCounts(oldHero.CharacterObject, 1, true, 0, 0, true, -1);


                for (int j = oldMainParty.MemberRoster.Count - 1; j >= 0; j--)
                {
                    TroopRosterElement elementCopyAtIndex = oldMainParty.MemberRoster.GetElementCopyAtIndex(j);
                    oldMainParty.MemberRoster.RemoveTroop(elementCopyAtIndex.Character, elementCopyAtIndex.Number);
                    newPartyForOldHero.MemberRoster.Add(elementCopyAtIndex);
                }

                DestroyPartyAction.Apply(null, oldMainParty);
            } 
#endif

            if (newHero.IsPrisoner)
            {
                PlayerCaptivity.OnPlayerCharacterChanged();
            }

            PartyBase.MainParty.SetVisualAsDirty();
            Campaign.Current.MainHeroIllDays = -1;

            Hero.MainHero.SetHasMet();

            Campaign.Current.LogEntryHistory.DeleteOutdatedLogs();
            Campaign.Current.LogEntryHistory.GameActionLogs.Clear();
            Campaign.Current.InitializeSinglePlayerReferences();

            Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;

            PreviousPlayerCharacters.Instance?.CharacterChanged(oldHero);
            return position;
        }
    }
}
