#region using directives

using System.Collections.ObjectModel;
using SettingsManager.SampleData;

#endregion

// projname: $projectname$
// itemname: SampleData
// username: jeffs
// created:  4/11/2020 10:14:11 AM

namespace SettingsManagerV70.SampleData
{
	class SampleData2
	{
	#region private fields

		private string dataName;

	#endregion

	#region ctor

		public SampleData2(string dataName)
		{
			this.dataName = dataName;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public ObservableCollection<SampleItem2> LoadSampleData()
		{
			ObservableCollection<SampleItem2> root = new ObservableCollection<SampleItem2>();

//			SampleItem2 SubSubBranch;
//			SampleItem2 subBranch;
//			SampleItem2 branch;

			// leaf 1
			root.Add(MakeItem(1.0, "leaf"));

			// branch 2
			root.Add(SubLeaves(2.0, 3, 0.1, 0));

			// leaf 3
			root.Add(MakeItem(3.0, "leaf"));

			// branch 4
			root.Add(SubLeaves(4.0, 3, 0.1, 2));

			return root;
		}

	#endregion

	#region private methods

		private SampleItem2 SubLeaves(double branchId, int qty, double increment, int depth)
		{
			ObservableCollection<SampleItem2> branch;

			SampleItem2 item2;

			double leafId = branchId;

			branch = new ObservableCollection<SampleItem2>();

			for (int i = 0; i < qty; i++)
			{
				leafId += increment;

				if (i == 1 && depth > 0)
				{
					item2 = SubLeaves(leafId, qty, increment / 10, depth - 1);
				}
				else
				{
					item2 = MakeItem(leafId, "leaf");
				}

				branch.Add(item2);
			}

			// branch 2
			item2 = new SampleItem2($"branch{dataName} {branchId:N}", $"branch{dataName} {branchId:N} data");

			item2.Leaves = branch;

			return item2;
		}

		private SampleItem2 MakeItem(double leafId, string name)
		{
			return new SampleItem2($"{name}{dataName} {leafId:G}", $"{name}{dataName} {leafId:G} data");
		}

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SampleData";
		}

	#endregion
	}
}