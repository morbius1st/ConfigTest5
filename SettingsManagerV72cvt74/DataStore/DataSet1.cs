#region using directives

using System.Runtime.Serialization;
using SettingsManager;

#endregion

// itemname: DataSettingSample2
// username: jeffs
// created:  4/28/2020 1:10:24 PM

namespace SettingsManager
{

#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class DataStoreSet1: IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "data 7.2cvt4d";

		[IgnoreDataMember]
		public string DataFileDescription =>"this is a description from the data class for SettingsManager v7.2cvt4";

		[IgnoreDataMember]
		public string DataFileNotes => "data / any notes go here";
		[DataMember(Order = 1)]
		public string SampleDataString1 { get; set; } = "this is a sample string";
		
		[DataMember(Order = 2)]
		public int SampleDataInt1 { get; set; } = 72;
		
		[DataMember(Order = 3)]
		public double SampleDataDouble1 { get; set; } = 7.2;

	}

#endregion
}
