using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using SettingsManager.SampleData;
using SettingsManagerV70.SampleData;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

namespace SettingsManager.Windows
{

	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private const string DATA_NAME2 = "(SD2)";

		public static SampleDataManager2 sd2x { get; set; } = new SampleDataManager2(DATA_NAME2);
		private SampleDataManager2 sd2;

		// private StorageManager<SampleDataData3> ds1;
		// private StorageManager<SampleDataData3> ds2;

		private static string messageRight;
		private static string messageLeft;

		private static MainWindow instance;


		// private UserSetg UserSettings = new UserSetg();
		// private AppSetg AppSettings = new AppSetg();
		//

		public MainWindow()
		{

			InitializeComponent();

			instance = this;

			SD2 = new SampleDataManager2(DATA_NAME2);
		}

		public SampleDataManager2 SD2
		{
			get => sd2;
			private set
			{
				sd2 = value;
				OnPropertyChange();
			}
		}


		public static MainWindow Instance
		{
			get
			{
				if (instance == null)
				{
					throw new NullReferenceException();
				}

				return instance;
			}
//			private set => instance = value;
		}

		public string MessageLeft
		{
			get => messageLeft;
			set
			{
				messageLeft += value;

				OnPropertyChange();
			}
		}

		public string MessageRight
		{
			get => messageRight;
			set
			{
				messageRight += value;

				OnPropertyChange();
			}
		}

		private void mainWin_Loaded(object sender, RoutedEventArgs e)
		{
			MsgLeftLine("main window", "loaded");

			// SettingsTest();

			// testData();

			// testData3();

			testDataFile();
		}

		// public SampleDataData3 Drx
		// {
		// 	get
		// 	{
		// 		if (ds1 == null) return null;
		//
		// 		return ds1.Data;
		// 	}
		// }
		//
		// public ObservableCollection<SampleItem2> Root
		// {
		// 	get
		// 	{
		// 		if (ds1 == null) return null;
		//
		// 		return ds1.Data.Root;
		// 	}
		// }

		private void testDataFile()
		{
			MsgLeftLine("main window", "testDataFile");

			Debug.WriteLine("@mainwindow| testDataFile");


			BaseDataFile<SampleDataData3> df1 = new BaseDataFile<SampleDataData3>();
			BaseDataFile<SampleDataData3> df2 = new BaseDataFile<SampleDataData3>();

			df1.Configure(
				@"B:\Programming\VisualStudioProjects\SettingsManager\SettingsManagerV70\SettingsManagerData",
				@"SampleData1.xml"
				);

			df2.Configure(
				@"B:\Programming\VisualStudioProjects\SettingsManager\SettingsManagerV70\SettingsManagerData",
				@"SampleData2.xml"
				);

			

			MsgLeftLine("main window", "configured");
			Debug.WriteLine("@mainwindow| configured");

			if (df1.Initialized)
			{
				df1.Admin.Read();
				// df1.Admin.Write();

				MsgLeftLine("main window", "1 read");
				Debug.WriteLine("@mainwindow| 1 read");

				MsgLeftLine("main window", "description| " + df1.Info.Description);
				Debug.WriteLine("@mainwindow| description| " + df1.Info.Description);

				MsgLeftLine("main window", "filename| " + df1.Path.FileName);
				Debug.WriteLine("@mainwindow| filename| " + df1.Path.FileName);

				MsgLeftLine("main window", "done\n");
				Debug.WriteLine("@mainwindow| done\n");

			}

			if (df2.Initialized)
			{
				df2.Admin.Read();
				// df2.Admin.Write();

				MsgLeftLine("main window", "2 read");
				Debug.WriteLine("@mainwindow| 2 read");

				MsgLeftLine("main window", "description| " + df2.Info.Description);
				Debug.WriteLine("@mainwindow| description| " + df2.Info.Description);

				MsgLeftLine("main window", "filename| " + df2.Path.FileName);
				Debug.WriteLine("@mainwindow| filename| " + df2.Path.FileName);

				MsgLeftLine("main window", "done\n");
				Debug.WriteLine("@mainwindow| done\n");

			}


		}



// 		private void testData3()
// 		{
// 			MsgLeftLine("main window", "testdata3");
//
// 			Debug.WriteLine("@mainwindow| testdate3");
//
// 			ds1 = new StorageManager<SampleDataData3>();
// 			ds2 = new StorageManager<SampleDataData3>();
// 			
// 			ds1.Configure(
// 				@"B:\Programming\VisualStudioProjects\SettingsManager\SettingsManagerV70\SettingsManagerData",
// 				@"SampleData1.xml"
// 				);
//
// 			ds2.Configure(
// 				@"B:\Programming\VisualStudioProjects\SettingsManager\SettingsManagerV70\SettingsManagerData",
// 				@"SampleData2.xml"
// 				);
//
// 			MsgLeftLine("main window", "configured");
// 			Debug.WriteLine("@mainwindow| configured");
//
// 			bool test = ds1.Initialized;
//
// 			bool result = ds1.Read();
//
// 			MsgLeftLine("main window", "read");
// 			Debug.WriteLine("@mainwindow| read");
//
// 			OnPropertyChange("Root");
//
// 			MsgLeftLine("main window", "property change notification");
// 			Debug.WriteLine("@mainwindow| motification");
//
// //			ds1.Data.DataRoot = sd2x.Root;
// //
// //			test = ds1.Write();
//
// 		}




