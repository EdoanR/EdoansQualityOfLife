using Terraria;
using Terraria.ModLoader;

namespace EdoansQualityOfLife
{
	public class EdoanGlobalItem : GlobalItem
	{
		public bool extractSpeed;

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			return base.Clone(item, itemClone);
		}

		public override void SetDefaults(Item item)
		{
			if (ModContent.GetInstance<EdoanConfig>().IncreaseMaxStack && item.maxStack > 10 && item.maxStack != 100 && (item.type < 71 || item.type > 74))
			{
				item.maxStack = 9999;
			}
		}

		public override bool CanUseItem(Item item, Player player)
		{
			if (item.type == Terraria.ID.ItemID.SiltBlock || item.type == Terraria.ID.ItemID.SlushBlock || item.type == Terraria.ID.ItemID.DesertFossil)
			{
				if (ModContent.GetInstance<EdoanConfig>().FasterExtractSpeed && player.GetModPlayer<EdoanPlayer>().extractSpeed)
				{
					item.useTime = 2;
					item.useAnimation = 3;
				}
				else
				{
					item.useTime = 10;
					item.useAnimation = 15;
				}
			}
			return base.CanUseItem(item, player);
		}

	}
}