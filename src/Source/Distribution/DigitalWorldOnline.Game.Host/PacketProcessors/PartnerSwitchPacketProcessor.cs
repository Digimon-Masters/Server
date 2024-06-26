using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartnerSwitchPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerSwitch;

        private readonly PartyManager _partyManager;
        private readonly StatusManager _statusManager;
        private readonly AssetsLoader _assets;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public PartnerSwitchPacketProcessor(
            PartyManager partyManager,
            StatusManager statusManager,
            AssetsLoader assets,
            MapServer mapServer,
            DungeonsServer dungeonServer,
            ILogger logger,
            ISender sender
        )
        {
            _partyManager = partyManager;
            _statusManager = statusManager;
            _assets = assets;
            _mapServer = mapServer;
            _dungeonServer = dungeonServer;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var slot = packet.ReadByte();

            var previousId = client.Partner.Id;
            var previousType = client.Partner.CurrentType;

            var newPartner = client.Tamer.Digimons.First(x => x.Slot == slot);

            client.Tamer.RemovePartnerPassiveBuff();
            await _sender.Send(new UpdateDigimonBuffListCommand(client.Partner.BuffList));

            if (client.DungeonMap)
            {
                _dungeonServer.SwapDigimonHandlers(client.Tamer.Location.MapId, client.Partner, newPartner);
            }
            else
            {
                _mapServer.SwapDigimonHandlers(client.Tamer.Location.MapId, client.Partner, newPartner);
            }

            client.Tamer.SwitchPartner(slot);
            client.Partner.UpdateCurrentType(client.Partner.BaseType);
            client.Partner.SetTamer(client.Tamer);
            client.Partner.NewLocation(client.Tamer.Location.MapId, client.Tamer.Location.X, client.Tamer.Location.Y);

            client.Tamer.Partner.SetBaseInfo(
                _statusManager.GetDigimonBaseInfo(
                    client.Tamer.Partner.CurrentType
                )
            );

            client.Tamer.Partner.SetBaseStatus(
                _statusManager.GetDigimonBaseStatus(
                    client.Tamer.Partner.CurrentType,
                    client.Tamer.Partner.Level,
                    client.Tamer.Partner.Size
                )
            );

            client.Partner.SetSealStatus(_assets.SealInfo);

            if (client.Tamer.InBattle)
            {
                var battleTagItem = client.Tamer.Inventory.FindItemBySection(16400);

                if (client.Tamer.Inventory.RemoveOrReduceItem(battleTagItem, 1))
                {
                    _logger.Verbose($"Character {client.TamerId} consumed battle tag {battleTagItem.ItemId}.");
                    client.Send(new PartnerSwitchInBattlePacket(slot, client.Tamer.Model.GetHashCode()));
                }
            }

            client.Tamer.SetPartnerPassiveBuff();

            foreach (var buff in client.Tamer.Partner.BuffList.ActiveBuffs)
                buff.SetBuffInfo(_assets.BuffInfo.FirstOrDefault(x => x.SkillCode == buff.SkillId && buff.BuffInfo == null || x.DigimonSkillCode == buff.SkillId && buff.BuffInfo == null));

            if (client.DungeonMap)
            {
                _dungeonServer.BroadcastForTamerViewsAndSelf(
                client.TamerId,
                new PartnerSwitchPacket(client.Tamer.GenericHandler, previousType, client.Partner, slot).Serialize()
            );

            }
            else
            {
                _mapServer.BroadcastForTamerViewsAndSelf(
                client.TamerId,
                new PartnerSwitchPacket(client.Tamer.GenericHandler, previousType, client.Partner, slot).Serialize()
            );

            }

            if (client.Tamer.Partner.BuffList.Buffs.Any())
            {

                var buffToApply = client.Tamer.Partner.BuffList.Buffs;


                buffToApply.ForEach(buffToApply =>
                {
                    var Ts = 0;

                    if (buffToApply.Duration != 0)
                        Ts = UtilitiesFunctions.RemainingTimeSeconds(buffToApply.RemainingSeconds);

                    if (client.DungeonMap)
                    {
                        _dungeonServer.BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffInfo, (short)buffToApply.TypeN, Ts).Serialize());
                    }
                    else
                    {
                        _mapServer.BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffInfo, (short)buffToApply.TypeN, Ts).Serialize());
                    }

                });

            }


            client.Send(new UpdateStatusPacket(client.Tamer));

            if (client.Tamer.HasXai)
            {
                client.Send(new XaiInfoPacket(client.Tamer.Xai));
                client.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge,client.Tamer.XCrystals));
            }

            var party = _partyManager.FindParty(client.TamerId);

            if (party != null)
            {
                party.UpdateMember(party[client.TamerId]);

                _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
              new PartyMemberPartnerSwitchPacket(party[client.TamerId]).Serialize());

                _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                new PartyMemberPartnerSwitchPacket(party[client.TamerId]).Serialize());

            }

            await _sender.Send(new UpdatePartnerCurrentTypeCommand(client.Partner));
            await _sender.Send(new UpdateCharacterDigimonsOrderCommand(client.Tamer));
            await _sender.Send(new UpdateDigimonBuffListCommand(client.Partner.BuffList));

            _logger.Verbose($"Character {client.TamerId} switched partner {previousId}({previousType}) with {client.Partner.Id}({client.Partner.BaseType}).");
        }
    }
}