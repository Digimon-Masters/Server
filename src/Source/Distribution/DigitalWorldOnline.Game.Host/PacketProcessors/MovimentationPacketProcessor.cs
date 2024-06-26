using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;

using MediatR;
using System.Diagnostics.Eventing.Reader;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class MovimentationPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TamerMovimentation;

        private readonly PartyManager _partyManager;
        private readonly PvpServer _pvpServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly MapServer _mapServer;
        private readonly ISender _sender;

        public MovimentationPacketProcessor(
            PartyManager partyManager,
            PvpServer pvpServer,
            MapServer mapServer,
            ISender sender,
            DungeonsServer dungeonServer)
        {
            _partyManager = partyManager;
            _pvpServer = pvpServer;
            _mapServer = mapServer;
            _sender = sender;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var ticks = packet.ReadInt();
            var handler = packet.ReadInt();
            var newX = packet.ReadInt();
            var newY = packet.ReadInt();
            var newZ = packet.ReadFloat();

            if (client.PvpMap)
            {
                if (client.Tamer.PreviousCondition == ConditionEnum.Ride && client.Tamer.CurrentCondition == ConditionEnum.Away)
                {
                    client.Tamer.ResetAfkNotifications();
                    client.Tamer.UpdateCurrentCondition(ConditionEnum.Ride);
                    _pvpServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());
                }

                if (client.Tamer.Riding)
                {
                    client.Tamer.NewLocation(newX, newY, newZ);
                    client.Tamer.Partner.NewLocation(newX, newY, newZ);

                    _pvpServer.BroadcastForTargetTamers(client.TamerId, new TamerWalkPacket(client.Tamer).Serialize());
                    _pvpServer.BroadcastForTargetTamers(client.TamerId, new DigimonWalkPacket(client.Tamer.Partner).Serialize());
                }
                else
                {
                    if (client.Tamer.CurrentCondition == ConditionEnum.Away)
                    {
                        client.Tamer.ResetAfkNotifications();
                        client.Tamer.UpdateCurrentCondition(ConditionEnum.Default);
                        _pvpServer.BroadcastForTargetTamers(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());
                    }

                    if (handler >= short.MaxValue)
                    {
                        client.Tamer.NewLocation(newX, newY, newZ);
                        _pvpServer.BroadcastForTargetTamers(client.TamerId, new TamerWalkPacket(client.Tamer).Serialize());
                    }
                    else
                    {
                        client.Tamer.Partner.NewLocation(newX, newY, newZ);
                        _pvpServer.BroadcastForTargetTamers(client.TamerId, new DigimonWalkPacket(client.Tamer.Partner).Serialize());
                    }
                }
            }
            else if(client.DungeonMap)
            {
                if (client.Tamer.PreviousCondition == ConditionEnum.Ride && client.Tamer.CurrentCondition == ConditionEnum.Away)
                {
                    client.Tamer.ResetAfkNotifications();
                    client.Tamer.UpdateCurrentCondition(ConditionEnum.Ride);
                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());
                }

                if (client.Tamer.Riding)
                {
                    client.Tamer.NewLocation(newX, newY, newZ);
                    client.Tamer.Partner.NewLocation(newX, newY, newZ);

                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new TamerWalkPacket(client.Tamer).Serialize());
                    _dungeonServer.BroadcastForTargetTamers(client.TamerId, new DigimonWalkPacket(client.Tamer.Partner).Serialize());
                }
                else
                {
                    if (client.Tamer.CurrentCondition == ConditionEnum.Away)
                    {
                        client.Tamer.ResetAfkNotifications();
                        client.Tamer.UpdateCurrentCondition(ConditionEnum.Default);
                        _dungeonServer.BroadcastForTargetTamers(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());
                    }

                    if (handler >= short.MaxValue)
                    {
                        client.Tamer.NewLocation(newX, newY, newZ);
                        _dungeonServer.BroadcastForTargetTamers(client.TamerId, new TamerWalkPacket(client.Tamer).Serialize());
                    }
                    else
                    {
                        client.Tamer.Partner.NewLocation(newX, newY, newZ);
                        _dungeonServer.BroadcastForTargetTamers(client.TamerId, new DigimonWalkPacket(client.Tamer.Partner).Serialize());
                    }
                }

                var party = _partyManager.FindParty(client.TamerId);
                if (party != null)
                {
                    party.UpdateMember(party[client.TamerId]);

                    _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                        new PartyMemberMovimentationPacket(party[client.TamerId]).Serialize());

                    _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                       new PartyMemberMovimentationPacket(party[client.TamerId]).Serialize());
                }

            }
            else
            {
                if (client.Tamer.PreviousCondition == ConditionEnum.Ride && client.Tamer.CurrentCondition == ConditionEnum.Away)
                {
                    client.Tamer.ResetAfkNotifications();
                    client.Tamer.UpdateCurrentCondition(ConditionEnum.Ride);
                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());
                }

                if (client.Tamer.Riding)
                {
                    client.Tamer.NewLocation(newX, newY, newZ);
                    client.Tamer.Partner.NewLocation(newX, newY, newZ);

                    _mapServer.BroadcastForTargetTamers(client.TamerId, new TamerWalkPacket(client.Tamer).Serialize());
                    _mapServer.BroadcastForTargetTamers(client.TamerId, new DigimonWalkPacket(client.Tamer.Partner).Serialize());
                }
                else
                {
                    if (client.Tamer.CurrentCondition == ConditionEnum.Away)
                    {
                        client.Tamer.ResetAfkNotifications();
                        client.Tamer.UpdateCurrentCondition(ConditionEnum.Default);
                        _mapServer.BroadcastForTargetTamers(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());
                    }

                    if (handler >= short.MaxValue)
                    {
                        client.Tamer.NewLocation(newX, newY, newZ);
                        _mapServer.BroadcastForTargetTamers(client.TamerId, new TamerWalkPacket(client.Tamer).Serialize());
                    }
                    else
                    {
                        client.Tamer.Partner.NewLocation(newX, newY, newZ);
                        _mapServer.BroadcastForTargetTamers(client.TamerId, new DigimonWalkPacket(client.Tamer.Partner).Serialize());
                    }
                }

                var party = _partyManager.FindParty(client.TamerId);
                if (party != null)
                {
                    party.UpdateMember(party[client.TamerId]);

                    _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                        new PartyMemberMovimentationPacket(party[client.TamerId]).Serialize());

                    _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                       new PartyMemberMovimentationPacket(party[client.TamerId]).Serialize());
                }

                await _sender.Send(new UpdateCharacterLocationCommand(client.Tamer.Location));
                await _sender.Send(new UpdateDigimonLocationCommand(client.Partner.Location)); 
            }
            
        }
    }
}
