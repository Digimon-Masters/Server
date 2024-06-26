using DigitalWorldOnline.Application.Separar.Commands.Delete;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class GuildDeletePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.GuildDelete;

        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public GuildDeletePacketProcessor(
            MapServer mapServer,
            ILogger logger,
            ISender sender,
            DungeonsServer dungeonServer)
        {
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            if (client.Tamer.Guild != null)
            {
                //TODO: send to members if the client let you delete with members
                _logger.Debug($"Sending guild delete packet for character {client.TamerId}...");
                client.Send(new GuildDeletePacket(client.Tamer.Guild.Name));

                _logger.Information($"Deleting guild for guild {client.Tamer.Guild.Id}...");
                await _sender.Send(new DeleteGuildCommand(client.Tamer.Guild.Id));

                _logger.Verbose($"Character {client.TamerId} deleted guild {client.Tamer.Guild.Id} {client.Tamer.Guild.Name}.");

                client.Tamer.SetGuild();

                if (client.DungeonMap)
                {
                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new UnloadTamerPacket(client.Tamer).Serialize());
                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new LoadTamerPacket(client.Tamer).Serialize());
                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new LoadBuffsPacket(client.Tamer).Serialize());
                }
                else
                {

                    _mapServer.BroadcastForTargetTamers(client.TamerId, new UnloadTamerPacket(client.Tamer).Serialize());
                    _mapServer.BroadcastForTargetTamers(client.TamerId, new LoadTamerPacket(client.Tamer).Serialize());
                    _mapServer.BroadcastForTargetTamers(client.TamerId, new LoadBuffsPacket(client.Tamer).Serialize());
                }
            }
        }
    }
}