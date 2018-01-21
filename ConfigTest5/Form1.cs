using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

//using static ConfigTest5.SettingsUser;
using static ConfigTest5.SettingsUser;
using static ConfigTest5.SettingsApp;
//using static ConfigTest5.SettingsUsr;


namespace ConfigTest5
{
	public partial class Form1 : Form
	{
		public static string nl = Environment.NewLine;

		public Form1()
		{
			InitializeComponent();

			tbxMessasge.Text = "setting info" + nl;
						tbxMessasge.AppendText("file location| App| " + ASettings.SettingsPathAndFile + nl);
						tbxMessasge.AppendText("file location| Usr| " + Usettings.SettingsPathAndFile + nl);

//			ProcessUsrSettings();

			try
			{
				
				ProcessAppSettings();
				ProcessUserSettings();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Config Test 2");
				Application.Exit();
			}
		}

		private const int V = 10;

		private void ProcessUserSettings()
		{
			tbxMessasge.AppendText("user path| " + Usettings.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "user before" + nl);

			DisplayUserSettingData();

			USet.GeneralValues.TestB = true;
			USet.GeneralValues.TestD = V + 0.2;
			USet.GeneralValues.TestS = "using generic setting file " + V;
			USet.GeneralValues.TestI = V;
			USet.GeneralValues.TestIs[1] = V;
			USet.GeneralValues.TestSs[1] = "generic " + V;
			USet.UnCategorizedValue = V;
			USet.MainWindow.height = V * 50;
			USet.MainWindow.width = V * 100;
			USet.testDictionary3["one"] = new testStruct(V * 10 + 4, V * 10 + 5, V * 10 + 6);
			USet.testDictionary3["two"] = new testStruct(V * 10 + 1, V * 10 + 2, V * 10 + 3);
			USet.testDictionary3["three"] = new testStruct(V * 10 + 7, V * 10 + 8, V * 10 + 9);

			Usettings.Save();

			tbxMessasge.AppendText("user after" + nl);

			DisplayUserSettingData();

//			Usettings.Reset();
//			Usettings.Save();
//
//			tbxMessasge.AppendText("user reset" + nl);
//			DisplayUserSettingData();
		}

		private void DisplayUserSettingData()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(Usettings.SettingsPathAndFile);
			sb.Append("test int    | ").AppendLine(USet.GeneralValues.TestI.ToString());
			sb.Append("test bool   | ").AppendLine(USet.GeneralValues.TestB.ToString());
			sb.Append("test double | ").AppendLine(USet.GeneralValues.TestD.ToString());
			sb.Append("test string | ").AppendLine(USet.GeneralValues.TestS);
			sb.Append("test int[0] | ").AppendLine(USet.GeneralValues.TestIs[0].ToString());
			sb.Append("test int[1] | ").AppendLine(USet.GeneralValues.TestIs[1].ToString());
			sb.Append("test str[0] | ").AppendLine(USet.GeneralValues.TestSs[0]);
			sb.Append("test str[1] | ").AppendLine(USet.GeneralValues.TestSs[1]);
			sb.Append("test str[2] | ").AppendLine(USet.GeneralValues.TestSs[2]);
			sb.Append("win height  | ").AppendLine(USet.MainWindow.height.ToString());
			sb.Append("win width   | ").AppendLine(USet.MainWindow.width.ToString());
			sb.Append("uncat value | ").AppendLine(USet.UnCategorizedValue.ToString());
			
			
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}


		private void ProcessAppSettings()
		{
			tbxMessasge.AppendText(" app path| " + ASettings.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "app before" + nl);

			DisplayAppSettingData();

			ASet.AppS = "generic app data " + V;
			ASet.AppB = false;
			ASet.AppD = V + 0.1;
			ASet.AppI = V;
			ASet.AppIs[0] = V;

			tbxMessasge.AppendText("app after" + nl);

			DisplayAppSettingData();

//			ASettings.Reset();
//			ASettings.Save();
//
//			tbxMessasge.AppendText("app reset" + nl);
//			DisplayAppSettingData();
		}

		private void DisplayAppSettingData()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(ASettings.SettingsPathAndFile);
			sb.Append("test string | ").AppendLine(ASet.AppS);
			sb.Append("test bool   | ").AppendLine(ASet.AppB.ToString());
			sb.Append("test double | ").AppendLine(ASet.AppD.ToString());
			sb.Append("test int    | ").AppendLine(ASet.AppI.ToString());
			sb.Append("test int[0] | ").AppendLine(ASet.AppIs[0].ToString());
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

	}

}
