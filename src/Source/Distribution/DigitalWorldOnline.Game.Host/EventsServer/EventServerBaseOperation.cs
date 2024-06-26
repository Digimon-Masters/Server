using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Models;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Map;


namespace DigitalWorldOnline.GameHost.EventsServer
{
    public sealed partial class EventServer
    {
        private readonly int _startToSee = 8000;
        private readonly int _stopSeeing = 8001;

        /// <summary>
        /// The default hosted service "starting" method.
        /// </summary>
        /// <param name="cancellationToken">Control token for the operation</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var tasks = new List<Task>();

                    Maps.ForEach(map => { tasks.Add(RunMap(map, cancellationToken)); });

                    await Task.WhenAll(tasks);

                    await Task.Delay(300, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Unexpected map exception: {ex.Message} {ex.StackTrace}");
                    await Task.Delay(3000, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Gets the map latest mobs.
        /// </summary>
        /// <returns>The mobs collection</returns>
        private async Task<List<MobConfigModel>> GetMapMobs(GameMap map, CancellationToken token)
        {
            var mobList = new List<MobConfigModel>();

            var mobs = _mapper.Map<List<MobConfigModel>>(await _sender.Send(new MapMobsByIdQuery(map.MapId), token));

            if (!mobs.Any())
                return mobList;

            var id = 1;
            while (mobList.Count < MobAmount)
            {
                mobs.ForEach(mob =>
                {
                    var newMob = (MobConfigModel)mob.Clone();

                    var location = _randomPoints.First(x => x.Free);
                    location.UsePoint();

                    newMob.SetLocation((short)map.MapId, location.X, location.Y);
                    newMob.SetId(id);
                    id++;

                    mobList.Add(newMob);
                });
            }

            return mobList;
        }

        /// <summary>
        /// Runs the target map operations.
        /// </summary>
        /// <param name="map">the target map</param>
        private async Task RunMap(GameMap map, CancellationToken token)
        {
            try
            {
                if (!map.Initialized)
                {
                    var mobs = await GetMapMobs(map, token);
                    map.RequestMobsUpdate(mobs);
                    map.UpdateMobsList();
                }

                map.Initialize();
                map.ManageHandlers();

                await TamerOperation(map);
                await MonsterOperation(map);
                await DropsOperation(map);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error at event map running: {ex.Message} {ex.StackTrace}.");
            }
        }

        /// <summary>
        /// Adds a new gameclient to the target map.
        /// </summary>
        /// <param name="client">The game client to be added.</param>
        public Task AddClient(GameClient client)
        {
            var map = Maps
                    .FirstOrDefault(x => x.Initialized &&
                                         x.MapId == client.Tamer.Location.MapId);

            client.SetLoading();

            if (map != null)
            {
                client.Tamer.MobsInView.Clear();
                map.AddClient(client);
                client.Tamer.Revive();
            }
            else
            {
                Task.Run(() =>
                {
                    while (map == null)
                    {
                        Thread.Sleep(2000);
                        map = Maps
                            .FirstOrDefault(x => x.Initialized &&
                                                 x.MapId == client.Tamer.Location.MapId);

                        _logger.Warning($"Waiting map {client.Tamer.Location.MapId} initialization.");
                    }

                    client.Tamer.MobsInView.Clear();
                    map.AddClient(client);
                    client.Tamer.Revive();
                });
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes the gameclient from the target map.
        /// </summary>
        /// <param name="client">The gameclient to be removed.</param>
        public void RemoveClient(GameClient client)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == client.Tamer.Location.MapId);

            map?.RemoveClient(client);
        }

        public void BroadcastForChannel(byte channel, byte[] packet)
        {
            var maps = Maps.Where(x => x.Channel == channel).ToList();

            maps?.ForEach(map => { map.BroadcastForMap(packet); });
        }

        public void BroadcastGlobal(byte[] packet)
        {
            var maps = Maps.Where(x => x.Clients.Any()).ToList();

            maps?.ForEach(map => { map.BroadcastForMap(packet); });
        }

        public void BroadcastForMap(short mapId, byte[] packet)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == mapId);

            map?.BroadcastForMap(packet);
        }

