using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "SuiteSettings", Namespace = "")]
	internal class SuiteSettingInfo<T> : SuiteSettingInfoBase<T>
		where T : HeaderData, new ()
	{
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class SuiteSettingData : HeaderData
	{
		public string SetDataDescription() => "suite setting file for SettingsManager v7.3";
		public string SetDataNotes() => "any notes go here";
		public string SetDataClassVersion() => "suite 7.3su";

		[DataMember(Order = 1)]
		public int SuiteSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public string SiteRootPath { get; set; }
			= @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerV73" ;
	}

#endregion
}