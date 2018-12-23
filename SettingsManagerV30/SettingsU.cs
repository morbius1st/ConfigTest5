using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SettingManager;
using static SettingManager.SettingsUser;
using static UtilityLibrary.MessageUtilities2;

namespace SettingsManagerV30
{
	[DataContract]
	public abstract class UserSettingBase : SettingsPathFileUserBase, IComparable<UserSettingBase>
	{
		protected abstract string FILEVERSION { get; }

		public UserSettingBase()
		{
			Heading = new Header(FileVersion);
			Heading.SettingFileNotes = "Created in Version " + FileVersion;
		}

		public Header Header
		{
			get => Heading;
			set => Heading = value;
		}

		public abstract void Upgrade(UserSettingBase prior);

		public int CompareTo(UserSettingBase other)
		{
			return String.Compare(FileVersion, other.FileVersion, StringComparison.Ordinal);
		}

		public override string FileVersion
		{
			get => FILEVERSION;
			set { }
		}
	}


	[DataContract]
	public abstract class AppSettingBase : SettingsPathFileAppBase, IComparable<AppSettingBase>
	{
		public AppSettingBase()
		{
			Heading = new Header(FileVersion);
		}

//		public abstract override string FileVersion { get; set; }

		public Header Header
		{
			get => Heading;
			set => Heading = value;
		}

		public abstract void Upgrade(AppSettingBase prior);

		public int CompareTo(AppSettingBase other)
		{
			return String.Compare(FileVersion, other.FileVersion, StringComparison.Ordinal);
		}
	}


	public static class USetgUpgrade
	{
		public static List<UserSettingBase> USetgBase = new List<UserSettingBase>();

		// add current and past data structures
		static USetgUpgrade()
		{
			if (!USetgAdmin.FileVersionsMatch())
			{
				List<UserSettingBase> Us1 = USetgBase;

				USetgBase.Add(USetgData);
				USetgBase.Add(new UserSettings20());
				USetgBase.Add(new UserSettings21());

				USetgBase.Sort();

				Upgrade();

				USetgAdmin.Save();
			}
		}

		private static void Upgrade()
		{
			List<UserSettingBase> Us1 = USetgBase;

			for (int i = 0; i < USetgBase.Count; i++)
			{
				int j = String.Compare(USetgBase[i].FileVersion, USetgAdmin.ReadFileVersion(), StringComparison.Ordinal);

				if (j == 0)
				{
					USetgBase[i] = USetgAdmin.Read(USetgBase[i].GetType());
				} 
				else if (j > 0)
				{
					USetgBase[i].Upgrade(USetgBase[i - 1]);
				}
			}
		}
	}
}