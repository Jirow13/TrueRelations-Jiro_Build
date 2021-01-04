using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace TrueRelations
{
	[HarmonyPatch(typeof(CompanionGrievanceBehavior), "OnHourlyTick")]
	internal class HourlyTickOverride
	{
		public static bool Prefix()
		{
			CompanionBanter.banterID = 0;
			CompanionBanter.subBanterID = 0;
			return false;
		}
	}
}
