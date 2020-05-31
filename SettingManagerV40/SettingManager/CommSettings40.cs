#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SettingManager;

#endregion


// projname: SettingManagerV40
// itemname: CommSettings40
// username: jeffs
// created:  4/5/2020 4:39:43 PM


namespace SettingManagerV40
{
//
//#region Comm data class
//
//	// this is the actual data set saved to the user's configuration file
//	// this is unique for each program
//	[DataContract(Name = "CommSettingData40")]
//	public class CommSettingData40
//	{
//		[DataMember(Order = 1)]
//		public int CommSettingsValue { get; set; } = 2;
//
//	}
//
//#endregion
//
//#region management info class
//
//	[DataContract(Name = "CommSettingInfo40")]
//	public class CommSettingInfo40 : CommSettingBase
//	{
//		[DataMember]
//		public SettingManagerV40.CommSettingData40 Data = new SettingManagerV40.CommSettingData40();
//
//		public override string ClassVersion => "4.0c";
//
//		public override void UpgradeFromPrior(SettingBase prior) { }
//	}
//
//#endregion
//
//#region management root class
//
//	public static class CommSettings
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		public static SettingsMgr<SettingManagerV40.CommSettingInfo40> Admin { get; private set; }
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		public static SettingManagerV40.CommSettingInfo40 Info { get; private set; }
//		public static SettingManagerV40.CommSettingData40 Data { get; private set; }
//
//		// initialize and create the setting objects
//		static CommSettings()
//		{
//			Admin = new SettingsMgr<SettingManagerV40.CommSettingInfo40>(ResetData);
//			Info = Admin.Info;
//			Data = Info.Data;
//
//			MainWindow.Instance.MsgLeftLine("");
//			MainWindow.Instance.MsgLeftLine("at ctor CommSettings|   status", Admin.Status.ToString());
//			MainWindow.Instance.MsgLeftLine("at ctor CommSettings|     path", Admin.Info.SettingPath);
//			MainWindow.Instance.MsgLeftLine("at ctor CommSettings| filename", Admin.Info.FileName);
//
//		}
//
//		public static void ResetData()
//		{
//			// this makes sure the above static class points
//			// to the current data structure
//			Info  = Admin.Info;
//			Data  = Info.Data;
//		}
//	}
//
//#endregion

}
