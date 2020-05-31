using System.Collections.Generic;
using System.Runtime.Serialization;
using SettingsManager;

using static UtilityLibrary.MessageUtilities2;

namespace SettingsManagerV30
{
	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "AppSettingData20")]
	public class AppSettingData20
	{
		[DataMember(Order = 1)]
		public int AppI { get; set; } = 0;

		[DataMember(Order = 2)]
		public bool AppB { get; set; } = false;

		[DataMember(Order = 3)]
		public double AppD { get; set; } = 0.0;

		[DataMember(Order = 4)]
		public string AppS { get; set; } = "this is an App";

		[DataMember(Order = 5)]
		public int[] AppIs { get; set; } = new[] {20, 30};

		[DataMember(Order = 20)]
		public int AppI20 { get; set; } = 0;
	}
	
	[DataContract(Name = "AppSettingInfo20")]
	public class AppSettingInfo20 : AppSettingBase
	{
		[DataMember]
		public AppSettingData20 Data = new AppSettingData20();

		public override string ClassVersion => "2.0";

		// upgrade from pre 2.0 to 2.0 - n/a
		public override void UpgradeFromPrior(SettingBase prior)
		{
		}
	}



}