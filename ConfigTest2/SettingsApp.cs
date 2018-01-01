#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#endregion

// itemname:	SettingsApp
// username:	jeffs
// created:		12/31/2017 6:53:43 PM


namespace ConfigTest2
{
	class SettingsApp
	{
		public int AppI { get; set; } = 0;
		public bool AppB { get; set; } = false;
		public double AppD { get; set; } = 0.0;
		public string AppS { get; set; } = "this is a App";
		public int[] AppIs { get; set; } = new[] { 20, 30 };

		private static string ConfigFileName = "adf";

		public SettingsApp()
		{

		}

		internal static SettingsApp GetConfigData()
		{
			// does the file already exist?
			if (File.Exists(ConfigFileName))
			{
				// file exists - get the current values
				using (FileStream fs = new FileStream(ConfigFileName, FileMode.Open))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SettingsApp));
					SettingsApp cd = (SettingsApp) xs.Deserialize(fs);

					return cd;
				}
			}
			else
			{
				// file does not exist - create file and save default values
				using (FileStream fs = new FileStream(ConfigFileName, FileMode.Create))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SettingsApp));
					SettingsApp cd = new SettingsApp();
					xs.Serialize(fs, cd);

					return cd;
				}
			}
		}

		public static bool SetConfigData(SettingsApp cd)
		{
			if (File.Exists(ConfigFileName))
			{
				// file exists - process
				using (FileStream fs = new FileStream(ConfigFileName, FileMode.Open))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SettingsApp));
					xs.Serialize(fs, cd);
				}

				return true;
			}

			return false;
		}
	}
}
