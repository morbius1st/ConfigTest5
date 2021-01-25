using System.Runtime.Serialization;

// username: jeffs
// created:  12/23/2018 1:14:35 PM

// ReSharper disable once CheckNamespace

namespace SettingsManager
{

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class UserSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "user 7.2cvt4u";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file for SettingsManager v7.2cvt4";

		[IgnoreDataMember]
		public string DataFileNotes => "user / any notes go here";

		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 7;
	}

#endregion
}