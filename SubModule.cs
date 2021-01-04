using System;
using System.IO;
using System.Xml.Serialization;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace TrueRelations
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
			new Harmony("HLC.TrueRelations").PatchAll();
			Support.LogMessage("True Relations Loaded");
			try
			{
				using (Stream stream = new FileStream(Path.Combine(BasePath.Name, "Modules", "TrueRelations", "Settings.xml"), FileMode.Open))
				{
					Support.settings = (Settings)new XmlSerializer(typeof(Settings)).Deserialize(stream);
				}
			}
			catch (Exception ex)
			{
				Support.LogMessage("True Relations: Could not read setting, using default values!");
				Support.settings = new Settings();
			}
		}

		public override void OnGameInitializationFinished(Game game)
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(BattleEndEventListener.PostBattleRelationships));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(CompanionBanter.AddressPlayer));
		}
	}
}
