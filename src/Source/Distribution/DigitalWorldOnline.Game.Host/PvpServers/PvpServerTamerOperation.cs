using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using System.Diagnostics;

namespace DigitalWorldOnline.GameHost
{
    public sealed partial class PvpServer
    {
        public void TamerOperation(GameMap map)
        {
            if (!map.ConnectedTamers.Any())
            {
                map.SetNoTamers();
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var tamer in map.ConnectedTamers)
            {
                var client = map.Clients.FirstOrDefault(x => x.TamerId == tamer.Id);

                if (client == null || !client.IsConnected || client.Partner == null)
                    continue;

                ShowOrHideTamer(map, tamer);

                PartnerAutoAttack(tamer);

                tamer.AutoRegen();
                tamer.ActiveEvolutionReduction();

                if (tamer.BreakEvolution)
                {
                    tamer.ActiveEvolution.SetDs(0);
                    tamer.ActiveEvolution.SetXg(0);

                    if (tamer.Riding)
                    {
                        tamer.StopRideMode();

                        BroadcastForTamerViewsAndSelf(tamer.Id,
                            new UpdateMovementSpeedPacket(tamer).Serialize());

                        BroadcastForTamerViewsAndSelf(tamer.Id,
                            new RideModeStopPacket(tamer.GeneralHandler, tamer.Partner.GeneralHandler).Serialize());
                    }

                    map.BroadcastForTamerViewsAndSelf(tamer.Id,
                        new DigimonEvolutionSucessPacket(tamer.GeneralHandler,
                            tamer.Partner.GeneralHandler,
                            tamer.Partner.BaseType,
                            DigimonEvolutionEffectEnum.Back).Serialize());

                    var currentHp = client.Partner.CurrentHp;
                    var currentMaxHp = client.Partner.HP;
                    var currentDs = client.Partner.CurrentDs;
                    var currentMaxDs = client.Partner.DS;

                    tamer.Partner.UpdateCurrentType(tamer.Partner.BaseType);

                    tamer.Partner.SetBaseInfo(
                        _statusManager.GetDigimonBaseInfo(
                            tamer.Partner.CurrentType
                        )
                    );

                    tamer.Partner.SetBaseStatus(
                        _statusManager.GetDigimonBaseStatus(
                            tamer.Partner.CurrentType,
                            tamer.Partner.Level,
                            tamer.Partner.Size
                        )
                    );

                    client.Partner.AdjustHpAndDs(currentHp, currentMaxHp, currentDs, currentMaxDs);
                    client.Send(new UpdateStatusPacket(tamer));

                    _sender.Send(new UpdatePartnerCurrentTypeCommand(client.Partner));
                    _sender.Send(new UpdateCharacterActiveEvolutionCommand(tamer.ActiveEvolution));
                }

                if (tamer.CheckBuffsTime)
                {
                    tamer.UpdateBuffsCheckTime();

                    if (tamer.BuffList.HasActiveBuffs)
                    {
                        var buffsToRemove = tamer.BuffList.Buffs
                            .Where(x => x.Expired)
                            .ToList();

                        buffsToRemove.ForEach(buffToRemove =>
                        { map.BroadcastForTamerViewsAndSelf(tamer.Id, new RemoveBuffPacket(tamer.GeneralHandler, buffToRemove.BuffId).Serialize()); });

                        if (buffsToRemove.Any())
                        {
                            client?.Send(new UpdateStatusPacket(tamer));
                            map.BroadcastForTargetTamers(map.TamersView[tamer.Id], new UpdateCurrentHPRatePacket(tamer.GeneralHandler, tamer.HpRate).Serialize());
                            _sender.Send(new UpdateCharacterBuffListCommand(tamer.BuffList));
                        }
                    }

                    if (tamer.Partner.BuffList.HasActiveBuffs)
                    {
                        var buffsToRemove = tamer.Partner.BuffList.Buffs
                            .Where(x => x.Expired)
                            .ToList();

                        buffsToRemove.ForEach(buffToRemove =>
                        { map.BroadcastForTamerViewsAndSelf(tamer.Id, new RemoveBuffPacket(tamer.Partner.GeneralHandler, buffToRemove.BuffId).Serialize()); });

                        if (buffsToRemove.Any())
                        {
                            client?.Send(new UpdateStatusPacket(tamer));
                            map.BroadcastForTargetTamers(map.TamersView[tamer.Id], new UpdateCurrentHPRatePacket(tamer.Partner.GeneralHandler, tamer.Partner.HpRate).Serialize());

                            _sender.Send(new UpdateDigimonBuffListCommand(tamer.Partner.BuffList));
                        }
                    }
                }

                if (tamer.SyncResourcesTime)
                {
                    tamer.UpdateSyncResourcesTime();

                    client?.Send(new UpdateCurrentResourcesPacket(tamer.GeneralHandler, (short)tamer.CurrentHp, (short)tamer.CurrentDs, 0));
                    client?.Send(new UpdateCurrentResourcesPacket(tamer.Partner.GeneralHandler, (short)tamer.Partner.CurrentHp, (short)tamer.Partner.CurrentDs, 0));
                    map.BroadcastForTargetTamers(map.TamersView[tamer.Id], new UpdateCurrentHPRatePacket(tamer.GeneralHandler, tamer.HpRate).Serialize());
                    map.BroadcastForTargetTamers(map.TamersView[tamer.Id], new UpdateCurrentHPRatePacket(tamer.Partner.GeneralHandler, tamer.Partner.HpRate).Serialize());
                    map.BroadcastForTamerViewsAndSelf(tamer.Id, new SyncConditionPacket(tamer.GeneralHandler, tamer.CurrentCondition, tamer.ShopName).Serialize());
                }

                if (tamer.SaveResourcesTime)
                {
                    tamer.UpdateSaveResourcesTime();

                    var subStopWatch = new Stopwatch();
                    subStopWatch.Start();

                    _sender.Send(new UpdateCharacterLocationCommand(tamer.Location));
                    _sender.Send(new UpdateDigimonLocationCommand(tamer.Partner.Location));

                    _sender.Send(new UpdateCharacterBasicInfoCommand(tamer));

                    _sender.Send(new UpdateCharacterBasicInfoCommand(tamer));
                    _sender.Send(new UpdateEvolutionCommand(tamer.Partner.CurrentEvolution));

                    //_sender.Send(new UpdateCharacterProgressCommand(tamer.Progress));

                    subStopWatch.Stop();

                    if (subStopWatch.ElapsedMilliseconds >= 1500)
                    {
                        Console.WriteLine($"Save resources elapsed time: {subStopWatch.ElapsedMilliseconds}");
                    }
                }
            }

            stopwatch.Stop();

            var totalTime = stopwatch.Elapsed.TotalMilliseconds;

            if (totalTime >= 1000)
                Console.WriteLine($"TamersOperation ({map.ConnectedTamers.Count}): {totalTime}.");
        }

