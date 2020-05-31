#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;
using SettingsManagerV50;
using static SettingsManager.SettingMgrStatus;
using UtilityLibrary;

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
//	ver 3.1		added call to SyncData() at the end of a normal read();
//	ver 4.0		updated to allow a machine level and site level setting file

// standard settings manager

// ReSharper disable once CheckNamespace
namespace SettingsManager
{
#region + Header

	[DataContract(Namespace =  N_SPACE)]
	public class Heading
	{
		public static string ClassVersionName = nameof(DataClassVersion);
		public static string SystemVersionName = nameof(SystemVersion);

		public enum SettingFileType
		{
			APP = 0,
			USER = 1,
			MACH = 2,
			SITE = 3,

			// ReSharper disable once UnusedMember.Global
			LENGTH = 4
		}

		public const string N_SPACE = "";

		public Heading(string dataClassVersion) => DataClassVersion = dataClassVersion;

		[DataMember(Order = 1)] public string SaveDateTime         = DateTime.Now.ToString("yyyy-MM-dd - HH:mm zzz");
		[DataMember(Order = 2)] public string AssemblyVersion      = CsUtilities.AssemblyVersion;
		[DataMember(Order = 3)] public string SystemVersion = "5.0";
		[DataMember(Order = 4)] public string DataClassVersion;
		[DataMember(Order = 5)] public string Notes = "created by v5.0";
		[DataMember(Order = 6)] public string Description;
	}

#endregion

	public enum SettingMgrStatus
	{
		SAVEDISALLOWED = -4,
		INVALIDPATH = -3,
		NOPATH = -2,
		DOESNOTEXIST = -1,

		BEGIN = 0,
		CONSTRUCTED,
		INITIALIZED,
		CREATED,
		READ,
		SAVED,
		EXISTS
	}

	public delegate void RstData();

