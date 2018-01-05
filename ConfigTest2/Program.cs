using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConfigTest2;

namespace ConfigTest2
{
	// version 1.0.1.1 - at xml save changed "file.open" to "file.create"
	static class Program
	{
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
