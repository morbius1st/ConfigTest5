using System.Collections.Generic;
using System.Runtime.Serialization;
using UtilityLibrary;

namespace SettingsManagerV26
{
	public static class SettingsUser
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<UserSettings> USettings { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static UserSettings USet { get; private set; }

		// initalize and create the setting objects
		static SettingsUser()
		{
			USettings = new SettingsMgr<UserSettings>(ResetClass);
			USet = USettings.Settings;
		}

		public static void ResetClass()
		{
			USet = USettings.Settings;
		}

	}


	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "UserSettings")]
	public class UserSettings : SettingsPathFileUserBase
	{
		public const string USERSETTINGFILEVERSION = "2.1";

		[DataMember]
		public int UnCategorizedValue = 1000;

		[DataMember]
		public int UnCategorizedValue2 = 2000;

		[DataMember]
		public GeneralValues GeneralValues = new GeneralValues();

		[DataMember]
		public Window1 MainWindow { get; set; } = new Window1();

		[DataMember(Name = "DictionaryTest3")]
		public CustDict<string, TestStruct> TestDictionary3 =
			new CustDict<string, TestStruct>()
			{
				{"one", new TestStruct(1, 2, 3)},
				{"two", new TestStruct(1, 2, 3)},
				{"three", new TestStruct(1, 2, 3)}
			};

		public override string FileVersion { get; set; }
	}

	// sample sub-class of dictionary to provide names to elements
	[CollectionDataContract(Name = "CustomDict", KeyName = "key", ValueName = "data", ItemName = "row")]
	public class CustDict<T1, T2> : Dictionary<T1, T2>
	{
	}
	// sample struct / data
	public struct TestStruct
	{
		[DataMember(Name = "line1")]
		public int IntA;

		[DataMember(Name = "line2")]
		public int IntB;

		[DataMember(Name = "line3")]
		public int IntC;

		public TestStruct(int a,
			int b,
			int c)
		{
			IntA = a;
			IntB = b;
			IntC = c;
		}
	}

	// sample class / data
	public class GeneralValues
	{
		public int TestI = 0;
		public bool TestB = false;
		public double TestD = 0.0;
		public string TestS = "this is a test";
		public int[] TestIs = new[] {20, 30};
		public string[] TestSs = new[] {"user 1", "user 2", "user 3"};
	}

	// sample class / data
	public class Window1
	{
		public int Height = 50;
		public int Width = 100;
	}
}