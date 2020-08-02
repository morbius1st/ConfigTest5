#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SettingsManager;
using UtilityLibrary;

#endregion

// projname: WpfSharingTest01
// itemname: MainWindowShare01
// username: jeffs
// created:  8/1/2020 9:12:14 AM

namespace WpfSharingTest01.Windows
{
	/// <summary>
	/// Interaction logic for MainWindowShare01.xaml
	/// </summary>
	public partial class MainWindowShare01 : Window, INotifyPropertyChanged
	{
	#region private fields

		// UserSetg UserSettings = new UserSetg();
		// AppSetg AppSettings = new AppSetg();

	#endregion

	#region ctor

		public MainWindowShare01()
		{
			InitializeComponent();
			

		}

	#endregion

	#region public properties

		public string UserSettingFilePath => UserSettings.Path.SettingFilePath;

		public string UserSettingFileDescription => UserSettings.Info.Description;

		public string AppSettingFilePath => AppSettings.Path.SettingFilePath;

		public string AppSettingFileDescription => AppSettings.Info.Description;

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UserSettings.Admin.Read(); 
			UserSettings.Admin.Write();

			OnPropertyChange("UserSettingFilePath");
			OnPropertyChange("UserSettingFileDescription");

			AppSettings.Admin.Read();
			AppSettings.Admin.Write();

			OnPropertyChange("AppSettingFilePath");
			OnPropertyChange("AppSettingFileDescription");

		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}
}