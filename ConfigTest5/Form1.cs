using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

//using static ConfigTest5.SettingsUser;
using static ConfigTest5.SettingsUser2;
using static ConfigTest5.SettingsApp;

using static ConfigTest5.Settings2;


namespace ConfigTest5
{
	public partial class Form1 : Form
	{
		public static string nl = Environment.NewLine;

		public Form1()
		{
			InitializeComponent();

			tbxMessasge.Text = "setting info" + nl;

			ProcessUserSettings2();

			try
			{
				

//				ProcessAppSettings();

			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Config Test 2");
				Application.Exit();
			}
		}

		private const int v = 1;

		private void ProcessUserSettings2()
		{
			//			tbxMessasge.AppendText("user path| " + USetting.SettingsPathAndFile + nl);
			//
			//			tbxMessasge.AppendText(nl + "user before" + nl);
			//
			//			DisplayUserSettingData();

			tbxMessasge.AppendText("data processed");

//			bool b = USetting2.Setting.GeneralValues.TestB;

			USet2.GeneralValues.TestI = 20000;

			USetting2.Save2();

			


//			DisplayUserSetting2("before");
//
//			xUset.int1 = v * 10;
//			xUset.string1 = "string " + xUset.int1.ToString();
//
//			Settings2.Save(xUset);
//			Settings2.Save(xAset);
//
//			Settings2.Read(xUset);
//			Settings2.Read(xAset);
//
//			Debug.WriteLine("");
//			DisplayUserSetting2("after");

		}

		private void DisplayUserSetting2(string msg)
		{
			Debug.WriteLine("setting2| " + msg);
			Debug.WriteLine("    int1| " + xUset.int1);
			Debug.WriteLine(" string1| " + xUset.string1);
			Debug.WriteLine("    int2| " + xAset.int1);
			Debug.WriteLine(" string2| " + xAset.string1);
		}


		private void ProcessAppSettings()
		{
//			Settings<AppSettings> ASetting = new Settings<AppSettings>();

			tbxMessasge.AppendText(" app path| " + ASetting.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "app before" + nl);

			DisplayAppSettingData();

			ASet.AppS = "generic app data " + v;
			ASet.AppB = false;
			ASet.AppD = v + 0.1;
			ASet.AppI = v;
			ASet.AppIs[0] = v;

			ASetting.Save();

			tbxMessasge.AppendText("app after" + nl);

			DisplayAppSettingData();

//			ASetting.Reset();

			tbxMessasge.AppendText("app reset" + nl);

			DisplayAppSettingData();
		}

		private void DisplayAppSettingData()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(ASetting.SettingsPathAndFile);
			sb.Append("test string | ").AppendLine(ASet.AppS);
			sb.Append("test bool   | ").AppendLine(ASet.AppB.ToString());
			sb.Append("test double | ").AppendLine(ASet.AppD.ToString());
			sb.Append("test int    | ").AppendLine(ASet.AppI.ToString());
			sb.Append("test int[0] | ").AppendLine(ASet.AppIs[0].ToString());
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

//		private void ProcessUserSettings()
//		{
////			Settings<UserSettings> USetting = new Settings<UserSettings>();
//
//			tbxMessasge.AppendText("user path| " + USetting.SettingsPathAndFile + nl);
//
//			tbxMessasge.AppendText(nl + "user before" + nl);
//
//			DisplayUserSettingData();
//
//			USet.GeneralValues.TestB = true;
//			USet.GeneralValues.TestD = v + 0.2;
//			USet.GeneralValues.TestS = "using generic setting file " + v;
//			USet.GeneralValues.TestI = v;
//			USet.GeneralValues.TestIs[1] = v;
//			USet.GeneralValues.TestSs[1] = "generic " + v;
//			USet.UnCategorizedValue = v;
//			USet.MainWindow.height = v * 50;
//			USet.MainWindow.width = v * 100;
//
//			USetting.Save();
//
//			tbxMessasge.AppendText("user after" + nl);
//
//			DisplayUserSettingData();
//
//			USetting.Reset();
//
//			tbxMessasge.AppendText("user reset" + nl);
//
//			DisplayUserSettingData();
//		}
//
//		private void DisplayUserSettingData()
//		{
//			StringBuilder sb = new StringBuilder();
//
//			sb.Append("file name   | ").AppendLine(USetting.SettingsPathAndFile);
//			sb.Append("test int    | ").AppendLine(USet.GeneralValues.TestI.ToString());
//			sb.Append("test bool   | ").AppendLine(USet.GeneralValues.TestB.ToString());
//			sb.Append("test double | ").AppendLine(USet.GeneralValues.TestD.ToString());
//			sb.Append("test string | ").AppendLine(USet.GeneralValues.TestS);
//			sb.Append("test int[0] | ").AppendLine(USet.GeneralValues.TestIs[0].ToString());
//			sb.Append("test int[1] | ").AppendLine(USet.GeneralValues.TestIs[1].ToString());
//			sb.Append("test str[0] | ").AppendLine(USet.GeneralValues.TestSs[0]);
//			sb.Append("test str[1] | ").AppendLine(USet.GeneralValues.TestSs[1]);
//			sb.Append("test str[2] | ").AppendLine(USet.GeneralValues.TestSs[2]);
//			sb.Append("win height  | ").AppendLine(USet.MainWindow.height.ToString());
//			sb.Append("win width   | ").AppendLine(USet.MainWindow.width.ToString());
//			sb.Append("uncat value | ").AppendLine(USet.UnCategorizedValue.ToString());
//			
//			
//			sb.Append(nl).Append(nl);
//
//			tbxMessasge.AppendText(sb.ToString());
//		}

		
	}

}
