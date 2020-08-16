#region using directives

using System.Runtime.Serialization;
using SettingsManager;

#endregion

// itemname: DataSettingSample2
// username: jeffs
// created:  4/28/2020 1:10:24 PM

namespace SettingsManager
{
#region info class

	// [DataContract(Name = "DataStorage", Namespace = "")]
	// public class StorageMgrInfo<TData> : StorageMgrInfoBase<TData>
	// 	where TData : new ()
	// {
	// 	[DataMember(Order = 1)]
	// 	public override string DataClassVersion { get; set; } = "base version";
	//
	// 	[DataMember(Order = 2)]
	// 	public override string Description { get; set; } = "base description";
	//
	// 	[DataMember(Order = 3)]
	// 	public override string Notes { get; set; } = "base notes";
	// }

#endregion
#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class DataSet1
	{
		[DataMember(Order = 1)]
		public string SampleDataString1 { get; set; } = "this is a sample string";
		
		[DataMember(Order = 2)]
		public int SampleDataInt1 { get; set; } = 72;
		
		[DataMember(Order = 3)]
		public double SampleDataDouble1 { get; set; } = 7.2;

	}

#endregion
}
