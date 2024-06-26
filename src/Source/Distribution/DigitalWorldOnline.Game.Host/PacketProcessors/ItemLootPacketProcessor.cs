using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Enums.Party;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ItemLootPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.LootItem;

        private readonly PartyManager _partyManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly AssetsLoader _assets;
        private readonly ISender _sender;
        private readonly ILogger _logger;

        public ItemLootPacketProcessor(
            PartyManager partyManager,
            MapServer mapServer,
            AssetsLoader assets,
            ISender sender,
            ILogger logger,
            DungeonsServer dungeonServer)
        {
            _partyManager = partyManager;
            _assets = assets;
            _mapServer = mapServer;
            _sender = sender;
            _logger = logger;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var dropHandler = packet.ReadInt();

            var targetDrop = _mapServer.GetDrop(client.Tamer.Location.MapId, dropHandler);

            if (targetDrop == null)
            {
                var targetdungeonDrop = _dungeonServer.GetDrop(client.Tamer.Location.MapId, dropHandler, client.TamerId);
                if (targetdungeonDrop == null)
                {
                    client.Send(
                        UtilitiesFunctions.GroupPackets(
                            new SystemMessagePacket($"Drop has no data.").Serialize(),
                            new PickItemFailPacket(PickItemFailReasonEnum.Unknow).Serialize(),
                            new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                        )
                    );

                    _dungeonServer.BroadcastForTamerViewsAndSelf(
                        client.TamerId,
                        new UnloadDropsPacket(dropHandler).Serialize());
                    return;
                }
                else
                {

                    if (targetdungeonDrop.Collected)
                        return;

                    var dropDungeonClone = (ItemModel)targetdungeonDrop.DropInfo.Clone();
                    var dungeonParty = _partyManager.FindParty(client.TamerId);

                    var dungeonLootType = false;
                    var dungeonOrderType = PartyLootShareTypeEnum.Normal;

                    if (dungeonParty != null)
                    {
                        if (dungeonParty.LootType == PartyLootShareTypeEnum.Free)
                        {
                            dungeonLootType = true;
                        }
                        else if (dungeonParty.LootType == PartyLootShareTypeEnum.Order)
                        {
                            dungeonOrderType = PartyLootShareTypeEnum.Order;
                        }
                    }

                    if (targetdungeonDrop.OwnerId == client.TamerId || dungeonLootType)
                    {
                        targetdungeonDrop.SetCollected(true);

                        if (targetdungeonDrop.BitsDrop)
                        {

                            if (dungeonParty != null)
                            {
                                var partyClients = new List<GameClient>();

                                foreach (var partyMemberId in dungeonParty.Members.Values.Select(x => x.Id))
                                {
                                    var partyMemberClient = _dungeonServer.FindClientByTamerId(partyMemberId);
                                    if (partyMemberClient == null || partyMemberId == client.TamerId || partyMemberClient.Tamer.Location.MapId != client.Tamer.Location.MapId)
                                        continue;

                                    partyClients.Add(partyMemberClient);
                                }

                                partyClients.Add(client);

                                var bitsAmount = dropDungeonClone.Amount / partyClients.Count;

                                foreach (var partyClient in partyClients)
                                {
                                    partyClient.Tamer.Inventory.AddBits(bitsAmount);

                                    await UpdateItemListBits(partyClient);

                                    partyClient.Send(
                                        new PickBitsPacket(
                                            partyClient.Tamer.GeneralHandler,
                                            bitsAmount
                                        )
                                    );
                                }

                                _logger.Verbose($"Character {client.TamerId} looted bits x{bitsAmount} for party {dungeonParty.Id}.");
                            }
                            else
                            {
                                client.Send(new PickBitsPacket(
                                    client.Tamer.GeneralHandler,
                                    dropDungeonClone.Amount));

                                client.Tamer.Inventory.AddBits(dropDungeonClone.Amount);

                                _logger.Verbose($"Character {client.TamerId} looted bits x{dropDungeonClone.Amount}.");
                            }

                            _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new UnloadDropsPacket(targetdungeonDrop).Serialize());

                            _dungeonServer.RemoveDrop(targetdungeonDrop, client.TamerId);

                            await UpdateItemListBits(client);

                            client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

                        }
                        else
                        {
                            var itemInfo = _assets.ItemInfo.FirstOrDefault(x => x.ItemId == targetdungeonDrop.DropInfo.ItemId);

                            if (itemInfo == null)
                            {
                                _logger.Warning($"Item has no data info {targetdungeonDrop.DropInfo.ItemId}.");
                                client.Send(
                                    UtilitiesFunctions.GroupPackets(
                                        new PickItemFailPacket(PickItemFailReasonEnum.Unknow).Serialize(),
                                        new SystemMessagePacket($"Item has no data info {targetdungeonDrop.DropInfo.ItemId}.").Serialize(),
                                        new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                                    )
                                );
                                return;
                            }

                            var aquireClone = (ItemModel)targetdungeonDrop.DropInfo.Clone();

                            targetdungeonDrop.DropInfo.SetItemInfo(itemInfo);
                            dropDungeonClone.SetItemInfo(itemInfo);
                            aquireClone.SetItemInfo(itemInfo);

                            if (dungeonOrderType != PartyLootShareTypeEnum.Order)
                            {

                                if (client.Tamer.Inventory.AddItem(aquireClone))
                                {

                                    await UpdateItemsTask(client);

                                    _logger.Verbose($"Character {client.TamerId} looted item {dropDungeonClone.ItemId} x{dropDungeonClone.Amount}.");

                                    _dungeonServer.BroadcastForTamerViewsAndSelf(
                                        client.TamerId,
                                        UtilitiesFunctions.GroupPackets(
                                            new PickItemPacket(
                                                client.Tamer.AppearenceHandler,
                                                dropDungeonClone).Serialize(),
                                            new UnloadDropsPacket(targetdungeonDrop).Serialize()
                                        )
                                    );

                                    _dungeonServer.RemoveDrop(targetdungeonDrop, client.TamerId);

                                    if (dungeonParty != null)
                                    {

                                        _dungeonServer.BroadcastForTargetTamers(dungeonParty.GetMembersIdList(), new PartyLootItemPacket(client.Tamer, aquireClone).Serialize());
                                    }

                                }
                                else
                                {
                                    targetdungeonDrop.SetCollected(false);

                                    _logger.Verbose($"Character {client.TamerId} has not enough free space to loot drop handler {dropHandler} " +
                                        $"with item {targetdungeonDrop.DropInfo.ItemId} x{targetdungeonDrop.DropInfo.Amount}.");

                                    client.Send(new PickItemFailPacket(PickItemFailReasonEnum.InventoryFull));
                                }
                            }
                            else
                            {
                                var randomIndex = new Random().Next(dungeonParty.Members.Count + 1);
                                var sortedPlayer = dungeonParty.Members.ElementAt(randomIndex).Value;
                                var diceNumber = new Random().Next(0, 255);

                                var sortedClient = _mapServer.FindClientByTamerId(sortedPlayer.Id);

                                if (sortedClient != null)
                                {
                                    if (sortedClient.Tamer.Inventory.AddItem(aquireClone))
                                    {

                                        await UpdateItemsTask(sortedClient);

                                        _logger.Verbose($"Character {client.TamerId} looted item {dropDungeonClone.ItemId} x{dropDungeonClone.Amount}.");

                                        _dungeonServer.BroadcastForTamerViewsAndSelf(
                                            client.TamerId,
                                            UtilitiesFunctions.GroupPackets(
                                                new PickItemPacket(
                                                    client.Tamer.AppearenceHandler,
                                                    aquireClone).Serialize(),
                                                new UnloadDropsPacket(targetDrop).Serialize()
                                            )
                                        );

                                        _dungeonServer.RemoveDrop(targetDrop, client.TamerId);

                                        if (dungeonParty != null)
                                        {

                                            _dungeonServer.BroadcastForTargetTamers(dungeonParty.GetMembersIdList(), new PartyLootItemPacket(sortedClient.Tamer, aquireClone, (byte)diceNumber, client.Tamer.Name).Serialize());

                                        }

                                    }
                                    else
                                    {
                                        _logger.Verbose($"Character {client.TamerId} has not enough free space to loot drop handler {dropHandler} " +
                                            $"with item {targetDrop.DropInfo.ItemId} x{targetDrop.DropInfo.Amount}.");

                                        client.Send(new PickItemFailPacket(PickItemFailReasonEnum.InventoryFull));
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        _logger.Verbose($"Character {client.TamerId} has no right to loot drop handler {dropHandler} with item {targetDrop.DropInfo.ItemId} x{targetDrop.DropInfo.Amount}.");
                        client.Send(new PickItemFailPacket(PickItemFailReasonEnum.NotTheOwner));
                    }
                }
            }
            else
            {


                if (targetDrop.Collected)
                    return;


                var dropClone = (ItemModel)targetDrop.DropInfo.Clone();
                var party = _partyManager.FindParty(client.TamerId);

                var LootType = false;
                var OrderType = PartyLootShareTypeEnum.Normal;

                if (party != null)
                {
                    if (party.LootType == PartyLootShareTypeEnum.Free)
                    {
                        LootType = true;
                    }
                    else if (party.LootType == PartyLootShareTypeEnum.Order)
                    {
                        OrderType = PartyLootShareTypeEnum.Order;
                    }
                }

                if (targetDrop.OwnerId == client.TamerId || LootType)
                {
                    targetDrop.SetCollected(true);

                    if (targetDrop.BitsDrop)
                    {

                        if (party != null)
                        {
                            var partyClients = new List<GameClient>();

                            foreach (var partyMemberId in party.Members.Values.Select(x => x.Id))
                            {
                                var partyMemberClient = _mapServer.FindClientByTamerId(partyMemberId);
                                if (partyMemberClient == null || partyMemberId == client.TamerId || partyMemberClient.Tamer.Location.MapId != client.Tamer.Location.MapId)
                                    continue;

                                partyClients.Add(partyMemberClient);
                            }

                            partyClients.Add(client);

                            var bitsAmount = dropClone.Amount / partyClients.Count;

                            foreach (var partyClient in partyClients)
                            {
                                partyClient.Tamer.Inventory.AddBits(bitsAmount);

                                await UpdateItemListBits(partyClient);

                                partyClient.Send(
                                    new PickBitsPacket(
                                        partyClient.Tamer.GeneralHandler,
                                        bitsAmount
                                    )
                                );
                            }

                            _logger.Verbose($"Character {client.TamerId} looted bits x{bitsAmount} for party {party.Id}.");
                        }
                        else
                        {
                            client.Send(new PickBitsPacket(
                                client.Tamer.GeneralHandler,
                                dropClone.Amount));

                            client.Tamer.Inventory.AddBits(dropClone.Amount);

                            _logger.Verbose($"Character {client.TamerId} looted bits x{dropClone.Amount}.");
                        }

                        _mapServer.BroadcastForTamerViewsAndSelf(
                            client.TamerId,
                            new UnloadDropsPacket(targetDrop).Serialize());

                        _mapServer.RemoveDrop(targetDrop);

                        await UpdateItemListBits(client);

                        client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

                    }
                    else
                    {
                        var itemInfo = _assets.ItemInfo.FirstOrDefault(x => x.ItemId == targetDrop.DropInfo.ItemId);

                        if (itemInfo == null)
                        {
                            _logger.Warning($"Item has no data info {targetDrop.DropInfo.ItemId}.");
                            client.Send(
                                UtilitiesFunctions.GroupPackets(
                                    new PickItemFailPacket(PickItemFailReasonEnum.Unknow).Serialize(),
                                    new SystemMessagePacket($"Item has no data info {targetDrop.DropInfo.ItemId}.").Serialize(),
                                    new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory).Serialize()
                                )
                            );
                            return;
                        }

                        targetDrop.SetCollected(true);

                        var aquireClone = (ItemModel)targetDrop.DropInfo.Clone();

                        targetDrop.DropInfo.SetItemInfo(itemInfo);
                        dropClone.SetItemInfo(itemInfo);
                        aquireClone.SetItemInfo(itemInfo);

                        if (OrderType != PartyLootShareTypeEnum.Order)
                        {

                            if (client.Tamer.Inventory.AddItem(aquireClone))
                            {

                                await UpdateItemsTask(client);

                                _logger.Verbose($"Character {client.TamerId} looted item {dropClone.ItemId} x{dropClone.Amount}.");

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    UtilitiesFunctions.GroupPackets(
                                        new PickItemPacket(
                                            client.Tamer.AppearenceHandler,
                                            dropClone).Serialize(),
                                        new UnloadDropsPacket(targetDrop).Serialize()
                                    )
                                );

                                _mapServer.RemoveDrop(targetDrop);

                                if (party != null)
                                {

                                    _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(), new PartyLootItemPacket(client.Tamer, aquireClone).Serialize());
                                }

                            }
                            else
                            {
                                targetDrop.SetCollected(false);

                                _logger.Verbose($"Character {client.TamerId} has not enough free space to loot drop handler {dropHandler} " +
                                    $"with item {targetDrop.DropInfo.ItemId} x{targetDrop.DropInfo.Amount}.");

                                client.Send(new PickItemFailPacket(PickItemFailReasonEnum.InventoryFull));
                            }
                        }
                        else
                        {
                            var randomIndex = new Random().Next(party.Members.Count + 1);
                            var sortedPlayer = party.Members.ElementAt(randomIndex).Value;
                            var diceNumber = new Random().Next(0, 255);

                            var sortedClient = _mapServer.FindClientByTamerId(sortedPlayer.Id);

                            if (sortedClient != null)
                            {
                                if (sortedClient.Tamer.Inventory.AddItem(aquireClone))
                                {

                                    await UpdateItemsTask(sortedClient);

                                    _logger.Verbose($"Character {client.TamerId} looted item {dropClone.ItemId} x{dropClone.Amount}.");

                                    _mapServer.BroadcastForTamerViewsAndSelf(
                                        client.TamerId,
                                        UtilitiesFunctions.GroupPackets(
                                            new PickItemPacket(
                                                client.Tamer.AppearenceHandler,
                                                aquireClone).Serialize(),
                                            new UnloadDropsPacket(targetDrop).Serialize()
                                        )
                                    );

                                    _mapServer.RemoveDrop(targetDrop);

                                    if (party != null)
                                    {

                                        _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(), new PartyLootItemPacket(sortedClient.Tamer, aquireClone, (byte)diceNumber, client.Tamer.Name).Serialize());

                                    }

                                }
                                else
                                {
                                    _logger.Verbose($"Character {client.TamerId} has not enough free space to loot drop handler {dropHandler} " +
                                        $"with item {targetDrop.DropInfo.ItemId} x{targetDrop.DropInfo.Amount}.");

                                    sortedClient.Send(new PickItemFailPacket(PickItemFailReasonEnum.InventoryFull));
                                }
                            }
                        }

                    }
                }
                else
                {
                    _logger.Verbose($"Character {client.TamerId} has no right to loot drop handler {dropHandler} with item {targetDrop.DropInfo.ItemId} x{targetDrop.DropInfo.Amount}.");
                    client.Send(new PickItemFailPacket(PickItemFailReasonEnum.NotTheOwner));
                }
            }
        }

        public async Task UpdateItemsTask(GameClient client)
        {
            await Task.Run(async () =>
            {
                // Coloque seu código aqui para enviar o comando
                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
            });
        }
        public async Task UpdateItemListBits(GameClient client)
        {
            await Task.Run(async () =>
            {
                // Coloque seu código aqui para enviar o comando
                await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));
            });
        }


    }

}