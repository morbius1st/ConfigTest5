#region using directives
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManagerV60.SampleData;

#endregion

// projname: SettingsManagerV60
// itemname: StoreAndReadSampleData
// username: jeffs
// created:  4/16/2020 9:38:57 PM

namespace SettingsManager.SampleData
{
#region data manager class
	public class SarMgr : INotifyPropertyChanged
	{
		private static readonly Lazy<SarMgr> instance =
			new Lazy<SarMgr>(()=> new SarMgr());

		private SarMgr()
		{

			SampleDataStore.Admin.Path.FileName = @"SampleData.xml";
//			SampleDataStore.Admin.Path.RootPath =  CsUtilities.AssemblyDirectory;
//			SampleDataStore.Admin.Path.SubFolders = new [] { "SettingsManagerData" };
			SampleDataStore.Admin.Path.SubFolders = null;
			SampleDataStore.Admin.Path.RootPath =  @"B:\Programming\VisualStudioProjects\SettingsManager\SettingsManagerV60\SettingsManagerData";

			Read();
			
		}

		public static SarMgr Instance => instance.Value;

		public void Read()
		{
			SampleDataStore.Admin.Read();

			OnPropertyChange("Root");
//			OnPropertyChange("Instance");
		}
		
		public void Write()
		{
			SampleDataStore.Admin.Write();

			OnPropertyChange("Root");
		}

		public ObservableCollection<SampleItem2> Root
		{
			get => SampleDataStore.Data.DataRoot;
			set
			{
				SampleDataStore.Data.DataRoot = value;
				OnPropertyChange();
			}
		}

		public int SampleValue
		{
			get => SampleDataStore.Data.SampleValue;
			set
			{
				SampleDataStore.Data.SampleValue = value;
				OnPropertyChange();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

#endregion


}
