using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "MachSettings", Namespace = "")]
	internal class MachSettingInfo<T> : MachSettingInfoBase<T>
		where T : HeaderData, new ()
	{
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class MachSettingData : HeaderData
	{
		public string SetDataDescription() => "machine setting file for SettingsManager v7.3";
		public string SetDataNotes() => "any notes go here";
		public string SetDataClassVersion() => "mach 7.3m";

		[DataMember(Order = 1)]
		public int MachSettingsValue { get; set; } = 7;
	}

#endregion
}