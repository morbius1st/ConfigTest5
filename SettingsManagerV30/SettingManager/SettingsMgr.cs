#region  {

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using SettingsManagerV30;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities2;

using static SettingManager.SettingMgrStatus;

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
	#region + Settings User

	public static class SettingsUser
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<UserSettings> USetgAdmin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static UserSettings USetgData { get; private set; }

		public static SettingMgrStatus Status => USetgAdmin.Status;

		// initalize and create the setting objects
		static SettingsUser()
		{
			USetgAdmin        = new SettingsMgr<UserSettings>(ResetClass);
			USetgData         = USetgAdmin.Settings;
			USetgData.Heading = new Header(USetgData.FileVersion);
		}

		public static void ResetClass()
		{
			USetgData = USetgAdmin.Settings;
		}
	}

	#endregion

	#region + Header

	[DataContract(Namespace = NSpace)]
	//	[DataContract]
	public class Header
	{
		public const string NSpace = "";

		public Header(string settingFileVersion)
		{
			SettingFileVersion = settingFileVersion;
		}

		[DataMember(Order = 1)] public string SaveDateTime         = DateTime.Now.ToString("yyyy-MM-dd - HH:mm zzz");
		[DataMember(Order = 2)] public string AssemblyVersion      = CsUtilities.AssemblyVersion;
		[DataMember(Order = 3)] public string SettingSystemVersion = "3.0";
		[DataMember(Order = 4)] public string SettingFileVersion;
		[DataMember(Order = 5)] public string SettingFileNotes = "created as v3.0";

	}

	#endregion

	public enum SettingMgrStatus
	{
		VERSIONMISMATCH = -3,
		DOESNOTEXIST = -2,
		NOPATH = -1,

		CREATED = 1,
		READ = 2,
		SAVED = 3,
		EXISTS = 4,
	}

	public delegate void RstData();

	public class SettingsMgr<T> where T : SettingsPathFileBase, new()
	{
		#region + Constructor

		public SettingsMgr(RstData rst)
		{
			SettingsPathAndFile = Settings.SettingsPathAndFile;

			if (String.IsNullOrWhiteSpace(SettingsPathAndFile))
			{
				Status = NOPATH;
			}
			else
			{
				// set file exists status
				if (FileExists())
				{
					Status = EXISTS;

					if ( !FileVersionsMatch())
					{
						Status = VERSIONMISMATCH;
//						logMsgLn2("file versions do not match", " memory vs file" +
//							Settings.FileVersion + "  vs  " +
//							GetFileVersion()
//							);
					}
				}
				else
				{
					Status = DOESNOTEXIST;
				}
			}

			ResetData = rst;
		}

		#endregion

		#region + Properties

		public SettingMgrStatus Status { get; private set; } = CREATED;

		public T Settings { get; private set; } = new T();

		public string SettingsPathAndFile { get; private set; }

		public string SaveDateTime => Settings?.Heading.SaveDateTime ?? "undefined";
		public string AssemblyVersion => Settings?.Heading.AssemblyVersion ?? "undefined";
		public string SettingFileNotes => Settings?.Heading.SettingFileNotes ?? "undefined";

		#endregion

		#region + Read

		public void Read()
		{
			// does the file already exist?
			if (FileExists())
			{
				try
				{
					DataContractSerializer ds = new DataContractSerializer(typeof(T));

					// file exists - get the current values
					using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Open))
					{
						Settings = (T) ds.ReadObject(fs);
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
						+ SettingsPathAndFile + MessageUtilities.nl
						+ "(" + e.Message +")" + MessageUtilities.nl
						, e.InnerException);
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

					using (FileStream fs = new FileStream(SettingsPathAndFile, FileMode.Open))
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

			Settings = new T();

			Status = CREATED;
		}

		#endregion

		#region + Save

		public void Save()
		{
			XmlWriterSettings xmlSettings = new XmlWriterSettings() {Indent = true};

			DataContractSerializer ds = new DataContractSerializer(typeof(T));

//			Settings.Heading = new Header(Settings.FileVersion);

			using (XmlWriter w = XmlWriter.Create(SettingsPathAndFile, xmlSettings))
			{
				ds.WriteObject(w, Settings);
			}

			// since file and memory match
			Status = SAVED;
		}

		#endregion

		#region +Reset

		private RstData ResetData;

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


		// report whether the setting file does exist
		public bool FileExists()
		{
			bool result = FileExists(SettingsPathAndFile);

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
		
		// report whether the setting file does exist
		public static bool FileExists(string pathAndFile)
		{
			return File.Exists(pathAndFile);
		}

		public bool FileVersionsMatch()
		{
			return
				(GetFileVersion()?.Equals(Settings.FileVersion) ?? false);
		}


		public string GetFileVersion()
		{
			if (!FileExists())
			{
				return null;
			}

			// use xml reader to find the version from
			// the file
			using (XmlReader reader = XmlReader.Create(SettingsPathAndFile))
			{
				while (reader.Read())
				{
					if (reader.IsStartElement(nameof(
						SettingsUser.USetgData.Heading.SettingFileVersion)))
					{
						return reader.ReadString();
					}
				}
			}
			return null;
		}


		public string GetSystemVersion()
		{
			if (Status.Equals(NOPATH) || Status.Equals(DOESNOTEXIST))
			{
				return null;
			}

			// use xml reader to find the version from
			// the file
			using (XmlReader reader = XmlReader.Create(SettingsPathAndFile))
			{
				while (reader.Read())
				{
					if (reader.IsStartElement(nameof(
						SettingsUser.USetgData.Heading.SettingSystemVersion)))
					{
						return reader.ReadString();
					}
				}
			}
			return null;
		}

		#endregion
	}


	#region Support Classes

	[DataContract(Namespace = Header.NSpace)]
	//	[DataContract]
	public abstract class SettingsPathFileBase
	{
		[DataMember] public Header Heading;

		public abstract string FileVersion { get; set; }

		protected string FileName;
		protected string RootPath;
		protected string[] SubFolders;

		protected const string SETTINGFILEBASE = @".setting.xml";

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

	// define the path and file for the
	// user's setting file
	[DataContract(Namespace = Header.NSpace)]
	public abstract class SettingsPathFileUserBase : SettingsPathFileBase
	{
		protected SettingsPathFileUserBase()
		{
			FileName = @"user" + SETTINGFILEBASE;
			RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			SubFolders = new[]
			{
				CsUtilities.CompanyName,
				CsUtilities.AssemblyName
			};
		}
	}

//	// define the path and file name for the 
//	// application's setting file
//	[DataContract(Namespace = Header.NSpace)]
//	public abstract class SettingsPathFileAppBase : SettingsPathFileBase
//	{
//		protected SettingsPathFileAppBase()
//		{
//			FileName = CsUtilities.AssemblyName + SETTINGFILEBASE;
//			RootPath = CsUtilities.AssemblyDirectory;
//			SubFolders = null;
//			
//		}
//	}

	// define the path and file name for the 
	// application's setting file - revised location
	[DataContract(Namespace = Header.NSpace)]
	public abstract class SettingsPathFileAppBase : SettingsPathFileBase
	{
		protected SettingsPathFileAppBase()
		{
			FileName = CsUtilities.AssemblyName + SETTINGFILEBASE;
			RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			SubFolders = new[]
			{
				CsUtilities.CompanyName,
				CsUtilities.AssemblyName,
				"AppSettings"
			};

		}
	}

	#endregion

}








//	#region Settings User
//	
//	public static class SettingsUser
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		public static SettingsMgr<UserSettings> USetgAdmin { get; private set; }
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		public static UserSettings USetgData { get; private set; }
//
//		// initalize and create the setting objects
//		static SettingsUser()
//		{
//			USetgAdmin = new SettingsMgr<UserSettings>();
//			USetgData = USetgAdmin.Settings;
//			USetgAdmin.ResetData = ResetData;
//		}
//
//		// reset the settings data to their current value
//		public static void ResetData()
//		{
//			USetgData = USetgAdmin.Settings;
//		}
//	}

//
//	#endregion

//	
//	#region Settings App
//	
//	public static class SettingsApp
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		public static SettingsMgr<AppSettings> ASettings { get; private set; }
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		public static AppSettings ASet { get; private set; }
//
//		// initalize and create the setting objects
//		static SettingsApp()
//		{
//			ASettings = new SettingsMgr<AppSettings>();
//			ASet = ASettings.Settings;
//			ASettings.ResetData = ResetData;
//		}
//
//		// reset the settings data to their current value
//		public static void ResetData()
//		{
//			ASet = ASettings.Settings;
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
// USetgAdmin.SettingsPathAndFile (for example)
//
// for the individual fields:
// USetgData.UnCategorizedValue  (for example)
// USetgData.GeneralValues.TestI  (for example)
//

//	// this is the actual data set saved to the user's configuration file
//	// this is unique for each program
//	[DataContract(Name = "UserSettings")]
//	public class UserSettings : SettingsPathFileUserBase
//	{
//		// this is just the version of this class
//		public override string FileVersion { get; }
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
//		FileVersion = "2.0";
//		UnCategorizedValue = 1000;
//		UnCategorizedValue2 = 2000;
//	}
//
//
//	public UserSettings()
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
// ASettings.SettingsPathAndFile (for example)
//
// for the individual fields:
// ASet.AppI  (for example)
//

//	[DataContract(Name = "AppSettings")]
//	public class AppSettings : SettingsPathFileAppBase
//	{
//		// this is the version of this class
//		public override string FileVersion { get; } = "1.0";
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











