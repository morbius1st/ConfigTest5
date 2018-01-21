#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		12/31/2017 8:29:37 AM


namespace ConfigTest5
{
	[DataContract(Name = "UserSettings")]
	public class UserSettings : SettingsPathFileUserBase
	{
		public const string USERSETTINGFILEVERSION = "2.0";

		[DataMember]
		public int UnCategorizedValue = 1000;
		[DataMember]
		public int UnCategorizedValue2 = 2000;
		[DataMember]
		public generalValues GeneralValues = new generalValues();
		[DataMember]
		public window1 MainWindow { get; set; } = new window1();

		[DataMember(Name = "DictionaryTest3")]
		public CustDict<string, testStruct> testDictionary3 =
			new CustDict<string, testStruct>()
			{
				{"one", new testStruct(1, 2, 3)},
				{"two", new testStruct(1, 2, 3)},
				{"three", new testStruct(1, 2, 3)}
			};
	}
	
	public struct testStruct
	{
		[DataMember(Name = "line1")]
		public int intA;
		[DataMember(Name = "line2")]
		public int intB;
		[DataMember(Name = "line3")]
		public int intC;

		public testStruct(int a, int b, int c)
		{
			intA = a;
			intB = b;
			intC = c;
		}
	}

	[CollectionDataContract(Name = "CustomDict", KeyName = "key", ValueName = "data", ItemName = "row")]
	public class CustDict<T1, T2> : Dictionary<T1, T2>
	{
	}

	public class window1
	{
		public int height = 50;
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
