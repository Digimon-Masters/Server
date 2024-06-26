using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;


using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class WarpGateDungeonPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.WarpGateDungeon;

        private const string GameServerAddress = "GameServer:Address";
        private const string GamerServerPublic = "GameServer:PublicAddress";
        private const string GameServerPort = "GameServer:Port";

        private readonly PartyManager _partyManager;
        private readonly IConfiguration _configuration;
        private readonly AssetsLoader _assets;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ISender _sender;
        private readonly ILogger _logger;

        public WarpGateDungeonPacketProcessor(
            PartyManager partyManager,
            IConfiguration configuration,
            AssetsLoader assets,
            MapServer mapServer,
            ISender sender,
            ILogger logger,
            DungeonsServer dungeonServer)
        {
            _partyManager = partyManager;
            _configuration = configuration;
            _assets = assets;
            _mapServer = mapServer;
            _sender = sender;
            _logger = logger;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var portalId = packet.ReadInt();

            var portal = _assets.Portal.FirstOrDefault(x => x.Id == portalId);

            var portalRequestInfo = _assets.Npcs.FirstOrDefault(x => x.NpcId == portal.NpcId).Portals.ToList();

            if (portal == null)
            {
                client.Send(new SystemMessagePacket($"Portal {portalId} not found."));
                _logger.Error($"Portal id {portalId} not found.");


                var mapId = client.Tamer.Location.MapId;

                var mapConfig = await _sender.Send(new GameMapConfigByMapIdQuery(mapId));
                var waypoints = await _sender.Send(new MapRegionListAssetsByMapIdQuery(mapId));
                if (mapConfig == null || waypoints == null || !waypoints.Regions.Any())
                {
                    client.Send(new SystemMessagePacket($"Map information not found for {mapId}"));
                    _logger.Error($"Map information not found for {mapId}");
                    return;
                }

                if (client.DungeonMap)
                {
                    _dungeonServer.RemoveClient(client);
                }
                else
                {
                    _mapServer.RemoveClient(client);
                }

                var destination = waypoints.Regions.First();

                client.Tamer.NewLocation(mapId, destination.X, destination.Y);
                await _sender.Send(new UpdateCharacterLocationCommand(client.Tamer.Location));

                client.Tamer.Partner.NewLocation(mapId, destination.X, destination.Y);
                await _sender.Send(new UpdateDigimonLocationCommand(client.Tamer.Partner.Location));

                client.Tamer.UpdateState(CharacterStateEnum.Loading);
                await _sender.Send(new UpdateCharacterStateCommand(client.TamerId, CharacterStateEnum.Loading));

                client.Send(
                    new MapSwapPacket(
                        _configuration[GamerServerPublic],
                        _configuration[GameServerPort],
                        mapId,
                        destination.X,
                        destination.Y
                    )
                );
            }
            else
            {
                if (portalRequestInfo != null)
                {
                    var Request = portalRequestInfo.SelectMany(x => x.PortalsAsset).ToList();

                    var RemoveInfo = Request[portal.PortalIndex];

                    for (int i = 0; i < 3; i++)
                    {

                        switch (RemoveInfo.npcPortalsAsset[i].Type)
                        {
                            case NpcResourceTypeEnum.Money:
                                {

                                    client.Tamer.Inventory.RemoveBits(RemoveInfo.npcPortalsAsset[i].ItemId);

                                    await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));

                                }
                                break;

                            case NpcResourceTypeEnum.Item:
                                {
                                    var targeItem = client.Tamer.Inventory.FindItemById(RemoveInfo.npcPortalsAsset[i].ItemId);

                                    if (targeItem != null)
                                    {
                                        client.Tamer.Inventory.RemoveOrReduceItem(targeItem, 1)
                                            ;
                                        await _sender.Send(new UpdateItemCommand(targeItem));
                                    }

                                }
                                break;
                        }
                    }
                }

                _mapServer.RemoveClient(client);

                client.Tamer.NewLocation(portal.DestinationMapId, portal.DestinationX, portal.DestinationY);
                await _sender.Send(new UpdateCharacterLocationCommand(client.Tamer.Location));

                client.Tamer.Partner.NewLocation(portal.DestinationMapId, portal.DestinationX, portal.DestinationY);
                await _sender.Send(new UpdateDigimonLocationCommand(client.Tamer.Partner.Location));

                client.Tamer.UpdateState(CharacterStateEnum.Loading);
                await _sender.Send(new UpdateCharacterStateCommand(client.TamerId, CharacterStateEnum.Loading));

                client.SetGameQuit(false);

                client.Send(
                    new MapSwapPacket(
                        _configuration[GamerServerPublic],
                        _configuration[GameServerPort],
                        client.Tamer.Location.MapId,
                        client.Tamer.Location.X,
                        client.Tamer.Location.Y
                    )
                );

                var party = _partyManager.FindParty(client.TamerId);
                if (party != null)
                {
                    party.UpdateMember(party[client.TamerId]);

                    _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                        new PartyMemberWarpGatePacket(party[client.TamerId]).Serialize());

                    _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                        new PartyMemberWarpGatePacket(party[client.TamerId]).Serialize());
                }
            }

            //client.Send(new SendHandler(client.Tamer.GeneralHandler));

        }
    }
}