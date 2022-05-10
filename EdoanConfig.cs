using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EdoansQualityOfLife
{
	public class EdoanConfig : ModConfig
	{

		[Label("$Mods.EdoansQualityOfLife.Common.LongerStationBuffsLabel")]
		[Tooltip("$Mods.EdoansQualityOfLife.Common.LongerStationBuffsTooltip")]
		[DefaultValue(true)]
		public bool LongerStationBuffs;

		[Label("$Mods.EdoansQualityOfLife.Common.StaffOfRegrowthReplantHerbsLabel")]
		[DefaultValue(true)]
		public bool StaffOfRegrowthReplantHerbs;

		[Label("$Mods.EdoansQualityOfLife.Common.LongerCratePotionBuffLabel")]
		[Tooltip("$Mods.EdoansQualityOfLife.Common.LongerCratePotionBuffTooltip")]
		[DefaultValue(true)]
		public bool LongerCratePotionBuff;

		[Label("$Mods.EdoansQualityOfLife.Common.FasterExtractSpeedLabel")]
		[DefaultValue(true)]
		public bool FasterExtractSpeed;

		[Label("$Mods.EdoansQualityOfLife.Common.IncreaseMaxStackLabel")]
		[DefaultValue(true)]
		public bool IncreaseMaxStack;

		[Label("$Mods.EdoansQualityOfLife.Common.GemtreeGuaranteedLabel")]
		[DefaultValue(true)]
		public bool GemtreeGuaranteed;

		[Label("$Mods.EdoansQualityOfLife.Common.IncreasePlayerBuffSlotLabel")]
		[Tooltip("$Mods.EdoansQualityOfLife.Common.IncreasePlayerBuffSlotTooltip")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool IncreasePlayerBuffSlot;

		public override ConfigScope Mode => ConfigScope.ServerSide;

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (!EdoansQualityOfLife.IsPlayerLocalServerOwner(Terraria.Main.player[whoAmI]))
			{
				message = "$Mods.EdoansQualityOfLife.Common.OnlyServerOwnerChangeConfig";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}

	}
}