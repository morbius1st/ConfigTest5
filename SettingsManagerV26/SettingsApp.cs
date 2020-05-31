using System.Runtime.Serialization;

using UtilityLibrary;



namespace SettingsManagerV26
{

	public static class SettingsApp
	{
		public static SettingsMgr<AppSettings> ASettings { get; private set; }

		public static AppSettings ASet { get; private set; }

		static SettingsApp()
		{
			ASettings = new SettingsMgr<AppSettings>(ResetClass);
			ASet = ASettings.Settings;
		}

		public static void ResetClass()
		{
			ASet = ASettings.Settings;
		}
	}


	[DataContract(Name = "AppSettings")]
	public class AppSettings : SettingsPathFileAppBase
	{
		public const string APPSETTINGFILEVERSION = "2.1";

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

		public override string FileVersion { get; set; }
	}
}