#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SettingsManager;

using WpfSharingTest01.Windows;

#endregion

// projname: WpfSharingTest02
// itemname: MainWindowShare02
// username: jeffs
// created:  8/1/2020 9:12:38 AM

namespace WpfSharingTest02.Windows
{
	/// <summary>
	/// Interaction logic for MainWindowShare02.xaml <br/>
	/// Note, since this references WpfSharingTest01, <br/>
	/// do not reference SharedProject01 as this is <br/>
	/// included with WpfSharingTest01
	/// </summary>
	public partial class MainWindowShare02 : Window, INotifyPropertyChanged
	{
	#region private fields

		// UserSetg UserSettings = new UserSetg();
		// AppSetg AppSettings = new AppSetg();

	#endregion

	#region ctor

		public MainWindowShare02()
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

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			// when this window is used / loaded, it will use the current
			// assembly's name and, therefore, this assembly's user & app
			// setting file
			MainWindowShare01 win = new MainWindowShare01();

			bool? r = win.ShowDialog();

		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) { }

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}