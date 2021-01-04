using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.SandBox.Source.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TrueRelations
{
	//[HarmonyPatch(typeof(TournamentCampaignBehavior), "OnTournamentWon")]
	[HarmonyPatch(typeof(TournamentCampaignBehavior), "OnTournamentFinished", MethodType.Constructor)]
	internal class TournamentEndRelations
	{
		public static void Postfix(CharacterObject winner, Town town)
		{
			InformationManager.DisplayMessage(new InformationMessage("Triggered OnTournamentFinished", Color.ConvertStringToColor("#FF0042FF")));
			bool flag = winner != null && town != null;
			if (flag)
			{
				bool flag2 = winner.IsHero && winner.HeroObject == Hero.MainHero && Support.settings.tournament_bonus_enabled;
				if (flag2)
				{
					int num = 1;
					int num2 = 4;
					bool flag3 = town.Owner != null;
					if (flag3)
					{
						Hero hero = Support.FindHero(town.Owner);
						bool flag4 = hero != null;
						if (flag4)
						{
							CharacterTraits heroTraits = hero.GetHeroTraits();
							bool flag5 = heroTraits.Honor > 0;
							if (flag5)
							{
								num++;
								num2++;
							}
							bool flag6 = heroTraits.Valor > 0;
							if (flag6)
							{
								num++;
								num2++;
							}
							bool flag7 = heroTraits.Generosity > 0;
							if (flag7)
							{
								int num3 = Support.Random(100, 1000);
								bool flag8 = hero.Gold >= num3 * 3;
								if (flag8)
								{
									GiveGoldAction.ApplyBetweenCharacters(hero, Hero.MainHero, num3, false);
									Support.LogFriendlyMessage(hero.Name.ToString() + " has sent you a pouch of gold to congratulate you on your triumph.");
								}
							}
							Support.ChangeRelation(Hero.MainHero, hero, Support.Random(num, num2));
							Support.ChangeFamilyRelation(Hero.MainHero, hero, num2 / 3, 0);
						}
					}
					bool flag9 = town.Settlement != null;
					if (flag9)
					{
						bool flag10 = town.Settlement.Notables != null;
						if (flag10)
						{
							bool flag11 = town.Settlement.Notables.Any<Hero>();
							if (flag11)
							{
								for (int i = 0; i < 3; i++)
								{
									Support.ChangeRelation(Hero.MainHero, town.Settlement.Notables.GetRandomElement<Hero>(), Support.Chance(1, 2));
								}
							}
						}
					}
					List<Hero> heroesInMainParty = Support.GetHeroesInMainParty();
					for (int j = 0; j < heroesInMainParty.Count; j++)
					{
						bool flag12 = (Support.settings.battle_companions_good_reputation_enabled && heroesInMainParty[j].IsPlayerCompanion) || (Support.settings.battle_leaders_good_reputation_enabled && !heroesInMainParty[j].IsPlayerCompanion);
						if (flag12)
						{
							num = 0;
							num2 = 1;
							bool flag13 = heroesInMainParty[j].GetHeroTraits().Valor > 0;
							if (flag13)
							{
								num2++;
							}
							bool flag14 = heroesInMainParty[j].GetHeroTraits().Honor > 0;
							if (flag14)
							{
								num2++;
							}
						}
						Support.ChangeRelation(Hero.MainHero, heroesInMainParty[j], Support.Chance(num, num2));
					}
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, Support.Random(10, 40));
					InformationManager.DisplayMessage(new InformationMessage("Triggered AddValor & Charm", Color.ConvertStringToColor("#FF0042FF")));
					Hero.MainHero.AddSkillXp(DefaultSkills.Charm, (float)Support.Random(20, 100));
					Support.LogMessage(Hero.MainHero.Name.ToString() + " has shown great valor in the tournament.");
				}
			}

			InformationManager.DisplayMessage(new InformationMessage("Triggered EndPostFix", Color.ConvertStringToColor("#FF0042FF")));
		}

		static bool Prepare() { return true; }	
	}
}
