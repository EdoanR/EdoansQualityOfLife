using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EdoansQualityOfLife
{
	public class EdoanConfig : ModConfig
	{

		[Label("[i:2999] Longer Station Buffs")]
		[Tooltip("It will last 2 hours.")]
		[DefaultValue(true)]
		public bool LongerStationBuffs;

		[Label("[i:213] Staff of Regrowth Replant Herbs")]
		[DefaultValue(true)]
		public bool StaffOfRegrowthReplantHerbs;

		[Label("[i:2356] Longer Crate Potion Buff")]
		[Tooltip("It will last 8 min instead of 3 min.")]
		[DefaultValue(true)]
		public bool LongerCratePotionBuff;

		[Label("[i:997] Faster Extractinator")]
		[DefaultValue(true)]
		public bool FasterExtractSpeed;

		[Label("[i:2] Increased Max Stacks")]
		[DefaultValue(true)]
		public bool IncreaseMaxStack;

		[Label("[i:4856] Gem Trees Always Drop Gems")]
		[DefaultValue(true)]
		public bool GemtreeGuaranteed;

		public override ConfigScope Mode => ConfigScope.ServerSide;

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (!EdoansQualityOfLife.IsPlayerLocalServerOwner(Terraria.Main.player[whoAmI]))
			{
				message = "Only the server owner can change this config";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}

	}
}