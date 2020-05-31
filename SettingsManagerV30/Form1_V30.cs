using System;
using System.IO;
using System.Windows.Forms;
using SettingsManager;
using SettingsManagerV30.Properties;
using UtilityLibrary;
using static SettingsManagerV30.AppSettings;
using static UtilityLibrary.MessageUtilities2;



namespace SettingsManagerV30
{

	public partial class Form1_V30 : Form
	{
		public static string nl = Environment.NewLine;

		private const string USER_SETG = "UserSettings";
		private const string APP_SETG = "AppSettings";

		public Form1_V30()
		{
			InitializeComponent();
			rtbMessasge.Text = "setting info" + nl;
			rtbMessasge.Select(0,0);

			MessageUtilities.OutLocation = MessageUtilities.OutputLocation.TEXT_BOX;
//			MessageUtilities.OutLocation = MessageUtilities.OutputLocation.DEBUG;
			MessageUtilities.RichTxtBox = rtbMessasge;

		}

		#region +Controls

		private void button2_Click(object sender,
			EventArgs e)
		{
			int which = 5;

			switch (which)
			{
			case 1:
				{
					Files();
					break;
				}
			case 2:
				{
					Status(2);
					break;
				}
			case 3:
				{
					SubclassTest();
					break;
				}
			case 4:
				{
					Reset();
					break;
				}
			case 5:
				{
					logMsgLn2("******* at switch", "create AppSettings object ******");
					logMsgLn2();
					UpgradeControl<AppSettingInfo22>(APP_SETG);

					logMsgLn2(nl);
					logMsgLn2(nl);
					logMsgLn2("****** at switch", "create UserSettings object ******");
					logMsgLn2();

					UpgradeControl<UserSettingInfo22>(USER_SETG);

					logMsgLn2(nl);
					logMsgLn2();
					logMsgLn2("******* data list", "AppSettings object ******");
					DisplayAppSettingData();

					logMsgLn2(nl);
					logMsgLn2();
					logMsgLn2("******* data list", "UserSettings object ******");
					DisplayUserSettingData();

					break;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				ProcessAppSettings(true);
				ProcessUserSettings(true);

			}
			catch (Exception ex)
			{
				logMsgLn2(nl + "EXCEPTION" + nl);
				logMsgLn2(ex.Message + nl);
				logMsgLn2(ex.InnerException + nl);
			}
		}

		#endregion

		#region +Reset

		private void Reset()
		{
			ResetAndSaveAppSettings();
			ResetAndSaveUserSettings();
		}

		#endregion

		#region +Files

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
		

		#endregion

		#region +Status

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
				Admin.Create();
				logMsgLn2("create status", Admin.Status);

				logMsgLn2("SetttingsApp", "saving");
				Admin.Save();
				logMsgLn2("save status", Admin.Status);

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
				Admin.Initialize();
				logMsgLn2("status", Admin.Status);

				logMsgLn2();
				logMsgLn2("SetttingsUser", "initialize");
				UserSettings.Admin.Initialize();
				logMsgLn2("status", UserSettings.Admin.Status);
			}

			logMsgLn2();
			logMsgLn2("status", "complete");
		}

		#endregion

		#region +SubclassTest

		private void SubclassTest()
		{
			logMsgLn2();
			logMsgLn2("at SubclassTest", "appsettings");
			logMsgLn2("appsettings", "classversion| " +
				Info.ClassVersion);

			logMsgLn2();
			logMsgLn2("at SubclassTest", "UserSettings");
			logMsgLn2("UserSettings", "classversion| " +
				UserSettings.Info.ClassVersion);
		}

		#endregion

		#region +UpgradeControl

//		private void UpgradeControl<T>(SettingsMgr<T> admin, string name) where T : SettingBase, new()
		private void UpgradeControl<T>(string name) where T : SettingBase, new()
		{
			SettingsMgr<T> admin = null;

			logMsgLn2(nl);
			logMsgLn2("at UpgradeControl", "**** start ****");

			for (int i = 0; i < 2; i++)
			{
				logMsgLn2(nl);
				logMsgLn2("******** Begin", name + " *****");

				logMsgLn2();
				logMsgLn2("at UpgradeControl", name + "| replace");

				switch (name) 
				{
				case APP_SETG:
					{
						ReplaceTestFileApp2();

						logMsgLn2();
						logMsgLn2("at UpgradeControl", name + "| create object");
						admin = AppSettings.Admin as SettingsMgr<T>;
						break;
					}
				case USER_SETG:
					{
						ReplaceTestFileUser2();

						logMsgLn2();
						logMsgLn2("at UpgradeControl", name + "| create object");
						admin = UserSettings.Admin as SettingsMgr<T>;
						break;
					}
				}

				// the object is not created here
				// display status
				logMsgLn2();
				logMsgLn2("at UpgradeControl",
					"*** " + name + "| get status| " + admin.Status);

				// test upgrade process
				// auto upgrade
				if (i == 0)
				{
					logMsgLn2();
					logMsgLn2("at UpgradeControl", name + "| set CanAutoUpgrade| true");
					admin.CanAutoUpgrade = true;

					logMsgLn2();
					logMsgLn2("at UpgradeControl", name + "| initialize | expect auto upgrade");
					admin.Initialize();

//					ShowAppStatus(admin, name);
				}
				else
				{
					logMsgLn2();
					logMsgLn2("at UpgradeControl", name + "| set CanAutoUpgrade| false");
					admin.CanAutoUpgrade = false;

					logMsgLn2();
					logMsgLn2("at UpgradeControl", name + "| initialize | expect manual upgrade");
					admin.Initialize();

//					ShowAppStatus(admin, name);

					logMsgLn2();
					logMsgLn2("at UpgradeControl", name + "| manual upgrade");
					admin.Upgrade();

//					ShowAppStatus(admin, name);
				}
			}
		}

