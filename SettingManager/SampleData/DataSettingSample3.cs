#region using directives

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SettingsManager.SampleData;

#endregion

// itemname: DataSettingSample2
// username: jeffs
// created:  4/28/2020 1:10:24 PM

namespace SettingsManagerV70.SampleData
{
#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "SampleDataData3", Namespace = "")]
	public class SampleDataData3
	{
		[DataMember(Order = 1)]
		public string Description { get; private set; } = "This is SampleDataData3";

		[DataMember(Order = 2)]
		public ObservableCollection<SampleItem2> Root { get; set; }

	}

#endregion
}
