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
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;
using ConfigTest2.Properties;

#endregion

// itemname:	Config
// username:	jeffs
// created:		12/30/2017 4:42:00 PM


namespace ConfigTest2
{
	public class Config<U, T> where T: new() where U : ConfigPathData, new() 
	{
		internal readonly FileData<U> FileData;

		internal T Settings { get; private set; }

		internal string ConfigFileName { get; private set; }

		public Config()
		{
			FileData = new FileData<U>();

			ConfigFileName = FileData.ConfigFileName;
		}

		internal void GetConfigData()
		{
			// does the file already exist?
			if (File.Exists(ConfigFileName))
			{
				try
				{
					// file exists - get the current values
					using (FileStream fs = new FileStream(ConfigFileName, FileMode.Open))
					{
						XmlSerializer xs = new XmlSerializer(typeof(T));
						Settings = (T) xs.Deserialize(fs);
					}

				}
				catch (Exception e)
				{
					
					throw new Exception("Cannot get configuration data for file:\n"
						+ ConfigFileName + "\n"
						+ e.Message);
				}
			}
			else
			{
				CreateConfigFile();

//				// file does not exist - create file and save default values
//				using (FileStream fs = new FileStream(ConfigFileName, FileMode.Create, FileAccess.ReadWrite))
//				{
//					XmlSerializer xs = new XmlSerializer(typeof(T));
//					Settings = new T();
//					xs.Serialize(fs, Settings);
//				}
			}
		}

		public bool SetConfigData()
		{
			if (File.Exists(ConfigFileName))
			{
				// file exists - process
				using (FileStream fs = new FileStream(ConfigFileName, FileMode.Open))
				{
					XmlSerializer xs = new XmlSerializer(typeof(T));
					xs.Serialize(fs, Settings);
				}

				return true;
			}

			return false;
		}

		private void CreateConfigFile()
		{
			// file does not exist - create file and save default values
			using (FileStream fs = new FileStream(ConfigFileName, FileMode.Create, FileAccess.ReadWrite))
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));
				Settings = new T();
				xs.Serialize(fs, Settings);
			}
		}
	}
	

	public class FileData<T> where T : ConfigPathData, new()
	{
		internal readonly T ConfigPathInfo;

		public FileData()
		{
			ConfigPathInfo = new T();
		}

		internal string ConfigFileName
		{
			get
			{
				if (!Directory.Exists(ConfigPathInfo.ConfigPath))
				{
					if (!ConfigPathInfo.CreateUserConfigFolder())
					{
						throw new DirectoryNotFoundException("configuration file path");
					}
				}
				return ConfigPathInfo.ConfigFileName();
			}
		}
	}

	class CfgPathUser : ConfigPathData
	{
		public string AssemblyName { get; }
		public string CompanyName { get; }

		public CfgPathUser()
		{
			fileName = @"user.config.xml";

			rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			AssemblyName = typeof(Program).Assembly.GetName().Name;

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

		public override string ConfigFileName()
		{
			return ConfigPath + "\\" + fileName;
		}

		public override bool CreateUserConfigFolder()
		{
			if (!Directory.Exists(rootPath)) { return false; }

			for (int i = 0; i < subFolders.Length; i++)
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

	class CfgPathApp : ConfigPathData
	{
		public CfgPathApp()
		{
			fileName = @"app.config.xml";
			rootPath = AssemblyDirectory;
			subFolders = null;
		}

		public override string ConfigFileName()
		{
			if (Directory.Exists(ConfigPath))
			{
				return ConfigPath + "\\" + fileName;
			}
			return "";
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

		public override bool CreateUserConfigFolder()
		{
			return true;
		}
	}
	
	
}

