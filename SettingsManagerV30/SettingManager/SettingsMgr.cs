#region  
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using static SettingManager.SettingMgrStatus;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities2;
// ReSharper disable IdentifierTypo

#endregion

// requires a reference to:
// System.Runtime.Serialization
// System.Xaml


// item name:	Config
// username:	jeff s
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
//	ver 3.0		updated to incorporate checking for setting file being
//				a different version and to upgrade to the current version

// standard settings manager

// ReSharper disable once CheckNamespace
namespace SettingManager
{
	#region + Header

	[DataContract(Namespace = N_SPACE)]
	//	[DataContract]
	public class Heading
	{
		public static string ClassVersionName = nameof(ClassVersion);
		public static string SystemVersionName = nameof(SystemVersion);

		public enum SettingFileType
		{
			APP = 0,
			USER = 1,
			// ReSharper disable once UnusedMember.Global
			LENGTH = 2
		}

		public const string N_SPACE = "";

		public Heading(string classVersion) => ClassVersion = classVersion;

		[DataMember(Order = 1)] public string SaveDateTime         = DateTime.Now.ToString("yyyy-MM-dd - HH:mm zzz");
		[DataMember(Order = 2)] public string AssemblyVersion      = CsUtilities.AssemblyVersion;
		[DataMember(Order = 3)] public string SystemVersion = "3.0";
		[DataMember(Order = 4)] public string ClassVersion;
		[DataMember(Order = 5)] public string Notes = "created by v3.0";
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

	public delegate void RstData();

	public class SettingsMgr<T> 
		where T : SettingBase, new()
	{
		#region + Constructor

		public SettingsMgr(RstData rst, bool autoUpgrade)
		{
			Status = CONSTRUCTED;

			if (String.IsNullOrWhiteSpace(Info.SettingPathAndFile))
			{
				Status = NOPATH;
			}

#if DEBUG
			logMsgLn2();
			logMsgLn2("at ctor SettingsMgr", "status| " + Status +
				" file type| " + Info.FileType.ToString()
			+ "  autoupgrade?| " + AutoUpgrade + " :: " + AllowAutoUpgrade
				);
#endif
			AutoUpgrade = autoUpgrade;

			_resetData = rst;
		}

		public void Initialize()
		{
			if (Status != NOPATH)
			{
				Status = INITIALIZED;
#if DEBUG
				logMsgLn2();
				logMsgLn2("at SettingsMgr initialize", "status| " + Status +
					" file type| " + Info.FileType.ToString()
					+ "  autoupgrade?| " + AutoUpgrade + " :: " + AllowAutoUpgrade
					);
#endif

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

		public bool AutoUpgrade { get; set; } = false;

		public static bool AllowAutoUpgrade { get; set; } = false;

		public static bool CanAutoUpgrade { get; set; } = false;

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
#if DEBUG
					logMsgLn2();
					logMsgLn2("at read", "upgrading");
#endif
					Upgrade();
				}
			}
			else
			{
				Create();
				Save();
			}
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
			// has been saved, note as not initialized
			Info = new T();

			Status = CREATED;
		}

		#endregion

		#region + Save

		public void Save()
		{
			XmlWriterSettings xmlSettings = new XmlWriterSettings() {Indent = true};

			DataContractSerializer ds = new DataContractSerializer(typeof(T));

			using (XmlWriter w = XmlWriter.Create(Info.SettingPathAndFile, xmlSettings))
			{
				ds.WriteObject(w, Info);
			}

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

		private readonly RstData _resetData;

		// reset the data portion to it's default values
		public void Reset()
		{
			Create();

			_resetData?.Invoke();
		}

		public void SyncData()
		{
			if (_resetData != null)
			{
				_resetData?.Invoke();
			}
		}

		#endregion

		#region + Utilities

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

			List<SettingBase> list = new List<SettingBase> {Info};

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
#if DEBUG
				logMsgLn2("upgrading settings", "nothing to upgrade");
#endif
				return;
			}

			if (Info.ClassVersionsMatch)
			{
#if DEBUG
				logMsgLn2("upgrading settings", "class versions do match - do nothing");
#endif
				return;
			}

#if DEBUG
			logMsgLn2("upgrading settings", "class versions do not match");
#endif

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
#if DEBUG
					logMsgLn2("upgrading", "from this version: " +
						settings[i].ClassVersion);
#endif

					settings[i] = Read(settings[i].GetType());
				}
				else
				{
#if DEBUG
					logMsgLn2("upgrading", "to this version: " +
						settings[i].ClassVersion);
#endif
					settings[i].UpgradeFromPrior(settings[i - 1]);
				}
			}
		}

		#endregion
	}

	#region Support Classes

	[DataContract(Namespace = Heading.N_SPACE)]
	//	[DataContract]
	public abstract class SettingBase : IComparable<SettingBase>
	{
		[DataMember] public Heading Header;

		public abstract bool AllowAutoUpgrade { get; set; }

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

		public SettingBase() => Header = new Heading(ClassVersion) { Notes = "Created in Version " + ClassVersion };

		public int CompareTo(SettingBase other) => String.Compare(ClassVersion, other.ClassVersion, StringComparison.Ordinal);

		public abstract void UpgradeFromPrior(SettingBase prior);
	}

	[DataContract]
	// define file type specific information: User
	public abstract class UserSettingBase : SettingBase
	{
		public override bool AllowAutoUpgrade { get; set; }

		public override bool Exists => PathAndFile.User.Exists;

		public override string SettingPath => PathAndFile.User.SettingPath;
		public override string SettingPathAndFile => PathAndFile.User.SettingPathAndFile;

		public override string FileName => PathAndFile.User.FileName;
		public override string RootPath => PathAndFile.User.RootPath;
		public override string[] SubFolders => PathAndFile.User.SubFolders;

		public override string ClassVersionFromFile => PathAndFile.User.ClassVersionFromFile;

		public override string SystemVersionFromFile => PathAndFile.User.SystemVersionFromFile;

		public override bool ClassVersionsMatch => Header.ClassVersion.Equals(ClassVersionFromFile);

		public override Heading.SettingFileType FileType => Heading.SettingFileType.USER;
	}

	[DataContract]
	// define file type specific information: App
	public abstract class AppSettingBase : SettingBase
	{
		public override bool AllowAutoUpgrade { get; set; }

		public override bool Exists => PathAndFile.App.Exists;

		public override string SettingPath        => PathAndFile.App.SettingPath;
		public override string SettingPathAndFile => PathAndFile.App.SettingPathAndFile;

		public override string   FileName   => PathAndFile.App.FileName;
		public override string   RootPath   => PathAndFile.App.RootPath;
		public override string[] SubFolders => PathAndFile.App.SubFolders;

		public override string ClassVersionFromFile => PathAndFile.App.ClassVersionFromFile;

		public override string SystemVersionFromFile => PathAndFile.App.SystemVersionFromFile;

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

		internal abstract class PathAndFileBase
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

			protected string GetVersionFromFile(string elementName) => CsUtilities.ScanXmlForElementValue(SettingPathAndFile, elementName);
		}

		internal class AppPathAndFile : PathAndFileBase
		{
			public override string FileName => CsUtilities.AssemblyName + SETTINGFILEBASE;

			public override string RootPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			public override string[] SubFolders => new []
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
			public override string FileName => @"user" + SETTINGFILEBASE;

			public override string RootPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			public override string[] SubFolders => new string []
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





// the below are example user setting and app setting files - these are out-of-date



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











