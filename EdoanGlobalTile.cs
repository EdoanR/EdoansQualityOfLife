using Terraria;
using Terraria.ModLoader;

namespace EdoansQualityOfLife
{
	public class EdoanGlobalTile : GlobalTile
	{
		public override void MouseOver(int i, int j, int type)
		{
			if (type == 219)
			{
				Main.player[Main.myPlayer].GetModPlayer<EdoanPlayer>().extractSpeed = true;
			}
		}

	}
}