using System.Runtime.Serialization;
using SettingManager;

// projname: SettingsManagerV40
// itemname: SiteSettingInfo40
// username: jeffs

namespace SettingManagerV40
{
/*
#region site settings data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "SiteSettingData40")]
	public class SiteSettingData40
	{
		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 1;
	}

#endregion

#region site settings management info class

	[DataContract(Name = "SiteSettingInfo40")]
	public class SiteSettingInfo40 : SiteSettingBase
	{
		[DataMember]
		public SiteSettingData40 Data = new SiteSettingData40();

		public override string ClassVersion => "4.0s";

		public override void UpgradeFromPrior(SettingBase prior) { }
	}

#endregion


#region site settings management root class

	public static class SiteSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<SiteSettingInfo40> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static SiteSettingInfo40 Info { get; private set; }
		public static SiteSettingData40 Data { get; private set; }

		// initialize and create the setting objects
		static SiteSettings()
		{
			Admin = new SettingsMgr<SiteSettingInfo40>(ResetClass);
			Info = Admin.Info;
			Data = Info.Data;

			MainWindow.Instance.MsgLeftLine("");
			MainWindow.Instance.MsgLeftLine("at ctor SiteSettings|   status", Admin.Status.ToString());
			MainWindow.Instance.MsgLeftLine("at ctor SiteSettings|     path", Admin.Info.SettingPath);
			MainWindow.Instance.MsgLeftLine("at ctor SiteSettings| filename", Admin.Info.FileName);
			MainWindow.Instance.MsgLeftLine("");
		}

		// if we need to reset to the "factory" default
		public static void ResetClass()
		{
			Info = Admin.Info;
			Data = Info.Data;
		}
	}

#endregion

	*/
}