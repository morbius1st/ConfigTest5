#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		12/31/2017 8:29:37 AM


namespace ConfigTest2
{
	public class SettingsUser : ASetting
	{
		public int UnCategorizedValue = 10;
		public generalValues GeneralValues = new generalValues();
		public window1 MainWindow { get; set; } = new window1();

		protected override AConfigPathData CfgFile { get; } = new CfgPathUser();

		public override string SettingFileAndPath => CfgFile.FileNameAndPath;

		public SettingsUser()
		{
			MainWindow.height = 50;
			MainWindow.widht = 100;
		}

		public class window1
		{
			public int height;
			public int widht;
		}

		public class generalValues
		{
			public int TestI = 0;
			public bool TestB = false;
			public double TestD = 0.0;
			public string TestS = "this is a test";
			public int[] TestIs = new[] {20, 30};
			public string[] TestSs = new[] {"user 1", "user 2", "user 3"};
		}

		class CfgPathUser : AConfigPathData
		{
			public string AssemblyName { get; }
			public string CompanyName { get; }

			public CfgPathUser()
			{
				FileName = @"user.config.xml";

				RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

				AssemblyName = ConfigUtil.GetAssemblyName();
				CompanyName = ConfigUtil.GetCompanyName();

				SubFolders = new[] {CompanyName, AssemblyName};
			}

			protected override string ConfigFileName()
			{
				return ConfigPath + "\\" + FileName;
			}

			protected override bool CreateUserConfigFolder()
			{
				if (!Directory.Exists(RootPath)) { return false; }

				for (int i = 0; i < SubFolders.Length; i++)
				{
					string path = SubFolder(i);

					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				}

				return true;
			}
		}
	}
}
