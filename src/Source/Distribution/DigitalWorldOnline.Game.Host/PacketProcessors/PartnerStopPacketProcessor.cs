using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.GameHost;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartnerStopPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerStop;

        private readonly MapServer _mapServer;

        public PartnerStopPacketProcessor(MapServer mapServer)
        {
            _mapServer = mapServer;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            client.Tamer.Partner.StopAutoAttack();

          
            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new PartnerStopPacket(client.Tamer.Partner.GeneralHandler).Serialize());

            return Task.CompletedTask;
        }
    }
}