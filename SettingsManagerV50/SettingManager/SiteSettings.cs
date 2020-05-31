// Solution:     SettingsManager
// Project:       SettingsManagerV50
// File:             SiteSettings.cs
// Created:      -- ()

using System.Runtime.Serialization;
using SettingsManager;
using SettingsManagerV50.Windows;

namespace SettingsManagerV50 {

// this is the actual data set saved to the user's configuration file
// this is unique for each program
	[DataContract(Name = "SiteSettingData50")]
	public class SiteSettingData50
	{
		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 1;
	}

	[DataContract(Name = "SiteSettingInfo50")]
	public class SiteSettingInfo50 : SiteSettingInfoBase
	{
		[DataMember]
		public SiteSettingData50 Data = new SiteSettingData50();

		public override string DataClassVersion => "5.0s";
		public override string Description => "site setting file for SettingsManagerV50";

		public override void UpgradeFromPrior(SettingInfoBase prior) { }
	}

	public static class SiteSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<SiteSettingPath50, SiteSettingInfo50> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static SiteSettingPath50 Path { get; private set; } = new SiteSettingPath50();
		public static SiteSettingInfo50 Info { get; private set; }
		public static SiteSettingData50 Data { get; private set; }

		// initialize and create the setting objects
		static SiteSettings()
		{
			Path.RootPath =
				@"D:\Users\Jeff\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\AppData";

			Admin = new SettingsMgr<SiteSettingPath50, SiteSettingInfo50>(Path, ResetData);
			Info = Admin.Info;
			Data = Info.Data;

			MainWindow.Instance.MsgLeftLine("");
			MainWindow.Instance.MsgLeftLine("at ctor SiteSettings|   status", Admin.Status.ToString());
			MainWindow.Instance.MsgLeftLine("at ctor SiteSettings|     path", Path?.SettingPath ?? "is null");
			MainWindow.Instance.MsgLeftLine("at ctor SiteSettings| filename", Path?.FileName ?? "is null");
			MainWindow.Instance.MsgLeftLine("");
		}

		// if we need to reset to the "factory" default
		public static void ResetData()
		{
			if (Admin == null) return;
			// this makes sure the above static class points
			// to the current data structure
			Info = Admin.Info;
			Data = Info.Data;
		}
	}
}