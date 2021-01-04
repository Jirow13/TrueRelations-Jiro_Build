using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace TrueRelations
{
	[HarmonyPatch(typeof(CampaignMapConversation), "OpenConversation")]
	internal class CampaignMapConversationClear
	{
		public static void Postfix()
		{
			CompanionBanter.banterID = 0;
		}
	}
}
