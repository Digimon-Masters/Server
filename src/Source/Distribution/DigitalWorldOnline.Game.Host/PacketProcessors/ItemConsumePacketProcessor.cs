using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Models.Summon;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Diagnostics.Eventing.Reader;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ItemConsumePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.ConsumeItem;

        private const string GamerServerPublic = "GameServer:PublicAddress";
        private const string GameServerPort = "GameServer:Port";
        private readonly StatusManager _statusManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly AssetsLoader _assets;
        private readonly ConfigsLoader _configs;
        private readonly ExpManager _expManager;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ItemConsumePacketProcessor(
            StatusManager statusManager,
            MapServer mapServer,
            DungeonsServer dungeonsServer,
            AssetsLoader assets,
            ExpManager expManager,
            ConfigsLoader configs,
            ISender sender,
            ILogger logger,
            IConfiguration configuration)
        {
            _statusManager = statusManager;
            _mapServer = mapServer;
            _dungeonServer = dungeonsServer;
            _expManager = expManager;
            _assets = assets;
            _configs = configs;
            _sender = sender;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            packet.Skip(4);
            var itemSlot = packet.ReadShort();

            if (client.Partner == null)
            {
                _logger.Warning($"Invalid partner for tamer id {client.TamerId}.");
                client.Send(new SystemMessagePacket($"Invalid partner."));
                return;
            }

            var targetItem = client.Tamer.Inventory.FindItemBySlot(itemSlot);
            if (targetItem == null)
            {
                _logger.Warning($"Invalid item at slot {itemSlot} for tamer id {client.TamerId}.");
                client.Send(new SystemMessagePacket($"Invalid item at slot {itemSlot}."));
                return;
            }

          
            if (targetItem.ItemInfo.Type == 60)
            {

                if (targetItem.ItemInfo?.SkillInfo == null)
                {
                    client.Send(
                        UtilitiesFunctions.GroupPackets(
                            new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                            new SystemMessagePacket($"Invalid skill info for item id {targetItem.ItemId}.").Serialize(),
                            new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                        )
                    );

                    _logger.Warning($"Invalid skill info for item id {targetItem.ItemId} and tamer id {client.TamerId}.");
                    return;
                }

                foreach (var apply in targetItem.ItemInfo?.SkillInfo.Apply)
                {
                    switch (apply.Type)
                    {

                        case SkillCodeApplyTypeEnum.Default:
                            {
                                switch (apply.Attribute)
                                {
                                    case SkillCodeApplyAttributeEnum.EXP:
                                        {
                                            switch (targetItem.ItemInfo.Target)
                                            {
                                                case ItemConsumeTargetEnum.Both:
                                                    {
                                                        var value = Convert.ToInt64(apply.Value);

                                                        var result = _expManager.ReceiveTamerExperience(value, client.Tamer);
                                                        var result2 = _expManager.ReceiveDigimonExperience(value, client.Tamer.Partner);

                                                        if (result.Success)
                                                        {
                                                            client.Send(
                                                                new ReceiveExpPacket(
                                                                    value,
                                                                    0,
                                                                    client.Tamer.CurrentExperience,
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    0,
                                                                    0,
                                                                    client.Tamer.Partner.CurrentExperience,
                                                                    0
                                                                )
                                                            );
                                                        }
                                                        else
                                                        {
                                                            client.Send(new SystemMessagePacket($"No proper configuration for tamer {client.Tamer.Model} leveling."));
                                                            return;
                                                        }

                                                        if (result.LevelGain > 0)
                                                        {
                                                            client.Tamer.SetLevelStatus(
                                                                _statusManager.GetTamerLevelStatus(
                                                                    client.Tamer.Model,
                                                                    client.Tamer.Level
                                                                )
                                                            );

                                                            if (client.DungeonMap)
                                                            {
                                                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                                                client.TamerId,
                                                                new LevelUpPacket(
                                                                    client.Tamer.GeneralHandler,
                                                                    client.Tamer.Level).Serialize());
                                                            }
                                                            else
                                                            {
                                                                _mapServer.BroadcastForTamerViewsAndSelf(
                                                                client.TamerId,
                                                                new LevelUpPacket(
                                                                    client.Tamer.GeneralHandler,
                                                                    client.Tamer.Level).Serialize());

                                                            }
                                                            client.Tamer.FullHeal();

                                                            client.Send(new UpdateStatusPacket(client.Tamer));
                                                        }

                                                        if (result.Success)
                                                            await _sender.Send(new UpdateCharacterExperienceCommand(client.TamerId, client.Tamer.CurrentExperience, client.Tamer.Level));

                                                        if (result2.Success)
                                                        {
                                                            client.Send(
                                                                new ReceiveExpPacket(
                                                                    0,
                                                                    0,
                                                                    client.Tamer.CurrentExperience,
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    value,
                                                                    0,
                                                                    client.Tamer.Partner.CurrentExperience,
                                                                    0
                                                                )
                                                            );
                                                        }

                                                        if (result2.LevelGain > 0)
                                                        {
                                                            client.Partner.SetBaseStatus(
                                                                _statusManager.GetDigimonBaseStatus(
                                                                    client.Partner.CurrentType,
                                                                    client.Partner.Level,
                                                                    client.Partner.Size
                                                                )
                                                            );

                                                            if (client.DungeonMap)
                                                            {
                                                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                                                client.TamerId,
                                                                new LevelUpPacket(
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    client.Tamer.Partner.Level
                                                                ).Serialize()
                                                            );

                                                            }
                                                            else
                                                            {
                                                                _mapServer.BroadcastForTamerViewsAndSelf(
                                                                client.TamerId,
                                                                new LevelUpPacket(
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    client.Tamer.Partner.Level
                                                                ).Serialize()
                                                            );


                                                            }

                                                            client.Partner.FullHeal();

                                                            client.Send(new UpdateStatusPacket(client.Tamer));
                                                        }

                                                        if (result2.Success)
                                                            await _sender.Send(new UpdateDigimonExperienceCommand(client.Partner));
                                                    }

                                                    break;

                                                case ItemConsumeTargetEnum.Digimon:
                                                    {
                                                        var digimonResult = _expManager.ReceiveDigimonExperience(apply.Value, client.Tamer.Partner);
                                                        var value = Convert.ToInt64(apply.Value);

                                                        if (digimonResult.Success)
                                                        {
                                                            client.Send(
                                                                new ReceiveExpPacket(
                                                                    0,
                                                                    0,
                                                                    client.Tamer.CurrentExperience,
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    value,
                                                                    0,
                                                                    client.Tamer.Partner.CurrentExperience,
                                                                    0
                                                                )
                                                            );
                                                        }

                                                        if (digimonResult.LevelGain > 0)
                                                        {
                                                            client.Partner.SetBaseStatus(
                                                                _statusManager.GetDigimonBaseStatus(
                                                                    client.Partner.CurrentType,
                                                                    client.Partner.Level,
                                                                    client.Partner.Size
                                                                )
                                                            );

                                                            if (client.DungeonMap)
                                                            {
                                                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                                                    client.TamerId,
                                                                    new LevelUpPacket(
                                                                        client.Tamer.Partner.GeneralHandler,
                                                                        client.Tamer.Partner.Level
                                                                    ).Serialize()
                                                                );
                                                            }
                                                            else
                                                            {
                                                                _mapServer.BroadcastForTamerViewsAndSelf(
                                                                    client.TamerId,
                                                                    new LevelUpPacket(
                                                                        client.Tamer.Partner.GeneralHandler,
                                                                        client.Tamer.Partner.Level
                                                                    ).Serialize()
                                                                );
                                                            }

                                                            client.Partner.FullHeal();

                                                            client.Send(new UpdateStatusPacket(client.Tamer));
                                                        }

                                                        if (digimonResult.Success)
                                                            await _sender.Send(new UpdateDigimonExperienceCommand(client.Partner));
                                                        break;
                                                    }

                                                case ItemConsumeTargetEnum.Tamer:
                                                    {
                                                        var value = Convert.ToInt64(apply.Value);

                                                        var result = _expManager.ReceiveTamerExperience(value, client.Tamer);
                                                        if (result.Success)
                                                        {
                                                            client.Send(
                                                                new ReceiveExpPacket(
                                                                    value,
                                                                    0,
                                                                    client.Tamer.CurrentExperience,
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    0,
                                                                    0,
                                                                    client.Tamer.Partner.CurrentExperience,
                                                                    0
                                                                )
                                                            );
                                                        }
                                                        else
                                                        {
                                                            client.Send(new SystemMessagePacket($"No proper configuration for tamer {client.Tamer.Model} leveling."));
                                                            return;
                                                        }

                                                        if (result.LevelGain > 0)
                                                        {
                                                            client.Tamer.SetLevelStatus(
                                                                _statusManager.GetTamerLevelStatus(
                                                                    client.Tamer.Model,
                                                                    client.Tamer.Level
                                                                )
                                                            );

                                                            _mapServer.BroadcastForTamerViewsAndSelf(
                                                            client.TamerId,
                                                            new LevelUpPacket(
                                                                client.Tamer.GeneralHandler,
                                                                client.Tamer.Level).Serialize());

                                                            client.Tamer.FullHeal();

                                                            client.Send(new UpdateStatusPacket(client.Tamer));
                                                        }

                                                        if (result.Success)
                                                            await _sender.Send(new UpdateCharacterExperienceCommand(client.TamerId, client.Tamer.CurrentExperience, client.Tamer.Level));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;


                                }
                            }
                            break;
                    }
                }

                if (client.DungeonMap)
                {
                    _dungeonServer.BroadcastForTargetTamers(client.TamerId,
                        UtilitiesFunctions.GroupPackets(
                            new UpdateCurrentHPRatePacket(
                                client.Tamer.GeneralHandler,
                                client.Tamer.HpRate).Serialize(),
                            new UpdateCurrentHPRatePacket(
                                client.Tamer.Partner.GeneralHandler,
                                client.Tamer.Partner.HpRate).Serialize()
                        )
                    );
                }
                else
                {
                    _mapServer.BroadcastForTargetTamers(client.TamerId,
                        UtilitiesFunctions.GroupPackets(
                            new UpdateCurrentHPRatePacket(
                                client.Tamer.GeneralHandler,
                                client.Tamer.HpRate).Serialize(),
                            new UpdateCurrentHPRatePacket(
                                client.Tamer.Partner.GeneralHandler,
                                client.Tamer.Partner.HpRate).Serialize()
                        )
                    );
                }

                _logger.Verbose($"Character {client.TamerId} consumed {targetItem.ItemId}.");

                client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1, itemSlot);

                await _sender.Send(new UpdateItemCommand(targetItem));

                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );

                return;
            }

            if (targetItem.ItemInfo.Type == 61)
            {
                await ConsumeFoodItem(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 62)
            {
                var SummonInfo = _assets.SummonInfo.FirstOrDefault(x => x.ItemId == targetItem.ItemId);

                if (SummonInfo != null)
                {
                    await SummonMonster(client, itemSlot, targetItem, SummonInfo);
                }
                else
                {
                    await ConsumeAchievement(client, itemSlot, targetItem);
                }

            }
            else if (targetItem.ItemInfo.Type == 63)
            {
                await BuffItem(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 89)
            {
                await Fruits(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 155)
            {
                await IncreaseInventorySlots(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 156)
            {
                await IncreaseWarehouseSlots(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 159)
            {
                await IncreaseDigimonSlots(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 160)
            {
                await IncreaseArchiveSlots(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 170)
            {
                await ContainerItem(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 180)
            {
                await CashTamerSkills(client, itemSlot, targetItem);

            }
            else if (targetItem.ItemInfo.Type == 201)
            {
                await ConsumeFoodItem(client, itemSlot, targetItem);
            }
            else if (targetItem.ItemInfo.Type == 72)
            {
                await BombTeleport(client, itemSlot, targetItem);
            }
            else
                client.Send(new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type));
        }

        private async Task CashTamerSkills(GameClient client, short itemSlot, ItemModel targetItem)
        {
            if (client.Tamer.TamerSkill.EquippedItems.Count == 5)
            {
                client.Send(new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type));
                return;
            }
            else
            {
                var targetSkill = _assets.TamerSkills.FirstOrDefault(x => x.SkillId == targetItem.ItemInfo?.SkillCode);

                if (targetSkill != null)
                {
                    targetItem.ItemInfo?.SetSkillInfo(_assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == targetSkill.SkillCode));
                }

                if (targetItem.ItemInfo?.SkillInfo == null)
                {
                    client.Send(
                        UtilitiesFunctions.GroupPackets(
                            new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                            new SystemMessagePacket($"Invalid skill info for item id {targetItem.ItemId}.").Serialize(),
                            new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                        )
                    );

                    _logger.Warning($"Invalid skill info for item id {targetItem.ItemId} and tamer id {client.TamerId}.");
                    return;
                }

                var activeSkill = client.Tamer.ActiveSkill.FirstOrDefault(x => x.SkillId == 0 || x.SkillId == targetSkill?.SkillId);
                if(activeSkill != null)
                {
                    if(activeSkill.SkillId == targetSkill?.SkillId)
                    {
                        activeSkill.IncreaseEndDate(targetItem.ItemInfo.UsageTimeMinutes);
                    }
                    else
                    {
                        activeSkill.SetTamerSkill(targetSkill.SkillId, 0, TamerSkillTypeEnum.Cash, targetItem.ItemInfo.UsageTimeMinutes);
                    }
                }
               


                await _sender.Send(new UpdateTamerSkillCooldownByIdCommand(activeSkill));

                client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

                client.Send(
            UtilitiesFunctions.GroupPackets(
                new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize(),
                new ActiveTamerCashSkill(activeSkill.SkillId,UtilitiesFunctions.RemainingTimeMinutes(activeSkill.RemainingMinutes)).Serialize()
                     )

              );


                return;
            }
        }

        private async Task SummonMonster(GameClient client, short itemSlot, ItemModel targetItem, SummonModel? SummonInfo)
        {
            if (!SummonInfo.Maps.Contains(client.Tamer.Location.MapId))
            {
                client.Send(new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type, ItemConsumeFailEnum.InvalidArea));


            }
            else
            {
                var count = 0;

                foreach (var mobToAdd in SummonInfo.SummonedMobs)
                {
                    count++;

                    var mob = (SummonMobModel)mobToAdd.Clone();

                    if (mob?.Location?.X != 0 && mob?.Location?.Y != 0)
                    {
                        var diff = UtilitiesFunctions.CalculateDistance(mob.Location.X, client.Tamer.Location.X, mob.Location.Y, client.Tamer.Location.Y);

                        if (diff > 5000)
                        {
                            client.Send(new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type, ItemConsumeFailEnum.InvalidArea));
                            break;
                        }
                        else if (count == 1)
                        {
                            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

                            client.Send(
                          UtilitiesFunctions.GroupPackets(
                              new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                              new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                          )

                      );

                        }
                    }
                    else
                    {

                        client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);
                        await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

                        client.Send(
                      UtilitiesFunctions.GroupPackets(
                          new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                          new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                      )

                  );

                    }

                    int radius = 500; // Ajuste este valor para controlar a dispersão dos chefes
                    var random = new Random();

                    // Gerando valores aleatórios para deslocamento em X e Y
                    int xOffset = random.Next(-radius, radius + 1);
                    int yOffset = random.Next(-radius, radius + 1);

                    // Calculando as novas coordenadas do chefe de raid
                    int bossX = client.Tamer.Location.X + xOffset;
                    int bossY = client.Tamer.Location.Y + yOffset;

                    if (client.DungeonMap)
                    {
                        var map = _dungeonServer.Maps.FirstOrDefault(x => x.Clients.Exists(x => x.TamerId == client.TamerId));

                        var mobId = map.SummonMobs.Count + 1;

                        mob.SetId(mobId);

                        if (mob?.Location?.X != 0 && mob?.Location?.Y != 0)
                        {
                            bossX = mob.Location.X;
                            bossY = mob.Location.Y;

                            mob.SetLocation(client.Tamer.Location.MapId, bossX, bossY);
                        }
                        else
                        {
                            mob.SetLocation(client.Tamer.Location.MapId, bossX, bossY);

                        }

                        mob.SetDuration();
                        mob.SetTargetSummonHandle(client.Tamer.GeneralHandler);
                        _dungeonServer.AddSummonMobs(client.Tamer.Location.MapId, mob, client.TamerId);
                    }
                    else
                    {
                        var map = _mapServer.Maps.FirstOrDefault(x => x.MapId == client.Tamer.Location.MapId);
                        var mobId = map.SummonMobs.Count + 1;

                        mob.SetId(mobId);

                        if (mob?.Location?.X != 0 && mob?.Location?.Y != 0)
                        {
                            bossX = mob.Location.X;
                            bossY = mob.Location.Y;

                            mob.SetLocation(client.Tamer.Location.MapId, bossX, bossY);
                        }
                        else
                        {
                            mob.SetLocation(client.Tamer.Location.MapId, bossX, bossY);

                        }

                        mob.SetDuration();
                        mob.SetTargetSummonHandle(client.Tamer.GeneralHandler);
                        _mapServer.AddSummonMobs(client.Tamer.Location.MapId, mob);

                    }
                }
            }
        }

        private async Task ConsumeAchievement(GameClient client, short itemSlot, ItemModel targetItem)
        {
            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

            client.Send(
          UtilitiesFunctions.GroupPackets(
              new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
              new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
          )

      );
            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
        }

        private async Task BombTeleport(GameClient client, short itemSlot, ItemModel targetIte)
        {
            Dictionary<int, int> itemMapIdMapping = new Dictionary<int, int> {
                // Esse é um dicionário de ID de mapas e ID de itens
                {
                    25001, // ID do item
                    3 // ID do mapa
                },
                {
                    9025,
                    3
                },
                {
                    25003,
                    1100
                },
                {
                    9027,
                    1100
                },
                {
                    25006,
                    2100
                },
                {
                    25019,
                    2100
                },
                {
                    25004,
                    1103
                },
                {
                    9028,
                    1103
                }

            };
            // Verificar o ID do item e o ID do mapa que ele vai teleportar
            if (itemMapIdMapping.TryGetValue(targetIte.ItemId, out int mapId))
            {
                // if (client.Tamer.Location.MapId == mapId)
                // {
                //     client.Send(new SystemMessagePacket($"You are already in this map."));
                //     _logger.Error($"You are already in this map.");
                //     
                //     return;
                // }
                var waypoints = await _sender.Send(new MapRegionListAssetsByMapIdQuery(mapId));
                var destination = waypoints.Regions.First();

                // Ajusta os valores de X e Y com base no mapID
                switch (mapId)
                {
                    case 3:
                        destination.X = 19981;
                        destination.Y = 14501;
                        break;
                    case 1100:
                        destination.X = 21377;
                        destination.Y = 56675;
                        break;
                    case 2100:
                        destination.X = 9425;
                        destination.Y = 9680;
                        break;
                    case 1103:
                        destination.X = 4847;
                        destination.Y = 39008;
                        break;
                }

                client.Tamer.NewLocation(mapId, destination.X, destination.Y);
                await _sender.Send(new UpdateCharacterLocationCommand(client.Tamer.Location));
                client.Tamer.Partner.NewLocation(mapId, destination.X, destination.Y);
                await _sender.Send(new UpdateDigimonLocationCommand(client.Tamer.Partner.Location));
                client.Tamer.UpdateState(CharacterStateEnum.Loading);
                await _sender.Send(new UpdateCharacterStateCommand(client.TamerId, CharacterStateEnum.Loading));
                client.SetGameQuit(false);

                client.Send(new MapSwapPacket(
                        _configuration[GamerServerPublic],
                        _configuration[GameServerPort],
                        client.Tamer.Location.MapId,
                        client.Tamer.Location.X,
                        client.Tamer.Location.Y)
                    .Serialize());
                client.Tamer.Inventory.RemoveOrReduceItem(
                    targetIte,
                    1,
                    itemSlot
                );
                await _sender.Send(new UpdateItemCommand(targetIte));
            }
            else
            {
                Console.WriteLine($"ItemID {targetIte.ItemId} não encontrado no mapeamento.");
            }

        }
        private async Task Fruits(GameClient client, short itemSlot, ItemModel targetItem)
        {
            var fruitConfig = _configs.Fruits.FirstOrDefault(x => x.ItemId == targetItem.ItemId);
            if (fruitConfig == null)
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new SystemMessagePacket($"Invalid fruit config for item {targetItem.ItemId}.").Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
                _logger.Error($"Invalid fruit config for item {targetItem.ItemId}.");
                return;
            }

            var sizeList = fruitConfig.SizeList.Where(x => x.HatchGrade == client.Partner.HatchGrade && x.Size > 1);
            if (!sizeList.Any())
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new SystemMessagePacket($"Invalid size list for fruit {targetItem.ItemId} and {client.Partner.HatchGrade} grade.").Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
                _logger.Error($"Invalid size list for fruit {targetItem.ItemId} and {client.Partner.HatchGrade} grade.");
                return;
            }

            short newSize = 0;
            var changeSize = false;
            bool rare = false;
            while (!changeSize)
            {
                var availableSizes = sizeList.Randomize();
                foreach (var size in availableSizes)
                {
                    if (size.Chance >= UtilitiesFunctions.RandomDouble())
                    {
                        rare = size.Size == availableSizes.Max(x => x.Size);

                        newSize = (short)(size.Size * 100);
                        changeSize = true;
                        break;
                    }
                }
            }

            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

            _logger.Verbose($"Character {client.TamerId} used {targetItem.ItemId} to change partner {client.Partner.Id} size from {client.Partner.Size / 100}% to {newSize / 100}%.");

            client.Partner.SetSize(newSize);
            if (client.DungeonMap)
            {
                _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new UpdateSizePacket(client.Partner.GeneralHandler, client.Partner.Size).Serialize());
            }
            else
            {
                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new UpdateSizePacket(client.Partner.GeneralHandler, client.Partner.Size).Serialize());
            }

            client.Partner.SetBaseStatus(
                _statusManager.GetDigimonBaseStatus(
                    client.Partner.CurrentType,
                    client.Partner.Level,
                    client.Partner.Size
                )
            );

            if (rare)
            {
                _mapServer.BroadcastGlobal(new NeonMessagePacket(NeonMessageTypeEnum.Scale, client.Tamer.Name, client.Partner.BaseType, client.Partner.Size).Serialize());
                _dungeonServer.BroadcastGlobal(new NeonMessagePacket(NeonMessageTypeEnum.Scale, client.Tamer.Name, client.Partner.BaseType, client.Partner.Size).Serialize());
            }

            await _sender.Send(new UpdateItemCommand(targetItem));
            await _sender.Send(new UpdateDigimonSizeCommand(client.Partner.Id, client.Partner.Size));

            client.Send(
                UtilitiesFunctions.GroupPackets(
                    new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                    new UpdateStatusPacket(client.Tamer).Serialize(),
                    new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                )
            );
        }

        private async Task ConsumeFoodItem(GameClient client, short itemSlot, ItemModel targetItem)
        {
            if (targetItem.ItemInfo?.SkillInfo == null)
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new SystemMessagePacket($"Invalid skill info for item id {targetItem.ItemId}.").Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );

                _logger.Warning($"Invalid skill info for item id {targetItem.ItemId} and tamer id {client.TamerId}.");
                return;
            }

            foreach (var apply in targetItem.ItemInfo?.SkillInfo.Apply)
            {
                switch (apply.Type)
                {
                    case SkillCodeApplyTypeEnum.Percent:
                    case SkillCodeApplyTypeEnum.AlsoPercent:
                        {
                            switch (apply.Attribute)
                            {
                                case SkillCodeApplyAttributeEnum.HP:
                                    {
                                        switch (targetItem.ItemInfo.Target)
                                        {
                                            case ItemConsumeTargetEnum.Both:
                                                {
                                                    client.Tamer.RecoverHp((int)Math.Ceiling((double)(apply.Value) / 100 * client.Tamer.HP));
                                                    client.Partner.RecoverHp((int)Math.Ceiling((double)(apply.Value) / 100 * client.Partner.HP));
                                                }
                                                break;

                                            case ItemConsumeTargetEnum.Digimon:
                                                client.Partner.RecoverHp((int)Math.Ceiling((double)(apply.Value) / 100 * client.Partner.HP));
                                                break;

                                            case ItemConsumeTargetEnum.Tamer:
                                                client.Tamer.RecoverHp((int)Math.Ceiling((double)(apply.Value) / 100 * client.Tamer.HP));
                                                break;
                                        }
                                    }
                                    break;

                                case SkillCodeApplyAttributeEnum.DS:
                                    {
                                        switch (targetItem.ItemInfo.Target)
                                        {
                                            case ItemConsumeTargetEnum.Both:
                                                {
                                                    client.Tamer.RecoverDs((int)Math.Ceiling((double)(apply.Value) / 100 * client.Tamer.DS));
                                                    client.Partner.RecoverDs((int)Math.Ceiling((double)(apply.Value) / 100 * client.Partner.DS));
                                                }
                                                break;

                                            case ItemConsumeTargetEnum.Digimon:
                                                client.Partner.RecoverDs((int)Math.Ceiling((double)(apply.Value) / 100 * client.Partner.DS));
                                                break;

                                            case ItemConsumeTargetEnum.Tamer:
                                                client.Partner.RecoverDs((int)Math.Ceiling((double)(apply.Value) / 100 * client.Partner.DS));
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    case SkillCodeApplyTypeEnum.Default:
                        {
                            switch (apply.Attribute)
                            {
                                case SkillCodeApplyAttributeEnum.HP:
                                    {
                                        switch (targetItem.ItemInfo.Target)
                                        {
                                            case ItemConsumeTargetEnum.Both:
                                                {
                                                    client.Tamer.RecoverHp(apply.Value);
                                                    client.Partner.RecoverHp(apply.Value);
                                                }
                                                break;

                                            case ItemConsumeTargetEnum.Digimon:
                                                client.Partner.RecoverHp(apply.Value);
                                                break;

                                            case ItemConsumeTargetEnum.Tamer:
                                                client.Tamer.RecoverHp(apply.Value);
                                                break;
                                        }
                                    }
                                    break;

                                case SkillCodeApplyAttributeEnum.DS:
                                    {
                                        switch (targetItem.ItemInfo.Target)
                                        {
                                            case ItemConsumeTargetEnum.Both:
                                                {
                                                    client.Tamer.RecoverDs(apply.Value);
                                                    client.Partner.RecoverDs(apply.Value);
                                                    client.Send(new UpdateCurrentResourcesPacket(client.Tamer.GeneralHandler, (short)client.Tamer.CurrentHp, (short)client.Tamer.CurrentDs, (short)0));
                                                    client.Send(new UpdateCurrentResourcesPacket(client.Tamer.Partner.GeneralHandler, (short)client.Tamer.Partner.CurrentHp, (short)client.Tamer.Partner.CurrentDs, (short)0));

                                                    if (client.DungeonMap)
                                                    {
                                                        _dungeonServer.BroadcastForTargetTamers(client.TamerId,
                                                            UtilitiesFunctions.GroupPackets(
                                                                new UpdateCurrentHPRatePacket(
                                                                    client.Tamer.GeneralHandler,
                                                                    client.Tamer.HpRate).Serialize(),
                                                                new UpdateCurrentHPRatePacket(
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    client.Tamer.Partner.HpRate).Serialize()
                                                            )
                                                        );
                                                    }
                                                    else
                                                    {
                                                        _mapServer.BroadcastForTargetTamers(client.TamerId,
                                                            UtilitiesFunctions.GroupPackets(
                                                                new UpdateCurrentHPRatePacket(
                                                                    client.Tamer.GeneralHandler,
                                                                    client.Tamer.HpRate).Serialize(),
                                                                new UpdateCurrentHPRatePacket(
                                                                    client.Tamer.Partner.GeneralHandler,
                                                                    client.Tamer.Partner.HpRate).Serialize()
                                                            )
                                                        );
                                                    }
                                                }
                                                break;

                                            case ItemConsumeTargetEnum.Digimon:
                                                client.Partner.RecoverDs(apply.Value);
                                                client.Send(new UpdateCurrentResourcesPacket(client.Tamer.Partner.GeneralHandler, (short)client.Tamer.Partner.CurrentHp, (short)client.Tamer.Partner.CurrentDs, (short)0));

                                                if (client.DungeonMap)
                                                {
                                                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new UpdateCurrentHPRatePacket(client.Tamer.Partner.GeneralHandler, client.Tamer.Partner.HpRate).Serialize());
                                                }
                                                else
                                                {
                                                    _mapServer.BroadcastForTargetTamers(client.TamerId, new UpdateCurrentHPRatePacket(client.Tamer.Partner.GeneralHandler, client.Tamer.Partner.HpRate).Serialize());
                                                }
                                                break;

                                            case ItemConsumeTargetEnum.Tamer:
                                                client.Tamer.RecoverDs(apply.Value);
                                                client.Send(new UpdateCurrentResourcesPacket(client.Tamer.GeneralHandler, (short)client.Tamer.CurrentHp, (short)client.Tamer.CurrentDs, (short)0));

                                                if (client.DungeonMap)
                                                {
                                                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new UpdateCurrentHPRatePacket(client.Tamer.GeneralHandler, client.Tamer.HpRate).Serialize());
                                                }
                                                else
                                                {
                                                    _mapServer.BroadcastForTargetTamers(client.TamerId, new UpdateCurrentHPRatePacket(client.Tamer.GeneralHandler, client.Tamer.HpRate).Serialize());
                                                }
                                                break;
                                        }
                                    }
                                    break;

                                case SkillCodeApplyAttributeEnum.XG:
                                    {
                                        switch (targetItem.ItemInfo.Target)
                                        {
                                            case ItemConsumeTargetEnum.Both:
                                                {
                                                    client.Tamer.SetXGauge(apply.Value);
                                                    client.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge, client.Tamer.XCrystals));
                                                }
                                                break;                                    
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }




            _logger.Verbose($"Character {client.TamerId} consumed {targetItem.ItemId}.");

            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1, itemSlot);

            await _sender.Send(new UpdateItemCommand(targetItem));

            client.Send(
                UtilitiesFunctions.GroupPackets(
                    new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                    new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                )
            );
        }

        private async Task IncreaseArchiveSlots(GameClient client, short itemSlot, ItemModel targetItem)
        {
            client.Tamer.DigimonArchive.AddSlot();

            _logger.Verbose($"Character {client.TamerId} used {targetItem.ItemId} to expand digimon archive slots to {client.Tamer.DigimonArchive.Slots}.");

            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

            await _sender.Send(new UpdateItemCommand(targetItem));
            await _sender.Send(new CreateCharacterDigimonArchiveSlotCommand(
                    client.Tamer.DigimonArchive.DigimonArchives.Last(),
                    client.Tamer.DigimonArchive.Id
                )
            );

            client.Send(
                UtilitiesFunctions.GroupPackets(
                    new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                    new DigimonArchiveLoadPacket(client.Tamer.DigimonArchive).Serialize(),
                    new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                )
            );
        }

        private async Task IncreaseDigimonSlots(GameClient client, short itemSlot, ItemModel targetItem)
        {
            client.Tamer.AddDigimonSlots();

            _logger.Verbose($"Character {client.TamerId} used {targetItem.ItemId} to expand digimon slots to {client.Tamer.DigimonSlots}.");

            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

            await _sender.Send(new UpdateCharacterDigimonSlotsCommand(client.Tamer.Id, client.Tamer.DigimonSlots));
            await _sender.Send(new UpdateItemCommand(targetItem));

            client.Send(
                UtilitiesFunctions.GroupPackets(
                    new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                    new UpdateDigimonSlotsPacket(client.Tamer.DigimonSlots).Serialize(),
                    new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                )
            );
        }

        private async Task IncreaseWarehouseSlots(GameClient client, short itemSlot, ItemModel targetItem)
        {
            var newSlot = client.Tamer.Warehouse.AddSlot();

            _logger.Verbose($"Character {client.TamerId} used {targetItem.ItemId} to expand warehouse slots to {client.Tamer.Warehouse.Size}.");

            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

            await _sender.Send(new UpdateItemCommand(targetItem));
            await _sender.Send(new AddInventorySlotCommand(newSlot));

            client.Send(
                UtilitiesFunctions.GroupPackets(
                    new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                    new LoadInventoryPacket(client.Tamer.Warehouse, InventoryTypeEnum.Warehouse).Serialize()
                )
            );
        }

        private async Task IncreaseInventorySlots(GameClient client, short itemSlot, ItemModel targetItem)
        {
            var newSlot = client.Tamer.Inventory.AddSlot();

            _logger.Verbose($"Character {client.TamerId} used {targetItem.ItemId} to expand inventory slots to {client.Tamer.Inventory.Size}.");

            client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);

            await _sender.Send(new UpdateItemCommand(targetItem));
            await _sender.Send(new AddInventorySlotCommand(newSlot));

            client.Send(
                UtilitiesFunctions.GroupPackets(
                    new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                    new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                )
            );
        }

        private async Task ContainerItem(GameClient client, short itemSlot, ItemModel targetItem)
        {
            var containerItem = client.Tamer.Inventory.FindItemBySlot(itemSlot);
            var ItemId = 0;

            if (containerItem == null || containerItem.ItemId == 0 || containerItem.ItemInfo == null)
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new SystemMessagePacket($"Invalid item on slot {itemSlot} for tamer {client.TamerId}").Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
                _logger.Warning($"Invalid item on slot {itemSlot} for tamer {client.TamerId}.");
                return;
            }

            var containerAsset = _assets.Container.FirstOrDefault(x => x.ItemId == containerItem.ItemId);
            if (containerAsset == null)
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new SystemMessagePacket($"No container configuration for item id {containerItem.ItemId}.").Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
                _logger.Warning($"No container configuration for item id {containerItem.ItemId}");
                return;
            }

            if (!containerAsset.Rewards.Any())
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new SystemMessagePacket($"Container config for item {containerAsset.ItemId} has incorrect rewards configuration.").Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
                _logger.Warning($"Container config for item {containerAsset.ItemId} has incorrect rewards configuration.");
                return;
            }

            var receivedItems = new List<ItemModel>();
            var possibleRewards = containerAsset.Rewards.OrderBy(x => Guid.NewGuid()).ToList();
            var rewardsToReceive = containerAsset.RewardAmount;
            var receivedRewardsAmount = 0;
            var error = false;

            ItemId = containerItem.ItemId;

            var needChance = rewardsToReceive < possibleRewards.Count;

            while (receivedRewardsAmount < rewardsToReceive && !error)
            {
                foreach (var possibleReward in possibleRewards)
                {
                    if (needChance && possibleReward.Chance < UtilitiesFunctions.RandomDouble())
                        continue;

                    var contentItem = new ItemModel();
                    contentItem.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == possibleReward.ItemId));

                    if (contentItem.ItemInfo == null)
                    {
                        client.Send(new SystemMessagePacket($"Invalid item info for item {possibleReward.ItemId}."));
                        _logger.Warning($"Invalid item info for item {possibleReward.ItemId} in tamer {client.TamerId} scan.");
                        error = true;
                        return;
                    }

                    contentItem.SetItemId(possibleReward.ItemId);
                    contentItem.SetAmount(UtilitiesFunctions.RandomInt(possibleReward.MinAmount, possibleReward.MaxAmount));

                    if (contentItem.IsTemporary)
                        contentItem.SetRemainingTime((uint)contentItem.ItemInfo.UsageTimeMinutes);

                    var tempItem = (ItemModel)contentItem.Clone();
                    receivedItems.Add(tempItem);
                    receivedRewardsAmount++;

                    if (receivedRewardsAmount >= rewardsToReceive || error)
                        break;
                }
            }

            if (error)
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
            }
            else
            {
                var receiveList = string.Join(',', receivedItems.Select(x => $"{x.ItemId} x{x.Amount}"));

                _logger.Verbose($"Character {client.TamerId} openned box {containerItem.ItemId} and obtained {receiveList}");

                client.Tamer.Inventory.RemoveOrReduceItem(containerItem, 1, itemSlot);

                client.Send(
             UtilitiesFunctions.GroupPackets(
                 new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                 new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
             )
         );

                receivedItems.ForEach(receivedItem =>
                {
                    client.Tamer.Inventory.AddItem(receivedItem);
                    client.Send(new ReceiveItemPacket(receivedItem, InventoryTypeEnum.Inventory));
                });

                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));



                if (ItemId == 70102) // TODO: Mudar 
                {
                    var buffData = new List<(int BuffId, int Value1, int Value2)>
                    {
                        (50121, 2700022, 2592000),
                        (50122, 2700023, 2592000),
                        (50123, 2700024, 2592000)
                    };

                    foreach (var (BuffId, Value1, Value2) in buffData)
                    {
                        var buff = _assets.BuffInfo.FirstOrDefault(x => x.SkillCode == Value1);
                        if (buff != null)
                        {
                            if (!client.Tamer.BuffList.Buffs.Any(x => x.BuffId == BuffId))
                            {

                                var duration = UtilitiesFunctions.RemainingTimeSeconds(Value2);

                                var newCharacterBuff = CharacterBuffModel.Create(BuffId, Value1, Value2);
                                newCharacterBuff.SetBuffInfo(buff);

                                client.Tamer.BuffList.Add(newCharacterBuff);
                                await _sender.Send(new UpdateCharacterBuffListCommand(client.Tamer.BuffList));

                                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Tamer.GeneralHandler, buff, (short)0, duration).Serialize());

                            }
                            else
                            {

                                var BuffInfo = client.Tamer.BuffList.Buffs.FirstOrDefault(x => x.BuffId == BuffId);

                                if (BuffInfo != null)
                                {

                                    BuffInfo.SetDuration(Value2);

                                    var duration = UtilitiesFunctions.RemainingTimeSeconds(BuffInfo.Duration);

                                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                  new UpdateBuffPacket(client.Tamer.GeneralHandler, buff, (short)0, duration).Serialize());


                                    await _sender.Send(new UpdateCharacterBuffListCommand(client.Tamer.BuffList));
                                }
                            }
                        }

                        client.IncreaseMembershipDuration(2592000);
                        client.Send(new MembershipPacket(client.MembershipExpirationDate!.Value, client.MembershipUtcSeconds));
                        await _sender.Send(new UpdateAccountMembershipCommand(client.AccountId, client.MembershipExpirationDate));

                        client.Send(new UpdateStatusPacket(client.Tamer));
                    }
                }
            }
        }

        private async Task BuffItem(GameClient client, short itemSlot, ItemModel targetItem)
        {
            var buff = _assets.BuffInfo.FirstOrDefault(x => x.SkillCode == targetItem.ItemInfo.SkillCode);

            if (buff != null)
            {
                var duration = UtilitiesFunctions.RemainingTimeSeconds(targetItem.ItemInfo.TimeInSeconds);

                var newCharacterBuff = CharacterBuffModel.Create(buff.BuffId, buff.SkillId, targetItem.ItemInfo.TypeN, targetItem.ItemInfo.TimeInSeconds);
                newCharacterBuff.SetBuffInfo(buff);

                var newDigimonBuff = DigimonBuffModel.Create(buff.BuffId, buff.SkillId, targetItem.ItemInfo.TypeN, targetItem.ItemInfo.TimeInSeconds);

                newDigimonBuff.SetBuffInfo(buff);

                var characterBuffs = new List<SkillCodeApplyAttributeEnum>
                    {
                        SkillCodeApplyAttributeEnum.MS,
                        SkillCodeApplyAttributeEnum.MovementSpeedIncrease,
                        SkillCodeApplyAttributeEnum.EXP,
                        SkillCodeApplyAttributeEnum.AttributeExperienceAdded
                    };

                if (characterBuffs.Contains(buff.SkillInfo.Apply.First().Attribute))
                {
                    if (client.Tamer.BuffList.ActiveBuffs.Any(x => x.BuffId == buff.BuffId))
                    {
                        if (client.DungeonMap)
                        {
                            client.Tamer.BuffList.ForceExpired(newCharacterBuff.BuffId);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new RemoveBuffPacket(client.Tamer.GeneralHandler, newCharacterBuff.BuffId).Serialize());

                            client.Tamer.BuffList.Add(newCharacterBuff);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Tamer.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                        else
                        {
                            client.Tamer.BuffList.ForceExpired(newCharacterBuff.BuffId);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new RemoveBuffPacket(client.Tamer.GeneralHandler, newCharacterBuff.BuffId).Serialize());

                            client.Tamer.BuffList.Add(newCharacterBuff);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Tamer.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                    }
                    else
                    {
                        if (client.DungeonMap)
                        {
                            client.Tamer.BuffList.Add(newCharacterBuff);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Tamer.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                        else
                        {
                            client.Tamer.BuffList.Add(newCharacterBuff);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Tamer.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                    }
                }
                else
                {
                    if (client.Partner.BuffList.ActiveBuffs.Any(x => x.BuffId == buff.BuffId))
                    {
                        if (client.DungeonMap)
                        {
                            client.Partner.BuffList.ForceExpired(newDigimonBuff.BuffId);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new RemoveBuffPacket(client.Partner.GeneralHandler, newCharacterBuff.BuffId).Serialize());

                            client.Partner.BuffList.Add(newDigimonBuff);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Partner.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                        else
                        {
                            client.Partner.BuffList.ForceExpired(newDigimonBuff.BuffId);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new RemoveBuffPacket(client.Partner.GeneralHandler, newCharacterBuff.BuffId).Serialize());

                            client.Partner.BuffList.Add(newDigimonBuff);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Partner.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                    }
                    else
                    {
                        if (client.DungeonMap)
                        {
                            client.Partner.BuffList.Add(newDigimonBuff);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Partner.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                        else
                        {
                            client.Partner.BuffList.Add(newDigimonBuff);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new AddBuffPacket(client.Partner.GeneralHandler, buff, (short)targetItem.ItemInfo.TypeN, duration).Serialize());
                        }
                    }
                }

                _logger.Verbose($"Character {client.TamerId} consumed {targetItem.ItemId} to get buff [todo: details].");

                client.Tamer.Inventory.RemoveOrReduceItem(targetItem, 1);
                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                await _sender.Send(new UpdateCharacterBuffListCommand(client.Tamer.BuffList));
                await _sender.Send(new UpdateDigimonBuffListCommand(client.Partner.BuffList));

                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeSuccessPacket(client.Tamer.GeneralHandler, itemSlot).Serialize(),
                        new UpdateStatusPacket(client.Tamer).Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
            }
            else
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemConsumeFailPacket(itemSlot, targetItem.ItemInfo.Type).Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );
            }
        }
    }
}