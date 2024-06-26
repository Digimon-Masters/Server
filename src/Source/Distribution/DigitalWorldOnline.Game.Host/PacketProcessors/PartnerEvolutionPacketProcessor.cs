using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;
using System;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartnerEvolutionPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerEvolution;

        private readonly PartyManager _partyManager;
        private readonly StatusManager _statusManager;
        private readonly AssetsLoader _assets;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ISender _sender;
        private readonly ILogger _logger;

        public PartnerEvolutionPacketProcessor(
            PartyManager partyManager,
            StatusManager statusManager,
            AssetsLoader assets,
            MapServer mapServer,
            ISender sender,
            ILogger logger,
            DungeonsServer dungeonServer)
        {
            _partyManager = partyManager;
            _statusManager = statusManager;
            _assets = assets;
            _mapServer = mapServer;
            _sender = sender;
            _logger = logger;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            if (client.Partner == null)
            {
                client.Send(new DigimonEvolutionFailPacket());
                return;
            }

            var evoLine = _assets.EvolutionInfo
                .FirstOrDefault(x => x.Type == client.Partner.BaseType)?
                .Lines.FirstOrDefault(x => x.Type == client.Partner.CurrentType)?
                .Stages;

            var evoInfo = _assets.EvolutionInfo
                .FirstOrDefault(x => x.Type == client.Partner.BaseType)?
                .Lines.FirstOrDefault(x => x.Type == client.Partner.CurrentType);

            if (evoLine == null || !evoLine.Any())
            {
                client.Send(new DigimonEvolutionFailPacket());
                return;
            }

            var digimonHandle = packet.ReadInt();
            var evoStage = packet.ReadByte();

            //TODO: desbloquear corretamente na criação
            var starterPartners = new List<int>() { 31001, 31002, 31003, 31004 };
            if (!client.Partner.BaseType.IsBetween(starterPartners.ToArray()))
            {
                var targetEvo = client.Partner.Evolutions.FirstOrDefault(x => x.Type == evoLine[evoStage].Type);

                if (targetEvo == null || targetEvo.Unlocked == 0)
                {
                    _logger.Warning($"Character {client.TamerId} tryied to evolve {client.Partner.Id} into {targetEvo?.Type} without unlocking the evo.");
                    client.Send(new DigimonEvolutionFailPacket());
                    return;
                }
            }



            var buffToRemove = client.Tamer.Partner.BuffList.TamerBaseSkill();

            if (buffToRemove != null)
            {
                if (client.DungeonMap)
                {
                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new RemoveBuffPacket(client.Partner.GeneralHandler, buffToRemove.BuffId).Serialize());
                }
                else
                {
                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new RemoveBuffPacket(client.Partner.GeneralHandler, buffToRemove.BuffId).Serialize());

                }
            }

            client.Tamer.RemovePartnerPassiveBuff();
            await _sender.Send(new UpdateDigimonBuffListCommand(client.Partner.BuffList));

            DigimonEvolutionEffectEnum evoEffect;

            if (evoStage == 8)
            {
                evoEffect = DigimonEvolutionEffectEnum.Back;
                client.Tamer.ActiveEvolution.SetDs(0);
                client.Tamer.ActiveEvolution.SetXg(0);
            }
            else
            {
                var evolutionType = _assets.DigimonBaseInfo.First(x => x.Type == evoLine[evoStage].Type).EvolutionType;
                var targetEvo = _assets.DigimonBaseInfo.First(x => x.Type == evoLine[evoStage].Type);
                switch ((EvolutionRankEnum)evolutionType)
                {
                    case EvolutionRankEnum.Rookie:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;
                            client.Tamer.ActiveEvolution.SetDs(0);
                            client.Tamer.ActiveEvolution.SetXg(0);
                        }
                        break;

                    case EvolutionRankEnum.Champion:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;

                            if (client.Partner.Level < evoInfo.UnlockLevel || !client.Tamer.ConsumeDs(20))
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ActiveEvolution.SetDs(8);
                        }
                        break;

                    case EvolutionRankEnum.Ultimate:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;

                            if (client.Partner.Level < evoInfo.UnlockLevel || !client.Tamer.ConsumeDs(50))
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ActiveEvolution.SetDs(10);
                        }
                        break;

                    case EvolutionRankEnum.Mega:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;

                            if (client.Partner.Level < evoInfo.UnlockLevel || !client.Tamer.ConsumeDs(152))
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ActiveEvolution.SetDs(12);
                        }
                        break;

                    case EvolutionRankEnum.BurstMode:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.BurstMode;

                            var accelerator = client.Tamer.Inventory.FindItemById(9400);

                            if (accelerator == null)
                                accelerator = client.Tamer.Inventory.FindItemById(41002);
                            if (evoInfo.RequiredItem > 0)
                            {
                                if (client.Partner.Level < evoInfo.UnlockLevel && !client.Tamer.ConsumeDs(148)
                                    && client.Tamer.Inventory.RemoveOrReduceItem(accelerator, 3))
                                {
                                    client.Send(new DigimonEvolutionFailPacket());
                                    return;
                                }
                            }
                            else
                            {
                                if (client.Partner.Level < evoInfo.UnlockLevel && !client.Tamer.ConsumeDs(148))
                                {
                                    client.Send(new DigimonEvolutionFailPacket());
                                    return;
                                }
                            }

                            client.Tamer.ActiveEvolution.SetDs(40);
                            client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));
                        }
                        break;

                    case EvolutionRankEnum.Jogress:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;

                            if (evoInfo.RequiredItem > 0)
                            {
                                var accelerator = client.Tamer.Inventory.FindItemById(evoInfo.RequiredItem);

                                if (client.Partner.Level < evoInfo.UnlockLevel && !client.Tamer.ConsumeDs(180)
                                    && !client.Tamer.Inventory.RemoveOrReduceItem(accelerator, 1))
                                {
                                    client.Send(new DigimonEvolutionFailPacket());
                                    return;
                                }
                            }
                            else
                            {
                                if (client.Partner.Level < evoInfo.UnlockLevel && !client.Tamer.ConsumeDs(180))
                                {
                                    client.Send(new DigimonEvolutionFailPacket());
                                    return;
                                }

                            }

                            client.Tamer.ActiveEvolution.SetDs(80);
                        }
                        break;

                    case EvolutionRankEnum.RookieX:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;

                            if (client.Partner.Level < evoInfo.UnlockLevel)
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ConsumeXg(68);
                            client.Tamer.ActiveEvolution.SetXg(1);
                        }
                        break;

                    case EvolutionRankEnum.ChampionX:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;

                            if (client.Partner.Level < evoInfo.UnlockLevel)
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ConsumeXg(92);
                            client.Tamer.ActiveEvolution.SetXg(1);
                        }
                        break;

                    case EvolutionRankEnum.UltimateX:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;
                           
                            if (client.Partner.Level < evoInfo.UnlockLevel)
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }
                            client.Tamer.ConsumeXg(130);

                            client.Tamer.ActiveEvolution.SetXg(1);
                        }
                        break;

                    case EvolutionRankEnum.MegaX:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Default;
                            
                          
                            if (client.Partner.Level < evoInfo.UnlockLevel)
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ConsumeXg(174);

                            client.Tamer.ActiveEvolution.SetXg(1);
                        }
                        break;

                    case EvolutionRankEnum.Capsule:
                        {
                            evoEffect = DigimonEvolutionEffectEnum.Unknown;

                            if (client.Partner.Level < evoInfo.UnlockLevel || !client.Tamer.ConsumeDs(75))
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ActiveEvolution.SetDs(3);

                        }
                        break;

                    case EvolutionRankEnum.JogressX:
                    case EvolutionRankEnum.BurstModeX:
                        {
                            //TODO: Teste BM X (custo e efeito)
                            evoEffect = DigimonEvolutionEffectEnum.BurstMode;

                            if (client.Partner.Level < evoInfo.UnlockLevel)
                            {
                                client.Send(new DigimonEvolutionFailPacket());
                                return;
                            }

                            client.Tamer.ConsumeXg(280);
                            client.Tamer.ActiveEvolution.SetXg(1);
                        }
                        break;

                    default:
                        {
                            client.Send(new DigimonEvolutionFailPacket());
                            return;
                        }
                }

                if (client.Tamer.HasXai)
                {

                    client.Send(new XaiInfoPacket(client.Tamer.Xai));
                    client.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge, client.Tamer.XCrystals));
                }
            }

            if (evoStage == 8)
                _logger.Verbose($"Character {client.TamerId} devolved partner {client.Partner.Id} from {client.Partner.CurrentType} to {evoLine[evoStage]?.Type}.");
            else
                _logger.Verbose($"Character {client.TamerId} evolved partner {client.Partner.Id} from {client.Partner.CurrentType} to {evoLine[evoStage]?.Type}.");

            client.Partner.UpdateCurrentType(evoLine[evoStage].Type);


            if (client.DungeonMap)
            {
                if (client.Tamer.Riding)
                {
                    client.Tamer.StopRideMode();

                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new UpdateMovementSpeedPacket(client.Tamer).Serialize());

                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new RideModeStopPacket(client.Tamer.GeneralHandler, client.Partner.GeneralHandler).Serialize());
                }

                _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                    new DigimonEvolutionSucessPacket(
                        client.Tamer.GeneralHandler,
                        client.Partner.GeneralHandler,
                        client.Partner.CurrentType,
                        evoEffect
                    ).Serialize()
                );
            }
            else
            {
                if (client.Tamer.Riding)
                {
                    client.Tamer.StopRideMode();

                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new UpdateMovementSpeedPacket(client.Tamer).Serialize());

                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new RideModeStopPacket(client.Tamer.GeneralHandler, client.Partner.GeneralHandler).Serialize());
                }

                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                    new DigimonEvolutionSucessPacket(
                        client.Tamer.GeneralHandler,
                        client.Partner.GeneralHandler,
                        client.Partner.CurrentType,
                        evoEffect
                    ).Serialize()
                );
            }
            UpdateSkillCooldown(client);



            var currentHp = client.Partner.CurrentHp;
            var currentMaxHp = client.Partner.HP;
            var currentDs = client.Partner.CurrentDs;
            var currentMaxDs = client.Partner.DS;

            client.Tamer.Partner.SetBaseInfo(
                _statusManager.GetDigimonBaseInfo(
                    client.Tamer.Partner.CurrentType
                )
            );

            client.Tamer.Partner.SetBaseStatus(
                _statusManager.GetDigimonBaseStatus(
                    client.Tamer.Partner.CurrentType,
                    client.Tamer.Partner.Level,
                    client.Tamer.Partner.Size
                )
            );

            client.Partner.SetSealStatus(_assets.SealInfo);
            client.Tamer.SetPartnerPassiveBuff();

            if (evoStage != 8)
                client.Partner.FullHeal();
            else
                client.Partner.AdjustHpAndDs(currentHp, currentMaxHp, currentDs, currentMaxDs);


            var currentTitleBuff = _assets.AchievementAssets.FirstOrDefault(x => x.QuestId == client.Tamer.CurrentTitle && x.BuffId > 0);

            if (currentTitleBuff != null)
            {
                foreach (var buff in client.Tamer.Partner.BuffList.ActiveBuffs.Where(x => x.BuffId != currentTitleBuff.BuffId))
                    buff.SetBuffInfo(_assets.BuffInfo.FirstOrDefault(x => x.SkillCode == buff.SkillId && buff.BuffInfo == null || x.DigimonSkillCode == buff.SkillId && buff.BuffInfo == null));

                if (client.Tamer.Partner.BuffList.TamerBaseSkill() != null)
                {
                    var buffToApply = client.Tamer.Partner.BuffList.Buffs
                                .Where(x => x.Duration == 0 && x.BuffId != currentTitleBuff.BuffId)
                                .ToList();


                    buffToApply.ForEach(buffToApply =>
                    {
                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffId, buffToApply.SkillId, (short)buffToApply.TypeN, 0).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffId, buffToApply.SkillId, (short)buffToApply.TypeN, 0).Serialize());
                        }
                    });

                }
            }
            else
            {
                foreach (var buff in client.Tamer.Partner.BuffList.ActiveBuffs)
                    buff.SetBuffInfo(_assets.BuffInfo.FirstOrDefault(x => x.SkillCode == buff.SkillId && buff.BuffInfo == null || x.DigimonSkillCode == buff.SkillId && buff.BuffInfo == null));

                if (client.Tamer.Partner.BuffList.TamerBaseSkill() != null)
                {
                    var buffToApply = client.Tamer.Partner.BuffList.Buffs
                                .Where(x => x.Duration == 0)
                                .ToList();


                    buffToApply.ForEach(buffToApply =>
                    {
                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffId, buffToApply.SkillId, (short)buffToApply.TypeN, 0).Serialize());
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffId, buffToApply.SkillId, (short)buffToApply.TypeN, 0).Serialize());

                        }
                    });


                }
            }

            client.Send(new UpdateStatusPacket(client.Tamer));


            client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

            var party = _partyManager.FindParty(client.TamerId);

            if (party != null)
            {
                party.UpdateMember(party[client.TamerId]);

                _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                    new PartyMemberInfoPacket(party[client.TamerId]).Serialize());

                _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                  new PartyMemberInfoPacket(party[client.TamerId]).Serialize());
            }


            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
            await _sender.Send(new UpdatePartnerCurrentTypeCommand(client.Partner));
            await _sender.Send(new UpdateCharacterActiveEvolutionCommand(client.Tamer.ActiveEvolution));
            await _sender.Send(new UpdateCharacterBasicInfoCommand(client.Tamer));
            await _sender.Send(new UpdateDigimonBuffListCommand(client.Partner.BuffList));
        }

        private void UpdateSkillCooldown(GameClient client)
        {

            if (client.Tamer.Partner.HasActiveSkills())
            {

                foreach (var evolution in client.Tamer.Partner.Evolutions)
                {
                    foreach (var skill in evolution.Skills)
                    {
                        if (skill.Duration > 0 && skill.Expired)
                        {
                            skill.ResetCooldown();
                        }
                    }

                    _sender.Send(new UpdateEvolutionCommand(evolution));
                }

                List<int> SkillIds = new List<int>(5);
                var packetEvolution = client.Tamer.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Tamer.Partner.CurrentType);

                if (packetEvolution != null)
                {

                    var slot = -1;

                    foreach (var item in packetEvolution.Skills)
                    {
                        slot++;

                        var skillInfo = _assets.DigimonSkillInfo.FirstOrDefault(x => x.Type == client.Partner.CurrentType && x.Slot == slot);
                        if (skillInfo != null)
                        {
                            SkillIds.Add(skillInfo.SkillId);
                        }
                    }

                    client?.Send(new SkillUpdateCooldownPacket(client.Tamer.Partner.GeneralHandler, client.Tamer.Partner.CurrentType, packetEvolution, SkillIds));

                }
            }
        }
    }
}