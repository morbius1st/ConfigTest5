#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ConfigTest2.Properties;

#endregion

// itemname:	Config
// username:	jeffs
// created:		12/30/2017 4:42:00 PM


namespace ConfigTest2
{
	class Config
	{
		private static FileData fd2;
		internal static string UserConfigFile { get; private set; }
		internal static string AppConfigFile { get; private set; }

		static Config()
		{
			fd2 = new FileData();
			UserConfigFile = fd2.UserConfigFile;
			AppConfigFile = fd2.AppConfigFile;
		}

		internal static SettingsUser GetConfigData()
		{
			// does the file already exist?
			if (File.Exists(UserConfigFile))
			{
				// file exists - get the current values
				using (FileStream fs = new FileStream(UserConfigFile, FileMode.Open))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SettingsUser));
					SettingsUser cd = (SettingsUser) xs.Deserialize(fs);

					return cd;
				}
			}
			else
			{
				// file does not exist - create file and save default values
				using (FileStream fs = new FileStream(UserConfigFile, FileMode.Create))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SettingsUser));
					SettingsUser cd = new SettingsUser();
					xs.Serialize(fs, cd);

					return cd;
				}
			}
		}

		public static bool SetConfigData(SettingsUser cd)
		{
			if (File.Exists(UserConfigFile))
			{
				// file exists - process
				using (FileStream fs = new FileStream(UserConfigFile, FileMode.Open))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SettingsUser));
					xs.Serialize(fs, cd);
				}

				return true;
			}

			return false;
		}
		
	}
	
	public class FileData
	{
		private CfgPathUser cfgUser;
		private CfgPathApp cfgApp;

		private const string userConfigFile = @"user.config.xml";
		private const string appConfigFile = @"app.config.xml";

		public FileData()
		{
			cfgUser = new CfgPathUser();
			cfgApp = new CfgPathApp();
		}

		public string AppConfigFile
		{
			get
			{
				if (Directory.Exists(cfgApp.ConfigPath))
				{
					return cfgApp.ConfigPath + "\\" + appConfigFile;
				}

				throw new DirectoryNotFoundException("app configuration");
			}
		}

		public string UserConfigFile
		{
			get
			{
				if (!Directory.Exists(cfgUser.ConfigPath))
				{
					if (!CreateUserConfigFolder())
					{
						throw new DirectoryNotFoundException("user configuration");
					}
				}

				return cfgUser.ConfigPath + "\\" + userConfigFile;
			}
		}

		private bool CreateUserConfigFolder()
		{
			if (!Directory.Exists(cfgUser.RootPath)) return false;

			string path = cfgUser.RootPath;

			for (int i = 0; i < cfgUser.SubFolderCount; i++)
			{
				path = cfgUser.SubFolder(i);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}

			return true;
		}


		private class CfgPathUser : ConfigPathData
		{
			public string AssemblyName { get; }
			public string CompanyName { get; }

			public CfgPathUser()
			{
				rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

				AssemblyName = typeof(Config).Assembly.GetName().Name;

				object[] att = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (att.Length > 0)
				{
					CompanyName = ((AssemblyCompanyAttribute) att[0]).Company;
				}
				else
				{
					throw new MissingFieldException("Company is Missing from Assembly Information");
				}

				subFolders = new[] { CompanyName, AssemblyName};
			}
		}

		private class CfgPathApp : ConfigPathData
		{
			public CfgPathApp()
			{
				rootPath = AssemblyDirectory;
				subFolders = null;
			}

			private string AssemblyDirectory
			{
				get
				{
					string codebase = Assembly.GetExecutingAssembly().CodeBase;
					UriBuilder uri = new UriBuilder(codebase);
					string path = Uri.UnescapeDataString(uri.Path);
					return Path.GetDirectoryName(path);
				}
			}
		}
	}
	
}

