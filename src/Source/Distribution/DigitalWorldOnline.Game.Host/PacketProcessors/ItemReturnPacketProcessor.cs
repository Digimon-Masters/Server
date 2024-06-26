using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ItemReturnPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.ItemReturn;

        private readonly ISender _sender;
        private readonly ILogger _logger;

        public ItemReturnPacketProcessor(
            ISender sender,
            ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var vipEnabled = packet.ReadByte();
            var portableReturnPosition = packet.ReadInt();
            var npcId = packet.ReadInt();
            var itemSlot = packet.ReadInt();

            var inventoryItem = client.Tamer.Inventory.FindItemBySlot(itemSlot);
            if (inventoryItem == null || inventoryItem.ItemId == 0 || inventoryItem.ItemInfo == null)
            {
                client.Send(new SystemMessagePacket($"Invalid item at slot {itemSlot}."));
                _logger.Warning($"Invalid item on slot {itemSlot} for tamer {client.TamerId} on returning.");
                return;
            }

            var totalGain = (int)(inventoryItem.Amount * inventoryItem.ItemInfo.SellPrice);

            _logger.Verbose($"Character {client.TamerId} sold {inventoryItem.ItemId} x{inventoryItem.Amount} for {totalGain} bits.");

            var returnQuest = client.Tamer.Progress.InProgressQuestData.FirstOrDefault(x => x.QuestId == 4021);
            if (returnQuest != null && inventoryItem.ItemId == 9072)
            {
                returnQuest.UpdateCondition(0, 1);
                var questToUpdate = client.Tamer.Progress.InProgressQuestData.FirstOrDefault(x => x.QuestId == 4021);
                _sender.Send(new UpdateCharacterInProgressCommand(questToUpdate));
                client.Send(new QuestGoalUpdatePacket(4021, 1, 1));
            }

            client.Tamer.Inventory.AddBits(totalGain);
            client.Tamer.Inventory.RemoveOrReduceItem(inventoryItem, inventoryItem.Amount, itemSlot);

            await _sender.Send(new UpdateItemCommand(inventoryItem));
            await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));

            client.Send(new ItemReturnPacket(totalGain, client.Tamer.Inventory.Bits));
        }
    }
}