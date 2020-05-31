#region using directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManager.SampleData;

#endregion

// projname: $projectname$
// itemname: SampleDataManager2
// username: jeffs
// created:  4/11/2020 10:08:15 AM

namespace SettingsManagerV60.SampleData
{
	
	public class SampleDataManager2 : INotifyPropertyChanged
	{
	#region private fields

		private static SampleData2 sd;

		private ObservableCollection<SampleItem2> root;


	#endregion

	#region ctor

		public SampleDataManager2(string dataName)
		{
			sd = new SampleData2(dataName);

			Root = sd.LoadSampleData();

		}

	#endregion

	#region public properties

		public ObservableCollection<SampleItem2> Root
		{
			get => root;
			set
			{
				root = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleDataManager2";
		}

	#endregion
	}
}