using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace ConfigTest2
{
	public partial class Form1 : Form
	{
		public static string nl = Environment.NewLine;

		public Form1()
		{
			InitializeComponent();

			tbxMessasge.Text = "setting info" + nl;

			try
			{
				ProcessUserSettings();

				ProcessAppSettings();

			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Config Test 2");
				Application.Exit();
			}
		}

		private const int v = 16;

		private void ProcessAppSettings()
		{
			Settings<SettingsApp> AppSettings = new Settings<SettingsApp>();

			tbxMessasge.AppendText(" app path| " + AppSettings.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "app before" + nl);

			DisplayAppSettingData(AppSettings);

			AppSettings.Setting.AppS = "generic app data " + v;
			AppSettings.Setting.AppB = false;
			AppSettings.Setting.AppD = v + 0.1;
			AppSettings.Setting.AppI = v;
			AppSettings.Setting.AppIs[0] = v;

			AppSettings.Save();

			tbxMessasge.AppendText("app after" + nl);

			DisplayAppSettingData(AppSettings);

		}

		private void DisplayAppSettingData(Settings<SettingsApp> settingApp)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(settingApp.SettingsPathAndFile);
			sb.Append("test string | ").AppendLine(settingApp.Setting.AppS);
			sb.Append("test bool   | ").AppendLine(settingApp.Setting.AppB.ToString());
			sb.Append("test double | ").AppendLine(settingApp.Setting.AppD.ToString());
			sb.Append("test int    | ").AppendLine(settingApp.Setting.AppI.ToString());
			sb.Append("test int[0] | ").AppendLine(settingApp.Setting.AppIs[0].ToString());
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

		private void ProcessUserSettings()
		{
			Settings<SettingsUser> UserSettings = new Settings<SettingsUser>();

			tbxMessasge.AppendText("user path| " + UserSettings.SettingsPathAndFile + nl);

			tbxMessasge.AppendText(nl + "user before" + nl);

			DisplayUserSettingData(UserSettings);

			UserSettings.Setting.GeneralValues.TestB = true;
			UserSettings.Setting.GeneralValues.TestD = v + 0.2;
			UserSettings.Setting.GeneralValues.TestS = "using generic setting file " + v;
			UserSettings.Setting.GeneralValues.TestI = v;
			UserSettings.Setting.GeneralValues.TestIs[1] = v;
			UserSettings.Setting.GeneralValues.TestSs[1] = "generic " + v;
			UserSettings.Setting.UnCategorizedValue = v;
			UserSettings.Setting.MainWindow.height = v * 50;
			UserSettings.Setting.MainWindow.width = v * 100;

			UserSettings.Save();

			tbxMessasge.AppendText("user after" + nl);

			DisplayUserSettingData(UserSettings);
		}

		private void DisplayUserSettingData(Settings<SettingsUser> settingUser)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(settingUser.SettingsPathAndFile);
			sb.Append("test int    | ").AppendLine(settingUser.Setting.GeneralValues.TestI.ToString());
			sb.Append("test bool   | ").AppendLine(settingUser.Setting.GeneralValues.TestB.ToString());
			sb.Append("test double | ").AppendLine(settingUser.Setting.GeneralValues.TestD.ToString());
			sb.Append("test string | ").AppendLine(settingUser.Setting.GeneralValues.TestS);
			sb.Append("test int[0] | ").AppendLine(settingUser.Setting.GeneralValues.TestIs[0].ToString());
			sb.Append("test int[1] | ").AppendLine(settingUser.Setting.GeneralValues.TestIs[1].ToString());
			sb.Append("test str[0] | ").AppendLine(settingUser.Setting.GeneralValues.TestSs[0]);
			sb.Append("test str[1] | ").AppendLine(settingUser.Setting.GeneralValues.TestSs[1]);
			sb.Append("test str[2] | ").AppendLine(settingUser.Setting.GeneralValues.TestSs[2]);
			sb.Append("uncat value | ").AppendLine(settingUser.Setting.UnCategorizedValue.ToString());
			sb.Append("win height  | ").AppendLine(settingUser.Setting.MainWindow.height.ToString());
			sb.Append("win width   | ").AppendLine(settingUser.Setting.MainWindow.width.ToString());
			
			
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

		
	}

}
