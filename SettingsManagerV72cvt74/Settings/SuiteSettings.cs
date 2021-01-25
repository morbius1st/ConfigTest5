using System.Runtime.Serialization;

// username: jeffs
// Created:      -- ()

// ReSharper disable once CheckNamespace

namespace SettingsManager
{

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SuiteSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "suite 7.2cvt4su";

		[IgnoreDataMember]
		public string DataFileDescription => "suite setting file for SettingsManager v7.2cvt4";

		[IgnoreDataMember]
		public string DataFileNotes => "suite / any notes go here";

		[DataMember(Order = 1)]
		public int SuiteSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public string SiteRootPath { get; set; }
			= @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManagerV72cvt74" ;
	}

#endregion
}