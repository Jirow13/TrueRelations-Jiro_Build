using System;

namespace TrueRelations
{
	public class Settings
	{
		public bool battle_leaders_good_reputation_enabled { get; set; }

		public bool battle_leaders_bad_reputation_enabled { get; set; }

		public bool battle_leaders_ai_reputation_enabled { get; set; }

		public bool player_trait_skill_gain_enabled { get; set; }

		public double player_trait_skill_gain_rate { get; set; }

		public bool battle_companions_good_reputation_enabled { get; set; }

		public bool battle_companions_bad_reputation_enabled { get; set; }

		public bool companions_skill_gain_enabled { get; set; }

		public double companions_skill_gain_rate { get; set; }

		public bool companion_trait_sharing_enabled { get; set; }

		public bool battle_family_good_reputation_enabled { get; set; }

		public bool battle_family_bad_reputation_enabled { get; set; }

		public bool family_global_bonus_enabled { get; set; }

		public bool family_skill_gain_enabled { get; set; }

		public double family_skill_gain_rate { get; set; }

		public bool family_trait_sharing_enabled { get; set; }

		public bool faction_global_bonus_enabled { get; set; }

		public bool random_events_enabled { get; set; }

		public bool confrontation_events_enabled { get; set; }

		public double random_events_chance { get; set; }

		public bool bandit_battle_bonus_enabled { get; set; }

		public int bandit_battle_bandits_for_bonus { get; set; }

		public int bandit_battle_minimum_bonus { get; set; }

		public bool tournament_bonus_enabled { get; set; }

		public Settings()
		{
			this.battle_leaders_good_reputation_enabled = true;
			this.battle_leaders_bad_reputation_enabled = true;
			this.battle_leaders_ai_reputation_enabled = true;
			this.player_trait_skill_gain_enabled = true;
			this.player_trait_skill_gain_rate = 1.0;
			this.battle_companions_good_reputation_enabled = true;
			this.battle_companions_bad_reputation_enabled = true;
			this.companions_skill_gain_enabled = true;
			this.companions_skill_gain_rate = 1.0;
			this.companion_trait_sharing_enabled = false;
			this.battle_family_good_reputation_enabled = true;
			this.battle_family_bad_reputation_enabled = true;
			this.family_global_bonus_enabled = true;
			this.family_skill_gain_enabled = true;
			this.family_trait_sharing_enabled = false;
			this.family_skill_gain_rate = 1.0;
			this.faction_global_bonus_enabled = true;
			this.random_events_enabled = true;
			this.confrontation_events_enabled = true;
			this.random_events_chance = 0.03;
			this.bandit_battle_bonus_enabled = true;
			this.bandit_battle_bandits_for_bonus = 10;
			this.bandit_battle_minimum_bonus = 1;
			this.tournament_bonus_enabled = true;
		}
	}
}
