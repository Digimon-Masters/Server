using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Packets.MapServer;

namespace DigitalWorldOnline.GameHost.EventsServer
{
    public sealed partial class EventServer
    {
        private Task DropsOperation(GameMap map)
        {
            if (!map.ConnectedTamers.Any())
                return Task.CompletedTask;

            lock (map.DropsToAdd)
            {
                foreach (var drop in map.DropsToAdd)
                    map.AddDrop(drop);

                map.DropsToAdd.Clear();
            }

            foreach (var drop in map.Drops)
            {
                var nearTamers = map.NearestTamers(drop);
                var farTamers = map.FarawayTamers(drop);

                ShowAndHideDrop(map, drop, nearTamers, farTamers);

                CheckLostDrop(map, drop);

                //CheckExpiredDrop(map, drop);
            }

            lock (map.DropsToRemove)
            {
                foreach (var drop in map.DropsToRemove)
                    map.RemoveDrop(drop);

                map.DropsToRemove.Clear();
            }

            return Task.CompletedTask;
        }

        private void ShowAndHideDrop(GameMap map, Drop drop, List<long> nearTamers, List<long> farTamers)
        {
            foreach (var targetTamer in nearTamers)
            {
                if (!map.ViewingDrop(drop.Id, targetTamer))
                {
                    var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == targetTamer);

                    map.ShowDrop(drop.Id, targetTamer);
                    targetClient?.Send(new LoadDropsPacket(drop));
                }
            }

            foreach (var farTamer in farTamers)
            {
                if (map.ViewingDrop(drop.Id, farTamer) && !nearTamers.Contains(farTamer))
                {
                    var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == farTamer);

                    map.HideDrop(drop.Id, farTamer);
                    targetClient?.Send(new UnloadDropsPacket(drop));
                }
            }
        }

        private void CheckLostDrop(GameMap map, Drop drop)
        {
            if (!drop.Lost && !drop.Thrown && drop.NoOwner)
            {
                var dropViews = new List<long>();
                dropViews.AddRange(map.GetDropViews(drop.Id));

                foreach (var tamer in dropViews)
                {
                    var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == tamer);

                    targetClient?.Send(new UnloadDropsPacket(drop));
                    targetClient?.Send(new LoadDropsPacket(drop, targetClient.Tamer.GeneralHandler));
                }

                drop.SetLost();
            }
        }
    }
}