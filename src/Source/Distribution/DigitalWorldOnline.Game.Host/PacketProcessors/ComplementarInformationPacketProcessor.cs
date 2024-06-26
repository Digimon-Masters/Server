using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Events;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Models.Mechanics;
using DigitalWorldOnline.Commons.Models.Servers;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;
using System.IO;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ComplementarInformationPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.ComplementarInformation;

        private readonly PartyManager _partyManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public ComplementarInformationPacketProcessor(
            PartyManager partyManager,
            MapServer mapServer,
            DungeonsServer dungeonsServer,
            AssetsLoader assets,
            ILogger logger,
            ISender sender,
            IMapper mapper)
        {
            _partyManager = partyManager;
            _mapServer = mapServer;
            _dungeonServer = dungeonsServer;
            _assets = assets;
            _logger = logger;
            _sender = sender;
            _mapper = mapper;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            _logger.Debug($"Sending seal info packet for character {client.TamerId}...");
            client.Send(new SealsPacket(client.Tamer.SealList));

            if (client.Tamer.TamerShop?.Count > 0)
            {
                _logger.Debug($"Recovering tamer shop items for character {client.TamerId}...");
                client.Tamer.Inventory.AddItems(client.Tamer.TamerShop.Items);
                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

                client.Tamer.TamerShop.Clear();
                await _sender.Send(new UpdateItemsCommand(client.Tamer.TamerShop));
            }

            UpdateSkillCooldown(client);

            _logger.Debug($"Sending inventory packet for character {client.TamerId}...");
            client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

            _logger.Debug($"Sending warehouse packet for character {client.TamerId}...");
            client.Send(new LoadInventoryPacket(client.Tamer.Warehouse, InventoryTypeEnum.Warehouse));

            _logger.Debug($"Sending account warehouse packet for character {client.TamerId}...");
            client.Send(new LoadInventoryPacket(client.Tamer.AccountWarehouse, InventoryTypeEnum.AccountWarehouse));

            _logger.Debug($"Getting server exp information for character {client.TamerId}...");
            var serverInfo = _mapper.Map<ServerObject>(await _sender.Send(new ServerByIdQuery(client.ServerId)));

            client.SetServerExperience(serverInfo.Experience);

            if (!client.DungeonMap)
            {
                _logger.Debug($"Sending server experience packet for character {client.TamerId}...");
                client.Send(new ServerExperiencePacket(serverInfo));
            }

            if (client.MembershipExpirationDate != null)
            {
                _logger.Debug($"Sending account membership duration packet for character {client.TamerId}...");
                client.Send(new MembershipPacket(client.MembershipExpirationDate.Value, client.MembershipUtcSeconds));
            }

            //TODO: verificar se a visualização do client não esta duplicando
            _logger.Debug($"Sending account cash coins packet for character {client.TamerId}...");
            client.Send(new CashShopCoinsPacket(client.Premium, client.Silk));

            //TODO: DebugLog($"Sending time reward packet for character {client.TamerId}...");
            //client.Send(new TimeRewardPacket(client.Tamer.TimeReward));

            if (client.ReceiveWelcome)
            {
                var welcomeMessages = await _sender.Send(new ActiveWelcomeMessagesAssetsQuery());

                _logger.Debug($"Sending welcome message packet for account {client.AccountId}...");
                client.Send(new WelcomeMessagePacket(welcomeMessages.PickRandom().Message));
            }

            if (client.Tamer.HasXai)
            {                        
                _logger.Debug($"Sending XAI info packet for character {client.TamerId}...");
                client.Send(new XaiInfoPacket(client.Tamer.Xai));

                _logger.Debug($"Sending tamer XAI resources packet for character {client.TamerId}...");
                client.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge, client.Tamer.XCrystals));
            }

            //TODO: verificar se a visualização do client não esta duplicando
            _logger.Debug($"Sending tamer relations packet for character {client.TamerId}...");
            client.Send(new TamerRelationsPacket(client.Tamer.Friends, client.Tamer.Foes));
        
            _logger.Debug($"Sending attendance event packet for character {client.TamerId}...");
            client.Send(new TamerAttendancePacket(client.Tamer.AttendanceReward));

            _logger.Debug($"Sending update status packet for character {client.TamerId}...");
            client.Send(new UpdateStatusPacket(client.Tamer));

            _logger.Debug($"Sending update movement speed packet for character {client.TamerId}...");
            client.Send(new UpdateMovementSpeedPacket(client.Tamer));

            //if (client.Tamer.EventState != CharacterEventStateEnum.None)
            //{
            //    var packet = new PacketWriter();
            //    packet.Type(4126);
            //    packet.WriteInt(0);
            //    packet.WriteInt(92137);
            //    packet.WriteInt(0);
            //    client.Send(packet);
            //
            //    client.Send(new ChatMessagePacket("You have 2 minutes to get stronger. Run!", ChatTypeEnum.Area));
            //}

            _logger.Debug($"Searching guild information for character {client.TamerId}...");
            client.Tamer.SetGuild(_mapper.Map<GuildModel>(await _sender.Send(new GuildByCharacterIdQuery(client.TamerId))));

            if (client.Tamer.Guild != null)
            {
                foreach (var guildMember in client.Tamer.Guild.Members)
                {
                    if (guildMember.CharacterInfo == null)
                    {
                        var guildMemberClient = _mapServer.FindClientByTamerId(guildMember.CharacterId);
                        if (guildMemberClient != null)
                        {
                            guildMember.SetCharacterInfo(guildMemberClient.Tamer);
                        }
                        else
                        {
                            guildMember.SetCharacterInfo(_mapper.Map<CharacterModel>(await _sender.Send(new CharacterByIdQuery(guildMember.CharacterId))));
                        }
                    }
                }

                foreach (var guildMember in client.Tamer.Guild.Members)
                {
                    if (client.ReceiveWelcome)
                    {
                        _logger.Debug($"Sending guild information packet for character {client.TamerId}...");
                        _mapServer.BroadcastForUniqueTamer(guildMember.CharacterId, new GuildInformationPacket(client.Tamer.Guild).Serialize());
                        _dungeonServer.BroadcastForUniqueTamer(guildMember.CharacterId, new GuildInformationPacket(client.Tamer.Guild).Serialize());
                    }
                }

                _logger.Debug($"Sending guild historic packet for character {client.TamerId}...");
                client.Send(new GuildInformationPacket(client.Tamer.Guild));

                _logger.Debug($"Sending guild historic packet for character {client.TamerId}...");
                client.Send(new GuildHistoricPacket(client.Tamer.Guild.Historic));
            }

            if (client.ReceiveWelcome)
            {
                client.Tamer.Friends
                .ToList()
                .ForEach(friend =>
                {
                    _logger.Debug($"Sending friend connection packet for character {friend.FriendId}...");
                    _mapServer.BroadcastForUniqueTamer(friend.FriendId, new FriendConnectPacket(client.Tamer.Name).Serialize());
                    _dungeonServer.BroadcastForUniqueTamer(friend.FriendId, new FriendConnectPacket(client.Tamer.Name).Serialize());
                });

                if (client.Tamer.Guild != null)
                {
                    _logger.Debug($"Getting guild rank position for guild {client.Tamer.Guild.Id}...");
                    var guildRank = await _sender.Send(new GuildCurrentRankByGuildIdQuery(client.Tamer.Guild.Id));

                    if (guildRank > 0 && guildRank <= 100)
                    {
                        _logger.Debug($"Sending guild rank packet for character {client.TamerId}...");
                        client.Send(new GuildRankPacket(guildRank));
                    }
                }
            }

            //TODO: ItemExpired(Tamer, client);

            _logger.Information($"Complementar information packets sent for character {client.TamerId}.");
            _logger.Debug($"Updating tamer state for character {client.TamerId}...");
            client.Tamer.UpdateState(CharacterStateEnum.Ready);
            await _sender.Send(new UpdateCharacterStateCommand(client.TamerId, CharacterStateEnum.Ready));

            _logger.Debug($"Updating account welcome flag for account {client.AccountId}...");
            await _sender.Send(new UpdateAccountWelcomeFlagCommand(client.AccountId, false));

            var channels = new Dictionary<byte, byte>
            {
                { 0, 30 }
            };

            if (!client.DungeonMap)
            {
                _logger.Debug($"Sending available channels packet...");
                client.Send(new AvailableChannelsPacket(channels));
            }


            if (!client.DungeonMap)
            {
                var map = _mapServer.Maps.FirstOrDefault(x => x.MapId == client.Tamer.Location.MapId);

                if (map != null)
                {
                    NotifyTamerKillSpawnEnteringMap(client, map);
                }
            }

            var currentMap = _assets.Maps.FirstOrDefault(x => x.MapId == client.Tamer.Location.MapId);
            if (currentMap != null)
            {
                var characterRegion = client.Tamer.MapRegions[currentMap.RegionIndex];
                if (characterRegion != null)
                {
                    if (characterRegion.Unlocked == 0)
                    {
                        characterRegion.Unlock();
                        await _sender.Send(new UpdateCharacterMapRegionCommand(characterRegion));
                        _logger.Verbose($"Character {client.TamerId} unlocked region {currentMap.RegionIndex} at {client.TamerLocation}.");
                    }
                }
                else
                {
                    client.Send(new SystemMessagePacket($"Unknown region index {currentMap.RegionIndex}."));
                    _logger.Warning($"Unknown region index {currentMap.RegionIndex} for character {client.TamerId} at {client.TamerLocation}.");
                }
            }
            else
            {
                client.Send(new SystemMessagePacket($"Unknown map info for map id {client.Tamer.Location.MapId}."));
                _logger.Warning($"Unknown map info for map id {client.Tamer.Location.MapId}.");
            }
        }

        private void UpdateSkillCooldown(GameClient client)
        {

            if (client.Tamer.Partner.HasActiveSkills())
            {

                foreach (var evolution in client.Tamer.Partner.Evolutions)
                {
                    foreach (var skill in evolution.Skills)
                    {
                        if (skill.Duration > 0 && skill.Expired)
                        {
                            skill.ResetCooldown();
                        }
                    }

                    _sender.Send(new UpdateEvolutionCommand(evolution));
                }

                List<int> SkillIds = new List<int>(5);
                var packetEvolution = client.Tamer.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Tamer.Partner.CurrentType);

                if (packetEvolution != null)
                {

                    var slot = -1;

                    foreach (var item in packetEvolution.Skills)
                    {
                        slot++;

                        var skillInfo = _assets.DigimonSkillInfo.FirstOrDefault(x => x.Type == client.Partner.CurrentType && x.Slot == slot);
                        if (skillInfo != null)
                        {
                            SkillIds.Add(skillInfo.SkillId);
                        }
                    }

                    client?.Send(new SkillUpdateCooldownPacket(client.Tamer.Partner.GeneralHandler, client.Tamer.Partner.CurrentType, packetEvolution, SkillIds));

                }
            }
        }

        public void NotifyTamerKillSpawnEnteringMap(GameClient client, GameMap map)
        {
            foreach (var sourceKillSpawn in map.KillSpawns)
            {
                foreach (var mob in sourceKillSpawn.SourceMobs.Where(x => x.CurrentSourceMobRequiredAmount <= 10))
                {
                    NotifyMinimap(client, mob);
                }

                if (sourceKillSpawn.Spawn())
                {
                    NotifyMapChat(client, map, sourceKillSpawn);
                }
            }
        }

        private void NotifyMinimap(GameClient client, KillSpawnSourceMobConfigModel mob)
        {
            client.Send(new KillSpawnMinimapNotifyPacket(mob.SourceMobType, mob.CurrentSourceMobRequiredAmount).Serialize());

        }

        private void NotifyMapChat(GameClient client, GameMap map, KillSpawnConfigModel sourceKillSpawn)
        {
            foreach (var targetMob in sourceKillSpawn.TargetMobs)
            {
                client.Send(new KillSpawnChatNotifyPacket(map.MapId, map.Channel, targetMob.TargetMobType).Serialize());

            }
        }
    }
}
