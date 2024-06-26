using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Delete;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.TamerShop;
using DigitalWorldOnline.Commons.Packets.PersonalShop;
using DigitalWorldOnline.GameHost;



using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ConsignedShopViewPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.ConsignedShopView;

        private readonly AssetsLoader _assets;
        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public ConsignedShopViewPacketProcessor(
            MapServer mapServer,
            AssetsLoader assets,
            ILogger logger,
            IMapper mapper,
            ISender sender)
        {
            _mapServer = mapServer;
            _assets = assets;
            _mapper = mapper;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            _logger.Debug($"Getting parameters...");
            packet.Skip(4);
            var handler = packet.ReadInt();

            _logger.Debug($"{handler}");

            _logger.Debug($"Searching consigned shop with handler {handler}...");
            var consignedShop = _mapper.Map<ConsignedShop>(await _sender.Send(new ConsignedShopByHandlerQuery(handler)));

            if (consignedShop == null)
            {
                _logger.Warning($"Consigned shop not found with handler {handler}.");
                _logger.Debug($"Sending consigned shop items view packet...");
                client.Send(new ConsignedShopItemsViewPacket());

                _logger.Debug($"Sending unload consigned shop packet...");
                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new UnloadConsignedShopPacket(handler).Serialize());
                return;
            }

            _logger.Debug($"Searching consigned shop owner with id {consignedShop.CharacterId}...");
            var shopOwner = _mapper.Map<CharacterModel>(await _sender.Send(new CharacterAndItemsByIdQuery(consignedShop.CharacterId)));

            if (shopOwner == null || shopOwner.ConsignedShopItems.Count == 0)
            {
                _logger.Debug($"Deleting consigned shop...");
                await _sender.Send(new DeleteConsignedShopCommand(handler));

                _logger.Debug($"Sending consigned shop items view packet...");
                client.Send(new ConsignedShopItemsViewPacket());

                _logger.Debug($"Sending unload consigned shop packet...");
                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new UnloadConsignedShopPacket(handler).Serialize());
                return;
            }

            foreach (var item in shopOwner.ConsignedShopItems.Items)
            {
                item.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == item.ItemId));

                //TODO: generalizar isso em rotina
                if (item.ItemId > 0 && item.ItemInfo == null)
                {
                    item.SetItemId();
                    shopOwner.ConsignedShopItems.CheckEmptyItems();
                    _logger.Debug($"Updating consigned shop item list...");
                    await _sender.Send(new UpdateItemsCommand(shopOwner.ConsignedShopItems));
                }
            }

            _logger.Debug($"Sending consigned shop item list view packet...");
            client.Send(new ConsignedShopItemsViewPacket(consignedShop, shopOwner.ConsignedShopItems, shopOwner.Name));
        }
    }
}