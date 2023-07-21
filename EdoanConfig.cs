using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace EdoansQualityOfLife
{
	public class EdoanConfig : ModConfig
	{

		[DefaultValue(true)]
		public bool LongerStationBuffs;

		[DefaultValue(true)]
		public bool StaffOfRegrowthReplantHerbs;

		[DefaultValue(true)]
		public bool LongerCratePotionBuff;

		[DefaultValue(true)]
		public bool FasterExtractSpeed;

		[DefaultValue(true)]
		public bool IncreaseMaxStack;

		[DefaultValue(true)]
		public bool GemtreeGuaranteed;

		[DefaultValue(true)]
		[ReloadRequired]
		public bool IncreasePlayerBuffSlot;

		public override ConfigScope Mode => ConfigScope.ServerSide;

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (!EdoansQualityOfLife.IsPlayerLocalServerOwner(Terraria.Main.player[whoAmI]))
			{
				message = Language.GetTextValue("Mods.EdoansQualityOfLife.Configs.OnlyOwner");
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}

	}
}