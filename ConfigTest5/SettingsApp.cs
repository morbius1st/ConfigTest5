#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#endregion

// itemname:	SettingsApp
// username:	jeffs
// created:		12/31/2017 6:53:43 PM


namespace ConfigTest5
{
	[DataContract(Name = "AppSettings")]
	public class AppSettings : SettingsPathFileAppBase
	{
		public const string APPSETTINGFILEVERSION = "2.0";

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
