using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TrueRelations
{
	public static class CompanionBanter
	{
		public static void AddressPlayer(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("hlc_banter_event_start", "start", "hlc_banter_preintro", "{=HLC10000}{HLC_PREINTRO}", new ConversationSentence.OnConditionDelegate(CompanionBanter.Start), null, 120, null);
			campaignGameStarter.AddPlayerLine("hlc_banter_event_preintro", "hlc_banter_preintro", "hlc_banter_intro", "{=HLC10001}{HLC_PREINTRO_RESPONSE}", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("hlc_banter_event_preintro", "hlc_banter_preintro", "close_window", "{=HLC10002}{HLC_PREINTRO_END}", null, new ConversationSentence.OnConsequenceDelegate(CompanionBanter.Ignore_Effect), 100, null, null);
			campaignGameStarter.AddDialogLine("hlc_banter_event_intro", "hlc_banter_intro", "hlc_banter_choice", "{=HLC10003}{HLC_INTRO}", null, null, 120, null);
			campaignGameStarter.AddPlayerLine("hlc_banter_event_choice", "hlc_banter_choice", "hlc_banter_result", "{=HLC10004}{HLC_CHOICE1}", null, new ConversationSentence.OnConsequenceDelegate(CompanionBanter.Choice_1_Effect), 100, null, null);
			campaignGameStarter.AddPlayerLine("hlc_banter_event_choice", "hlc_banter_choice", "hlc_banter_result", "{=HLC10005}{HLC_CHOICE2}", null, new ConversationSentence.OnConsequenceDelegate(CompanionBanter.Choice_2_Effect), 100, null, null);
			campaignGameStarter.AddPlayerLine("hlc_banter_event_choice", "hlc_banter_choice", "hlc_banter_result", "{=HLC10006}{HLC_CHOICE3}", null, new ConversationSentence.OnConsequenceDelegate(CompanionBanter.Choice_3_Effect), 100, null, null);
			campaignGameStarter.AddDialogLine("hlc_banter_event_result_1", "hlc_banter_result", "close_window", "{=HLC10007}{HLC_RESULT}", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("hlc_companion_opinion", "hero_main_options", "hlc_companion_opinion_details1", "{=HLC10020}How do you think our party is doing?", new ConversationSentence.OnConditionDelegate(CompanionBanter.CompanionOpinionStart), null, 120, null, null);
			campaignGameStarter.AddDialogLine("hlc_companion_opinion_info1", "hlc_companion_opinion_details1", "hlc_companion_opinion_details2", "{=HLC10020}{HLC_CHOICE1}", null, null, 100, null);
			campaignGameStarter.AddDialogLine("hlc_companion_opinion_info2", "hlc_companion_opinion_details2", "hlc_companion_opinion_details3", "{=HLC10020}{HLC_CHOICE2}", null, null, 100, null);
			campaignGameStarter.AddDialogLine("hlc_companion_opinion_info3", "hlc_companion_opinion_details3", "hero_main_options", "{=HLC10020}{HLC_CHOICE3}", null, null, 100, null);
		}

		public static void Trigger(Hero selectedHero, int selectedID = 0)
		{
			bool flag = selectedHero != null;
			if (flag)
			{
				bool flag2 = selectedHero.CharacterObject != null;
				if (flag2)
				{
					bool isHero = selectedHero.CharacterObject.IsHero;
					if (isHero)
					{
						CompanionBanter.LoadVairables(selectedHero);
						bool flag3 = CompanionBanter.playerCharm < 25;
						if (flag3)
						{
							CompanionBanter.playerCharm = 25;
						}
						CompanionBanter.banterID = selectedID;
						CompanionBanter.subBanterID = 0;
						bool flag4 = CompanionBanter.banterID > 0;
						if (flag4)
						{
							CompanionBanter.subBanterID = CompanionBanter.banterID;
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(Hero.MainHero.CharacterObject, null, false, false, false, false), new ConversationCharacterData(CompanionBanter.hero.CharacterObject, null, false, false, false, false));
							CompanionBanter.banterID = 0;
						}
					}
				}
			}
		}

		public static void LoadVairables(Hero selectedHero)
		{
			CompanionBanter.hero = selectedHero;
			CompanionBanter.heroTraits = CompanionBanter.hero.GetHeroTraits();
			CompanionBanter.honorable = false;
			CompanionBanter.rogue = false;
			CompanionBanter.logical = false;
			CompanionBanter.kind = false;
			switch (Support.EvaluatePersonality(CompanionBanter.heroTraits))
			{
			case 1:
				CompanionBanter.honorable = true;
				break;
			case 2:
				CompanionBanter.rogue = true;
				break;
			case 3:
				CompanionBanter.logical = true;
				break;
			default:
				CompanionBanter.kind = true;
				break;
			}
			CompanionBanter.playerTraits = Hero.MainHero.GetHeroTraits();
			CompanionBanter.playerTitle = (Hero.MainHero.IsFemale ? "lady" : "lord");
			CompanionBanter.playerCharm = Hero.MainHero.GetSkillValue(DefaultSkills.Charm);
			bool flag = CompanionBanter.playerCharm < 25;
			if (flag)
			{
				CompanionBanter.playerCharm = 25;
			}
		}

		public static bool TimeForBanter(int cutoff = 1)
		{
			return Support.Random(1, 1000) <= cutoff * 10;
		}

		public static bool Start()
		{
			bool flag = CompanionBanter.banterID > 0;
			bool result;
			if (flag)
			{
				string text = "A moment of your time my " + CompanionBanter.playerTitle + ".";
				string text2 = "Go on, I'm listening";
				string text3 = "We'll talk another time";
				string text4 = "Greetings my " + CompanionBanter.playerTitle + ".";
				string text5 = "Yes";
				string text6 = "Maybe";
				string text7 = "No";
				int num = CompanionBanter.banterID;
				int num2 = num;
				if (num2 <= 160)
				{
					if (num2 <= 120)
					{
						if (num2 != 100)
						{
							if (num2 != 110)
							{
								if (num2 == 120)
								{
									bool flag2 = CompanionBanter.honorable;
									if (flag2)
									{
										text = "We have claimed victory my " + CompanionBanter.playerTitle + ", and our enemies have paid dearly for their mistake.[rf:happy][rb:very_positive]";
										text4 = "It seems that greater numbers do not guarantee winning battles, as some of the less intellectual lords believe.[rb:positive]";
									}
									else
									{
										bool flag3 = CompanionBanter.rogue;
										if (flag3)
										{
											text = "My " + CompanionBanter.playerTitle + ", the battle is over, and absolutely over for the enemy fallen.[rf:happy][rb:very_positive]";
											text4 = "It seems their approach was flawed, perhaps you should give them a few lessons in tactics.[rb:positive]";
										}
										else
										{
											bool flag4 = CompanionBanter.logical;
											if (flag4)
											{
												text = "You have brought us victory my " + CompanionBanter.playerTitle + ". We salute you.[rf:happy][rb:very_positive]";
												text4 = "Your superiority shines over the lesser fools, their logic is insufficient to defy you.[rb:positive]";
											}
											else
											{
												text = "So the battle is over and we stand victorious. I doubt there are many whom can match you my " + CompanionBanter.playerTitle + ".[rf:happy][rb:very_positive]";
												text4 = "I know for certain that a lesser lord than you would not have achieved such a victory today. I pray our enemies never learn from you the arts of tactics and strategy.[rb:positive]";
											}
										}
									}
									text2 = "Those fools do not understand how, who or even where to fight, their defeat was guaranteed";
									text3 = "Our victory was inevitable, nothing else needs to be said";
									text5 = "Strong willed soldiers, well armed and equipped can hold against the strongest horde";
									text6 = "Understanding the strengths and weaknesses of both ourselves and the enemy is the only true path to victory";
									text7 = "Breaking the enemy with devastating precision reveals their fear and true nature, then all we must do is grab hold";
								}
							}
							else
							{
								bool flag5 = CompanionBanter.honorable;
								if (flag5)
								{
									text = "My " + CompanionBanter.playerTitle + ", have you seen our injuries list?[rb:unsure]";
									text4 = "You risk the lives of your men too willingly my " + CompanionBanter.playerTitle + ", these are living and loyal soldiers, not mere pawns to be sacrificed.[if:idle_pleased][rb:unsure]";
								}
								else
								{
									bool flag6 = CompanionBanter.rogue;
									if (flag6)
									{
										text = "Your " + CompanionBanter.playerTitle + "ship, I think we need to get more medics. A lot more.[if:idle_pleased][rb:unsure]";
										text4 = "I'm not questioning your command but, if you keep throwing your men in harm's way like this you will soon have no more men to lead.[if:idle_pleased][rb:unsure]";
									}
									else
									{
										bool flag7 = CompanionBanter.logical;
										if (flag7)
										{
											text = "Forgive me my " + CompanionBanter.playerTitle + ", but we must discuss the damage to our force.[rb:unsure]";
											text4 = "We have achieved victory yes, but at what cost? Such a high price in blood should not be justified through victory, we are now vulnerable.[if:idle_pleased][rb:unsure]";
										}
										else
										{
											text = "My " + CompanionBanter.playerTitle + ", our victory today is drenched in our own blood.[rb:unsure]";
											text4 = "Our casualty rate is far too high, this battle was a near death sentence. This cannot continue![if:idle_pleased][rb:unsure]";
										}
									}
								}
								text2 = "I have something to say, then say it";
								text3 = "We have many soldiers to tend to, this is not the time to speak";
								CompanionBanter.option1Cost = Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers * 5;
								bool flag8 = Hero.MainHero.Gold >= CompanionBanter.option1Cost;
								if (flag8)
								{
									text5 = "The suffering of my men has not gone unnoticed, arrange for additional medicine and care for the wounded";
								}
								else
								{
									text5 = "You are right... The price paid was too steep, I must find ways to better protect the lives of my soldiers";
								}
								text6 = "While the price was high, defeat would have been far more costly";
								text7 = "The duty of a soldier is to bleed for his master, as I see it these men have simply done their duty";
							}
						}
						else
						{
							bool flag9 = CompanionBanter.honorable;
							if (flag9)
							{
								text = "My " + CompanionBanter.playerTitle + "! You have utterly broken the enemy. I salute your might in battle.[rb:very_positive]";
								text4 = "The men are honored to serve under your banner my " + CompanionBanter.playerTitle + ", but they are simple soldiers and should be rewarded for their heroism.[rb:positive]";
							}
							else
							{
								bool flag10 = CompanionBanter.rogue;
								if (flag10)
								{
									text = "Your " + CompanionBanter.playerTitle + "ship, I can see now why the vultures love you, you've brought them a feast![rb:very_positive]";
									text4 = "While great deeds are their own rewards, I reckon the men would appreciate a gesture on your part my " + CompanionBanter.playerTitle + ".[rb:positive]";
								}
								else
								{
									bool flag11 = CompanionBanter.logical;
									if (flag11)
									{
										text = "This victory will be sung in your honor my " + CompanionBanter.playerTitle + ", as your enemies shiver in reminiscence.[rb:very_positive]";
										text4 = string.Concat(new string[]
										{
											"Surely a great ",
											CompanionBanter.playerTitle,
											" such as yourself would not let ",
											Hero.MainHero.IsFemale ? "her" : "his",
											" troops go unrewarded after such a feat.[rb:positive]"
										});
									}
									else
									{
										text = "My " + CompanionBanter.playerTitle + ", the enemy has become nothing more than dust. As they should be.[rb:very_positive]";
										text4 = "With such a victory, I believe the men have earned a reward, have they not?[rf:happy][rb:positive]";
									}
								}
							}
							text2 = "This victory was earned, and built upon the corpses of our foes";
							text3 = "The battle is done, but we now have other matters to attend to";
							CompanionBanter.option1Cost = Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers * 8;
							CompanionBanter.option2Cost = Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers * 2;
							bool flag12 = Hero.MainHero.Gold >= CompanionBanter.option1Cost;
							if (flag12)
							{
								text5 = "Great achievements deserve a matching feast, spare no expense";
							}
							else
							{
								text5 = "The men deserve a reward, but I cannot afford such a luxury at the moment";
							}
							bool flag13 = Hero.MainHero.Gold >= CompanionBanter.option2Cost;
							if (flag13)
							{
								text6 = "Purchase a few barrels of mead, let the men drink and be merry";
							}
							else
							{
								text6 = "A reward is indeed just, but not feasible at the moment";
							}
							text7 = "My soldiers have their pay and a share of the loot, that is enough";
						}
					}
					else if (num2 <= 140)
					{
						if (num2 != 130)
						{
							if (num2 == 140)
							{
								bool flag14 = CompanionBanter.honorable;
								if (flag14)
								{
									text = "My " + CompanionBanter.playerTitle + ", victory is ours once more.[rb:very_positive]";
									text4 = "We claim victory after victory my " + CompanionBanter.playerTitle + " and many ears hear of our deeds. But I wonder sometimes, how do the civilians see us.[rb:positive]";
								}
								else
								{
									bool flag15 = CompanionBanter.rogue;
									if (flag15)
									{
										text = "Your " + CompanionBanter.playerTitle + "ship, another victory for us! It's almost getting boring, almost.[rb:very_positive]";
										text4 = "People know of us, speak of us, be it good or bad, it helps our legend grow. That being said, we should remember our public image, we are after all as the masses see us.[rb:positive]";
									}
									else
									{
										bool flag16 = CompanionBanter.logical;
										if (flag16)
										{
											text = "Victory is yours my " + CompanionBanter.playerTitle + ", as always.[rb:very_positive]";
											text4 = "With every battle and deed we earn ever greater renown, but we must be weary of perspectives. Be it as heroes or villains, having a consistent persona will help us better achieve our desires.[rb:positive]";
										}
										else
										{
											text = "My " + CompanionBanter.playerTitle + ", the enemy is done for and we remain standing, a new notch on our victories belt.[rb:very_positive]";
											text4 = "Our belt has grown heavy with notches, I only hope that the common people see us with hope rather than despair.[rf:happy][rb:positive]";
										}
									}
								}
								text2 = "With every victory we earn ourselves more glory and renown";
								text3 = "The battle is over and the deed is done, let the men and I rest for a moment";
								text5 = "We are the protectors of the innocent and the outlaw hunters, that is how civilians should envision us";
								text6 = "The people should see us as a professional military force, unstoppable and unbreakable";
								text7 = "This force stands as an unmovable wall, the masses should recognize us as order, for that is what we are";
							}
						}
						else
						{
							bool flag17 = CompanionBanter.honorable;
							if (flag17)
							{
								text = "This day will be remembered in the epics my " + CompanionBanter.playerTitle + ", the enemy is naught.[rf:happy][rb:very_positive]";
								text4 = "We should send word and coin to the bards, have them tell this tale for all time to come.[rb:positive]";
							}
							else
							{
								bool flag18 = CompanionBanter.rogue;
								if (flag18)
								{
									text = "My " + CompanionBanter.playerTitle + ", we won? Don't tell the poor enemy troops, they seem to be still in shock.[rf:happy][rb:very_positive]";
									text4 = "This is the sort of tale that storytellers and bards dream of. I say let their tongues run loose.[rb:positive]";
								}
								else
								{
									bool flag19 = CompanionBanter.logical;
									if (flag19)
									{
										text = "A grand victory from the jaws of defeat, I expected nothing less of you my " + CompanionBanter.playerTitle + ".[rf:happy][rb:very_positive]";
										text4 = "I say let the bards sing of your praises and virtues your " + CompanionBanter.playerTitle + "ship, you have certainly earned it this day.[rb:positive]";
									}
									else
									{
										text = "An outstanding victory my " + CompanionBanter.playerTitle + ", our enemies will not easily forget this day.[rf:happy][rb:very_positive]";
										text4 = "I would suggest sending a few men with gold to line the pockets of tellers, so that the whole continent may hear of this achievement.[rb:positive]";
									}
								}
							}
							text2 = "Those weak bastards charge with numbers, but we have shown them their true worth";
							text3 = "This battle was hard fought and well earned, but now I must rest";
							CompanionBanter.option1Cost = Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers * 3;
							bool flag20 = Hero.MainHero.Gold > CompanionBanter.option1Cost;
							if (flag20)
							{
								text5 = "Then release the bards and have them leave no soul unaware";
							}
							else
							{
								text5 = "Our silver is better suited in the service of our brave soldiers";
							}
							text6 = "Perhaps, however I do have other ideas";
							text7 = "We do not need bards or tellers, our actions speak for themselves!";
						}
					}
					else if (num2 != 150)
					{
						if (num2 == 160)
						{
							bool flag21 = CompanionBanter.honorable;
							if (flag21)
							{
								text = "My " + CompanionBanter.playerTitle + ", we are victorious without a single wound. Incredible.[rf:happy][rb:very_positive]";
								text4 = "How you managed to break the enemy so perfectly amazes me. Your command is truly without flaw.[rf:happy][rb:very_positive]";
							}
							else
							{
								bool flag22 = CompanionBanter.rogue;
								if (flag22)
								{
									text = "We didn't even take a single real hit in that battle. Either they are incredibly weak, or you are too powerful my " + CompanionBanter.playerTitle + ".[rb:positive]";
									text4 = "Those poor souls, they thought they could win against you. Let those of them who live learn what true command looks like.[rb:positive]";
								}
								else
								{
									bool flag23 = CompanionBanter.logical;
									if (flag23)
									{
										text = "Your " + CompanionBanter.playerTitle + "ship, your command of this battle was exceptional, as is expected of you.[rb:very_positive]";
										text4 = "The perfection you have demonstrated in this battle is without equal my " + CompanionBanter.playerTitle + ". I commend you.[rb:positive]";
									}
									else
									{
										text = "How on earth did they not manage even a single wound? Are they that incompetent, or are we just that good?[rf:happy][rb:very_positive]";
										text4 = "I'm a bit in shock, they had the numbers, and yet it was so meaningless that we crushed them as if they were insects.[rf:happy][rb:very_positive]";
									}
								}
							}
							text2 = "They were weak and moronic, they never stood a chance";
							text3 = "It is my duty to protect the lives of my men, and I have done so, nothing else needs to be said";
							text5 = "The enemy was pitiful, their efforts to defeat us were in vain";
							text6 = "Any enemy poses a threat, but careful tactics and awareness will always protect the lives of our own";
							text7 = "Regardless of what the enemy is, my priority is always to bring our troops back alive";
						}
					}
					else
					{
						bool flag24 = CompanionBanter.honorable;
						if (flag24)
						{
							text = "The enemy is defeated and the glory is ours my " + CompanionBanter.playerTitle + ".[rb:very_positive]";
							text4 = "Times have been kind to us and our coffers fill with coin. We should share it with the less fortunate so that they may live better lives.[rb:positive]";
						}
						else
						{
							bool flag25 = CompanionBanter.rogue;
							if (flag25)
							{
								text = "A beautiful victory your " + CompanionBanter.playerTitle + "ship, and the plunder of it is all the sweeter.[rb:very_positive]";
								text4 = "While I'm not fond of throwing away our coin, I do suggest we use a bit to popularize ourselves with the nearby citizens, it can only do us well.[rb:positive]";
							}
							else
							{
								bool flag26 = CompanionBanter.logical;
								if (flag26)
								{
									text = "I salute your valor in battle my " + CompanionBanter.playerTitle + ", victory has been claimed.[rb:very_positive]";
									text4 = "We have a steady stream of coin, be it small or large, from said battles. I highly recommend we invest a portion in spreading our reputation among the masses.[rb:positive]";
								}
								else
								{
									text = "The day is won my " + CompanionBanter.playerTitle + ", and let none dare claim otherwise.[rb:very_positive]";
									text4 = "Our pouches are beginning to fill with every victory, but I feel a bit ashamed. While we grow wealthy most people around us strave and suffer, we should share a bit with them, show them some of the rare mercy of this world.[rf:happy][rb:positive]";
								}
							}
						}
						text2 = "Indeed, our victories and loot are accumulating nicely";
						text3 = "One battle at a time, that is how we'll proceed, you've done well today";
						CompanionBanter.option1Cost = 1000;
						text5 = "Very well then, send some coin to those nearby and let their leaders distribute as they see fit [" + CompanionBanter.option1Cost.ToString() + "{GOLD_ICON}]";
						text6 = "Then spread the coin among the notables, and remind them who it was that gifted them the coins of life [" + CompanionBanter.option1Cost.ToString() + "{GOLD_ICON}]";
						text7 = "Our coin is best suited here, among our ranks, where we can ensure it is put to proper use";
					}
				}
				else if (num2 <= 260)
				{
					if (num2 <= 200)
					{
						if (num2 != 170)
						{
							if (num2 == 200)
							{
								bool flag27 = CompanionBanter.honorable;
								if (flag27)
								{
									text = "It is done my " + CompanionBanter.playerTitle + ", the attackers are broken.[rf:happy][rb:very_positive]";
									text4 = "The villagers here will not soon forget what you have done for them.[rf:happy][rb:very_positive]";
								}
								else
								{
									bool flag28 = CompanionBanter.rogue;
									if (flag28)
									{
										text = "Good fight my " + CompanionBanter.playerTitle + ", the militia wasn't completely useless even.[rb:positive]";
										text4 = "It amazes me how these villagers survive. Between the brigands and the raging lords I would have lost my mind long ago.[rb:positive]";
									}
									else
									{
										bool flag29 = CompanionBanter.logical;
										if (flag29)
										{
											text = "My " + CompanionBanter.playerTitle + ", the people of this village shall honor you and celebrate this victory of yours.[rb:very_positive]";
											text4 = "You have earned both honor and gratitude on this day, this will serve us well over time.[rb:positive]";
										}
										else
										{
											text = "Off with those raiders, and may their corpses rot forever.[rf:happy][rb:very_positive]";
											text4 = "I'm glad we were here on this day, we spared the villagers much agony and suffering.[rf:happy][rb:very_positive]";
										}
									}
								}
								text2 = "They dared approach this village in our presence, that was their grave mistake";
								text3 = "Honor demands that we protect the innocent, and so we have, nothing further needs to be said";
								text5 = "Let the people of the village know that they are under our protection, any who seek to harm them shall feel our wrath";
								text6 = "We are the protectors of the innocents, let all know the fate of raiders in this region";
								text7 = "Gather a tribute from the village in exchange for our heroic service, I believe it is only just";
							}
						}
						else
						{
							bool flag30 = CompanionBanter.honorable;
							if (flag30)
							{
								text = "The outlaws have been scattered my " + CompanionBanter.playerTitle + ", may the heavens strike down any who remain.[rb:positive]";
								text4 = "While I appreciate that we are helping the nearby settlements, are there not more worthy targets to pursue my " + CompanionBanter.playerTitle + "?[rb:unsure]";
							}
							else
							{
								bool flag31 = CompanionBanter.rogue;
								if (flag31)
								{
									text = "And a few more brigands lay in the dirt, it's practically a routine at this point.[rb:unsure]";
									text4 = "I grow tired of such soft targets, we should seek larger and more valuable opponents.[rb:unsure]";
								}
								else
								{
									bool flag32 = CompanionBanter.logical;
									if (flag32)
									{
										text = "The land is cleansed of yet another band of criminals my " + CompanionBanter.playerTitle + ", though more will manifest, they always do.[rb:unsure]";
										text4 = "May I suggest we redirect our efforts to higher value targets, preferably those with lasting effect.[rb:unsure]";
									}
									else
									{
										text = "Another group of bandits has been swatted your " + CompanionBanter.playerTitle + "ship. I know I shouldn't enjoy this so much, but I do.[rb:very_positive]";
										text4 = "While I'm happy to rid the world of these parasites, they are truly never ending. Perhaps we can find new, less bothersome, targets?[rf:happy][rb:positive]";
									}
								}
							}
							text2 = "Brigands may rise, but they just as swiftly fall to our judgement";
							text3 = "Just another band of misfits squashed, nothing new under the sun";
							text5 = "Protecting settlements is important in the long run, they are our source of recruits. Naturally they are more cooperative with those who keep their surroundings safe.";
							text6 = "Destroying such criminals and miscreants is our duty, we will not falter in it.";
							text7 = "When a meaningful target presents itself we will strike, for now this is a mere distraction.";
						}
					}
					else if (num2 != 250)
					{
						if (num2 == 260)
						{
							bool flag33 = CompanionBanter.honorable;
							if (flag33)
							{
								text = "My " + CompanionBanter.playerTitle + ", the brigands are broken.[rf:happy][rb:very_positive]";
								text4 = "We've devastated the hold of these scum in the region. The area should know a bit more peace, for a while.[rf:happy][rb:very_positive]";
							}
							else
							{
								bool flag34 = CompanionBanter.rogue;
								if (flag34)
								{
									text = "Your " + CompanionBanter.playerTitle + "ship, nice job clearing this place out.[rb:positive]";
									text4 = "Those buggers were amateurs, no hidden lookouts, camps spread far between and worst yet, no good alcohol. Shameful display.[rb:positive]";
								}
								else
								{
									bool flag35 = CompanionBanter.logical;
									if (flag35)
									{
										text = "My " + CompanionBanter.playerTitle + ", this action will be the praise of every notable within ears reach. I congratulate you.[rb:very_positive]";
										text4 = "While I don't particularly enjoy hunting these brigands, breaking into their home seems oddly poetic and satisfying.[rb:positive]";
									}
									else
									{
										text = "You my " + CompanionBanter.playerTitle + " are a beautiful creature. Looters and bandits in the area will be running for hills, at least for a while.[rf:happy][rb:very_positive]";
										text4 = "I'm sure there are more brigands out there nearby, I only wish I could see their faces when they realize they've been raided themselves.[rf:happy][rb:very_positive]";
									}
								}
							}
							text2 = "These bastards are the scum of the earth, they deserve both the sword and the boot";
							text3 = "The area will know peace for a time now, alas I will not, for there is much to do";
							text5 = "Send word to the nearby settlements, let them enjoy the quiet while it lasts";
							text6 = "Burn the camp, leave nothing for any surviving stragglers to find";
							text7 = "Let us take a quick piss upon their graves, so that they may know how much we cherish them";
						}
					}
					else
					{
						bool flag36 = CompanionBanter.honorable;
						if (flag36)
						{
							text = "My " + CompanionBanter.playerTitle + ", victory is ours. Then again, victory is a given for our band of heroes.[rb:very_positive]";
							text4 = "So what are your plans for our future? Shall we continue to fight as a small elite band or do you plan to assemble a larger force?[rf:happy][rb:positive]";
						}
						else
						{
							bool flag37 = CompanionBanter.rogue;
							if (flag37)
							{
								text = "My " + CompanionBanter.playerTitle + ", the day is won. Unfortunately the enemy didn't stick around to hear our cheers.[rb:very_positive]";
								text4 = "While I enjoy the company of a tight knit group I must ask, should we expect few additions to our troop, or many?[rb:positive]";
							}
							else
							{
								bool flag38 = CompanionBanter.logical;
								if (flag38)
								{
									text = "Your " + CompanionBanter.playerTitle + "ship, victory is yours, with us your faithful followers as witnesses.[rb:very_positive]";
									text4 = "I must admit, I enjoy the company of this band. Though I do wonder if it would not be better to allow our numbers to grow.[rb:positive]";
								}
								else
								{
									text = "A good battle my " + CompanionBanter.playerTitle + ", though the enemy never stood a chance.[rb:very_positive]";
									text4 = "We stand as a small yet powerful fighting force, as you have made us to be. Perhaps there is wisdom then in using your leadership to add greater numbers to our own.[rf:happy][rb:positive]";
								}
							}
						}
						text2 = "Our force may be small, but we are more than capable of banishing the enemy";
						text3 = "That is quite true, but I fear my attention is needed elsewhere";
						text5 = "I intend to grow this force into an army that will tremble the earth with its march";
						text6 = "We will select only the best and most valuable of recruits, those who would bring honor to this company";
						text7 = "This group was built as an efficient and elite battle group, I plan to keep it this way";
					}
				}
				else if (num2 <= 410)
				{
					if (num2 != 400)
					{
						if (num2 == 410)
						{
							bool flag39 = CompanionBanter.honorable;
							if (flag39)
							{
								text = "My " + CompanionBanter.playerTitle + ", I must speak to you about our new supplies.";
								text4 = "Confistating good this way, it makes us no better than brigands. Was there no way to avoid this? Could we not afford to purchase the supplies?[if:idle_pleased][rb:unsure]";
							}
							else
							{
								bool flag40 = CompanionBanter.rogue;
								if (flag40)
								{
									text = "About our fresh supplies my " + CompanionBanter.playerTitle + ", I have some concerns.";
									text4 = "I like fresh meat and a good mug of beer as much as anyone, but was it worth it raiding like that? We're not an unknown band, our actions will be remembered.[rb:unsure]";
								}
								else
								{
									bool flag41 = CompanionBanter.logical;
									if (flag41)
									{
										text = "Your " + CompanionBanter.playerTitle + "ship, we should discuss this recent supplies grab.";
										text4 = "While I understand the need for supplies, were we genuinely so desperate that tarnishing our name is justified?[if:idle_pleased][rb:unsure]";
									}
									else
									{
										text = "My " + CompanionBanter.playerTitle + ", I have some reservations regarding these supplies we've gathered.";
										text4 = "We've effectively brought those people to ruins, and for what? We could have easily purchased what we needed, this was unjust.[if:idle_pleased][rb:unsure]";
									}
								}
							}
							text2 = "Is there a problem with the supplies?";
							text3 = "Our force has what it needs, unless there is something else this conversation is over";
							text5 = "Supplies are the lifeline of any fighting force. We acquired what was needed to continue our mission, nothing more or less";
							text6 = "Would you prefer to see our troops starving or lacking in essentials? I did what had to be done to do my men justice";
							text7 = "We've secured valued resources and damaged the abilities of potential threats, that is all the reason that is needed";
						}
					}
					else
					{
						bool flag42 = CompanionBanter.honorable;
						if (flag42)
						{
							text = "I must speak with you about this raid my " + CompanionBanter.playerTitle + ".";
							text4 = "Raiding like this, it makes us no better than brigands. I ask you, why have we done this?[if:idle_pleased][rb:unsure]";
						}
						else
						{
							bool flag43 = CompanionBanter.rogue;
							if (flag43)
							{
								text = "My " + CompanionBanter.playerTitle + ", I must have words with you.";
								text4 = "So, we got two pieces of bread and a bucket of tears from that raid. Was it worth it?[rb:unsure]";
							}
							else
							{
								bool flag44 = CompanionBanter.logical;
								if (flag44)
								{
									text = "My " + CompanionBanter.playerTitle + ", I wish to speak to you, if I may.";
									text4 = "This raid, it has cost us reputation and will stain our image for some time. I do not see the gains worthy of the sacrifice. Unless you have a different perspective?[if:idle_pleased][rb:unsure]";
								}
								else
								{
									text = "My " + CompanionBanter.playerTitle + ", these is something I must lay off my chest.";
									text4 = "I have followed you faithfully, but on this day my faith is shaken. However I turn it in my head, I cannot justify our actions. Why have we done this? Why?[if:idle_pleased][rb:unsure]";
								}
							}
						}
						text2 = "You seem troubled, what seems to be the matter?";
						text3 = "There are more pressing matters to attend to I'm afraid";
						text5 = "What was done was necessary. I will not deny that it was a dark act, but if it could have been avoided, then I would have chosen another path.";
						text6 = "Our party, our troops needed supplies and our enemies needed to be weakened. Harsh as it may seem, this world we live in demands sacrifices. I would rather sacrifice the stranger, rather than my own.";
						text7 = "They had resources that we needed, the weak may beg and barter, but the strong simply take. That is what we have done.";
					}
				}
				else if (num2 != 420)
				{
					if (num2 == 900)
					{
						bool flag45 = CompanionBanter.honorable;
						if (flag45)
						{
							switch (Support.Random(1, 4))
							{
							case 1:
								text = "My " + CompanionBanter.playerTitle + ", we must speak, immediately!";
								break;
							case 2:
								text = "My " + CompanionBanter.playerTitle + ", I must speak to you at once.";
								break;
							case 3:
								text = "Your " + CompanionBanter.playerTitle + "ship, there is something you should know.";
								break;
							default:
								text = "We must speak immediately my " + CompanionBanter.playerTitle + "!";
								break;
							}
							switch (Support.Random(1, 4))
							{
							case 1:
								text4 = "I have waited and waited, but I do not see this company changing in the direction I desire. I no longer wish to be part of it.";
								break;
							case 2:
								text4 = "I have been patient and tolerant, but no more. I have reached the last straw and will be leaving.";
								break;
							case 3:
								text4 = "Call me a coward if you will, but I have no further desire to put my life at risk for you.";
								break;
							default:
								text4 = "I grow tired of this. I have no more will to fight for you.";
								break;
							}
						}
						else
						{
							bool flag46 = CompanionBanter.rogue;
							if (flag46)
							{
								switch (Support.Random(1, 4))
								{
								case 1:
									text = "My " + CompanionBanter.playerTitle + ", I will have words with you.";
									break;
								case 2:
									text = "My " + CompanionBanter.playerTitle + ", I have something you need to hear.";
									break;
								case 3:
									text = "I will speak, and you will hear me now your " + CompanionBanter.playerTitle + "ship.";
									break;
								default:
									text = "You will hear me out NOW my " + CompanionBanter.playerTitle + "!";
									break;
								}
								switch (Support.Random(1, 4))
								{
								case 1:
									text4 = "I have waited long enough, and tolerated far more that I should have. I am resigning from this party of yours.";
									break;
								case 2:
									text4 = "I don't mind risking my life, but not for you, not any longer. My service to you is at an end I fear.";
									break;
								case 3:
									text4 = "I have no more will to fight for you, or for anyone else for that matter. Thus, I wish to leave.";
									break;
								default:
									text4 = "I have no more desire to sweat and bleed for you, though I would definitely shit for you. It is time for me to go.";
									break;
								}
							}
							else
							{
								bool flag47 = CompanionBanter.logical;
								if (flag47)
								{
									switch (Support.Random(1, 4))
									{
									case 1:
										text = "I must speak to you at once my " + CompanionBanter.playerTitle + ".";
										break;
									case 2:
										text = "My " + CompanionBanter.playerTitle + ", I ask that you hear what I must say.";
										break;
									case 3:
										text = "I must have words with you my " + CompanionBanter.playerTitle + ".";
										break;
									default:
										text = "My " + CompanionBanter.playerTitle + ", we must speak, if you would be so kind.";
										break;
									}
									switch (Support.Random(1, 4))
									{
									case 1:
										text4 = "The heavens seem to have forsaken me within this company. I wish to try to appease them elsewhere.";
										break;
									case 2:
										text4 = "My service to you cannot continue your " + CompanionBanter.playerTitle + "ship. I beg your pardon, but I am done.";
										break;
									case 3:
										text4 = "I cannot remain part of this company I'm afraid. Forgive me, but I must leave.";
										break;
									default:
										text4 = "There is no end to my misery in this company. I no longer wish to remain here.";
										break;
									}
								}
								else
								{
									switch (Support.Random(1, 4))
									{
									case 1:
										text = "My " + CompanionBanter.playerTitle + ", we must speak!";
										break;
									case 2:
										text = "My " + CompanionBanter.playerTitle + ", there is something you must know.";
										break;
									case 3:
										text = "I must inform you of something my " + CompanionBanter.playerTitle + ".";
										break;
									default:
										text = "We must speak, immediately!";
										break;
									}
									switch (Support.Random(1, 4))
									{
									case 1:
										text4 = "It is time for us to part ways. I have no interest in continuing to serve you.";
										break;
									case 2:
										text4 = "There is no more fight within me, not for this company. I wish to leave.";
										break;
									case 3:
										text4 = "My mind and my soul are in ruins. I do not wish to abandon you my " + CompanionBanter.playerTitle + ", but I cannot remain in your service.";
										break;
									default:
										text4 = "Forgive me, but I cannot remain part of this company. I must leave.";
										break;
									}
								}
							}
						}
						text += "[if:idle_pleased][rb:unsure]";
						text4 += "[if:idle_pleased][rb:unsure]";
						text2 = "What seems to be the matter?";
						text3 = "I do not wish to speak to you at the moment";
						bool flag48 = CompanionBanter.playerCharm <= 50;
						if (flag48)
						{
							text5 = "You must remain here, I need you";
						}
						else
						{
							bool flag49 = CompanionBanter.playerCharm <= 100;
							if (flag49)
							{
								text5 = "I ask that you reconsider, you are a valued member of this company and your loss will be deeply felt";
							}
							else
							{
								bool flag50 = CompanionBanter.playerCharm > 100;
								if (flag50)
								{
									text5 = "I understand your concerns, but what is done is done, and now I ask you to stand with me as we move forward, together";
								}
							}
						}
						CompanionBanter.option2Cost = CompanionBanter.hero.Level * 150;
						bool flag51 = CompanionBanter.heroTraits.Honor < 0;
						if (flag51)
						{
							CompanionBanter.option2Cost = (int)((double)CompanionBanter.option2Cost * 1.25);
						}
						bool flag52 = CompanionBanter.heroTraits.Generosity < 0;
						if (flag52)
						{
							CompanionBanter.option2Cost = (int)((double)CompanionBanter.option2Cost * 2.0);
						}
						bool flag53 = Hero.MainHero.Gold >= CompanionBanter.option2Cost;
						if (flag53)
						{
							text6 = "Perhaps a pouch of well earned coins can change your mind? [" + CompanionBanter.option2Cost.ToString() + "{GOLD_ICON}]";
						}
						else
						{
							text6 = "I can only offer you this pouch of coin, all that is at my disposal, in the hopes that you change your mind [" + Hero.MainHero.Gold.ToString() + "{GOLD_ICON}]";
						}
						text7 = "Off with you then!";
					}
				}
				else
				{
					bool flag54 = CompanionBanter.honorable;
					if (flag54)
					{
						text = "We must speak of the new recruits my " + CompanionBanter.playerTitle + ".";
						text4 = "How exactly do you expect to make soldiers out of forced recruits? They despise you, and with good reason.[if:idle_pleased][rb:unsure]";
					}
					else
					{
						bool flag55 = CompanionBanter.rogue;
						if (flag55)
						{
							text = "My " + CompanionBanter.playerTitle + ", we need to talk about the new elephant in the room.";
							text4 = "Getting new recruits sounds fantastic, problem is I'm pretty sure some of them want to slit your throat, and mine. Are they even worth it?[rb:unsure]";
						}
						else
						{
							bool flag56 = CompanionBanter.logical;
							if (flag56)
							{
								text = "My " + CompanionBanter.playerTitle + ", we must discuss the matter the of the fresh bloods, if you have moment.";
								text4 = "We've branded ourselves outlaws and criminals for a band of armed peasants? This trade seems illogical.[if:idle_pleased][rb:unsure]";
							}
							else
							{
								text = "I must speak to you of our new recruitment method my " + CompanionBanter.playerTitle + ".";
								text4 = "Surely there's a better way to get recruits then to force them from their homes, isn't there? I can't imagine such recruits being good for morale, or even skilled enough to hold their own for that matter.[if:idle_pleased][rb:unsure]";
							}
						}
					}
					text2 = "What about the new recruits?";
					text3 = "We needed recruits, we acquired then, that is all";
					text5 = "These men will now get to earn their honor, have their bellies filled, their pouches will fill with gold and their renown will soar. And in the process, they will reinforce our troops further thus increasing our might.";
					text6 = "Our numbers needed immediate bolstering. I would rather make a few peasants unhappy then risk the lives of my men through an underpowered force.";
					text7 = "Necessaty! We must acquire manpower and resources where we can to move forward. I will not apologize for strengthening my forces.";
				}
				MBTextManager.SetTextVariable("HLC_PREINTRO", text, false);
				MBTextManager.SetTextVariable("HLC_PREINTRO_RESPONSE", text2, false);
				MBTextManager.SetTextVariable("HLC_PREINTRO_END", text3, false);
				MBTextManager.SetTextVariable("HLC_INTRO", text4, false);
				MBTextManager.SetTextVariable("HLC_CHOICE1", text5, false);
				MBTextManager.SetTextVariable("HLC_CHOICE2", text6, false);
				MBTextManager.SetTextVariable("HLC_CHOICE3", text7, false);
				CompanionBanter.banterID = 0;
				CompanionBanter.subBanterID = 0;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void Choice_1_Effect()
		{
			string text = "Yes my " + CompanionBanter.playerTitle + ".";
			bool flag = CompanionBanter.subBanterID > 0;
			if (flag)
			{
				int num = CompanionBanter.subBanterID;
				int num2 = num;
				if (num2 <= 160)
				{
					if (num2 <= 120)
					{
						if (num2 != 100)
						{
							if (num2 != 110)
							{
								if (num2 == 120)
								{
									Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1f;
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 20);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 5);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 30);
									Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 10f);
									Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 30f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 10f);
									bool flag2 = CompanionBanter.honorable;
									if (flag2)
									{
										text = "You are without a doubt correct my " + CompanionBanter.playerTitle + ". An armored knight is worth a hundred peasants.[rf:happy][ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
									}
									else
									{
										bool flag3 = CompanionBanter.rogue;
										if (flag3)
										{
											text = "Clever... We crush the lesser enemies while maintaining much of our force though armor and skill. So as the enemy crumbles from defeats we remain strong.[rf:happy][rb:very_positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
										}
										else
										{
											bool flag4 = CompanionBanter.logical;
											if (flag4)
											{
												text = "A logical approach, though one should take the cost into consideration. Soldiers are expensive, but a heavy mail is far more so.[ib:confident2][rb:positive]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
											}
											else
											{
												text = "Armed, armored and ready for a fight. It also helps that many of the enemy fighters flee at the sight of us. Haha![ib:confident2][rb:positive]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
											}
										}
									}
									Support.LogMessage("You troops stand tall for their commander, ready to prove " + (Hero.MainHero.IsFemale ? "her" : "his") + " words true.");
								}
							}
							else
							{
								bool flag5 = Hero.MainHero.Gold >= CompanionBanter.option1Cost;
								if (flag5)
								{
									Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 3f;
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers / 3);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers);
									GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, CompanionBanter.option1Cost, false);
									Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 25f);
									Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50f);
									Hero.MainHero.AddSkillXp(DefaultSkills.Medicine, 30f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 5f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 10f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Medicine, 5f);
									bool flag6 = CompanionBanter.honorable;
									if (flag6)
									{
										text = "Your actions speak loudly my " + CompanionBanter.playerTitle + ", your will be done.[rf:happy][ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
									else
									{
										bool flag7 = CompanionBanter.rogue;
										if (flag7)
										{
											text = "Extra medicine is nice and all, but avoiding needing it would be even better.[if:idle_pleased][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-4, -1));
										}
										else
										{
											bool flag8 = CompanionBanter.logical;
											if (flag8)
											{
												text = "Certainly, I will see to it at once. However, this does not change our high casualty rate.[if:idle_pleased][rb:unsure]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
											}
											else
											{
												text = "I'll take care of that, and I'm sure the men will appreciate it.[if:idle_pleased][rb:unsure]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
											}
										}
									}
									Support.LogMessage("Your soldiers swear that they will rise even stronger to fight for you once more.");
								}
								else
								{
									Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 1f;
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, -60);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers / 6);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers);
									bool flag9 = CompanionBanter.honorable;
									if (flag9)
									{
										text = "I'm glad you understand my " + CompanionBanter.playerTitle + ". I recognize that you must make difficult decisions, all I ask is that you strive to protect our men as we seek victory.[if:idle_pleased][ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										bool flag10 = CompanionBanter.rogue;
										if (flag10)
										{
											text = "No disrespect your " + CompanionBanter.playerTitle + "ship, but words are cheap. The soldiers need to see action.[if:idle_pleased][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-6, -3));
										}
										else
										{
											bool flag11 = CompanionBanter.logical;
											if (flag11)
											{
												text = "As you say, but know that the men will weigh your defeats far more heavily then your victories, and act accordingly.[if:idle_pleased][rb:unsure]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
											}
											else
											{
												text = "I pray that you do my " + CompanionBanter.playerTitle + ". I'm not particularly fond of the idea of dying in a meat grinder.[if:idle_pleased][rb:unsure]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-2, 2));
											}
										}
									}
									Support.LogMessage("The faith of the men in you is slightly shaken for a leader cannot afford mistakes.");
								}
							}
						}
						else
						{
							bool flag12 = Hero.MainHero.Gold >= CompanionBanter.option1Cost;
							if (flag12)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 10f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 10);
								GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, CompanionBanter.option1Cost, false);
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 25f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 10f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 5f);
								bool flag13 = CompanionBanter.honorable;
								if (flag13)
								{
									text = "Your generosity knows no bounds my " + CompanionBanter.playerTitle + ", it will be done.[rf:happy][ib:confident][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(2, 3));
								}
								else
								{
									bool flag14 = CompanionBanter.rogue;
									if (flag14)
									{
										text = "A nice reward is always to be appreciated, and don't worry I'll be sure to take my cut.[rf:happy][ib:confident2][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
									}
									else
									{
										bool flag15 = CompanionBanter.logical;
										if (flag15)
										{
											text = "As you command my " + CompanionBanter.playerTitle + ", the men will eat well and toast to their leader this day.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
										else
										{
											text = "My " + CompanionBanter.playerTitle + ", I'll see to it at one.[rf:happy][ib:confident2][rb:very_positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
										}
									}
								}
								Support.LogMessage("Your troops feast as kings and rejoice.");
							}
							else
							{
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers / 3);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -10);
								bool flag16 = CompanionBanter.honorable;
								if (flag16)
								{
									text = "That's a shame, but as you said, it is a luxury after all, they can live without it.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									bool flag17 = CompanionBanter.rogue;
									if (flag17)
									{
										text = "Well, that's too bad. I was looking forward to some extra loot. Oh well.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										bool flag18 = CompanionBanter.logical;
										if (flag18)
										{
											text = "Yes your " + CompanionBanter.playerTitle + "ship, no reason to waste resources unnecessarily.[ib:confident2][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
										else
										{
											text = "Well my " + CompanionBanter.playerTitle + ", we've got our share of the loot and our pay, that's good enough for us.[ib:confident2][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
									}
								}
							}
						}
					}
					else if (num2 <= 140)
					{
						if (num2 != 130)
						{
							if (num2 == 140)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 3f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 25);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 2);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 10);
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 10f);
								CompanionBanter.ChangeSettlementRelations(1, 1000.0);
								bool flag19 = CompanionBanter.honorable;
								if (flag19)
								{
									text = "Naturally my " + CompanionBanter.playerTitle + ", we shall never hesitate to crush the never ending brigands and end the murderous raids.[rf:happy][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
								}
								else
								{
									bool flag20 = CompanionBanter.rogue;
									if (flag20)
									{
										text = "Playing the hero is nice and all my " + CompanionBanter.playerTitle + ", but we should focus more on control and respect. The problem with playing the hero is that if things go wrong, everyone will blame you.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										bool flag21 = CompanionBanter.logical;
										if (flag21)
										{
											text = "I see your perspective, playing the part of the protector will generate belief in us and rally the willing to our ranks. A reasonable play.[ib:confident2][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
										else
										{
											text = "We keep the people safe, that is our purpose. I cannot imagine a more noble purpose my " + CompanionBanter.playerTitle + ".[rf:happy][rb:very_positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 6));
										}
									}
								}
								Support.LogMessage("Your men are boldened by your words and vow to hunt down every criminal they can.");
							}
						}
						else
						{
							bool flag22 = Hero.MainHero.Gold > CompanionBanter.option1Cost;
							if (flag22)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 3f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 3);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 30);
								GainRenownAction.Apply(Hero.MainHero, (float)((int)MathF.Clamp((float)Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 6f, 5f, 50f)), false);
								GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, CompanionBanter.option1Cost, false);
								bool flag23 = CompanionBanter.honorable;
								if (flag23)
								{
									text = "I will send word and coin then my " + CompanionBanter.playerTitle + ", all will hear of this.[rf:happy][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									bool flag24 = CompanionBanter.rogue;
									if (flag24)
									{
										text = "Right then, I'll send some runners and birds. I reckon we'll hear a new stanza or two soon.[rf:happy][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
									else
									{
										bool flag25 = CompanionBanter.logical;
										if (flag25)
										{
											text = "By your leave my  " + CompanionBanter.playerTitle + ", I will see to it.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
										else
										{
											text = "The song of this day will spread to every tavern on the continent, soon enough.[rf:happy][rb:very_positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
									}
								}
								Support.LogMessage("Word of your heroic victory will spread far and wide.");
							}
							else
							{
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 6);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -10);
								Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 10f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 30f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 10f);
								bool flag26 = CompanionBanter.honorable;
								if (flag26)
								{
									text = "We do not need bards, we've earned our own renown this day.[rf:happy][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									bool flag27 = CompanionBanter.rogue;
									if (flag27)
									{
										text = "It would be easier to wet some lips rather then our blades to get renown, but it's your choice.[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-4, 0));
									}
									else
									{
										bool flag28 = CompanionBanter.logical;
										if (flag28)
										{
											text = "Indeed, best to conserve our funds for supplies and equipment. Renown means little if we are weakened before we can use it.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
										else
										{
											text = "People talk and nearbies will witness, they will carry the word, with or without coin.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
									}
								}
							}
						}
					}
					else if (num2 != 150)
					{
						if (num2 == 160)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1.2f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 50);
							GainRenownAction.Apply(Hero.MainHero, (float)((int)MathF.Clamp((float)(Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 8), 5f, 50f)), false);
							Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 30f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
							bool flag29 = CompanionBanter.honorable;
							if (flag29)
							{
								text = "Indeed my " + CompanionBanter.playerTitle + ", though they made good target practice.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(2, 3));
							}
							else
							{
								bool flag30 = CompanionBanter.rogue;
								if (flag30)
								{
									text = "Pitiful or not, never underestimate ate the enemy. Though they definitely deserved to be underestimated in this battle.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
								else
								{
									bool flag31 = CompanionBanter.logical;
									if (flag31)
									{
										text = "Certainly your " + CompanionBanter.playerTitle + "ship, they are no equals to your brilliance, not even remotely.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										text = "They must have been a sorry band if they were crushed so easily. Not to underplay your own excellent command my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
								}
							}
							Support.LogMessage("Your soldiers laugh in memory of the worthless enemy.");
						}
					}
					else
					{
						Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 2f;
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, CompanionBanter.option1Cost / 10);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, CompanionBanter.option1Cost / 20);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -CompanionBanter.option1Cost / 20);
						Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 120f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 20f);
						GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, CompanionBanter.option1Cost, false);
						CompanionBanter.ChangeSettlementRelations(3, 1000.0);
						bool flag32 = CompanionBanter.honorable;
						if (flag32)
						{
							text = "The people will cheer your name for this my " + CompanionBanter.playerTitle + ", I will see to it.[rf:happy][rb:positive]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
						}
						else
						{
							bool flag33 = CompanionBanter.rogue;
							if (flag33)
							{
								text = "As nice of a gesture as that is, we should have probably linked that to us somehow, let the coin spread our fame. Then again I grow fat with your generosity, so I am in no position to complain.[ib:confident2][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
							}
							else
							{
								bool flag34 = CompanionBanter.logical;
								if (flag34)
								{
									text = "A true act of generosity and mercy, I hope these civilians learn swiftly to whom they owe their new found comfort.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									text = "This deed will feed the hungry and heal the wounded, you are most generous and honorable my " + CompanionBanter.playerTitle + ".[rf:happy][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 7));
								}
							}
						}
						Support.LogMessage("All those who live nearby will not soon forget your generosity.");
					}
				}
				else if (num2 <= 260)
				{
					if (num2 <= 200)
					{
						if (num2 != 170)
						{
							if (num2 == 200)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1.5f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 40);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 20);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 20);
								GainRenownAction.Apply(Hero.MainHero, (float)((int)MathF.Clamp((float)(Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 12), 5f, 50f)), false);
								CompanionBanter.ChangeSettlementRelations(10, 10.0);
								bool flag35 = CompanionBanter.honorable;
								if (flag35)
								{
									text = "I will spread the word personally my " + CompanionBanter.playerTitle + ", it would be my honor.[rf:happy][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
								}
								else
								{
									bool flag36 = CompanionBanter.rogue;
									if (flag36)
									{
										text = "So, we're vowing to protect this place? What happens if it gets attached while we're halfway across the world? Oh well, not my problem.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										bool flag37 = CompanionBanter.logical;
										if (flag37)
										{
											text = "As you wish my " + CompanionBanter.playerTitle + ", though I wouldn't recommend dedicating too many resources. Protecting a village like this is from a real raid is nigh impossible.[ib:confident2][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
										}
										else
										{
											text = "Then I vow to protect this place with you my " + CompanionBanter.playerTitle + ", let these people enjoy a semblance of a happy life.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
									}
								}
								Support.LogMessage("The residents revere you and your men as heroes.");
							}
						}
						else
						{
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 50);
							Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20f);
							Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 40f);
							Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 10f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 5f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 5f);
							CompanionBanter.ChangeSettlementRelations(1, 1000.0);
							bool flag38 = CompanionBanter.honorable;
							if (flag38)
							{
								text = "Ofcourse, your logic is sound my " + CompanionBanter.playerTitle + ", we protect the innocent and gain their favor. A worthy objective.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
							}
							else
							{
								bool flag39 = CompanionBanter.rogue;
								if (flag39)
								{
									text = "Playing the long game I see, clever as ever my " + CompanionBanter.playerTitle + ".[rf:happy][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									bool flag40 = CompanionBanter.logical;
									if (flag40)
									{
										text = "Your logic is sound, it can only benefit us as time progresses. That being said, I would still appreciate a change in opponents.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										text = "As always you are thinking far ahead of me my " + CompanionBanter.playerTitle + ", I had not taken that into consideration. Off to hunt more bandits then![rf:happy][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
									}
								}
							}
							Support.LogMessage("Your soldiers smile, knowing that your choice will make them warmly welcomed in towns and villages.");
						}
					}
					else if (num2 != 250)
					{
						if (num2 == 260)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 2.5f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 25);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 25);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 25);
							GainRenownAction.Apply(Hero.MainHero, 20f, false);
							CompanionBanter.ChangeSettlementRelations(2, 1000.0);
							bool flag41 = CompanionBanter.honorable;
							if (flag41)
							{
								text = "Good call my " + CompanionBanter.playerTitle + ", settlements rarely get a moment of peace. I'm glad they will finally enjoy it, if only for a short time.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
							}
							else
							{
								bool flag42 = CompanionBanter.rogue;
								if (flag42)
								{
									text = "I bet a few villagers will come here looking for loot soon, under the claim of confirming the hideout is gone themselves, ofcourse.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									bool flag43 = CompanionBanter.logical;
									if (flag43)
									{
										text = "The people of this region will surely be grateful your " + CompanionBanter.playerTitle + "ship, as they should be.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										text = "My " + CompanionBanter.playerTitle + ", I can already smell meat cooking in celebration. We should consider visiting one of the nearby settlements, partake in the festivities.[rf:happy][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
								}
							}
							Support.LogMessage("The nearby settlements rejoice with the destruction of the hideout.");
						}
					}
					else
					{
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 20);
						Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 100f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 10f);
						bool flag44 = CompanionBanter.honorable;
						if (flag44)
						{
							text = "Men will flock to serve you my " + CompanionBanter.playerTitle + ", our force will grow in no time at all, if that is what you desire.[ib:confident2][rb:positive]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
						}
						else
						{
							bool flag45 = CompanionBanter.rogue;
							if (flag45)
							{
								text = "Good! More manpower means bigger targets, and the bigger they are the more loot they will bring.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 4));
							}
							else
							{
								bool flag46 = CompanionBanter.logical;
								if (flag46)
								{
									text = "An excellent choice, a more significant force will surely allow us to leave a larger footprint within this world.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									text = "Sounds quite terrifying my " + CompanionBanter.playerTitle + ", to our enemies that is. I look forward to meeting our new comrades.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
							}
						}
						Support.LogMessage("You men smile at the prospect of larger battles and greater glory.");
					}
				}
				else if (num2 <= 410)
				{
					if (num2 != 400)
					{
						if (num2 == 410)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 0.8f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -100);
							Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 100f);
							bool flag47 = CompanionBanter.honorable;
							if (flag47)
							{
								text = "Required or not, what we did was dishonorable![if:idle_angry][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-10, -5));
							}
							else
							{
								bool flag48 = CompanionBanter.rogue;
								if (flag48)
								{
									text = "Fair enough, I understand. We needed resources on the fly and they had them, why pay when you can simply claim.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(0, 1));
									Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								}
								else
								{
									bool flag49 = CompanionBanter.logical;
									if (flag49)
									{
										text = "The new supplies are welcomed, but I hope their value outweighs their consequences, my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-1, 0));
										Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
									}
									else
									{
										text = "This was a travesty of justice my " + CompanionBanter.playerTitle + ", we needed it so we took it. Is that not the mentality of brigands?[ib:confident2][rb:very_negative]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-7, -3));
									}
								}
							}
							Support.LogMessage("Your actions today will not soon be forgotten.");
						}
					}
					else
					{
						Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 1f;
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -90);
						Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 100f);
						CompanionBanter.ChangeSettlementRelations(-5, 1000.0);
						bool flag50 = CompanionBanter.honorable;
						if (flag50)
						{
							text = "Necessary? We could have easily sold something and bought what we needed. Or are you thinking of denying some enemy? All we did was destroy innocent lives.[if:idle_angry][rb:very_negative]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-15, -5));
						}
						else
						{
							bool flag51 = CompanionBanter.rogue;
							if (flag51)
							{
								text = "Necessary or not, we just earned hatred and enemies. Not that we lack any of those to begin with. Oh well, better others misery than mine.[ib:confident2][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
							}
							else
							{
								bool flag52 = CompanionBanter.logical;
								if (flag52)
								{
									text = "Very well, you have my complete trust. If you believe it was necessary then so be it.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								}
								else
								{
									text = "My " + CompanionBanter.playerTitle + ", this is no justification. You could have found another way, we should have.[if:idle_angry][rb:very_negative]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-7, -3));
								}
							}
						}
						Support.LogMessage("This raid will be remembered.");
					}
				}
				else if (num2 != 420)
				{
					if (num2 == 900)
					{
						bool flag53 = (Support.Random(1, CompanionBanter.playerCharm) <= 25 && CompanionBanter.honorable) || (Support.Random(1, CompanionBanter.playerCharm) <= 50 && CompanionBanter.kind) || (Support.Random(1, CompanionBanter.playerCharm) <= 75 && CompanionBanter.logical) || (Support.Random(1, CompanionBanter.playerCharm) <= 100 && CompanionBanter.rogue);
						if (flag53)
						{
							bool flag54 = CompanionBanter.honorable;
							if (flag54)
							{
								switch (Support.Random(1, 4))
								{
								case 1:
									text = "Your words mean nothing to me. Farewell.";
									break;
								case 2:
									text = "I will hear no more from you. Goodbye.";
									break;
								case 3:
									text = "You cannot change my mind. Good day to you.";
									break;
								default:
									text = "My choice is already made.";
									break;
								}
								text += "[if:idle_angry][rb:very_negative]";
							}
							else
							{
								bool flag55 = CompanionBanter.rogue;
								if (flag55)
								{
									switch (Support.Random(1, 4))
									{
									case 1:
										text = "Nice try, but not good enough. I'm done.";
										break;
									case 2:
										text = "That was a good attempt, sort of. Alas, it failed. I'll be leaving now.";
										break;
									case 3:
										text = "That was a fair try, but words won't fix this. See ya around.";
										break;
									default:
										text = "You had your say, but it hasn't changed much I'm afraid. I'm off.";
										break;
									}
									text += "[if:idle_pleased][rb:unsure]";
								}
								else
								{
									bool flag56 = CompanionBanter.logical;
									if (flag56)
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "Your attempt is commendable, though insufficient. I bid you good day.";
											break;
										case 2:
											text = "You have spoken in vain. Godspeed.";
											break;
										case 3:
											text = "Sweetened words will not fix what has been broken. Perhaps time will. For now, farewell.";
											break;
										default:
											text = "Unfortunately for you, I find your words lacking. I will be taking my leave now.";
											break;
										}
										text += "[if:idle_pleased][rb:unsure]";
									}
									else
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "Forgive me, but I am not convinced. Perhaps we'll meet again someday.";
											break;
										case 2:
											text = "Your words almost ring true, almost. I'll drink to your health, though for now I will be leaving.";
											break;
										case 3:
											text = "I thank you for sentiment, but it isn't enough. Farewell.";
											break;
										default:
											text = "Words will not mend this, actions may have, but now it's too late. I wish you luck with your ambitions.";
											break;
										}
									}
								}
							}
							text += "[if:idle_pleased][rb:unsure]";
							Support.RemoveCompanion(CompanionBanter.hero, true);
						}
						else
						{
							Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 250f);
							bool flag57 = CompanionBanter.honorable;
							if (flag57)
							{
								switch (Support.Random(1, 4))
								{
								case 1:
									text = "I will hold my peace then, and remain as I was. This does not mean I am appeased however.";
									break;
								case 2:
									text = "You damned smooth talker. Fine, I will stay a while longer.";
									break;
								case 3:
									text = "I do not wish to turn my back to you, nor will i. For the time being, I'll remain.";
									break;
								default:
									text = "You know my thoughts now, and I hope you can change them. I will fall in line for the time being.";
									break;
								}
								text += "[rf:happy][ib:confident2][rb:positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(15, 25));
							}
							else
							{
								bool flag58 = CompanionBanter.rogue;
								if (flag58)
								{
									switch (Support.Random(1, 4))
									{
									case 1:
										text = "Damn it " + (Hero.MainHero.IsFemale ? "woman" : "man") + ", you could talk a snail out of its bloody shell. Fine, I'll stay.";
										break;
									case 2:
										text = "You silver tongued bugger. Alright, I'll stick around a while longer.";
										break;
									case 3:
										text = "Alright, alright. I'll hang up my complaints for the moment.";
										break;
									default:
										text = "Well, I said my peace, and it does feel a bit lighter. I'll hold for a while longer then.";
										break;
									}
									text += "[rf:happy][ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(20, 30));
								}
								else
								{
									bool flag59 = CompanionBanter.logical;
									if (flag59)
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "I am still displeased, but your words ring true. You have my continued support.";
											break;
										case 2:
											text = "You certainly know your way around words. Very well, you've convinced me to stay a while longer.";
											break;
										case 3:
											text = "Fine words and well spoken. Very well, I will remain at your disposal, for now.";
											break;
										default:
											text = "Hm, a fine statement. Doesn't resolve the issue, but it does buy you time. I hope you use it well.";
											break;
										}
										text += "[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(10, 15));
									}
									else
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "Very well, I will remain at your side a while longer.";
											break;
										case 2:
											text = "I'm still unhappy, but I will try to be more patient.";
											break;
										case 3:
											text = "Perhaps I am being hasty, I will reconsider, for the time being.";
											break;
										default:
											text = "Well... Very well, I will stay, for a bit longer.";
											break;
										}
										text += "[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(12, 20));
									}
								}
							}
						}
					}
				}
				else
				{
					Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 4.5f;
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -80);
					Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 100f);
					bool flag60 = CompanionBanter.honorable;
					if (flag60)
					{
						text = "I see... Well, I suppose good military service is preferable to the life of a dung slinger. But, it wasn't our right to force that upon them.[ib:confident2][rb:unsure]";
						Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-2, 1));
						Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
					}
					else
					{
						bool flag61 = CompanionBanter.rogue;
						if (flag61)
						{
							text = "That might be true, but that just means we have more peasants to feed. Besides being expendable troops, I don't quite see their value.[ib:confident2][rb:unsure]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-3, 0));
						}
						else
						{
							bool flag62 = CompanionBanter.logical;
							if (flag62)
							{
								text = "Excellent, so now we have more mouths to feed, with little to no added value. Add to that the negative reputation we've earned, and we've made a ridiculously idiotic deal.[ib:confident2][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
							}
							else
							{
								text = "I understand your perspective my " + CompanionBanter.playerTitle + ", you take care of your own. But that doesn't change the fact that we ripped them from their homes.[if:idle_angry][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-7, -3));
							}
						}
					}
					Support.LogMessage("The new recruits share glances of hatred with their new comrades.");
				}
				MBTextManager.SetTextVariable("HLC_RESULT", text, false);
			}
		}

		public static void Choice_2_Effect()
		{
			string text = "Maybe my " + CompanionBanter.playerTitle + ".";
			bool flag = CompanionBanter.subBanterID > 0;
			if (flag)
			{
				int num = CompanionBanter.subBanterID;
				int num2 = num;
				if (num2 <= 160)
				{
					if (num2 <= 120)
					{
						if (num2 != 100)
						{
							if (num2 != 110)
							{
								if (num2 == 120)
								{
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 60);
									Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20f);
									Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 100f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 20f);
									bool flag2 = CompanionBanter.honorable;
									if (flag2)
									{
										text = "Indeed my " + CompanionBanter.playerTitle + ", pitting our strength against their weak points. No need for clashing shield walls when the enemy starts breaking from every direction.[rf:happy][ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
									}
									else
									{
										bool flag3 = CompanionBanter.rogue;
										if (flag3)
										{
											text = "Ofcourse! Find the chink in the armor, press the knife and twist.[rf:happy][rb:very_positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 5));
										}
										else
										{
											bool flag4 = CompanionBanter.logical;
											if (flag4)
											{
												text = "Ofcouse my " + CompanionBanter.playerTitle + ", we do not need brute strength, we merely need to take the right set of actions, and any enemy force my be brought low.[ib:confident2][rb:positive]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 6));
											}
											else
											{
												text = "No military force is perfect, but I believe you my " + CompanionBanter.playerTitle + " can find the weakness in any force, and from there you will find us victory.[rf:happy][ib:confident2][rb: very_positive]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 4));
											}
										}
									}
									Support.LogMessage("The troops share a silent nod among themselves, acknowledging the superior tactics of their commander.");
								}
							}
							else
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 5f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 20);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 20);
								Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 20f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Roguery, 5f);
								bool flag5 = CompanionBanter.honorable;
								if (flag5)
								{
									text = "A victory is meaningless my " + CompanionBanter.playerTitle + " if it is built upon the corpses of your own men.[if:idle_angry][ib:confident2][rb:very_negative]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-9, -4));
								}
								else
								{
									bool flag6 = CompanionBanter.rogue;
									if (flag6)
									{
										text = "Seeking victory is all fine and good, but if you keep throwing your men at the enemy there will soon be none left to fight for you.[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, -2));
									}
									else
									{
										bool flag7 = CompanionBanter.logical;
										if (flag7)
										{
											text = "I understand. Necessity dictates sacrifice. I can only hope this will not be a constant occurrence, otherwise we will quick run dry on troops.[if:idle_pleased][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-3, 0));
										}
										else
										{
											text = "I'm glad we won, I truly am, but it is difficult to rejoice while surrounded by the blood of our comrades.[if:idle_pleased][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, -1));
										}
									}
								}
								Support.LogMessage("The men shudder, as they hear their lives are expendable in the name of victory.");
							}
						}
						else
						{
							bool flag8 = Hero.MainHero.Gold >= CompanionBanter.option2Cost;
							if (flag8)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 2.5f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers / 3);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 5);
								GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, CompanionBanter.option2Cost, false);
								Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 10f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 40f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 10f);
								bool flag9 = CompanionBanter.honorable;
								if (flag9)
								{
									text = "The men will be please my " + CompanionBanter.playerTitle + ", I'll arrange it.[rf:happy][ib:confident2][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
								}
								else
								{
									bool flag10 = CompanionBanter.rogue;
									if (flag10)
									{
										text = "Looks like the boys are gonna be happy tonight, and drunk. No reason for me not to join.[rf:happy][ib:confident2][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(2, 3));
									}
									else
									{
										bool flag11 = CompanionBanter.logical;
										if (flag11)
										{
											text = "I'm not particularly fond of our entire force getting drunk my " + CompanionBanter.playerTitle + ", but I suppose it will help with morale. I'll see to it.[if:idle_pleased][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
										}
										else
										{
											text = "I'll be drinking to your health tonight then my " + CompanionBanter.playerTitle + ". I'll take care of getting the mead.[rf:happy][ib:confident2][rb:very_positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
										}
									}
								}
								Support.LogMessage("The soldiers drink till they drop, smiling and laughing as they fall.");
							}
							else
							{
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers / 6);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -5);
								bool flag12 = CompanionBanter.honorable;
								if (flag12)
								{
									text = "That's a shame, the men could use a drink and laugh. I suppose we can always do this another day.[if:idle_pleased][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
								else
								{
									bool flag13 = CompanionBanter.rogue;
									if (flag13)
									{
										text = "And here I was hoping to drink myself to bed tonight, oh well.[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										bool flag14 = CompanionBanter.logical;
										if (flag14)
										{
											text = "Likely a wise choice my " + CompanionBanter.playerTitle + ", no need for our force to be marching drunk into an ambush.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
										else
										{
											text = "Well I suppose we'll be sober a bit longer, the upside is the drinks later will taste even sweeter.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
										}
									}
								}
							}
						}
					}
					else if (num2 <= 140)
					{
						if (num2 != 130)
						{
							if (num2 == 140)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 2f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 2);
								Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 40f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 10f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 5f);
								bool flag15 = CompanionBanter.honorable;
								if (flag15)
								{
									text = "We are professional soldiers in essence my " + CompanionBanter.playerTitle + ", it is only fitting for us to be seen this way.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									bool flag16 = CompanionBanter.rogue;
									if (flag16)
									{
										text = "With might and steel, with boots and heels, we'll forge our path ahead. Sounds like a slogan after my own heart.[rf:happy][ib:confident2][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 6));
									}
									else
									{
										bool flag17 = CompanionBanter.logical;
										if (flag17)
										{
											text = "I support this notion, we should maintain a professional presence. It will open many doors for us and put our actions above question.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
										}
										else
										{
											text = "We are your soldiers, the lot of us your " + CompanionBanter.playerTitle + "ship. If people see us as professional soldiers, then they have the right idea.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
										}
									}
								}
								Support.LogMessage("Your troops stand at attention awaiting commands, as true professionals.");
							}
						}
						else
						{
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 40);
							Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20f);
							Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 5f);
							bool flag18 = CompanionBanter.honorable;
							if (flag18)
							{
								text = "Well then, I look forward to seeing your plans unfold.[ib:confident2][rb:positive]";
							}
							else
							{
								bool flag19 = CompanionBanter.rogue;
								if (flag19)
								{
									text = "I can't imagine what could be a better use than this, but you're the " + (Hero.MainHero.IsFemale ? "woman" : "man") + " with the plan, not me.[ib:confident2][rb:unsure]";
								}
								else
								{
									bool flag20 = CompanionBanter.logical;
									if (flag20)
									{
										text = "Forgive me then my " + CompanionBanter.playerTitle + ", I was unaware you already had plans in motion.[ib:confident2][rb:positive]";
									}
									else
									{
										text = "This should be interesting then, I look forward to see how your plans play out.[ib:confident2][rb:positive]";
									}
								}
							}
							Support.LogMessage("Rumors spread about what your plans might be, but none know for certain.");
						}
					}
					else if (num2 != 150)
					{
						if (num2 == 160)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 2f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 20);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 30);
							Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20f);
							Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 30f);
							Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 10f);
							bool flag21 = CompanionBanter.honorable;
							if (flag21)
							{
								text = "Well said my " + CompanionBanter.playerTitle + ", I am glad you are thinking not only of victory, but of the safety of the men as well.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
							}
							else
							{
								bool flag22 = CompanionBanter.rogue;
								if (flag22)
								{
									text = "Good, good. Using your head as always I see. As long as you see the risk of the enemy, there is nothing to fear, now is there.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									bool flag23 = CompanionBanter.logical;
									if (flag23)
									{
										text = "Well spoken my " + CompanionBanter.playerTitle + ". Defeating the enemy while preserving our own troops, as every commander should strive to do.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										text = "Glad to know you're measuring the enemy and keeping an eye out for us my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
								}
							}
							Support.LogMessage("Your forces trust in your tactics and command completely.");
						}
					}
					else
					{
						Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 0.5f;
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, CompanionBanter.option1Cost / 50);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 30);
						Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 40f);
						Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 5f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Roguery, 10f);
						GainRenownAction.Apply(Hero.MainHero, (float)((int)MathF.Clamp((float)(Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 4), 10f, 55f)), false);
						GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, CompanionBanter.option1Cost, false);
						CompanionBanter.ChangeSettlementRelations(1, 1000.0);
						bool flag24 = CompanionBanter.honorable;
						if (flag24)
						{
							text = "They will happily accept and spread our name my " + CompanionBanter.playerTitle + ". While I don't like the idea of bribing the people, it does play to their benefit.[ib:confident2][rb:positive]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
						}
						else
						{
							bool flag25 = CompanionBanter.rogue;
							if (flag25)
							{
								text = "Well played my " + CompanionBanter.playerTitle + ", those poor sods get to fill their bellies in exchange for singing our songs, a fair trade if I've ever seen one.[rf:happy][rb:positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
							}
							else
							{
								bool flag26 = CompanionBanter.logical;
								if (flag26)
								{
									text = "A rational approach. Give the people the silver their need in exchange for providing the service of marketing, a fair trade.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									text = "The poor wretches get the help they need and we earn yet more reputation, not a bad trade my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 4));
								}
							}
						}
						Support.LogMessage("Word spreads from lip to ear of your great honor and deeds.");
					}
				}
				else if (num2 <= 260)
				{
					if (num2 <= 200)
					{
						if (num2 != 170)
						{
							if (num2 == 200)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1.75f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 50);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -30);
								GainRenownAction.Apply(Hero.MainHero, (float)((int)MathF.Clamp((float)(Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 20), 2f, 50f)), false);
								Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 10f);
								CompanionBanter.ChangeSettlementRelations(2, 1000.0);
								bool flag27 = CompanionBanter.honorable;
								if (flag27)
								{
									text = "Yes my " + CompanionBanter.playerTitle + ", all will know that there is a price to pay for attacking this region.[rf:happy][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									bool flag28 = CompanionBanter.rogue;
									if (flag28)
									{
										text = "Even more villages to protect? Oh boy, we're gonna be stuck in this area forever.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
									}
									else
									{
										bool flag29 = CompanionBanter.logical;
										if (flag29)
										{
											text = "I see, well this will certainly appease the notables in the region my " + CompanionBanter.playerTitle + ". But I would recommend keeping this as a desire, rather than a promise.[ib:confident2][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
										}
										else
										{
											text = "That's a lot of ground to cover, but we'll do what we can my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										}
									}
								}
							}
						}
						else
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 0.5f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 20);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 30);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -20);
							bool flag30 = CompanionBanter.honorable;
							if (flag30)
							{
								text = "That it is my " + CompanionBanter.playerTitle + ", and we shall make you proud![rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(4, 8));
							}
							else
							{
								bool flag31 = CompanionBanter.rogue;
								if (flag31)
								{
									text = "Considering they have little value or loot, I'd say that's a waste of time... But that's just me.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
								}
								else
								{
									bool flag32 = CompanionBanter.logical;
									if (flag32)
									{
										text = "As honorable as such an action might be, we must remain focused on the bigger picture. We shouldn't waste too much time with the rabble.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										text = "As you say my " + CompanionBanter.playerTitle + ", just point me to the bandits and watch them fall.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
								}
							}
							Support.LogMessage("The troops sharpen their blades and strap their boots for the brigand hunt.");
						}
					}
					else if (num2 != 250)
					{
						if (num2 == 260)
						{
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 20);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -10);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 40);
							GainRenownAction.Apply(Hero.MainHero, 10f, false);
							Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 20f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 5f);
							CompanionBanter.RaiseSettlementProsperity(20);
							bool flag33 = CompanionBanter.honorable;
							if (flag33)
							{
								text = "Ha! We'll burn every last bit of filth in this place.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
							}
							else
							{
								bool flag34 = CompanionBanter.rogue;
								if (flag34)
								{
									text = "Damaging the camp beyond repair? Sounds like fun, especially since fire is involved.[rf:happy][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									bool flag35 = CompanionBanter.logical;
									if (flag35)
									{
										text = "An excellent move. Devastating this camp will weaken the regional brigands for longer, though they will inevitably return.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
									else
									{
										text = "I'll get the torches my " + CompanionBanter.playerTitle + ", let's see how these buggers like having their own homes burned down.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
								}
							}
							Support.LogMessage("The nearby settlements shall enjoy peace and prosperity for longer even.");
						}
					}
					else
					{
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 10);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 30);
						Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 30f);
						Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 30f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 5f);
						bool flag36 = CompanionBanter.honorable;
						if (flag36)
						{
							text = "Looking cautiously at new possible recruits my " + CompanionBanter.playerTitle + "? Good, we should only take on those whom are worthy.[rf:happy][ib:confident2][rb:very_positive]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
						}
						else
						{
							bool flag37 = CompanionBanter.rogue;
							if (flag37)
							{
								text = "Weighing value vs cost I see. Makes sense to me. Cherry pick while there's time, and if times grow dark we take whatever we can.[ib:confident2][rb:positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
							}
							else
							{
								bool flag38 = CompanionBanter.logical;
								if (flag38)
								{
									text = "I support this logic, we ensure our resources are well spent by being selective. This ensures both efficiency and that our reputation as highly skilled warriors remains intact.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									text = "So we're picking the best of the best? Probably a good thing, at least we don't have to worry about unskilled soldiers.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
							}
						}
						Support.LogMessage("Your soldiers are excited to see what type of recruits the future will bring.");
					}
				}
				else if (num2 <= 410)
				{
					if (num2 != 400)
					{
						if (num2 == 410)
						{
							Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150f);
							bool flag39 = CompanionBanter.honorable;
							if (flag39)
							{
								text = "Protecting our own and ensuring we have what we need is noble, but it doesn't justify this. I ask you to find a better a supplying method, my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-4, -1));
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
							}
							else
							{
								bool flag40 = CompanionBanter.rogue;
								if (flag40)
								{
									text = "In that case, I thank you for my full belly this evening. Just keep in mind these supplies have a cost in reputation.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								}
								else
								{
									bool flag41 = CompanionBanter.logical;
									if (flag41)
									{
										text = "A rational perspective, though bartering would have been a far more favorable approach in this situation.[ib:confident2][rb:very_negative]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
										Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
									}
									else
									{
										text = "I understand your concern for our troops and the desire to ensure they are supplied, but surely we can find better ways than this in the future.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, -1));
										Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
									}
								}
							}
							Support.LogMessage("This deed will be remembered for quite some time.");
						}
					}
					else
					{
						Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 0.5f;
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -5);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -100);
						Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150f);
						CompanionBanter.ChangeSettlementRelations(-5, 1000.0);
						bool flag42 = CompanionBanter.honorable;
						if (flag42)
						{
							text = "I understand my " + CompanionBanter.playerTitle + ". I cannot simply ignore the severity of what we did, but I understand the weights of command. I only hope we need not do this again.[ib:confident2][rb:unsure]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, 1));
							Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
						}
						else
						{
							bool flag43 = CompanionBanter.rogue;
							if (flag43)
							{
								text = "Putting the men first is fine and all, but that doesn't mean we should act moronicly. To save our men we must be seen as brigands, that's like shooting someone in foot with a bolt to make them rest in bed.[if:idle_angry][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-8, -3));
							}
							else
							{
								bool flag44 = CompanionBanter.logical;
								if (flag44)
								{
									text = "I see. I do not see much wisdom in that logic, then again, this is not my command, there may be some pieces I'm missing. I shall adhere to your judgement.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
								else
								{
									text = "So to protect our own we must harm others? That is not the right way my " + CompanionBanter.playerTitle + ", it simply isn't. What happens if next time we must sacrifice our own?[ib:confident2][rb:very_negative]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-6, -2));
								}
							}
						}
						Support.LogMessage("The people who live here will remember you and your deed on this day.");
					}
				}
				else if (num2 != 420)
				{
					if (num2 == 900)
					{
						bool flag45 = Hero.MainHero.Gold >= CompanionBanter.option2Cost;
						if (flag45)
						{
							bool flag46 = Support.Random(1, CompanionBanter.playerCharm) <= 10 || CompanionBanter.honorable;
							if (flag46)
							{
								switch (Support.Random(1, 4))
								{
								case 1:
									text = "This, is nothing short of an insult. Good day!";
									break;
								case 2:
									text = "You cannot buy me. You know where to shove your coin.";
									break;
								case 3:
									text = "Your coin means nothing to me. Off with you!";
									break;
								default:
									text = "You and your coin can sod off.";
									break;
								}
								text += "[if:idle_angry][rb:very_negative]";
								Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 100f);
								Support.RemoveCompanion(CompanionBanter.hero, true);
							}
							else
							{
								bool flag47 = CompanionBanter.rogue;
								if (flag47)
								{
									switch (Support.Random(1, 4))
									{
									case 1:
										text = "Well now, that is an offer I cannot refuse. I suppose I can stay a while longer.";
										break;
									case 2:
										text = "So, coin is indeed the answer to all problems. How can I say no.";
										break;
									case 3:
										text = "A bit more time in exchange for a pouch of coin? Sounds like a fair trade.";
										break;
									default:
										text = "While I'm still unhappy, a pouch of silver will definitely make me feel better.";
										break;
									}
									text += "[if:idle_pleased][rb:unsure]";
									Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 200f);
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(20, 30));
									GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, CompanionBanter.hero, CompanionBanter.option2Cost, false);
								}
								else
								{
									bool flag48 = CompanionBanter.logical;
									if (flag48)
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "Your offer is not unreasonable. I will remain at your side, for now.";
											break;
										case 2:
											text = "Financial compensation for my dissatisfaction? A reasonable trade, though only a temporary solution.";
											break;
										case 3:
											text = "Hmmm. I will not decline your coin, but know that I remain distraught of the state of the company.";
											break;
										default:
											text = "Your offer is most reasonable. I humbly accept. You have my continued support, for the moment.";
											break;
										}
										text += "[if:idle_pleased][rb:unsure]";
										Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 200f);
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(10, 25));
										Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 10);
										GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, CompanionBanter.hero, CompanionBanter.option2Cost, false);
									}
									else
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "While I don't like the idea of being paid to stay, I will not turn down your gold.";
											break;
										case 2:
											text = "That hardly seems to be the solution, but I will try to be more patient then.";
											break;
										case 3:
											text = "A pouch of coin for loyalty. I must be a true mercenary now. Very well, you have me, for the time being.";
											break;
										default:
											text = "Your coin is not needed, but welcomed. If you want me to stay then I will, for a bit of time.";
											break;
										}
										text += "[if:idle_pleased][rb:unsure]";
										Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 200f);
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(15, 25));
										Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 10);
										GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, CompanionBanter.hero, CompanionBanter.option2Cost, false);
									}
								}
							}
						}
						else
						{
							bool flag49 = Support.Random(1, CompanionBanter.playerCharm) >= 50 || CompanionBanter.honorable || CompanionBanter.rogue || CompanionBanter.logical;
							if (flag49)
							{
								switch (Support.Random(1, 4))
								{
								case 1:
									text = "There is no need. If you care so much then I will remain, just for a while longer.";
									break;
								case 2:
									text = "That is not required. You have shown me you genuinely care, hence I will remain steadfast.";
									break;
								case 3:
									text = "Coin is not required. If you truly want me to remain, then I will.";
									break;
								default:
									text = "All the coin you have? Haha, I suppose you truly want me to stay. So I shall, for now. No coin required.";
									break;
								}
								text += "[rf:happy][ib:confident2][rb:very_positive]";
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 10);
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(10, 30));
							}
							else
							{
								bool flag50 = CompanionBanter.rogue;
								if (flag50)
								{
									switch (Support.Random(1, 4))
									{
									case 1:
										text = "Too little, too late I'm afraid.";
										break;
									case 2:
										text = "You insult me with that sum. Good day to ya.";
										break;
									case 3:
										text = "That pouch is a bit light your " + CompanionBanter.playerTitle + "ship, it won't do I'm afraid. Best of luck to ya.";
										break;
									default:
										text = "You seem a bit light there boss, that won't cut it I'm afraid. Good luck out there.";
										break;
									}
									text += "[if:idle_angry][rb:very_negative]";
									Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
									Support.RemoveCompanion(CompanionBanter.hero, true);
								}
								else
								{
									bool flag51 = CompanionBanter.logical;
									if (flag51)
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "Pardon me friend, but that won't do. I'll be taking my leave now.";
											break;
										case 2:
											text = "That sum is insufficient, unless you're willing to present more I bid you a good day.";
											break;
										case 3:
											text = "Your coin is too weak to speak for you it seems. Farewell your " + CompanionBanter.playerTitle + "ship.";
											break;
										default:
											text = "That sum is pitiful, you may keep it. As for myself, I shall take my leave";
											break;
										}
										text += "[if:idle_angry][rb:very_negative]";
										Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
										Support.RemoveCompanion(CompanionBanter.hero, true);
									}
									else
									{
										switch (Support.Random(1, 4))
										{
										case 1:
											text = "I do not like the idea of being bought, but your desire to keep me is warming. I will remain a bit longer then.";
											break;
										case 2:
											text = "I won't say no to this pouch. Though I remain only because you are willing to sacrifice your coin for it. You have my support.";
											break;
										case 3:
											text = "Your action speaks volumes. Sacrificing everything to keep me? Then I will remain, though I will also be keeping the silver.";
											break;
										default:
											text = "Emptying your pockets for me? I'm flattered, though I fear I cannot refuse, I do need the coin. But now you have my support once more.";
											break;
										}
										text += "[if:idle_pleased][rb:unsure]";
										Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
										Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
										Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 10);
										GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, CompanionBanter.hero, Hero.MainHero.Gold, false);
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(15, 35));
									}
								}
							}
						}
					}
				}
				else
				{
					Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 4f;
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -5);
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -80);
					Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150f);
					CompanionBanter.ChangeSettlementRelations(-5, 1000.0);
					bool flag52 = CompanionBanter.honorable;
					if (flag52)
					{
						text = "Protecting our own is good and all, but not like this. These men are nothing more than sacrificial lambs, I'd be surprised if any survive for long. I cannot condone such an action.[ib:confident2][rb:unsure]";
						Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-6, -3));
					}
					else
					{
						bool flag53 = CompanionBanter.rogue;
						if (flag53)
						{
							text = "So you're assembling meat shields and inflating our force. Not a bad plan, I would have probably done the same if I were you. Thankfully I wasn't though.[ib:confident2][rb:unsure]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
							Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
						}
						else
						{
							bool flag54 = CompanionBanter.logical;
							if (flag54)
							{
								text = "This still feels unnecessary and foolish. But, you have the full perspective, I do not. I trust that you know what you are doing.[ib:confident2][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
							}
							else
							{
								text = "These people did not choose to fight for you my " + CompanionBanter.playerTitle + ", bringing them into our ranks might save our lives but it has definitely crippled morale, and our reputation.[ib:confident2][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-4, -1));
							}
						}
					}
					Support.LogMessage("The eyes of the new recruits are filled with rage and hatred, for now.");
				}
				MBTextManager.SetTextVariable("HLC_RESULT", text, false);
			}
		}

		public static void Choice_3_Effect()
		{
			string text = "No my " + CompanionBanter.playerTitle + ".";
			bool flag = CompanionBanter.subBanterID > 0;
			if (flag)
			{
				int num = CompanionBanter.subBanterID;
				int num2 = num;
				if (num2 <= 160)
				{
					if (num2 <= 120)
					{
						if (num2 != 100)
						{
							if (num2 != 110)
							{
								if (num2 == 120)
								{
									Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1f;
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -50);
									Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 50);
									GainRenownAction.Apply(Hero.MainHero, (float)((int)MathF.Clamp((float)Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 4f, 4f, 25f)), false);
									Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 10f);
									Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 70f);
									CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 15f);
									bool flag2 = CompanionBanter.honorable;
									if (flag2)
									{
										text = "Then let us trample the insects my " + CompanionBanter.playerTitle + ", until there is nothing left but dust.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										bool flag3 = CompanionBanter.rogue;
										if (flag3)
										{
											text = "Brutal your " + CompanionBanter.playerTitle + "ship, though it will save lives in the long run.[rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
										}
										else
										{
											bool flag4 = CompanionBanter.logical;
											if (flag4)
											{
												text = "Very efficient, break the  enemy's will and send them fleeing. Once the remaining force is brought down the deserters will be mere target practice.[rf:happy][ib:confident2][rb: very_positive]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(5, 8));
											}
											else
											{
												text = "Then let us break the enemy, they shall flee before your might my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:positive]";
												Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
											}
										}
									}
									Support.LogMessage("The enemy survivors often speak of our might in terror and awe, further spreading our legend.");
								}
							}
							else
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 15f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -15);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -Hero.MainHero.PartyBelongedTo.Party.NumberOfWoundedRegularMembers * 3);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -50);
								Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
								bool flag5 = CompanionBanter.honorable;
								if (flag5)
								{
									text = "What is this madness you speak? These are you loyal soldiers, yet you would use them as shields of flesh? We have nothing more to speak of.[if:idle_angry][ib:confident][rb:very_negative]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-49, -24));
								}
								else
								{
									bool flag6 = CompanionBanter.rogue;
									if (flag6)
									{
										text = "I can understand the idea of expendable soldiers, though I'd keep that notion to yourself. I doubt many would appreciate being your sacrificial lambs.[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-6, -4));
									}
									else
									{
										bool flag7 = CompanionBanter.logical;
										if (flag7)
										{
											text = "Crudely spoken like a murderous lord. While what you say may be true, the men certainly do not need to be reminded of it, especially after today.[if:idle_pleased][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-6, -2));
										}
										else
										{
											text = "So we are all expendable to you? Forgive me, but I cannot stand to be in your presence at this moment.[if:idle_angry][ib:confident][rb:very_negative]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-45, -21));
										}
									}
								}
								Support.LogMessage("Discontent spreads among the troops as your words are retold.");
							}
						}
						else
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 0.5f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -Hero.MainHero.PartyBelongedTo.Party.NumberOfRegularMembers);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -10);
							Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 15f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 5f);
							bool flag8 = CompanionBanter.honorable;
							if (flag8)
							{
								text = "Ofcourse, the men should be satisfied with what they have.[if:idle_pleased][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
							}
							else
							{
								bool flag9 = CompanionBanter.rogue;
								if (flag9)
								{
									text = "Not saying that you don't pay us well, but an occasional bonus does show appreciation. Regardless, it's your choice.[if:idle_pleased][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-4, -1));
								}
								else
								{
									bool flag10 = CompanionBanter.logical;
									if (flag10)
									{
										text = "You are quite right your " + CompanionBanter.playerTitle + "ship, the soldiers should be pleased with what they have, no reason to waste our coin on fancifulness.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										text = "Well, you know best my lord. Just, remember to reward your men every once in a while. It does them a great deal of good to know their actions are appreciated.[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
									}
								}
							}
							Support.LogMessage("Rumors spread among the troops about you hoarding a mountain of gold.");
						}
					}
					else if (num2 <= 140)
					{
						if (num2 != 130)
						{
							if (num2 == 140)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
								Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 100f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50f);
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 100f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 20f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 10f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 20f);
								CompanionBanter.RaiseSettlementProsperity(20);
								bool flag11 = CompanionBanter.honorable;
								if (flag11)
								{
									text = "Then order we shall bring, with our might if we must my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
								}
								else
								{
									bool flag12 = CompanionBanter.rogue;
									if (flag12)
									{
										text = "With might and steel, with boots and heels, we'll forge our path ahead. Sounds like a slogan after my own heart.[rf:happy][ib:confident2][rb:very_positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 6));
									}
									else
									{
										bool flag13 = CompanionBanter.logical;
										if (flag13)
										{
											text = "A force of order? I admit that I do like the sound of that, both terrifying and respectable.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(2, 5));
										}
										else
										{
											text = "So we are to be the guardians and wardens of the people? I suppose if anyone must hold the mantle, then it might as well be us.[ib:confident2][rb:positive]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
										}
									}
								}
								Support.LogMessage("Your troops brace their shields and salute.");
							}
						}
						else
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 0.75f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 5);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers / 10);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -50);
							bool flag14 = CompanionBanter.honorable;
							if (flag14)
							{
								text = "Truer words were never spoken. Damned be the tellers, we earn our own renown.[rf:happy][ib:confident2][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(5, 8));
							}
							else
							{
								bool flag15 = CompanionBanter.rogue;
								if (flag15)
								{
									text = "Well, it would certainly be nice to get quick and easy renown, no risk to life required, but as you say.[if:idle_pleased][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
								}
								else
								{
									bool flag16 = CompanionBanter.logical;
									if (flag16)
									{
										text = "You are not wrong my " + CompanionBanter.playerTitle + ", an earned victory rings truer than any words. Though spreading the tale is also a useful tool to empower our victories.[if:idle_pleased][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-2, -1));
									}
									else
									{
										text = "As you say my " + CompanionBanter.playerTitle + ", bring us the enemy and shall break them and earn our renown.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
								}
							}
							Support.LogMessage("Your forces prepare for many more battles to come.");
						}
					}
					else if (num2 != 150)
					{
						if (num2 == 160)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 10f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 20);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -40);
							Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 60f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 15f);
							bool flag17 = CompanionBanter.honorable;
							if (flag17)
							{
								text = "A noble objective my " + CompanionBanter.playerTitle + ", victory means little if we throw the lives of our men away to achieve it.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
							}
							else
							{
								bool flag18 = CompanionBanter.rogue;
								if (flag18)
								{
									text = "A nice sentiment, just don't get over protective. One has to take risks to make progress.[ib:confident2][rb:positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
								else
								{
									bool flag19 = CompanionBanter.logical;
									if (flag19)
									{
										text = "Certainly my " + CompanionBanter.playerTitle + ", but as you are aware victory often demands sacrifices. A good commander should seek to avoid such things, but use them well if needed.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										text = "Which is why I value this company my " + CompanionBanter.playerTitle + ". To you we are not mere soldiers to be used and discarded. If only this was the same with other commanders.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(2, 3));
									}
								}
							}
							Support.LogMessage("The troops salute their leader, " + (Hero.MainHero.IsFemale ? "she" : "her") + " who preserves their lives.");
						}
					}
					else
					{
						Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 0.25f;
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -CompanionBanter.option1Cost / 20);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 10);
						Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 10f);
						Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Leadership, 5f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 15f);
						bool flag20 = CompanionBanter.honorable;
						if (flag20)
						{
							text = "Very well my " + CompanionBanter.playerTitle + ", we will keep our coin in store, I'm certain you have a good plan for it.[ib:confident2][rb:positive]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
						}
						else
						{
							bool flag21 = CompanionBanter.rogue;
							if (flag21)
							{
								text = "That just means we have to keep fighting and bleeding to earn the same reputation that we could have gotten with denars... Oh well, more coin for us.[ib:confident2][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
							}
							else
							{
								bool flag22 = CompanionBanter.logical;
								if (flag22)
								{
									text = "A wasted opportunity from my perspective, but I'm certain you have a plan of your own my " + CompanionBanter.playerTitle + ".[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
								}
								else
								{
									text = "We have plenty of coin my " + CompanionBanter.playerTitle + ", surely we could have spared some for the innocents. But, I will hold my breath, the command is yours.[if:idle_pleased][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-3, 0));
								}
							}
						}
						Support.LogMessage("Some of the soldiers are hoping for a raise, though that not likely to occur.");
					}
				}
				else if (num2 <= 260)
				{
					if (num2 <= 200)
					{
						if (num2 != 170)
						{
							if (num2 == 200)
							{
								Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 1.25f;
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -80);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, -20);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -40);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -90);
								Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 30);
								GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, Support.Random(300, 1500), false);
								Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 100f);
								CompanionBanter.hero.AddSkillXp(DefaultSkills.Roguery, 20f);
								CompanionBanter.ChangeSettlementRelations(-5, 1000.0);
								bool flag23 = CompanionBanter.honorable;
								if (flag23)
								{
									text = "I'm not fond of the idea of extorting money from these people. Our defense for them shouldn't cost them an arm and a leg.[if:idle_pleased][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-10, -4));
								}
								else
								{
									bool flag24 = CompanionBanter.rogue;
									if (flag24)
									{
										text = "Now that sounds reasonable. A solid defense in exchange for a monetary reward, perfectly logical.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
									else
									{
										bool flag25 = CompanionBanter.logical;
										if (flag25)
										{
											text = "If that is how you wish to play this. This will naturally erase much of the good will we've earned with these people, as I'm sure you're aware.[ib:confident2][rb:unsure]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
										}
										else
										{
											text = "We're going to force them to pay us? So we fended off the raiders only to raid them ourselves?[if:idle_pleased][rb:very_negative]";
											Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-8, -4));
										}
									}
								}
								Support.LogMessage("The soldiers collect tribute from the villagers, no smiles were shared.");
							}
						}
						else
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 1f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 30);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 20);
							bool flag26 = CompanionBanter.honorable;
							if (flag26)
							{
								text = "Very well my " + CompanionBanter.playerTitle + ", then let us distract ourselves some more.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 2));
							}
							else
							{
								bool flag27 = CompanionBanter.rogue;
								if (flag27)
								{
									text = "Oh, so these are just field exercises. I wish you have told me, I would have stayed in my tent.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
								else
								{
									bool flag28 = CompanionBanter.logical;
									if (flag28)
									{
										text = "No desire to stay idle I see. I commend you, best to maintain a rhythm of motion.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
									}
									else
									{
										text = "Then merely say the word when the target appears my " + CompanionBanter.playerTitle + ", and we shall bring you victory.[ib:confident2][rb:positive]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(1, 2));
									}
								}
							}
							Support.LogMessage("The soldiers are looking forward for what is to come.");
						}
					}
					else if (num2 != 250)
					{
						if (num2 == 260)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale += 2f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -10);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers);
							Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 30f);
							CompanionBanter.hero.AddSkillXp(DefaultSkills.Roguery, 5f);
							bool flag29 = CompanionBanter.honorable;
							if (flag29)
							{
								text = "Never thought I'd enjoy taking a piss with the company of others. Funny how things turn out, haha.[rf:happy][rb:very_positive]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(1, 3));
							}
							else
							{
								bool flag30 = CompanionBanter.rogue;
								if (flag30)
								{
									text = "Ey! I get the first burst, I've been drinking all day and my bladder demands vengeance.[rf:happy][rb:very_positive]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(0, 3));
								}
								else
								{
									bool flag31 = CompanionBanter.logical;
									if (flag31)
									{
										text = "A callous action, though good for morale I suppose. Just don't expect me to participate.[rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
									else
									{
										text = "I'm not exactly comfortable revealing my parts in front of others, but I'll make an exception for this.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
									}
								}
							}
							Support.LogMessage("Your troops laugh and holler.");
						}
					}
					else
					{
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 30);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 60);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -25);
						Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 10f);
						Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20f);
						Hero.MainHero.AddSkillXp(DefaultSkills.Tactics, 40f);
						Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 30f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Steward, 5f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Tactics, 10f);
						CompanionBanter.hero.AddSkillXp(DefaultSkills.Charm, 5f);
						bool flag32 = CompanionBanter.honorable;
						if (flag32)
						{
							text = "Then more glory to us my " + CompanionBanter.playerTitle + "![rf:happy][ib:confident2][rb:very_positive]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(3, 6));
						}
						else
						{
							bool flag33 = CompanionBanter.rogue;
							if (flag33)
							{
								text = "We're keeping ourselves a soft target by staying small. Then again, we're also drawing a smaller target. Give and take I suppose.[if:idle_pleased][rb: unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
							}
							else
							{
								bool flag34 = CompanionBanter.logical;
								if (flag34)
								{
									text = "We must expand if we hope to survive in this world my " + CompanionBanter.playerTitle + ", but obviously you are aware of this.[if:idle_pleased][rb: unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
								else
								{
									text = "Personally I'd feel more comfortable in a larger group my " + CompanionBanter.playerTitle + ". Skilled as we might be, against a much larger force we would be putting our lives in serious jeopardy.[if:idle_pleased][rb: unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(-1, 0));
								}
							}
						}
						Support.LogMessage("Concerns are quickly washed away as travelers regard your force as a squad of heroes.");
					}
				}
				else if (num2 <= 410)
				{
					if (num2 != 400)
					{
						if (num2 == 410)
						{
							Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 2f;
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -90);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -180);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -80);
							Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 20);
							Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 200f);
							bool flag35 = CompanionBanter.honorable;
							if (flag35)
							{
								text = "And in the process we've harmed innocent civilians and likely killed them from starvation. There is no honor or valor in this.[if:idle_angry][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-52, -26));
							}
							else
							{
								bool flag36 = CompanionBanter.rogue;
								if (flag36)
								{
									text = "Smart play, scorched earth. Take what the enemy has and leave a burning pit behind. Makes us hated by those affected, but still, smart.[ib:confident2][rb:unsure]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-1, 5));
									Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
								}
								else
								{
									bool flag37 = CompanionBanter.logical;
									if (flag37)
									{
										text = "I see, a strategic move then. Forgive me for not seeing your reasoning earlier.[ib:confident2][rb:unsure]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Chance(0, 1));
										Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
									}
									else
									{
										text = "So we've devastated those peoples lives in the name of weakening some lord or lady that hardly cares? This is madness.[if:idle_angry][rb:very_negative]";
										Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-30, -15));
									}
								}
							}
							Support.LogMessage("Soldiers fight and squabble over the best of the new supplies.");
						}
					}
					else
					{
						Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 2f;
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -100);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -200);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -50);
						Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, -20);
						Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 200f);
						bool flag38 = CompanionBanter.honorable;
						if (flag38)
						{
							text = "You despicable fool. You've made us all into thugs and bandits, in the name of power.[if:idle_angry][rb:very_negative]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-50, -25));
						}
						else
						{
							bool flag39 = CompanionBanter.rogue;
							if (flag39)
							{
								text = "Well, I'm not against the notion of easy loot. I would recommend picking a soft target however, the risk should never outweigh the reward.[ib:confident2][rb:unsure]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-2, 2));
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50f);
							}
							else
							{
								bool flag40 = CompanionBanter.logical;
								if (flag40)
								{
									text = "Spoken like a true bandit leader, or so will anyone who hears you say that believe. I recommend wisdom and caution, a show of force often draws retaliation.[ib:confident2][rb:very_negative]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-9, -4));
								}
								else
								{
									text = "So we have truly become looters and brigands? When I joined this company I was not expecting this, not even remotely.[ib:confident2][rb:very_negative]";
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-30, -16));
								}
							}
						}
						Support.LogMessage("Pitiful and weak villagers, at the side of your path, curse you and your troops endlessly.");
					}
				}
				else if (num2 != 420)
				{
					if (num2 == 900)
					{
						bool flag41 = CompanionBanter.honorable;
						if (flag41)
						{
							switch (Support.Random(1, 10))
							{
							case 1:
								text = "Serving you has been my greatest dishonor, never again.";
								break;
							case 2:
								text = "Good riddance.";
								break;
							case 3:
								text = "May we never cross paths again.";
								break;
							default:
								text = "I hope you live a long and miserable life, my " + CompanionBanter.playerTitle + ".";
								break;
							}
							text += "[if:idle_angry][rb:very_negative]";
						}
						else
						{
							bool flag42 = CompanionBanter.rogue;
							if (flag42)
							{
								switch (Support.Random(1, 10))
								{
								case 1:
									text = "Right then, see ya around.";
									break;
								case 2:
									text = "Until we meet again.";
									break;
								case 3:
									text = "If we ever meet again, It'll probably be in a tavern.";
									break;
								default:
									text = "Then until our next meeting your " + CompanionBanter.playerTitle + "ship.";
									break;
								}
								text += "[ib:confident2][rb:unsure]";
							}
							else
							{
								bool flag43 = CompanionBanter.logical;
								if (flag43)
								{
									switch (Support.Random(1, 10))
									{
									case 1:
										text = "Good day to you my " + CompanionBanter.playerTitle + ".";
										break;
									case 2:
										text = "Godspeed.";
										break;
									case 3:
										text = "Then I bid you farewell my " + CompanionBanter.playerTitle + ".";
										break;
									default:
										text = "Your " + CompanionBanter.playerTitle + "ship, good luck with your ambitions.";
										break;
									}
									text += "[if:idle_pleased][ib:confident2][rb:unsure]";
								}
								else
								{
									switch (Support.Random(1, 10))
									{
									case 1:
										text = "Then I suppose this is farewell.";
										break;
									case 2:
										text = "I will take my leave then.";
										break;
									case 3:
										text = "Goodbye my " + CompanionBanter.playerTitle + ".";
										break;
									default:
										text = "I hope we meet again, under better circumstances.";
										break;
									}
									text += "[if:idle_pleased][ib:confident2][rb:unsure]";
								}
							}
						}
						Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50f);
						Support.RemoveCompanion(CompanionBanter.hero, true);
					}
				}
				else
				{
					Hero.MainHero.PartyBelongedTo.RecentEventsMorale -= 4f;
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -80);
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, -160);
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, -30);
					Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Calculating, 10);
					Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 200f);
					bool flag44 = CompanionBanter.honorable;
					if (flag44)
					{
						text = "No apology required. You've already cursed us and sullied our reputation. What else is there to say.[if:idle_angry][rb:very_negative]";
						Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-40, -22));
					}
					else
					{
						bool flag45 = CompanionBanter.rogue;
						if (flag45)
						{
							text = "I get the whole notion of maintaining power, but this isn't quite it. If anything it shows we are growing weaker.[ib:confident2][rb:unsure]";
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-3, 0));
						}
						else
						{
							bool flag46 = CompanionBanter.logical;
							if (flag46)
							{
								text = "You are very powerful my " + CompanionBanter.playerTitle + ", that is beyond question. But throwing our weight around in such a manner, and the consequences of it, are not strength in any form.[ib:confident2][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, -2));
							}
							else
							{
								text = "So we are brigands now my " + CompanionBanter.playerTitle + "? Kidnapping, looting, should we ransom these people back when we find the next broker? I pity this company for what it has become.[if:idle_angry][rb:very_negative]";
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-37, -21));
							}
						}
					}
					Support.LogMessage("The new recruits curse and belittle the company under their breaths.");
				}
				MBTextManager.SetTextVariable("HLC_RESULT", text, false);
			}
		}

		public static void Ignore_Effect()
		{
			bool flag = CompanionBanter.subBanterID > 0;
			if (flag)
			{
				int num = CompanionBanter.subBanterID;
				int num2 = num;
				if (num2 <= 400)
				{
					if (num2 == 110)
					{
						bool flag2 = CompanionBanter.honorable;
						if (flag2)
						{
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-8, -4));
						}
						else
						{
							bool flag3 = CompanionBanter.rogue;
							if (flag3)
							{
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, -2));
							}
							else
							{
								bool flag4 = CompanionBanter.logical;
								if (flag4)
								{
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-3, 0));
								}
								else
								{
									Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-7, -5));
								}
							}
						}
						goto IL_188;
					}
					if (num2 != 400)
					{
						goto IL_188;
					}
				}
				else if (num2 != 410 && num2 != 420)
				{
					if (num2 != 900)
					{
						goto IL_188;
					}
					Support.RemoveCompanion(CompanionBanter.hero, true);
					goto IL_188;
				}
				bool flag5 = CompanionBanter.honorable;
				if (flag5)
				{
					Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-4, -2));
				}
				else
				{
					bool flag6 = CompanionBanter.rogue;
					if (flag6)
					{
						Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-5, -1));
					}
					else
					{
						bool flag7 = CompanionBanter.logical;
						if (flag7)
						{
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-8, -5));
						}
						else
						{
							Support.ChangeRelation(Hero.MainHero, CompanionBanter.hero, Support.Random(-3, -1));
						}
					}
				}
				IL_188:;
			}
		}

		public static bool CheckForConfrontation()
		{
			bool flag = false;
			bool confrontation_events_enabled = Support.settings.confrontation_events_enabled;
			if (confrontation_events_enabled)
			{
				int num = 0;
				int num2 = Hero.MainHero.CompanionsInParty.Count<Hero>();
				while (!flag && num < num2)
				{
					Hero hero = Hero.MainHero.CompanionsInParty.ElementAt(num);
					int heroRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, hero);
					bool flag2 = heroRelation <= -100 || (heroRelation <= -60 && hero.GetHeroTraits().Honor < 0);
					if (flag2)
					{
						CompanionBanter.Trigger(hero, 900);
						flag = true;
					}
					num++;
				}
			}
			return flag;
		}

		private static void ChangeSettlementRelations(int value, double range = 1000.0)
		{
			for (int i = 0; i < Settlement.All.Count; i++)
			{
				CompanionBanter.settlement = Settlement.All[i];
				bool flag = CompanionBanter.settlement != null;
				if (flag)
				{
					bool flag2 = (CompanionBanter.settlement.IsVillage || CompanionBanter.settlement.IsTown) && (double)CompanionBanter.settlement.Position2D.DistanceSquared(new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().y)) <= range;
					if (flag2)
					{
						CompanionBanter.settlement.Prosperity = CompanionBanter.settlement.Prosperity + 20f;
						bool flag3 = CompanionBanter.settlement.Notables.Any<Hero>();
						if (flag3)
						{
							for (int j = 0; j < CompanionBanter.settlement.Notables.Count<Hero>(); j++)
							{
								Support.ChangeRelation(Hero.MainHero, CompanionBanter.settlement.Notables.ElementAt(j), Support.Chance(value, value + 1));
							}
						}
					}
				}
			}
		}

		private static void RaiseSettlementProsperity(int value)
		{
			for (int i = 0; i < Settlement.All.Count; i++)
			{
				CompanionBanter.settlement = Settlement.All[i];
				bool flag = CompanionBanter.settlement != null;
				if (flag)
				{
					bool flag2 = (CompanionBanter.settlement.IsVillage || CompanionBanter.settlement.IsTown) && (double)CompanionBanter.settlement.Position2D.DistanceSquared(new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().y)) <= 1000.0;
					if (flag2)
					{
						CompanionBanter.settlement.Prosperity = CompanionBanter.settlement.Prosperity + (float)value;
					}
				}
			}
		}

		private static bool CompanionOpinionStart()
		{
			bool result = false;
			string text = "I have no opinion to share";
			string text2 = "No really, I have nothing";
			string text3 = "I swear";
			bool flag = Hero.OneToOneConversationHero != null;
			if (flag)
			{
				bool flag2 = Hero.OneToOneConversationHero.CharacterObject != null;
				if (flag2)
				{
					bool flag3 = Hero.OneToOneConversationHero.Clan == Clan.PlayerClan && Hero.OneToOneConversationHero.IsPlayerCompanion && Hero.OneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty;
					if (flag3)
					{
						CompanionBanter.LoadVairables(Hero.OneToOneConversationHero);
						int num = 0;
						int num2 = 0;
						int heroRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, CompanionBanter.hero);
						bool flag4 = Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers <= 10;
						int num3;
						if (flag4)
						{
							num3 = 1;
						}
						else
						{
							bool flag5 = Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers > 10 && Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers <= 25;
							if (flag5)
							{
								num3 = 2;
							}
							else
							{
								bool flag6 = Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers > 25 && Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers <= 60;
								if (flag6)
								{
									num3 = 3;
								}
								else
								{
									bool flag7 = Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers > 60 && Hero.MainHero.PartyBelongedTo.Party.NumberOfAllMembers <= 120;
									if (flag7)
									{
										num3 = 4;
									}
									else
									{
										num3 = 5;
									}
								}
							}
						}
						bool flag8 = Hero.MainHero.Gold < 1000;
						int num4;
						if (flag8)
						{
							num4 = 1;
						}
						else
						{
							bool flag9 = Hero.MainHero.Gold >= 1000 && Hero.MainHero.Gold < 3000;
							if (flag9)
							{
								num4 = 2;
							}
							else
							{
								bool flag10 = Hero.MainHero.Gold >= 3000 && Hero.MainHero.Gold < 8000;
								if (flag10)
								{
									num4 = 3;
								}
								else
								{
									bool flag11 = Hero.MainHero.Gold >= 8000 && Hero.MainHero.Gold < 20000;
									if (flag11)
									{
										num4 = 4;
									}
									else
									{
										num4 = 5;
									}
								}
							}
						}
						bool flag12 = Hero.MainHero.PartyBelongedTo.Morale < 25f;
						if (flag12)
						{
							num = 1;
						}
						else
						{
							bool flag13 = Hero.MainHero.PartyBelongedTo.Morale >= 25f && Hero.MainHero.PartyBelongedTo.Morale < 50f;
							if (flag13)
							{
								num = 2;
							}
							else
							{
								bool flag14 = Hero.MainHero.PartyBelongedTo.Morale >= 50f && Hero.MainHero.PartyBelongedTo.Morale < 65f;
								if (flag14)
								{
									num = 3;
								}
								else
								{
									bool flag15 = Hero.MainHero.PartyBelongedTo.Morale >= 65f && Hero.MainHero.PartyBelongedTo.Morale < 80f;
									if (flag15)
									{
										num = 4;
									}
									else
									{
										bool flag16 = Hero.MainHero.PartyBelongedTo.Morale > 80f;
										if (flag16)
										{
											num = 5;
										}
									}
								}
							}
						}
						bool flag17 = heroRelation < -50;
						if (flag17)
						{
							num2 = 1;
						}
						else
						{
							bool flag18 = heroRelation >= -50 && heroRelation < 0;
							if (flag18)
							{
								num2 = 2;
							}
							else
							{
								bool flag19 = heroRelation >= 0 && heroRelation < 30;
								if (flag19)
								{
									num2 = 3;
								}
								else
								{
									bool flag20 = heroRelation >= 30 && heroRelation < 65;
									if (flag20)
									{
										num2 = 4;
									}
									else
									{
										bool flag21 = heroRelation >= 65;
										if (flag21)
										{
											num2 = 5;
										}
									}
								}
							}
						}
						int num5 = num2 + num + num4 + num3;
						bool flag22 = CompanionBanter.honorable;
						if (flag22)
						{
							text = "Well my lord our force is ";
							switch (num3)
							{
							case 2:
								text += "quite small, but we should be able to face non military threats without issues. I would suggest we focus on defensive actions and protecting citizens for the time being. ";
								break;
							case 3:
								text += "decent is scale, we can take on small military targets, though we should maintain caution. We should avoid engagements in deep enemy territories, or we risk being overwhelmed. ";
								break;
							case 4:
								text += "sizable, allowing us to target serious military threats. With larger scale composition is critical for success, so must be weary of that. ";
								break;
							case 5:
								text += "massive, we can face entire armies under the right conditions. That being said, logistics are going to be a quite overwhelming, so we should likely avoid unnecessary deployments. ";
								break;
							default:
								text += "rather tiny, which limits our abilities greatly. This will greatly limit our combat options. ";
								break;
							}
							bool flag23 = num3 < 3 && num4 > 2;
							if (flag23)
							{
								text += "However ";
							}
							else
							{
								bool flag24 = num3 >= 3 && num4 <= 2;
								if (flag24)
								{
									text += "But ";
								}
								else
								{
									text += "As for ";
								}
							}
							text += "our financial state it is ";
							switch (num4)
							{
							case 2:
								text += "acceptable for the time being. Our men will not want for coin but recruitment should be selective.";
								break;
							case 3:
								text += "excellent, thus we shouldn't have any immediate issues with coin. Though we should use our military spending wisely.";
								break;
							case 4:
								text += "outstanding, we have a great deal of excess. This should comfortably afford us new troops and equipment.";
								break;
							case 5:
								text += "staggering, as we effectively have a massive pool of funding. We may want to consider bringing in honorable mercenaries or expanding our forces across the continent.";
								break;
							default:
								text += "too far on the edge, but we should endure. ";
								break;
							}
							text2 = "Our troops are ";
							switch (num)
							{
							case 2:
								text2 += "dissatisfied. While the ungrateful bastards should learn to honor their leader, the fact remains that we face desertion. ";
								break;
							case 3:
								text2 += "adequate. They are appeased with what they have, as they should be. ";
								break;
							case 4:
								text2 += "quite happy under your banner, rightly so. ";
								break;
							case 5:
								text2 += "honored to be led by you. They will march through the gates of hell if you command it. ";
								break;
							default:
								text2 += "unnerved and disheartened. We should seek to correct this before desertion overwhelms us. ";
								break;
							}
							bool flag25 = num < 3 && num2 > 2;
							if (flag25)
							{
								text2 += "In contrast, I am ";
							}
							else
							{
								bool flag26 = num >= 3 && num2 <= 2;
								if (flag26)
								{
									text2 += "But as for myself, I'm ";
								}
								else
								{
									text2 += "Myself however, I am ";
								}
							}
							switch (num2)
							{
							case 2:
								text2 += "a bit dissatisfied with this part. This is simply my opinion obviously.";
								break;
							case 3:
								text2 += "satisfied with the state of our force. Though I see much room for improvements.";
								break;
							case 4:
								text2 = text2 + "very much happy fighting under your command. You have led us true my " + CompanionBanter.playerTitle + ".";
								break;
							case 5:
								text2 += "remain your humble servant my friend, and will continue to stand by you through thick and thin.";
								break;
							default:
								text2 += "finding myself quite displeased with this company, though that is my own opinion.";
								break;
							}
							bool flag27 = num5 <= 8;
							if (flag27)
							{
								text3 = "All in all, we have much to improve still. That is my opinion. Now, did you want to know anything else?";
							}
							else
							{
								bool flag28 = num5 > 8 && num5 <= 11;
								if (flag28)
								{
									text3 = "Overall we're in a decent situation, though we can certainly stand to improve it. So, can I do anything else for you?";
								}
								else
								{
									bool flag29 = num5 > 11 && num5 <= 14;
									if (flag29)
									{
										text3 = "I'd say we're in a good spot at the moment, improvements and expansion are always welcomed however. Well then, what else can I do for you?";
									}
									else
									{
										text3 = "I dare say we're in an excellent condition, and who knows perhaps we can improve further still. That is how I personally see things. Alright then, anything else my " + CompanionBanter.playerTitle + "?";
									}
								}
							}
						}
						bool flag30 = CompanionBanter.kind;
						if (flag30)
						{
							text = "Alright, our party is ";
							switch (num3)
							{
							case 2:
								text += "not too bad scale wise, but we should probably avoid any larger scale combat. On the other hand we are secure and fast enough for travel and trade. ";
								break;
							case 3:
								text += "quite nice in size, we can hold our own in a military engagement if needed. Naturally I would prefer to avoid high risk battles, we should focus on smaller tasks, lower the risk. ";
								break;
							case 4:
								text += "great in size, any military force would be cautious to attack us. We should be able to conduct our business with little hassle and danger. ";
								break;
							case 5:
								text += "grand is scope, why even armies would think twice before attacking us. That does not mean we should jump head-on into battles. We must take the lives our our forces into consideration. ";
								break;
							default:
								text += "very small, we should avoid risks for the time being. I would recommend sticking to small, low risk tasks. ";
								break;
							}
							bool flag31 = num3 < 3 && num4 > 2;
							if (flag31)
							{
								text += "That being said, for ";
							}
							else
							{
								bool flag32 = num3 >= 3 && num4 <= 2;
								if (flag32)
								{
									text += "On the other hand, ";
								}
								else
								{
									text += "In regards to ";
								}
							}
							text += "the finances, we are ";
							switch (num4)
							{
							case 2:
								text += "on the lower end of the coin pouch, but such is life, we will carry on.";
								break;
							case 3:
								text += "comfortable, with plenty of coin to spare.";
								break;
							case 4:
								text += "rather wealthy. We might want to consider sparing a few denars for the less fortunate.";
								break;
							case 5:
								text += "incredibly rich. We can truly help the people, invest and improve lives. I look forward to it in fact.";
								break;
							default:
								text += "quite poor to be honest, but we'll make due. ";
								break;
							}
							text2 = "Your soldiers are ";
							switch (num)
							{
							case 2:
								text2 += "unhappy, though not without cause. Perhaps a gesture of good will or a touch of variety will do them good. ";
								break;
							case 3:
								text2 += "happy, they have what they need and then some. You are truly a good commander. ";
								break;
							case 4:
								text2 = text2 + "glad to be in your service my " + CompanionBanter.playerTitle + ". You are among the best of leaders. ";
								break;
							case 5:
								text2 += "undyingly loyal to you. These men will gladly face death in your name. ";
								break;
							default:
								text2 += "scared and worried. We should try to ease their minds as quick as we can. ";
								break;
							}
							bool flag33 = num < 3 && num2 > 2;
							if (flag33)
							{
								text2 += "On the other hand, I am ";
							}
							else
							{
								bool flag34 = num >= 3 && num2 <= 2;
								if (flag34)
								{
									text2 += "As for me, I'm";
								}
								else
								{
									text2 += "For me, I am ";
								}
							}
							switch (num2)
							{
							case 2:
								text2 += "somewhat worried about the state of our company. Though these are my own personal concerns.";
								break;
							case 3:
								text2 = text2 + "glad to be a part of this force. I hope you bring us even more victories and renown my " + CompanionBanter.playerTitle + ".";
								break;
							case 4:
								text2 = text2 + "as happy as can be under your command my " + CompanionBanter.playerTitle + ". You are a most wise and just leader.";
								break;
							case 5:
								text2 += "your eternal subordinate. Merely command me, and I will obey.";
								break;
							default:
								text2 += "concerned for this party and its current state, but that's just my opinion.";
								break;
							}
							bool flag35 = num5 <= 8;
							if (flag35)
							{
								text3 = "Generally speaking, we aren't in the best of shapes, but surely we'll improve. Anyhow, that is my opinion. Any other orders?";
							}
							else
							{
								bool flag36 = num5 > 8 && num5 <= 11;
								if (flag36)
								{
									text3 = "Well, we're in a decent spot and things are going well for us. They can naturally get even better but patience is a virtue. At any rate, those are my thoughts. Now, what else would you have me do?";
								}
								else
								{
									bool flag37 = num5 > 11 && num5 <= 14;
									if (flag37)
									{
										text3 = "We're in great shape as I see it, there's little left to improve! Anyhow, what else can I do for you?";
									}
									else
									{
										text3 = "We are in an outstanding state my " + CompanionBanter.playerTitle + ", we are lucky to have you as our leader. At least, that's what I think. Anyway, what else will you have me do?";
									}
								}
							}
						}
						bool flag38 = CompanionBanter.rogue;
						if (flag38)
						{
							text = "Well, since you asked. Our band of misfets is ";
							switch (num3)
							{
							case 2:
								text += ", well, small. We don't matter all that much in the grand scheme of things, as a party that is. ";
								break;
							case 3:
								text += "average in size I would say. We can have some minor impact, but nothing too major in our current state. ";
								break;
							case 4:
								text += "rather decent in scale. We should be able to hit some serious target, get ourselves some real coin and glory. ";
								break;
							case 5:
								text += "terrifyingly large. We can take on nearly anything, claim whatever we wish. Actually, that's exactly what we should do. ";
								break;
							default:
								text += "microscopic, we are barely a spec on the map. We can leech off some glory and a few coins here and there, or slip past most threats. ";
								break;
							}
							bool flag39 = num3 < 3 && num4 > 2;
							if (flag39)
							{
								text += "However, ";
							}
							else
							{
								bool flag40 = num3 >= 3 && num4 <= 2;
								if (flag40)
								{
									text += "But, ";
								}
								else
								{
									text += "As for ";
								}
							}
							text += "in regards to coin, our pouches ";
							switch (num4)
							{
							case 2:
								text += "have a few denars in them, but not nearly enough.";
								break;
							case 3:
								text += "sing with ever turn, but we can certainly do better.";
								break;
							case 4:
								text += "are swelling, but there's always room for more.";
								break;
							case 5:
								text += "can't hold all our coin. In fact, I'd be happy to hold your denars for you, if you'd like.";
								break;
							default:
								text += "are far too light, we need to fix that urgently.";
								break;
							}
							text2 = "Our soldiers are ";
							switch (num)
							{
							case 2:
								text2 += "problematic, there are too many rumors running around, too much doubt. We need to whip them back in line. ";
								break;
							case 3:
								text2 += "cheery, like a maid that just lost her cherry. Well, maybe not quite like that, most of those end up in disappointments, haha. ";
								break;
							case 4:
								text2 = text2 + "happy to be in your " + CompanionBanter.playerTitle + "ships service. Good food and plenty of loot make soldiers smile, who knew. ";
								break;
							case 5:
								text2 += "happier than a clam in the ocean. You point and they'll fight, gladly. ";
								break;
							default:
								text2 += "weak and worrisome. We need to get their heads out of their asses or get rid of them. ";
								break;
							}
							bool flag41 = num < 3 && num2 > 2;
							if (flag41)
							{
								text2 += "But as for little old me, I'm ";
							}
							else
							{
								bool flag42 = num >= 3 && num2 <= 2;
								if (flag42)
								{
									text2 += "Me on the other end of ths stick, I'm";
								}
								else
								{
									text2 += "Now for me, I'm ";
								}
							}
							switch (num2)
							{
							case 2:
								text2 += "a tiny bit worried about the company, for undisclosed reasons.";
								break;
							case 3:
								text2 += "quite happy to be here. There's food, loot and women to boot within reach. What's not to like. ";
								break;
							case 4:
								text2 += "a happy camper at your side, I get what I'm owed and then some.";
								break;
							case 5:
								text2 += "excited to be here, ol'buddy. You just lead and I'll follow.";
								break;
							default:
								text2 += "not too happy about where this party is going. But don't let that bother you.";
								break;
							}
							bool flag43 = num5 <= 8;
							if (flag43)
							{
								text3 = "Well, we're not exactly in tip top shape, so I say let's fix that! Now, anything else I can do for you?";
							}
							else
							{
								bool flag44 = num5 > 8 && num5 <= 11;
								if (flag44)
								{
									text3 = "We're not too bad, we've got our heads about and heading up the river. This part is gonna just be just fine. So, I have a mug of mead beckoning me, or did you need anything else?";
								}
								else
								{
									bool flag45 = num5 > 11 && num5 <= 14;
									if (flag45)
									{
										text3 = "This company is in a fairly good spot. We march and the world feels it. But anyway, you needed something else?";
									}
									else
									{
										text3 = "We are ready to conquer the world, more or less. Or so a little birdie told me. Now, you needed something else?";
									}
								}
							}
						}
						bool flag46 = CompanionBanter.logical;
						if (flag46)
						{
							text = "From what I've seen, I can say the scale of our presence is ";
							switch (num3)
							{
							case 2:
								text += "somewhat irrelevant. With such a small force on hand we're hardly suited for any significant action, primarily security patrols which are best left to others. ";
								break;
							case 3:
								text += "adequate. We have sufficient impact to maintain security and engage reasonably limited target. ";
								break;
							case 4:
								text += "sufficient. Our immediate military capacity allows us to engage in most military conflicts directly. ";
								break;
							case 5:
								text += "significant. I suspect we will face little challenge to accomplish our immediate objectives. ";
								break;
							default:
								text += "miniscule. We have very little relevance in the grand scheme of things as a military party. ";
								break;
							}
							bool flag47 = num3 < 3 && num4 > 2;
							if (flag47)
							{
								text += "However, ";
							}
							else
							{
								bool flag48 = num3 >= 3 && num4 <= 2;
								if (flag48)
								{
									text += "In regards to ";
								}
								else
								{
									text += "Regarding the subject of ";
								}
							}
							text += "financially, we are ";
							switch (num4)
							{
							case 2:
								text += "running on low values. An increase in our reserves would be appropriate.";
								break;
							case 3:
								text += "operating at normal capacity. We should strive to improve this state however.";
								break;
							case 4:
								text += "running with overflow. I recommend investing in additional forces or assets to maintain this flow.";
								break;
							case 5:
								text += "at an excess. I highly recommend we immediately invest in expansion to ensure consistent income.";
								break;
							default:
								text += "too fragile. We must establish a new stream of income immediately.";
								break;
							}
							text2 = "The military personnel are ";
							switch (num)
							{
							case 2:
								text2 += "being rather problematic. We should address their concerns if possible to avoid an escalation. ";
								break;
							case 3:
								text2 += "satisfied, as they should be.";
								break;
							case 4:
								text2 += "spirited. Which ensures loyalty and adequate field performance. ";
								break;
							case 5:
								text2 = text2 + "ecstatic. Well done my " + CompanionBanter.playerTitle + ", the soldiers will gladly push our objectives forward. ";
								break;
							default:
								text2 += "troublesome. Weakness and low morale is eroding them, we should address this promptly. ";
								break;
							}
							bool flag49 = num < 3 && num2 > 2;
							if (flag49)
							{
								text2 += "Personally I ";
							}
							else
							{
								bool flag50 = num >= 3 && num2 <= 2;
								if (flag50)
								{
									text2 += "As for myself, i";
								}
								else
								{
									text2 += "Regardinbg my own self, I ";
								}
							}
							switch (num2)
							{
							case 2:
								text2 += "have reservations on some of the decisions made. However, I remain steadfast.";
								break;
							case 3:
								text2 += "am pleased with the current state. We are running at reasonable conditions. ";
								break;
							case 4:
								text2 += "acknowledge the excellent state we are in. I suspect few other military groups are run as smoothly.";
								break;
							case 5:
								text2 += "excited. I see a great future ahead of us.";
								break;
							default:
								text2 += "see a lack of logic and reasoning in this command, but I remain obedient.";
								break;
							}
							bool flag51 = num5 <= 8;
							if (flag51)
							{
								text3 = "Overall, our state is inadequate, we should correct this. Now, do you require any additional information?";
							}
							else
							{
								bool flag52 = num5 > 8 && num5 <= 11;
								if (flag52)
								{
									text3 = "Our overall status is adequate, though fragile. Further development and enhancement is recommended. Now, will you be requiring anything else?";
								}
								else
								{
									bool flag53 = num5 > 11 && num5 <= 14;
									if (flag53)
									{
										text3 = "The company maintains a high state at this time, quite pleasing to see. Forgive me, but I have other matters to attend to, unless you needed anything else?";
									}
									else
									{
										text3 = "We are running at near optimal efficiency, yet I look forward to improving our status further regardless. Other than this, can I do anything for you?";
									}
								}
							}
						}
						MBTextManager.SetTextVariable("HLC_CHOICE1", text, false);
						MBTextManager.SetTextVariable("HLC_CHOICE2", text2, false);
						MBTextManager.SetTextVariable("HLC_CHOICE3", text3, false);
						result = true;
					}
				}
			}
			return result;
		}

		public static Hero hero;

		public static Hero subHero;

		public static CharacterTraits heroTraits;

		public static bool honorable = false;

		public static bool rogue = false;

		public static bool logical = false;

		public static bool kind = false;

		public static CharacterTraits playerTraits;

		public static string playerTitle;

		public static int playerCharm;

		public static int banterID = 0;

		public static int subBanterID = 0;

		public static int option1Cost = 0;

		public static int option2Cost = 0;

		public static int option3Cost = 0;

		public static Settlement settlement;

		public const int tournamentWonID = 80;

		public const int highEnemyCasualtiesVictoryID = 100;

		public const int highCasualtiesVictoryID = 110;

		public const int tacticalVictoryID = 120;

		public const int heroicVictoryID = 130;

		public const int typeOfVictoryID = 140;

		public const int shareVictoryID = 150;

		public const int flawlessVictoryID = 160;

		public const int banditVictoryID = 170;

		public const int villageDefenseVictoryID = 200;

		public const int heroicCompanyVictoryID = 250;

		public const int hideoutVictoryID = 260;

		public const int raidID = 400;

		public const int raidSuppliesID = 410;

		public const int raidRecruitsID = 420;

		public const int ultimatumID = 900;
	}
}
