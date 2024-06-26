using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.Items;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class LoadGiftStoragePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.LoadGiftStorage;

        private readonly ILogger _logger;

        public LoadGiftStoragePacketProcessor(
            ILogger logger)
        {
            _logger = logger;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {           
            client.Send(new LoadGiftStoragePacket(client.Tamer.GiftWarehouse));
            _logger.Debug($"Sending loadgiftstorage packet for character {client.TamerId}...");
 
        }
    }
}