using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SettingManager;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities2;



namespace SettingsManagerV30
{

	public partial class Form1_V30 : Form
	{


		public static string nl = Environment.NewLine;

		public Form1_V30()
		{
			InitializeComponent();
			rtbMessasge.Text = "setting info" + nl;
			rtbMessasge.Select(0,0);

			MessageUtilities.OutLocation = MessageUtilities.OutputLocation.TEXT_BOX;
			MessageUtilities.RichTxtBox = rtbMessasge;

			Upgrade(3);

//			InitAp();

//			InitUs();

			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				ProcessAppSettings(false);
				ProcessUserSettings(false);

			}
			catch (Exception ex)
			{
				logMsgLn2(nl + "EXCEPTION" + nl);
				logMsgLn2(ex.Message + nl);
				logMsgLn2(ex.InnerException + nl);
			}
		}

		private void Upgrade(int which)
		{
			if (which == 1 || which == 3)
			{
				if (UserSettings.Exists)
				{
//					SettingsMgr<UserSettingInfo22> x =
//						UserSettings.Admin;

					ReplaceTestFileUser();

					TestUs();

					UserSettingUpgrade uSup = new UserSettingUpgrade();

					List<SettingBase> Us = uSup.su.SetgClasses;

					uSup.Upgrade();
				}
				else
				{
					UserSettings.Admin.Save();
				}
			}

			if (which == 2 || which == 3)
			{
				if (AppSettings.Exists)
				{
					ReplaceTestFileApp();

					TestAp();

					AppSettingUpgrade aSup = new AppSettingUpgrade();

					List<SettingBase> As = aSup.su.SetgClasses;

					aSup.Upgrade();
				}
				else
				{
					AppSettings.Admin.Save();
				}
			}
		}

		private void ReplaceTestFileUser()
		{
			string testFileName =
				UserSettings.Admin.Info.SettingsPath 
				+ "\\" + "user.setting.xml.v20";

			File.Delete(UserSettings.Admin.Info.SettingsPathAndFile);

			File.Copy(testFileName, 
				UserSettings.Admin.Info.SettingsPathAndFile);

			logMsg2(nl);
			logMsgLn2("SetttingsUser", "test file replaced");
			logMsg2(nl);

			UserSettings.Admin.SetFileExistStatus();
		}

		private void ReplaceTestFileApp()
		{
			string testFileName =
				AppSettings.Admin.Info.SettingsPath 
				+ "\\" + "SettingsManagerV30.setting.xml.v20";

			File.Delete(AppSettings.Admin.Info.SettingsPathAndFile);

			File.Copy(testFileName, 
				AppSettings.Admin.Info.SettingsPathAndFile);

			logMsg2(nl);
			logMsgLn2("SetttingsApp", "test file replaced");
			logMsg2(nl);

			AppSettings.Admin.SetFileExistStatus();
		}



		private void TestUs()
		{
			SettingsMgr<UserSettingInfo22> admin = UserSettings.Admin;

			UserSettingInfo21 x = new UserSettingInfo21();

			logMsgLn2("class", "SettingsUser");
			logMsg2(nl);

		}

		private void TestAp()
		{
			SettingsMgr<AppSettingInfo22> admin = AppSettings.Admin;

			AppSettingInfo21 x = new AppSettingInfo21();

			logMsgLn2("class", "SettingsApp");
			logMsg2(nl);

		}


		private void InitUs()
		{
			logMsgLn2("class", "SetttingsUser");

			logMsgLn2("path"                    , UserSettings.Admin.SettingsPathAndFile);
			logMsgLn2("status"                  , UserSettings.Admin.Status);
			logMsg2(nl);
			logMsgLn2("file versions match"     , UserSettings.Admin.VersionsMatch());
			logMsgLn2("file version (in file)"  , UserSettings.Admin.GetVersionOfFile() ?? "does not exist");
			logMsgLn2("file version (in memory)", UserSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , UserSettings.Admin.SaveDateTime);
			logMsgLn2("assembly version"        , UserSettings.Admin.AssemblyVersion);
			logMsgLn2("file notes"              , UserSettings.Admin.SettingFileNotes);
			logMsg2(nl);
		}

