using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace EdoansQualityOfLife
{
	public class EdoansQualityOfLife : Mod
	{
        public enum HerbStyle
        {
            Daybloom = 0,
            Moonglow = 1,
            Blinkroot = 2,
            Deathweed = 3,
            Waterleaf = 4,
            Fireblossom = 5,
            Shiverthorn = 6
        }

        public override void Load()
        {
            On.Terraria.Player.AddBuff += Player_AddBuff;

            On.Terraria.Player.PlaceThing_Tiles_BlockPlacementForAssortedThings += Player_PlaceThing_Tiles_BlockPlacementForAssortedThings;
            On.Terraria.WorldGen.KillTile_GetItemDrops += WorldGen_KillTile_GetItemDrops;
            On.Terraria.WorldGen.SetGemTreeDrops += WorldGen_SetGemTreeDrops;
            
        }

        public override uint ExtraPlayerBuffSlots
        {
            get
            {
                if (ModContent.GetInstance<EdoanConfig>().IncreasePlayerBuffSlot)
                {
                    return 200u;
                } else
                {
                    return base.ExtraPlayerBuffSlots;
                }
            }
        }

        private void WorldGen_SetGemTreeDrops(On.Terraria.WorldGen.orig_SetGemTreeDrops orig, int gemType, int seedType, Tile tileCache, ref int dropItem, ref int secondaryItem)
        {
            if (!ModContent.GetInstance<EdoanConfig>().GemtreeGuaranteed)
            {
                orig.Invoke(gemType, seedType, tileCache, ref dropItem, ref secondaryItem);
                return;
            }

            dropItem = gemType;
            if (tileCache.TileFrameX >= 22 && tileCache.TileFrameY >= 198 && Main.rand.NextBool(2))
            {
                secondaryItem = seedType;
            }
        }

        private void Player_AddBuff(On.Terraria.Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
        {
            if (ModContent.GetInstance<EdoanConfig>().LongerStationBuffs)
            {
                switch (type)
                {
                    case Terraria.ID.BuffID.AmmoBox:
                    case Terraria.ID.BuffID.Bewitched:
                    case Terraria.ID.BuffID.Clairvoyance:
                    case Terraria.ID.BuffID.Sharpened:
                    case Terraria.ID.BuffID.SugarRush:
                        timeToAdd = 432000; // 2 hours.
                        break;
                    default:
                        break;
                }
            }

            if (type == Terraria.ID.BuffID.Crate && ModContent.GetInstance<EdoanConfig>().LongerCratePotionBuff)
            {
                timeToAdd = 8 * 60 * 60;
            }
            
            orig.Invoke(self, type, timeToAdd, quiet, foodHack);
        }

        // This is to change the amount of seeds that herbs can drop.
        private void WorldGen_KillTile_GetItemDrops(On.Terraria.WorldGen.orig_KillTile_GetItemDrops orig, int x, int y, Tile tileCache, out int dropItem, out int dropItemStack, out int secondaryItem, out int secondaryItemStack, bool includeLargeObjectDrops)
        {

            if (!ModContent.GetInstance<EdoanConfig>().StaffOfRegrowthReplantHerbs)
            {
                orig.Invoke(x, y, tileCache, out dropItem, out dropItemStack, out secondaryItem, out secondaryItemStack, includeLargeObjectDrops);
                return;
            }

            secondaryItem = 0;
            secondaryItemStack = 1;

            var player = EdoansQualityOfLife.GetPlayerForTile(x, y);
            bool shouldModifyDrop = EdoansQualityOfLife.IsHarvestable(tileCache, player);

            if (shouldModifyDrop)
            {
                int style = tileCache.TileFrameX / 18;
                dropItem = Terraria.ID.ItemID.Daybloom + style;
                int seedID = Terraria.ID.ItemID.DaybloomSeeds + style;
                if (style == (int)HerbStyle.Shiverthorn)
                {
                    dropItem = Terraria.ID.ItemID.Shiverthorn;
                    seedID = Terraria.ID.ItemID.ShiverthornSeeds;
                }

                dropItemStack = Main.rand.Next(1, 3);

                int seedAmount = Main.rand.Next(0, 5); // Original value: 1, 6
                if (seedAmount > 0)
                {
                    secondaryItem = seedID;
                    secondaryItemStack = seedAmount;
                }

            }
            else
            {
                orig.Invoke(x, y, tileCache, out dropItem, out dropItemStack, out secondaryItem, out secondaryItemStack, includeLargeObjectDrops);

            }
        }

        // This is to change the effect of the Staff of Regrowth.
        private bool Player_PlaceThing_Tiles_BlockPlacementForAssortedThings(On.Terraria.Player.orig_PlaceThing_Tiles_BlockPlacementForAssortedThings orig, Player self, bool canPlace)
        {

            if (!ModContent.GetInstance<EdoanConfig>().StaffOfRegrowthReplantHerbs)
            {
                return orig.Invoke(self, canPlace);
            }

            int targetX = Player.tileTargetX;
            int targetY = Player.tileTargetY;

            var tile = Main.tile[targetX, targetY];
            int style = tile.TileFrameX / 18;

            bool isImmature = tile.TileType == Terraria.ID.TileID.ImmatureHerbs;

            // Prevent staff of regrowth breaking immature herbs.
            // This will prevent from breaking then placing and then breaking again the herb tile on pots or herbs plaftorms.
            if (isImmature && self.inventory[self.selectedItem].type == Terraria.ID.ItemID.StaffofRegrowth)
            {
                return false;
            }

            var shouldPlace = EdoansQualityOfLife.IsHarvestable(tile, self);

            // After this Invoke the tile might be null, because might be destroyed.
            // Because of that is important to get the info and check before this.
            canPlace = orig.Invoke(self, canPlace);

            if (!canPlace && shouldPlace)
            {
                // Place the seed.
                WorldGen.PlaceTile(i: targetX, j: targetY, Type: Terraria.ID.TileID.ImmatureHerbs, style: style);

                if (Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendTileSquare(-1, targetX, targetY, Terraria.ID.TileChangeType.None);
                }

            }

            return canPlace;
        }

        public static bool IsHarvestable(Tile tile, Player player)
        {
            if (tile == null) return false;
            if (player == null) return false;

            int style = tile.TileFrameX / 18;

            // Player is not using the Staff of Regrowth.
            if (player.inventory[player.selectedItem].type != Terraria.ID.ItemID.StaffofRegrowth) return false;

            // Checking if the tile is a herb and if is ready to harvest.
            if (tile.TileType != Terraria.ID.TileID.MatureHerbs && tile.TileType != Terraria.ID.TileID.BloomingHerbs)
            {
                return false;
            }
            bool harvestable = WorldGen.IsHarvestableHerbWithSeed(tile.TileType, style);
            if (!harvestable) return false;

            // Can safely replant the seed!
            return true;
        }

        // This is the same as the Worldgen.GetPlayerForTile, but inaccessible for being private.
        private static Player GetPlayerForTile(int x, int y) => Main.player[Player.FindClosest(new Vector2(x, y) * 16f, 16, 16)];

        public static bool IsPlayerLocalServerOwner(Player player)
        {
            if (Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int plr = 0; plr < Main.maxPlayers; plr++)
                if (Netplay.Clients[plr].State == 10 && Main.player[plr] == player && Netplay.Clients[plr].Socket.GetRemoteAddress().IsLocalHost())
                    return true;
            return false;
        }
    }
}