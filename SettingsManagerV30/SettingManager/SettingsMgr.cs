#region  
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using static SettingManager.SettingMgrStatus;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities2;

#endregion

// requires a reference to:
// System.Runtime.Serialization
// System.Xaml


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
//	ver 2.5		move static setting classes into base file
//				added delegate to properly reset data
//	ver 2.6		incorporate [OnDeserializing] and proper default values

// standard settings manager

namespace SettingManager
{
	#region + Header

	[DataContract(Namespace = NSpace)]
	//	[DataContract]
	public class Heading
	{
		public static string ClassVersionName = nameof(ClassVersion);
		public static string SystemVersionName = nameof(SystemVersion);

		public enum SettingFileType
		{
			APP = 0,
			USER = 1,
			LENGTH = 2
		}

		public const string NSpace = "";

		public Heading(string classVersion)
		{
			ClassVersion = classVersion;
		}

		[DataMember(Order = 1)] public string SaveDateTime         = DateTime.Now.ToString("yyyy-MM-dd - HH:mm zzz");
		[DataMember(Order = 2)] public string AssemblyVersion      = CsUtilities.AssemblyVersion;
		[DataMember(Order = 3)] public string SystemVersion = "3.0";
		[DataMember(Order = 4)] public string ClassVersion;
		[DataMember(Order = 5)] public string Notes = "created by v3.0";


//		public static string[] ClassVersionFromFile;
//		public static string[] SystemVersionFromFile;
//
//		public bool ClassVersionsMatch => 
//			ClassVersion.Equals(ClassVersionFromFile[]);
	}

	#endregion

	public enum SettingMgrStatus
	{
		DOESNOTEXIST = -2,
		NOPATH = -1,
		
		CONSTRUCTED = 0,
		INITIALIZED,
		CREATED,
		READ,
		SAVED,
		EXISTS
	}

	public interface ITest
	{
		void Read();
		dynamic Read(Type type);
		void Save();
	}

	public delegate void RstData();

