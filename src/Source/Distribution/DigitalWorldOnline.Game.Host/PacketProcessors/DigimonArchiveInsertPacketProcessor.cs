using AutoMapper;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Game.Managers;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class DigimonArchiveInsertPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.DigimonArchiveInsert;

        private readonly StatusManager _statusManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public DigimonArchiveInsertPacketProcessor(
            StatusManager statusManager,
            IMapper mapper,
            ILogger logger,
            ISender sender)
        {
            _statusManager = statusManager;
            _mapper = mapper;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var vipEnabled = Convert.ToBoolean(packet.ReadByte());
            var digiviceSlot = packet.ReadInt();
            var archiveSlot = packet.ReadInt() - 1000;

            var digivicePartner = client.Tamer.Digimons.FirstOrDefault(x => x.Slot == digiviceSlot);
            var archivePartner = client.Tamer.DigimonArchive.DigimonArchives.First(x => x.Slot == archiveSlot);
            var price = client.Tamer.DigimonArchive.ArchivePrice(digivicePartner?.Level);

            if (digivicePartner == null)
            {
                await MovePartnerToDigivice(client, digiviceSlot, archiveSlot, digivicePartner, archivePartner);
            }
            else if (archivePartner.DigimonId == 0)
            {
                await MovePartnerToArchive(client, digiviceSlot, archiveSlot, digivicePartner, archivePartner, price);
            }
            else
            {
                client.Tamer.RemoveDigimon((byte)digiviceSlot, false);
                archivePartner.AddDigimon(digivicePartner.Id);

                digivicePartner.SetSlot(byte.MaxValue);
                await _sender.Send(new UpdateDigimonSlotCommand(digivicePartner.Id, digivicePartner.Slot));

                archivePartner.Digimon!.SetSlot((byte)digiviceSlot);
                
                client.Tamer.AddDigimon(archivePartner.Digimon);
                
                client.Tamer.Inventory.RemoveBits(price);

                await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));
                await _sender.Send(new UpdateCharacterDigimonsOrderCommand(client.Tamer));
                await _sender.Send(new UpdateCharacterDigimonArchiveItemCommand(archivePartner));

                _logger.Verbose($"Character {client.Tamer} swapped partner {digivicePartner.Id}({digivicePartner.BaseType}) with partner " +
                    $"{archivePartner.Id}({archivePartner.Digimon!.BaseType}) on digivice slot {digiviceSlot} and archive slot {archiveSlot} for {price} bits.");
            }

            client.Send(new DigimonArchiveManagePacket(digiviceSlot, archiveSlot, price));
        }

        private async Task MovePartnerToDigivice(
            GameClient client,
            int digiviceSlot,
            int archiveSlot,
            DigimonModel? digivicePartner,
            CharacterDigimonArchiveItemModel archivePartner)
        {
            digivicePartner = _mapper.Map<DigimonModel>(
                await _sender.Send(
                    new GetDigimonByIdQuery(archivePartner.DigimonId)
                )
            );

            digivicePartner.SetBaseInfo(
                _statusManager.GetDigimonBaseInfo(
                    digivicePartner.BaseType
                )
            );

            digivicePartner.SetBaseStatus(
                _statusManager.GetDigimonBaseStatus(
                    digivicePartner.BaseType,
                    digivicePartner.Level,
                    digivicePartner.Size
                )
            );

            digivicePartner.SetSlot((byte)digiviceSlot);

            archivePartner.RemoveDigimon();

            client.Tamer.AddDigimon(digivicePartner);

            await _sender.Send(new UpdateCharacterDigimonsOrderCommand(client.Tamer));
            await _sender.Send(new UpdateCharacterDigimonArchiveItemCommand(archivePartner));

            _logger.Verbose($"Character {client.Tamer} moved partner {digivicePartner.Id}({digivicePartner.BaseType}) " +
                $"from archive slot {archiveSlot} to digivice slot {digiviceSlot}.");
        }

        private async Task MovePartnerToArchive(
            GameClient client,
            int digiviceSlot,
            int archiveSlot,
            DigimonModel? digivicePartner,
            CharacterDigimonArchiveItemModel archivePartner,
            int price)
        {
            archivePartner.AddDigimon(digivicePartner.Id);
            digivicePartner.SetSlot(byte.MaxValue);

            client.Tamer.Inventory.RemoveBits(price);

            await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));
            await _sender.Send(new UpdateCharacterDigimonsOrderCommand(client.Tamer));
            await _sender.Send(new UpdateCharacterDigimonArchiveItemCommand(archivePartner));

            _logger.Verbose($"Character {client.Tamer} moved partner {digivicePartner.Id}({digivicePartner.BaseType}) " +
                $"to digimon archive at slot {archiveSlot} for {price} bits.");

            client.Tamer.RemoveDigimon(byte.MaxValue);
            await _sender.Send(new UpdateCharacterDigimonsOrderCommand(client.Tamer));
        }
    }
}