using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SettingManager;
using static SettingManager.SettingsUser;
using static UtilityLibrary.MessageUtilities2;

namespace SettingsManagerV30
{
	[DataContract]
	public abstract class UserSettingBase : SettingsPathFileUserBase
	{
		public override string FileVersion { get; set; }

		public Header Header => Heading;

		public abstract void Upgrade(UserSettingBase prior);
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

				USetgBase.Add(new UserSettings20());
				USetgBase.Add(new UserSettings21());
				USetgBase.Add(USetgData);

				Upgrade();

				USetgAdmin.Save();
			}
		}

		private static void Upgrade()
		{
			List<UserSettingBase> Us1 = USetgBase;

			for (int i = 0; i < USetgBase.Count; i++)
			{
				int j = String.Compare(USetgBase[i].FileVersion, USetgAdmin.GetFileVersion(), StringComparison.Ordinal);

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