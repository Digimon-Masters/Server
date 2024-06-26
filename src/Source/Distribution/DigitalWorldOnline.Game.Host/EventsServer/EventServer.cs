using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Game.Managers;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.GameHost.EventsServer
{
    public sealed partial class EventServer
    {
        private readonly EventQueueManager _eventQueueManager;
        private readonly StatusManager _statusManager;
        private readonly ExpManager _expManager;
        private readonly DropManager _dropManager;
        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public List<GameMap> Maps { get; set; }

        public int MobAmount { get; }
        public int DropAmount { get; }

        private List<Point> _randomPoints = new();

        public EventServer(
            EventQueueManager eventQueueManager,
            AssetsLoader assets,
            StatusManager statusManager,
            ExpManager expManager,
            DropManager dropManager,
            ILogger logger,
            ISender sender,
            IMapper mapper)
        {
            _eventQueueManager = eventQueueManager;
            _statusManager = statusManager;
            _expManager = expManager;
            _dropManager = dropManager;
            _assets = assets.Load();
            _logger = logger;
            _sender = sender;
            _mapper = mapper;

            MobAmount = 100;
            DropAmount = 50;

            _randomPoints = GenerateRandomPoints(MobAmount + DropAmount, 17662, 71080, 13055, 79041, 1000);

            AddContent();
        }

        /*
        1 - 15819 20253
        2 - 21809 12602
        3 - 28231 13720
        4 - 32128 23940
        5 - 40158 24186
        6 - 43741 30970
        7 - 39892 39970
        8 - 25502 41207
        9 - 16880 46273
        10 - 17478 55724
        11 - 20586 66258
        12 - 33182 60792
        13 - 40993 50675
        14 - 52773 55483
        15 - 57050 70033
        16 - 66715 76088
        */

        private void AddContent()
        {
            Maps = new List<GameMap>()
            {
                new GameMap(9001, AddMobs(), AddDrops()),
                new GameMap(9002, AddBoss(), new List<Drop>())
            };
        }

        private List<MobConfigModel> AddMobs()
        {
            var mobs = new List<MobConfigModel>();
            return mobs;
        }

        private List<Drop> AddDrops()
        {
            return new List<Drop>();
        }

        private List<MobConfigModel> AddBoss()
        {
            //11940
            //15213

            return new List<MobConfigModel>();
        }

        private static List<Point> GenerateRandomPoints(int numberOfPoints, int minX, int maxX, int minY, int maxY, int minDistance)
        {
            var points = new List<Point>();
            Random random = new Random();

            while (points.Count < numberOfPoints)
            {
                int x = random.Next(minX, maxX + 1);
                int y = random.Next(minY, maxY + 1);

                bool valid = true;
                foreach (Point existingPoint in points)
                {
                    int distanceSquared = (x - existingPoint.X) * (x - existingPoint.X) + (y - existingPoint.Y) * (y - existingPoint.Y);
                    if (distanceSquared < minDistance * minDistance)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    points.Add(new Point(x, y));
                }
            }

            return points;
        }
    }

    class Point
    {
        public bool Free { get; private set; }
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
            Free = true;
        }

        public void UsePoint() => Free = false;
    }
}