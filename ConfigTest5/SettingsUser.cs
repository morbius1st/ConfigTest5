#region Using directives

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		12/31/2017 8:29:37 AM


// define all of the settings and data structures here
namespace ConfigTest5
{

	// this is the settings for the user configuration
	// this is the same for each program that uses this system


	// alternate form of configuration and access
	// uses factory method??
	// this does work - but looks messy
	// this system auto initalizes the various objects
//	public static class SettingsUser2
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		private static SettingsBase<UserSettings> _uSettings;
//
//		public static SettingsBase<UserSettings> USettings
//		{
//			get
//			{
//				if (_uSettings == null)
//				{
//					Initalize();
//				}
//
//				return _uSettings;
//			}
//		}
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		private static UserSettings _uSet;
//
//		public static UserSettings USet
//		{
//			get
//			{
//				if (_uSet == null)
//				{
//					Initalize();
//				}
//
//				return _uSet;
//			}
//		}
//
//		// initalize and create the setting objects (automatic)
//		private static void Initalize()
//		{
//			_uSettings = new SettingsBase<UserSettings>();
//			_uSet = USettings.Settings;
//			_uSet.Header = new Header(UserSettings.USERSETTINGFILEVERSION);
//		}
//	}


}
