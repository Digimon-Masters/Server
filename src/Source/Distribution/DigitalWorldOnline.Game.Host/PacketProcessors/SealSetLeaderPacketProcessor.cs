using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;

using MediatR;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class SealSetLeaderPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.SetSealLeader;

        private readonly MapServer _mapServer;
        private readonly ISender _sender;

        public SealSetLeaderPacketProcessor(
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

            client.Tamer.SealList.SetLeader(sealSequentialId);

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetSealLeaderPacket(client.Tamer.GeneralHandler, sealSequentialId).Serialize());
            await _sender.Send(new UpdateCharacterSealsCommand(client.Tamer.SealList));
        }
    }
}