﻿using System.Runtime.Serialization;

// File:             SiteSettings.cs
// Created:      -- ()

// ReSharper disable once CheckNamespace

namespace SettingsManager
{
#region info class

	[DataContract(Name = "SiteSettings", Namespace = "")]
	internal class SiteSettingInfo<T> : SiteSettingInfoBase<T>
		where T : new ()
	{
		public SiteSettingInfo()
		{
			// these are specific to this data file
			DataClassVersion =  "site 7.2as";
			Description =  "site setting file for SettingsManager v7.2";
			Notes = "any notes goes here";
		}

		internal override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class SiteSettingData
	{
		[DataMember(Order = 1)]
		public int SiteSettingsValue { get; set; } = 7;
	}

#endregion
}