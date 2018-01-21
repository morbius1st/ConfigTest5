#region Using directives
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
using ConfigTest5;

#endregion

// itemname:	Config
// username:	jeffs
// created:		12/30/2017 4:42:00 PM


namespace ConfigTest5
{
	[DataContract]
	public class Header
	{
		public Header(string settingFileVersion)
		{
			SettingFileVersion = settingFileVersion;
		}
		[DataMember(Order = 1)]
		public string SaveDateTime = System.DateTime.Today.ToString("G");
		[DataMember(Order = 2)]
		public string AssemblyVersion = SettingsUtil.AssemblyVersion;
		[DataMember(Order = 3)]
		public string SettingSystemVersion = "2.2";
		[DataMember(Order = 4)]
		public string SettingFileVersion;
	}


	public static class SettingsUser
	{
		public static readonly SettingsBase<UserSettings> Usettings;

		public static readonly UserSettings USet;

		static SettingsUser()
		{
			Usettings = new SettingsBase<UserSettings>();
			USet = Usettings.Settings;
		}
	}

	public static class SettingsApp
	{
		public static readonly SettingsBase<AppSettings> ASettings;

		public static readonly AppSettings ASet;

		static SettingsApp()
		{
			ASettings = new SettingsBase<AppSettings>();
			ASet = ASettings.Settings;
		}
	}

	public class SettingsBase<T> where T : SettingsPathFileBase, new()
	{
		public T Settings { get; private set; }

		public string SettingsPathAndFile { get; private set; }

		public SettingsBase()
		{
			SettingsPathAndFile = (new T()).SettingsPathAndFile;

			Read();
		}

		public void Reset()
		{
			Settings = new T();
		}

		private void Read()
		{
			// does the file already exist?
			if (File.Exists(SettingsPathAndFile))
			{
				try
				{
					DataContractSerializer ds = new DataContractSerializer(typeof(T));

					// file exists - get the current values
					using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Open))
					{
						Settings = (T) ds.ReadObject(fs);
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
				Settings = new T();
				Save();
			
			}
		}

		public void Save()
		{
			XmlWriterSettings xmlSettings = new XmlWriterSettings() { Indent = true };

			DataContractSerializer ds = new DataContractSerializer(typeof(T));

			using (XmlWriter w = XmlWriter.Create(SettingsPathAndFile, xmlSettings))
			{
				ds.WriteObject(w, Settings);
			}
		}
	}


	public static class SettingsUtil
	{
		internal static string AssemblyName => typeof(SettingsUtil).Assembly.GetName().Name;

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
			get { return typeof(SettingsUtil).Assembly.GetName().Version.ToString(); }
		}

		internal static bool CreateSubFolders(string RootPath, string[] SubFolders)
		{
			if (!Directory.Exists(RootPath)) { return false; }

			for (int i = 0; i < SubFolders.Length; i++)
			{
				string path = SubFolder(i, RootPath, SubFolders);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}

			return true;
		}

		// create the folder path if needed
		public static bool CreateFolders(string rootPath, string[] subFolders)
		{
			if (subFolders == null) return true;

			return SettingsUtil.CreateSubFolders(rootPath, subFolders);
		}

