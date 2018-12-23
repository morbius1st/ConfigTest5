#region Using directives
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

#endregion

// itemname:	Config
// username:	jeffs
// created:		12/30/2017 4:42:00 PM

//	ver 1.0		initial version
//	ver 2.0		revise to use DataContract
//	ver 2.1		refine use fewer classes / abstract classes
//	ver	2.2		refine
//	ver 2.3		refine move utility methods to library file
//	ver 2.4		move setting file specific info out of base file
//	ver 2.4.1	revise date format


namespace SettingsManager
{
	public static class SettingsUser
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsBase<UserSettings> USettings { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static UserSettings USet { get; private set; }

		// initalize and create the setting objects
		static SettingsUser()
		{
			USettings = new SettingsBase<UserSettings>();
			USet = USettings.Settings;
			USet.Header = new Header(UserSettings.USERSETTINGFILEVERSION);
			USettings.ResetClass = ResetClass;
		}

		public static void ResetClass()
		{
			USet = USettings.Settings;
		}

	}

	public static class SettingsApp
	{
		public static SettingsBase<AppSettings> ASettings { get; private set; }

		public static AppSettings ASet { get; private set; }

		static SettingsApp()
		{
			ASettings = new SettingsBase<AppSettings>();
			ASet = ASettings.Settings;
			ASet.Header = new Header(AppSettings.APPSETTINGFILEVERSION);
			ASettings.ResetClass = ResetClass;
		}

		public static void ResetClass()
		{
			ASet = ASettings.Settings;
		}
	}

	[DataContract]
	public class Header
	{
		public Header(string settingFileVersion)
		{
			SettingFileVersion = settingFileVersion;
		}
		[DataMember(Order = 1)]
		public string SaveDateTime = DateTime.Now.ToString("yyyy-MM-dd - HH:mm zzz");
		[DataMember(Order = 2)]
		public string AssemblyVersion = CsUtilities.AssemblyVersion;
		[DataMember(Order = 3)]
		public string SettingSystemVersion = "2.4.1";
		[DataMember(Order = 4)]
		public string SettingFileVersion;
	}

	public class SettingsBase2<T> where T : SettingsPathFileBase, new()
	{
		public T Settings { get; private set; }

		public string SettingsPathAndFile { get; private set; }

		public SettingsBase2()
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

	public delegate void RstClass();

	public class SettingsBase<T> where T : SettingsPathFileBase, new()
	{
		public T Settings { get; private set; }

		public string SettingsPathAndFile { get; private set; }

		public RstClass ResetClass { private get; set; }

		public SettingsBase()
		{
			SettingsPathAndFile = (new T()).SettingsPathAndFile;

			Read();
		}

		public void Reset()
		{
			Settings = new T();

			ResetClass?.Invoke();
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
					throw new MessageException("Cannot read setting data for file:" + nl
						+ SettingsPathAndFile, e);
				}
			}
			else
			{
				Settings = new T();
				Save();

			}
		}

		// using DataContractSerializer
		public void Save()
		{
			XmlWriterSettings xmlSettings = new XmlWriterSettings() { Indent = true };

			DataContractSerializer ds = new DataContractSerializer(typeof(T));

			using (XmlWriter w = XmlWriter.Create(SettingsPathAndFile, xmlSettings))
			{
				ds.WriteObject(w, Settings);
			}
		}


		// using NetDataContractSerializer
		public void Save2()
		{
			using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Create))
			{
				XmlWriterSettings s = new XmlWriterSettings();
				s.Indent = true;
				XmlWriter w = XmlDictionaryWriter.Create(fs, s);

				NetDataContractSerializer ns = new NetDataContractSerializer();

				logMsgDbLn2("max items| ", ns.MaxItemsInObjectGraph);

				ns.WriteObject(w, Settings);
				w.Flush();
				w.Close();
			}
		}
	}



	[DataContract]
	public abstract class SettingsPathFileBase
	{
		protected string FileName;
		protected string RootPath;
		protected string[] SubFolders;

		protected const string SETTINGFILEBASE = @".setting.xml";

		[DataMember]
		public Header Header { get; set; }

		// get the path to the setting file
		public string SettingsPath
		{
			get
			{
				if (SubFolders == null)
				{
					return RootPath;
				}
				return CsUtilities.SubFolder(SubFolders.Length - 1,
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
					if (!CsUtilities.CreateSubFolders(RootPath, SubFolders))
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
//		public override Header Header { get; set; } = new Header(UserSettings.USERSETTINGFILEVERSION);

		public SettingsPathFileUserBase()
		{
			FileName = @"user" + SETTINGFILEBASE;

			RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			SubFolders = new[] {
				CsUtilities.CompanyName,
				CsUtilities.AssemblyName };
		}
	}

	[DataContract]
	public class SettingsPathFileAppBase : SettingsPathFileBase
	{
//		public override Header Header { get; set; } = new Header(AppSettings.APPSETTINGFILEVERSION);

		public SettingsPathFileAppBase()
		{
			FileName = CsUtilities.AssemblyName + SETTINGFILEBASE;
			RootPath = CsUtilities.AssemblyDirectory;
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

