using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SettingsManager;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

namespace SettingsManagerV50.Windows
{

	public partial class MainWindow : Window, INotifyPropertyChanged
	{

		private static string messageRight;
		private static string messageLeft;

		private static MainWindow instance;

		public MainWindow()
		{
			InitializeComponent();

			instance = this;
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

			UserSettings2.Admin.Read();
			MsgLeftLine("After initial read");
			testUser2();
			modifyUser2();

			MsgLeftLine("");
			MsgLeftLine("After second read & upgrade");
			testUser2();

//			AppSettings.Admin.Read();
//			UserSettings.Admin.Read();
//
//			testApp();
//			modifyApp();
//			testApp();
//
//			testUser();
//			modifyUser();
//			testUser();
//
//			MsgLeftLine("");
//
//			MachSettings.Admin.Read();
//			SiteSettings.Admin.Read();
//
//			testMachine();
//			modifyMach();
//			testMachine();
//
//			testSite();
//			modifySite();
//			testSite();
//
//			MsgLeftLine("");
//
//			testReset();

		}


		private void testUser2()
		{
			MsgLeftLine("");

			MsgLeftLine("user read test| data test", UserSettings2.Data.UserSettingsValue.ToString());
			MsgLeftLine("user read test| info test", UserSettings2.Info.DataClassVersion);
			MsgLeftLine("user read test| info test", UserSettings2.Info.Description);
		}

		private void modifyUser2()
		{
			UserSettings2.Data.UserSettingsValue = 500;
			UserSettings2.Admin.Write();

			MsgLeftLine("");
			MsgLeftLine("After modifying");
			testUser2();

			UserSettings2.Admin.Reset();

			MsgLeftLine("");
			MsgLeftLine("After reset");
			testUser2();

			UserSettings2.Admin.Read();
			UserSettings2.Admin.Upgrade();
		}



		private void modifySite()
		{
			SiteSettings.Data.SiteSettingsValue = 503;
			SiteSettings.Admin.Write();
			SiteSettings.Admin.Reset();

			testSite();

			SiteSettings.Admin.Read();
			SiteSettings.Admin.Upgrade();
		}

		private void testSite()
		{
			MsgLeftLine("");

			MsgLeftLine("site read test| info test", SiteSettings.Data.SiteSettingsValue.ToString());
			MsgLeftLine("site read test| info test", SiteSettings.Info?.DataClassVersion ?? "not defined");
			MsgLeftLine("site read test| info test", SiteSettings.Info?.Description ?? "not defined");

		}

		private void modifyMach()
		{
			MachSettings.Data.MachSettingsValue = 502;
			MachSettings.Admin.Write();
			MachSettings.Admin.Reset();

			testMachine();

			MachSettings.Admin.Read();
			MachSettings.Admin.Upgrade();
		}

		private void testMachine()
		{
			MsgLeftLine("");
			MsgLeftLine("mach read test| info test", MachSettings.Data.MachSettingsValue.ToString());
			MsgLeftLine("mach read test| info test", MachSettings.Info?.DataClassVersion ?? "not defined");
			MsgLeftLine("mach read test| info test", MachSettings.Info?.Description ?? "not defined");

		}

		private void modifyUser()
		{
			UserSettings.Data.UserSettingsValue = 500;
			UserSettings.Admin.Write();
			UserSettings.Admin.Reset();

			testUser();
			
			UserSettings.Admin.Read();
			UserSettings.Admin.Upgrade();
		}


		private void testUser()
		{
			MsgLeftLine("");

			MsgLeftLine("user read test| data test", UserSettings.Data.UserSettingsValue.ToString());
			MsgLeftLine("user read test| info test", UserSettings.Info.DataClassVersion);
			MsgLeftLine("user read test| info test", UserSettings.Info.Description);
		}

		private void modifyApp()
		{
			AppSettings.Data.AppSettingsValue = 510;
			AppSettings.Admin.Write();
			AppSettings.Admin.Reset();

			testApp();


			AppSettings.Admin.Read();
			AppSettings.Admin.Upgrade();
		}

		private void testApp()
		{
			MsgLeftLine("");
			MsgLeftLine("app read test| data test", AppSettings.Data.AppSettingsValue.ToString());
			MsgLeftLine("app read test| info test", AppSettings.Info.DataClassVersion);
			MsgLeftLine("app read test| info test", AppSettings.Info.Description);
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
