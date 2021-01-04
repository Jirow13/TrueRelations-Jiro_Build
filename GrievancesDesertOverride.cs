using System;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace TrueRelations
{
	[HarmonyPatch(typeof(CompanionGrievanceBehavior), "OnPlayerDesertedBattle")]
	internal class GrievancesDesertOverride
	{
		public static bool Prefix(int sacrificedMenCount)
		{
			bool flag = sacrificedMenCount >= 1;
			if (flag)
			{
				Support.LogMessage("The troops left behind march bravely to their fates.");
				bool player_trait_skill_gain_enabled = Support.settings.player_trait_skill_gain_enabled;
				if (player_trait_skill_gain_enabled)
				{
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -(3 * sacrificedMenCount));
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -sacrificedMenCount);
				}
				List<Hero> heroesInMainParty = Support.GetHeroesInMainParty();
				int num = sacrificedMenCount / 5;
				bool flag2 = num < 2;
				if (flag2)
				{
					num = 2;
				}
				int num2 = num / 2;
				for (int i = 0; i < heroesInMainParty.Count; i++)
				{
					bool flag3 = (Support.settings.battle_companions_bad_reputation_enabled && heroesInMainParty[i].IsPlayerCompanion) || (Support.settings.battle_family_bad_reputation_enabled && !heroesInMainParty[i].IsPlayerCompanion);
					if (flag3)
					{
						CharacterTraits heroTraits = heroesInMainParty[i].GetHeroTraits();
						int num3 = Support.EvaluatePersonality(heroTraits);
						int num4 = num3;
						if (num4 != 1)
						{
							if (num4 == 4)
							{
								Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Random(-num2, 0));
							}
						}
						else
						{
							Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Random(-num, 0));
						}
					}
				}
			}
			return false;
		}
	}
}
