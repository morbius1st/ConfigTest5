using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

using SettingsManager;

namespace SettingsManagerV40
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
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
			AppSettings.Admin.Initialize();
			AppSettings.Admin.Read();
			AppSettings.Admin.Initialize();
			UserSettings.Admin.Read();

			testUser();

			testApp();

			MsgLeftLine("");
			MsgLeftLine("app setting| auto-read mach", AppSettings.Info.AutoReadMachData.ToString());
			MsgLeftLine("app setting| auto-read site", AppSettings.Info.AutoReadSiteData.ToString());

			testMachine();

			testSite();

			testReset();

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

			AppSettings.SiteSettings.Admin.Reset();
			AppSettings.SiteSettings.Admin.Write();

			testSite();
		}

		private void testMachReset()
		{
			MsgLeftLine("");
			MsgLeftLine("reset mach");
			MsgLeftLine("");

			AppSettings.MachSettings.Admin.Reset();
			AppSettings.MachSettings.Admin.Write();

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

		private void testSite()
		{
			MsgLeftLine("");
			MsgLeftLine("app setting| site file exists", AppSettings.SiteInfo?.Exists.ToString() ?? "siteinfo is null");

			if (!AppSettings.Info.AutoReadSiteData)
			{
				MsgLeftLine("");
				MsgLeftLine("Reading site settings");

				AppSettings.ReadSiteSettings();
			}

			MsgLeftLine("site read test| data test",
				AppSettings.SiteData?.SiteSettingsValue.ToString() ?? "not defined");
			MsgLeftLine("site read test| info test", AppSettings.SiteInfo?.ClassVersion ?? "not defined");
			MsgLeftLine("site read test| info test", AppSettings.SiteInfo?.Description ?? "not defined");

		}

		private void testMachine()
		{
			MsgLeftLine("");
			MsgLeftLine("app setting| mach file exists", AppSettings.MachInfo?.Exists.ToString() ?? "machinfo is null");

			if (!AppSettings.Info.AutoReadMachData)
			{
				MsgLeftLine("");
				MsgLeftLine("Reading mach settings");

				AppSettings.ReadMachineSettings();
			}

			MsgLeftLine("mach read test| data test",
				AppSettings.MachData?.MachSettingsValue.ToString() ?? "not defined");
			MsgLeftLine("mach read test| info test", AppSettings.MachInfo?.ClassVersion ?? "not defined");
			MsgLeftLine("mach read test| info test", AppSettings.MachInfo?.Description ?? "not defined");

		}


		private void testUser()
		{
			MsgLeftLine("");

			MsgLeftLine("user read test| data test", UserSettings.Data.UserSettingsValue.ToString());
			MsgLeftLine("user read test| info test", UserSettings.Info.ClassVersion);
			MsgLeftLine("user read test| info test", UserSettings.Info.Description);
		}

		private void testApp()
		{
			MsgLeftLine("");
			MsgLeftLine("app read test| data test", AppSettings.Data.AppSettingsValue.ToString());
			MsgLeftLine("app read test| info test", AppSettings.Info.ClassVersion);
			MsgLeftLine("app read test| info test", AppSettings.Info.Description);
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

			AppSettings.ResetData();
			UserSettings.ResetData();
			AppSettings.MachSettings.ResetData();
			AppSettings.SiteSettings.ResetData();

			mainWin_Loaded(null, null);
		}
	}
}
