using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordRestartPlus.Actions
{
    public static class CreateHeroAction
    {
        public static Hero Apply(bool requireFamily = false)
        {
            Kingdom kingdom = Kingdom.All.Where(k => k.Culture != null).ToList().GetRandomElement<Kingdom>();
            CultureObject culture = kingdom.Culture;
            Settlement settlement = kingdom.Settlements.FirstOrDefault<Settlement>((Settlement x) => x.IsTown) ?? kingdom.Settlements.GetRandomElement<Settlement>() ?? Settlement.All.GetRandomElement<Settlement>();
            TextObject textObject = NameGenerator.Current.GenerateClanName(culture, settlement);
            TextObject textObject2 = NameGenerator.Current.GenerateClanName(culture, settlement);
            textObject = new TextObject(textObject.ToString() + " " + textObject2.ToString());
            Clan clan = Clan.CreateClan($"no_clan_{Clan.All.Count}");
            TextObject textObject1 = new TextObject("{=!}informal", null);
            CultureObject cultureObject = Kingdom.All.Where(k => k.Culture != null).ToList().GetRandomElement<Kingdom>().Culture;
            Banner banner = Banner.CreateRandomClanBanner(-1);
            Vec2 vec2 = new Vec2();
            clan.InitializeClan(textObject, textObject1, cultureObject, banner, vec2, false);
            CharacterObject characterObject = culture.LordTemplates.FirstOrDefault<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.Lord);
            Settlement randomElement = kingdom.Settlements.GetRandomElement<Settlement>();
            var hero = HeroCreator.CreateSpecialHero(characterObject ?? kingdom.Leader.CharacterObject, randomElement, age: MBRandom.RandomInt(18, 36));
            hero.ChangeState(Hero.CharacterStates.Active);
            clan.SetLeader(hero);
            if (clan.HomeSettlement == null)
            {
                clan.UpdateHomeSettlement(hero.HomeSettlement ?? hero.BornSettlement ?? hero.CurrentSettlement ?? settlement);
            }

            if (requireFamily)
            {
                CharacterObject fatherObject = culture.LordTemplates.FirstOrDefault<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.Lord && !x.IsFemale);
                var father = HeroCreator.CreateSpecialHero(fatherObject ?? kingdom.Leader.CharacterObject, randomElement, age: 70);
                father.Clan = clan;
                hero.Father = father;

                CharacterObject motherObject = culture.LordTemplates.FirstOrDefault<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.Lord && x.IsFemale);
                var mother = HeroCreator.CreateSpecialHero(motherObject ?? kingdom.Leader.CharacterObject, randomElement, age: 69);
                mother.Clan = clan;
                hero.Mother = mother;

                //var brother = HeroCreator.CreateSpecialHero(father.CharacterObject, randomElement, age: 30);
                //brother.Clan = clan;
                //brother.Father = father;
                //brother.Mother = mother;


                KillCharacterAction.ApplyByDeathMarkForced(father, false);
                KillCharacterAction.ApplyByDeathMarkForced(mother, false);
            }

            return hero;
        }
    }
}
