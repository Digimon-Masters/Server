using DigitalWorldOnline.Application.Admin.Commands;
using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Chat;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using DigitalWorldOnline.Infraestructure.Migrations;
using MediatR;
using Serilog;
using System.Xml.Linq;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TamerChangeNamePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TamerChangeName;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public TamerChangeNamePacketProcessor(
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
            var packet = new GamePacketReader(packetData);
           
            int itemSlot = packet.ReadInt();
            var newName = packet.ReadString();
            var oldName = client.Tamer.Name;
            var AvaliabeName = await _sender.Send(new CharacterByNameQuery(newName)) == null;

            if (!AvaliabeName)
            {
                client.Send(new TamerChangeNamePacket(CharacterChangeNameType.Existing, oldName, newName, itemSlot));
                return;
            }

            var inventoryItem = client.Tamer.Inventory.FindItemBySlot(itemSlot);

            if (inventoryItem != null)
            {
                client.Tamer.Inventory.RemoveOrReduceItem(inventoryItem, 1, itemSlot);
                client.Tamer.UpdateName(newName);

                await _sender.Send(new ChangeTamerNameByIdCommand(client.Tamer.Id,newName));
                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

                client.Send( new TamerChangeNamePacket(CharacterChangeNameType.Sucess,itemSlot,oldName, newName));
                client.Send( new TamerChangeNamePacket(CharacterChangeNameType.Complete, newName, newName, itemSlot));

                _logger.Verbose($"Character {client.TamerId} Changed Name {oldName} to {newName}.");
            }

        }
    }
}