	public class SettingsMgr<Tpath, Tinfo>
		where Tpath : PathAndFileBase, new()
		where Tinfo : SettingInfoBase, new()
	{
	#region + Constructor

		public SettingsMgr(Tpath path, RstData rst)
		{
			Status = BEGIN;

			Path = path;

			if (path == null || String.IsNullOrWhiteSpace(Path.SettingPathAndFile))
			{
				Status = NOPATH;
			}

			_resetData = rst;

			Initialize();

			Status = CONSTRUCTED;
		}

		private void Initialize()
		{
			if (Status != NOPATH)
			{
				Status = INITIALIZED;

				UpgradeRequired = !Info.ClassVersionsMatch(Path.ClassVersionFromFile);

				if (FileExists())
				{
					if (!Info.ClassVersionsMatch(Path.ClassVersionFromFile) && CanAutoUpgrade)
					{
						Upgrade();
					}
				}
			}
		}

	#endregion

	#region + Properties

		public Tinfo Info { get; private set; } =
			new Tinfo();

		private Tpath Path { get; set; }

		public SettingMgrStatus Status { get; private set; }

		public bool CanAutoUpgrade { get; set; } = false;

		public bool UpgradeRequired { get; set; } = false;

	#endregion

	#region + Read

		public void Read()
		{
			if (Path.SettingPathAndFile.IsVoid() )
			{
				Status = INVALIDPATH;
				return;
			}

			// does the file already exist?
			if (FileExists())
			{
				if (Info.ClassVersionsMatch(Path.ClassVersionFromFile))
				{
					try
					{
						DataContractSerializer ds = new DataContractSerializer(typeof(Tinfo));

						// file exists - get the current values
						using (FileStream fs = new FileStream(Path.SettingPathAndFile, FileMode.Open))
						{
							Info = (Tinfo) ds.ReadObject(fs);
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
							+ Path.SettingPathAndFile + MessageUtilities.nl
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

			// added in v3.1
			SyncData();
		}

		public dynamic Read(Type type)
		{
			dynamic p = null;

			if (FileExists())
			{
				try
				{
					DataContractSerializer ds = new DataContractSerializer(type);

					using (FileStream fs = new FileStream(Path.SettingPathAndFile, FileMode.Open))
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
			if (Path.SettingPathAndFile.IsVoid() )
			{
				Status = INVALIDPATH;
				return;
			}

			// since the data is "new" and may not match what
			// has been saved, note as not initialized
			Info = new Tinfo();

			Status = CREATED;
		}

	#endregion

	#region + Write

		public void Write()
		{
			Save();
		}

		public void Save()
		{
			if (Path.IsReadOnly || Path.SettingPathAndFile.IsVoid())
			{
				Status = SAVEDISALLOWED;
				return;
			}

			XmlWriterSettings xmlSettings = new XmlWriterSettings() {Indent = true};

			DataContractSerializer ds = new DataContractSerializer(typeof(Tinfo));

			using (XmlWriter w = XmlWriter.Create(Path.SettingPathAndFile, xmlSettings))
			{
				ds.WriteObject(w, Info);
			}

			// since file and memory match
			Status = SAVED;
		}

	#endregion

	#region + Upgrade

		// upgrade from a prior class version to the current class version
		// this method is less restricted to allow the user to perform
		// a manual upgrade - if CanAutoUpgrade is false
		public void Upgrade()
		{
			// is an upgrade needed
			if (!Info.ClassVersionsMatch(Path.ClassVersionFromFile))
			{
				// get a list of prior setting classes from 
				// which to possible upgrade
				List<SettingInfoBase> settings = null;

				switch (Info.FileType)
				{
				case Heading.SettingFileType.APP:
					{
						settings =
							GetMatchingClasses<AppSettingInfoBase>();
						break;
					}
				case Heading.SettingFileType.USER:
					{
						settings =
							GetMatchingClasses<UserSettingInfoBase>();
						break;
					}
				}

				// if needed, preform upgrade
				if (settings != null)
				{
					UpgradeList(settings);

					Save();

					UpgradeRequired = false;
				}
			}
		}

	#endregion

	#region + Reset

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
			bool result = Path.Exists;


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
		private List<SettingInfoBase> GetMatchingClasses<U>() where U : SettingInfoBase
		{
			Type baseType = typeof(Tinfo).BaseType;

			List<SettingInfoBase> list = new List<SettingInfoBase> {Info};

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
		private void UpgradeList(List<SettingInfoBase> settings)
		{
			if (settings == null || settings.Count < 2)
			{
				return;
			}

			if (Info.ClassVersionsMatch(Path.ClassVersionFromFile))
			{
				return;
			}

			settings.Sort(); // must be in the correct order from oldest to newest

			for (int i = 0; i < settings.Count; i++)
			{
				int j = String.Compare(settings[i].DataClassVersion,
					Path.ClassVersionFromFile, StringComparison.Ordinal);

				if (j < 0) continue;

				if (j == 0)
				{
					// found the starting point, read the current setting
					// file into memory
					settings[i] = Read(settings[i].GetType());
				}
				else
				{
					settings[i].UpgradeFromPrior(settings[i - 1]);
				}
			}
		}

	#endregion
	}

#region + Support Classes

	[DataContract(Namespace = Heading.N_SPACE)]
	public abstract class SettingInfoBase : IComparable<SettingInfoBase>
	{
		private bool? classVersionsMatch = null;
		[DataMember] public Heading Header;

		public SettingInfoBase()
		{
			Header = new Heading(DataClassVersion)
			{
				Notes = "Created in Version " + DataClassVersion,
				Description = Description
			};
		}

		public abstract string Description                 { get; }
		public abstract string DataClassVersion            { get; }
		public abstract Heading.SettingFileType  FileType  { get; }

		public int CompareTo(SettingInfoBase other) =>
			String.Compare(DataClassVersion, other.DataClassVersion, StringComparison.Ordinal);

		public abstract void UpgradeFromPrior(SettingInfoBase prior);

		public bool ClassVersionsMatch(string fileClassVersion)
		{
			if (classVersionsMatch == null)
			{
				classVersionsMatch = fileClassVersion?.Equals(DataClassVersion) ?? true;
			}

			return classVersionsMatch.Value;
		}
	}

	[DataContract]
	// define file type specific information: User
	public abstract class UserSettingInfoBase : SettingInfoBase
	{
		public override Heading.SettingFileType FileType => Heading.SettingFileType.USER;
	}

	[DataContract]
	// define file type specific information: aPP
	public abstract class AppSettingInfoBase : SettingInfoBase
	{
		public override Heading.SettingFileType FileType => Heading.SettingFileType.APP;
	}

	[DataContract]
	// define file type specific information: aPP
	public abstract class MachSettingInfoBase : SettingInfoBase
	{
		public override Heading.SettingFileType FileType => Heading.SettingFileType.MACH;
	}

	[DataContract]
	// define file type specific information: aPP
	public abstract class SiteSettingInfoBase : SettingInfoBase
	{
		public override Heading.SettingFileType FileType => Heading.SettingFileType.SITE;
	}


#endregion

#region + Path File and Utilities

	// this class will be associated with the main class
	public abstract class PathAndFileBase
	{
		protected const string SETTINGFILEBASE = @".setting.xml";

		private string   _fileName;
		private string   _rootPath;
		private string[] _subFolders;
		private string   _settingPath;
		private string   _settingPathAndFile;

		protected PathAndFileBase()
		{
			Configure();
		}

		public string FileName
		{
			get => _fileName;
			set
			{
				_fileName = value;
				_settingPathAndFile = null;
			}
		}

		public string RootPath
		{
			get => _rootPath;
			set
			{
				_rootPath = value;
				_settingPath = null;
				_settingPathAndFile = null;
			}
		}
		//=>       PathConfig.RootPath;

		public string[] SubFolders
		{
			get => _subFolders;
			set
			{
				_subFolders = value;
				_settingPath = null;
				_settingPathAndFile = null;
			}
		}

		public bool IsReadOnly { get; set; }
		public bool IsSettingFile { get; set; }

		public bool Exists
		{
			get
			{
				if (SettingPathAndFile.IsVoid()) return false;

				return File.Exists(SettingPathAndFile);
			}
		}

		public string SettingPath
		{
			get
			{
				if (_settingPath.IsVoid())
				{
					_settingPath = CreateSettingPath();
				}

				return _settingPath;
			}
		}

		public string SettingPathAndFile
		{
			get
			{
				if (_settingPathAndFile.IsVoid())
				{
					_settingPathAndFile = CreateSettingPathAndFile();
				}

				return _settingPathAndFile;
			}
		}

		public string ClassVersionFromFile => GetVersionFromFile(Heading.ClassVersionName);
		public string SystemVersionFromFile => GetVersionFromFile(Heading.SystemVersionName);

		protected string CreateSettingPath()
		{
			if (_subFolders == null) return _rootPath;

			return CsUtilities.SubFolder( _subFolders.Length - 1,
				_rootPath,  _subFolders);
		}

		protected string CreateSettingPathAndFile()
		{
			if (!_rootPath.IsVoid())
			{
				if (!Directory.Exists(SettingPath))
				{
					if (!CsUtilities.CreateSubFolders(_rootPath, _subFolders))
					{
						throw new DirectoryNotFoundException(
							"setting file path| " + _rootPath ?? "path is null");
					}
				}

				return SettingPath + "\\" + _fileName;
			}

			return null;
		}

		public abstract void Configure();

		protected string GetVersionFromFile(string elementName) =>
			CsUtilities.ScanXmlForElementValue(SettingPathAndFile, elementName);
	}

	public class UserSettingPath50 : PathAndFileBase
	{
		public override void Configure()
		{
			FileName = @"user" + SETTINGFILEBASE;
			RootPath =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			SubFolders = new []
			{
				CsUtilities.CompanyName,
				CsUtilities.AssemblyName
			};
			IsReadOnly = false;
			IsSettingFile = true;
		}
	}

	public class AppSettingPath50 : PathAndFileBase
	{
		public override void Configure()
		{
			FileName = CsUtilities.AssemblyName + SETTINGFILEBASE;
			RootPath =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			SubFolders = new []
			{
				CsUtilities.CompanyName,
				CsUtilities.AssemblyName,
				"AppSettings"
			};
			IsReadOnly = false;
			IsSettingFile = true;
		}
	}

	public class MachSettingPath50 : PathAndFileBase
	{
		public override void Configure()
		{
			FileName = CsUtilities.AssemblyName + ".Machine" + SETTINGFILEBASE;
			RootPath =  Environment. GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			SubFolders = new []
			{
				CsUtilities.CompanyName,
				CsUtilities.AssemblyName
			};
			IsReadOnly = false;
			IsSettingFile = true;
		}
	}

	public class SiteSettingPath50 : PathAndFileBase
	{
		public override void Configure()
		{
			FileName = CsUtilities.AssemblyName + ".Site" + SETTINGFILEBASE;
			IsReadOnly = true;
			IsSettingFile = true;
		}
	}

#endregion





	[DataContract(Namespace = Heading.N_SPACE)]
	public abstract class SettingInfoBase2<T> : SettingInfoBase
		where T : new()
	{
		[DataMember]
		public abstract T Data { get; set; }
	}


	[DataContract]
	// define file type specific information: User
	public abstract class UserSettingInfoBase2<T> : SettingInfoBase2<T>
		where T : new()
	{
		public override Heading.SettingFileType FileType => Heading.SettingFileType.USER;
	}


	[DataContract(Name = "UserSettingInfoInfo50")]
	public class UserSettingInfo502<T> : UserSettingInfoBase2<T>
		where T : new ()
	{
		[DataMember]
		public override T Data { get; set; } = new T();

		public override string DataClassVersion => "5.0u";
		public override string Description => "user setting file for SettingsManagerV50";
		public override void UpgradeFromPrior(SettingInfoBase prior) { }

	}


	public class BaseSettings<Tpath, Tinfo, Tdata>
		where Tpath : PathAndFileBase, new()
		where Tinfo : SettingInfoBase2<Tdata>, new()
		where Tdata : new()
	{
		public static SettingsMgr<Tpath, Tinfo> Admin { get; private set; }

		public static Tpath Path { get; private set; } = new Tpath();
		public static Tinfo Info { get; private set; }
		public static Tdata Data { get; private set; }

		static BaseSettings()
		{
			Admin = new SettingsMgr<Tpath, Tinfo>(Path, ResetData);
			ResetData();
		}

		public static void ResetData()
		{
			Info = Admin.Info;
			Data = Info.Data;
		}
	}


	public class UserSettings2 :
		BaseSettings<UserSettingPath50,
		UserSettingInfo502<UserSettingData50>,
		UserSettingData50>
	{

		public static string setPath => Path.RootPath;


	}


}