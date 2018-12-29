using System.Runtime.Serialization;
using SettingManager;

// projname: SettingsManagerV30
// itemname: UserSettingInfo20
// username: jeffs

namespace SettingsManagerV30
{

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Name = "UserSettingData20")]
	public class UserSettingData20
	{
		[DataMember]
		public int UnCategorizedValue = 1000;

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

	[DataContract(Name = "UserSettingInfo20")]
	public class UserSettingInfo20 : UsrSettingBase
	{
		[DataMember]
		public UserSettingData20 Data = new UserSettingData20();

		public override string ClassVersion => "2.0";

		// this is the base of the chain - this has nothing to upgrade from
		public override void Upgrade(SettingsPathFileBase prior)
		{
		}
	}

}