	public class SettingsMgr<T> : ITest
		where T : SettingBase, new()
	{
		#region + Constructor

//		public SettingsMgr(RstData rst, [CallerMemberName] string  name = "")
		public SettingsMgr(RstData rst)
		{
			Status = CONSTRUCTED;

			if (String.IsNullOrWhiteSpace(Info.SettingPathAndFile))
			{
				Status = NOPATH;
			}

			logMsgLn2();
			logMsgLn2("at ctor SettingsMgr", "status| " + Status +
				" file type| " + Info.FileType.ToString()
				);

			ResetData = rst;
		}

		public void initialize()
		{
			if (Status != NOPATH)
			{
				Status = INITIALIZED;
				logMsgLn2();
				logMsgLn2("at SettingsMgr initialize", "status| " + Status +
					" file type| " + Info.FileType.ToString()
					);

				if (FileExists())
				{
					Upgrade();
				}
			}
		}

		#endregion

		#region + Properties

		public T Info { get; private set; } = new T();

		public SettingMgrStatus Status { get; private set; }

//		public static bool Exists { get; }

//		private string SettingPathAndFile { get; set; }
//		public string SaveDateTime => Info?.Header.SaveDateTime ?? "undefined";
//		public string AssemblyVersion => Info?.Header.AssemblyVersion ?? "undefined";
//		public string SettingFileNotes => Info?.Header.Notes ?? "undefined";

		#endregion

		#region + Read

		public void Read()
		{
			// does the file already exist?
			if (FileExists())
			{
				if (Info.ClassVersionsMatch)
				{
					try
					{
						DataContractSerializer ds = new DataContractSerializer(typeof(T));

						// file exists - get the current values
						using (FileStream fs = new FileStream(Info.SettingPathAndFile, FileMode.Open))
						{
							Info = (T) ds.ReadObject(fs);
							SyncData();
						}

						Status = READ;
					}
					// catch malformed XML data file and replace with default
					catch (System.Runtime.Serialization.SerializationException)
					{
						Create();
						Save();
					}
					// catch any other errors
					catch (Exception e)
					{
						throw new MessageException(MessageUtilities.nl
							+ "Cannot read setting data for file: "
							+ Info.SettingPathAndFile + MessageUtilities.nl
							+ "("                     + e.Message + ")" + MessageUtilities.nl
							, e.InnerException);
					}
				}
				else
				{
					Upgrade();
				}
			}
			else
			{
				Create();
				Save();
			}

//			SetFileStatus();
		}

		public dynamic Read(Type type)
		{
			dynamic p = null;

			if (FileExists())
			{
				try
				{
					DataContractSerializer ds = new DataContractSerializer(type);

					using (FileStream fs = new FileStream(Info.SettingPathAndFile, FileMode.Open))
					{
						p = ds.ReadObject(fs);
					}

				}
				catch { }
			}

			return p;
		}

		#endregion

		#region + Create

		// create a empty object - initialized with the
		// default data from the app / user setting's class
		public void Create()
		{
			// since the data is "new" and may not match what
			// has been saved, not as not initalized

			Info = new T();

			Status = CREATED;
		}

		#endregion

		#region + Save

		public void Save()
		{
			XmlWriterSettings xmlSettings = new XmlWriterSettings() {Indent = true};

			DataContractSerializer ds = new DataContractSerializer(typeof(T));

//			Settings.Header = new Heading(Settings.ClassVersion);

			using (XmlWriter w = XmlWriter.Create(Info.SettingPathAndFile, xmlSettings))
			{
				ds.WriteObject(w, Info);

//				w.Close();
			}

//			SetFileStatus();

			// since file and memory match
			Status = SAVED;
		}

		#endregion

		#region +Upgrade

		public void Upgrade()
		{
			if (!Info.ClassVersionsMatch)
			{
				List<SettingBase> settings = null;

				switch (Info.FileType)
				{
				case Heading.SettingFileType.APP:
					{
						settings =
							GetMatchingClasses<AppSettingBase>();
						break;
					}
				case Heading.SettingFileType.USER:
					{
						settings =
							GetMatchingClasses<UserSettingBase>();
						break;
					}
				}

				if (settings != null)
				{
					UpgradeList(settings);

					Save();
				}
			}
		}


		#endregion

		#region +Reset

		private RstData ResetData;

		// reset the data portion to it's default values
		public void Reset()
		{
			Create();

			ResetData?.Invoke();
		}

		public void SyncData()
		{
			if (ResetData != null)
			{
				ResetData?.Invoke();
			}
		}

		#endregion

		#region + Utilities

//		public void SetFileStatus()
//		{
//			// set file exists status
//			if (FileExists())
//			{
//				if (!Info.ClassVersionsMatch)
//				{
//					Status = VERSIONMISMATCH;
//				}
//			}
//		}

		// report whether the setting file does exist
		private bool FileExists()
		{
			bool result = Info.Exists;

			if (result)
			{
				Status = EXISTS;
			}
			else
			{
				Status = DOESNOTEXIST;
			}

			return result;
		}

		// use reflection to create a list of classes of matching type
		// that may be used to upgrade the existing class to the
		// current version
		private List<SettingBase> GetMatchingClasses<U>() where U : SettingBase
		{
			Type baseType = typeof(T).BaseType;

			List<SettingBase> list = new List<SettingBase>();

			list.Add(Info);

			Type[] types = Assembly.GetExecutingAssembly().GetTypes();

			// scan the list of all of the types to find the
			// few of the correct type
			foreach (Type type in types)
			{
				if (type == Info.GetType()) continue;

				Type testType = type.BaseType;

				if (testType == baseType)
				{
					list.Add((U) Activator.CreateInstance(type));
				}
			}

			return list;
		}

		// based on a list of matching setting of prior versions
		// upgrade the existing to the current version
		private void UpgradeList(List<SettingBase> settings)
		{
			if (settings == null || settings.Count < 2)
			{
				logMsgLn2("upgrading settings", "nothing to upgrade");
				return;
			}

			if (Info.ClassVersionsMatch)
			{
				logMsgLn2("upgrading settings", "class versions do match - do nothing");
				return;
			}

			logMsgLn2("upgrading settings", "class versions do not match");

			settings.Sort(); // must be in the correct order from oldest to newest

			for (int i = 0; i < settings.Count; i++)
			{
				int j = String.Compare(settings[i].ClassVersion, 
					Info.ClassVersionFromFile, StringComparison.Ordinal);

				if (j < 0) continue;

				if (j == 0)
				{
					// found the starting point, read the current setting
					// file into memory
					logMsgLn2("upgrading", "from this version: " +
						settings[i].ClassVersion);

					settings[i] = Read(settings[i].GetType());
				}
				else
				{
					logMsgLn2("upgrading", "to this version: " +
						settings[i].ClassVersion);
					settings[i].UpgradeFromPrior(settings[i - 1]);
				}
			}
		}

		
//		// report whether the setting file does exist
//		public static bool FileExists(string pathAndFile)
//		{
//			return File.Exists(pathAndFile);
//		}

//		public bool VersionsMatch()
//		{
//			return Info.Header.ClassVersion.Equals(Heading.ClassVersionFromFile);
//		}

//		private void SetClassVersionFromFile()
//		{
//
////			Heading.ClassVersionFromFile[(int) Info.FileType] = 
//			Info.ClassVersionFromFile =
//				GetClassVersionFromFile();
//		}
//
//		private void SetSystemVersionFromFile()
//		{
////			Heading.SystemVersionFromFile[(int) Info.FileType] = 
//			Info.SystemVersionFromFile =
//				GetSystemVersionFromFile();
//		}
//
//		// gets the class version from the file
//		private string GetClassVersionFromFile()
//		{
//			if (!FileExists())
//			{
//				return null;
//			}
//
//			using (XmlReader reader = XmlReader.Create(Info.SettingPathAndFile))
//			{
//				while (reader.Read())
//				{
//					if (reader.IsStartElement(nameof(
//						Info.Header.ClassVersion)))
//					{
//						return reader.ReadString();
//					}
//				}
//			}
//			return "";
//		}
//
//		// gets the system version from the file
//		private string GetSystemVersionFromFile()
//		{
//			if (Status.Equals(NOPATH) || Status.Equals(DOESNOTEXIST))
//			{
//				return null;
//			}
//
//			// use xml reader to find the version from
//			// the file
//			using (XmlReader reader = XmlReader.Create(Info.SettingPathAndFile))
//			{
//				while (reader.Read())
//				{
//					if (reader.IsStartElement(nameof(
//						Info.Header.SystemVersion)))
//					{
//						return reader.ReadString();
//					}
//				}
//			}
//			return null;
//		}

		#endregion
	}

