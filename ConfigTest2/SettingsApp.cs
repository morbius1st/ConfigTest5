#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#endregion

// itemname:	SettingsApp
// username:	jeffs
// created:		12/31/2017 6:53:43 PM


namespace ConfigTest2
{
	public class AppSettings : SettingsPathFileAppBase
	{
		public int AppI { get; set; } = 0;
		public bool AppB { get; set; } = false;
		public double AppD { get; set; } = 0.0;
		public string AppS { get; set; } = "this is a App";
		public int[] AppIs { get; set; } = new[] {20, 30};
		
	}
}
