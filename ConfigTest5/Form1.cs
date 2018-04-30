using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using static ConfigTest5.SettingsUser;
using static ConfigTest5.SettingsApp;



namespace ConfigTest5
{
	public partial class Form1 : Form
	{
		public static string nl = Environment.NewLine;

		public Form1()
		{
			InitializeComponent();
			tbxMessasge.Text = "setting info" + nl;
			tbxMessasge.Select(0,0);

		}

		private const int V = 40;

		private void ProcessUserSettings()
		{
			tbxMessasge.AppendText("user path| " + USettings.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "user before" + nl);

			DisplayUserSettingData();

			ModifyAndSaveUserSettings();

//			ResetUserSettings();

		}


		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				ProcessAppSettings();
//				ProcessUserSettings();

			}
			catch (Exception ex)
			{
				tbxMessasge.AppendText(nl + "EXCEPTION" + nl);
				tbxMessasge.AppendText(ex.Message + nl);
				tbxMessasge.AppendText(ex.InnerException + nl);
			}
		}

		private void ResetUserSettings()
		{
			USettings.Reset();
			USettings.Save();

			tbxMessasge.AppendText("user reset" + nl);
			DisplayUserSettingData();
		}


		private void ModifyAndSaveUserSettings()
		{

			USet.GeneralValues.TestB = true;
			USet.GeneralValues.TestD = V + 0.2;
			USet.GeneralValues.TestS = "using generic setting file " + V;
			USet.GeneralValues.TestI = V;
			USet.GeneralValues.TestIs[1] = V;
			USet.GeneralValues.TestSs[1] = "generic " + V;
			USet.UnCategorizedValue = V;
			USet.MainWindow.Height = V * 50;
			USet.MainWindow.Width = V * 100;
			USet.TestDictionary3["one"] = new TestStruct(V * 10 + 4, V * 10 + 5, V * 10 + 6);
			USet.TestDictionary3["two"] = new TestStruct(V * 10 + 1, V * 10 + 2, V * 10 + 3);
			USet.TestDictionary3["three"] = new TestStruct(V * 10 + 7, V * 10 + 8, V * 10 + 9);

			USettings.Save();

			tbxMessasge.AppendText("user after" + nl);
			DisplayUserSettingData();
		}


		private void DisplayUserSettingData()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(USettings.SettingsPathAndFile);
			sb.Append("test int    | ").AppendLine(USet.GeneralValues.TestI.ToString());
			sb.Append("test bool   | ").AppendLine(USet.GeneralValues.TestB.ToString());
			sb.Append("test double | ").AppendLine(USet.GeneralValues.TestD.ToString());
			sb.Append("test string | ").AppendLine(USet.GeneralValues.TestS);
			sb.Append("test int[0] | ").AppendLine(USet.GeneralValues.TestIs[0].ToString());
			sb.Append("test int[1] | ").AppendLine(USet.GeneralValues.TestIs[1].ToString());
			sb.Append("test str[0] | ").AppendLine(USet.GeneralValues.TestSs[0]);
			sb.Append("test str[1] | ").AppendLine(USet.GeneralValues.TestSs[1]);
			sb.Append("test str[2] | ").AppendLine(USet.GeneralValues.TestSs[2]);
			sb.Append("win height  | ").AppendLine(USet.MainWindow.Height.ToString());
			sb.Append("win width   | ").AppendLine(USet.MainWindow.Width.ToString());
			sb.Append("uncat value | ").AppendLine(USet.UnCategorizedValue.ToString());
			sb.Append("uncat value2| ").AppendLine(USet.UnCategorizedValue2.ToString());


			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}


		private void ProcessAppSettings()
		{
			tbxMessasge.AppendText(" app path| " + ASettings.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "app before" + nl);

			DisplayAppSettingData();

			ModifyAndSaveAppSettings();

//			ResetAndSaveAppSettings();
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

		private void ModifyAndSaveAppSettings()
		{
			ASet.AppS = "generic app data " + V;
			ASet.AppB = false;
			ASet.AppD = V + 0.1;
			ASet.AppI = V;
			ASet.AppIs[0] = V;

			ASettings.Save();

			tbxMessasge.AppendText("app after" + nl);

			DisplayAppSettingData();
		}

		private void ResetAndSaveAppSettings()
		{
			ASettings.Reset();
			ASettings.Save();
			
			tbxMessasge.AppendText("app reset" + nl);
			DisplayAppSettingData();
		}

	}
}
