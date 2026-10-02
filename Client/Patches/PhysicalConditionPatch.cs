using System.Reflection;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace MakeMedsGreatAgain.Patches
{
    /// <summary>
    /// Stops the local player from getting the movement restrictions of healing:
    /// HealingLegs (surgery, can't walk) and UsingMeds (can't sprint).
    /// </summary>
    internal class PhysicalConditionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MovementContext), nameof(MovementContext.SetPhysicalCondition));
        }

        [PatchPrefix]
        private static bool Prefix(EPhysicalCondition c, ref bool __result, Player ____player)
        {
            if (____player == null || !____player.IsYourPlayer || ____player is HideoutPlayer)
            {
                return true;
            }

            var skip = (c == EPhysicalCondition.HealingLegs && Plugin.CanWalkInSurgery.Value)
                || (c == EPhysicalCondition.UsingMeds && Plugin.CanSprintUsingMeds.Value);

            if (skip)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