	#region Support Classes

	[DataContract(Namespace = Heading.NSpace)]
	//	[DataContract]
	public abstract class SettingBase : IComparable<SettingBase>
	{
		[DataMember] public Heading Header;

		public abstract Heading.SettingFileType  FileType { get; }
		public abstract bool ClassVersionsMatch { get; }

		public abstract string ClassVersion { get; }
		public abstract string ClassVersionFromFile { get; }
		public abstract string SystemVersionFromFile { get; }
		public abstract string FileName { get; }
		public abstract string RootPath { get; }
		public abstract string[] SubFolders { get; }

		public abstract string SettingPath { get; }
		public abstract string SettingPathAndFile { get; }

		public abstract bool Exists { get; }

		public SettingBase()
		{
			Header = new Heading(ClassVersion);
			Header.Notes = "Created in Version " + ClassVersion;
		}

//		public const string SETTINGFILEBASE = @".setting.xml";

//		// get the path to the setting file
//		public string SettingPath
//		{
//			get
//			{
//				if (SubFolders == null)
//				{
//					return RootPath;
//				}
//				return CsUtilities.SubFolder(SubFolders.Length - 1,
//					RootPath, SubFolders);
//			}
//		}
//
//		// get the path and the file name for the setting file
//		public string SettingPathAndFile
//		{
//			get
//			{
//				if (!Directory.Exists(SettingPath))
//				{
//					if (!CsUtilities.CreateSubFolders(RootPath, SubFolders))
//					{
//						throw new DirectoryNotFoundException("setting file path");
//					}
//				}
//				return SettingPath + "\\" + FileName;
//			}
//		}

		public int CompareTo(SettingBase other)
		{
			return String.Compare(ClassVersion, other.ClassVersion, StringComparison.Ordinal);
		}

//		public abstract List<T> GetUpgradeList<T>() where T : SettingBase;
		public abstract void UpgradeFromPrior(SettingBase prior);
	}

	[DataContract]
	// define file type specific information: User
	public abstract class UserSettingBase : SettingBase
	{
//		private static string _classVersionFromFile;
//		private static string _systemVersionFromFile;
//
//		private static readonly string _FileName = @"user" + SettingBase.SETTINGFILEBASE;
//		private static readonly string _RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
//
//		private static readonly string[] _SubFolders =
//		{
//			CsUtilities.CompanyName,
//			CsUtilities.AssemblyName
//		};

		public override bool Exists => PathAndFile.User.Exists;

		public override string SettingPath => PathAndFile.User.SettingPath;
		public override string SettingPathAndFile => PathAndFile.User.SettingPathAndFile;

		public override string FileName => PathAndFile.User.FileName;
		public override string RootPath => PathAndFile.User.RootPath;
		public override string[] SubFolders => PathAndFile.User.SubFolders;

		public override string ClassVersionFromFile => PathAndFile.User.ClassVersionFromFile;
//		{
//			get => _classVersionFromFile;
//			set => _classVersionFromFile = value;
//		}

		public override string SystemVersionFromFile => PathAndFile.User.SystemVersionFromFile;
//		{
//			get => _systemVersionFromFile;
//			set => _systemVersionFromFile = value;
//		}

