using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTest2
{
//	public class ConfigPathDataUser : ConfigPathData
//	{
//		public ConfigPathDataUser(string rPath, string[] sFolder)
//		{
//			rootPath = rPath;
//			subFolders = sFolder;
//		}
//	}
//
//	public class ConfigPathDataApp : ConfigPathData
//	{
//		public ConfigPathDataApp(string rPath, string[] sFolder)
//		{
//			rootPath = rPath;
//			subFolders = sFolder;
//		}
//	}

	public abstract class ConfigPathData
	{
		protected string fileName;

		protected string rootPath;
		protected string[] subFolders;

		public string RootPath => rootPath;
		public int SubFolderCount => subFolders.Length;

		public string FileName => fileName;

		public ConfigPathData() { }

		public ConfigPathData(string rPath, string[] sFolders)
		{
			rootPath = rPath;
			subFolders = sFolders;
		}

		public string SubFolder(int i)
		{
			if (i < 0 ||
				i >= SubFolderCount) return null;

			string path = rootPath;
			for (int j = 0; j < i + 1; j++)
			{
				path += "\\" + subFolders[j];
			}

			return path;
		}

		public abstract string ConfigFileName();

		public abstract bool CreateUserConfigFolder();

		public string ConfigPath
		{
			get
			{
				if (subFolders == null)
				{
					return rootPath;
				}

				return SubFolder(subFolders.Length - 1);
			}
		}

	}
	
}
