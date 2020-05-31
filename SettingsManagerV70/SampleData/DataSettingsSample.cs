#region + Using Directives

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SettingsManager;
using SettingsManager.SampleData;

#endregion


// projname: SettingsManagerV70.SampleData
// itemname: DataSettingsSample
// username: jeffs
// created:  4/27/2020 11:16:38 PM


namespace SettingsManagerV70.SampleData
{
#region data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "SampleDataData", Namespace = "")]
	public class SampleDataData
	{
		[DataMember(Order = 1)]
		public int SampleValue { get; set; } = 101;

		[DataMember(Order = 2)]
		public ObservableCollection<SampleItem2> DataRoot { get; set; }

	}

#endregion

#region info class

	[DataContract(Name = "SampleDataInfo", Namespace = "")]
	public class SampleDataInfo<Tdata> : SettingInfoBase<Tdata>
		where Tdata : new ()
	{
		public override string DataClassVersion => "1.0";
		public override string Description => "sample data file";
		public override SettingFileType FileType => SettingFileType.OTHER;
		public override void UpgradeFromPrior(SettingInfoBase<Tdata> prior) { }
	}

#endregion

#region management classes

	public class SampleDataPath : PathAndFileBase
	{
		public override void Configure()
		{
//			FileName = @"SampleData.xml";
//			RootPath =  CsUtilities.AssemblyDirectory;
		}
	}

	// ReSharper disable once ClassNeverInstantiated.Global
	public class SampleDataStore :
		BaseSettings<SampleDataPath,
		SampleDataInfo<SampleDataData>,
		SampleDataData> { }

#endregion

}
