using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Asset;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Models.Summon;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;
using static DigitalWorldOnline.Commons.Packets.GameServer.AddBuffPacket;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public partial class PartnerSkillPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerSkill;

        private readonly AssetsLoader _assets;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public PartnerSkillPacketProcessor(

            AssetsLoader assets,
            MapServer mapServer,
            ILogger logger, ISender sender, DungeonsServer dungeonServer)
        {
            _assets = assets;
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;
            _dungeonServer = dungeonServer;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var skillSlot = packet.ReadByte();
            var attackerHandler = packet.ReadInt();
            var targetHandler = packet.ReadInt();

            if (client.Partner == null)
                return Task.CompletedTask;

            var skill = _assets.DigimonSkillInfo.FirstOrDefault(x => x.Type == client.Partner.CurrentType && x.Slot == skillSlot);
            if (skill == null || skill.SkillInfo == null)
                return Task.CompletedTask;

            var targetSummonMobs = new List<SummonMobModel>();
            SkillTypeEnum skillType;
            if (client.DungeonMap)
            {
                if (_dungeonServer.GetMobByHandler(client.Tamer.Location.MapId, targetHandler, true, client.TamerId) != null)
                {

                    if (skill.SkillInfo.AreaOfEffect > 0)
                    {
                        skillType = SkillTypeEnum.Implosion;

                        var targets = _dungeonServer.GetMobsNearbyPartner(client.Partner.Location, skill.SkillInfo.AreaOfEffect, true, client.TamerId);

                        targetSummonMobs.AddRange(targets);
                    }
                    else if (skill.SkillInfo.AoEMaxDamage > 0)
                    {
                        skillType = SkillTypeEnum.TargetArea;

                        var targets = _dungeonServer.GetMobsNearbyTargetMob(client.Tamer.Location.MapId, targetHandler, skill.SkillInfo.Range / 10, true, client.TamerId);

                        targetSummonMobs.AddRange(targets);
                    }
                    else
                    {
                        skillType = SkillTypeEnum.Single;

                        var mob = _dungeonServer.GetMobByHandler(client.Tamer.Location.MapId, targetHandler, true, client.TamerId);

                        if (mob == null)
                            return Task.CompletedTask;

                        targetSummonMobs.Add(mob);
                    }

                    if (targetSummonMobs.Any())
                    {
                        if (skillType == SkillTypeEnum.Single && !targetSummonMobs.First().Alive)
                            return Task.CompletedTask;

                        client.Partner.ReceiveDamage(skill.SkillInfo.HPUsage);
                        client.Partner.UseDs(skill.SkillInfo.DSUsage);

                        var castingTime = (int)Math.Round((float)0);
                        if (castingTime <= 0) castingTime = 2000;

                        client.Partner.SetEndCasting(0);

                        targetSummonMobs.ForEach(targetMob =>
                        {
                            _logger.Verbose($"Character {client.Tamer.Id} engaged {targetMob.Id} - {targetMob.Name} with skill {skill.SkillId}.");
                        });

                        if (!client.Tamer.InBattle)
                        {
                            client.Tamer.SetHidden(false);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(attackerHandler).Serialize());
                            client.Tamer.StartBattleWithSkill(targetSummonMobs, skillType);
                        }
                        else
                        {
                            client.Tamer.SetHidden(false);
                            client.Tamer.UpdateTargetWithSkill(targetSummonMobs, skillType);
                        }

                        if (skillType != SkillTypeEnum.Single)
                        {
                            var finalDmg = 0;

                            if (!client.Tamer.GodMode)
                            {
                                //TODO: regra de 3 para redução de dano conforme distancia do ponto de origem
                                if (skill.SkillInfo.AoEMinDamage > 0 && skill.SkillInfo.AoEMaxDamage > 0)
                                    finalDmg = UtilitiesFunctions.RandomInt(skill.SkillInfo.AoEMinDamage, skill.SkillInfo.AoEMaxDamage);
                                else
                                    finalDmg = client.Partner.AT + UtilitiesFunctions.RandomInt(2300, 3500);
                            }
                            else
                                finalDmg = int.MaxValue;

                            targetSummonMobs.ForEach(targetMob =>
                            {
                                if (finalDmg <= 0) finalDmg = 1;
                                if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                                if (!targetMob.InBattle)
                                {
                                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                    targetMob.StartBattle(client.Tamer);
                                }
                                else
                                {
                                    targetMob.AddTarget(client.Tamer);
                                }

                                var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);
                                if (newHp > 0)
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");
                                }
                                else
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");
                                    targetMob?.Die();
                                }
                            });

                            _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new CastSkillPacket(
                                    skillSlot,
                                    attackerHandler,
                                    targetHandler
                                ).Serialize()
                            );

                            _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new AreaSkillPacket(
                                    attackerHandler,
                                    client.Partner.HpRate,
                                    targetSummonMobs,
                                    skillSlot,
                                    finalDmg
                                ).Serialize()
                            );
                        }
                        else
                        {


                            var targetMob = targetSummonMobs.First();

                            if (!targetMob.InBattle)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                targetMob.StartBattle(client.Tamer);
                            }
                            else
                            {
                                targetMob.AddTarget(client.Tamer);
                            }

                            var finalDmg = client.Tamer.GodMode ? targetMob.CurrentHP : CalculateDamageOrHeal(client, targetMob, skill, _assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId), skillSlot);

                            if (finalDmg <= 0) finalDmg = 1;
                            if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                            var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);

                            if (newHp > 0)
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");

                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new CastSkillPacket(
                                        skillSlot,
                                        attackerHandler,
                                        targetHandler).Serialize());

                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new SkillHitPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg,
                                        targetMob.CurrentHpRate
                                        ).Serialize());
                            }
                            else
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");

                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new KillOnSkillPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg
                                        ).Serialize());

                                targetMob?.Die();
                            }
                        }

                        if (!_dungeonServer.MobsAttacking(client.Tamer.Location.MapId, client.TamerId, true))
                        {
                            client.Tamer.StopBattle(true);

                            SendBattleOffTask(client, attackerHandler, true);
                        }

                        var evolution = client.Tamer.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Tamer.Partner.CurrentType);

                        if (evolution != null && skill.SkillInfo.Cooldown / 1000 > 59)
                        {
                            evolution.Skills[skillSlot].SetCooldown(skill.SkillInfo.Cooldown / 1000);
                            _sender.Send(new UpdateEvolutionCommand(evolution));
                        }
                    }
                }
                else
                {

                    var targetMobs = new List<MobConfigModel>();



                    if (skill.SkillInfo.AreaOfEffect > 0)
                    {
                        skillType = SkillTypeEnum.Implosion;

                        var targets = _dungeonServer.GetMobsNearbyPartner(client.Partner.Location, skill.SkillInfo.AreaOfEffect, client.TamerId);

                        targetMobs.AddRange(targets);
                    }
                    else if (skill.SkillInfo.AoEMaxDamage > 0)
                    {
                        skillType = SkillTypeEnum.TargetArea;

                        var targets = _dungeonServer.GetMobsNearbyTargetMob(client.Tamer.Location.MapId, targetHandler, skill.SkillInfo.Range / 10, client.TamerId);

                        targetMobs.AddRange(targets);
                    }
                    else
                    {
                        skillType = SkillTypeEnum.Single;

                        var mob = _dungeonServer.GetMobByHandler(client.Tamer.Location.MapId, targetHandler, client.TamerId);
                        if (mob == null)
                            return Task.CompletedTask;

                        targetMobs.Add(mob);
                    }

                    if (targetMobs.Any())
                    {
                        if (skillType == SkillTypeEnum.Single && !targetMobs.First().Alive)
                            return Task.CompletedTask;

                        client.Partner.ReceiveDamage(skill.SkillInfo.HPUsage);
                        client.Partner.UseDs(skill.SkillInfo.DSUsage);

                        var castingTime = (int)Math.Round((float)0);
                        if (castingTime <= 0) castingTime = 2000;

                        client.Partner.SetEndCasting(0);

                        targetMobs.ForEach(targetMob =>
                        {
                            _logger.Verbose($"Character {client.Tamer.Id} engaged {targetMob.Id} - {targetMob.Name} with skill {skill.SkillId}.");
                        });

                        if (!client.Tamer.InBattle)
                        {
                            client.Tamer.SetHidden(false);
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(attackerHandler).Serialize());
                            client.Tamer.StartBattleWithSkill(targetMobs, skillType);
                        }
                        else
                        {
                            client.Tamer.SetHidden(false);
                            client.Tamer.UpdateTargetWithSkill(targetMobs, skillType);
                        }

                        if (skillType != SkillTypeEnum.Single)
                        {
                            var finalDmg = 0;

                            if (!client.Tamer.GodMode)
                            {
                                //TODO: regra de 3 para redução de dano conforme distancia do ponto de origem
                                finalDmg = CalculateDamageOrHeal(client, targetMobs.First(), skill, _assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId), skillSlot);

                                if (finalDmg != 0)
                                {
                                    finalDmg = DebuffReductionDamage(client, finalDmg);
                                }
                            }
                            else
                                finalDmg = int.MaxValue;

                            targetMobs.ForEach(targetMob =>
                            {
                                if (finalDmg <= 0) finalDmg = 1;
                                if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                                if (!targetMob.InBattle)
                                {
                                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                    targetMob.StartBattle(client.Tamer);
                                }
                                else
                                {
                                    targetMob.AddTarget(client.Tamer);
                                }

                                var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);
                                if (newHp > 0)
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");
                                }
                                else
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");
                                    targetMob?.Die();
                                }
                            });

                            _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new CastSkillPacket(
                                    skillSlot,
                                    attackerHandler,
                                    targetHandler
                                ).Serialize()
                            );

                            _dungeonServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new AreaSkillPacket(
                                    attackerHandler,
                                    client.Partner.HpRate,
                                    targetMobs,
                                    skillSlot,
                                    finalDmg
                                ).Serialize()
                            );
                        }
                        else
                        {
                            var targetMob = targetMobs.First();

                            if (!targetMob.InBattle)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                targetMob.StartBattle(client.Tamer);
                            }
                            else
                            {
                                targetMob.AddTarget(client.Tamer);
                            }

                            var finalDmg = client.Tamer.GodMode ? targetMob.CurrentHP : CalculateDamageOrHeal(client, targetMob, skill, _assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId), skillSlot);
                            
                            if (finalDmg != 0 && !client.Tamer.GodMode)
                            {
                                finalDmg = DebuffReductionDamage(client, finalDmg);
                            }

                            if (finalDmg <= 0) finalDmg = 1;
                            if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                            var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);

                            if (newHp > 0)
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");

                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new CastSkillPacket(
                                        skillSlot,
                                        attackerHandler,
                                        targetHandler).Serialize());

                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new SkillHitPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg,
                                        targetMob.CurrentHpRate
                                        ).Serialize());

                                
                            }
                            else
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");

                                _dungeonServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new KillOnSkillPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg
                                        ).Serialize());

                                targetMob?.Die();
                            }
                        }

                        if (!_dungeonServer.MobsAttacking(client.Tamer.Location.MapId, client.TamerId))
                        {
                            client.Tamer.StopBattle();

                            SendBattleOffTask(client, attackerHandler, true);
                        }

                        var evolution = client.Tamer.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Tamer.Partner.CurrentType);

                        if (evolution != null && skill.SkillInfo.Cooldown / 1000 > 59)
                        {
                            evolution.Skills[skillSlot].SetCooldown(skill.SkillInfo.Cooldown / 1000);
                            _sender.Send(new UpdateEvolutionCommand(evolution));
                        }
                    }
                }
            }
            else
            {
                if (_mapServer.GetMobByHandler(client.Tamer.Location.MapId, targetHandler, true) != null)
                {

                    if (skill.SkillInfo.AreaOfEffect > 0)
                    {
                        skillType = SkillTypeEnum.Implosion;

                        var targets = _mapServer.GetMobsNearbyPartner(client.Partner.Location, skill.SkillInfo.AreaOfEffect, true);

                        targetSummonMobs.AddRange(targets);
                    }
                    else if (skill.SkillInfo.AoEMaxDamage > 0)
                    {
                        skillType = SkillTypeEnum.TargetArea;

                        var targets = _mapServer.GetMobsNearbyTargetMob(client.Tamer.Location.MapId, targetHandler, skill.SkillInfo.Range / 10, true);

                        targetSummonMobs.AddRange(targets);
                    }
                    else
                    {
                        skillType = SkillTypeEnum.Single;

                        var mob = _mapServer.GetMobByHandler(client.Tamer.Location.MapId, targetHandler, true);
                        if (mob == null)
                            return Task.CompletedTask;

                        targetSummonMobs.Add(mob);
                    }

                    if (targetSummonMobs.Any())
                    {
                        if (skillType == SkillTypeEnum.Single && !targetSummonMobs.First().Alive)
                            return Task.CompletedTask;

                        client.Partner.ReceiveDamage(skill.SkillInfo.HPUsage);
                        client.Partner.UseDs(skill.SkillInfo.DSUsage);

                        var castingTime = (int)Math.Round((float)0);
                        if (castingTime <= 0) castingTime = 2000;

                        client.Partner.SetEndCasting(0);

                        targetSummonMobs.ForEach(targetMob =>
                        {
                            _logger.Verbose($"Character {client.Tamer.Id} engaged {targetMob.Id} - {targetMob.Name} with skill {skill.SkillId}.");
                        });

                        if (!client.Tamer.InBattle)
                        {
                            client.Tamer.SetHidden(false);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(attackerHandler).Serialize());
                            client.Tamer.StartBattleWithSkill(targetSummonMobs, skillType);
                        }
                        else
                        {
                            client.Tamer.SetHidden(false);
                            client.Tamer.UpdateTargetWithSkill(targetSummonMobs, skillType);
                        }

                        if (skillType != SkillTypeEnum.Single)
                        {
                            var finalDmg = 0;

                            if (!client.Tamer.GodMode)
                            {
                                //TODO: regra de 3 para redução de dano conforme distancia do ponto de origem

                                finalDmg = CalculateDamageOrHeal(client, targetSummonMobs.First(), skill, _assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId), skillSlot);

                                if (finalDmg != 0 && !client.Tamer.GodMode)
                                {
                                    finalDmg = DebuffReductionDamage(client, finalDmg);
                                }
                            }
                            else
                                finalDmg = int.MaxValue;

                            targetSummonMobs.ForEach(targetMob =>
                            {
                                if (finalDmg <= 0) finalDmg = 1;
                                if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                                if (!targetMob.InBattle)
                                {
                                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                    targetMob.StartBattle(client.Tamer);
                                }
                                else
                                {
                                    targetMob.AddTarget(client.Tamer);
                                }

                                var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);
                                if (newHp > 0)
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");
                                }
                                else
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");
                                    targetMob?.Die();
                                }
                            });

                            _mapServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new CastSkillPacket(
                                    skillSlot,
                                    attackerHandler,
                                    targetHandler
                                ).Serialize()
                            );

                            _mapServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new AreaSkillPacket(
                                    attackerHandler,
                                    client.Partner.HpRate,
                                    targetSummonMobs,
                                    skillSlot,
                                    finalDmg
                                ).Serialize()
                            );
                        }
                        else
                        {


                            var targetMob = targetSummonMobs.First();

                            if (!targetMob.InBattle)
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                targetMob.StartBattle(client.Tamer);
                            }
                            else
                            {
                                targetMob.AddTarget(client.Tamer);
                            }

                            var finalDmg = client.Tamer.GodMode ? targetMob.CurrentHP : CalculateDamageOrHeal(client, targetMob, skill, _assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId), skillSlot);

                            if (finalDmg != 0 && !client.Tamer.GodMode)
                            {
                                finalDmg = DebuffReductionDamage(client, finalDmg);
                            }

                            if (finalDmg <= 0) finalDmg = 1;
                            if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                            var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);

                            if (newHp > 0)
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new CastSkillPacket(
                                        skillSlot,
                                        attackerHandler,
                                        targetHandler).Serialize());

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new SkillHitPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg,
                                        targetMob.CurrentHpRate
                                        ).Serialize());

                            }
                            else
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new KillOnSkillPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg
                                        ).Serialize());

                                targetMob?.Die();
                            }
                        }

                        if (!_mapServer.MobsAttacking(client.Tamer.Location.MapId, client.TamerId, true))
                        {
                            client.Tamer.StopBattle(true);

                            SendBattleOffTask(client, attackerHandler);
                        }

                        var evolution = client.Tamer.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Tamer.Partner.CurrentType);

                        if (evolution != null && skill.SkillInfo.Cooldown / 1000 > 59)
                        {
                            evolution.Skills[skillSlot].SetCooldown(skill.SkillInfo.Cooldown / 1000);
                            _sender.Send(new UpdateEvolutionCommand(evolution));
                        }
                    }
                }
                else
                {

                    var targetMobs = new List<MobConfigModel>();



                    if (skill.SkillInfo.AreaOfEffect > 0)
                    {
                        skillType = SkillTypeEnum.Implosion;

                        var targets = _mapServer.GetMobsNearbyPartner(client.Partner.Location, skill.SkillInfo.AreaOfEffect);

                        targetMobs.AddRange(targets);
                    }
                    else if (skill.SkillInfo.AoEMaxDamage > 0)
                    {
                        skillType = SkillTypeEnum.TargetArea;

                        var targets = _mapServer.GetMobsNearbyTargetMob(client.Tamer.Location.MapId, targetHandler, skill.SkillInfo.Range / 10);

                        targetMobs.AddRange(targets);
                    }
                    else
                    {
                        skillType = SkillTypeEnum.Single;

                        var mob = _mapServer.GetMobByHandler(client.Tamer.Location.MapId, targetHandler);
                        if (mob == null)
                            return Task.CompletedTask;

                        targetMobs.Add(mob);
                    }

                    if (targetMobs.Any())
                    {
                        if (skillType == SkillTypeEnum.Single && !targetMobs.First().Alive)
                            return Task.CompletedTask;

                        client.Partner.ReceiveDamage(skill.SkillInfo.HPUsage);
                        client.Partner.UseDs(skill.SkillInfo.DSUsage);

                        var castingTime = (int)Math.Round((float)0);
                        if (castingTime <= 0) castingTime = 2000;

                        client.Partner.SetEndCasting(0);

                        targetMobs.ForEach(targetMob =>
                        {
                            _logger.Verbose($"Character {client.Tamer.Id} engaged {targetMob.Id} - {targetMob.Name} with skill {skill.SkillId}.");
                        });

                        if (!client.Tamer.InBattle)
                        {
                            client.Tamer.SetHidden(false);
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(attackerHandler).Serialize());
                            client.Tamer.StartBattleWithSkill(targetMobs, skillType);
                        }
                        else
                        {
                            client.Tamer.SetHidden(false);
                            client.Tamer.UpdateTargetWithSkill(targetMobs, skillType);
                        }

                        if (skillType != SkillTypeEnum.Single)
                        {
                            var finalDmg = 0;

                            if (!client.Tamer.GodMode)
                            {
                                //TODO: regra de 3 para redução de dano conforme distancia do ponto de origem
                                if (skill.SkillInfo.AoEMinDamage > 0 && skill.SkillInfo.AoEMaxDamage > 0)
                                    finalDmg = UtilitiesFunctions.RandomInt(skill.SkillInfo.AoEMinDamage, skill.SkillInfo.AoEMaxDamage);
                                else
                                    finalDmg = client.Partner.AT + UtilitiesFunctions.RandomInt(2300, 3500);
                            }
                            else
                                finalDmg = int.MaxValue;

                            targetMobs.ForEach(targetMob =>
                            {
                                if (finalDmg <= 0) finalDmg = 1;
                                if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                                if (!targetMob.InBattle)
                                {
                                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                    targetMob.StartBattle(client.Tamer);
                                }
                                else
                                {
                                    targetMob.AddTarget(client.Tamer);
                                }

                                var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);
                                if (newHp > 0)
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");
                                }
                                else
                                {
                                    _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");
                                    targetMob?.Die();
                                }
                            });

                            _mapServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new CastSkillPacket(
                                    skillSlot,
                                    attackerHandler,
                                    targetHandler
                                ).Serialize()
                            );

                            _mapServer.BroadcastForTamerViewsAndSelf(
                                client.TamerId,
                                new AreaSkillPacket(
                                    attackerHandler,
                                    client.Partner.HpRate,
                                    targetMobs,
                                    skillSlot,
                                    finalDmg
                                ).Serialize()
                            );
                        }
                        else
                        {
                            var targetMob = targetMobs.First();

                            if (!targetMob.InBattle)
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SetCombatOnPacket(targetHandler).Serialize());
                                targetMob.StartBattle(client.Tamer);
                            }
                            else
                            {
                                targetMob.AddTarget(client.Tamer);
                            }

                            var finalDmg = client.Tamer.GodMode ? targetMob.CurrentHP : CalculateDamageOrHeal(client, targetMob, skill, _assets.SkillCodeInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId), skillSlot);

                            if (finalDmg != 0 && !client.Tamer.GodMode)
                            {
                                finalDmg = DebuffReductionDamage(client, finalDmg);
                            }

                            if (finalDmg <= 0) finalDmg = 1;
                            if (finalDmg > targetMob.CurrentHP) finalDmg = targetMob.CurrentHP;

                            var newHp = targetMob.ReceiveDamage(finalDmg, client.TamerId);

                            if (newHp > 0)
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} inflicted {finalDmg} damage with skill {skill.SkillId} in mob {targetMob?.Id} - {targetMob?.Name}.");

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new CastSkillPacket(
                                        skillSlot,
                                        attackerHandler,
                                        targetHandler).Serialize());

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new SkillHitPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg,
                                        targetMob.CurrentHpRate
                                        ).Serialize());

                                var buffInfo = _assets.BuffInfo.FirstOrDefault(x => x.SkillCode == skill.SkillId || x.DigimonSkillCode == skill.SkillId);

                                if (buffInfo != null)
                                {
                                    foreach (var type in buffInfo.SkillInfo.Apply)
                                    {
                                        switch (type.Attribute)
                                        {
                                            case SkillCodeApplyAttributeEnum.CrowdControl:
                                                {
                                                    var rand = new Random();

                                                    if (UtilitiesFunctions.IncreasePerLevelStun.Contains(skill.SkillId))
                                                    {
                                                      
                                                       

                                                        if (type.Chance >= rand.Next(100))
                                                        {
                                                            var duration = UtilitiesFunctions.RemainingTimeSeconds((1 * client.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Partner.CurrentType).Skills[skillSlot].CurrentLevel));

                                                            var newMobDebuff = MobDebuffModel.Create(buffInfo.BuffId, skill.SkillId, 0, (1 * client.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Partner.CurrentType).Skills[skillSlot].CurrentLevel));
                                                            newMobDebuff.SetBuffInfo(buffInfo);

                                                            var activeBuff = targetMob.DebuffList.Buffs.FirstOrDefault(x => x.BuffId == buffInfo.BuffId);

                                                            if (activeBuff != null)
                                                            {
                                                                activeBuff.IncreaseEndDate((1 * client.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Partner.CurrentType).Skills[skillSlot].CurrentLevel));
                                                            }
                                                            else
                                                            {
                                                                targetMob.DebuffList.Buffs.Add(newMobDebuff);
                                                            }                                                 

                                                           if(targetMob.CurrentAction != Commons.Enums.Map.MobActionEnum.CrowdControl)
                                                            {
                                                                targetMob.UpdateCurrentAction(Commons.Enums.Map.MobActionEnum.CrowdControl);
                                                            }

                                                            Thread.Sleep(100);

                                                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new AddStunDebuffPacket(targetMob.GeneralHandler, newMobDebuff.BuffId, newMobDebuff.SkillId, duration).Serialize());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (type.Chance >= rand.Next(100))
                                                        {
                                                            var duration = UtilitiesFunctions.RemainingTimeSeconds(skill.TimeForCrowdControl());

                                                            var newMobDebuff = MobDebuffModel.Create(buffInfo.BuffId, skill.SkillId, 0, skill.TimeForCrowdControl());
                                                            newMobDebuff.SetBuffInfo(buffInfo);

                                                            var activeBuff = targetMob.DebuffList.Buffs.FirstOrDefault(x => x.BuffId == buffInfo.BuffId);

                                                            if (activeBuff != null)
                                                            {
                                                                activeBuff.IncreaseEndDate(skill.TimeForCrowdControl());
                                                            }
                                                            else
                                                            {
                                                                targetMob.DebuffList.Buffs.Add(newMobDebuff);
                                                            }

                                                            if (targetMob.CurrentAction != Commons.Enums.Map.MobActionEnum.CrowdControl)
                                                            {
                                                                targetMob.UpdateCurrentAction(Commons.Enums.Map.MobActionEnum.CrowdControl);
                                                            }

                                                            Thread.Sleep(100);

                                                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new AddStunDebuffPacket(targetMob.GeneralHandler, newMobDebuff.BuffId, newMobDebuff.SkillId, duration).Serialize());
                                                        }

                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                _logger.Verbose($"Partner {client.Partner.Id} killed mob {targetMob?.Id} - {targetMob?.Name} with {finalDmg} skill {skill.Id} damage.");

                                _mapServer.BroadcastForTamerViewsAndSelf(
                                    client.TamerId,
                                    new KillOnSkillPacket(
                                        attackerHandler,
                                        targetMob.GeneralHandler,
                                        skillSlot,
                                        finalDmg
                                        ).Serialize());

                                targetMob?.Die();
                            }
                        }

                        if (!_mapServer.MobsAttacking(client.Tamer.Location.MapId, client.TamerId))
                        {
                            client.Tamer.StopBattle();

                            SendBattleOffTask(client, attackerHandler);
                        }

                        var evolution = client.Tamer.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Tamer.Partner.CurrentType);

                        if (evolution != null && skill.SkillInfo.Cooldown / 1000 > 59)
                        {
                            evolution.Skills[skillSlot].SetCooldown(skill.SkillInfo.Cooldown / 1000);
                            _sender.Send(new UpdateEvolutionCommand(evolution));
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        public async Task SendBattleOffTask(GameClient client, int attackerHandler)
        {
            await Task.Run(async () =>
            {
                Thread.Sleep(4000);

                _mapServer.BroadcastForTamerViewsAndSelf(
                        client.TamerId,
                        new SetCombatOffPacket(attackerHandler).Serialize()
                    );
            });
        }
        public async Task SendBattleOffTask(GameClient client, int attackerHandler, bool dungeon)
        {
            await Task.Run(async () =>
            {
                Thread.Sleep(4000);

                _dungeonServer.BroadcastForTamerViewsAndSelf(
                        client.TamerId,
                        new SetCombatOffPacket(attackerHandler).Serialize()
                    );
            });
        }

        private static int DebuffReductionDamage(GameClient client, int finalDmg)
        {
            if (client.Tamer.Partner.DebuffList.ActiveDebuffReductionDamage())
            {
                var debuffInfo = client.Tamer.Partner.DebuffList.ActiveBuffs
                .Where(buff => buff.BuffInfo.SkillInfo.Apply
                    .Any(apply => apply.Attribute == Commons.Enums.SkillCodeApplyAttributeEnum.AttackPowerDown))

                .ToList();

                var totalValue = 0;
                var SomaValue = 0;

                foreach (var debuff in debuffInfo)
                {
                    foreach (var apply in debuff.BuffInfo.SkillInfo.Apply)
                    {

                        switch (apply.Type)
                        {
                            case SkillCodeApplyTypeEnum.Default:
                                totalValue += apply.Value;
                                break;

                            case SkillCodeApplyTypeEnum.AlsoPercent:
                            case SkillCodeApplyTypeEnum.Percent:
                                {

                                    SomaValue += apply.Value + (debuff.TypeN) * apply.IncreaseValue;

                                    double fatorReducao = SomaValue / 100;

                                    // Calculando o novo finalDmg após a redução
                                    finalDmg -= (int)(finalDmg * fatorReducao);

                                }
                                break;

                            case SkillCodeApplyTypeEnum.Unknown200:
                                {

                                    SomaValue += apply.AdditionalValue;

                                    double fatorReducao = SomaValue / 100.0;

                                    // Calculando o novo finalDmg após a redução
                                    finalDmg -= (int)(finalDmg * fatorReducao);

                                }
                                break;

                        }
                        break;
                    
                    }
                }
            }

            return finalDmg;
        }
        private int CalculateDamageOrHeal(GameClient client, MobConfigModel? targetMob, DigimonSkillAssetModel? targetSkill, SkillCodeAssetModel? skill, byte skillSlot)
        {

            var SkillValue = skill.Apply.FirstOrDefault(x => x.Type > 0);

            double f1BaseDamage = (SkillValue.Value) + ((client.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Partner.CurrentType).Skills[skillSlot].CurrentLevel) * SkillValue.IncreaseValue);
            double SkillFactor = 0;
            double MultiplierAttribute = 0;

            var Percentual = (decimal)client.Partner.SCD / 100;
            SkillFactor = (double)Percentual; // AttributeMultiplier

            double factorFromPF = 144.0 / client.Tamer.Partner.Digiclone.ATValue;

            double cloneFactor = Math.Round(1.0 + (0.43 / factorFromPF), 2);
            f1BaseDamage = Math.Floor(f1BaseDamage * cloneFactor);

            double addedf1Damage = Math.Floor(f1BaseDamage * SkillFactor / 100.0);


            var attributeVantage = client.Tamer.Partner.BaseInfo.Attribute.
                HasAttributeAdvantage(targetMob.Attribute);
            var elementVantage = client.Tamer.Partner.BaseInfo.Element
                .HasElementAdvantage(targetMob.Element);


            var Damage = (int)Math.Floor(f1BaseDamage + addedf1Damage + client.Tamer.Partner.AT + client.Tamer.Partner.SKD);


            if (client.Partner.AttributeExperience.CurrentAttributeExperience && attributeVantage)
            {

                MultiplierAttribute = (2 + ((client.Partner.ATT) / 200.0));
                Random random = new Random();

                // Gere um valor aleatório entre 0% e 5% a mais do valor original
                double percentagemBonus = random.NextDouble() * 0.05;

                // Calcule o valor final com o bônus
                return (int)((int)Math.Floor(MultiplierAttribute * Damage) * (1.0 + percentagemBonus));

            }
            else if (client.Partner.AttributeExperience.CurrentElementExperience && elementVantage)
            {
                MultiplierAttribute = 2;

                Random random = new Random();

                // Gere um valor aleatório entre 0% e 5% a mais do valor original
                double percentagemBonus = random.NextDouble() * 0.05;

                // Calcule o valor final com o bônus
                return (int)((int)Math.Floor(MultiplierAttribute * Damage) * (1.0 + percentagemBonus));
            }
            else
            {
                Random random = new Random();

                // Gere um valor aleatório entre 0% e 5% a mais do valor original
                double percentagemBonus = random.NextDouble() * 0.05;

                // Calcule o valor final com o bônus
                return (int)(Damage * (1.0 + percentagemBonus));


            }

        }
        private int CalculateDamageOrHeal(GameClient client, SummonMobModel? targetMob, DigimonSkillAssetModel? targetSkill, SkillCodeAssetModel? skill, byte skillSlot)
        {

            var SkillValue = skill.Apply.FirstOrDefault(x => x.Type > 0);

            double f1BaseDamage = (SkillValue.Value) + ((client.Partner.Evolutions.FirstOrDefault(x => x.Type == client.Partner.CurrentType).Skills[skillSlot].CurrentLevel) * SkillValue.IncreaseValue);
            double SkillFactor = 0;
            double MultiplierAttribute = 0;

            var Percentual = (decimal)client.Partner.SCD / 100;
            SkillFactor = (double)Percentual; // AttributeMultiplier

            double factorFromPF = 144.0 / client.Tamer.Partner.Digiclone.ATValue;

            double cloneFactor = Math.Round(1.0 + (0.43 / factorFromPF), 2);
            f1BaseDamage = Math.Floor(f1BaseDamage * cloneFactor);

            double addedf1Damage = Math.Floor(f1BaseDamage * SkillFactor / 100.0);


            var attributeVantage = client.Tamer.Partner.BaseInfo.Attribute.
                HasAttributeAdvantage(targetMob.Attribute);
            var elementVantage = client.Tamer.Partner.BaseInfo.Element
                .HasElementAdvantage(targetMob.Element);


            var Damage = (int)Math.Floor(f1BaseDamage + addedf1Damage + client.Tamer.Partner.AT + client.Tamer.Partner.SKD);


            if (client.Partner.AttributeExperience.CurrentAttributeExperience && attributeVantage)
            {

                MultiplierAttribute = (2 + ((client.Partner.ATT) / 200.0));
                Random random = new Random();

                // Gere um valor aleatório entre 0% e 5% a mais do valor original
                double percentagemBonus = random.NextDouble() * 0.05;

                // Calcule o valor final com o bônus
                return (int)((int)Math.Floor(MultiplierAttribute * Damage) * (1.0 + percentagemBonus));

            }
            else if (client.Partner.AttributeExperience.CurrentElementExperience && elementVantage)
            {
                MultiplierAttribute = 2;

                Random random = new Random();

                // Gere um valor aleatório entre 0% e 5% a mais do valor original
                double percentagemBonus = random.NextDouble() * 0.05;

                // Calcule o valor final com o bônus
                return (int)((int)Math.Floor(MultiplierAttribute * Damage) * (1.0 + percentagemBonus));
            }
            else
            {
                Random random = new Random();

                // Gere um valor aleatório entre 0% e 5% a mais do valor original
                double percentagemBonus = random.NextDouble() * 0.05;

                // Calcule o valor final com o bônus
                return (int)(Damage * (1.0 + percentagemBonus));


            }

        }
    }
}