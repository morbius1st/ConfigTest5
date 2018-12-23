using System;
using System.Collections.Generic;
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

			//			SettingsUserX<UserSettings21> ver21 = new SettingsUserX<UserSettings21>();
			//			ver21.Init(UserSettings21.USERSETTINGFILEVERSION);
			//
			//			SetCtrl.Add(UserSettings21.USERSETTINGFILEVERSION, ver21);
			//
			//			List<Setg> Y = SetCtrl.SetList;
			//
			//			dynamic curr = SetCtrl.Find(SettingsUser.)

			// this just sets up the system / structure but does not read the data
//			SettingsUser Current = new SettingsUser();
//			SettingsUser.Init(UserSettings.USERSETTINGFILEVERSION);


//			dynamic x = Current.UxSettings.Read(typeof(UserSettings20)) as UserSettings20;

			List<UserSettingBase> Us = USetgUpgrade.USetgBase;

			//			Init1();

			InitCurr();

//			Test1();

		}

		private void Test1()
		{
			DisplayUserSettingData();
		}

		private void InitCurr()
		{
			logMsgLn2("class", "SetttingsUser");

			logMsgLn2("path", SettingsUser.USetgAdmin.SettingsPathAndFile);
			logMsgLn2("status", SettingsUser.USetgAdmin.Status);
			logMsg2(nl);
			logMsgLn2("file versions match", SettingsUser.USetgAdmin.FileVersionsMatch());
			logMsgLn2("file version (in file)", SettingsUser.USetgAdmin.GetFileVersion() ?? "does not exist");
			logMsgLn2("file version (in memory)", SettingsUser.USetgData.FileVersion);
			logMsgLn2("save date time", SettingsUser.USetgAdmin.SaveDateTime);
			logMsgLn2("assembly version", SettingsUser.USetgAdmin.AssemblyVersion);
			logMsgLn2("file notes", SettingsUser.USetgAdmin.SettingFileNotes);
			logMsg2(nl);
		}

		private void Init1()
		{
			logMsgLn2("path", SettingsUser.USetgAdmin.SettingsPathAndFile);
			SettingsUser.USetgAdmin.Save();
			logMsgLn2("status", SettingsUser.USetgAdmin.Status);
			logMsgLn2("system version", SettingsUser.USetgAdmin.GetSystemVersion());
			logMsgLn2("file version", SettingsUser.USetgAdmin.GetFileVersion());
			logMsg2("\n");
			logMsgLn2("path", SettingsApp.ASettings.SettingsPathAndFile);
			SettingsApp.ASettings.Save();
			logMsgLn2("status", SettingsApp.ASettings.Status);
			logMsgLn2("file version", SettingsApp.ASettings.GetFileVersion());
			logMsgLn2("system version", SettingsApp.ASettings.GetSystemVersion());
			logMsg2("\n");
		}

		private const int V = 40;

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				ProcessAppSettings();
				ProcessUserSettings(false);

			}
			catch (Exception ex)
			{
				logMsgLn2(nl + "EXCEPTION" + nl);
				logMsgLn2(ex.Message + nl);
				logMsgLn2(ex.InnerException + nl);
			}
		}

		private void ProcessUserSettings(bool modify)
		{
			logMsgLn2(nl + "user path", SettingsUser.USetgAdmin.SettingsPathAndFile + nl);

			logMsgLn2(nl + "user before");

			DisplayUserSettingData();
			if (modify)
			{
				ModifyAndSaveUserSettings();

				logMsgLn2(nl + "user after");
				DisplayUserSettingData();
			}
		}
		

		private void ResetUserSettings()
		{
			SettingsUser.USetgAdmin.Reset();
			SettingsUser.USetgAdmin.Save();

			logMsgLn2(nl + "user reset");
			DisplayUserSettingData();
		}


		private void ModifyAndSaveUserSettings()
		{

			SettingsUser.USetgData.GeneralValues.TestB = true;
			SettingsUser.USetgData.GeneralValues.TestD = V + 0.2;
			SettingsUser.USetgData.GeneralValues.TestS = "using generic setting file " + V;
			SettingsUser.USetgData.GeneralValues.TestI = V;
			SettingsUser.USetgData.GeneralValues.TestIs[1] = V;
			SettingsUser.USetgData.GeneralValues.TestSs[1] = "generic " + V;
			SettingsUser.USetgData.UnCategorizedValue = V;
			SettingsUser.USetgData.MainWindow.Height = V * 50;
			SettingsUser.USetgData.MainWindow.Width = V * 100;
			SettingsUser.USetgData.TestDictionary3["one"] = new TestStruct(V * 10 + 4, V * 10 + 5, V * 10 + 6);
			SettingsUser.USetgData.TestDictionary3["two"] = new TestStruct(V * 10 + 1, V * 10 + 2, V * 10 + 3);
			SettingsUser.USetgData.TestDictionary3["three"] = new TestStruct(V * 10 + 7, V * 10 + 8, V * 10 + 9);

			SettingsUser.USetgAdmin.Save();
		}

		private void DisplayUserSettingData()
		{
			logMsgLn2("system version", SettingsUser.USetgAdmin.Settings.Heading.SettingSystemVersion);
			logMsgLn2("status", SettingsUser.USetgAdmin.Status);
			logMsgLn2("file name", SettingsUser.USetgAdmin.SettingsPathAndFile);
			logMsgLn2("file version (in file)", SettingsUser.USetgAdmin.GetFileVersion() ?? "does not exist");
			logMsgLn2("file version (in memory)", SettingsUser.USetgData.FileVersion);

			if ((int)SettingsUser.USetgAdmin.Status > 0)
			{
				logMsgLn2("test int", SettingsUser.USetgData.GeneralValues.TestI.ToString());
				logMsgLn2("test bool", SettingsUser.USetgData.GeneralValues.TestB.ToString());
				logMsgLn2("test double", SettingsUser.USetgData.GeneralValues.TestD.ToString());
				logMsgLn2("test string", SettingsUser.USetgData.GeneralValues.TestS);
				logMsgLn2("test int[0]", SettingsUser.USetgData.GeneralValues.TestIs[0].ToString());
				logMsgLn2("test int[1]", SettingsUser.USetgData.GeneralValues.TestIs[1].ToString());
				logMsgLn2("test str[0]", SettingsUser.USetgData.GeneralValues.TestSs[0]);
				logMsgLn2("test str[1]", SettingsUser.USetgData.GeneralValues.TestSs[1]);
				logMsgLn2("test str[2]", SettingsUser.USetgData.GeneralValues.TestSs[2]);
				logMsgLn2("win height", SettingsUser.USetgData.MainWindow.Height.ToString());
				logMsgLn2("win width", SettingsUser.USetgData.MainWindow.Width.ToString());
				logMsgLn2("uncat value", SettingsUser.USetgData.UnCategorizedValue.ToString());
				logMsgLn2("uncat value2", SettingsUser.USetgData.UnCategorizedValue2.ToString());
			}

			logMsgLn2(nl);
		}


		private void ProcessAppSettings()
		{
			logMsgLn2(nl + "app path",SettingsApp.ASettings.SettingsPathAndFile + nl);
			logMsgLn2(nl + "app before");

			DisplayAppSettingData();

			ModifyAndSaveAppSettings();

			logMsgLn2(nl + "app after");
			DisplayAppSettingData();

//			ResetAndSaveAppSettings();
		}

		private void DisplayAppSettingData()
		{
			logMsgLn2("file name",SettingsApp.ASettings.SettingsPathAndFile);
			logMsgLn2("file version",SettingsApp.ASettings.GetFileVersion());
			logMsgLn2("system version",SettingsApp.ASettings.Settings.Heading.SettingSystemVersion);
			logMsgLn2("test string",SettingsApp.ASet.AppS);
			logMsgLn2("test bool",SettingsApp.ASet.AppB.ToString());
			logMsgLn2("test double",SettingsApp.ASet.AppD.ToString());
			logMsgLn2("test int",SettingsApp.ASet.AppI.ToString());
			logMsgLn2("test int[0]",SettingsApp.ASet.AppIs[0].ToString());
			logMsgLn2(nl);
		}

		private void ModifyAndSaveAppSettings()
		{
			SettingsApp.ASet.AppS = "generic app data " + V;
			SettingsApp.ASet.AppB = false;
			SettingsApp.ASet.AppD = V + 0.1;
			SettingsApp.ASet.AppI = V;
			SettingsApp.ASet.AppIs[0] = V;

			SettingsApp.ASettings.Save();
		}

		private void ResetAndSaveAppSettings()
		{
			SettingsApp.ASettings.Reset();
			SettingsApp.ASettings.Save();

			logMsgLn2(nl + "app reset");
			DisplayAppSettingData();
		}

	}
}
