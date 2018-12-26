using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SettingManager;


namespace SettingsManagerV30
{
	[DataContract]
	public abstract class SettingBase : SettingsPathFileBase, IComparable<SettingBase>
	{
		public abstract string ClassVersionOfFile { get; }
		public abstract bool ClassVersionsMatch { get; }

		public abstract bool IsUserSettings { get; set; }

		public SettingBase()
		{
			Heading = new Header(ClassVersion);
			Heading.Notes = "Created in Version " + ClassVersion;

			if (IsUserSettings)
			{
				FileName   = UserPathAndFile.FileName;
				RootPath   = UserPathAndFile.RootPath;
				SubFolders = UserPathAndFile.SubFolders;
			}
			else

			{
				FileName   = AppPathAndFile.FileName;
				RootPath   = AppPathAndFile.RootPath;
				SubFolders = AppPathAndFile.SubFolders;
			}
		}

//		protected abstract string CLASSVERSION { get; }

		public int CompareTo(SettingBase other)
		{
			return String.Compare(ClassVersion, other.ClassVersion, StringComparison.Ordinal);
		}

//		public override string ClassVersion
//		{
//			get => CLASSVERSION;
//			set { }
//		}

		public abstract void Upgrade(SettingBase prior);
	}

	[DataContract]
	public abstract class UsrSettingBase : SettingBase
	{
		public override Header.SettingFileType FileType =>
			Header.SettingFileType.USER;

		public override bool IsUserSettings { get; set; } = true;

		public override string ClassVersionOfFile => 
			Header.ClassVersionOfFile[(int) FileType];
		public override bool ClassVersionsMatch => 
			Header.ClassVersionsMatch[(int) FileType];
	}
	
	[DataContract]
	public abstract class AppSettingBase : SettingBase
	{
		public override Header.SettingFileType FileType =>
			Header.SettingFileType.APP;

		public override bool IsUserSettings { get; set; } = false;

		public override string ClassVersionOfFile =>
			Header.ClassVersionOfFile[(int) FileType];
		public override bool ClassVersionsMatch =>
			Header.ClassVersionsMatch[(int) FileType];
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
		public List<SettingBase> SetgClasses = new List<SettingBase>();

		private ITest Admin;

		public SettingUpgrade(ITest admin)
		{
			Admin = admin;
		}

		public void Upgrade()
		{
			if (!SetgClasses[0].ClassVersionsMatch)
			{
				List<SettingBase> Us1 = SetgClasses;

				SetgClasses.Sort();

				Process(Admin);

				Admin.Save();
			}
		}

		private void Process(ITest Admin)
		{
			List<SettingBase> Us1 = SetgClasses;

			for (int i = 0; i < SetgClasses.Count; i++)
			{
				int j = String.Compare(SetgClasses[i].ClassVersion, SetgClasses[0].ClassVersionOfFile, StringComparison.Ordinal);

				if (j == 0)
				{
					SetgClasses[i] = Admin.Read(SetgClasses[i].GetType());
				} 
				else if (j > 0)
				{
					SetgClasses[i].Upgrade(SetgClasses[i - 1]);
				}
			}
		}
	}
}