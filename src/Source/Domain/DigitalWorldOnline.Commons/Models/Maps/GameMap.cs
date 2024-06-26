using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.TamerShop;

namespace DigitalWorldOnline.Commons.Models.Map
{
    public sealed partial class GameMap : MapConfigModel
    {

        // Dynamic
        public byte Channel { get; private set; }
        public DateTime WithoutTamers { get; private set; }
        public DateTime NextDatabaseOperation { get; private set; }
        public bool Initialized { get; private set; }
        public bool Operating { get; private set; }
        public bool UpdateMobs { get; private set; }
        public List<MobConfigModel> MobsToAdd { get; private set; }
        public List<MobConfigModel> MobsToRemove { get; private set; }
        public List<GameClient> Clients { get; private set; }
        public List<Drop> Drops { get; private set; }
        public List<ConsignedShop> ConsignedShops { get; private set; }
        public Dictionary<long, List<long>> TamersView { get; private set; }
        public Dictionary<long, List<long>> MobsView { get; private set; }
        public Dictionary<long, List<long>> DropsView { get; private set; }
        public Dictionary<long, List<long>> ConsignedShopView { get; private set; }
        public Dictionary<short, long> TamerHandlers { get; private set; }
        public Dictionary<short, long> DigimonHandlers { get; private set; }
        public Dictionary<short, long> MobHandlers { get; private set; }
        public Dictionary<short, long> DropHandlers { get; private set; }
        public List<int> ColiseumMobs = new();

        public object DropsLock { get; private set; }
        public object ClientsLock { get; private set; }
        public object DigimonHandlersLock { get; private set; }
        public object TamerHandlersLock { get; private set; }

        public GameMap(short mapId, List<MobConfigModel> mobs, List<Drop> drops) : base(mapId, mobs)
        {
            Drops = drops;
            DropsLock = new object();
            ClientsLock = new object();
            DigimonHandlersLock = new object();
            TamerHandlersLock = new object();
        }

        public GameMap()
        {
            DropsLock = new object();
            ClientsLock = new object();
            DigimonHandlersLock = new object();
            TamerHandlersLock = new object();

            Channel = 0; //TODO: !
            Drops = new List<Drop>();
            Clients = new List<GameClient>();
            ConsignedShops = new List<ConsignedShop>();

            WithoutTamers = DateTime.MaxValue;
            NextDatabaseOperation = DateTime.Now.AddSeconds(30);
        }
    }
}