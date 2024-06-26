using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Models.Summon;

namespace DigitalWorldOnline.Commons.Models.Config
{
    public partial class MapConfigModel
    {
        /// <summary>
        /// Unique sequential identifier.
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Unique sequential identifier.
        /// </summary>
        public long DungeonId { get; private set; }


        /// <summary>
        /// Client id reference to target map.
        /// </summary>
        public int MapId { get; private set; }

        /// <summary>
        /// Map name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Map type enumeration.
        /// </summary>
        public MapTypeEnum Type { get; private set; }

        /// <summary>
        /// Child mobs.
        /// </summary>
        public List<MobConfigModel> Mobs { get; private set; }

        /// <summary>
        /// Child mobs.
        /// </summary>
        public List<SummonMobModel> SummonMobs { get; private set; }

        /// <summary>
        /// Kill spawns.
        /// </summary>
        public List<KillSpawnConfigModel> KillSpawns { get; private set; }

        public MapConfigModel(short mapid, List<MobConfigModel> mobs)
        {
            Type = MapTypeEnum.Default;
            MapId = mapid;
            Mobs = mobs;
        }

        public MapConfigModel()
        {
            Type = MapTypeEnum.Default;
            Mobs = new List<MobConfigModel>();
            SummonMobs = new List<SummonMobModel>();
            KillSpawns = new List<KillSpawnConfigModel>();
        }
    }
}