		// // this will create a starting data set /  collection
		// private void testData()
		// {
		// 	SampleDataStore.Admin.Read();
		//
		// 	SD2.Root = sd2x.Root;
		//
		// 	MsgLeftLine("");
		// 	MsgLeftLine("SampleDataStore| PathAndFile", SampleDataStore.Path.SettingFilePath);
		//
		// 	SD2.Root[0].Name = "this is a test and only a test";
		//
		// 	SampleDataStore.Data.DataRoot = SD2.Root;
		// 	SampleDataStore.Admin.Write();
		//
		// }

		private void SettingsTest()
		{
			status();

			int all = 1;

			int user = 2;

			userTest(user == 0 ? all : user );
			appTest();
			suiteTest();
			machTest();
			siteTest();

//			testReset();
		}


		private void status()
		{
			MsgLeftLine("usersettings.adm is null?", (UserSettings.Admin == null).ToString());
			MsgLeftLine("appsettings.adm is null?", (AppSettings.Admin == null).ToString());
			MsgLeftLine("suitesettings.adm is null?", (SuiteSettings.Admin == null).ToString());
			MsgLeftLine("machsettings.adm is null?", (MachSettings.Admin == null).ToString());
			MsgLeftLine("sitesettings.adm is null?", (SiteSettings.Admin == null).ToString());
			MsgLeftLine("sitesettings.info is null?", (SiteSettings.Info == null).ToString());
			MsgLeftLine("sitesettings.data is null?", (SiteSettings.Data == null).ToString());
			MsgLeftLine("");
		}

		private void testReset()
		{
			MsgLeftLine("");

			MsgLeftLine("test reset");

			testUserReset();
			testAppReset();
			testMachReset();
			testSiteReset();
		}

		private void testSetting<Tpath, Tinfo, Tdata>(string title, SettingsMgr<Tpath, Tinfo, Tdata> setg)
			where Tpath : PathAndFileBase, new ()
			where Tinfo : SettingInfoBase<Tdata>, new ()
			where Tdata : new ()
		{
			MsgLeftLine(title + "| generic info description", setg.Info.Description);
			MsgLeftLine(title + "| generic path filename", setg.Path?.FileName ?? "no file name");
			MsgLeftLine(title + "| generic path SettingFilePath", setg.Path?.SettingFilePath ?? "no path and file");

		}


	#region user tests

		private void userTest(int which)
		{
			MsgLeftLine("");
			UserSettings.Admin.Read();

			switch (which)
			{
			case 1:
				{
					userTest1();
					break;
				}
			case 2:
				{
					userTest2();
					break;
				}
			}
		}

		private void userTest1()
		{
			testSetting("user", UserSettings.Admin);
			MsgLeftLine("");

			MsgLeftLine("After initial read");
			testUser();
			modifyUser();

			MsgLeftLine("");
			MsgLeftLine("After second read & upgrade");
			testUser();
		}

		private void userTest2()
		{
			MsgLeftLine("path tests");
			MsgLeftLine("user exists", UserSettings.Path.Exists.ToString());

			if (UserSettings.Path.Exists)
			{
				MsgLeftLine("user path: root path", UserSettings.Path.RootFolderPath);
				MsgLeftLine("user path: file name", UserSettings.Path.FileName);
				MsgLeftLine("user path: path and file", UserSettings.Path.SettingFilePath);
				MsgLeftLine("user path: has path and file", UserSettings.Path.HasFilePath.ToString());
				MsgLeftLine("user path: sub-folders");
				foreach (string pathSubFolder in UserSettings.Path.SubFolders)
				{
					MsgLeftLine("user path: sub-folder", pathSubFolder);
				}
			}

		}


		private void testUser()
		{
			MsgLeftLine("");

			MsgLeftLine("user test", UserSettings.Data.UserSettingsValue.ToString());
			MsgLeftLine("user test", UserSettings.Info.DataClassVersion);
			MsgLeftLine("user test", UserSettings.Info.Description);
		}

		private void modifyUser()
		{
			UserSettings.Data.UserSettingsValue = 700;
			UserSettings.Admin.Write();

			MsgLeftLine("");
			MsgLeftLine("After modifying");
			testUser();

			UserSettings.Admin.Reset();

			MsgLeftLine("");
			MsgLeftLine("After reset");

			testUser();

			UserSettings.Admin.Read();
//			UserSettings.Admin.Upgrade();
		}

		private void testUserReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset user");
			MsgLeftLine("");

			UserSettings.Admin.Reset();
			UserSettings.Admin.Write();

