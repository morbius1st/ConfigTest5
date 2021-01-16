#region using
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SettingsManager;
using SettingsManagerv74.DataStore;
using SettingsManagerv74.DataStore.DataSet2;
using UtilityLibrary;

#endregion

// projname: SettingsManagerProposed
// itemname: MainWindow
// username: jeffs
// created:  8/15/2020 12:20:30 PM

namespace SettingsManagerv74.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		private const int COLUMN = 30;
		private string nl = Environment.NewLine;
		private string textBoxMessage;

		private static MainWindow me;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			me = this;
		}

	#endregion

	#region public properties

		public static MainWindow Me => me;

		public string TextBoxMessage
		{
			get => textBoxMessage;
			set
			{
				if (value == null)
				{
					textBoxMessage = "";
					OnPropertyChange();
					return;
				}

				textBoxMessage += value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void AddMessage<T>(string message1, T message2, bool newline = true)
		{
			TextBoxMessage = addMessage(message1, message2);
		}

		public void AddMessage(string message1, string message2 = null, bool newline = true)
		{
			TextBoxMessage = addMessage(message1, message2);
		}

		public void ClearMessage()
		{
			TextBoxMessage = null;
		}

	#endregion

	#region private methods

		private void process()
		{
			// try
			// {
			// testUser();
			// testApp();
			// testSuite();
			// testMach();
			// testSite();
			

			// data
			testData();
			readData();

			// testData2();

			// listAllSettingFileInfo();

			// }
			// catch (Exception e)
			// {
			// 	AddMessage("!! EXCEPTION !!", e.Message);
			//
			// 	if (e.InnerException != null)
			// 	{
			// 		AddMessage("Inner Exception", e.InnerException.Message);
			// 	}
			// }
		}



		private string addMessage<T>(string message1, T message2, bool newline = true)
		{
			return message1.PadLeft(COLUMN)
				+ (!(message2?.Equals(null)).Equals(null) ? "| " + message2.ToString() : "")
				+ (newline ? nl : "");
		}

		private void listAllSettingFileInfo()
		{
			int idx = 0;
			AddMessage("");
			AddMessage("Setting File Info", "");

			// listSettingFileInfo(SiteSettings.Admin  , "site settings");
			// listSettingFileInfo(MachSettings.Admin  , "mach settings");
			// listSettingFileInfo(SuiteSettings.Admin , "suite settings");
			// listSettingFileInfo(AppSettings.Admin   , "app settings");
			listSettingFileInfo(UserSettings.Admin  , "user settings");

			AddMessage("");
		}

		private void listSettingFileInfo<TPath, TInfo, TData>(SettingsMgr<TPath, TInfo, TData> s, string who)
			where TPath : PathAndFileBase, new()
			where TInfo : SettingInfoBase<TData>, new()
			where TData : HeaderData, new()
		{
			AddMessage("");
			AddMessage("Setting File Info", who);

			s.Path.ConfigureFilePath();

			AddMessage(who + " |     root path", s.Path?.RootFolderPath ?? "is null");

			for (var i = 0; i < (s.Path.SubFolders ?? new string[0]).Length; i++)
			{
				AddMessage(who + " |      folder " + i, s.Path?.SubFolders[i] ?? "is null");
			}

			AddMessage(who + " |   folder path", s.Path?.SettingFolderPath ?? "is null");
			AddMessage(who + " | folder exists", s.Path?.FolderExists ?? false);
			AddMessage(who + " |     file path", s.Path?.SettingFilePath ?? "is null");
			AddMessage(who + " |   file exists", s.Path?.Exists ?? false);
		}

		delegate void setgData(string info, int test);


		private void testSetting<TPath, TInfo, TData>(SettingsMgr<TPath, TInfo, TData> s, string who, int test,
			setgData sd)
			where TPath : PathAndFileBase, new()
			where TInfo : SettingInfoBase<TData>, new()
			where TData : HeaderData, new()
		{
			AddMessage(who + " Settings", "start");

			AddMessage(who + " Settings", "Reading 1");

			s.Read();

			listSettingFileInfo(s, who + " settings");

			AddMessage("");

			AddMessage(who + " Settings", "Read");

			listHeader(s, who);

			// AddMessage(who + " Settings| file type", s.Info.FileType);
			// AddMessage(who + " Settings| desc", s.Info.Description);
			// AddMessage(who + " Settings| data ver", s.Info.DataClassVersion);
			// AddMessage(who + " Settings| notes", s.Info.Notes);
			// AddMessage(who + " Settings| saved by", s.Info.Header.SavedBy);

			sd(who, test);

			AddMessage("");
		}

		private void listHeader<TPath, TInfo, TData>(SettingsMgr<TPath, TInfo, TData> s, string who)
			where TPath : PathAndFileBase, new()
			where TInfo : SettingInfoBase<TData>, new()
			where TData : HeaderData, new()
		{
			AddMessage(who + " Settings| file type", s.Info.FileType);
			AddMessage(who + " Settings| desc", s.Info.Description);
			AddMessage(who + " Settings| data ver", s.Info.DataClassVersion);
			AddMessage(who + " Settings| notes", s.Info.Notes);
			AddMessage(who + " Settings| saved by", s.Info.Header.SavedBy);
		}

	#if SITE_SETTINGS

		private void testSite()
		{
			testSetting(SiteSettings.Admin, "Site", 814, setgDataSite);
		}

		private void setgDataSite(string who, int test)
		{
			AddMessage(who + " Settings", "test file specific");
			AddMessage(who + " Settings|  value", SiteSettings.Data.SiteSettingsValue);

			AddMessage(who + " Settings", "Changing Value");
			SiteSettings.Data.SiteSettingsValue = test;

			AddMessage(who + " Settings", "Writing");
			SiteSettings.Admin.Write();

			AddMessage(who + " Settings", "Reading 2");
			SiteSettings.Admin.Read();
			AddMessage(who + " Settings", "Read");
			AddMessage(who + " Settings| value", SiteSettings.Data.SiteSettingsValue);
		}
	#endif

	#if MACH_SETTINGS

		private void testMach()
		{
			testSetting(MachSettings.Admin, "Mach", 813, setgDataMach);
		}

		private void setgDataMach(string who, int test)
		{
			AddMessage(who + " Settings", "test file specific");
			AddMessage(who + " Settings|  value", MachSettings.Data.MachSettingsValue);

			AddMessage(who + " Settings", "Changing Value");
			MachSettings.Data.MachSettingsValue = test;

			AddMessage(who + " Settings", "Writing");
			MachSettings.Admin.Write();

			AddMessage(who + " Settings", "Reading 2");
			MachSettings.Admin.Read();
			AddMessage(who + " Settings", "Read");
			AddMessage(who + " Settings| value", MachSettings.Data.MachSettingsValue);
		}

	#endif

	#if SUITE_SETTINGS

		private void testSuite()
		{
			testSetting(SuiteSettings.Admin, "Suite", 812, setgDataSuite);
		}

		private void setgDataSuite(string who, int test)
		{
			AddMessage(who + " Settings", "test file specific");
			AddMessage(who + " Settings|  value", SuiteSettings.Data.SuiteSettingsValue);

			AddMessage(who + " Settings", "Changing Value");
			SuiteSettings.Data.SuiteSettingsValue = test;

			AddMessage(who + " Settings", "Writing");
			SuiteSettings.Admin.Write();

			AddMessage(who + " Settings", "Reading 2");
			SuiteSettings.Admin.Read();
			AddMessage(who + " Settings", "Read");
			AddMessage(who + " Settings| value", SuiteSettings.Data.SuiteSettingsValue);
		}
	#endif

	#if APP_SETTINGS

		private void testApp()
		{
			testSetting(AppSettings.Admin, "App", 811, setgDataApp);
		}

		private void setgDataApp(string who, int test)
		{
			AddMessage(who + " Settings", "test file specific");
			AddMessage(who + " Settings|  value", AppSettings.Data.AppSettingsValue);

			AddMessage(who + " Settings", "Changing Value");
			AppSettings.Data.AppSettingsValue = test;

			AddMessage(who + " Settings", "Writing");
			AppSettings.Admin.Write();

			AddMessage(who + " Settings", "Reading 2");
			AppSettings.Admin.Read();
			AddMessage(who + " Settings", "Read");
			AddMessage(who + " Settings| value", AppSettings.Data.AppSettingsValue);
		}

	#endif

	#if USER_SETTINGS

		private void testUser()
		{
			testSetting(UserSettings.Admin, "User", 810, setgDataUser);
		}

		private void setgDataUser(string who, int test)
		{
			AddMessage(who + " Settings", "test file specific");
			AddMessage(who + " Settings|  value", UserSettings.Data.UserSettingsValue);

			AddMessage(who + " Settings", "Changing Value");
			UserSettings.Data.UserSettingsValue = test;

			AddMessage(who + " Settings", "Writing");
			UserSettings.Admin.Write();

			AddMessage(who + " Settings", "Reading 2");
			UserSettings.Admin.Read();
			AddMessage(who + " Settings", "Read");
			AddMessage(who + " Settings| value", UserSettings.Data.UserSettingsValue);
		}

	#endif

		private void listTestData2<T>(DataManager<T> d) where T: class, HeaderData, new()
		{
			AddMessage("Data store 2x", "");
			AddMessage("ver", d.Info?.DataClassVersion ?? "is null" );
			AddMessage("desc", d.Info?.Description ?? "is null" );
			AddMessage("note", d.Info?.Notes ?? "is null");
		}


		private void testData2()
		{
			DataManager<DataSet2> dm1 = new DataManager<DataSet2>();
			FilePath<FileNameSimple> path1 = new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet2_1.xml");
			testData2(dm1, path1, "dm21");

			DataManager<DataSet2> dm2 = new DataManager<DataSet2>();
			FilePath<FileNameSimple> path2 = new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet2_2.xml");
			testData2(dm2, path2, "dm22");

		}


		private void testData2<T>(
			DataManager<T> dm1,
			FilePath<FileNameSimple> dataFile21,
			string who ) where T: DataSet2, new()
		{
			AddMessage("Data store "
				+ who, "start");

			listTestData2(dm1);

			AddMessage("Data store "
				+ who, "create");
			dm1.Create(dataFile21);
			dm1.Admin.Read();

			AddMessage("Data store "
				+ who, "created");
			listTestData2(dm1);

			AddMessage("Data store "
				+ who, "SampleDataString1| " + dm1.Data.SampleDataString1);
			AddMessage("Data store "
				+ who, "   SampleDataInt1| " + dm1.Data.SampleDataInt1);

			AddMessage("Data store "
				+ who, "changing");
			dm1.Data.SampleDataString1 = "(" + who + ")  to be or not to be, that is the question";

			AddMessage("Data store "
				+ who, "writing");
			dm1.Admin.Write();

			AddMessage("Data store "
				+ who, "resetting");
			dm1 = new DataManager<T>();

			AddMessage("Data store "
				+ who, "configure");
			dm1.Configure(dataFile21);

			AddMessage("Data store "
				+ who, "read");
			dm1.Admin.Read();

			AddMessage("Data store "
				+ who, "SampleDataString1| " + dm1.Data.SampleDataString1);
			AddMessage("Data store "
				+ who, "   SampleDataInt1| " + dm1.Data.SampleDataInt1);

			AddMessage("\n");

		}


		private void readData()
		{
			AddMessage("\n");
			AddMessage("Data Settings", "Test: read data");

			DataManager<DataSet1> ds1_1 = new DataManager<DataSet1>();

			AddMessage("Data Set 1_1", "configure");
			ds1_1.Configure(
				new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_1.xml" ));

			AddMessage("Data Set 1_1", "configured");

			ds1_1.Admin.Read();

			listHeader(ds1_1.Admin, "ds1_1");

			listSettingFileInfo( ds1_1.Admin, "ds1_1");
		}

		private void testData()
		{
			AddMessage("Data Settings", "Test");

			DataManager<DataSet1> ds1_1 = new DataManager<DataSet1>();

			DataManager<DataSet1> ds1_2 = new DataManager<DataSet1>();

			AddMessage("Data Set 1_1", "configure");
			ds1_1.Configure(
				new FilePath<FileNameSimple>(
					@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_1.xml"));

			AddMessage("Data Set 1_1", "configured");

			listHeader(ds1_1.Admin, "ds1_1");

			listSettingFileInfo( ds1_1.Admin, "ds1_1");


			AddMessage("");

			AddMessage("Data Set 1_2", "configure");
			ds1_2.Configure(
				new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_2.xml"));
			AddMessage("Data Set 1_2", "configured");

			listHeader(ds1_2.Admin, "ds1_2");

			listSettingFileInfo(ds1_2.Admin, "ds1_2");


			AddMessage("");

			// ds1_1.Admin.Write();

			ds1_1.Admin.Read();

			testDs1_1(ds1_1);
			testDs1_2(ds1_2);
		}

		private void testDs1_1(DataManager<DataSet1> ds1_1)
		{
			AddMessage("\n");
			AddMessage("Data Settings", "Test: ds1_1");

			if (ds1_1.IsInitialized)
			{
				listDataSet(ds1_1, "Data Set 1_1");

				AddMessage("Data Set 1_1", "changing");

				ds1_1.Info.Description = "DataSet 1_1";
				ds1_1.Info.DataClassVersion = "1.1.0.0";
				ds1_1.Info.Notes = "Initial Save 1_1";

				ds1_1.Data.SampleDataString1 = "Sample data set 1_1";
				ds1_1.Data.SampleDataDouble1 = 117.4;

				AddMessage("Data Set 1_1", "writing");
				ds1_1.Admin.Write();
				AddMessage("Data Set 1_1", "written");

				listDataSet(ds1_1, "Data Set 1_1");
			}
		}

		private void testDs1_2(DataManager<DataSet1> ds1_2)
		{
			AddMessage("\n");
			AddMessage("Data Settings", "Test: ds1_2");

			if (ds1_2.IsInitialized)
			{
				listDataSet(ds1_2, "Data Set 1_2");

				AddMessage("Data Set 1_2", "changing");

				ds1_2.Info.Description = "DataSet 1_2";
				ds1_2.Info.DataClassVersion = "1.2.0.0";
				ds1_2.Info.Notes = "Initial Save 1_2";

				ds1_2.Data.SampleDataString1 = "Sample data set 1_2";
				ds1_2.Data.SampleDataDouble1 = 127.4;

				AddMessage("Data Set 1_2", "writing");
				ds1_2.Admin.Write();
				AddMessage("Data Set 1_2", "written");

				listDataSet(ds1_2, "Data Set 1_2");
			}
		}

		private void listDataSet(DataManager<DataSet1> ds, string title)
		{
			if (ds.IsInitialized)
			{
				AddMessage(title, "reading");
				ds.Admin.Read();
				AddMessage(title, "read");

				AddMessage(title, "SampleDataString1| " + ds.Data.SampleDataString1);
				AddMessage(title, "SampleDataDouble1| " + ds.Data.SampleDataDouble1);
			}
		}

	#endregion

	#region event processing

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void winMainSetgMgrProposed_Loaded(object sender, RoutedEventArgs e)
		{
			process();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}