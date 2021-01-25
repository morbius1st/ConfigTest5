#region using directives

using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Windows.Media.Animation;
using SettingsManager;
using UtilityLibrary;

#endregion

// itemname: DataSettingSample2
// username: jeffs
// created:  4/28/2020 1:10:24 PM

namespace SettingsManagerv74.DataStore.DataSet2
{

#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class DataStoreSet2 : IDataFile
	{
		[IgnoreDataMember]
		public string DataFileVersion => "data 7.4d";

		[IgnoreDataMember]
		public string DataFileDescription =>"this is a description from the data class for SettingsManager v7.4";

		[IgnoreDataMember]
		public string DataFileNotes => "data / any notes go here";

		[DataMember(Order = 1)]
		public string SampleDataString1 { get; set; } = "this is a sample string";
		
		[DataMember(Order = 2)]
		public int SampleDataInt1 { get; set; } = 72;
		
		[DataMember(Order = 3)]
		public double SampleDataDouble1 { get; set; } = 7.4;
	}

#endregion

}
