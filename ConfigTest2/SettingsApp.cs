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

// itemname:	SettingsApp
// username:	jeffs
// created:		12/31/2017 6:53:43 PM


namespace ConfigTest2
{
	public class SettingsApp : ASetting
	{
		public int AppI { get; set; } = 0;
		public bool AppB { get; set; } = false;
		public double AppD { get; set; } = 0.0;
		public string AppS { get; set; } = "this is a App";
		public int[] AppIs { get; set; } = new[] {20, 30};

		public SettingsApp()
		{
		}

		protected override AConfigPathData CfgFile { get; } = new CfgPathApp();

		public override string SettingFileAndPath => CfgFile.FileNameAndPath;

		class CfgPathApp : AConfigPathData
		{
			public CfgPathApp()
			{
				FileName = ConfigUtil.GetAssemblyName() + @".config.xml";
				RootPath = ConfigUtil.AssemblyDirectory;
				SubFolders = null;
			}

			protected override string ConfigFileName()
			{
				if (Directory.Exists(ConfigPath))
				{
					return ConfigPath + "\\" + FileName;
				}
				return "";
			}

			protected override bool CreateUserConfigFolder()
			{
				return true;
			}
		}
	}
}