		private void InitAp()
		{
			logMsgLn2("class", "AppSettings");
			logMsgLn2("path"					, AppSettings.Admin.SettingsPathAndFile);
			logMsgLn2("status"					, AppSettings.Admin.Status);
			logMsg2(nl);
			logMsgLn2("file versions match"		, AppSettings.Admin.VersionsMatch());
			logMsgLn2("file version (in file)"	, AppSettings.Admin.GetVersionOfFile() ?? "does not exist");
			logMsgLn2("file version (in memory)", AppSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , AppSettings.Admin.SaveDateTime);
			logMsgLn2("assembly version"        , AppSettings.Admin.AssemblyVersion);
			logMsgLn2("file notes"              , AppSettings.Admin.SettingFileNotes);
			logMsg2(nl);
		}

		private const int V = 40;

		private void ProcessUserSettings(bool modify)
		{
			logMsgLn2(nl);
			logMsgLn2("user path", UserSettings.Admin.SettingsPathAndFile);
			logMsgLn2("user before");

			if (UserSettings.Exists)
			{
				UserSettings.Admin.Read();
				DisplayUserSettingData();

				if (modify)
				{
					ModifyAndSaveUserSettings();

					logMsgLn2(nl + "user after");
					DisplayUserSettingData();
				}
			}
			else
			{
				logMsgLn2("user setting file", "does not exist");
			}
		}

		private void ResetUserSettings()
		{
			UserSettings.Admin.Reset();
			UserSettings.Admin.Save();

			logMsgLn2(nl + "user reset");
			DisplayUserSettingData();
		}


		private void ModifyAndSaveUserSettings()
		{
			UserSettings.Data.GeneralValues.TestB = true;
			UserSettings.Data.GeneralValues.TestD = V + 0.2;
			UserSettings.Data.GeneralValues.TestS = "using generic setting file " + V;
			UserSettings.Data.GeneralValues.TestI = V;
			UserSettings.Data.GeneralValues.TestIs[1] = V;
			UserSettings.Data.GeneralValues.TestSs[1] = "generic " + V;
			UserSettings.Data.UnCategorizedValue = V;
			UserSettings.Data.MainWindow.Height = V * 50;
			UserSettings.Data.MainWindow.Width = V * 100;
			UserSettings.Data.TestDictionary3["one"] = new TestStruct(V * 10 + 4, V * 10 + 5, V * 10 + 6);
			UserSettings.Data.TestDictionary3["two"] = new TestStruct(V * 10 + 1, V * 10 + 2, V * 10 + 3);
			UserSettings.Data.TestDictionary3["three"] = new TestStruct(V * 10 + 7, V * 10 + 8, V * 10 + 9);

			UserSettings.Admin.Save();
		}

