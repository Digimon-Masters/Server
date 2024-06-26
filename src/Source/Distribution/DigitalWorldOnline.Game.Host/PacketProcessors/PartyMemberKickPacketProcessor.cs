using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartyMemberKickPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartyMemberKick;

        private const string GameServerAddress = "GameServer:Address";
        private const string GamerServerPublic = "GameServer:PublicAddress";
        private const string GameServerPort = "GameServer:Port";

        private readonly PartyManager _partyManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;

        public PartyMemberKickPacketProcessor(
            PartyManager partyManager,
            MapServer mapServer,
            ILogger logger,
            ISender sender,
            IConfiguration configuration,
            DungeonsServer dungeonServer)
        {
            _partyManager = partyManager;
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;
            _configuration = configuration;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var targetName = packet.ReadString();

            var party = _partyManager.FindParty(client.TamerId);
            if (party == null)
            {
                client.Send(new SystemMessagePacket($"The target tamer is not in a party."));
                _logger.Warning($"Character {client.TamerId} kicked {targetName} from the party but he/she was not in the party.");
                return;
            }

            _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                new PartyMemberKickPacket(party[targetName].Key).Serialize());

            _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                new PartyMemberKickPacket(party[targetName].Key).Serialize());

            if (party.Members.Count == 2)
            {
                var map = UtilitiesFunctions.MapGroup(client.Tamer.Location.MapId);

                var mapConfig = await _sender.Send(new GameMapConfigByMapIdQuery(map));
                var waypoints = await _sender.Send(new MapRegionListAssetsByMapIdQuery(map));

                if (mapConfig == null || waypoints == null || !waypoints.Regions.Any())
                {
                    client.Send(new SystemMessagePacket($"Map information not found for map Id {map}."));
                    _logger.Warning($"Map information not found for map Id {map} on character {client.TamerId} jump booster.");
                    return;
                }
                var destination = waypoints.Regions.First();

                foreach (var member in party.Members)
                {
                    var dungeonClient = _dungeonServer.FindClientByTamerId(member.Value.Id);

                    if (dungeonClient == null)
                    {
                        continue;
                    }

                    _dungeonServer.RemoveClient(dungeonClient);

                    dungeonClient.Tamer.NewLocation(map, destination.X, destination.Y);
                    await _sender.Send(new UpdateCharacterLocationCommand(dungeonClient.Tamer.Location));

                    dungeonClient.Tamer.Partner.NewLocation(map, destination.X, destination.Y);
                    await _sender.Send(new UpdateDigimonLocationCommand(dungeonClient.Tamer.Partner.Location));

                    dungeonClient.Tamer.UpdateState(CharacterStateEnum.Loading);
                    await _sender.Send(new UpdateCharacterStateCommand(dungeonClient.TamerId, CharacterStateEnum.Loading));

                    _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                        new PartyMemberWarpGatePacket(party[dungeonClient.TamerId]).Serialize());



                    dungeonClient.SetGameQuit(false);

                    dungeonClient.Send(new MapSwapPacket(
                        _configuration[GamerServerPublic],
                        _configuration[GameServerPort],
                        dungeonClient.Tamer.Location.MapId,
                        dungeonClient.Tamer.Location.X,
                        dungeonClient.Tamer.Location.Y));
                }
            }

            party.RemoveMember(party[targetName].Key);

            if (party.Members.Count < 2)
            {
                _logger.Verbose($"Character {client.TamerId} removed {targetName} from the party {party.Id}, which was dissolved.");
               
                _partyManager.RemoveParty(party.Id);
            }
            else
                _logger.Verbose($"Character {client.TamerId} removed {targetName} from the party {party.Id}.");
        }
    }
}