		public override bool ClassVersionsMatch => Header.ClassVersion.Equals(ClassVersionFromFile);

		public override Heading.SettingFileType FileType => Heading.SettingFileType.USER;
	}

	[DataContract]
	// define file type specific information: App
	public abstract class AppSettingBase : SettingBase
	{
//		private static string _classVersionFromFile;
//		private static string _systemVersionFromFile;
//
//		private static readonly string _FileName = CsUtilities.AssemblyName + SettingBase.SETTINGFILEBASE;
//		private static readonly string _RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
//
//		private static readonly string[] _SubFolders =
//		{
//			CsUtilities.CompanyName,
//			CsUtilities.AssemblyName,
//			"AppSettings"
//		};

		public override bool Exists => PathAndFile.App.Exists;

		public override string SettingPath        => PathAndFile.App.SettingPath;
		public override string SettingPathAndFile => PathAndFile.App.SettingPathAndFile;

		public override string   FileName   => PathAndFile.App.FileName;
		public override string   RootPath   => PathAndFile.App.RootPath;
		public override string[] SubFolders => PathAndFile.App.SubFolders;

		public override string ClassVersionFromFile => PathAndFile.App.ClassVersionFromFile;
//		{
//			get => _classVersionFromFile;
//			set => _classVersionFromFile = value;
//		}

		public override string SystemVersionFromFile => PathAndFile.App.SystemVersionFromFile;
//		{
//			get => _systemVersionFromFile;
//			set => _systemVersionFromFile = value;
//		}

		public override bool ClassVersionsMatch => Header.ClassVersion.Equals(ClassVersionFromFile);

		public override Heading.SettingFileType FileType => Heading.SettingFileType.APP;
	}

	#endregion

	#region + Path File and Utilities

	public static class PathAndFile
	{
		internal static AppPathAndFile  App  = new AppPathAndFile();
		internal static UserPathAndFile User = new UserPathAndFile();

		private const string SETTINGFILEBASE = @".setting.xml";

		internal interface IPathAndFile
		{
			string   FileName   { get; }
			string   RootPath   { get; }
			string[] SubFolders { get; }

			string ClassVersionFromFile  { get; }
			string SystemVersionFromFile { get; }

			bool Exists { get; }
		}

		internal abstract class PathAndFileBase : IPathAndFile
		{
			public abstract string   FileName              { get; }
			public abstract string   RootPath              { get; }
			public abstract string[] SubFolders            { get; }
			public abstract string   ClassVersionFromFile  { get; }
			public abstract string   SystemVersionFromFile { get; }

			public bool Exists => File.Exists(SettingPathAndFile);

			public string SettingPath
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
			public string SettingPathAndFile
			{
				get
				{
					if (!Directory.Exists(SettingPath))
					{
						if (!CsUtilities.CreateSubFolders(RootPath, SubFolders))
						{
							throw new DirectoryNotFoundException("setting file path");
						}
					}

					return SettingPath + "\\" + FileName;
				}
			}

			public string GetVersionFromFile(string elementName)
			{
				return CsUtilities.ScanXmlForElementValue(SettingPathAndFile, elementName);
			}
		}

		internal class AppPathAndFile : PathAndFileBase
		{
			private static string   _FileName   { get; set; }
			private static string   _RootPath   { get; set; }
			private static string[] _SubFolders { get; set; }

			public override string FileName => _FileName =
				CsUtilities.AssemblyName + SETTINGFILEBASE;

			public override string RootPath => _RootPath =
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			public override string[] SubFolders => _SubFolders =
				new string []
				{
					CsUtilities.CompanyName,
					CsUtilities.AssemblyName,
					"AppSettings"
				};

			public override string ClassVersionFromFile => GetVersionFromFile(Heading.ClassVersionName);

			public override string SystemVersionFromFile => GetVersionFromFile(Heading.SystemVersionName);
		}

		internal class UserPathAndFile : PathAndFileBase
		{
			private static string   _FileName   { get; set; }
			private static string   _RootPath   { get; set; }
			private static string[] _SubFolders { get; set; }

			public override string FileName => _FileName =
				@"user" + SETTINGFILEBASE;

			public override string RootPath => _RootPath =
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			public override string[] SubFolders => _SubFolders =
				new string []
				{
					CsUtilities.CompanyName,
					CsUtilities.AssemblyName
				};

			public override string ClassVersionFromFile => GetVersionFromFile(Heading.ClassVersionName);

			public override string SystemVersionFromFile => GetVersionFromFile(Heading.SystemVersionName);
		}
	}


	#endregion


}








