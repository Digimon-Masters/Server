using DigitalWorldOnline.Application;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Models.Summon;
using DigitalWorldOnline.Commons.Utils;
using Serilog;

namespace DigitalWorldOnline.Game.Managers
{
    public class ReceiveExpResult
    {
        public byte LevelGain { get; private set; }
        public bool Success { get; private set; }

        public ReceiveExpResult(byte levelGain, bool success)
        {
            LevelGain = levelGain;
            Success = success;
        }
    }

    public class ExpManager
    {
        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;

        public ExpManager(AssetsLoader assets, ILogger logger)
        {
            _assets = assets;
            _logger = logger;
        }

        public ReceiveExpResult ReceiveMaxTamerExperience(CharacterModel tamer)
        {
            if (tamer.Level >= 120) return new ReceiveExpResult(0, true);

            var tamerInfos = _assets.TamerLevelInfo
                .Where(x => x.Type == tamer.Model)
                .ToList();

            if (tamerInfos == null || !tamerInfos.Any() || tamerInfos.Count != 120)
            {
                _logger.Error($"Incomplete level config for tamer {tamer.Model}.");
                return new ReceiveExpResult(0, false);
            }

            var levelGain = (byte)(120 - tamer.Level);
            tamer.LevelUp(levelGain);
            tamer.SetExp(0);

            _logger.Verbose($"Character {tamer.Id} received max exp.");
            return new ReceiveExpResult(levelGain, true);
        }

        public ReceiveExpResult ReceiveTamerExperience(
            long receivedExp,
            CharacterModel tamer
        )
        {
            if (tamer.Level >= 120) return new ReceiveExpResult(0, true);

            var tamerInfos = _assets.TamerLevelInfo
                .Where(x => x.Type == tamer.Model)
                .ToList();

            if (tamerInfos == null || !tamerInfos.Any() || tamerInfos.Count != 120)
            {
                _logger.Error($"Incomplete level config for tamer {tamer.Model}.");
                return new ReceiveExpResult(0, false);
            }

            if (receivedExp < 0) receivedExp = 0;

            var currentLevel = tamerInfos.First(x => x.Level == tamer.Level);
            var expToGain = receivedExp;

            byte levelGain = 0;

            while (expToGain > 0)
            {
                if (expToGain + tamer.CurrentExperience >= currentLevel.ExpValue)
                {
                    var expToReceive = currentLevel.ExpValue - tamer.CurrentExperience;
                    tamer.ReceiveExp(expToReceive);
                    expToGain -= expToReceive;
                    levelGain++;
                    tamer.LevelUp();
                    currentLevel = tamerInfos.First(x => x.Level == tamer.Level);

                    if (tamer.Level == 120)
                    {
                        tamer.SetExp(0);
                        expToGain = 0;
                    }
                }
                else
                {
                    tamer.ReceiveExp(expToGain);
                    expToGain = 0;
                }
            }

            if(levelGain > 0)
                _logger.Verbose($"Character {tamer.Id} received {receivedExp} exp and gained {levelGain} levels.");
            else
                _logger.Verbose($"Character {tamer.Id} received {receivedExp} exp.");

            return new ReceiveExpResult(levelGain, true);
        }

        public ReceiveExpResult ReceiveDigimonExperience(
            long receivedExp,
            DigimonModel digimon
        )
        {
            if (digimon.Level >= 120) return new ReceiveExpResult(0, true);

            var digimonInfos = _assets.DigimonLevelInfo
                .Where(x => x.ScaleType == digimon.BaseInfo.ScaleType)
                .ToList();

            if (digimonInfos == null || !digimonInfos.Any() || digimonInfos.Count != 120)
            {
                _logger.Error($"Incomplete level config for digimon {digimon.Model} {digimon.BaseInfo.ScaleType}.");
                return new ReceiveExpResult(0, false);
            }

            if (receivedExp < 0) receivedExp = 0;

            var currentLevel = digimonInfos.First(x => x.Level == digimon.Level);
            var expToGain = receivedExp;

            byte levelGain = 0;

            while (expToGain > 0)
            {
                if (expToGain + digimon.CurrentExperience >= currentLevel.ExpValue)
                {
                    var expToReceive = currentLevel.ExpValue - digimon.CurrentExperience;
                    digimon.ReceiveExp(expToReceive);
                    expToGain -= expToReceive;
                    levelGain++;
                    digimon.LevelUp();
                    currentLevel = digimonInfos.First(x => x.Level == digimon.Level);

                    if (digimon.Level == 120)
                    {
                        digimon.SetExp(0);
                        expToGain = 0;
                    }
                }
                else
                {
                    digimon.ReceiveExp(expToGain);
                    expToGain = 0;
                }
            }

            if (levelGain > 0)
                _logger.Verbose($"Partner {digimon.Id} received {receivedExp} exp and gained {levelGain} levels.");
            else
                _logger.Verbose($"Partner {digimon.Id} received {receivedExp} exp.");

            return new ReceiveExpResult(levelGain, true);
        }

