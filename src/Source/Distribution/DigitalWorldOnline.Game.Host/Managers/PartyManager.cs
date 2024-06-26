using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Mechanics;

namespace DigitalWorldOnline.Game.Managers
{
    public class PartyManager
    {
        private int _partyId;

        public List<GameParty> Parties { get; private set; }

        public PartyManager()
        {
            Parties = new();
        }

        public GameParty CreateParty(CharacterModel leader, CharacterModel member)
        {
            _partyId++;

            var party = GameParty.Create(_partyId, leader, member);
            Parties.Add(party);
            return party;
        }

        public GameParty? FindParty(long leaderOrMemberId)
        {
            var party = Parties.FirstOrDefault(x => x.Members.Values.Any(y => y.Id == leaderOrMemberId));
            if (party != null && party.Members.Count == 1)
            {
                RemoveParty(party.Id);
                return null;
            }

            return party;
        }

        public void RemoveParty(int partyId)
        {
            Parties.RemoveAll(x => x.Id == partyId);
        }
    }
}
