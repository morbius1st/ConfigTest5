using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SettingManager;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities2;


namespace SettingsManagerV30
{
	
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
			if (!SetgClasses[0].ClassVersionsMatch())
			{
				logMsgLn2("upgrading", "class versions do not match - upgrade");

				List<SettingBase> Us1 = SetgClasses;

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
			List<SettingBase> Us1 = SetgClasses;

			string v = SetgClasses[0].ClassVersionFromFile;

			for (int i = 0; i < SetgClasses.Count; i++)
			{
				int j = String.Compare(SetgClasses[i].ClassVersion, 
					SetgClasses[i].ClassVersionFromFile, StringComparison.Ordinal);

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