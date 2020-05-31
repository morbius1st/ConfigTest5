using System.Collections.Generic;
using System.Runtime.Serialization;
using SettingsManager;

using static UtilityLibrary.MessageUtilities2;

// projname: SettingsManagerV30
// itemname: UserSettingInfo21
// username: jeffs

namespace SettingsManagerV30
{
	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "UserSettingData21")]
	public class UserSettingData21
	{
		[DataMember]
		public int UnCategorizedValue = 1000;

		// added with version 2.1
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
	}

	[DataContract(Name = "UserSettingInfo21")]
	public class UserSettingInfo21 : UserSettingBase
	{
		[DataMember]
		public UserSettingData21 Data = new UserSettingData21();

		public override string ClassVersion => "2.1";

		// upgrade the prior version to this version
		public override void UpgradeFromPrior(SettingBase prior)
		{
			UserSettingInfo20 p = (UserSettingInfo20) prior;

			Header.Notes = 
				p.Header.Notes + " :: updated to v" + ClassVersion;

			Data.UnCategorizedValue  = p.Data.UnCategorizedValue;
			Data.GeneralValues.TestB = p.Data.GeneralValues.TestB;
			Data.GeneralValues.TestD = p.Data.GeneralValues.TestD;
			Data.GeneralValues.TestI = p.Data.GeneralValues.TestI;
			Data.GeneralValues.TestS = p.Data.GeneralValues.TestS;
			Data.MainWindow.Height   = p.Data.MainWindow.Height;
			Data.MainWindow.Width    = p.Data.MainWindow.Width;

			for (int i = 0; i < Data.GeneralValues.TestIs.Length; i++)
			{
				Data.GeneralValues.TestIs[i] =
					p.Data.GeneralValues.TestIs[i];
			}

			p.Data.GeneralValues.TestSs.CopyTo(Data.GeneralValues.TestSs, 0);

			foreach (KeyValuePair<string, TestStruct> kvp in p.Data.TestDictionary3)
			{
				if (Data.TestDictionary3.ContainsKey(kvp.Key))
				{
					Data.TestDictionary3[kvp.Key] =
						p.Data.TestDictionary3[kvp.Key];
				}
			}
		}
	}

}