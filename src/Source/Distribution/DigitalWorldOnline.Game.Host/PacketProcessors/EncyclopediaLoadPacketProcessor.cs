using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class EncyclopediaLoadPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.EncyclopediaLoad;

        public EncyclopediaLoadPacketProcessor() { }

        public async Task Process(GameClient client, byte[] packetData)
        {
            for (int i = 0; i <= 100; i++)
            {
                PacketWriter writer = new();
                writer.Type(3234);
                writer.WriteInt(client.Partner.Evolutions.Count(x => x.Unlocked > 0));
                writer.WriteInt(client.Partner.BaseType);
                writer.WriteShort(client.Partner.Level);

                writer.WriteInt64(8);

                writer.WriteShort(client.Partner.Digiclone.ATLevel);
                writer.WriteShort(client.Partner.Digiclone.BLLevel);
                writer.WriteShort(client.Partner.Digiclone.CTLevel);
                writer.WriteShort(client.Partner.Digiclone.EVLevel);
                writer.WriteShort(client.Partner.Digiclone.HPLevel);

                writer.WriteShort(client.Partner.Size);
                writer.WriteByte(0);

                writer.WriteByte(0);

                client.Send(writer.Serialize());
            }
        }
    }
}