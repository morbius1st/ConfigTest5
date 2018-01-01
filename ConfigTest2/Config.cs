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

	public class Config<T> where T : ASetting, new()
	{
		internal T Settings { get; private set; }

		internal string ConfigFileName { get; private set; }

		public Config()
		{
			ConfigFileName = (new T()).SettingFileAndPath;
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
				// file does not exist - create file and save default values
				using (FileStream fs = new FileStream(ConfigFileName, FileMode.Create, FileAccess.ReadWrite))
				{
					XmlSerializer xs = new XmlSerializer(typeof(T));
					Settings = new T();
					xs.Serialize(fs, Settings);
				}
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
	}

	public static class ConfigUtil
	{
		internal static string GetAssemblyName()
		{
			return typeof(Program).Assembly.GetName().Name;
		}

		internal static string GetCompanyName()
		{
			object[] att = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
			if (att.Length > 0)
			{
				return ((AssemblyCompanyAttribute) att[0]).Company;
			}

			throw new MissingFieldException("Company is Missing from Assembly Information");
		}

		internal static string AssemblyDirectory
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

	public abstract class AConfigPathData
	{
		protected string FileName;

		protected string RootPath;
		protected string[] SubFolders;

		private int SubFolderCount => SubFolders.Length;

		protected string SubFolder(int i)
		{
			if (i < 0 ||
				i >= SubFolderCount) return null;

			string path = RootPath;
			for (int j = 0; j < i + 1; j++)
			{
				path += "\\" + SubFolders[j];
			}

			return path;
		}

		protected abstract string ConfigFileName();

		protected abstract bool CreateUserConfigFolder();

		protected string ConfigPath
		{
			get
			{
				if (SubFolders == null)
				{
					return RootPath;
				}

				return SubFolder(SubFolders.Length - 1);
			}
		}

		public string FileNameAndPath
		{
			get
			{
				if (!Directory.Exists(ConfigPath))
				{
					if (!CreateUserConfigFolder())
					{
						throw new DirectoryNotFoundException("configuration file path");
					}
				}

				return ConfigFileName();
			}
		}
	}

	public abstract class ASetting
	{
		protected abstract AConfigPathData CfgFile { get; }

		public abstract string SettingFileAndPath { get; }
	}


}

