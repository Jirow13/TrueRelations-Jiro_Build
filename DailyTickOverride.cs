using System;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace TrueRelations
{
	[HarmonyPatch(typeof(CompanionGrievanceBehavior), "OnDailyTick")]
	internal class DailyTickOverride
	{
		public static bool Prefix()
		{
			bool flag = MobileParty.MainParty != null;
			if (flag)
			{
				bool flag2 = MobileParty.MainParty.Party != null;
				if (flag2)
				{
					bool flag3 = !CompanionBanter.CheckForConfrontation();
					if (flag3)
					{
						bool flag4 = false;
						bool flag5 = false;
						bool isStarving = PartyBase.MainParty.IsStarving;
						if (isStarving)
						{
							Support.LogMessage("Your soldiers are starving.");
							flag4 = true;
							bool player_trait_skill_gain_enabled = Support.settings.player_trait_skill_gain_enabled;
							if (player_trait_skill_gain_enabled)
							{
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -MobileParty.MainParty.Party.NumberOfAllMembers);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -MobileParty.MainParty.Party.NumberOfAllMembers);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -MobileParty.MainParty.Party.NumberOfAllMembers);
							}
						}
						bool flag6 = (double)MobileParty.MainParty.HasUnpaidWages > 0.0;
						if (flag6)
						{
							Support.LogMessage("With no pay in sight, the men speak of desertion.");
							flag5 = true;
							bool player_trait_skill_gain_enabled2 = Support.settings.player_trait_skill_gain_enabled;
							if (player_trait_skill_gain_enabled2)
							{
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -(2 * MobileParty.MainParty.Party.NumberOfAllMembers));
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -MobileParty.MainParty.Party.NumberOfAllMembers);
							}
						}
						bool flag7 = flag4 || flag5;
						if (flag7)
						{
							List<Hero> heroesInMainParty = Support.GetHeroesInMainParty();
							for (int i = 0; i < heroesInMainParty.Count; i++)
							{
								bool flag8 = (Support.settings.battle_companions_bad_reputation_enabled && heroesInMainParty[i].IsPlayerCompanion) || (Support.settings.battle_family_bad_reputation_enabled && !heroesInMainParty[i].IsPlayerCompanion);
								if (flag8)
								{
									CharacterTraits heroTraits = heroesInMainParty[i].GetHeroTraits();
									bool flag9 = flag4;
									if (flag9)
									{
										switch (Support.EvaluatePersonality(heroTraits))
										{
										case 1:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Random(-2, 0));
											break;
										case 2:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Random(-4, -1));
											break;
										case 3:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Random(-3, 0));
											break;
										default:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Chance(-2, -1));
											break;
										}
									}
									bool flag10 = flag5;
									if (flag10)
									{
										switch (Support.EvaluatePersonality(heroTraits))
										{
										case 1:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Chance(-1, 0));
											break;
										case 2:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Random(-7, -3));
											break;
										case 3:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Chance(-4, -1));
											break;
										default:
											Support.ChangeRelation(Hero.MainHero, heroesInMainParty[i], Support.Chance(-2, -1));
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
	}
}