        private void ShowOrHideTamer(GameMap map, CharacterModel tamer)
        {
            foreach (var connectedTamer in map.ConnectedTamers.Where(x => x.Id != tamer.Id))
            {
                ShowTamer(map, tamer, connectedTamer.Id);
            }
        }

        private void ShowTamer(GameMap map, CharacterModel tamerToShow, long tamerToSeeId)
        {
            if (!map.ViewingTamer(tamerToShow.Id, tamerToSeeId))
            {
                foreach (var item in tamerToShow.Equipment.EquippedItems.Where(x => x.ItemInfo == null))
                    item?.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == item?.ItemId));

                map.ShowTamer(tamerToShow.Id, tamerToSeeId);

                var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == tamerToSeeId);
                if (targetClient != null)
                {
                    targetClient.Send(new LoadTamerPacket(tamerToShow));
                    if (tamerToShow.InBattle)
                    {
                        targetClient.Send(new SetCombatOnPacket(tamerToShow.GeneralHandler));
                        targetClient.Send(new SetCombatOnPacket(tamerToShow.Partner.GeneralHandler));
                    }
                }
            }
        }

        private void PartnerAutoAttack(CharacterModel tamer)
        {
            if (!tamer.Partner.AutoAttack)
                return;

            if (!tamer.Partner.IsAttacking && tamer.TargetPartner != null && tamer.TargetPartner.Alive)
            {
                tamer.Partner.SetEndAttacking();
                tamer.SetHidden(false);

                if (!tamer.InBattle)
                {
                    _logger.Verbose($"Character {tamer.Id} engaged partner {tamer.TargetPartner.Id} - {tamer.TargetPartner.Name}.");
                    BroadcastForTamerViewsAndSelf(tamer.Id, new SetCombatOnPacket(tamer.Partner.GeneralHandler).Serialize());
                    tamer.StartBattle(tamer.TargetPartner);
                }

                if (!tamer.TargetPartner.Character.InBattle)
                {
                    BroadcastForTamerViewsAndSelf(tamer.Id, new SetCombatOnPacket(tamer.TargetPartner.Character.GeneralHandler).Serialize());
                    tamer.TargetPartner.Character.StartBattle(tamer.Partner);
                }

                var missed = false;

                if (tamer.Partner.Level <= tamer.TargetPartner.Level)
                {
                    missed = tamer.CanMissHit();
                }

                if (missed)
                {
                    _logger.Verbose($"Partner {tamer.Partner.Id} missed hit on partner {tamer.TargetPartner.Id} - {tamer.TargetPartner.Name}.");
                    BroadcastForTamerViewsAndSelf(tamer.Id, new MissHitPacket(tamer.Partner.GeneralHandler, tamer.TargetPartner.GeneralHandler).Serialize());
                }
                else
                {
                    #region Hit Damage
                    var critBonusMultiplier = 0.00;
                    var blocked = false;
                    var finalDmg = CalculateDamage(tamer, out critBonusMultiplier, out blocked);
                    #endregion

                    if (finalDmg <= 0) finalDmg = 1;
                    if (finalDmg > tamer.TargetPartner.CurrentHp) finalDmg = tamer.TargetPartner.CurrentHp;

                    var newHp = tamer.TargetPartner.ReceiveDamage(finalDmg);

                    var hitType = blocked ? 2 : critBonusMultiplier > 0 ? 1 : 0;

                    if (newHp > 0)
                    {
                        _logger.Verbose($"Partner {tamer.Partner.Id} inflicted {finalDmg} to partner {tamer.TargetPartner?.Id} - {tamer.TargetPartner?.Name}.");

                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new HitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetPartner.GeneralHandler,
                                finalDmg,
                                tamer.TargetPartner.HP,
                                newHp,
                                hitType).Serialize());
                    }
                    else
                    {
                        _logger.Verbose($"Partner {tamer.Partner.Id} killed partner {tamer.TargetPartner?.Id} - {tamer.TargetPartner?.Name} with {finalDmg} damage.");

                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new KillOnHitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetPartner.GeneralHandler,
                                finalDmg,
                                hitType).Serialize());

                        tamer.TargetPartner.Character.Die();

                        if (!EnemiesAttacking(tamer.Location.MapId, tamer.Partner.Id))
                        {
                            tamer.StopBattle();

                            BroadcastForTamerViewsAndSelf(
                                tamer.Id,
                                new SetCombatOffPacket(tamer.Partner.GeneralHandler).Serialize());
                        }
                    }
                }

                tamer.Partner.UpdateLastHitTime();
            }

            if (tamer.TargetPartner == null || !tamer.TargetPartner.Alive)
                tamer.Partner?.StopAutoAttack();
        }

        private static int CalculateDamage(CharacterModel tamer, out double critBonusMultiplier, out bool blocked)
        {
            var baseDamage = tamer.Partner.AT - tamer.TargetPartner.DE + UtilitiesFunctions.RandomInt(1, 15);
            if (baseDamage < 0) baseDamage = 0;

            critBonusMultiplier = 0.00;
            double critChance = tamer.Partner.CC / 100;
            if (critChance >= UtilitiesFunctions.RandomDouble())
                critBonusMultiplier = tamer.Partner.CD;

            blocked = tamer.TargetPartner.BL >= UtilitiesFunctions.RandomDouble();
            var levelBonusMultiplier = tamer.Partner.Level > tamer.TargetPartner.Level ?
                (0.01f * (tamer.Partner.Level - tamer.TargetPartner.Level)) : 0; //TODO: externalizar no portal

            var attributeMultiplier = 0.00;
            if (tamer.Partner.BaseInfo.Attribute.HasAttributeAdvantage(tamer.TargetPartner.BaseInfo.Attribute))
            {
                var vlrAtual = tamer.Partner.GetAttributeExperience();
                var bonusMax = 50.0; //TODO: externalizar?
                var expMax = 10000; //TODO: externalizar?

                attributeMultiplier = (bonusMax * vlrAtual) / expMax;
            }
            else if (tamer.TargetPartner.BaseInfo.Attribute.HasAttributeAdvantage(tamer.Partner.BaseInfo.Attribute))
            {
                attributeMultiplier = -0.25;
            }

            var elementMultiplier = 0.00;
            if (tamer.Partner.BaseInfo.Element.HasElementAdvantage(tamer.TargetPartner.BaseInfo.Element))
            {
                var vlrAtual = tamer.Partner.GetElementExperience();
                var bonusMax = 0.5; //TODO: externalizar?
                var expMax = 10000; //TODO: externalizar?

                elementMultiplier = (bonusMax * vlrAtual) / expMax;
            }
            else if (tamer.TargetPartner.BaseInfo.Element.HasElementAdvantage(tamer.Partner.BaseInfo.Element))
            {
                elementMultiplier = -0.25;
            }

            baseDamage /= blocked ? 2 : 1;

            return (int)Math.Floor(baseDamage +
                (baseDamage * critBonusMultiplier) +
                (baseDamage * levelBonusMultiplier) +
                (baseDamage * attributeMultiplier) +
                (baseDamage * elementMultiplier));
        }
    }
}