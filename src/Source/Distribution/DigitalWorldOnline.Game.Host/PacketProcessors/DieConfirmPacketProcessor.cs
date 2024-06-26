﻿using AutoMapper;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Asset;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.GameHost;


using MediatR;
using Microsoft.Extensions.Configuration;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class DieConfirmPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.DieConfirm;

        private const string GameServerAddress = "GameServer:Address";
        private const string GamerServerPublic = "GameServer:PublicAddress";
        private const string GameServerPort = "GameServer:Port";

        private readonly MapServer _mapServer;
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly DungeonsServer _dungeonServer;
        public DieConfirmPacketProcessor(
            MapServer mapServer,
            ISender sender,
            IMapper mapper,
            IConfiguration configuration,
            DungeonsServer dungeonsServer)
        {
            _mapServer = mapServer;
            _sender = sender;
            _mapper = mapper;
            _configuration = configuration;
            _dungeonServer = dungeonsServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            if (client.DungeonMap)
            {
                var map = UtilitiesFunctions.MapGroup(client.Tamer.Location.MapId);

                var mapConfig = await _sender.Send(new GameMapConfigByMapIdQuery(map));
                var waypoints = await _sender.Send(new MapRegionListAssetsByMapIdQuery(map));

                if (mapConfig == null || waypoints == null || !waypoints.Regions.Any())
                {
                    client.Send(new SystemMessagePacket($"Map information not found for map Id {map}."));
                    return;
                }

                var destination = waypoints.Regions.First();

                client.Tamer.Die();

                await _sender.Send(new UpdateCharacterBasicInfoCommand(client.Tamer));
                await _sender.Send(new UpdateCharacterActiveEvolutionCommand(client.Tamer.ActiveEvolution));


                client.Tamer.NewLocation(map, destination.X, destination.Y);
                await _sender.Send(new UpdateCharacterLocationCommand(client.Tamer.Location));

                client.Tamer.Partner.NewLocation(map, destination.X, destination.Y);
                await _sender.Send(new UpdateDigimonLocationCommand(client.Tamer.Partner.Location));

                client.Tamer.UpdateState(CharacterStateEnum.Loading);
                await _sender.Send(new UpdateCharacterStateCommand(client.TamerId, CharacterStateEnum.Loading));

                _dungeonServer.RemoveClient(client);

                client.Send(new MapSwapPacket(
                    _configuration[GamerServerPublic],
                    _configuration[GameServerPort],
                    client.Tamer.Location.MapId,
                    client.Tamer.Location.X,
                    client.Tamer.Location.Y)
                    .Serialize());
            }
            else
            {
                var destiny = _mapper.Map<MapRegionListAssetModel>(await _sender.Send(new MapRegionListAssetsByMapIdQuery(client.Tamer.Location.MapId)));
                var region = destiny?.Regions.FirstOrDefault();

                if (region != null)
                {
                    client.Tamer.NewLocation(region.X, region.Y);
                    await _sender.Send(new UpdateCharacterLocationCommand(client.Tamer.Location));

                    client.Tamer.Partner.NewLocation(region.X, region.Y);
                    await _sender.Send(new UpdateDigimonLocationCommand(client.Tamer.Partner.Location));
                }

                client.Tamer.UpdateState(CharacterStateEnum.Loading);
                await _sender.Send(new UpdateCharacterStateCommand(client.TamerId, CharacterStateEnum.Loading));

                _mapServer.RemoveClient(client);

                client.Send(new MapSwapPacket(
                    _configuration[GamerServerPublic],
                    _configuration[GameServerPort],
                    client.Tamer.Location.MapId,
                    client.Tamer.Location.X,
                    client.Tamer.Location.Y)
                    .Serialize());
            }
        }
    }
}