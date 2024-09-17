using HarmonyLib;

namespace BannerlordRestartPlus.Patches.Optional
{
    public interface IOptionalPatch
    {
        public bool TryPatch(Harmony harmony);

        public bool MenusInitialised(Harmony harmony);
    }
}
