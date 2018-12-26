using System.Collections.Generic;
using System.Runtime.Serialization;


namespace SettingsManagerV30
{

	

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "UserSettings")]
	public class UserSettings_old : UserSetgBase
	{
		protected override string FILEVERSION => "2.2";

		[DataMember]
		public int UnCategorizedValue = 1000;

		// added with version 2.1
		[DataMember]
		public int UnCategorizedValue2 = 2000;

		// added with version 2.2
		[DataMember]
		public int UnCategorizedValue3 = 3000;

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

		// upgrade the prior version to the new version
		public override void Upgrade(UserSetgBase prior)
		{
			UserSettings21 p = (UserSettings21)prior;

			Heading.SettingFileNotes =
				p.Heading.SettingFileNotes + " :: updated to v" + FileVersion;

			this.UnCategorizedValue = p.UnCategorizedValue;
			this.UnCategorizedValue2 = p.UnCategorizedValue2;
			this.GeneralValues.TestB = p.GeneralValues.TestB;
			this.GeneralValues.TestD = p.GeneralValues.TestD;
			this.GeneralValues.TestI = p.GeneralValues.TestI;
			this.GeneralValues.TestS = p.GeneralValues.TestS;
			this.MainWindow.Height = p.MainWindow.Height;
			this.MainWindow.Width = p.MainWindow.Width;

			for (int i = 0; i < this.GeneralValues.TestIs.Length; i++)
			{
				this.GeneralValues.TestIs[i] =
					p.GeneralValues.TestIs[i];
			}

			p.GeneralValues.TestSs.CopyTo(this.GeneralValues.TestSs, 0);

			foreach (KeyValuePair<string, TestStruct> kvp in p.TestDictionary3)
			{
				if (TestDictionary3.ContainsKey(kvp.Key))
				{
					this.TestDictionary3[kvp.Key] =
						p.TestDictionary3[kvp.Key];
				}
			}
		}
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