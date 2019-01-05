﻿using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
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
//			MessageUtilities.OutLocation = MessageUtilities.OutputLocation.DEBUG;
			MessageUtilities.RichTxtBox = rtbMessasge;

		}

		private void button2_Click(object sender,
			EventArgs e)
		{
//			Files();

			Status(2);

//			Upgrade(3);
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

		public void Files()
		{
			logMsg2(nl);
			logMsgLn2("AppPathAndFile", "values");

			logMsgLn2("SettingPathAndFile", PathAndFile.App.SettingPathAndFile);
			logMsgLn2("exists", PathAndFile.App.Exists);
			logMsgLn2("FileName", PathAndFile.App.FileName);
			logMsgLn2("RootPath", PathAndFile.App.RootPath);
			logMsgLn2("SettingPath", PathAndFile.App.SettingPath);
			logMsgLn2("ClassVersionFromFile", PathAndFile.App.ClassVersionFromFile);

			logMsg2(nl);
			logMsgLn2("AppPathAndFile", "values");

			logMsgLn2("SettingPathAndFile", PathAndFile.User.SettingPathAndFile);
			logMsgLn2("exists", PathAndFile.User.Exists);
			logMsgLn2("FileName", PathAndFile.User.FileName);
			logMsgLn2("RootPath", PathAndFile.User.RootPath);
			logMsgLn2("SettingPath", PathAndFile.User.SettingPath);
			logMsgLn2("ClassVersionFromFile", PathAndFile.User.ClassVersionFromFile);

		}


		public void Status(int which)
		{
			logMsgLn2("at \"status\"");

			// option 1 = delete and provide new
			if (which == 1)
			{
				logMsgLn2("@1A");
				logMsg2(nl);


				if (PathAndFile.App.Exists)
				{
					logMsgLn2();
					logMsgLn2("SetttingsApp", "delete");
					DeleteAppSettingFile();
				}

				if (PathAndFile.User.Exists)
				{
					logMsgLn2();
					logMsgLn2("SetttingsUser", "delete");
					DeleteUserSettingFile();
				}

				logMsgLn2();
				logMsgLn2("SetttingsApp", "create");
				AppSettings.Admin.Create();
				logMsgLn2("create status", AppSettings.Admin.Status);

				logMsgLn2("SetttingsApp", "saving");
				AppSettings.Admin.Save();
				logMsgLn2("save status", AppSettings.Admin.Status);

				logMsgLn2();
				logMsgLn2("SetttingsUser", "create");
				UserSettings.Admin.Create();
				logMsgLn2("create status", UserSettings.Admin.Status);

				logMsgLn2();
				logMsgLn2("SetttingsUser", "saving");
				UserSettings.Admin.Save();
				logMsgLn2("save status", UserSettings.Admin.Status);


			} 
			else if (which == 2)
			{
				logMsgLn2();
				logMsgLn2("SetttingsApp", "replace");
				ReplaceTestFileApp2();

				logMsgLn2();
				logMsgLn2("SetttingsUser", "replace");
				ReplaceTestFileUser2();

				logMsgLn2();
				logMsgLn2("SetttingsApp", "initialize");
				AppSettings.Admin.initialize();
				logMsgLn2("status", AppSettings.Admin.Status);

				logMsgLn2();
				logMsgLn2("SetttingsUser", "initialize");
				UserSettings.Admin.initialize();
				logMsgLn2("status", UserSettings.Admin.Status);
			}

			logMsgLn2();
			logMsgLn2("status", "complete");
		}

		private void Upgrade(int which)
		{

			if (which == 2 || which == 3)
			{
				if (PathAndFile.App.Exists)
				{
					ReplaceTestFileApp();

					TestAp();

					AppSettingUpgrade aSup = new AppSettingUpgrade();

					List<SettingBase> As = aSup.su.SetgClasses;

					aSup.Upgrade();
				}
				else
				{
					logMsg2(nl);
					logMsgLn2("SetttingsApp", "created");


					AppSettings.Admin.Save();
				}
			}

			if (which == 1 || which == 3)
			{
				if (PathAndFile.User.Exists)
				{
					ReplaceTestFileUser();

					TestUs();

					UserSettingUpgrade uSup = new UserSettingUpgrade();

					List<SettingBase> Us = uSup.su.SetgClasses;

					uSup.Upgrade();
				}
				else
				{
					logMsg2(nl);
					logMsgLn2("SetttingsUser", "created");


					UserSettings.Admin.Save();
				}
			}
		}


		private void ProcessAppSettings(bool modify)
		{
			logMsgLn2();
			logMsgLn2("app path",AppSettings.Info.SettingPathAndFile);
			logMsgLn2("app before");

			if (PathAndFile.App.Exists)
			{
				AppSettings.Admin.Read();

				DisplayAppSettingData();

				if (modify)
				{
					ModifyAndSaveAppSettings();

					logMsgLn2();
					logMsgLn2("app after");
					DisplayAppSettingData();
				}
			}
			else
			{
				logMsgLn2("app setting file", "does not exist");
			}

//			ResetAndSaveAppSettings();
		}

		private void ProcessUserSettings(bool modify)
		{
			logMsgLn2();
			logMsgLn2("user path", UserSettings.Info.SettingPathAndFile);
			logMsgLn2("user before");

			if (PathAndFile.User.Exists)
			{
				UserSettings.Admin.Read();
				DisplayUserSettingData();

				if (modify)
				{
					ModifyAndSaveUserSettings();

					logMsgLn2();
					logMsgLn2("user after");
					DisplayUserSettingData();
				}
			}
			else
			{
				logMsgLn2("user setting file", "does not exist");
			}
		}

		private void ReplaceTestFileUser()
		{
			ReplaceTestFileUser2();

//			UserSettings.Admin.SetFileStatus();
		}

		// hard pathed to insure SettingMgr is not created now
		private void ReplaceTestFileUser2()
		{
//			string path = @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManagerV30";
//			string pathAndFile = path + @"\user.setting.xml";

			string testFileName = PathAndFile.User.SettingPath + @"\user.setting.xml.v20";

			DeleteUserSettingFile();

			File.Copy(testFileName, PathAndFile.User.SettingPathAndFile);

			logMsg2(nl);
			logMsgLn2("SetttingsUser", "test file replaced");
		}

		private void DeleteUserSettingFile()
		{
			File.Delete(PathAndFile.User.SettingPathAndFile);
		}

		private void ReplaceTestFileApp()
		{
			ReplaceTestFileApp2();
//			AppSettings.Admin.SetFileStatus();
		}

		// hard pathed to insure SettingMgr is not created now
		private void ReplaceTestFileApp2()
		{
//			string path = @"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManagerV30\AppSettings";
//			string pathAndFile = path + @"\SettingsManagerV30.setting.xml";

			string testFileName = PathAndFile.App.SettingPath + @"\SettingsManagerV30.setting.xml.v20";

			DeleteAppSettingFile();

			File.Copy(testFileName, PathAndFile.App.SettingPathAndFile);

			logMsg2(nl);
			logMsgLn2("SetttingsApp", "test file replaced");
		}

		private void DeleteAppSettingFile()
		{
			File.Delete(PathAndFile.App.SettingPathAndFile);
		}


		private void TestUs()
		{
			SettingsMgr<UserSettingInfo22> admin = UserSettings.Admin;

			UserSettingInfo21 x = new UserSettingInfo21();

			logMsg2(nl);
			logMsgLn2("class", "SettingsUser");

		}

		private void TestAp()
		{
			SettingsMgr<AppSettingInfo22> admin = AppSettings.Admin;

			AppSettingInfo21 x = new AppSettingInfo21();

			logMsg2(nl);
			logMsgLn2("class", "SettingsApp");

		}


		private void InitUs()
		{
			logMsgLn2("class", "SetttingsUser");

//			logMsgLn2("class", UserSettings);

			logMsgLn2("path"                    , UserSettings.Info.SettingPathAndFile);
			logMsgLn2("status"                  , UserSettings.Admin.Status);
			logMsg2(nl);
			logMsgLn2("file versions match"     , UserSettings.Admin.Info.ClassVersionsMatch);
			logMsgLn2("file version (from file)", UserSettings.Admin.Info.ClassVersionFromFile?? "does not exist");
			logMsgLn2("file version (in memory)", UserSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , UserSettings.Info.Header.SaveDateTime);
			logMsgLn2("assembly version"        , UserSettings.Info.Header.AssemblyVersion);
			logMsgLn2("file notes"              , UserSettings.Info.Header.Notes);
			logMsg2(nl);
		}

		private void InitAp()
		{
			logMsgLn2("class", "AppSettings");

			logMsgLn2("path"					, AppSettings.Info.SettingPathAndFile);
			logMsgLn2("status"					, AppSettings.Admin.Status);
			logMsg2(nl);
			logMsgLn2("file versions match"		, AppSettings.Admin.Info.ClassVersionsMatch);
			logMsgLn2("file version (from file)", AppSettings.Admin.Info.ClassVersionFromFile ?? "does not exist");
			logMsgLn2("file version (in memory)", AppSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , AppSettings.Info.Header.SaveDateTime);
			logMsgLn2("assembly version"        , AppSettings.Info.Header.AssemblyVersion);
			logMsgLn2("file notes"              , AppSettings.Info.Header.Notes);
			logMsg2(nl);
		}

		private const int V = 40;

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

			logMsgLn2("system version"          , UserSettings.Info.Header.SystemVersion);
			logMsgLn2("status"                  , UserSettings.Admin.Status);
			logMsgLn2("path and file"           , UserSettings.Info.SettingPathAndFile);
			logMsgLn2("path"                    , UserSettings.Info.SettingPath);
			logMsgLn2("root path"               , UserSettings.Info.RootPath);
			logMsgLn2("file name"               , UserSettings.Info.FileName);
			logMsgLn2("file version (from file)", UserSettings.Info.ClassVersionFromFile ?? "does not exist");
			logMsgLn2("class version (in memory)",UserSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , UserSettings.Info.Header.SaveDateTime);
			logMsgLn2("assembly version"        , UserSettings.Info.Header.AssemblyVersion);
			logMsgLn2("file notes"              , UserSettings.Info.Header.Notes);
			
			logMsgLn2("test dict one/IntA"      , UserSettings.Info.Data.TestDictionary3["one"].IntA.ToString());
			logMsgLn2("test dict one/IntB"      , UserSettings.Info.Data.TestDictionary3["one"].IntB.ToString());
			logMsgLn2("test dict one/IntC"      , UserSettings.Info.Data.TestDictionary3["one"].IntC.ToString());

			logMsgLn2("test int"                , UserSettings.Data.GeneralValues.TestI.ToString());
			logMsgLn2("test bool"               , UserSettings.Data.GeneralValues.TestB.ToString());
			logMsgLn2("test double"             , UserSettings.Data.GeneralValues.TestD.ToString());
			logMsgLn2("test string"             , UserSettings.Data.GeneralValues.TestS);
			logMsgLn2("test int[0]"             , UserSettings.Data.GeneralValues.TestIs[0].ToString());
			logMsgLn2("test int[1]"             , UserSettings.Data.GeneralValues.TestIs[1].ToString());
			logMsgLn2("test str[0]"             , UserSettings.Data.GeneralValues.TestSs[0]);
			logMsgLn2("test str[1]"             , UserSettings.Data.GeneralValues.TestSs[1]);
			logMsgLn2("test str[2]"             , UserSettings.Data.GeneralValues.TestSs[2]);
			logMsgLn2("win height"              , UserSettings.Data.MainWindow.Height.ToString());
			logMsgLn2("win width"               , UserSettings.Data.MainWindow.Width.ToString());
			logMsgLn2("uncat value"             , UserSettings.Data.UnCategorizedValue.ToString());

			logMsgLn2();
		}


		private void DisplayAppSettingData()
		{
			logMsgLn2("system version"          , AppSettings.Info.Header.SystemVersion);
			logMsgLn2("status"                  , AppSettings.Admin.Status);
			logMsgLn2("path and file"           , AppSettings.Info.SettingPathAndFile);
			logMsgLn2("path"                    , AppSettings.Info.SettingPath);
			logMsgLn2("root path"               , AppSettings.Info.SettingPath);
			logMsgLn2("file name"               , AppSettings.Info.FileName);
			logMsgLn2("file version (from file)" ,AppSettings.Info.ClassVersionFromFile ?? "does not exist");
			logMsgLn2("class version (in memory)",AppSettings.Info.ClassVersion);
			logMsgLn2("save date time"          , AppSettings.Info.Header.SaveDateTime);
			logMsgLn2("assembly version"        , AppSettings.Info.Header.AssemblyVersion);
			logMsgLn2("file notes"              , AppSettings.Info.Header.Notes);
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
