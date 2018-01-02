#region Using directives
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

#endregion

// itemname:	Config
// username:	jeffs
// created:		12/30/2017 4:42:00 PM


namespace ConfigTest2
{

	public class Settings<T> where T : SettingsBase, new()
	{
		internal T Setting { get; private set; }

		internal string SettingsPathAndFile { get; private set; }

		public Settings()
		{
			SettingsPathAndFile = (new T()).SettingsPath.SettingsPathAndFile;
			Read();
		}

		private void Read()
		{
			// does the file already exist?
			if (File.Exists(SettingsPathAndFile))
			{
				try
				{
					// file exists - get the current values
					using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Open))
					{
						XmlSerializer xs = new XmlSerializer(typeof(T));
						Setting = (T) xs.Deserialize(fs);
					}
				}
				catch (Exception e)
				{
					throw new Exception("Cannot read setting data for file:\n"
						+ SettingsPathAndFile + "\n"
						+ e.Message);
				}
			}
			else
			{
				// file does not exist - create file and save default values
				using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Create, FileAccess.ReadWrite))
				{
					XmlSerializer xs = new XmlSerializer(typeof(T));
					Setting = new T();
					xs.Serialize(fs, Setting);
				}
			}
		}

		public void Save()
		{
			if (!File.Exists(SettingsPathAndFile))
			{
				throw new FileNotFoundException(SettingsPathAndFile);
			}
			// file exists - process
			using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Open))
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));

				Setting.VersionInfo.AssemblyVersion = SettingsUtil.AssemblyVersion;
				Setting.VersionInfo.SettingFileVersion = Setting.SETTINGFILEVERSION;
				Setting.VersionInfo.SettingSystemVersion = versionInfo.SETTINGSYSTEMVERSION;

				xs.Serialize(fs, Setting);
			}
		}
	}

	public static class SettingsUtil
	{
		internal static string AssemblyName => typeof(Program).Assembly.GetName().Name;

		internal static string CompanyName
		{
			get
			{
				object[] att = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (att.Length > 0)
				{
					return ((AssemblyCompanyAttribute) att[0]).Company;
				}

				throw new MissingFieldException("Company is Missing from Assembly Information");
			}
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

		internal static string AssemblyVersion
		{
			get
			{
				return typeof(Program).Assembly.GetName().Version.ToString();

			}
		}
}

	public abstract class SettingsPathBase
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

		protected abstract bool CreateFolders();

		protected string SettingsPath
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

		public string SettingsPathAndFile
		{
			get
			{
				if (!Directory.Exists(SettingsPath))
				{
					if (!CreateFolders())
					{
						throw new DirectoryNotFoundException("setting file path");
					}
				}

				return SettingsPath + "\\" + FileName;
			}
		}
	}

	public abstract class SettingsBase
	{
		public const string SETTINGFILEBASE = @".setting.xml";
		public abstract string SETTINGFILEVERSION { get; }

		public abstract SettingsPathBase SettingsPath { get; }

		public versionInfo VersionInfo = new versionInfo();
	}

	public class versionInfo
	{
		public const string SETTINGSYSTEMVERSION = "1.0.0.0";

		[XmlAttribute]
		public string SettingFileVersion = "0.0.0.0";
		[XmlAttribute]
		public string AssemblyVersion = SettingsUtil.AssemblyVersion;
		[XmlAttribute]
		public string SettingSystemVersion = SETTINGSYSTEMVERSION;
	}

	// classes are not serialized
	class SettingsPathApp : SettingsPathBase
	{
		public SettingsPathApp()
		{
			FileName = SettingsUtil.AssemblyName + SettingsBase.SETTINGFILEBASE;
			RootPath = SettingsUtil.AssemblyDirectory;
			SubFolders = null;
		}

		protected override bool CreateFolders()
		{
			return true;
		}
	}

	class SettingsPathUser : SettingsPathBase
	{
		public string AssemblyName { get; }
		public string CompanyName { get; }

		public SettingsPathUser()
		{
			FileName = @"user" + SettingsBase.SETTINGFILEBASE;

			RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			AssemblyName = SettingsUtil.AssemblyName;
			CompanyName = SettingsUtil.CompanyName;

			SubFolders = new[] { CompanyName, AssemblyName };
		}

		protected override bool CreateFolders()
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

