using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TrueRelations
{
	public static class Support
	{
		public static void ChangeRelation(Hero lord1, Hero lord2, int change)
		{
			bool flag = lord1 != null && lord2 != null;
			if (flag)
			{
				int num = CharacterRelationManager.GetHeroRelation(lord1, lord2) + change;
				bool flag2 = num > 100;
				if (flag2)
				{
					num = 100;
				}
				else
				{
					bool flag3 = num < -100;
					if (flag3)
					{
						num = -100;
					}
				}
				CharacterRelationManager.SetHeroRelation(lord1, lord2, num);
			}
		}

		public static void ChangeFamilyRelation(Hero lord1, Hero lord2, int change, int offset = 0)
		{
			List<Hero> list = Support.FindFamily(lord2);
			for (int i = 0; i < list.Count; i++)
			{
				Support.ChangeRelation(lord1, list[i], Support.Random(change, change + offset));
			}
		}

		public static void ChangeFactionRelation(Hero lord1, Hero lord2, int change, int offset = 0)
		{
			List<Hero> list = Support.FindFaction(lord2);
			for (int i = 0; i < list.Count; i++)
			{
				Support.ChangeRelation(lord1, list[i], Support.Random(change, change + offset));
			}
		}

		public static Hero FindHero(PartyBase party)
		{
			bool flag = false;
			Hero result = null;
			bool flag2 = party.Owner != null;
			if (flag2)
			{
				result = party.Owner;
				flag = true;
			}
			bool flag3 = !flag;
			if (flag3)
			{
				bool flag4 = party.Leader != null;
				if (flag4)
				{
					bool flag5 = party.Leader.HeroObject != null;
					if (flag5)
					{
						result = party.Leader.HeroObject;
						flag = true;
					}
				}
			}
			bool flag6 = !flag;
			if (flag6)
			{
				bool flag7 = party.LeaderHero != null;
				if (flag7)
				{
					result = party.LeaderHero;
				}
			}
			return result;
		}

		public static List<Hero> FindFamily(Hero hero)
		{
			List<Hero> list = new List<Hero>();
			bool flag = hero != null;
			if (flag)
			{
				for (int i = 0; i < hero.Children.Count; i++)
				{
					Hero hero2 = hero.Children[i];
					bool flag2 = hero2 != null;
					if (flag2)
					{
						bool isAlive = hero2.IsAlive;
						if (isAlive)
						{
							list.Add(hero2);
						}
					}
				}
				for (int j = 0; j < hero.Siblings.Count<Hero>(); j++)
				{
					Hero hero2 = hero.Siblings.ElementAt(j);
					bool flag3 = hero2 != null;
					if (flag3)
					{
						bool isAlive2 = hero2.IsAlive;
						if (isAlive2)
						{
							list.Add(hero2);
						}
					}
				}
				bool flag4 = hero.Spouse != null;
				if (flag4)
				{
					bool isAlive3 = hero.Spouse.IsAlive;
					if (isAlive3)
					{
						list.Add(hero.Spouse);
					}
				}
				bool flag5 = hero.Father != null;
				if (flag5)
				{
					bool isAlive4 = hero.Father.IsAlive;
					if (isAlive4)
					{
						list.Add(hero.Father);
					}
				}
				bool flag6 = hero.Mother != null;
				if (flag6)
				{
					bool isAlive5 = hero.Mother.IsAlive;
					if (isAlive5)
					{
						list.Add(hero.Mother);
					}
				}
			}
			return list;
		}

		public static List<Hero> FindFaction(Hero hero)
		{
			List<Hero> list = new List<Hero>();
			bool flag = hero != null;
			if (flag)
			{
				bool flag2 = hero.MapFaction != null;
				if (flag2)
				{
					bool flag3 = hero.MapFaction.Heroes != null;
					if (flag3)
					{
						for (int i = 0; i < hero.MapFaction.Heroes.Count<Hero>(); i++)
						{
							Hero hero2 = hero.MapFaction.Heroes.ElementAt(i);
							bool flag4 = hero2 != null;
							if (flag4)
							{
								bool isAlive = hero2.IsAlive;
								if (isAlive)
								{
									list.Add(hero2);
								}
							}
						}
					}
				}
			}
			return list;
		}

		public static List<Hero> GetHeroesInMainParty()
		{
			List<Hero> list = new List<Hero>();
			bool flag = Hero.MainHero.CompanionsInParty != null;
			if (flag)
			{
				for (int i = 0; i < Hero.MainHero.CompanionsInParty.Count<Hero>(); i++)
				{
					Hero hero = Hero.MainHero.CompanionsInParty.ElementAt(i);
					bool flag2 = hero != null;
					if (flag2)
					{
						bool flag3 = hero.IsAlive && hero.PartyBelongedTo == MobileParty.MainParty && !hero.IsPrisoner;
						if (flag3)
						{
							list.Add(hero);
						}
					}
				}
			}
			bool flag4 = Hero.MainHero.Children != null;
			if (flag4)
			{
				for (int j = 0; j < Hero.MainHero.Children.Count<Hero>(); j++)
				{
					Hero hero = Hero.MainHero.Children.ElementAt(j);
					bool flag5 = hero != null;
					if (flag5)
					{
						bool flag6 = hero.IsAlive && hero.PartyBelongedTo == MobileParty.MainParty && !hero.IsPrisoner;
						if (flag6)
						{
							list.Add(hero);
						}
					}
				}
			}
			bool flag7 = Hero.MainHero.Siblings != null;
			if (flag7)
			{
				for (int k = 0; k < Hero.MainHero.Siblings.Count<Hero>(); k++)
				{
					Hero hero = Hero.MainHero.Siblings.ElementAt(k);
					bool flag8 = hero != null;
					if (flag8)
					{
						bool flag9 = hero.IsAlive && hero.PartyBelongedTo == MobileParty.MainParty && !hero.IsPrisoner;
						if (flag9)
						{
							list.Add(hero);
						}
					}
				}
			}
			bool flag10 = Hero.MainHero.Spouse != null;
			if (flag10)
			{
				Hero hero = Hero.MainHero.Spouse;
				bool flag11 = hero.IsAlive && hero.PartyBelongedTo == MobileParty.MainParty && !hero.IsPrisoner;
				if (flag11)
				{
					list.Add(hero);
				}
			}
			return list;
		}

		public static int EvaluatePersonality(CharacterTraits traits)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			bool flag = traits.Honor > 0;
			if (flag)
			{
				num++;
			}
			else
			{
				bool flag2 = traits.Honor < 0;
				if (flag2)
				{
					num3++;
				}
			}
			bool flag3 = traits.Valor > 0;
			if (flag3)
			{
				num++;
			}
			else
			{
				bool flag4 = traits.Valor < 0;
				if (flag4)
				{
					num2++;
				}
			}
			bool flag5 = traits.Generosity > 0;
			if (flag5)
			{
				num4++;
			}
			else
			{
				bool flag6 = traits.Generosity < 0;
				if (flag6)
				{
					num2++;
				}
			}
			bool flag7 = traits.Mercy > 0;
			if (flag7)
			{
				num4++;
			}
			else
			{
				bool flag8 = traits.Mercy < 0;
				if (flag8)
				{
					num3++;
				}
			}
			bool flag9 = traits.Calculating > 0;
			if (flag9)
			{
				num3++;
			}
			else
			{
				bool flag10 = traits.Calculating < 0;
				if (flag10)
				{
					num++;
				}
			}
			bool flag11 = num > num2 && num > num3 && num > num4;
			int result;
			if (flag11)
			{
				result = 1;
			}
			else
			{
				bool flag12 = num2 > num && num2 > num3 && num2 > num4;
				if (flag12)
				{
					result = 2;
				}
				else
				{
					bool flag13 = num3 > num && num3 > num2 && num3 > num4;
					if (flag13)
					{
						result = 3;
					}
					else
					{
						result = 4;
					}
				}
			}
			return result;
		}

		public static void SyncCompanionTraitLevel(Hero hero, TraitObject trait)
		{
			float num = hero.GetRelationWithPlayer() / 100f;
			bool flag = num > 0f;
			if (flag)
			{
				float num2 = 1f - num;
				hero.SetTraitLevel(trait, (int)((float)hero.GetTraitLevel(trait) * num2 + (float)Hero.MainHero.GetTraitLevel(trait) * num));
			}
		}

		public static void RemoveCompanion(Hero hero, bool message = true)
		{
			bool flag = hero != null;
			if (flag)
			{
				bool flag2 = hero.CharacterObject != null;
				if (flag2)
				{
					bool flag3 = hero.PartyBelongedTo.MemberRoster.Contains(hero.CharacterObject);
					if (flag3)
					{
						hero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
					if (message)
					{
						Support.LogMessage(hero.Name.ToString() + " has left your company");
					}
				}
			}
		}

		public static Settlement GetCurrentSettlement()
		{
			return (Settlement.CurrentSettlement == null) ? MobileParty.MainParty.CurrentSettlement : Settlement.CurrentSettlement;
		}

		public static int Random(int min = 0, int max = 1)
		{
			bool flag = min > max;
			if (flag)
			{
				int num = max;
				max = min;
				min = num;
			}
			return Support.random.Next(min * 100, max * 100) / 100;
		}

		public static int Chance(int yes = 0, int no = 1)
		{
			bool flag = Support.random.Next(1, 100) <= 50;
			int result;
			if (flag)
			{
				result = yes;
			}
			else
			{
				result = no;
			}
			return result;
		}

		public static void LogMessage(string message)
		{
			InformationManager.DisplayMessage(new InformationMessage(message));
		}

		public static void LogFriendlyMessage(string message)
		{
			InformationManager.DisplayMessage(new InformationMessage(message, Color.FromUint(4282569842U)));
		}

		public static Settings settings;

		public static Random random = new Random();
	}
}
