using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
		private StorageManager<SampleDataData3> ds;

		private static string messageRight;
		private static string messageLeft;

		private static MainWindow instance;

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

			SettingsTest();
//
//			testData();

//			testData3();
		}

		public SampleDataData3 Drx
		{
			get
			{
				if (ds == null) return null;

				return ds.Data;
			}
		}

		public ObservableCollection<SampleItem2> Root
		{
			get
			{
				if (ds == null) return null;

				return ds.Data.Root;
			}
		}

		private void testData3()
		{
			MsgLeftLine("main window", "testdata2");

			Debug.WriteLine("@mainwindow| testdate2");

			ds = new StorageManager<SampleDataData3>();

			ds.Configure(
				@"B:\Programming\VisualStudioProjects\SettingsManager\SettingsManagerV70\SettingsManagerData",
				@"SampleData2.xml"
				);

			MsgLeftLine("main window", "configured");
			Debug.WriteLine("@mainwindow| configured");

			bool test = ds.Initialized;

			bool result = ds.Read();

			MsgLeftLine("main window", "read");
			Debug.WriteLine("@mainwindow| read");

			OnPropertyChange("Root");

			MsgLeftLine("main window", "property change notification");
			Debug.WriteLine("@mainwindow| motification");

//			ds.Data.DataRoot = sd2x.Root;
//
//			test = ds.Write();


		}




		// this will create a starting data set /  collection
		private void testData()
		{
			SampleDataStore.Admin.Read();

			SD2.Root = sd2x.Root;

			MsgLeftLine("");
			MsgLeftLine("SampleDataStore| PathAndFile", SampleDataStore.Path.SettingPathAndFile);

			SD2.Root[0].Name = "this is a test and only a test";

			SampleDataStore.Data.DataRoot = SD2.Root;
			SampleDataStore.Admin.Write();

		}

		private void SettingsTest()
		{
			status();

			userTest();
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

		private void siteTest()
		{
			MsgLeftLine("");

			SiteSettings.Path.RootPath = SuiteSettings.SiteRootPath;
//			SiteSettings.Path.RootPath = AppSettings.SiteRootPath;

			SiteSettings.Admin.Read();
			testSetting("site", SiteSettings.Admin);
			MsgLeftLine("");
			testSite();
			modifySite();
			testSite();
		}

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

		private void userTest()
		{
			MsgLeftLine("");

			UserSettings.Admin.Read();
			testSetting("user", UserSettings.Admin);
			MsgLeftLine("");

			MsgLeftLine("After initial read");
			testUser();
			modifyUser();

			MsgLeftLine("");
			MsgLeftLine("After second read & upgrade");
			testUser();
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
			MsgLeftLine(title + "| generic path SettingPathAndFile", setg.Path?.SettingPathAndFile ?? "no path and file");

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
			UserSettings.Admin.Upgrade();
		}

		private void modifySite()
		{
			SiteSettings.Data.SiteSettingsValue = 703;
			SiteSettings.Admin.Write();
			SiteSettings.Admin.Reset();

			testSite();

			SiteSettings.Admin.Read();
			SiteSettings.Admin.Upgrade();
		}

		private void testSite()
		{
			MsgLeftLine("");

			MsgLeftLine("site test", SiteSettings.Data.SiteSettingsValue.ToString());
			MsgLeftLine("site test", SiteSettings.Info?.DataClassVersion ?? "not defined");
			MsgLeftLine("site test", SiteSettings.Info?.Description ?? "not defined");

		}

		private void modifyMach()
		{
			MachSettings.Data.MachSettingsValue = 702;
			MachSettings.Admin.Write();
			MachSettings.Admin.Reset();

			testMachine();

			MachSettings.Admin.Read();
			MachSettings.Admin.Upgrade();
		}

		private void testMachine()
		{
			MsgLeftLine("");
			MsgLeftLine("mach test", MachSettings.Data.MachSettingsValue.ToString());
			MsgLeftLine("mach test", MachSettings.Info?.DataClassVersion ?? "not defined");
			MsgLeftLine("mach test", MachSettings.Info?.Description ?? "not defined");

		}
		
		private void modifyApp()
		{
			AppSettings.Admin.Write();
			AppSettings.Admin.Reset();

			testApp();


			AppSettings.Admin.Read();
			AppSettings.Admin.Upgrade();
		}

		private void testApp()
		{
			MsgLeftLine("");
			MsgLeftLine("app test", AppSettings.Info.DataClassVersion);
			MsgLeftLine("app test", AppSettings.Info.Description);
		}

		private void modifySuite()
		{
			SuiteSettings.Admin.Write();
			SuiteSettings.Admin.Reset();

			testSuite();


			SuiteSettings.Admin.Read();
			SuiteSettings.Admin.Upgrade();
		}

		private void testSuite()
		{
			MsgLeftLine("");
			MsgLeftLine("suite test", SuiteSettings.Info.DataClassVersion);
			MsgLeftLine("suite test", SuiteSettings.Info.Description);
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

		private void testMachReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset mach");
			MsgLeftLine("");

			MachSettings.Admin.Reset();
			MachSettings.Admin.Write();

			testMachine();
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

		private void testUserReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset user");
			MsgLeftLine("");

			UserSettings.Admin.Reset();
			UserSettings.Admin.Write();

			testUser();
		}

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
