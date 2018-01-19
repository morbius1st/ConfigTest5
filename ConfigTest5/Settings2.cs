#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;



#endregion

// itemname:	Settings2
// username:	jeffs
// created:		1/17/2018 10:26:01 PM


namespace ConfigTest5
{
	static class Settings2
	{
		
		private static XmlWriterSettings xmlSettings = new XmlWriterSettings() {Indent = true};

		public static XSettingsUser xUset = new XSettingsUser();
		public static XSettingsApp xAset = new XSettingsApp();



		public static void Save<T>(T obj) where T: class, test1
		{
			DataContractSerializer ds = new DataContractSerializer(typeof(T));

			using (XmlWriter w = XmlWriter.Create(obj.filePathAndName, xmlSettings))
			{
				ds.WriteObject(w, obj);
			}

		}

		public static void Read<T>(T obj) where T: class, test1
		{
			DataContractSerializer ds = new DataContractSerializer(typeof(T));

			using (FileStream fs = File.Open(obj.filePathAndName, FileMode.Open))
			{
				obj = (T) ds.ReadObject(fs);
				if (obj == null)
				{
					Console.WriteLine("read failed");
					obj = null;
				}
				else
				{
					Console.WriteLine("read worked: {0}", obj.GetType());
				}
			}
		}

	}

	[DataContract(Namespace = "http://www.cyberstudio.pro", Name = "UserSettings")]
	class XSettingsUser : test1
	{
		public string filePathAndName { get; set; }
			= @"B:\Programming\VisualStudioProjects\ConfigTest5-DataContract\ConfigTest5\data\UserTest.xml";

		[DataMember(Name = "specialInt")]
		public int int1 { get; set; }= 1;

		[DataMember(Name = "specialString")]
		public string string1 { get; set; } = "string 1";
	}

	[DataContract(Namespace = "http://www.cyberstudio.pro", Name = "AppSettings")]
	class XSettingsApp : test1
	{
		public string filePathAndName { get; set; } =
			@"B:\Programming\VisualStudioProjects\ConfigTest5-DataContract\ConfigTest5\data\AppTest.xml";

		[DataMember(Name = "specialInt2")]
		public int int1 { get; set; } = 2;

		[DataMember(Name = "specialString2")]
		public string string1 { get; set; } = "four";
	}

	public interface test1
	{
		string filePathAndName { get; set; }

//		int int1 { get; set; }
//
//		string string1 { get; set; }

	}
}
