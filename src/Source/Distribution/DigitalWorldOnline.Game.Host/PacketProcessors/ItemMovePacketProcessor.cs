﻿using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Asset;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ItemMovePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.MoveItem;

        private readonly MapServer _mapServer;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly DungeonsServer _dungeonServer;

        public ItemMovePacketProcessor(
            MapServer mapServer,
            ISender sender,
            ILogger logger,
            DungeonsServer dungeonsServer,
            IMapper mapper)
        {
            _mapServer = mapServer;
            _sender = sender;
            _logger = logger;
            _dungeonServer = dungeonsServer;
            _mapper = mapper;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var originSlot = packet.ReadShort();
            var destinationSlot = packet.ReadShort();

            var itemListMovimentation = UtilitiesFunctions.SwitchItemList(originSlot, destinationSlot);

            var success = SwapItems(client, originSlot, destinationSlot, itemListMovimentation);

            if (success)
            {
                switch (itemListMovimentation)
                {
                    case ItemListMovimentationEnum.InventoryToInventory:
                        {
                            client.Tamer.Inventory.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                        }
                        break;

                    case ItemListMovimentationEnum.EquipmentToInventory:
                    case ItemListMovimentationEnum.InventoryToEquipment:
                        {
                            client.Tamer.Inventory.CheckEmptyItems();
                            client.Tamer.Equipment.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Equipment));
                    
                            
                        }
                        break;

                    case ItemListMovimentationEnum.InventoryToDigivice:
                    case ItemListMovimentationEnum.DigiviceToInventory:
                        {
                            client.Tamer.Inventory.CheckEmptyItems();
                            client.Tamer.Digivice.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Digivice));
                        }
                        break;

                    case ItemListMovimentationEnum.InventoryToChipset:
                    case ItemListMovimentationEnum.ChipsetToInventory:
                        {
                            client.Tamer.Inventory.CheckEmptyItems();
                            client.Tamer.ChipSets.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.ChipSets));
                        }
                        break;

                    case ItemListMovimentationEnum.WarehouseToInventory:
                    case ItemListMovimentationEnum.InventoryToWarehouse:
                        {
                            client.Tamer.Inventory.CheckEmptyItems();
                            client.Tamer.Warehouse.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Warehouse));
                        }
                        break;

                    case ItemListMovimentationEnum.AccountWarehouseToInventory:
                    case ItemListMovimentationEnum.InventoryToAccountWarehouse:
                        {
                            client.Tamer.Inventory.CheckEmptyItems();
                            client.Tamer.AccountWarehouse.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.AccountWarehouse));
                        }
                        break;

                    case ItemListMovimentationEnum.AccountWarehouseToWarehouse:
                    case ItemListMovimentationEnum.WarehouseToAccountWarehouse:
                        {
                            client.Tamer.AccountWarehouse.CheckEmptyItems();
                            client.Tamer.Warehouse.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Warehouse));
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.AccountWarehouse));
                        }
                        break;

                    case ItemListMovimentationEnum.AccountWarehouseToAccountWarehouse:
                        {
                            client.Tamer.AccountWarehouse.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.AccountWarehouse));
                        }
                        break;

                    case ItemListMovimentationEnum.WarehouseToWarehouse:
                        {
                            client.Tamer.Warehouse.CheckEmptyItems();
                            await _sender.Send(new UpdateItemsCommand(client.Tamer.Warehouse));
                        }
                        break;
                }

                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemMoveSuccessPacket(originSlot, destinationSlot).Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );

                if (originSlot == GeneralSizeEnum.XaiSlot.GetHashCode())
                {
                    client.Tamer.Xai.RemoveXai();
                    client.Send(new XaiInfoPacket());
                    client.Send(new TamerXaiResourcesPacket(0, (short)client.Tamer.XGauge));
                    await _sender.Send(new UpdateCharacterXaiCommand(client.Tamer.Xai));
                }

                if (destinationSlot == GeneralSizeEnum.XaiSlot.GetHashCode())
                {
                    var ItemId = client.Tamer.Equipment.FindItemBySlot(destinationSlot - 1000).ItemId;

                    var XaiInfo = _mapper.Map<XaiAssetModel>(await _sender.Send(new XaiInformationQuery(ItemId)));

                    client.Tamer.Xai.EquipXai(XaiInfo.ItemId, XaiInfo.XGauge, XaiInfo.XCrystals);

                    client.Send(new XaiInfoPacket(client.Tamer.Xai));
                    client.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge, client.Tamer.XCrystals));

                    await _sender.Send(new UpdateCharacterXaiCommand(client.Tamer.Xai));
                }

                _logger.Verbose($"Character {client.TamerId} moved an item from {originSlot} to {destinationSlot}.");
            }
            else
            {
                client.Send(
                    UtilitiesFunctions.GroupPackets(
                        new ItemMoveFailPacket(originSlot, destinationSlot).Serialize(),
                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                    )
                );

                _logger.Warning($"Character {client.TamerId} failled to move item from {originSlot} to {destinationSlot}.");
            }
        }

        private bool SwapItems(GameClient client, short originSlot, short destinationSlot, ItemListMovimentationEnum itemListMovimentation)
        {
            switch (itemListMovimentation)
            {
                case ItemListMovimentationEnum.InventoryToInventory:
                    return client.Tamer.Inventory.MoveItem(originSlot, destinationSlot);

                case ItemListMovimentationEnum.InventoryToDigivice:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.DigiviceSlot.GetHashCode();

                        var sourceItem = client.Tamer.Inventory.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Digivice.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            client.Tamer.Digivice.AddItemWithSlot(sourceItem, dstSlot);
                            client.Tamer.Inventory.AddItemWithSlot(tempItem, srcSlot);

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)13, destItem, 1).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)13, destItem, 1).Serialize());
                            }
                        }
                        else
                        {
                            client.Tamer.Digivice.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, 13, destItem, 1).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, 13, destItem, 1).Serialize());

                            }
                        }

                        client.Send(new UpdateStatusPacket(client.Tamer));

                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                              new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.ChipsetToInventory:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.ChipsetMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.ChipSets.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Inventory.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);

                            client.Tamer.ChipSets.AddItemWithSlot(tempItem, srcSlot);

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, destItem, 1).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, destItem, 1).Serialize());

                            }
                        }
                        else
                        {
                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, new ItemModel(), 0).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, new ItemModel(), 0).Serialize());

                            }
                        }

                        client.Send(new UpdateStatusPacket(client.Tamer));

                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                            new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                            new UpdateMovementSpeedPacket(client.Tamer).Serialize());

                        }

                        return true;
                    }

                case ItemListMovimentationEnum.InventoryToChipset:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.ChipsetMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Inventory.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.ChipSets.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            client.Tamer.ChipSets.AddItemWithSlot(sourceItem, dstSlot);

                            client.Tamer.Inventory.AddItemWithSlot(tempItem, srcSlot);

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 1).Serialize());

                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 1).Serialize());

                            }
                        }
                        else
                        {
                            client.Tamer.ChipSets.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 0).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 0).Serialize());

                            }
                        }

                        client.Send(new UpdateStatusPacket(client.Tamer));

                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                            new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                       new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        return true;
                    }

                case ItemListMovimentationEnum.InventoryToEquipment:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();
                        var dstSlot = destinationSlot == GeneralSizeEnum.XaiSlot.GetHashCode() ? 11 : destinationSlot - GeneralSizeEnum.EquipmentMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Inventory.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Equipment.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            client.Tamer.Equipment.AddItemWithSlot(sourceItem, dstSlot);

                            client.Tamer.Inventory.AddItemWithSlot(tempItem, srcSlot);

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 1).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 1).Serialize());

                            }
                        }
                        else
                        {
                            client.Tamer.Equipment.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 1).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                      client.TamerId,
                                       new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)dstSlot, destItem, 1).Serialize());
                            }
                        }

                        client.Send(new UpdateStatusPacket(client.Tamer));

                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new UpdateMovementSpeedPacket(client.Tamer).Serialize());

                        }

                        //if (client.Tamer.HasXai)
                        //{
                        //    var xai = await _sender.Send(new XaiInformationQuery(client.Tamer.Xai?.ItemId ?? 0));
                        //    client.Tamer.SetXai(_mapper.Map<CharacterXaiModel>(xai));
                        //
                        //    client.Send(new XaiInfoPacket(client.Tamer.Xai));
                        //
                        //    client.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge, client.Tamer.XCrystals));
                        //}

                        return true;
                    }

                case ItemListMovimentationEnum.InventoryToWarehouse:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.WarehouseMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Inventory.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Warehouse.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            if (destItem.ItemId == sourceItem.ItemId)
                            {
                                destItem.IncreaseAmount(sourceItem.Amount);
                                sourceItem.ReduceAmount(sourceItem.Amount);
                            }
                            else
                            {

                                client.Tamer.Warehouse.AddItemWithSlot(sourceItem, dstSlot);

                                client.Tamer.Inventory.AddItemWithSlot(tempItem, srcSlot);
                            }

                        }
                        else
                        {
                            client.Tamer.Warehouse.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.InventoryToAccountWarehouse:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.AccountWarehouseMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Inventory.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.AccountWarehouse.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {

                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            if (destItem.ItemId == sourceItem.ItemId)
                            {
                                destItem.IncreaseAmount(sourceItem.Amount);
                                sourceItem.ReduceAmount(sourceItem.Amount);
                            }
                            else
                            {

                                client.Tamer.AccountWarehouse.AddItemWithSlot(sourceItem, dstSlot);

                                client.Tamer.Inventory.AddItemWithSlot(tempItem, srcSlot);
                            }

                        }
                        else
                        {
                            client.Tamer.AccountWarehouse.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.EquipmentToInventory:
                    {
                        var srcSlot = originSlot == GeneralSizeEnum.XaiSlot.GetHashCode() ? 11 : originSlot - GeneralSizeEnum.EquipmentMinSlot.GetHashCode();
                        var dstSlot = destinationSlot;

                        var sourceItem = client.Tamer.Equipment.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Inventory.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);

                            client.Tamer.Equipment.AddItemWithSlot(tempItem, srcSlot);

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, destItem, 1).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, destItem, 1).Serialize());
                            }
                        }
                        else
                        {
                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, new ItemModel(), 0).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, (byte)srcSlot, new ItemModel(), 0).Serialize());

                            }
                        }

                        client.Send(new UpdateStatusPacket(client.Tamer));

                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                               new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }

                        

                        return true;
                    }

                case ItemListMovimentationEnum.DigiviceToInventory:
                    {
                        var srcSlot = 0;
                        var dstSlot = destinationSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Digivice.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Inventory.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);

                            client.Tamer.Digivice.AddItemWithSlot(tempItem, srcSlot);

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, 13, destItem, 0).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                               client.TamerId,
                               new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, 13, destItem, 0).Serialize());
                            }
                        }
                        else
                        {
                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, 13, sourceItem, 0).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new UpdateTamerAppearancePacket(client.Tamer.AppearenceHandler, 13, sourceItem, 0).Serialize());

                            }
                        }

                        client.Send(new UpdateStatusPacket(client.Tamer));

                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                               new UpdateMovementSpeedPacket(client.Tamer).Serialize());
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.WarehouseToWarehouse:
                    {
                        var orgSlot = (short)(originSlot - (short)GeneralSizeEnum.WarehouseMinSlot);
                        var destSlot = (short)(destinationSlot - (short)GeneralSizeEnum.WarehouseMinSlot);

                        return client.Tamer.Warehouse.MoveItem(orgSlot, destSlot);
                    }

                case ItemListMovimentationEnum.AccountWarehouseToAccountWarehouse:
                    {
                        var orgSlot = (short)(originSlot - (short)GeneralSizeEnum.AccountWarehouseMinSlot);
                        var destSlot = (short)(destinationSlot - (short)GeneralSizeEnum.AccountWarehouseMinSlot);

                        return client.Tamer.AccountWarehouse.MoveItem(orgSlot, destSlot);
                    }

                case ItemListMovimentationEnum.WarehouseToInventory:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.WarehouseMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Warehouse.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Inventory.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            if (destItem.ItemId == sourceItem.ItemId)
                            {
                                destItem.IncreaseAmount(sourceItem.Amount);
                                sourceItem.ReduceAmount(sourceItem.Amount);
                            }
                            else
                            {

                                client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                                client.Tamer.Warehouse.AddItemWithSlot(tempItem, srcSlot);
                            }

                        }
                        else
                        {
                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.WarehouseToAccountWarehouse:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.WarehouseMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.AccountWarehouseMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.Warehouse.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.AccountWarehouse.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            if (destItem.ItemId == sourceItem.ItemId)
                            {
                                destItem.IncreaseAmount(sourceItem.Amount);
                                sourceItem.ReduceAmount(sourceItem.Amount);
                            }
                            else
                            {

                                client.Tamer.AccountWarehouse.AddItemWithSlot(sourceItem, dstSlot);
                                client.Tamer.Warehouse.AddItemWithSlot(tempItem, srcSlot);
                            }

                        }
                        else
                        {
                            client.Tamer.AccountWarehouse.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.AccountWarehouseToInventory:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.AccountWarehouseMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.InventoryMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.AccountWarehouse.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Inventory.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            if (destItem.ItemId == sourceItem.ItemId)
                            {
                                destItem.IncreaseAmount(sourceItem.Amount);
                                sourceItem.ReduceAmount(sourceItem.Amount);
                            }
                            else
                            {

                                client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                                client.Tamer.AccountWarehouse.AddItemWithSlot(tempItem, srcSlot);
                            }

                        }
                        else
                        {
                            client.Tamer.Inventory.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();
                        }

                        return true;
                    }

                case ItemListMovimentationEnum.AccountWarehouseToWarehouse:
                    {
                        var srcSlot = originSlot - GeneralSizeEnum.AccountWarehouseMinSlot.GetHashCode();
                        var dstSlot = destinationSlot - GeneralSizeEnum.WarehouseMinSlot.GetHashCode();

                        var sourceItem = client.Tamer.AccountWarehouse.FindItemBySlot(srcSlot);
                        var destItem = client.Tamer.Warehouse.FindItemBySlot(dstSlot);

                        if (destItem.ItemId > 0)
                        {
                            var tempItem = (ItemModel)destItem.Clone();
                            tempItem.SetItemInfo(destItem.ItemInfo);

                            if (destItem.ItemId == sourceItem.ItemId)
                            {
                                destItem.IncreaseAmount(sourceItem.Amount);
                                sourceItem.ReduceAmount(sourceItem.Amount);
                            }
                            else
                            {

                                client.Tamer.Warehouse.AddItemWithSlot(sourceItem, dstSlot);
                                client.Tamer.AccountWarehouse.AddItemWithSlot(tempItem, srcSlot);
                            }

                        }
                        else
                        {
                            client.Tamer.Warehouse.AddItemWithSlot(sourceItem, dstSlot);
                            sourceItem.SetItemId();
                        }

                        return true;
                    }
            }

            return false;
        }
    }
}