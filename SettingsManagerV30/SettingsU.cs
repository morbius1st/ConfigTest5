using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SettingManager;

using static UtilityLibrary.MessageUtilities2;


namespace SettingsManagerV30
{
//	[DataContract]
//	public abstract class SettingBase : SettingsPathFileBase, IComparable<SettingBase>
//	{
//
//		public SettingBase()
//		{
//			Header = new Heading(ClassVersion);
//			Header.Notes = "Created in Version " + ClassVersion;
//
//			if (IsUserSettings)
//			{
//				FileName   = UserPathAndFile.FileName;
//				RootPath   = UserPathAndFile.RootPath;
//				SubFolders = UserPathAndFile.SubFolders;
//			}
//			else
//
//			{
//				FileName   = AppPathAndFile.FileName;
//				RootPath   = AppPathAndFile.RootPath;
//				SubFolders = AppPathAndFile.SubFolders;
//			}
//		}
//
//		public int CompareTo(SettingBase other)
//		{
//			return String.Compare(ClassVersion, other.ClassVersion, StringComparison.Ordinal);
//		}
//
//		public abstract void Upgrade(SettingBase prior);
//	}

	[DataContract]
	public abstract class UsrSettingBase : SettingsPathFileBase
	{
		public override Heading.SettingFileType FileType =>
			Heading.SettingFileType.USER;

		public override bool IsUserSettings { get; set; } = true;

		public override string ClassVersionOfFile => 
			Heading.ClassVersionOfFile[(int) FileType];
		public override bool ClassVersionsMatch => 
			Heading.ClassVersionsMatch[(int) FileType];
	}
	
	[DataContract]
	public abstract class AppSettingBase : SettingsPathFileBase
	{
		public override Heading.SettingFileType FileType =>
			Heading.SettingFileType.APP;

		public override bool IsUserSettings { get; set; } = false;

		public override string ClassVersionOfFile =>
			Heading.ClassVersionOfFile[(int) FileType];
		public override bool ClassVersionsMatch =>
			Heading.ClassVersionsMatch[(int) FileType];
	}


	public class UserSettingUpgrade
	{
		public SettingUpgrade su;

		public UserSettingUpgrade()
		{
			su = new SettingUpgrade(UserSettings.Admin);

			su.SetgClasses.Add(UserSettings.Info);
			su.SetgClasses.Add(new UserSettingInfo20());
			su.SetgClasses.Add(new UserSettingInfo21());
		}

		public void Upgrade ()
		{
			su.Upgrade();
		}
	}

	public class AppSettingUpgrade
	{
		public SettingUpgrade su;

		public AppSettingUpgrade()
		{
			su = new SettingUpgrade(AppSettings.Admin);

			su.SetgClasses.Add(AppSettings.Info);
			su.SetgClasses.Add(new AppSettingInfo20());
			su.SetgClasses.Add(new AppSettingInfo21());
		}

		public void Upgrade()
		{
			su.Upgrade();
		}
	}


	public class SettingUpgrade
	{
		public List<SettingsPathFileBase> SetgClasses = new List<SettingsPathFileBase>();

		private ITest Admin;

		public SettingUpgrade(ITest admin)
		{
			Admin = admin;
		}

		public void Upgrade()
		{
			if (!SetgClasses[0].ClassVersionsMatch)
			{
				logMsgLn2("upgrading", "class versions do not match - upgrade");

				List<SettingsPathFileBase> Us1 = SetgClasses;

				SetgClasses.Sort();

				Process(Admin);

				Admin.Save();
			}
			else
			{
				logMsgLn2("upgrading", "class versions do match - do nothing");
			}
		}

		private void Process(ITest Admin)
		{
			List<SettingsPathFileBase> Us1 = SetgClasses;

			for (int i = 0; i < SetgClasses.Count; i++)
			{
				int j = String.Compare(SetgClasses[i].ClassVersion, SetgClasses[0].ClassVersionOfFile, StringComparison.Ordinal);

				if (j == 0)
				{
					logMsgLn2("upgrading", "from this version: " +
						SetgClasses[i].ClassVersion);
					SetgClasses[i] = Admin.Read(SetgClasses[i].GetType());
				} 
				else if (j > 0)
				{
					logMsgLn2("upgrading", "to this version: " +
						SetgClasses[i].ClassVersion);

					SetgClasses[i].Upgrade(SetgClasses[i - 1]);
				}
			}
		}
	}
}