using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class MembershipInformationPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.MembershipInformation;

        public Task Process(GameClient client, byte[] packetData)
        {
            if (client.MembershipExpirationDate != null)
            {
                client.Send(new MembershipPacket(client.MembershipExpirationDate.Value, client.MembershipUtcSeconds));
            }
            else
            {
                client.Send(new MembershipPacket());
            }

            return Task.CompletedTask;
        }
    }
}
