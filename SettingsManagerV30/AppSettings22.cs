using System.Runtime.Serialization;
using SettingManager;

namespace SettingsManagerV30
{

	public static class AppSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<AppSettingInfo22> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static AppSettingInfo22 Info { get; private set; }
		public static AppSettingData22 Data { get; private set; }

		public static SettingMgrStatus Status => Admin.Status;
		public static bool Exists => Admin.Exists;

		// initalize and create the setting objects
		static AppSettings()
		{
			Admin = new SettingsMgr<AppSettingInfo22>(ResetClass);
			Info = Admin.Info;
			Data = Info.Data;
		}

		// if we need to reset to the "factory" default
		public static void ResetClass()
		{
			Info = Admin.Info;
		}
	}

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "AppSettingData22")]
	public class AppSettingData22
	{
		[DataMember(Order = 1)]
		public int AppI { get; set; } = 0;

		[DataMember(Order = 2)]
		public bool AppB { get; set; } = false;

		[DataMember(Order = 3)]
		public double AppD { get; set; } = 0.0;

		[DataMember(Order = 4)]
		public string AppS { get; set; } = "this is an App";

		[DataMember(Order = 5)]
		public int[] AppIs { get; set; } = new[] {20, 30};

		[DataMember(Order = 20)]
		public int AppI20 { get; set; } = 0;

		[DataMember(Order = 21)]
		public bool AppB21 { get; set; } = false;

		[DataMember(Order = 22)]
		public double AppD22 { get; set; } = 0.0;
	}
	
	[DataContract(Name = "AppSettingInfo22")]
	public class AppSettingInfo22 : AppSettingBase
	{
		[DataMember]
		public AppSettingData22 Data = new AppSettingData22();

		public override string ClassVersion => "2.2";

		public override void Upgrade(SettingBase prior)
		{
			AppSettingInfo21 p = (AppSettingInfo21) prior;

			Header.Notes =
				p.Header.Notes + " :: updated to v" + ClassVersion;

			Data.AppI   = p.Data.AppI;
			Data.AppB   = p.Data.AppB;
			Data.AppD   = p.Data.AppD;
			Data.AppS   = p.Data.AppS;
			Data.AppI20 = p.Data.AppI20;
			Data.AppB21 = p.Data.AppB21;

			for (int i = 0;
				i < (Data.AppIs.Length < p.Data.AppIs.Length ? Data.AppIs.Length : p.Data.AppIs.Length);
				i++)
			{
				Data.AppIs[i] = p.Data.AppIs[i];
			}

		}
	}
}