//	#region Info User
//	
//	public static class SettingsUser
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		public static SettingsMgr<UserSettingInfo22> Admin { get; private set; }
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		public static UserSettingInfo22 Info { get; private set; }
//
//		// initalize and create the setting objects
//		static SettingsUser()
//		{
//			Admin = new SettingsMgr<UserSettingInfo22>();
//			Info = Admin.Info;
//			Admin.ResetData = ResetData;
//		}
//
//		// reset the settings data to their current value
//		public static void ResetData()
//		{
//			Info = Admin.Info;
//		}
//	}

//
//	#endregion

//	
//	#region Info App
//	
//	public static class AppSettings
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		public static SettingsMgr<AppSettingData22> Admin { get; private set; }
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		public static AppSettingData22 Info { get; private set; }
//
//		// initalize and create the setting objects
//		static AppSettings()
//		{
//			Admin = new SettingsMgr<AppSettingData22>();
//			Info = Admin.Info;
//			Admin.ResetData = ResetData;
//		}
//
//		// reset the settings data to their current value
//		public static void ResetData()
//		{
//			Info = Admin.Info;
//		}
//	}
//	
//	#endregion
//}

// sample setting data classes
//
// **********************
// user settings: SettingsUserSettings.cs
// **********************
//
// access thus:
// for file configuration info:
// Admin.SettingPathAndFile (for example)
//
// for the individual fields:
// Info.UnCategorizedValue  (for example)
// Info.GeneralValues.TestI  (for example)
//

//	// this is the actual data set saved to the user's configuration file
//	// this is unique for each program
//	[DataContract(Name = "UserSettingInfo22")]
//	public class UserSettingInfo22 : SettingsPathFileUserBase
//	{
//		// this is just the version of this class
//		public override string ClassVersion { get; }
//
//		[DataMember] public int UnCategorizedValue = 1000;
//		[DataMember] public int UnCategorizedValue2 = 2000;
//		[DataMember] public GeneralValues GeneralValues = new GeneralValues();
//
//		[DataMember]
//		public Window1 MainWindow { get; set; } = new Window1();
//
//		[DataMember(Name = "DictionaryTest3")] public CustDict<string, TestStruct> TestDictionary3 =
//			new CustDict<string, TestStruct>()
//			{
//				{"one", new TestStruct(1, 2, 3)},
//				{"two", new TestStruct(1, 2, 3)},
//				{"three", new TestStruct(1, 2, 3)}
//			};
//
//	// provide all of the default values hee
//	private void SetDefaultValues()
//	{
//		ClassVersion = "2.0";
//		UnCategorizedValue = 1000;
//		UnCategorizedValue2 = 2000;
//	}
//
//
//	public UserSettingInfo22()
//	{
//		SetDefaultValues();
//	}
//
//	[OnDeserializing]
//	void OnDeserializing(StreamingContext context)
//	{
//		SetDefaultValues();
//	}
//
//	}
//
//	// sample sub-class of dictionary to provide names to elements
//	[CollectionDataContract(Name = "CustomDict", KeyName = "key", ValueName = "data", ItemName = "row")]
//	public class CustDict<T1, T2> : Dictionary<T1, T2>
//	{
//	}
//	// sample struct / data
//	public struct TestStruct
//	{
//		[DataMember(Name = "line1")] public int IntA;
//		[DataMember(Name = "line2")] public int IntB;
//		[DataMember(Name = "line3")] public int IntC;
//
//		public TestStruct(int a, int b, int c)
//		{
//			IntA = a;
//			IntB = b;
//			IntC = c;
//		}
//	}
//
//	// sample class / data
//	public class GeneralValues
//	{
//		public int TestI = 0;
//		public bool TestB = false;
//		public double TestD = 0.0;
//		public string TestS = "this is a test";
//		public int[] TestIs = new[] {20, 30};
//		public string[] TestSs = new[] {"user 1", "user 2", "user 3"};
//	}
//
//	// sample class / data
//	public class Window1
//	{
//		public int Height = 50;
//		public int Width = 100;
//	}

//
//
// **********************
// app settings:  SettingsAppSettngs.cs
// **********************
//
// access thus:
// for file configuration info:
// Admin.SettingPathAndFile (for example)
//
// for the individual fields:
// Info.AppI  (for example)
//

//	[DataContract(Name = "AppSettingData22")]
//	public class AppSettingData22 : SettingsPathFileAppBase
//	{
//		// this is the version of this class
//		public override string ClassVersion { get; } = "1.0";
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
//		public int[] AppIs { get; set; } = new[] {20, 30};
//
//		
//		see usersettings for OnDeserializing & properly setting
//		default values
//
//	}











