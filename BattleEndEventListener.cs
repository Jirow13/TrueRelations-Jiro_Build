using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TrueRelations
{
	public static class BattleEndEventListener
	{
		public static void PostBattleRelationships(MapEvent eve)
		{
			try
			{
				bool flag = (eve.IsFieldBattle || eve.IsSiegeAssault || eve.IsSiegeOutside || eve.IsHideoutBattle || eve.IsRaid || eve.IsForcingVolunteers || eve.IsForcingSupplies) && (eve.Winner != null && eve.AttackerSide != null) && eve.DefenderSide != null;
				if (flag)
				{
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = eve.DefeatedSide == BattleSideEnum.Attacker;
					MapEventSide mapEventSide;
					MapEventSide mapEventSide2;
					if (flag5)
					{
						mapEventSide = eve.AttackerSide;
						mapEventSide2 = eve.DefenderSide;
					}
					else
					{
						flag4 = true;
						mapEventSide = eve.DefenderSide;
						mapEventSide2 = eve.AttackerSide;
					}
					List<PartyBase> list = new List<PartyBase>(mapEventSide2.PartiesOnThisSide.ToList<PartyBase>());
					List<PartyBase> list2 = new List<PartyBase>(mapEventSide.PartiesOnThisSide.ToList<PartyBase>());
					int num = 1;
					bool flag6 = false;
					for (int i = 0; i < list.Count; i++)
					{
						PartyBase party = list.ElementAt(i);
						Hero hero = Support.FindHero(party);
						bool flag7 = hero != null;
						if (flag7)
						{
							bool flag8 = !hero.IsHumanPlayerCharacter;
							if (flag8)
							{
								for (int j = 0; j < list2.Count; j++)
								{
									PartyBase party2 = list2.ElementAt(j);
									Hero hero2 = Support.FindHero(party2);
									bool flag9 = hero2 != null;
									if (flag9)
									{
										bool flag10 = !hero2.IsHumanPlayerCharacter;
										if (flag10)
										{
											bool battle_leaders_ai_reputation_enabled = Support.settings.battle_leaders_ai_reputation_enabled;
											if (battle_leaders_ai_reputation_enabled)
											{
												Support.ChangeRelation(hero, hero2, -num);
											}
										}
										else
										{
											flag3 = true;
											flag2 = flag4;
											flag6 = true;
										}
									}
								}
								for (int k = 0; k < list.Count; k++)
								{
									bool flag11 = k != i;
									if (flag11)
									{
										PartyBase party3 = list.ElementAt(k);
										Hero hero3 = Support.FindHero(party3);
										bool flag12 = hero3 != null;
										if (flag12)
										{
											bool flag13 = !hero3.IsHumanPlayerCharacter;
											if (flag13)
											{
												bool battle_leaders_ai_reputation_enabled2 = Support.settings.battle_leaders_ai_reputation_enabled;
												if (battle_leaders_ai_reputation_enabled2)
												{
													Support.ChangeRelation(hero, hero3, num);
												}
											}
											else
											{
												flag3 = false;
												flag2 = !flag4;
												flag6 = true;
											}
										}
									}
								}
								bool battle_leaders_ai_reputation_enabled3 = Support.settings.battle_leaders_ai_reputation_enabled;
								if (battle_leaders_ai_reputation_enabled3)
								{
									bool flag14 = hero.MapFaction != null;
									if (flag14)
									{
										bool flag15 = hero.MapFaction.Leader != null;
										if (flag15)
										{
											Hero leader = hero.MapFaction.Leader;
											bool flag16 = leader != hero;
											if (flag16)
											{
												CharacterTraits heroTraits = hero.GetHeroTraits();
												Support.ChangeRelation(leader, hero, Support.Chance(0, 1));
												bool flag17 = heroTraits.Valor > 0;
												if (flag17)
												{
													Support.ChangeRelation(leader, hero, Support.Chance(0, 1));
												}
												bool flag18 = heroTraits.Honor > 0;
												if (flag18)
												{
													Support.ChangeRelation(leader, hero, Support.Chance(0, 1));
												}
												heroTraits = leader.GetHeroTraits();
												Support.ChangeRelation(hero, leader, Support.Chance(0, 1));
												bool flag19 = heroTraits.Valor > 0;
												if (flag19)
												{
													Support.ChangeRelation(hero, leader, Support.Chance(0, 1));
												}
												bool flag20 = heroTraits.Honor > 0;
												if (flag20)
												{
													Support.ChangeRelation(hero, leader, Support.Chance(0, 1));
												}
											}
										}
									}
									List<Hero> list3 = Support.FindFaction(hero);
									for (int l = 0; l < list3.Count; l++)
									{
										bool flag21 = list3[l] != null;
										if (flag21)
										{
											bool flag22 = list3[l] != hero;
											if (flag22)
											{
												bool flag23 = !list3[l].IsHumanPlayerCharacter;
												if (flag23)
												{
													CharacterTraits heroTraits = list3[l].GetHeroTraits();
													bool flag24 = heroTraits.Valor > 0;
													if (flag24)
													{
														Support.ChangeRelation(hero, list3[l], Support.Chance(0, 1));
													}
													bool flag25 = heroTraits.Honor < 0;
													if (flag25)
													{
														Support.ChangeRelation(hero, list3[l], Support.Chance(-1, 1));
													}
													else
													{
														Support.ChangeRelation(hero, list3[l], Support.Chance(0, 1));
													}
												}
											}
										}
									}
								}
							}
							else
							{
								flag3 = false;
								flag2 = !flag4;
								flag6 = true;
							}
						}
					}
					bool battle_leaders_ai_reputation_enabled4 = Support.settings.battle_leaders_ai_reputation_enabled;
					if (battle_leaders_ai_reputation_enabled4)
					{
						for (int m = 0; m < list2.Count; m++)
						{
							PartyBase party2 = list2.ElementAt(m);
							Hero hero2 = Support.FindHero(party2);
							bool flag26 = hero2 != null;
							if (flag26)
							{
								bool flag27 = !hero2.IsHumanPlayerCharacter;
								if (flag27)
								{
									bool flag28 = hero2.MapFaction != null;
									if (flag28)
									{
										bool flag29 = hero2.MapFaction.Leader != null;
										if (flag29)
										{
											Hero leader = hero2.MapFaction.Leader;
											bool flag30 = leader != hero2;
											if (flag30)
											{
												CharacterTraits heroTraits = hero2.GetHeroTraits();
												Support.ChangeRelation(leader, hero2, Support.Chance(-1, 0));
												bool flag31 = heroTraits.Valor < 0;
												if (flag31)
												{
													Support.ChangeRelation(leader, hero2, Support.Chance(-1, 0));
												}
												bool flag32 = heroTraits.Honor < 0;
												if (flag32)
												{
													Support.ChangeRelation(leader, hero2, Support.Chance(-1, 0));
												}
												heroTraits = leader.GetHeroTraits();
												Support.ChangeRelation(hero2, leader, Support.Chance(-1, 0));
												bool flag33 = heroTraits.Valor > 0;
												if (flag33)
												{
													Support.ChangeRelation(hero2, leader, Support.Chance(-1, 0));
												}
												bool flag34 = heroTraits.Honor > 0;
												if (flag34)
												{
													Support.ChangeRelation(hero2, leader, Support.Chance(-1, 0));
												}
											}
										}
									}
									List<Hero> list3 = Support.FindFaction(hero2);
									for (int n = 0; n < list3.Count; n++)
									{
										bool flag35 = list3[n] != null;
										if (flag35)
										{
											bool flag36 = list3[n] != hero2;
											if (flag36)
											{
												bool flag37 = !list3[n].IsHumanPlayerCharacter;
												if (flag37)
												{
													CharacterTraits heroTraits = list3[n].GetHeroTraits();
													bool flag38 = heroTraits.Valor > 0;
													if (flag38)
													{
														Support.ChangeRelation(hero2, list3[n], Support.Chance(-1, 0));
													}
													bool flag39 = heroTraits.Honor < 0;
													if (flag39)
													{
														Support.ChangeRelation(hero2, list3[n], Support.Chance(-1, 1));
													}
													else
													{
														Support.ChangeRelation(hero2, list3[n], Support.Chance(-1, 0));
													}
												}
											}
										}
									}
								}
							}
						}
					}
					bool flag40 = flag6;
					if (flag40)
					{
						bool flag41 = Hero.MainHero != null && Hero.MainHero.IsAlive && !Hero.MainHero.IsPrisoner && Hero.MainHero.PartyBelongedTo == MobileParty.MainParty;
						if (flag41)
						{
							MapEventSide mapEventSide3 = (eve.DefeatedSide != BattleSideEnum.Attacker) ? eve.DefenderSide : eve.AttackerSide;
							bool flag42 = flag3;
							MapEventSide mapEventSide4;
							MapEventSide mapEventSide5;
							if (flag42)
							{
								bool flag43 = flag4;
								if (flag43)
								{
									mapEventSide4 = eve.DefenderSide;
									mapEventSide5 = eve.AttackerSide;
								}
								else
								{
									mapEventSide4 = eve.AttackerSide;
									mapEventSide5 = eve.DefenderSide;
								}
							}
							else
							{
								bool flag44 = flag4;
								if (flag44)
								{
									mapEventSide4 = eve.AttackerSide;
									mapEventSide5 = eve.DefenderSide;
								}
								else
								{
									mapEventSide4 = eve.DefenderSide;
									mapEventSide5 = eve.AttackerSide;
								}
							}
							List<PartyBase> list4 = new List<PartyBase>(mapEventSide4.PartiesOnThisSide.ToList<PartyBase>());
							List<PartyBase> list5 = new List<PartyBase>(mapEventSide5.PartiesOnThisSide.ToList<PartyBase>());
							CharacterTraits heroTraits2 = Hero.MainHero.GetHeroTraits();
							int num2 = 1;
							float num3 = 1f;
							float num4 = (float)mapEventSide4.Casualties;
							float num5 = (float)mapEventSide5.Casualties;
							float num6 = num4 + num5;
							bool flag45 = num6 < 1f;
							if (flag45)
							{
								num6 = 1f;
							}
							float num7 = num5 / num6;
							num3 = MathF.Clamp(num5 / 50f * (num5 / num6 * 2f), 0f, 5f);
							bool flag46 = flag3;
							float num8;
							if (flag46)
							{
								num8 = num5 / num6;
							}
							else
							{
								num8 = mapEventSide4.GetPlayerPartyContributionRate();
							}
							num2 = (int)(num8 * num3);
							bool flag47 = flag3;
							if (flag47)
							{
								bool flag48 = flag2;
								if (flag48)
								{
									num2 = (int)((double)num2 * 0.8);
								}
								else
								{
									num2 = (int)((double)num2 * 0.5);
								}
							}
							else
							{
								bool flag49 = flag2;
								if (flag49)
								{
									num2 = (int)((double)num2 * 1.5);
								}
							}
							bool battle_leaders_good_reputation_enabled = Support.settings.battle_leaders_good_reputation_enabled;
							if (battle_leaders_good_reputation_enabled)
							{
								for (int num9 = 0; num9 < list4.Count; num9++)
								{
									PartyBase party4 = list4.ElementAt(num9);
									Hero hero4 = Support.FindHero(party4);
									bool flag50 = hero4 != null;
									if (flag50)
									{
										bool flag51 = !hero4.IsHumanPlayerCharacter;
										if (flag51)
										{
											CharacterTraits heroTraits3 = hero4.GetHeroTraits();
											int num10 = num2;
											bool flag52 = (heroTraits3.Honor > 0 && heroTraits2.Honor > 0) || (heroTraits3.Honor < 0 && heroTraits2.Honor < 0);
											if (flag52)
											{
												num10++;
											}
											else
											{
												bool flag53 = (heroTraits3.Honor > 0 && heroTraits2.Honor < 0) || (heroTraits3.Honor < 0 && heroTraits2.Honor > 0);
												if (flag53)
												{
													num10--;
												}
											}
											bool flag54 = heroTraits3.Valor > 0 && heroTraits2.Valor > 0;
											if (flag54)
											{
												num10++;
											}
											else
											{
												bool flag55 = (heroTraits3.Valor > 0 && heroTraits2.Valor < 0) || (heroTraits3.Valor < 0 && heroTraits2.Valor > 0);
												if (flag55)
												{
													num10--;
												}
											}
											bool flag56 = heroTraits2.Valor > 0;
											if (flag56)
											{
												num10++;
											}
											Support.ChangeRelation(Hero.MainHero, hero4, num10);
											Support.ChangeFamilyRelation(Hero.MainHero, hero4, num10 / 3, 0);
											Support.ChangeFactionRelation(Hero.MainHero, hero4, num10 / 5, 0);
										}
									}
								}
								Support.ChangeFamilyRelation(Hero.MainHero, Hero.MainHero, num2 / 2, 0);
								Support.ChangeFactionRelation(Hero.MainHero, Hero.MainHero, num2 / 4, 0);
								bool flag57 = num2 > 0 && list4.Count > 1;
								if (flag57)
								{
									Support.LogFriendlyMessage("Your reputation has improved with your allies in battle.");
								}
							}
							num2 = (int)(num8 * (1f + num3));
							bool flag58 = flag3;
							if (flag58)
							{
								bool flag59 = flag2;
								if (flag59)
								{
									num2 = (int)((double)num2 * 0.3);
								}
							}
							else
							{
								bool flag60 = flag2;
								if (flag60)
								{
									num2 = (int)((double)num2 * 0.6);
								}
								else
								{
									num2 = (int)((double)num2 * 1.4);
								}
							}
							bool battle_leaders_bad_reputation_enabled = Support.settings.battle_leaders_bad_reputation_enabled;
							if (battle_leaders_bad_reputation_enabled)
							{
								for (int num11 = 0; num11 < list5.Count; num11++)
								{
									PartyBase party4 = list5.ElementAt(num11);
									Hero hero4 = Support.FindHero(party4);
									bool flag61 = hero4 != null;
									if (flag61)
									{
										CharacterTraits heroTraits3 = hero4.GetHeroTraits();
										int num10 = -num2;
										bool flag62 = heroTraits3.Honor > 0 && heroTraits2.Honor > 0;
										if (flag62)
										{
											num10++;
										}
										else
										{
											bool flag63 = (heroTraits3.Honor < 0 && heroTraits2.Honor > 0) || (heroTraits3.Honor > 0 && heroTraits2.Honor < 0);
											if (flag63)
											{
												num10--;
											}
										}
										bool flag64 = heroTraits3.Valor > 0 && heroTraits2.Valor > 0;
										if (flag64)
										{
											num10++;
										}
										else
										{
											bool flag65 = (heroTraits3.Valor < 0 && heroTraits2.Valor > 0) || (heroTraits3.Valor > 0 && heroTraits2.Valor < 0);
											if (flag65)
											{
												num10--;
											}
										}
										bool flag66 = (heroTraits3.Mercy < 0 && heroTraits2.Mercy > 0) || (heroTraits3.Mercy > 0 && heroTraits2.Mercy < 0);
										if (flag66)
										{
											num10--;
										}
										bool flag67 = (heroTraits3.Generosity < 0 && heroTraits2.Generosity > 0) || (heroTraits3.Generosity > 0 && heroTraits2.Generosity < 0);
										if (flag67)
										{
											num10--;
										}
										bool flag68 = flag3;
										if (flag68)
										{
											bool flag69 = heroTraits3.Mercy > 0 || heroTraits3.Generosity > 0;
											if (flag69)
											{
												num10++;
											}
										}
										else
										{
											bool flag70 = heroTraits3.Honor < 0;
											if (flag70)
											{
												num10--;
											}
										}
										Support.ChangeRelation(Hero.MainHero, hero4, num10);
										Support.ChangeFamilyRelation(Hero.MainHero, hero4, num10 / 2, 0);
										Support.ChangeFactionRelation(Hero.MainHero, hero4, num10 / 4, 0);
									}
								}
							}
							bool flag71 = false;
							bool flag72 = !flag3;
							if (flag72)
							{
								bool isBanditFaction = eve.GetLeaderParty(eve.DefeatedSide).MapFaction.IsBanditFaction;
								if (isBanditFaction)
								{
									bool bandit_battle_bonus_enabled = Support.settings.bandit_battle_bonus_enabled;
									if (bandit_battle_bonus_enabled)
									{
										num2 = mapEventSide3.Casualties / Support.settings.bandit_battle_bandits_for_bonus + Support.settings.bandit_battle_minimum_bonus;
										foreach (Settlement settlement in Settlement.All)
										{
											bool flag73 = settlement != null;
											if (flag73)
											{
												bool flag74 = (settlement.IsVillage || settlement.IsTown) && (double)settlement.Position2D.DistanceSquared(eve.Position) <= 1000.0;
												if (flag74)
												{
													bool flag75 = settlement.Notables.Any<Hero>();
													if (flag75)
													{
														Support.ChangeRelation(Hero.MainHero, settlement.Notables.GetRandomElement<Hero>(), Support.Random(num2, num2 + 1));
													}
												}
											}
										}
										Support.LogFriendlyMessage("Your reputation has increased with nearby notables.");
									}
									flag71 = true;
								}
								bool flag76 = (eve.IsForcingVolunteers || eve.IsForcingSupplies || eve.IsRaid) && !flag2;
								bool flag77 = (eve.IsForcingVolunteers || eve.IsForcingSupplies || eve.IsRaid) && flag2;
								int num12 = (int)(num5 / 25f) + 1;
								int num13 = (int)((double)(2 * Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers) * Support.settings.player_trait_skill_gain_rate);
								int num14 = (int)(0.25 * (double)num5 * Support.settings.player_trait_skill_gain_rate);
								int num15 = (int)((double)Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers * Support.settings.player_trait_skill_gain_rate);
								bool player_trait_skill_gain_enabled = Support.settings.player_trait_skill_gain_enabled;
								if (player_trait_skill_gain_enabled)
								{
									bool flag78 = flag71;
									if (flag78)
									{
										Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, (int)((double)(5 + num2) * Support.settings.player_trait_skill_gain_rate));
									}
									bool flag79 = flag77 || eve.IsHideoutBattle;
									if (flag79)
									{
										Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, (int)((double)(50f + num5) * Support.settings.player_trait_skill_gain_rate));
									}
									else
									{
										bool flag80 = flag76;
										if (flag80)
										{
											Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -(int)((double)(75f + num5) * Support.settings.player_trait_skill_gain_rate));
											Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, (float)((int)((double)(400f + num5 * 10f) * Support.settings.player_trait_skill_gain_rate)));
										}
									}
									bool flag81 = flag71 || flag76;
									if (flag81)
									{
										num12 /= 2;
									}
									else
									{
										bool flag82 = flag3;
										if (flag82)
										{
											num12 /= 3;
										}
									}
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, num12);
									Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, (float)num13);
									Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, (float)num14);
									Hero.MainHero.AddSkillXp(DefaultSkills.Medicine, (float)num15);
								}
								bool flag83 = flag77;
								if (flag83)
								{
									TextObject name = Hero.MainHero.Name;
									Support.LogMessage(((name != null) ? name.ToString() : null) + " has shown great honor in this defense.");
								}
								else
								{
									bool flag84 = eve.IsHideoutBattle && !flag3;
									if (flag84)
									{
										TextObject name2 = Hero.MainHero.Name;
										Support.LogMessage(((name2 != null) ? name2.ToString() : null) + " has ended the reign of these brigands.");
									}
									else
									{
										bool flag85 = flag71;
										if (flag85)
										{
											TextObject name3 = Hero.MainHero.Name;
											Support.LogMessage(((name3 != null) ? name3.ToString() : null) + " has honorably dispatched the brigands.");
										}
										else
										{
											bool flag86 = flag76;
											if (flag86)
											{
												TextObject name4 = Hero.MainHero.Name;
												Support.LogMessage(((name4 != null) ? name4.ToString() : null) + " has dishonored " + (Hero.MainHero.IsFemale ? "herself" : "himself") + ".");
											}
										}
									}
								}
								bool flag87 = flag76;
								if (flag87)
								{
									Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 1f;
									bool isForcingVolunteers = eve.IsForcingVolunteers;
									if (isForcingVolunteers)
									{
										Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 10f;
									}
								}
								num2 = (int)num3;
								bool family_global_bonus_enabled = Support.settings.family_global_bonus_enabled;
								if (family_global_bonus_enabled)
								{
									Support.ChangeFamilyRelation(Hero.MainHero, Hero.MainHero, num2 / 4, 0);
								}
								bool faction_global_bonus_enabled = Support.settings.faction_global_bonus_enabled;
								if (faction_global_bonus_enabled)
								{
									Support.ChangeFactionRelation(Hero.MainHero, Hero.MainHero, num2 / 5, 0);
								}
								List<Hero> heroesInMainParty = Support.GetHeroesInMainParty();
								num13 /= 40;
								num14 /= 40;
								num15 /= 20;
								for (int num16 = 0; num16 < heroesInMainParty.Count; num16++)
								{
									bool isAlive = heroesInMainParty[num16].IsAlive;
									if (isAlive)
									{
										int num10 = num2;
										CharacterTraits heroTraits4 = heroesInMainParty[num16].GetHeroTraits();
										bool flag88 = false;
										bool flag89 = false;
										bool flag90 = false;
										bool flag91 = false;
										switch (Support.EvaluatePersonality(heroTraits4))
										{
										case 1:
											flag88 = true;
											break;
										case 2:
											flag89 = true;
											break;
										case 3:
											flag90 = true;
											break;
										default:
											flag91 = true;
											break;
										}
										bool flag92 = flag3;
										if (flag92)
										{
											bool flag93 = (Support.settings.battle_companions_bad_reputation_enabled && heroesInMainParty[num16].IsPlayerCompanion) || (Support.settings.battle_family_bad_reputation_enabled && !heroesInMainParty[num16].IsPlayerCompanion);
											if (flag93)
											{
												bool flag94 = flag89 || flag90;
												if (flag94)
												{
													bool isWounded = heroesInMainParty[num16].IsWounded;
													if (isWounded)
													{
														num10 -= Support.Random(2, 5);
													}
													else
													{
														num10 -= Support.Random(1, 3);
													}
												}
												else
												{
													bool flag95 = flag91;
													if (flag95)
													{
														bool isWounded2 = heroesInMainParty[num16].IsWounded;
														if (isWounded2)
														{
															num10 -= Support.Chance(1, 2);
														}
														else
														{
															num10 -= Support.Chance(0, 1);
														}
													}
													else
													{
														bool isWounded3 = heroesInMainParty[num16].IsWounded;
														if (isWounded3)
														{
															num10 -= Support.Chance(0, 1);
														}
													}
												}
												bool flag96 = flag76;
												if (flag96)
												{
													bool flag97 = flag91 || flag88;
													if (flag97)
													{
														num10 -= Support.Random(6, 12);
													}
												}
											}
										}
										else
										{
											bool flag98 = (Support.settings.battle_companions_bad_reputation_enabled && heroesInMainParty[num16].IsPlayerCompanion) || (Support.settings.battle_family_bad_reputation_enabled && !heroesInMainParty[num16].IsPlayerCompanion);
											if (flag98)
											{
												bool isWounded4 = heroesInMainParty[num16].IsWounded;
												if (isWounded4)
												{
													bool flag99 = flag89 || flag90;
													if (flag99)
													{
														num10 -= Support.Random(1, 3);
													}
													else
													{
														num10 -= Support.Chance(0, 1);
													}
												}
											}
											bool flag100 = (Support.settings.battle_companions_good_reputation_enabled && heroesInMainParty[num16].IsPlayerCompanion) || (Support.settings.battle_family_good_reputation_enabled && !heroesInMainParty[num16].IsPlayerCompanion);
											if (flag100)
											{
												bool isHideoutBattle = eve.IsHideoutBattle;
												if (isHideoutBattle)
												{
													bool flag101 = flag91 || flag88 || flag89;
													if (flag101)
													{
														num10 += Support.Chance(1, 2);
													}
													else
													{
														num10 += Support.Chance(0, 1);
													}
												}
												else
												{
													bool flag102 = flag76;
													if (flag102)
													{
														bool flag103 = flag91 || flag88;
														if (flag103)
														{
															num10 -= Support.Random(3, 7);
														}
													}
													else
													{
														num10 += Support.Chance(0, 1);
													}
												}
												bool flag104 = heroTraits4.Honor > 0;
												if (flag104)
												{
													num10 += heroTraits4.Honor;
													bool flag105 = heroTraits2.Honor > 0;
													if (flag105)
													{
														num10 += Support.Chance(0, 1);
													}
												}
												bool flag106 = heroTraits4.Valor > 0;
												if (flag106)
												{
													num10 += heroTraits4.Valor * 2;
													bool flag107 = heroTraits2.Valor > 0;
													if (flag107)
													{
														num10 += Support.Chance(0, 1);
													}
												}
											}
										}
										bool flag108 = (Support.settings.companion_trait_sharing_enabled && heroesInMainParty[num16].IsPlayerCompanion) || (Support.settings.family_trait_sharing_enabled && !heroesInMainParty[num16].IsPlayerCompanion);
										if (flag108)
										{
											Support.SyncCompanionTraitLevel(heroesInMainParty[num16], DefaultTraits.Honor);
											Support.SyncCompanionTraitLevel(heroesInMainParty[num16], DefaultTraits.Valor);
											Support.SyncCompanionTraitLevel(heroesInMainParty[num16], DefaultTraits.Mercy);
											Support.SyncCompanionTraitLevel(heroesInMainParty[num16], DefaultTraits.Generosity);
										}
										bool flag109 = (Support.settings.companions_skill_gain_enabled && heroesInMainParty[num16].IsPlayerCompanion) || (Support.settings.family_skill_gain_enabled && !heroesInMainParty[num16].IsPlayerCompanion);
										if (flag109)
										{
											heroesInMainParty[num16].AddSkillXp(DefaultSkills.Leadership, (float)((int)((double)num13 * Support.settings.companions_skill_gain_rate)));
											heroesInMainParty[num16].AddSkillXp(DefaultSkills.Tactics, (float)((int)((double)num14 * Support.settings.companions_skill_gain_rate)));
											heroesInMainParty[num16].AddSkillXp(DefaultSkills.Medicine, (float)((int)((double)num15 * Support.settings.companions_skill_gain_rate)));
											bool flag110 = flag76;
											if (flag110)
											{
												bool isPlayerCompanion = heroesInMainParty[num16].IsPlayerCompanion;
												if (isPlayerCompanion)
												{
													heroesInMainParty[num16].AddSkillXp(DefaultSkills.Roguery, (float)((int)(30.0 * Support.settings.companions_skill_gain_rate)));
												}
												else
												{
													heroesInMainParty[num16].AddSkillXp(DefaultSkills.Roguery, (float)((int)(30.0 * Support.settings.family_skill_gain_rate)));
												}
											}
										}
										bool flag111 = !heroesInMainParty[num16].IsPlayerCompanion;
										if (flag111)
										{
											bool flag112 = num10 < 0;
											if (flag112)
											{
												num10 = (int)((double)num10 * 0.6);
											}
											else
											{
												num10 = (int)((double)num10 * 1.8);
											}
										}
										Support.ChangeRelation(Hero.MainHero, heroesInMainParty[num16], num10);
									}
								}
								CompanionBanter.banterID = 0;
								CompanionBanter.subBanterID = 0;
								bool flag113 = eve.IsFieldBattle || eve.IsRaid || eve.IsForcingVolunteers || eve.IsForcingSupplies;
								if (flag113)
								{
									bool flag114 = !CompanionBanter.CheckForConfrontation() && Support.settings.random_events_enabled;
									if (flag114)
									{
										bool flag115 = CompanionBanter.TimeForBanter((int)(Support.settings.random_events_chance * 100.0));
										if (flag115)
										{
											bool flag116 = false;
											int num17 = 0;
											float num18 = 0f;
											bool flag117 = Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers > 0;
											if (flag117)
											{
												num18 = (float)(Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers / Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers);
											}
											float num19 = (float)mapEventSide5.Casualties / num6;
											List<Hero> list6 = new List<Hero>(Hero.MainHero.CompanionsInParty.ToList<Hero>());
											while (!flag116 && num17 < list6.Count)
											{
												bool isAlive2 = list6[num17].IsAlive;
												if (isAlive2)
												{
													bool flag118 = !list6[num17].IsWounded;
													if (flag118)
													{
														bool flag119 = Support.Chance(0, 1) == 1;
														if (flag119)
														{
															bool flag120 = flag76;
															if (flag120)
															{
																bool confrontation_events_enabled = Support.settings.confrontation_events_enabled;
																if (confrontation_events_enabled)
																{
																	bool isRaid = eve.IsRaid;
																	if (isRaid)
																	{
																		CompanionBanter.Trigger(list6[num17], 400);
																	}
																	else
																	{
																		bool isForcingSupplies = eve.IsForcingSupplies;
																		if (isForcingSupplies)
																		{
																			CompanionBanter.Trigger(list6[num17], 410);
																		}
																		else
																		{
																			bool isForcingVolunteers2 = eve.IsForcingVolunteers;
																			if (isForcingVolunteers2)
																			{
																				CompanionBanter.Trigger(list6[num17], 420);
																			}
																		}
																	}
																}
															}
															else
															{
																bool isHideoutBattle2 = eve.IsHideoutBattle;
																if (isHideoutBattle2)
																{
																	CompanionBanter.Trigger(list6[num17], 260);
																}
																else
																{
																	bool flag121 = flag77;
																	if (flag121)
																	{
																		CompanionBanter.Trigger(list6[num17], 200);
																	}
																	else
																	{
																		bool flag122 = mapEventSide4.Casualties == 0 && num5 >= 30f && (float)Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers < num5;
																		if (flag122)
																		{
																			CompanionBanter.Trigger(list6[num17], 160);
																		}
																		else
																		{
																			bool flag123 = num19 >= 0.75f && Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers > 10 && num5 > 30f && num18 < 0.5f;
																			if (flag123)
																			{
																				CompanionBanter.Trigger(list6[num17], 100);
																			}
																			else
																			{
																				bool flag124 = num18 >= 0.75f && Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers > 25;
																				if (flag124)
																				{
																					CompanionBanter.Trigger(list6[num17], 110);
																				}
																				else
																				{
																					bool flag125 = Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers == 0 && list6.Count >= 2 && num5 >= 10f;
																					if (flag125)
																					{
																						CompanionBanter.Trigger(list6[num17], 250);
																					}
																					else
																					{
																						bool flag126 = num18 <= 0.2f && num18 > 0f && (double)Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers < (double)num5 * 1.4;
																						if (flag126)
																						{
																							CompanionBanter.Trigger(list6[num17], 120);
																						}
																						else
																						{
																							bool flag127 = mapEventSide5.Casualties > Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers * 2 && Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers > 25;
																							if (flag127)
																							{
																								CompanionBanter.Trigger(list6[num17], 130);
																							}
																							else
																							{
																								bool flag128 = Hero.MainHero.Gold >= 3000 && num5 > 30f && Support.Chance(0, 1) == 1;
																								if (flag128)
																								{
																									CompanionBanter.Trigger(list6[num17], 150);
																								}
																								else
																								{
																									bool flag129 = flag71 && num5 > 10f && Support.Chance(0, 1) == 1;
																									if (flag129)
																									{
																										CompanionBanter.Trigger(list6[num17], 170);
																									}
																									else
																									{
																										bool flag130 = num5 > 30f;
																										if (flag130)
																										{
																											CompanionBanter.Trigger(list6[num17], 140);
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
															flag116 = true;
														}
													}
												}
												num17++;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Support.LogMessage("Mod failure - True Relations");
				Support.LogMessage("Cause: " + ex.ToString());
			}
		}
	}
}
