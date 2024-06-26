using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;

using MediatR;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class RemoveSealLeaderPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.RemoveSealLeader;

        private readonly MapServer _mapServer;
        private readonly ISender _sender;

        public RemoveSealLeaderPacketProcessor(
            MapServer mapServer,
            ISender sender)
        {
            _mapServer = mapServer;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var sealSequentialId = packet.ReadShort();

            client.Tamer.SealList.SetLeader(0);

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new RemoveSealLeaderPacket(client.Tamer.GeneralHandler, sealSequentialId).Serialize());
            await _sender.Send(new UpdateCharacterSealsCommand(client.Tamer.SealList));
        }
    }
}