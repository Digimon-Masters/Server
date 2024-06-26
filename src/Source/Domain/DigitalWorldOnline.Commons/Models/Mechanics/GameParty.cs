using DigitalWorldOnline.Commons.Enums.Party;
using DigitalWorldOnline.Commons.Models.Character;
using System.Reflection;

namespace DigitalWorldOnline.Commons.Models.Mechanics
{
    public class GameParty
    {
        private Dictionary<byte, CharacterModel> _members;
        public int Id { get; private set; }
        public PartyLootShareTypeEnum LootType { get; private set; }
        public PartyLootShareRarityEnum LootFilter { get; private set; }
        public int LeaderSlot { get; private set; }
        public long LeaderId { get; private set; }
        public DateTime CreateDate { get; }

        public Dictionary<byte, CharacterModel> Members
        {
            get
            {
                return _members
                    .OrderBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            private set
            {
                _members = value;
            }
        }

        public KeyValuePair<byte, CharacterModel> this[long memberId] => Members.First(x => x.Value.Id == memberId);
        public KeyValuePair<byte, CharacterModel> this[string memberName] => Members.First(x => x.Value.Name == memberName);

        private GameParty(
            int id,
            CharacterModel leader,
            CharacterModel member)
        {
            Id = id;
            CreateDate = DateTime.Now;
            LootType = PartyLootShareTypeEnum.Normal;
            LootFilter = PartyLootShareRarityEnum.Lv1;
            LeaderSlot = 0;
            LeaderId = leader.Id;

            Members = new()
            {
                { 0, leader },
                { 1, member }
            };
        }

        public static GameParty Create(
            int id,
            CharacterModel leader,
            CharacterModel member)
        {
            return new GameParty
            (
                id,
                leader,
                member
            );
        }
        public void AddMember(CharacterModel member)
        {
            byte newKey = (byte)(_members.Count);
            _members.Add(newKey, member);
        }

        public void ChangeLeader(int newLeaderSlot)
        {

            LeaderSlot = newLeaderSlot;
            LeaderId = _members.ElementAt(newLeaderSlot).Value.Id;
        }

        public void UpdateMember(KeyValuePair<byte, CharacterModel> member)
        {
            if (_members.ContainsKey(member.Key))
                _members[member.Key] = member.Value;
        }

        public void RemoveMember(byte memberSlot)
        {
            if (_members.ContainsKey(memberSlot))
            {
                _members.Remove(memberSlot);
                ReorderMembers();
            }
        }

        private void ReorderMembers()
        {
            var updatedMembers = _members.OrderBy(pair => pair.Key)
                                         .ToDictionary(pair => (byte)(pair.Key - 1), pair => pair.Value);

            _members.Clear();
            foreach (var kvp in updatedMembers)
            {
                if (kvp.Value.Id == LeaderId)
                    LeaderSlot = kvp.Key;

                    _members.Add(kvp.Key, kvp.Value);
            }
        }

        public void ChangeLootType(PartyLootShareTypeEnum lootType, PartyLootShareRarityEnum rareType)
        {
            LootType = lootType;
            LootFilter = rareType;
        }

        public List<long> GetMembersIdList() => _members.Values.Select(x => x.Id).ToList();

        public object Clone() => MemberwiseClone();
    }
}
