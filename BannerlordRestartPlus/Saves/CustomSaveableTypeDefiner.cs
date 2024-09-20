using System.Collections.Generic;

using TaleWorlds.SaveSystem;

namespace BannerlordRestartPlus.Saves
{
    internal sealed class CustomSaveableTypeDefiner : SaveableTypeDefiner
    {
        public const int SaveBaseId_b0tlanner0 = 300_711_200;
        public const int SaveBaseId = SaveBaseId_b0tlanner0 + 2;

        public CustomSaveableTypeDefiner() : base(SaveBaseId) { }

        protected override void DefineClassTypes()
        {
            base.DefineClassTypes();
            AddClassDefinition(typeof(PreviousPlayerCharacters), 1);
            AddClassDefinition(typeof(PlayerCharacter), 2);
        }

        protected override void DefineContainerDefinitions()
        {
            base.DefineContainerDefinitions();
            ConstructContainerDefinition(typeof(List<PlayerCharacter>));
        }
    }
}
