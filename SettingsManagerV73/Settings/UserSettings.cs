using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "UserSettings", Namespace = "")]
	internal class UserSettingInfo<T> : UserSettingInfoBase<T>
		where T : HeaderData, new ()
	{
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class UserSettingData : HeaderData
	{

		public string SetDataDescription() => "this is a description from the data class";
		public string SetDataNotes() => "any notes go here"; 
		public string SetDataClassVersion() => "user 7.3u";

		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 7;

	}

#endregion
}