using System.Runtime.Serialization;

// username: jeffs
// Created:      -- ()

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "SuiteSettings", Namespace = "")]
	internal class SuiteSettingInfo<T> : SuiteSettingInfoBase<T>
		where T : new ()
	{
		public SuiteSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion =  "suite 7.2su";
			Description =  "suite setting file for SettingsManager v7.2";
			Notes = "any notes go here";
		}

		internal override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SuiteSettingData
	{
		[DataMember(Order = 1)]
		public int SuiteSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public string SiteRootPath { get; set; }
			= @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerV72\SiteSettings" ;
	}

#endregion
}