			testUser();
		}

	#endregion


	#region app tests

		private void appTest()
		{
			MsgLeftLine("");

			AppSettings.Admin.Read();
			testSetting("app", AppSettings.Admin);
			MsgLeftLine("");
			MsgLeftLine("After initial read");
			testApp();
			modifyApp();

			MsgLeftLine("");
			MsgLeftLine("After second read & upgrade");
			testApp();
		}

		private void modifyApp()
		{
			AppSettings.Admin.Write();
			AppSettings.Admin.Reset();

			testApp();


			AppSettings.Admin.Read();
//			AppSettings.Admin.Upgrade();
		}

		private void testApp()
		{
			MsgLeftLine("");
			MsgLeftLine("app test", AppSettings.Info.DataClassVersion);
			MsgLeftLine("app test", AppSettings.Info.Description);
		}

		private void testAppReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset app");
			MsgLeftLine("");

			AppSettings.Admin.Reset();
			AppSettings.Admin.Write();

			testApp();
		}

	#endregion

	#region suite tests

		private void suiteTest()
		{
			MsgLeftLine("");

			SuiteSettings.Admin.Read();
			testSetting("suite", SuiteSettings.Admin);
			MsgLeftLine("");
			MsgLeftLine("After initial read");
			testSuite();
			modifySuite();

			MsgLeftLine("");
			MsgLeftLine("After second read & upgrade");
			testApp();
		}

		private void modifySuite()
		{
			SuiteSettings.Admin.Write();
			SuiteSettings.Admin.Reset();

			testSuite();


			SuiteSettings.Admin.Read();
//			SuiteSettings.Admin.Upgrade();
		}

		private void testSuite()
		{
			MsgLeftLine("");
			MsgLeftLine("suite test", SuiteSettings.Info.DataClassVersion);
			MsgLeftLine("suite test", SuiteSettings.Info.Description);
		}

	#endregion



	#region machine tests

		private void machTest()
		{
			MsgLeftLine("");

			MachSettings.Admin.Read();
			testSetting("mach", MachSettings.Admin);
			MsgLeftLine("");
			testMachine();
			modifyMach();
			testMachine();
		}

		private void modifyMach()
		{
			MachSettings.Data.MachSettingsValue = 702;
			MachSettings.Admin.Write();
			MachSettings.Admin.Reset();

			testMachine();

			MachSettings.Admin.Read();
//			MachSettings.Admin.Upgrade();
		}

		private void testMachine()
		{
			MsgLeftLine("");
			MsgLeftLine("mach test", MachSettings.Data.MachSettingsValue.ToString());
			MsgLeftLine("mach test", MachSettings.Info?.DataClassVersion ?? "not defined");
			MsgLeftLine("mach test", MachSettings.Info?.Description ?? "not defined");

		}

		private void testMachReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset mach");
			MsgLeftLine("");

			MachSettings.Admin.Reset();
			MachSettings.Admin.Write();

			testMachine();
		}

	#endregion


	#region site tests

		private void siteTest()
		{
			MsgLeftLine("");

			SiteSettings.Path.RootFolderPath = SuiteSettings.SiteRootPath;

			SiteSettings.Admin.Read();
			testSetting("site", SiteSettings.Admin);
			MsgLeftLine("");
			testSite();
			modifySite();
			testSite();
		}

		private void modifySite()
		{
			SiteSettings.Data.SiteSettingsValue = 703;
			SiteSettings.Admin.Write();
			SiteSettings.Admin.Reset();

			testSite();

			SiteSettings.Admin.Read();
//			SiteSettings.Admin.Upgrade();
		}

		private void testSite()
		{
			MsgLeftLine("");

			MsgLeftLine("site test", SiteSettings.Data.SiteSettingsValue.ToString());
			MsgLeftLine("site test", SiteSettings.Info?.DataClassVersion ?? "not defined");
			MsgLeftLine("site test", SiteSettings.Info?.Description ?? "not defined");

		}

		private void testSiteReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset site");
			MsgLeftLine("");

			SiteSettings.Admin.Reset();
			SiteSettings.Admin.Write();

			testSite();
		}

	#endregion

	#region messages

		public void MsgClear()
		{
			messageLeft = "";
			MessageLeft = "";
		}

		public void MsgLeft(string msg1, string msg2 = "")
		{
			MessageLeft =  FormatMsg(msg1, msg2);
		}
		
		public void MsgLeftLine(string msg1, string msg2 = "")
		{
			MessageLeft = FormatMsg(msg1, msg2, true);
		}

		public void MsgRight(string msg1, string msg2 = "")
		{
			MessageRight = FormatMsg(msg1, msg2);
		}

		public void MsgRightLine(string msg1, string msg2 = "")
		{
			MessageRight = FormatMsg(msg1, msg2, true);
		}

		private string FormatMsg(string msg1, string msg2 = null, bool addReturn = false)
		{
			if (!msg2.IsVoid())
			{
				msg1 += "| ";
			}

			string result = fmtMsg(msg1, msg2);

			if (addReturn)
			{
				result += nl;
			}

			return result;

		}

	#endregion


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			MsgRightLine("Debug Button Pressed");
		}
		
		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			SarMgr.Instance.Write();
		}

		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			MsgClear();

			UserSettings.ResetData();
//			AppSettings.ResetData();
//			AppSettings.MachSettings.ResetData();
//			AppSettings.SiteSettings.ResetData();

			mainWin_Loaded(null, null);
		}
	}
}
