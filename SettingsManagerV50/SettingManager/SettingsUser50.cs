using System.Runtime.Serialization;
using SettingsManager;
using SettingsManagerV50.Windows;

// projname: SettingsManagerV40
// itemname: UserSettingInfoInfo50
// username: jeffs
// created:  12/23/2018 1:14:35 PM


namespace SettingsManagerV50
{
#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "UserSettingData50")]
	public class UserSettingData50
	{
		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 5;
	}

#endregion

#region management info class

	[DataContract(Name = "UserSettingInfoInfo50")]
	public class UserSettingInfoInfo50 : UserSettingInfoBase
	{
		[DataMember]
		public UserSettingData50 Data = new UserSettingData50();

		public override string DataClassVersion => "5.0u";
		public override string Description => "user setting file for SettingsManagerV50";
		public override void UpgradeFromPrior(SettingInfoBase prior) { }
	}

#endregion

#region user management root class

	public static class UserSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<UserSettingPath50, UserSettingInfoInfo50> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static UserSettingPath50 Path { get; private set; } = new UserSettingPath50();
		public static UserSettingInfoInfo50 Info { get; private set; }
		public static UserSettingData50 Data { get; private set; }

		// initialize and create the setting objects
		static UserSettings()
		{
			Admin = new SettingsMgr<UserSettingPath50, UserSettingInfoInfo50>(Path, ResetData);
			Info = Admin.Info;

			MainWindow.Instance.MsgLeftLine("");
			MainWindow.Instance.MsgLeftLine("at ctor UserSettings|   status", Admin.Status.ToString());
			MainWindow.Instance.MsgLeftLine("at ctor UserSettings|     path", Path.SettingPath);
			MainWindow.Instance.MsgLeftLine("at ctor UserSettings| filename", Path.FileName);
		}

		public static void ResetData()
		{
			// this makes sure the above static class points
			// to the current data structure
			Info  = Admin.Info;
			Data  = Info.Data;
		}
	}


#endregion
}