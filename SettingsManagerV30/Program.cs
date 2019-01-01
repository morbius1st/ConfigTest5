using System;
using System.Windows.Forms;

using static UtilityLibrary.MessageUtilities2;

namespace SettingsManagerV30
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
			Application.Run( new Form1_V30());
		}
	}
}
