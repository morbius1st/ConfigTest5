using System.Runtime.Serialization;

// File:             SiteSettings.cs
// Created:      -- ()

// ReSharper disable once CheckNamespace

namespace SettingsManager
{

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SiteSettingDataFile: IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "site 7.2cvt4si";

		[IgnoreDataMember]
		public string DataFileDescription =>"site setting file for SettingsManager v7.2cvt4";

		[IgnoreDataMember]
		public string DataFileNotes => "site / any notes go here";

		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 7;
	}

#endregion
}