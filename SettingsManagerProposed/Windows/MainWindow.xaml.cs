#region using

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using SettingsManager;

#endregion

// projname: SettingsManagerProposed
// itemname: MainWindow
// username: jeffs
// created:  8/15/2020 12:20:30 PM

namespace SettingsManagerV72.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		private const int COLUMN = 30;
		private string nl = Environment.NewLine;
		private string textBoxMessage;

		private static MainWindow me;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			me = this;
		}

	#endregion

	#region public properties

		public static MainWindow Me => me;


		public string TextBoxMessage
		{
			get => textBoxMessage;
			set
			{
				if (value == null)
				{
					textBoxMessage = "";
					OnPropertyChange();
					return;
				}

				textBoxMessage += value;
				OnPropertyChange();

			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void AddMessage<T>(string message1, T message2, bool newline = true)
		{
			TextBoxMessage = addMessage(message1, message2);
		}

		public void AddMessage(string message1, string message2 = null, bool newline = true)
		{
			TextBoxMessage = addMessage(message1, message2);
		}

		public void ClearMessage()
		{
			TextBoxMessage = null;
		}

	#endregion

	#region private methods

		private string addMessage<T>(string message1, T message2, bool newline = true)
		{
			return message1.PadLeft(COLUMN)
				+ (!(message2?.Equals(null)).Equals(null) ? "| " + message2.ToString() : "")
				+ (newline ? nl : "");
		}

		private void process()
		{
			try
			{
				// testUser();
				// testApp();
				// testSuite();
				// testMach();
				testSite();

				// testData();
			}
			catch (Exception e)
			{
				AddMessage("!! EXCEPTION !!", e.Message);

				if (e.InnerException != null)
				{
					AddMessage("Inner Exception", e.InnerException.Message);
				}
			}
		}

		private void testSite()
		{
			AddMessage("Site Settings", "Reading Suite Settings - Get Path");

			SuiteSettings.Admin.Read();

			AddMessage("Site Settings", "Reading 1");

			SiteSettings.Path.RootFolderPath = SuiteSettings.Data.SiteRootPath;
			// SiteSettings.Path.ConfigureFilePath();

			SiteSettings.Admin.Read();

			AddMessage("Site Settings", "Read");
			AddMessage("Site Settings| value", SiteSettings.Data.SiteSettingsValue);
			AddMessage("Site Settings| file type", SiteSettings.Info.FileType);
			AddMessage("Site Settings| desc", SiteSettings.Info.Description);
			AddMessage("Site Settings| data ver", SiteSettings.Info.DataClassVersion);
			AddMessage("Site Settings| notes", SiteSettings.Info.Notes);
			AddMessage("Site Settings| saved by", SiteSettings.Info.Header.SavedBy);

			AddMessage("Site Settings", "Changing Value");

			SiteSettings.Data.SiteSettingsValue = 720;

			AddMessage("Site Settings", "Writing");

			SiteSettings.Admin.Write();

			AddMessage("Site Settings", "Reading 2");

			SiteSettings.Admin.Read();

			AddMessage("Site Settings", "Read");
			AddMessage("Site Settings| value", SiteSettings.Data.SiteSettingsValue);
			AddMessage("Site Settings| file type", SiteSettings.Info.FileType);

			AddMessage("");
		}
		
		private void testMach()
		{
			AddMessage("Mach Settings", "Reading 1");

			MachSettings.Admin.Read();

			AddMessage("Mach Settings", "Read");
			AddMessage("Mach Settings| value", MachSettings.Data.MachSettingsValue);
			AddMessage("Mach Settings| file type", MachSettings.Info.FileType);
			AddMessage("Mach Settings| desc", MachSettings.Info.Description);
			AddMessage("Mach Settings| data ver", MachSettings.Info.DataClassVersion);
			AddMessage("Mach Settings| notes", MachSettings.Info.Notes);
			AddMessage("Mach Settings| saved by", MachSettings.Info.Header.SavedBy);

			AddMessage("Mach Settings", "Changing Value");

			MachSettings.Data.MachSettingsValue = 720;

			AddMessage("Mach Settings", "Writing");

			MachSettings.Admin.Write();

			AddMessage("Mach Settings", "Reading 2");

			MachSettings.Admin.Read();

			AddMessage("Mach Settings", "Read");
			AddMessage("Mach Settings| value", MachSettings.Data.MachSettingsValue);
			AddMessage("Mach Settings| file type", MachSettings.Info.FileType);

			AddMessage("");
		}

		private void testSuite()
		{
			AddMessage("Suite Settings", "Reading 1");

			SuiteSettings.Admin.Read();

			AddMessage("Suite Settings", "Read");
			AddMessage("Suite Settings| value", SuiteSettings.Data.SuiteSettingsValue);
			AddMessage("Suite Settings| file type", SuiteSettings.Info.FileType);
			AddMessage("Suite Settings| desc", SuiteSettings.Info.Description);
			AddMessage("Suite Settings| data ver", SuiteSettings.Info.DataClassVersion);
			AddMessage("Suite Settings| notes", SuiteSettings.Info.Notes);
			AddMessage("Suite Settings| saved by", SuiteSettings.Info.Header.SavedBy);

			AddMessage("Suite Settings", "Changing Value");

			SuiteSettings.Data.SuiteSettingsValue = 720;

			AddMessage("Suite Settings", "Writing");

			SuiteSettings.Admin.Write();

			AddMessage("Suite Settings", "Reading 2");

			SuiteSettings.Admin.Read();

			AddMessage("Suite Settings", "Read");
			AddMessage("Suite Settings| value", SuiteSettings.Data.SuiteSettingsValue);
			AddMessage("Suite Settings| file type", SuiteSettings.Info.FileType);

			AddMessage("");
		}
		

		private void testApp()
		{
			AddMessage("App Settings", "Reading 1");

			AppSettings.Admin.Read();

			AddMessage("App Settings", "Read");
			AddMessage("App Settings| value", AppSettings.Data.AppSettingsValue);
			AddMessage("App Settings| file type", AppSettings.Info.FileType);
			AddMessage("App Settings| desc", AppSettings.Info.Description);
			AddMessage("App Settings| data ver", AppSettings.Info.DataClassVersion);
			AddMessage("App Settings| notes", AppSettings.Info.Notes);
			AddMessage("App Settings| saved by", AppSettings.Info.Header.SavedBy);

			AddMessage("App Settings", "Changing Value");

			AppSettings.Data.AppSettingsValue = 720;

			AddMessage("App Settings", "Writing");

			AppSettings.Admin.Write();

			AddMessage("App Settings", "Reading 2");

			AppSettings.Admin.Read();

			AddMessage("App Settings", "Read");
			AddMessage("App Settings| value", AppSettings.Data.AppSettingsValue);
			AddMessage("App Settings| file type", AppSettings.Info.FileType);

			AddMessage("");
		}

		private void testUser()
		{
			AddMessage("User Settings", "Reading 1");

			UserSettings.Admin.Read();

			AddMessage("User Settings", "Read");
			AddMessage("User Settings| value", UserSettings.Data.UserSettingsValue);
			AddMessage("User Settings| file type", UserSettings.Info.FileType);
			AddMessage("User Settings| desc", UserSettings.Info.Description);
			AddMessage("User Settings| data ver", UserSettings.Info.DataClassVersion);
			AddMessage("User Settings| notes", UserSettings.Info.Notes);
			AddMessage("User Settings| saved by", UserSettings.Info.Header.SavedBy);

			AddMessage("User Settings", "Changing Value");

			UserSettings.Data.UserSettingsValue = 720;

			AddMessage("User Settings", "Writing");

			UserSettings.Admin.Write();

			AddMessage("User Settings", "Reading 2");

			UserSettings.Admin.Read();

			AddMessage("User Settings", "Read");
			AddMessage("User Settings| value", UserSettings.Data.UserSettingsValue);
			AddMessage("User Settings| file type", UserSettings.Info.FileType);

			AddMessage("");
		}

		private void testData()
		{
			AddMessage("Data Settings", "Test");

			
			BaseDataFile<DataSet1> ds1_1 = new BaseDataFile<DataSet1>();


			BaseDataFile<DataSet1> ds1_2 = new BaseDataFile<DataSet1>();

			AddMessage("Data Set 1_1", "configure");
			ds1_1.Configure(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerProposed",
				@"DataSet1_1.xml" );

			AddMessage("Data Set 1_1", "configured");

			AddMessage("Data Set 1_2", "configure");
			ds1_2.Configure(
				@"C:\Users\jeffs\AppData\Roaming\CyberStudio\SettingsManager\SettingsManagerProposed",
				@"DataSet1_2.xml" );
			AddMessage("Data Set 1_2", "configured");

			// ds1_1.Admin.Write();

			ds1_1.Admin.Read();

			testDs1_1(ds1_1);
			testDs1_2(ds1_2);

		}

		private void testDs1_1(BaseDataFile<DataSet1> ds1_1)
		{
			if (ds1_1.Initialized)
			{
				listDataSet(ds1_1, "Data Set 1_1");

				AddMessage("Data Set 1_1", "changing");

				ds1_1.Info.Description = "DataSet 1_1";
				ds1_1.Info.DataClassVersion = "1.1.0.0";
				ds1_1.Info.Notes = "Initial Save 1_1";

				ds1_1.Data.SampleDataString1 = "Sample data set 1_1";
				ds1_1.Data.SampleDataDouble1 = 117.2;

				AddMessage("Data Set 1_1", "writing");
				ds1_1.Admin.Write();
				AddMessage("Data Set 1_1", "written");

				listDataSet(ds1_1, "Data Set 1_1");
			}
		}

		private void testDs1_2(BaseDataFile<DataSet1> ds1_2)
		{
			if (ds1_2.Initialized)
			{
				listDataSet(ds1_2, "Data Set 1_2");

				AddMessage("Data Set 1_1", "changing");

				ds1_2.Info.Description = "DataSet 1_2";
				ds1_2.Info.DataClassVersion = "1.2.0.0";
				ds1_2.Info.Notes = "Initial Save 1_2";

				ds1_2.Data.SampleDataString1 = "Sample data set 1_2";
				ds1_2.Data.SampleDataDouble1 = 127.2;

				AddMessage("Data Set 1_1", "writing");
				ds1_2.Admin.Write();
				AddMessage("Data Set 1_1", "written");

				listDataSet(ds1_2, "Data Set 1_2");
			}
		}
		private void listDataSet(BaseDataFile<DataSet1> ds, string title)
		{
			if (ds.Initialized)
			{
				AddMessage(title, "reading");
				ds.Admin.Read();
				AddMessage(title, "read");

				AddMessage(title, "SampleDataString1| " + ds.Data.SampleDataString1);
				AddMessage(title, "SampleDataDouble1| " + ds.Data.SampleDataDouble1);
			}
		}


	#endregion

	#region event processing

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {this.Close(); }

		private void winMainSetgMgrProposed_Loaded(object sender, RoutedEventArgs e)
		{
			process();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	}
}