#region using

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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

		private const string MARKER_DN = "vvvvvvvvvvvvvvvvvvvv";
		private const string MARKER_UP = "^^^^^^^^^^^^^^^^^^^^";

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

		private string addMessage<T>(string message1, T message2, bool newline = true)
		{
			return message1.PadLeft(COLUMN)
				+ (!(message2?.Equals(null)).Equals(null) ? "| " + message2.ToString() : "")
				+ (newline ? nl : "");
		}

	#endregion

	#region private methods

		private void process()
		{
			try
			{

				// USER_SETTINGS, APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS

				// int x = 1;
				//
				// SettingsMgr<MachSettingPath, MachSettingInfo<MachSettingDataFile>, MachSettingDataFile> a =
				// 	MachSettings.Admin;
				// SettingsMgr<SiteSettingPath, SiteSettingInfo<SiteSettingDataFile>, SiteSettingDataFile> b =
				// 	SiteSettings.Admin;


			#if  USER_SETTINGS
				testUser();
			#endif

			#if APP_SETTINGS
				testApp();
			#endif

			#if SUITE_SETTINGS
				testSuite();
			#endif

			#if MACH_SETTINGS
				testMach();
			#endif

			#if SITE_SETTINGS
				testSite();
				testSite2();
			#endif

				// data
				// testData();
				// readData();

				// testData2();

				listAllSettingFileInfo();
			}
			catch (Exception e)
			{
				Debug.WriteLine("!! EXCEPTION !! " +  e.Message);

				if (e.InnerException != null)
				{
					Debug.WriteLine("   !! INNER EXCEPTION !! " +  e.InnerException.Message);
				}

				throw;
			}
		}

	#if SITE_SETTINGS
		private void testSite2()
		{
			try
			{
				SiteSettings.Path.RootFolderPath = @"C:\ProgramData\CyberStudio\SettingsManagerV74";
			}
			catch (Exception e)
			{
				Debug.WriteLine("caught error| " + e.Message);

				if (e.InnerException != null)
				{
					Debug.WriteLine("caught inner error| " + e.InnerException.Message);
				}
			}

			testSetting(SiteSettings.Admin, "Site", 814, setgDataSite2);
		}

		private void setgDataSite2(string who, int test)
		{
			AddMessage(who, "test file specific");
			AddMessage(who + " Settings|  value", SiteSettings.Data.SiteSettingsValue);

			AddMessage(who, "Changing Value");
			SiteSettings.Data.SiteSettingsValue = test;

			AddMessage(who, "Writing");
			SiteSettings.Admin.Write();

			AddMessage(who, "Reading 2");
			SiteSettings.Admin.Read();
			AddMessage(who, "Read");
			AddMessage(who + " Settings| value", SiteSettings.Data.SiteSettingsValue);
		}
	#endif
		
	#if SITE_SETTINGS
		private void testSite()
		{
			testSetting(SiteSettings.Admin, "Site", 814, setgDataSite);
		}

		private void setgDataSite(string who, int test)
		{
			AddMessage(who, "test file specific");
			AddMessage(who + " Settings|  value", SiteSettings.Data.SiteSettingsValue);

			AddMessage(who, "Changing Value");
			SiteSettings.Data.SiteSettingsValue = test;

			AddMessage(who, "Writing");
			SiteSettings.Admin.Write();

			AddMessage(who, "Reading 2");
			SiteSettings.Admin.Read();
			AddMessage(who, "Read");
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
			AddMessage(who, "test file specific");
			AddMessage(who + " Settings|  value", MachSettings.Data.MachSettingsValue);

			AddMessage(who, "Changing Value");
			MachSettings.Data.MachSettingsValue = test;

			AddMessage(who, "Writing");
			MachSettings.Admin.Write();

			AddMessage(who, "Reading 2");
			MachSettings.Admin.Read();
			AddMessage(who, "Read");
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
			AddMessage(who, "test file specific");
			AddMessage(who + " Settings|  value", SuiteSettings.Data.SuiteSettingsValue);

			AddMessage(who, "Changing Value");
			SuiteSettings.Data.SuiteSettingsValue = test;

			AddMessage(who, "Writing");
			SuiteSettings.Admin.Write();

			AddMessage(who, "Reading 2");
			SuiteSettings.Admin.Read();
			AddMessage(who, "Read");
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
			AddMessage(who, "test file specific");
			AddMessage(who + " Settings|  value", AppSettings.Data.AppSettingsValue);

			AddMessage(who, "Changing Value");
			AppSettings.Data.AppSettingsValue = test;

			AddMessage(who, "Writing");
			AppSettings.Admin.Write();

			AddMessage(who, "Reading 2");
			AppSettings.Admin.Read();
			AddMessage(who, "Read");
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
			AddMessage(who, "test file specific");
			AddMessage(who + " Settings|  value", UserSettings.Data.UserSettingsValue);

			AddMessage(who, "Changing Value");
			UserSettings.Data.UserSettingsValue = test;

			AddMessage(who, "Writing");
			UserSettings.Admin.Write();

			AddMessage(who, "Reading 2");
			UserSettings.Admin.Read();
			AddMessage(who, "Read");
			AddMessage(who + " Settings| value", UserSettings.Data.UserSettingsValue);
		}

	#endif


		private void listAllSettingFileInfo()
		{
			AddMessage("");
			AddMessage("Setting File Info", "");

		#if SITE_SETTINGS
			listSettingFileInfo(SiteSettings.Admin  , "site settings");
		#endif

		#if MACH_SETTINGS
			listSettingFileInfo(MachSettings.Admin  , "mach settings");
		#endif

		#if SUITE_SETTINGS
			listSettingFileInfo(SuiteSettings.Admin , "suite settings");
		#endif

		#if APP_SETTINGS
			listSettingFileInfo(AppSettings.Admin   , "app settings");
		#endif

		#if USER_SETTINGS
			listSettingFileInfo(UserSettings.Admin  , "user settings");
		#endif

			AddMessage("");
		}

		private void listSettingFileInfo<TPath, TInfo, TData>(SettingsMgr<TPath, TInfo, TData> s, string who)
			where TPath : PathAndFileBase, new()
			where TInfo : SettingInfoBase<TData>, new()
			where TData : IDataFile, new()
		{
			AddMessage("");
			AddMessage("Setting File Info", who);

			// do I need this?
			// s.Path.ConfigureFilePath();

			listPath(s.Path, who);

			// AddMessage(who + " |     root path", s.Path?.RootFolderPath ?? "is null");
			//
			// for (var i = 0; i < (s.Path.SubFolders ?? new string[0]).Length; i++)
			// {
			// 	AddMessage(who + " |      folder " + i, s.Path?.SubFolders[i] ?? "is null");
			// }
			//
			// AddMessage(who + " |   folder path", s.Path?.SettingFolderPath ?? "is null");
			// AddMessage(who + " | folder exists", s.Path?.SettingFolderPathIsValid ?? false);
			// AddMessage(who + " |     file path", s.Path?.SettingFilePath ?? "is null");
			// AddMessage(who + " |   file exists", s.Path?.SettingFileExists ?? false);
		}

		delegate void setgData(string info, int test);

		private void testSetting<TPath, TInfo, TData>(SettingsMgr<TPath, TInfo, TData> s, string who, int test, setgData sd)
			where TPath : PathAndFileBase, new()
			where TInfo : SettingInfoBase<TData>, new()
			where TData : IDataFile, new()
		{
			AddMessage(MARKER_DN, MARKER_DN + MARKER_DN);

			AddMessage(who, "start");

			try
			{
				AddMessage(who + "| path", s.Path.SettingFilePath);
			}
			catch (Exception e)
			{
				Debug.WriteLine("caught error| " + e.Message);

				if (e.InnerException != null)
				{
					Debug.WriteLine("caught inner error| " + e.InnerException.Message);
				}
			}

			AddMessage(who, "Reading 1");

			s.Read();

			listSettingFileInfo(s, who);

			AddMessage("");

			AddMessage(who, "Read");

			listHeader(s, who);

			sd(who, test);

			s.Write();

			AddMessage(MARKER_UP, MARKER_UP + MARKER_UP);
			AddMessage("");
		}

		private void listHeader<TPath, TInfo, TData>(SettingsMgr<TPath, TInfo, TData> s, string who)
			where TPath : PathAndFileBase, new()
			where TInfo : SettingInfoBase<TData>, new()
			where TData : IDataFile, new()
		{
			AddMessage(who + "| file type", s.Info.FileType);
			AddMessage(who + "| desc", s.Info.Description);
			AddMessage(who + "| data ver", s.Info.DataClassVersion);
			AddMessage(who + "| notes", s.Info.Notes);
			AddMessage(who + "| saved by", s.Info.Header.SavedBy);
		}

		private void listPath<TPath>(TPath p, string who) where TPath : PathAndFileBase, new()
		{
			AddMessage(who + "| RootFolderPath", p.RootFolderPath);
			AddMessage(who + "| SubFolders", p.SubFolders.ToList());
			AddMessage(who + "| FileName", p.FileName);
			AddMessage(who + "| SettingFolderPath", p.SettingFolderPath);
			AddMessage(who + "| SettingFilePath", p.SettingFilePath);
			AddMessage(who + "| SettingFileExists", p.SettingFileExists);
			AddMessage(who + "| RootFolderPathIsValid", p.RootFolderPathIsValid);
			AddMessage(who + "| SettingFolderPathIsValid", p.SettingFolderPathIsValid);
			AddMessage(who + "| IsSettingFile", p.IsSettingFile);
			AddMessage(who + "| HasFilePath", p.HasFilePath);
			AddMessage(who + "| IsReadOnly", p.IsReadOnly);
		}

		private void listTestData2<T>(DataManager<T> d) where T : class, IDataFile, new()
		{
			AddMessage("Data store 2x", "");
			AddMessage("ver", d.Info?.DataClassVersion ?? "is null" );
			AddMessage("desc", d.Info?.Description ?? "is null" );
			AddMessage("note", d.Info?.Notes ?? "is null");

			listPath(d.Path, "");
		}

		private void testData2()
		{
			AddMessage("testData2", "start\n");

			FilePath<FileNameSimple> path21 = new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet2_1.xml");

			DataManager<DataStoreSet2> dm21 = new DataManager<DataStoreSet2>(path21);

			testData2(dm21, nameof(dm21));

			FilePath<FileNameSimple> path22 = new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet2_2.xml");

			DataManager<DataStoreSet2> dm22 = new DataManager<DataStoreSet2>(path22);

			testData2(dm22, nameof(dm22));
		}

		private void testData2<T>(DataManager<T> dm1, string who) where T : DataStoreSet2, new()
		{
			AddMessage(MARKER_DN, MARKER_DN + MARKER_DN);

			AddMessage(who, "start");

			// listTestData2(dm1);

			// AddMessage(who, "create");

			// dm1.Configure(dataFile21);
			dm1.Admin.Write();

			// AddMessage(who, "created");

			listTestData2(dm1);

			AddMessage(who, "SampleDataString1| " + dm1.Data.SampleDataString1);
			AddMessage(who, "SampleDataInt1   | " + dm1.Data.SampleDataInt1);

			AddMessage(who, "changing");
			dm1.Data.SampleDataString1 = "(" + who + ")  to be or not to be, that is the question";
			dm1.Data.SampleDataInt1 = 741;

			AddMessage(who, "writing");
			dm1.Admin.Write();

			// FilePath<FileNameSimple> df23 = 
			// 	new FilePath<FileNameSimple>(
			// 		@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet2_3.xml");
			//
			// AddMessage("Data store "
			// 	+ who, "resetting");
			// dm1 = new DataManager<T>(df23);

			// AddMessage("Data store "
			// 	+ who, "configure");
			// dm1.Configure(dataFile21);

			AddMessage("Data store "
				+ who, "read");
			dm1.Admin.Read();

			AddMessage("Data store "
				+ who, "SampleDataString1| " + dm1.Data.SampleDataString1);
			AddMessage("Data store "
				+ who, "SampleDataInt1   | " + dm1.Data.SampleDataInt1);

			AddMessage(MARKER_UP, MARKER_UP + MARKER_UP);

			AddMessage("\n");
		}

		private void readData()
		{
			AddMessage("\n");
			AddMessage("Data Settings", "Test: read data\n");

			FilePath<FileNameSimple> df11 = new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_1.xml" );

			DataManager<DataStoreSet1> ds1_1 = new DataManager<DataStoreSet1>(df11);

			// AddMessage("Data Set 1_1", "configure");
			// ds1_1.Configure(
			// 	new FilePath<FileNameSimple>(
			// 		@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_1.xml" ));

			AddMessage("Data Set 1_1", "configured");

			ds1_1.Admin.Read();

			listHeader(ds1_1.Admin, "ds1_1");

			// listSettingFileInfo( ds1_1.Admin, "ds1_1");

			AddMessage("\n");
		}

		private void testData()
		{
			AddMessage("Data Settings", "Test\n");

			FilePath<FileNameSimple> fs1 =
				new FilePath<FileNameSimple>(
					@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_1.xml");


			DataManager<DataStoreSet1> ds1_1 = new DataManager<DataStoreSet1>(fs1);
			ds1_1.Admin.Write();

			// AddMessage("Data Set 1_1", "configure");
			// ds1_1.Configure(fs1);

			AddMessage("Data Set 1_1", "configured");

			listHeader(ds1_1.Admin, "ds1_1");

			listSettingFileInfo( ds1_1.Admin, "ds1_1");

			AddMessage("");


			FilePath<FileNameSimple> fs2 = new FilePath<FileNameSimple>(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_2.xml");

			DataManager<DataStoreSet1> ds1_2 = new DataManager<DataStoreSet1>(fs2);
			ds1_2.Admin.Write();


			// AddMessage("Data Set 1_2", "configure");
			// ds1_2.Configure(
			// 	new FilePath<FileNameSimple>(
			// 		@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerv74\DataSet1_2.xml"));
			AddMessage("Data Set 1_2", "configured");

			listHeader(ds1_2.Admin, "ds1_2");

			listSettingFileInfo(ds1_2.Admin, "ds1_2");

			AddMessage("");

			// ds1_1.Admin.Write();

			// ds1_1.Admin.Read();

			testDs1_1(ds1_1);
			testDs1_2(ds1_2);
		}

		private void testDs1_1(DataManager<DataStoreSet1> ds1_1)
		{
			AddMessage("\n");
			AddMessage("Data Settings", "Test: ds1_1");
			AddMessage("IsInitialized", ds1_1.IsInitialized);

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

		private void testDs1_2(DataManager<DataStoreSet1> ds1_2)
		{
			AddMessage("\n");
			AddMessage("Data Settings", "Test: ds1_2");
			AddMessage("IsInitialized", ds1_2.IsInitialized);

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

		private void listDataSet(DataManager<DataStoreSet1> ds, string title)
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