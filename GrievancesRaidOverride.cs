using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace TrueRelations
{
	[HarmonyPatch(typeof(CompanionGrievanceBehavior), "OnVillageRaided")]
	internal class GrievancesRaidOverride
	{
		public static bool Prefix(Village village)
		{
			return false;
		}
	}
}
