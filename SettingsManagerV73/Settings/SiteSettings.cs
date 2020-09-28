using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "SiteSettings", Namespace = "")]
	internal class SiteSettingInfo<T> : SiteSettingInfoBase<T>
		where T : HeaderData, new ()
	{
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class SiteSettingData : HeaderData
	{
		public string SetDataDescription() => "this is a description from the data class";
		public string SetDataNotes() => "any notes go here";
		public string SetDataClassVersion() => "site 7.3as";

		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 7;
	}

#endregion
}