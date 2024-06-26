using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TradeConfirmationPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeConfirmation;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public TradeConfirmationPacketProcessor(
            MapServer mapServer,
            ILogger logger,
            ISender sender)
        {
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var targetClient = _mapServer.FindClientByTamerHandle(client.Tamer.TargetTradeGeneralHandle);


            client.Send(new TradeConfirmationPacket(client.Tamer.GeneralHandler));
            targetClient.Send(new TradeConfirmationPacket(client.Tamer.GeneralHandler));
            client.Tamer.SetTradeConfirm(true);

            if (client.Tamer.TradeConfirm && targetClient.Tamer.TradeConfirm)
            {
                if (client.Tamer.Inventory.TotalEmptySlots < targetClient.Tamer.TradeInventory.Count)
                {
                    InvalidTrade(client, targetClient);

                    return;

                }
                else if (targetClient.Tamer.Inventory.TotalEmptySlots < client.Tamer.TradeInventory.Count)
                {
                    InvalidTrade(client, targetClient);

                    return;
                }

                var firstTamerItems = client.Tamer.TradeInventory.EquippedItems.Select(x => $"{x.ItemId} x{x.Amount}");
                var secondTamerItems = targetClient.Tamer.TradeInventory.EquippedItems.Select(x => $"{x.ItemId} x{x.Amount}");
                var firstTamerBits = client.Tamer.TradeInventory.Bits;
                var secondTamerBits = targetClient.Tamer.TradeInventory.Bits;
              
                _logger.Verbose($"Character {client.TamerId} traded {string.Join('|', firstTamerItems)} and {firstTamerBits} with {targetClient.TamerId}.");
                _logger.Verbose($"Character {targetClient.TamerId} traded {string.Join('|', secondTamerItems)} and {secondTamerBits} with {client.TamerId}.");
               
                targetClient.Tamer.Inventory.AddItems(client.Tamer.TradeInventory.EquippedItems.Clone());
                client.Tamer.Inventory.RemoveOrReduceItems(client.Tamer.TradeInventory.EquippedItems.Clone());

                client.Tamer.Inventory.AddItems(targetClient.Tamer.TradeInventory.EquippedItems.Clone());
                targetClient.Tamer.Inventory.RemoveOrReduceItems(targetClient.Tamer.TradeInventory.EquippedItems.Clone());


                targetClient.Tamer.Inventory.AddBits(client.Tamer.TradeInventory.Bits);
                client.Tamer.Inventory.RemoveBits(client.Tamer.TradeInventory.Bits);


               client.Tamer.Inventory.AddBits(targetClient.Tamer.TradeInventory.Bits);
               targetClient.Tamer.Inventory.RemoveBits(targetClient.Tamer.TradeInventory.Bits);

                targetClient.Tamer.ClearTrade();
                client.Tamer.ClearTrade();

                client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));
                targetClient.Send(new LoadInventoryPacket(targetClient.Tamer.Inventory, InventoryTypeEnum.Inventory));

                client.Send(new TradeFinalConfirmationPacket(client.Tamer.GeneralHandler));
                targetClient.Send(new TradeFinalConfirmationPacket(client.Tamer.GeneralHandler));

                await _sender.Send(new UpdateItemsCommand(targetClient.Tamer.Inventory));
                await _sender.Send(new UpdateItemListBitsCommand(targetClient.Tamer.Inventory));

                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));
            }
        }

        private static void InvalidTrade(GameClient client, GameClient? targetClient)
        {
            client.Send(new TradeCancelPacket(targetClient.Tamer.GeneralHandler));
            client.Send(new PickItemFailPacket(PickItemFailReasonEnum.InventoryFull));
            targetClient.Send(new TradeCancelPacket(targetClient.Tamer.GeneralHandler));

            targetClient.Tamer.ClearTrade();
            client.Tamer.ClearTrade();
        }
    }

}