        public void BroadcastForUniqueTamer(long tamerId, byte[] packet)
        {
            var map = Maps.FirstOrDefault(x => x.Clients.Any(x => x.TamerId == tamerId));

            map?.BroadcastForUniqueTamer(tamerId, packet);
        }

        public void BroadcastForTargetTamers(List<long> targetTamers, byte[] packet)
        {
            var map = Maps.FirstOrDefault(x => x.Clients.Any(x => targetTamers.Contains(x.TamerId)));

            map?.BroadcastForTargetTamers(targetTamers, packet);
        }

        public void BroadcastForTargetTamers(long sourceId, byte[] packet)
        {
            var map = Maps.FirstOrDefault(x => x.Clients.Any(x => x.TamerId == sourceId));

            map?.BroadcastForTargetTamers(map.TamersView[sourceId], packet);
        }

        public void BroadcastForTamerViewsAndSelf(long sourceId, byte[] packet)
        {
            var map = Maps.FirstOrDefault(x => x.Clients.Any(x => x.TamerId == sourceId));

            map?.BroadcastForTamerViewsAndSelf(sourceId, packet);
        }

        public void AddMapDrop(Drop drop)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == drop.Location.MapId);

            map?.DropsToAdd.Add(drop);
        }

        public void RemoveDrop(Drop drop)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == drop.Location.MapId);

            map?.RemoveMapDrop(drop);
        }

        public Drop? GetDrop(short mapId, int dropHandler)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == mapId);

            return map?.GetDrop(dropHandler);
        }

        //Mobs
        public bool MobsAttacking(short mapId, long tamerId)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == mapId);

            return map?.MobsAttacking(tamerId) ?? false;
        }

        public List<CharacterModel> GetNearbyTamers(short mapId, long tamerId)
        {
            var map = Maps.FirstOrDefault(x => x.MapId == mapId);

            return map?.NearbyTamers(tamerId);
        }

        public MobConfigModel? GetMobByHandler(short mapId, int handler)
        {
            return Maps.
                FirstOrDefault(x => x.MapId == mapId)?
                .Mobs
                .FirstOrDefault(x => x.GeneralHandler == handler);
        }

        public List<MobConfigModel> GetMobsNearbyPartner(Location location, int range)
        {
            var targetMap = Maps.FirstOrDefault(x => x.MapId == location.MapId);
            if (targetMap == null)
                return default;

            var originX = location.X;
            var originY = location.Y;

            return GetTargetMobs(targetMap.Mobs.Where(x => x.Alive).ToList(), originX, originY, range).DistinctBy(x => x.Id).ToList();
        }

        public List<MobConfigModel> GetMobsNearbyTargetMob(short mapId, int handler, int range)
        {
            var targetMap = Maps.FirstOrDefault(x => x.MapId == mapId);
            if (targetMap == null)
                return default;

            var originMob = targetMap.Mobs.FirstOrDefault(x => x.GeneralHandler == handler);

            if (originMob == null)
                return default;

            var originX = originMob.CurrentLocation.X;
            var originY = originMob.CurrentLocation.Y;

            var targetMobs = new List<MobConfigModel>();
            targetMobs.Add(originMob);

            targetMobs.AddRange(GetTargetMobs(targetMap.Mobs.Where(x => x.Alive).ToList(), originX, originY, range));

            return targetMobs.DistinctBy(x => x.Id).ToList();
        }

        public static List<MobConfigModel> GetTargetMobs(List<MobConfigModel> mobs, int originX, int originY, int range)
        {
            var targetMobs = new List<MobConfigModel>();

            foreach (var mob in mobs)
            {
                var mobX = mob.CurrentLocation.X;
                var mobY = mob.CurrentLocation.Y;

                var distance = CalculateDistance(originX, originY, mobX, mobY);

                if (distance <= range)
                {
                    targetMobs.Add(mob);
                }
            }

            return targetMobs;
        }

        private static double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            var deltaX = x2 - x1;
            var deltaY = y2 - y1;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
