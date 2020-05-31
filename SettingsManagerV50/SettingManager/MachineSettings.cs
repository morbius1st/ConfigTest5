// Solution:     SettingsManager
// Project:       SettingsManagerV50
// File:             MachineSettings.cs
// Created:      -- ()

using System.Runtime.Serialization;
using SettingsManager;
using SettingsManagerV50.Windows;

namespace SettingsManagerV50
{
// this is the actual data set saved to the user's configuration file
// this is unique for each program
	[DataContract(Name = "MachSettingData50")]
	public class MachSettingData50
	{
		[DataMember(Order = 1)]
		public int MachSettingsValue { get; set; } = 2;
	}

	[DataContract(Name = "MachSettingInfo50")]
	public class MachSettingInfo50 : MachSettingInfoBase
	{
		[DataMember]
		public MachSettingData50 Data = new MachSettingData50();

		public override string DataClassVersion => "5.0c";
		public override string Description => "machine setting file for SettingsManagerV50";
		public override void UpgradeFromPrior(SettingInfoBase prior) { }
	}

	public static class MachSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<MachSettingPath50, MachSettingInfo50> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static MachSettingPath50 Path { get; private set; } = new MachSettingPath50();
		public static MachSettingInfo50 Info { get; private set; }
		public static MachSettingData50 Data { get; private set; }

		// initialize and create the setting objects
		static MachSettings()
		{
			Admin = new SettingsMgr<MachSettingPath50, MachSettingInfo50>(Path, ResetData);
			Info = Admin.Info;

			MainWindow.Instance.MsgLeftLine("");
			MainWindow.Instance.MsgLeftLine("at ctor MachSettings|   status", Admin.Status.ToString());
			MainWindow.Instance.MsgLeftLine("at ctor MachSettings|     path", Path.SettingPath);
			MainWindow.Instance.MsgLeftLine("at ctor MachSettings| filename", Path.FileName);
		}

		public static void ResetData()
		{
			if (Admin == null) return;
			// this makes sure the above static class points
			// to the current data structure
			Info  = Admin.Info;
			Data  = Info.Data;
		}
	}
}