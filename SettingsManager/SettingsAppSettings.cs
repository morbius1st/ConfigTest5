using System.Runtime.Serialization;

namespace SettingsManager
{
	[DataContract(Name = "AppSettings")]
	public class AppSettings : SettingsPathFileAppBase
	{
		public const string APPSETTINGFILEVERSION = "2.1";

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
		
	}
}