#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		12/31/2017 8:29:37 AM


namespace ConfigTest2
{
	public class SettingsUser
	{
		public int TestI { get; set; } = 0;
		public bool TestB { get; set; } = false;
		public double TestD { get; set; } = 0.0;
		public string TestS { get; set; } = "this is a test";
		public int[] TestIs { get; set; } = new[] { 20, 30 };
		public string[] TestSs { get; set; } = new[] { "user 1", "user 2", "user 3" };

		public SettingsUser()
		{
			
		}
	}
}
