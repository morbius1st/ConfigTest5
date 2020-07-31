#region using directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#endregion


// projname: $projectname$
// itemname: SampleItem2
// username: jeffs
// created:  4/11/2020 9:51:32 AM

namespace SettingsManager.SampleData
{
	[DataContract(Name = "SampleItem2", Namespace = "")]
	public class SampleItem2 : INotifyPropertyChanged
	{
	#region private fields

		private ObservableCollection<SampleItem2> leaves = null;
		private string name;
		private string data;

	#endregion

	#region ctor

		public SampleItem2(string name, string data)
		{
			this.name = name;
			this.data = data;
		}

	#endregion

	#region public properties


		[DataMember(Order = 1)]
		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 2)]
		public string Data
		{
			get => data;
			set
			{
				data = value;
				OnPropertyChange();
			}
		}

		[DataMember(Order = 3)]
		public ObservableCollection<SampleItem2> Leaves
		{
			get => leaves;
			set
			{ 
				leaves = value;
				OnPropertyChange();
			}
		}

		public bool IsBranch
		{
			get => leaves != null;

		}


	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleItem2";
		}

	#endregion
	}
}