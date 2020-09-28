using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "AppSettings", Namespace = "")]
	internal class AppSettingInfo<T> : AppSettingInfoBase<T>
		where T : HeaderData, new ()
	{
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class AppSettingData : HeaderData
	{
		public string SetDataDescription() => "app setting file for SettingsManager v7.3";
		public string SetDataNotes() => "any notes go here";
		public string SetDataClassVersion() => "app 7.3a";

		[DataMember(Order = 1)]
		public int AppSettingsValue { get; set; } = 7;
	}

#endregion
}