		internal static string SubFolder(int i, string rootPath, string[] subFolders)
		{
			if (i < 0 ||
				i >= subFolders.Length) return null;

			string path = rootPath;
			for (int j = 0; j < i + 1; j++)
			{
				path += "\\" + subFolders[j];
			}

			return path;
		}
	}

	[DataContract]
	public abstract class SettingsPathFileBase
	{
		protected string FileName;
		protected string RootPath;
		protected string[] SubFolders;

		public const string SETTINGFILEBASE = @".setting.xml";

		[DataMember]
		public abstract Header Header { get; set; }

		// get the count of sub-folders
		private int SubFolderCount => SubFolders.Length;


		// get the path to the setting file
		public string SettingsPath
		{
			get
			{
				if (SubFolders == null)
				{
					return RootPath;
				}
				return SettingsUtil.SubFolder(SubFolders.Length - 1,
					RootPath, SubFolders);
			}
		}

		// get the path and the file name for the setting file
		public string SettingsPathAndFile
		{
			get
			{
				if (!Directory.Exists(SettingsPath))
				{
					if (!SettingsUtil.CreateFolders(RootPath, SubFolders))
					{
						throw new DirectoryNotFoundException("setting file path");
					}
				}
				return SettingsPath + "\\" + FileName;
			}
		}
	}

	[DataContract]
	public class SettingsPathFileUserBase : SettingsPathFileBase
	{
		public override Header Header { get; set; } = new Header(UserSettings.USERSETTINGFILEVERSION);

		public SettingsPathFileUserBase()
		{
			FileName = @"user" + SETTINGFILEBASE;

			RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			SubFolders = new[] {
				SettingsUtil.CompanyName,
				SettingsUtil.AssemblyName };
		}
	}

	[DataContract]
	public class SettingsPathFileAppBase : SettingsPathFileBase
	{
		public override Header Header { get; set; } = new Header(AppSettings.APPSETTINGFILEVERSION);

		public SettingsPathFileAppBase()
		{
			FileName = SettingsUtil.AssemblyName + SETTINGFILEBASE;
			RootPath = SettingsUtil.AssemblyDirectory;
			SubFolders = null;
		}
	}

	// sample setting clases

	// sample app Settings file:
	//
	//	[DataContract(Name = "AppSettings")]
	//	public class AppSettings : SettingsPathFileAppBase
	//	{
	//		public const string APPSETTINGFILEVERSION = "2.0";
	//
	//		[DataMember(Order = 1)]
	//		public int AppI { get; set; } = 0;
	//
	//		[DataMember(Order = 2)]
	//		public bool AppB { get; set; } = false;
	//
	//		[DataMember(Order = 3)]
	//		public double AppD { get; set; } = 0.0;
	//
	//		[DataMember(Order = 4)]
	//		public string AppS { get; set; } = "this is an App";
	//
	//		[DataMember(Order = 5)]
	//		public int[] AppIs { get; set; } = new[] { 20, 30 };
	//
	//	}
	//
	//
	// sample user Settings file (complex)
	//
	//	[DataContract(Name = "UserSettings")]
	//	public class UserSettings : SettingsPathFileUserBase
	//	{
	//		public const string USERSETTINGFILEVERSION = "2.0";
	//
	//		[DataMember]
	//		public int UnCategorizedValue = 1000;
	//		[DataMember]
	//		public int UnCategorizedValue2 = 2000;
	//		[DataMember]
	//		public generalValues GeneralValues = new generalValues();
	//		[DataMember]
	//		public window1 MainWindow { get; set; } = new window1();
	//
	//		[DataMember(Name = "DictionaryTest3")]
	//		public CustDict<string, testStruct> testDictionary3 =
	//			new CustDict<string, testStruct>()
	//			{
	//				{"one", new testStruct(1, 2, 3)},
	//				{"two", new testStruct(1, 2, 3)},
	//				{"three", new testStruct(1, 2, 3)}
	//			};
	//	}
	//
	//	public struct testStruct
	//	{
	//		[DataMember(Name = "line1")]
	//		public int intA;
	//		[DataMember(Name = "line2")]
	//		public int intB;
	//		[DataMember(Name = "line3")]
	//		public int intC;
	//
	//		public testStruct(int a, int b, int c)
	//		{
	//			intA = a;
	//			intB = b;
	//			intC = c;
	//		}
	//	}
	//
	//	[CollectionDataContract(Name = "CustomDict", KeyName = "key", ValueName = "data", ItemName = "row")]
	//	public class CustDict<T1, T2> : Dictionary<T1, T2>
	//	{
	//	}
	//
	//	public class window1
	//	{
	//		public int height = 50;
	//		public int width = 100;
	//	}
	//
	//	public class generalValues
	//	{
	//		public int TestI = 0;
	//		public bool TestB = false;
	//		public double TestD = 0.0;
	//		public string TestS = "this is a test";
	//		public int[] TestIs = new[] { 20, 30 };
	//		public string[] TestSs = new[] { "user 1", "user 2", "user 3" };
	//	}
}

