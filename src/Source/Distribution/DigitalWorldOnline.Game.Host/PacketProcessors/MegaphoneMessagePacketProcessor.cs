using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Chat;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class MegaphoneMessagePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.MegaphoneMessage;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly DungeonsServer _dungeonServer;
        public MegaphoneMessagePacketProcessor(
            MapServer mapServer,
            ILogger logger,
            ISender sender,
            DungeonsServer dungeonsServer)
        {
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;
            _dungeonServer = dungeonsServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var message = packet.ReadString();
            var unk = packet.ReadByte();
            var slot = packet.ReadByte();

            var inventoryItem = client.Tamer.Inventory.FindItemBySlot(slot);
            if (inventoryItem == null) 
            {
                client.Send(new SystemMessagePacket($"Unable to find item in slot {slot}."));
                _logger.Warning($"Item not found in slot {slot} for player {client.TamerId} megaphone.");
                return;
            }

            _mapServer.BroadcastGlobal(new ChatMessagePacket(message, ChatTypeEnum.Megaphone, client.Tamer.Name, 
                inventoryItem.ItemId, client.Tamer.Level).Serialize());


            _dungeonServer.BroadcastGlobal(new ChatMessagePacket(message, ChatTypeEnum.Megaphone, client.Tamer.Name,
                inventoryItem.ItemId, client.Tamer.Level).Serialize());

            _logger.Verbose($"Character {client.TamerId} sent megaphone with item {inventoryItem.ItemId} and message {message}.");
            await _sender.Send(new CreateChatMessageCommand(ChatMessageModel.Create(client.TamerId, message)));

            if (!inventoryItem.ItemInfo.TemporaryItem)
            {
                client.Tamer.Inventory.RemoveOrReduceItem(inventoryItem, 1, slot);
                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
            }
        }
    }
}