		#endregion

		private void ShowAppStatus<T>(SettingsMgr<T> admin, string name)  where  T : SettingBase, new ()
		{
			logMsgLn2();
			logMsgLn2("at UpgradeControl",
				name + "|              status| " + admin.Status
				);
			logMsgLn2("at UpgradeControl",
				name + "|   upgrade required?| " + admin.UpgradeRequired
				);
			logMsgLn2("at UpgradeControl",
				name + "| ClassVersionsMatch?| " + admin.Info.ClassVersionsMatch
				);
		}

		private void ProcessAppSettings(bool modify)
		{
			logMsgLn2();
			logMsgLn2("app path",Info.SettingPathAndFile);
			logMsgLn2("app before");

			if (PathAndFile.App.Exists)
			{
				logMsgLn2();
				logMsgLn2("admin", "read");

				Admin.Read();

				DisplayAppSettingData();

				if (modify)
				{
					ModifyAndSaveAppSettings();

					logMsgLn2();
					logMsgLn2("app after modify");
					DisplayAppSettingData();
				}
			}
			else
			{
				logMsgLn2("app setting file", "does not exist");
			}
		}

		private void ProcessUserSettings(bool modify)
		{
			logMsgLn2();
			logMsgLn2("user path", UserSettings.Info.SettingPathAndFile);
			logMsgLn2("user before");

			if (PathAndFile.User.Exists)
			{
				logMsgLn2();
				logMsgLn2("admin", "read");

				UserSettings.Admin.Read();
				DisplayUserSettingData();

				if (modify)
				{
					ModifyAndSaveUserSettings();

					logMsgLn2();
					logMsgLn2("user after modify");
					DisplayUserSettingData();
				}
			}
			else
			{
				logMsgLn2("user setting file", "does not exist");
			}
		}

		// hard pathed to insure SettingMgr is not created now
		private void ReplaceTestFileUser2()
		{
			string testFileName = PathAndFile.User.SettingPath + @"\user.setting.xml.v20";

			DeleteUserSettingFile();

			File.Copy(testFileName, PathAndFile.User.SettingPathAndFile);

			logMsgLn2();
			logMsgLn2("at ReplaceTestFileUser2", "test file replaced");
		}

		private void DeleteUserSettingFile()
		{
			File.Delete(PathAndFile.User.SettingPathAndFile);
		}

		// hard pathed to insure SettingMgr is not created now
		private void ReplaceTestFileApp2()
		{
			string testFileName = PathAndFile.App.SettingPath + @"\SettingsManagerV30.setting.xml.v20";

			DeleteAppSettingFile();

			File.Copy(testFileName, PathAndFile.App.SettingPathAndFile);

			logMsgLn2();
			logMsgLn2("ReplaceTestFileApp2", "test file replaced");
		}

		private void DeleteAppSettingFile()
		{
			File.Delete(PathAndFile.App.SettingPathAndFile);
		}

		private void ResetAndSaveUserSettings()
		{
			UserSettings.Admin.Reset();
			UserSettings.Admin.Save();

			logMsgLn2(nl + "user reset");
			DisplayUserSettingData();
		}

		private const int V = 40;
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
			SettingsMgr<UserSettingInfo22> ua = UserSettings.Admin;


			logMsgLn2();
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
			logMsgLn2();
			logMsgLn2("system version"          , Info.Header.SystemVersion);
			logMsgLn2("status"                  , Admin.Status);
			logMsgLn2("path and file"           , Info.SettingPathAndFile);
			logMsgLn2("path"                    , Info.SettingPath);
			logMsgLn2("root path"               , Info.SettingPath);
			logMsgLn2("file name"               , Info.FileName);
			logMsgLn2("file version (from file)" ,Info.ClassVersionFromFile ?? "does not exist");
			logMsgLn2("class version (in memory)",Info.ClassVersion);
			logMsgLn2("save date time"          , Info.Header.SaveDateTime);
			logMsgLn2("assembly version"        , Info.Header.AssemblyVersion);
			logMsgLn2("file notes"              , Info.Header.Notes);
			logMsg2(nl);
			logMsgLn2("test string"             , Data.AppS);
			logMsgLn2("test bool"               , Data.AppB.ToString());
			logMsgLn2("test double"             , Data.AppD.ToString());
			logMsgLn2("test int"                , Data.AppI.ToString());
			logMsgLn2("test int[0]"             , Data.AppIs[0].ToString());
			logMsgLn2("test AppI20"             , Data.AppI20.ToString());
			logMsg2(nl);
		}

		private void ModifyAndSaveAppSettings()
		{
			Data.AppS = "generic app data " + V;
			Data.AppB = false;
			Data.AppD = V + 0.1;
			Data.AppI = V;
			Data.AppIs[0] = V;

			Admin.Save();
		}

		private void ResetAndSaveAppSettings()
		{
			Admin.Reset();
			Admin.Save();

			logMsgLn2(nl + "app reset");
			DisplayAppSettingData();
		}

	}
}
