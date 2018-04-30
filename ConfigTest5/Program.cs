using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConfigTest5;
using static ConfigTest5.Test;

namespace ConfigTest5
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

			// run the app - test settings usage
			Application.Run(new Form1());

//			Test t = new Test();
//			t.GetTypeInfo();
		}
	}
}
