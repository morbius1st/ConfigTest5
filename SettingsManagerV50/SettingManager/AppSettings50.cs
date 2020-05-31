using System;
using System.Runtime.Serialization;
using SettingsManager;
using SettingsManagerV50.Windows;
using UtilityLibrary;
using WpfProjectTemplate01.Properties;

// projname: SettingsManagerV40
// itemname: AppSettingInfo50
// username: jeffs

namespace SettingsManagerV50
{
	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "AppSettingData50")]
	public class AppSettingData50
	{
		[DataMember(Order = 1)]
		public int AppSettingsValue { get; set; } = 3;
	}


	[DataContract(Name = "AppSettingInfo50")]
	public class AppSettingInfo50 : AppSettingInfoBase
	{
		[DataMember]
		public AppSettingData50 Data = new AppSettingData50();

		public override string DataClassVersion => "5.0a";
		public override string Description => "app setting file for SettingsManagerV50";
		public override void UpgradeFromPrior(SettingInfoBase prior) { }
	}

	public static class AppSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<AppSettingPath50, AppSettingInfo50> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static AppSettingPath50 Path { get; private set; } = new AppSettingPath50();
		public static AppSettingInfo50 Info { get; private set; }
		public static AppSettingData50 Data { get; private set; }

		// initialize and create the setting objects
		static AppSettings()
		{
			Admin = new SettingsMgr<AppSettingPath50, AppSettingInfo50>(Path, ResetData);
			Info = Admin.Info;
			Data = Info.Data;

			MainWindow.Instance.MsgLeftLine("");
			MainWindow.Instance.MsgLeftLine("at ctor AppSettings|   status", Admin.Status.ToString());
			MainWindow.Instance.MsgLeftLine("at ctor AppSettings|     path", Path.SettingPath);
			MainWindow.Instance.MsgLeftLine("at ctor AppSettings| filename", Path.FileName);
			MainWindow.Instance.MsgLeftLine("");
		}

		// if we need to reset to the "factory" default
		public static void ResetData()
		{
			Info = Admin.Info;
			Data = Info.Data;
		}
	}


}