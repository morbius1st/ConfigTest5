using System.Runtime.Serialization;

// File:             MachineSettings.cs
// Created:      -- ()

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class MachSettingDataFile: IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "mach 7.2cvt4m";

		[IgnoreDataMember]
		public string DataFileDescription =>"mach setting file for SettingsManager v7.2cvt4";

		[IgnoreDataMember]
		public string DataFileNotes => "mach / any notes go here";

		[DataMember(Order = 1)]
		public int MachSettingsValue { get; set; } = 7;
	}

#endregion
}