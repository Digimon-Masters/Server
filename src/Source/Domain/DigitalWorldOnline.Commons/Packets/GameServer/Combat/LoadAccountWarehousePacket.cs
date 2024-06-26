﻿using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer.Combat
{
    public class LoadAccountWarehousePacket : PacketWriter
    {
        private const int PacketNumber = 3930;

        /// </summary>
        /// <param name="giftStorage">The list of Gift Storage</param>
        public LoadAccountWarehousePacket(ItemListModel accountWarehouse)
        {
            Type(PacketNumber);
            WriteShort(accountWarehouse.Count);
            WriteBytes(accountWarehouse.ToArray());
        }
    }
}