using Terraria.ModLoader;

namespace EdoansQualityOfLife
{
	public class EdoanPlayer : ModPlayer
	{
		public bool extractSpeed;

		public override void ResetEffects()
		{
			this.extractSpeed = false;
		}

	}
}