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
    public class LoadAccountCashWarehousePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.LoadAccountWarehouse;

        private readonly ILogger _logger;

        public LoadAccountCashWarehousePacketProcessor(
            ILogger logger)
        {
            _logger = logger;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            client.Send(new LoadAccountWarehousePacket(client.Tamer.AccountCashWarehouse));
            _logger.Debug($"Sending loadaccountwarehouse packet for character {client.TamerId}...");

        }
    }
}