#region Using directives
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		12/31/2017 8:29:37 AM


namespace ConfigTest2
{
	public class UserSettings : SettingsPathFileUserBase
	{
		public int UnCategorizedValue = 10;
		public generalValues GeneralValues = new generalValues();
		public window1 MainWindow { get; set; } = new window1();
	}

	public class window1
	{
		[XmlAttribute]
		public int height = 50;
		[XmlAttribute]
		public int width = 100;
	}

	public class generalValues
	{
		public int TestI = 0;
		public bool TestB = false;
		public double TestD = 0.0;
		public string TestS = "this is a test";
		public int[] TestIs = new[] { 20, 30 };
		public string[] TestSs = new[] { "user 1", "user 2", "user 3" };
	}

}
