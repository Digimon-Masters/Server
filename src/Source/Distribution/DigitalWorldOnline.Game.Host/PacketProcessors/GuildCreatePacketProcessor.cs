﻿using AutoMapper;
using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Mechanics;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class GuildCreatePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.CreateGuild;

        private readonly MapServer _mapServer;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GuildCreatePacketProcessor(
            MapServer mapServer,
            ISender sender,
            ILogger logger,
            IMapper mapper)
        {
            _mapServer = mapServer;
            _sender = sender;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var guildName = packet.ReadString();
            packet.Skip(1);
            var itemSlot = packet.ReadByte();

            var nameTaken = await _sender.Send(new GuildByGuildNameQuery(guildName)) != null;
            var guildPermit = client.Tamer.Inventory.FindItemBySlot(itemSlot);
            if (guildPermit == null || guildPermit.Amount <= 0 || nameTaken)
            {
                _logger.Debug($"Sending guild create fail packet for character {client.TamerId}...");
                client.Send(new GuildCreateFailPacket(client.Tamer.Name, guildName));
                return;
            }

            var guild = GuildModel.Create(guildName);
            guild.AddMember(client.Tamer, GuildAuthorityTypeEnum.Master);
            guild.AddHistoricEntry(GuildHistoricTypeEnum.GuildCreate, guild.Master, guild.Master);

            client.Tamer.SetGuild(_mapper.Map<GuildModel>(await _sender.Send(new CreateGuildCommand(guild))));

            _logger.Debug($"Sending guild create success packet for character {client.TamerId}...");
            client.Send(new GuildCreateSuccessPacket(client.Tamer.Name, itemSlot, guildName));

            _logger.Debug($"Sending guild information packet for character {client.TamerId}...");
            client.Send(new GuildInformationPacket(guild));

            _logger.Debug($"Sending guild historic packet for character {client.TamerId}...");
            client.Send(new GuildHistoricPacket(client.Tamer.Guild!.Historic));

            _logger.Debug($"Getting guild rank position for guild {client.Tamer.Guild.Id}...");
            var guildRank = await _sender.Send(new GuildCurrentRankByGuildIdQuery(client.Tamer.Guild.Id));

            if (guildRank > 0 && guildRank <= 100)
            {
                _logger.Debug($"Sending guild rank packet for character {client.TamerId}...");
                client.Send(new GuildRankPacket(guildRank));
            }

            _logger.Verbose($"Character {client.TamerId} created guild {client.Tamer.Guild.Id} {client.Tamer.Guild.Name}.");

            _mapServer.BroadcastForTargetTamers(client.TamerId, new UnloadTamerPacket(client.Tamer).Serialize());
            _mapServer.BroadcastForTargetTamers(client.TamerId, new LoadTamerPacket(client.Tamer).Serialize());
            _mapServer.BroadcastForTargetTamers(client.TamerId, new LoadBuffsPacket(client.Tamer).Serialize());

            client.Tamer.Inventory.RemoveOrReduceItem(guildPermit, 1);
            await _sender.Send(new UpdateItemCommand(guildPermit));
        }
    }
}