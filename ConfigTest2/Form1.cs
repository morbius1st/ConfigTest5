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

			FileData fd2 = new FileData();

			Debug.Print(" app path| " + fd2.AppConfigFile);
			Debug.Print("user path| " + fd2.UserConfigFile);

			//			Config cx = new Config();

			// test the process

			SettingsUser settingsUser = Config.GetConfigData();

			this.tbxMessasge.Text = "before" + nl;

			DisplayConfigData(settingsUser);

			settingsUser.TestB = false;
			settingsUser.TestD = 2.0;
			settingsUser.TestS = "testing 1, 2, 3, 4, 5, 6, 7";
			settingsUser.TestSs[1] = "revision 1";

			Config.SetConfigData(settingsUser);

			settingsUser = null;

			settingsUser = Config.GetConfigData();

			this.tbxMessasge.AppendText("after" + nl);

			DisplayConfigData(settingsUser);


//			string a = Properties.Settings.Default.AppConfigInfo;


		}

		private void DisplayConfigData(SettingsUser cd)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("file name   | ").AppendLine(Config.UserConfigFile);
			sb.Append("test int    | ").AppendLine(cd.TestI.ToString());
			sb.Append("test bool   | ").AppendLine(cd.TestB.ToString());
			sb.Append("test double | ").AppendLine(cd.TestD.ToString());
			sb.Append("test string | ").AppendLine(cd.TestS);
			sb.Append("test int[0] | ").AppendLine(cd.TestIs[0].ToString());
			sb.Append("test int[1] | ").AppendLine(cd.TestIs[1].ToString());
			sb.Append("test str[0] | ").AppendLine(cd.TestSs[0]);
			sb.Append("test str[1] | ").AppendLine(cd.TestSs[1]);
			sb.Append("test str[2] | ").AppendLine(cd.TestSs[2]);
			sb.Append(nl).Append(nl);

			tbxMessasge.AppendText(sb.ToString());
		}
	}

}