		private void DisplayUserSettingData()
		{

			logMsgLn2("system version"          , UserSettings.Admin.Info.Header.SystemVersion);
			logMsgLn2("status"                  , UserSettings.Admin.Status);
			logMsgLn2("file name"               , UserSettings.Admin.SettingsPathAndFile);
			logMsgLn2("file version (in file)"  , UserSettings.Admin.GetVersionOfFile() ?? "does not exist");
			logMsgLn2("class version (in memory)", UserSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , UserSettings.Admin.SaveDateTime);
			logMsgLn2("assembly version"        , UserSettings.Admin.AssemblyVersion);
			logMsgLn2("file notes"              , UserSettings.Admin.SettingFileNotes);

			
			logMsgLn2("test dict one/IntA"      , UserSettings.Info.Data.TestDictionary3["one"].IntA.ToString());
			logMsgLn2("test dict one/IntB"      , UserSettings.Info.Data.TestDictionary3["one"].IntB.ToString());
			logMsgLn2("test dict one/IntC"      , UserSettings.Info.Data.TestDictionary3["one"].IntC.ToString());


			logMsgLn2("test int"                , UserSettings.Info.Data.GeneralValues.TestI.ToString());
			logMsgLn2("test bool"               , UserSettings.Info.Data.GeneralValues.TestB.ToString());
			logMsgLn2("test double"             , UserSettings.Info.Data.GeneralValues.TestD.ToString());
			logMsgLn2("test string"             , UserSettings.Info.Data.GeneralValues.TestS);
			logMsgLn2("test int[0]"             , UserSettings.Info.Data.GeneralValues.TestIs[0].ToString());
			logMsgLn2("test int[1]"             , UserSettings.Info.Data.GeneralValues.TestIs[1].ToString());
			logMsgLn2("test str[0]"             , UserSettings.Info.Data.GeneralValues.TestSs[0]);
			logMsgLn2("test str[1]"             , UserSettings.Info.Data.GeneralValues.TestSs[1]);
			logMsgLn2("test str[2]"             , UserSettings.Info.Data.GeneralValues.TestSs[2]);
			logMsgLn2("win height"              , UserSettings.Info.Data.MainWindow.Height.ToString());
			logMsgLn2("win width"               , UserSettings.Info.Data.MainWindow.Width.ToString());
			logMsgLn2("uncat value"             , UserSettings.Info.Data.UnCategorizedValue.ToString());

			logMsgLn2();
		}


		private void ProcessAppSettings(bool modify)
		{
			logMsg2(nl);
			logMsgLn2("app path",AppSettings.Admin.SettingsPathAndFile);
			logMsgLn2("app before");

			if (AppSettings.Exists)
			{
				AppSettings.Admin.Read();

				DisplayAppSettingData();

				if (modify)
				{
					ModifyAndSaveAppSettings();

					logMsgLn2(nl + "app after");
					DisplayAppSettingData();
				}
			}
			else
			{
				logMsgLn2("app setting file", "does not exist");
			}

//			ResetAndSaveAppSettings();
		}

		private void DisplayAppSettingData()
		{
			logMsgLn2("system version"          , AppSettings.Admin.Info.Header.SystemVersion);
			logMsgLn2("status"                  , AppSettings.Admin.Status);
			logMsgLn2("file name"               , AppSettings.Admin.SettingsPathAndFile);
			logMsgLn2("file version (in file)"  , AppSettings.Admin.GetVersionOfFile() ?? "does not exist");
			logMsgLn2("class version (in memory)", AppSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , AppSettings.Admin.SaveDateTime);
			logMsgLn2("assembly version"        , AppSettings.Admin.AssemblyVersion);
			logMsgLn2("file notes"              , AppSettings.Admin.SettingFileNotes);
			logMsg2(nl);
			logMsgLn2("test string"             , AppSettings.Data.AppS);
			logMsgLn2("test bool"               , AppSettings.Data.AppB.ToString());
			logMsgLn2("test double"             , AppSettings.Data.AppD.ToString());
			logMsgLn2("test int"                , AppSettings.Data.AppI.ToString());
			logMsgLn2("test int[0]"             , AppSettings.Data.AppIs[0].ToString());
			logMsgLn2("test AppI20"             , AppSettings.Data.AppI20.ToString());
			logMsg2(nl);
		}

		private void ModifyAndSaveAppSettings()
		{
			AppSettings.Data.AppS = "generic app data " + V;
			AppSettings.Data.AppB = false;
			AppSettings.Data.AppD = V + 0.1;
			AppSettings.Data.AppI = V;
			AppSettings.Data.AppIs[0] = V;

			AppSettings.Admin.Save();
		}

		private void ResetAndSaveAppSettings()
		{
			AppSettings.Admin.Reset();
			AppSettings.Admin.Save();

			logMsgLn2(nl + "app reset");
			DisplayAppSettingData();
		}

	}
}
