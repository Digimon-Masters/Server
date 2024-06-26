using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TamerReactionPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TamerReaction;

        private readonly MapServer _mapServer;

        public TamerReactionPacketProcessor(MapServer mapServer)
        {
            _mapServer = mapServer;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var type = packet.ReadInt(); //TODO: enum?
            var value = packet.ReadInt(); //TODO: enum?

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                new TamerReactionPacket(client.Tamer.GeneralHandler, type, value).Serialize());

            return Task.CompletedTask;
        }
    }
}