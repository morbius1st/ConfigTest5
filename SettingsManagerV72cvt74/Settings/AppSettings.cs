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
	public class AppSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "app 7.2cvt4a";

		[IgnoreDataMember]
		public string DataFileDescription => "app setting file for SettingsManager v7.2cvt4";

		[IgnoreDataMember]
		public string DataFileNotes => "app / any notes go here";

		[DataMember(Order = 1)]
		public int AppSettingsValue { get; set; } = 7;
	}

#endregion
}