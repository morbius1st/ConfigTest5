#region + Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManager;

#endregion


// projname: SettingsManagerV70.SampleData
// itemname: StorageManager
// username: jeffs
// created:  4/28/2020 1:00:32 PM


namespace SettingsManagerV70.SampleData
{

	public class StorageManager<T> : INotifyPropertyChanged
		where T : class, new ()
	{
		private class DataStore :
			BaseSettings<StorageMgrPath,
			StorageMgrInfo<T>, T> { }

		// public BaseSettings<StorageMgrPath,
		// 	StorageMgrInfo<T>, T> DataStore = new 
		// 		BaseSettings<StorageMgrPath, StorageMgrInfo<T>, T>();

		public bool Initialized => DataStore.Admin.Path.HasPathAndFile;

		public bool Read()
		{
			if (!Initialized) return false;
			
			DataStore.Admin.Read();

			OnPropertyChange("Data");

			return true;
		}

		public bool Write()
		{
			if (!Initialized) return false;

			DataStore.Admin.Write();

			return true;
		}

		public T Data => DataStore.Data;
		public  SettingsMgr<StorageMgrPath, StorageMgrInfo<T>, T> Admin => DataStore.Admin;
		public StorageMgrInfo<T> Info => DataStore.Info;

		public void Configure(string rootPath, string filename)
		{
			DataStore.Admin.Path.SubFolders = null;
			DataStore.Admin.Path.RootFolderPath = rootPath;
			DataStore.Admin.Path.FileName = filename;

			DataStore.Admin.Path.ConfigurePathAndFile();

			OnPropertyChange("Initialized");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
