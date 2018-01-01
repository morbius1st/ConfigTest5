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

		private void ProcessAppConfig()
		{
			Config<CfgPathApp, SettingsApp> cfgApp = new Config<CfgPathApp, SettingsApp>();

			tbxMessasge.AppendText(" app path| " + cfgApp.ConfigFileName + nl);

			tbxMessasge.AppendText("app before" + nl);

			cfgApp.GetConfigData();

			DisplayAppConfigData(cfgApp);

			cfgApp.Settings.AppS = "generic app data 2";
			cfgApp.Settings.AppB = false;
			cfgApp.Settings.AppD = 2.1;
			cfgApp.Settings.AppI = 2;
			cfgApp.Settings.AppIs[0] = 2;

			cfgApp.SetConfigData();

			tbxMessasge.AppendText("app after" + nl);

			DisplayAppConfigData(cfgApp);

		}

		private void DisplayAppConfigData(Config<CfgPathApp, SettingsApp> cfgApp)
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
			Config<CfgPathUser, SettingsUser> cfgUser = new Config<CfgPathUser, SettingsUser>();

			tbxMessasge.AppendText("user path| " + cfgUser.ConfigFileName + nl);

			tbxMessasge.AppendText(" asm name| " + cfgUser.FileData.ConfigPathInfo.AssemblyName + nl);
			tbxMessasge.AppendText("  co name| " + cfgUser.FileData.ConfigPathInfo.CompanyName + nl);

			tbxMessasge.AppendText("user before" + nl);

			cfgUser.GetConfigData();

			DisplayUserConfigData(cfgUser);

			cfgUser.Settings.TestB = true;
			cfgUser.Settings.TestD = 4.0;
			cfgUser.Settings.TestS = "using generic setting file 4";
			cfgUser.Settings.TestSs[1] = "generic 4";

			cfgUser.SetConfigData();

			tbxMessasge.AppendText("user after" + nl);

			DisplayUserConfigData(cfgUser);
		}



		private void DisplayUserConfigData(Config<CfgPathUser, SettingsUser> cfgUser)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(cfgUser.ConfigFileName);
			sb.Append("test int    | ").AppendLine(cfgUser.Settings.TestI.ToString());
			sb.Append("test bool   | ").AppendLine(cfgUser.Settings.TestB.ToString());
			sb.Append("test double | ").AppendLine(cfgUser.Settings.TestD.ToString());
			sb.Append("test string | ").AppendLine(cfgUser.Settings.TestS);
			sb.Append("test int[0] | ").AppendLine(cfgUser.Settings.TestIs[0].ToString());
			sb.Append("test int[1] | ").AppendLine(cfgUser.Settings.TestIs[1].ToString());
			sb.Append("test str[0] | ").AppendLine(cfgUser.Settings.TestSs[0]);
			sb.Append("test str[1] | ").AppendLine(cfgUser.Settings.TestSs[1]);
			sb.Append("test str[2] | ").AppendLine(cfgUser.Settings.TestSs[2]);
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}

		
	}

}
