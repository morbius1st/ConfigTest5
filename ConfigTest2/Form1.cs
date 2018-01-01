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

			tbxMessasge.Text = "config info" + nl;

			try
			{
				ProcessUserConfig();

				ProcessAppConfig();

			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Config Test 2");
				Application.Exit();
			}
		}

		private const int v = 8;

		private void ProcessAppConfig()
		{
			Config<SettingsApp> cfgApp = new Config<SettingsApp>();

			tbxMessasge.AppendText(" app path| " + cfgApp.ConfigFileName + nl);

			tbxMessasge.AppendText(nl + "app before" + nl);

			cfgApp.GetConfigData();

			DisplayAppConfigData(cfgApp);

			cfgApp.Settings.AppS = "generic app data " + v;
			cfgApp.Settings.AppB = false;
			cfgApp.Settings.AppD = v + 0.1;
			cfgApp.Settings.AppI = v;
			cfgApp.Settings.AppIs[0] = v;

			cfgApp.SetConfigData();

			tbxMessasge.AppendText("app after" + nl);

			DisplayAppConfigData(cfgApp);

		}

		private void DisplayAppConfigData(Config<SettingsApp> cfgApp)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(cfgApp.ConfigFileName);
			sb.Append("test string | ").AppendLine(cfgApp.Settings.AppS);
			sb.Append("test bool   | ").AppendLine(cfgApp.Settings.AppB.ToString());
			sb.Append("test double | ").AppendLine(cfgApp.Settings.AppD.ToString());
			sb.Append("test int    | ").AppendLine(cfgApp.Settings.AppI.ToString());
			sb.Append("test int[0] | ").AppendLine(cfgApp.Settings.AppIs[0].ToString());
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

		private void ProcessUserConfig()
		{
			Config<SettingsUser> cfgUser = new Config<SettingsUser>();

			tbxMessasge.AppendText("user path| " + cfgUser.ConfigFileName + nl);

			tbxMessasge.AppendText(nl + "user before" + nl);

			cfgUser.GetConfigData();

			DisplayUserConfigData(cfgUser);

			cfgUser.Settings.GeneralValues.TestB = true;
			cfgUser.Settings.GeneralValues.TestD = v + 0.2;
			cfgUser.Settings.GeneralValues.TestS = "using generic setting file " + v;
			cfgUser.Settings.GeneralValues.TestI = v;
			cfgUser.Settings.GeneralValues.TestIs[1] = v;
			cfgUser.Settings.GeneralValues.TestSs[1] = "generic " + v;
			cfgUser.Settings.UnCategorizedValue = v;
			cfgUser.Settings.MainWindow.height = v * 50;
			cfgUser.Settings.MainWindow.widht = v * 100;

			cfgUser.SetConfigData();

			tbxMessasge.AppendText("user after" + nl);

			DisplayUserConfigData(cfgUser);
		}

		private void DisplayUserConfigData(Config<SettingsUser> cfgUser)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(cfgUser.ConfigFileName);
			sb.Append("test int    | ").AppendLine(cfgUser.Settings.GeneralValues.TestI.ToString());
			sb.Append("test bool   | ").AppendLine(cfgUser.Settings.GeneralValues.TestB.ToString());
			sb.Append("test double | ").AppendLine(cfgUser.Settings.GeneralValues.TestD.ToString());
			sb.Append("test string | ").AppendLine(cfgUser.Settings.GeneralValues.TestS);
			sb.Append("test int[0] | ").AppendLine(cfgUser.Settings.GeneralValues.TestIs[0].ToString());
			sb.Append("test int[1] | ").AppendLine(cfgUser.Settings.GeneralValues.TestIs[1].ToString());
			sb.Append("test str[0] | ").AppendLine(cfgUser.Settings.GeneralValues.TestSs[0]);
			sb.Append("test str[1] | ").AppendLine(cfgUser.Settings.GeneralValues.TestSs[1]);
			sb.Append("test str[2] | ").AppendLine(cfgUser.Settings.GeneralValues.TestSs[2]);
			sb.Append("uncat value | ").AppendLine(cfgUser.Settings.UnCategorizedValue.ToString());
			sb.Append("win height  | ").AppendLine(cfgUser.Settings.MainWindow.height.ToString());
			sb.Append("win width   | ").AppendLine(cfgUser.Settings.MainWindow.widht.ToString());
			
			
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

		
	}

}
