using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Game.Managers;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.GameHost
{
    public sealed partial class PvpServer
    {
        private readonly StatusManager _statusManager;
        private readonly AssetsLoader _assets;
        private readonly ConfigsLoader _configs;
        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public List<GameMap> Maps { get; set; }

        public PvpServer(
            AssetsLoader assets,
            ConfigsLoader configs,
            StatusManager statusManager,
            ILogger logger,
            ISender sender,
            IMapper mapper)
        {
            _statusManager = statusManager;
            _assets = assets.Load();
            _configs = configs.Load();
            _logger = logger;
            _sender = sender;
            _mapper = mapper;

            Maps = new List<GameMap>();
        }
    }
}