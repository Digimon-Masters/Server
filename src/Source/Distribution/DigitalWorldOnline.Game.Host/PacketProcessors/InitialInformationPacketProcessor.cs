using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Account;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class InitialInformationPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.InitialInformation;

        private readonly PartyManager _partyManager;
        private readonly StatusManager _statusManager;
        private readonly MapServer _mapServer;
        private readonly PvpServer _pvpServer;
        private readonly DungeonsServer _dungeonsServer;

        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public InitialInformationPacketProcessor(
            PartyManager partyManager,
            StatusManager statusManager,
            MapServer mapServer,
            PvpServer pvpServer,
            DungeonsServer dungeonsServer,
            AssetsLoader assets,
            ILogger logger,
            ISender sender,
            IMapper mapper)
        {
            _partyManager = partyManager;
            _statusManager = statusManager;
            _mapServer = mapServer;
            _pvpServer = pvpServer;
            _dungeonsServer = dungeonsServer;
            _assets = assets;
            _logger = logger;
            _sender = sender;
            _mapper = mapper;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            _logger.Debug("Getting packet parameters...");
            packet.Skip(4);
            var accountId = packet.ReadUInt();

            _logger.Debug($"{accountId}");

            _logger.Debug($"Getting account information...");
            _logger.Information($"Searching account with id {accountId}...");
            var account = _mapper.Map<AccountModel>(await _sender.Send(new AccountByIdQuery(accountId)));
            client.SetAccountInfo(account);

            _logger.Debug($"Getting character base information...");
            _logger.Information($"Searching character with id {account.LastPlayedCharacter} for account {account.Id}...");
            var character = _mapper.Map<CharacterModel>(await _sender.Send(new CharacterByIdQuery(account.LastPlayedCharacter)));
            if (character.Partner == null)
            {
                _logger.Error($"Invalid character information for tamer id {account.LastPlayedCharacter}.");
                return;
            }

            account.ItemList.ForEach(character.AddItemList);

            foreach (var digimon in character.Digimons)
            {
                digimon.SetTamer(character);

                digimon.SetBaseInfo(
                    _statusManager.GetDigimonBaseInfo(
                        digimon.CurrentType
                    )
                );

                digimon.SetBaseStatus(
                    _statusManager.GetDigimonBaseStatus(
                        digimon.CurrentType,
                        digimon.Level,
                        digimon.Size
                    )
                );

                digimon.SetTitleStatus(
                    _statusManager.GetTitleStatus(
                        character.CurrentTitle
                    )
                );

                digimon.SetSealStatus(_assets.SealInfo);
            }

            _logger.Debug($"Getting character status information...");
            character.SetBaseStatus(
                _statusManager.GetTamerBaseStatus(
                    character.Model
                )
            );

            character.SetLevelStatus(
                _statusManager.GetTamerLevelStatus(
                    character.Model,
                    character.Level
                )
            );

            character.NewViewLocation(character.Location.X, character.Location.Y);
            character.RemovePartnerPassiveBuff();
            character.SetPartnerPassiveBuff();

            await _sender.Send(new UpdateDigimonBuffListCommand(character.Partner.BuffList));
            
            _logger.Information($"Updating itens and buff status for character {character.Id}...");
            _logger.Debug($"Concatting character items information...");
            foreach (var item in character.ItemList.SelectMany(x => x.Items).Where(x => x.ItemId > 0))
                item.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == item?.ItemId));

            _logger.Debug($"Concatting buffs information...");
            foreach (var buff in character.BuffList.ActiveBuffs)
                buff.SetBuffInfo(_assets.BuffInfo.FirstOrDefault(x => x.SkillCode == buff.SkillId || x.DigimonSkillCode == buff.SkillId));

            foreach (var buff in character.Partner.BuffList.ActiveBuffs)
                buff.SetBuffInfo(_assets.BuffInfo.FirstOrDefault(x => x.SkillCode == buff.SkillId || x.DigimonSkillCode == buff.SkillId));

            _logger.Information($"Waiting an available channel for character {character.Id}...");
            _logger.Debug($"Getting available channels...");

            //var channels = await _sender.Send(new ChannelsByMapIdQuery(character.Location.MapId));
            //
            //byte? channel = character.Channel == byte.MaxValue ?
            //    channels?
            //    .OrderByDescending(x => x.Value)
            //    .FirstOrDefault(x => x.Value < byte.MaxValue)
            //    .Key : character.Channel;
            //
            //while (channel == null)
            //{
            //    if (channels?.Count > 15)
            //        break;
            //
            //    _logger.Debug($"Creating new channel for map {character.Location.MapId}...");
            //    channels?.Add(channels.Keys.GetNewChannel(), 0);
            //
            //    channel = character.Channel == byte.MaxValue ?
            //    channels?
            //    .OrderByDescending(x => x.Value)
            //    .FirstOrDefault(x => x.Value < byte.MaxValue)
            //    .Key : character.Channel;
            //}

            character.SetCurrentChannel(0);

            character.UpdateState(CharacterStateEnum.Loading);

            client.SetCharacter(character);

            _logger.Debug($"Updating character state...");
            await _sender.Send(new UpdateCharacterStateCommand(character.Id, CharacterStateEnum.Loading));

            if (character.Location.MapId == 9101)
            {
                _logger.Information($"Adding character {character.Id} to PvP map {character.Location.MapId}...");
                _pvpServer.AddClient(client);
            }
            else if (client.DungeonMap)
            {
                _logger.Information($"Adding character {character.Id} to map {character.Location.MapId}...");
                _dungeonsServer.AddClient(client);
            }
            else
            {
                _logger.Information($"Adding character {character.Id} to map {character.Location.MapId}...");
                _mapServer.AddClient(client);
            }

            while (client.Loading) await Task.Delay(1000);

            character.SetGenericHandler(character.Partner.GeneralHandler);

            var party = _partyManager.FindParty(client.TamerId);
            if (party != null)
            {
                party.UpdateMember(party[client.TamerId]);
            }

            if (!client.DungeonMap)
            {
                var region = _assets.Maps.FirstOrDefault(x => x.MapId == character.Location.MapId);

                if (region != null)
                {
                    if (character.MapRegions[region.RegionIndex].Unlocked != 0x80)
                    {
                        var characterRegion = character.MapRegions[region.RegionIndex];
                        characterRegion.Unlock();

                        await _sender.Send(new UpdateCharacterMapRegionCommand(characterRegion));
                    }
                }
            }

            await ReceiveArenaPoints(client);

            client.Send(new InitialInfoPacket(character, party));



            _logger.Debug($"Updating character channel...");
            await _sender.Send(new UpdateCharacterChannelCommand(character.Id, character.Channel));
        }

        private async Task ReceiveArenaPoints(GameClient client)
        {
            if (client.Tamer.Points.Amount > 0)
            {
                var newItem = new ItemModel();
                newItem.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == client.Tamer.Points.ItemId));



                newItem.ItemId = client.Tamer.Points.ItemId;
                newItem.Amount = client.Tamer.Points.Amount;

                if (newItem.IsTemporary)
                    newItem.SetRemainingTime((uint)newItem.ItemInfo.UsageTimeMinutes);

                var itemClone = (ItemModel)newItem.Clone();

                if (client.Tamer.Inventory.AddItem(newItem))
                {
                    await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                }
                else
                {
                    client.Tamer.GiftWarehouse.AddItem(newItem);
                    await _sender.Send(new UpdateItemsCommand(client.Tamer.GiftWarehouse));
                }

                client.Tamer.Points.SetAmount(0);
                client.Tamer.Points.SetCurrentStage(0);

                await _sender.Send(new UpdateCharacterArenaPointsCommand(client.Tamer.Points));
            }
            else if (client.Tamer.Points.CurrentStage > 0)
            {
                client.Tamer.Points.SetCurrentStage(0);
                await _sender.Send(new UpdateCharacterArenaPointsCommand(client.Tamer.Points));
            }
        }
    }
}
