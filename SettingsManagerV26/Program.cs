using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettingsManagerV26
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{

			// these are attempts to "fix" the poor text rendering
			// these work but not for dll's
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// run and test
			Application.Run(new Form1_V26());
		}
	}
}
