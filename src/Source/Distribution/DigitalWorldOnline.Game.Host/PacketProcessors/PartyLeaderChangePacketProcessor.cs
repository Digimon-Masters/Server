using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartyLeaderChangePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartyLeaderChange;

        private readonly PartyManager _partyManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;

        public PartyLeaderChangePacketProcessor(
            PartyManager partyManager,
            MapServer mapServer,
            DungeonsServer dungeonServer,
            ILogger logger)
        {
            _partyManager = partyManager;
            _mapServer = mapServer;
            _dungeonServer = dungeonServer;
            _logger = logger;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var newLeaderSlot = packet.ReadInt();

            var party = _partyManager.FindParty(client.TamerId);

            if (party != null)
            {
                party.ChangeLeader(newLeaderSlot);


                _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(), new PartyLeaderChangedPacket(newLeaderSlot).Serialize());
                _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(), new PartyLeaderChangedPacket(newLeaderSlot).Serialize());

                _logger.Verbose($"Character {client.TamerId} appointed party slot {newLeaderSlot} as leader.");
            }
            else
            {
                _logger.Warning($"Character {client.TamerId} appointed party leader to slot {newLeaderSlot} but was not in a party.");
            }

            return Task.CompletedTask;
        }
    }
}