        public ReceiveExpResult ReceiveMaxDigimonExperience(DigimonModel digimon)
        {
            if (digimon.Level >= 120) return new ReceiveExpResult(0, true);

            var digimonInfos = _assets.DigimonLevelInfo
                .Where(x => x.ScaleType == digimon.BaseInfo.ScaleType)
                .ToList();

            if (digimonInfos == null || !digimonInfos.Any() || digimonInfos.Count != 120)
            {
                _logger.Error($"Incomplete level config for digimon {digimon.Model} {digimon.BaseInfo.ScaleType}.");
                return new ReceiveExpResult(0, false);
            }

            var levelGain = (byte)(120 - digimon.Level);

            digimon.LevelUp(levelGain);
            digimon.SetExp(0);

            _logger.Verbose($"Partner {digimon.Id} received max exp.");
            return new ReceiveExpResult(levelGain, true);
        }

        internal void ReceiveAttributeExperience(
            DigimonModel partner,
            DigimonAttributeEnum targetAttribute,
            DigimonElementEnum targetElement,
            MobExpRewardConfigModel expReward)
        {
            if (partner.BaseInfo.Attribute.HasAttributeAdvantage(targetAttribute))
            {
                _logger.Verbose($"Partner {partner.Id} received {expReward.NatureExperience} nature exp.");
                partner.ReceiveNatureExp(expReward.NatureExperience);
            }

            if (partner.BaseInfo.Element.HasElementAdvantage(targetElement))
            {
                _logger.Verbose($"Partner {partner.Id} received {expReward.ElementExperience} element exp.");
                partner.ReceiveElementExp(expReward.ElementExperience);
            }
            else if (targetElement.HasElementAdvantage(partner.BaseInfo.Element))
            {
                _logger.Verbose($"Partner {partner.Id} lost 25 element exp.");
                partner.ReceiveElementExp(-25);
            }
            else if (partner.BaseInfo.Element == targetElement)
            {
                _logger.Verbose($"Partner {partner.Id} received {expReward.ElementExperience / 2} element exp.");
                partner.ReceiveElementExp((short)(expReward.ElementExperience / 2));
            }
        }
        internal void ReceiveAttributeExperience(
           DigimonModel partner,
           DigimonAttributeEnum targetAttribute,
           DigimonElementEnum targetElement,
           SummonMobExpRewardModel expReward)
        {
            if (partner.BaseInfo.Attribute.HasAttributeAdvantage(targetAttribute))
            {
                _logger.Verbose($"Partner {partner.Id} received {expReward.NatureExperience} nature exp.");
                partner.ReceiveNatureExp(expReward.NatureExperience);
            }

            if (partner.BaseInfo.Element.HasElementAdvantage(targetElement))
            {
                _logger.Verbose($"Partner {partner.Id} received {expReward.ElementExperience} element exp.");
                partner.ReceiveElementExp(expReward.ElementExperience);
            }
            else if (targetElement.HasElementAdvantage(partner.BaseInfo.Element))
            {
                _logger.Verbose($"Partner {partner.Id} lost 25 element exp.");
                partner.ReceiveElementExp(-25);
            }
            else if (partner.BaseInfo.Element == targetElement)
            {
                _logger.Verbose($"Partner {partner.Id} received {expReward.ElementExperience / 2} element exp.");
                partner.ReceiveElementExp((short)(expReward.ElementExperience / 2));
            }
        }
    }
}
