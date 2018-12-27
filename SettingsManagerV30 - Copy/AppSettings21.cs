using System.Runtime.Serialization;
using SettingManager;

namespace SettingsManagerV30
{

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "AppSettingData21")]
	public class AppSettingData21
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

		[DataMember(Order = 21)]
		public bool AppB21 { get; set; } = false;
	}
	
	[DataContract(Name = "AppSettingInfo21")]
	public class AppSettingInfo21 : AppSettingBase
	{
		[DataMember]
		public AppSettingData21 Data = new AppSettingData21();

		public override string ClassVersion => "2.1";
//		protected override string CLASSVERSION { get; } = "2.1";

		// upgrade from 2.0 to 2.1
		public override void Upgrade(SettingBase prior)
		{
			AppSettingInfo20 p = (AppSettingInfo20) prior;

			Heading.Notes =
				p.Heading.Notes + " :: updated to v" + ClassVersion;

			Data.AppI = p.Data.AppI;
			Data.AppB = p.Data.AppB;
			Data.AppD = p.Data.AppD;
			Data.AppS = p.Data.AppS;
			Data.AppI20 = p.Data.AppI20;

			for (int i = 0;
				i < (Data.AppIs.Length < p.Data.AppIs.Length ? Data.AppIs.Length : p.Data.AppIs.Length);
				i++)
			{
				Data.AppIs[i] = p.Data.AppIs[i];
			}

		}
	}



}