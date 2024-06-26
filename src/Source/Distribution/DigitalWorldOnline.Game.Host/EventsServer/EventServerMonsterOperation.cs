using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.Map;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Commons.Writers;


namespace DigitalWorldOnline.GameHost.EventsServer
{
    public sealed partial class EventServer
    {
        private Task MonsterOperation(GameMap map)
        {
            if (!map.ConnectedTamers.Any())
                return Task.CompletedTask;

            map.UpdateMapMobs();

            foreach (var mob in map.Mobs)
            {
                if (DateTime.Now > mob.ViewCheckTime)
                {
                    mob.SetViewCheckTime(2);

                    mob.TamersViewing.RemoveAll(x => !map.ConnectedTamers.Select(y => y.Id).Contains(x));

                    var nearTamers = map.NearestTamers(mob.Id);

                    if (!nearTamers.Any() && !mob.TamersViewing.Any())
                        continue;

                    if (!mob.Dead)
                    {
                        nearTamers.ForEach(nearTamer =>
                        {
                            if (!mob.TamersViewing.Contains(nearTamer))
                            {
                                mob.TamersViewing.Add(nearTamer);

                                var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == nearTamer);

                                targetClient?.Send(new LoadMobsPacket(mob));
                            }
                        });
                    }

                    var farTamers = map.ConnectedTamers.Select(x => x.Id).Except(nearTamers).ToList();

                    farTamers.ForEach(farTamer =>
                    {
                        if (mob.TamersViewing.Contains(farTamer))
                        {
                            var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == farTamer);

                            mob.TamersViewing.Remove(farTamer);
                            targetClient?.Send(new UnloadMobsPacket(mob));
                        }
                    });
                }

                if (!mob.CanAct)
                    continue;

                MobsOperation(map, mob);

                mob.SetNextAction();
            }

            return Task.CompletedTask;
        }

        private void BossOperation(GameMap map, MobConfigModel mob, List<long> nearTamers, List<long> farTamers)
        {
            throw new NotImplementedException();
        }

        private void MobsOperation(GameMap map, MobConfigModel mob)
        {
            switch (mob.CurrentAction)
            {
                case MobActionEnum.Respawn:
                    {
                        mob.Reset();
                        mob.ResetLocation();
                    }
                    break;

                case MobActionEnum.Reward:
                    {
                        ItemsReward(map, mob);

                        ExperienceReward(map, mob);
                    }
                    break;

                case MobActionEnum.Wait:
                    {
                        if (mob.Respawn && DateTime.Now > mob.DieTime.AddSeconds(2))
                        {
                            mob.SetNextWalkTime(UtilitiesFunctions.RandomInt(7, 14));
                            mob.SetAgressiveCheckTime(5);
                            mob.SetRespawn();
                        }
                        else
                        {
                            map.AttackNearbyTamer(mob, mob.TamersViewing, _assets.NpcColiseum);
                        }
                    }
                    break;

                case MobActionEnum.Walk:
                    {
                        //map.BroadcastForTargetTamers(mob.TamersViewing, new SyncConditionPacket(mob.GeneralHandler, ConditionEnum.Default).Serialize());
                        //mob.Move();
                        //map.BroadcastForTargetTamers(mob.TamersViewing, new MobWalkPacket(mob).Serialize());
                    }
                    break;

                case MobActionEnum.GiveUp:
                    {
                        map.BroadcastForTargetTamers(mob.TamersViewing, new SyncConditionPacket(mob.GeneralHandler, ConditionEnum.Immortal).Serialize());
                        mob.ResetLocation();
                        map.BroadcastForTargetTamers(mob.TamersViewing, new MobRunPacket(mob).Serialize());
                        map.BroadcastForTargetTamers(mob.TamersViewing, new SetCombatOffPacket(mob.GeneralHandler).Serialize());

                        foreach (var targetTamer in mob.TargetTamers)
                        {
                            if (targetTamer.TargetMobs.Count <= 1)
                            {
                                targetTamer.StopBattle();
                                map.BroadcastForTamerViewsAndSelf(targetTamer.Id, new SetCombatOffPacket(targetTamer.Partner.GeneralHandler).Serialize());
                            }
                        }

                        mob.Reset(true);
                        map.BroadcastForTargetTamers(mob.TamersViewing, new UpdateCurrentHPRatePacket(mob.GeneralHandler, mob.CurrentHpRate).Serialize());
                    }
                    break;

                case MobActionEnum.Attack:
                    {
                        if (!mob.Dead && ((mob.TargetTamer == null || mob.TargetTamer.Hidden) || DateTime.Now > mob.LastHitTryTime.AddSeconds(15))) //Anti-kite
                        {
                            mob.GiveUp();
                            break;
                        }

                        if (!mob.Dead && !mob.Chasing && mob.TargetAlive)
                        {
                            var diff = UtilitiesFunctions.CalculateDistance(
                                mob.CurrentLocation.X,
                                mob.Target.Location.X,
                                mob.CurrentLocation.Y,
                                mob.Target.Location.Y);

                            var range = Math.Max(mob.ARValue, mob.Target.BaseInfo.ARValue);
                            if (diff <= range)
                            {
                                if (DateTime.Now < mob.LastHitTime.AddMilliseconds(mob.ASValue))
                                    break;

                                var missed = false;

                                if (mob.TargetTamer != null && mob.TargetTamer.GodMode)
                                    missed = true;
                                else if (mob.CanMissHit())
                                    missed = true;

                                if (missed)
                                {
                                    mob.UpdateLastHitTry();
                                    map.BroadcastForTargetTamers(mob.TamersViewing, new MissHitPacket(mob.GeneralHandler, mob.TargetHandler).Serialize());
                                    mob.UpdateLastHit();
                                    break;
                                }

                                map.AttackTarget(mob, _assets.NpcColiseum);
                            }
                            else
                            {
                                map.ChaseTarget(mob);
                            }
                        }

                        if (mob.Dead)
                        {
                            foreach (var targetTamer in mob.TargetTamers)
                            {

                                targetTamer.StopBattle();
                                map.BroadcastForTamerViewsAndSelf(targetTamer.Id, new SetCombatOffPacket(targetTamer.Partner.GeneralHandler).Serialize());

                            }
                        }
                    }
                    break;
                case MobActionEnum.UseAttackSkill:
                    {
                        if (!mob.Dead && ((mob.TargetTamer == null || mob.TargetTamer.Hidden) || DateTime.Now > mob.LastSkillTryTime.AddSeconds(mob.Cooldown))) //Anti-kite
                        {
                            mob.GiveUp();
                            break;
                        }

                        if (!mob.Dead && !mob.Chasing && mob.TargetAlive)
                        {
                            var diff = UtilitiesFunctions.CalculateDistance(
                                mob.CurrentLocation.X,
                                mob.Target.Location.X,
                                mob.CurrentLocation.Y,
                                mob.Target.Location.Y);

                            var range = Math.Max(mob.ARValue, mob.Target.BaseInfo.ARValue);

                            if (diff <= range)
                            {
                                var skillList = _assets.MonsterSkillInfo.Where(x => x.Type == mob.Type).ToList();

                                if (!skillList.Any())
                                {
                                    mob.UpdateCheckSkill(true);
                                    mob.SetNextAction();
                                    break;
                                }

                                Random random = new Random();

                                var targetSkill = skillList[random.Next(0, skillList.Count)];

                                if (DateTime.Now < mob.LastSkillTryTime.AddMilliseconds(mob.Cooldown) && mob.Cooldown > 0)
                                    break;

                                map.AttackTarget(mob, _assets.NpcColiseum);
                            }
                            else
                            {
                                map.ChaseTarget(mob);
                            }
                        }

                        if (mob.Dead)
                        {
                            foreach (var targetTamer in mob.TargetTamers)
                            {

                                targetTamer.StopBattle();
                                map.BroadcastForTamerViewsAndSelf(targetTamer.Id, new SetCombatOffPacket(targetTamer.Partner.GeneralHandler).Serialize());

                            }
                        }
                    }
                    break;
            }
        }

        private void ItemsReward(GameMap map, MobConfigModel mob)
        {
            if (mob.DropReward == null)
                return;

            if (mob.Class == 8)
                RaidReward(map, mob);
            else
                DropReward(map, mob);
        }

        private void ExperienceReward(GameMap map, MobConfigModel mob)
        {
            if (mob.ExpReward == null)
                return;

            foreach (var tamer in mob.TargetTamers)
            {
                var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == tamer?.Id);
                if (targetClient == null)
                    continue;

                double expBonusMultiplier = tamer.BonusEXP / 100.0;

                var tamerExpToReceive = (long)(CalculateExperience(tamer.Partner.Level, mob.Level, mob.ExpReward.TamerExperience) * expBonusMultiplier); //TODO: +bonus


                if (CalculateExperience(tamer.Partner.Level, mob.Level, mob.ExpReward.TamerExperience) == 0)
                    tamerExpToReceive = 0;

                if (tamerExpToReceive > 100) tamerExpToReceive += UtilitiesFunctions.RandomInt(-15, 15);
                var tamerResult = ReceiveTamerExp(targetClient.Tamer, tamerExpToReceive);

                var partnerExpToReceive = (long)(CalculateExperience(tamer.Partner.Level, mob.Level, mob.ExpReward.DigimonExperience) * expBonusMultiplier); //TODO: +bonus

                if (CalculateExperience(tamer.Partner.Level, mob.Level, mob.ExpReward.DigimonExperience) == 0)
                    partnerExpToReceive = 0;

                if (partnerExpToReceive > 100) partnerExpToReceive += UtilitiesFunctions.RandomInt(-15, 15);
                var partnerResult = ReceivePartnerExp(targetClient.Partner, mob, partnerExpToReceive);

                targetClient.Send(
                    new ReceiveExpPacket(
                        tamerExpToReceive,
                        0,//TODO: obter os bonus
                        targetClient.Tamer.CurrentExperience,
                        targetClient.Partner.GeneralHandler,
                        partnerExpToReceive,
                        0,//TODO: obter os bonus
                        targetClient.Partner.CurrentExperience,
                        targetClient.Partner.CurrentEvolution.SkillExperience
                    )
                );

                //TODO: importar e tratar isso
                if (targetClient.Partner.CurrentEvolution.SkillMastery == 0 && targetClient.Partner.CurrentEvolution.SkillExperience >= 1792)
                {
                    targetClient.Partner.ReceiveSkillPoint();

                    var evolutionIndex = targetClient.Partner.Evolutions.IndexOf(targetClient.Partner.CurrentEvolution);

                    var packet = new PacketWriter();
                    packet.Type(1105);
                    packet.WriteInt(targetClient.Partner.GeneralHandler);
                    packet.WriteByte((byte)(evolutionIndex + 1));

                    packet.WriteByte(targetClient.Partner.CurrentEvolution.SkillPoints);
                    packet.WriteByte(targetClient.Partner.CurrentEvolution.SkillMastery);

                    packet.WriteInt(targetClient.Partner.CurrentEvolution.SkillExperience);

                    map.BroadcastForTamerViewsAndSelf(targetClient.TamerId, packet.Serialize());
                }

                if (tamerResult.LevelGain > 0 || partnerResult.LevelGain > 0)
                {
                    targetClient.Send(new UpdateStatusPacket(targetClient.Tamer));

                    map.BroadcastForTamerViewsAndSelf(targetClient.TamerId,
                        new UpdateMovementSpeedPacket(targetClient.Tamer).Serialize());
                }

                //if (tamerResult.Success)
                //{
                //    _sender.Send(
                //        new UpdateCharacterExperienceCommand(
                //            targetClient.TamerId,
                //            targetClient.Tamer.CurrentExperience,
                //            targetClient.Tamer.Level
                //        )
                //    );
                //}
                //
                //if (partnerResult.Success)
                //{
                //    _sender.Send(new UpdateDigimonExperienceCommand(targetClient.Partner));
                //}
            }
        }
        public long CalculateExperience(int tamerLevel, int mobLevel, long baseExperience)
        {
            int levelDifference = mobLevel - tamerLevel;

            if (levelDifference >= -30 && levelDifference <= 30)
            {
                if (levelDifference > 0)
                {
                    return (long)(baseExperience / (2 * levelDifference));
                }
                else if (levelDifference < 0)
                {
                    return (long)(baseExperience * (2 * Math.Abs(levelDifference)));
                }
                // Se a diferença for 0, não é aplicado redutor, a experiência base é mantida.
            }
            else
            {
                return 0; // A diferença de níveis é maior que 30, o tamer não recebe experiência
            }

            return baseExperience; // Se não houver redutor, a experiência base é mantida
        }

        private void DropReward(GameMap map, MobConfigModel mob)
        {
            var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == mob.TargetTamer?.Id);
            if (targetClient == null)
                return;

            var bitsReward = mob.DropReward.BitsDrop;

            if (bitsReward != null && bitsReward.Chance >= UtilitiesFunctions.RandomDouble())
            {
                var drop = _dropManager.CreateBitDrop(
                    targetClient.TamerId,
                    targetClient.Tamer.GeneralHandler,
                    bitsReward.MinAmount,
                    bitsReward.MaxAmount,
                    mob.CurrentLocation.MapId,
                    mob.CurrentLocation.X,
                    mob.CurrentLocation.Y
                );

                map.AddMapDrop(drop);
            }

            if (!mob.DropReward.Drops.Any())
                return;

            var itemsReward = new List<ItemDropConfigModel>();
            itemsReward.AddRange(mob.DropReward.Drops);

            var dropped = 0;
            var totalDrops = UtilitiesFunctions.RandomInt(
                mob.DropReward.MinAmount,
                mob.DropReward.MaxAmount);

            while (dropped < totalDrops)
            {
                if (!itemsReward.Any())
                {
                    _logger.Warning($"Mob {mob.Id} has incorrect drops configuration.");
                    break;
                }

                var possibleDrops = itemsReward.OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var itemDrop in possibleDrops)
                {
                    if (itemDrop.Chance >= UtilitiesFunctions.RandomDouble())
                    {
                        var drop = _dropManager.CreateItemDrop(
                            targetClient.Tamer.Id,
                            targetClient.Tamer.GeneralHandler,
                            itemDrop.ItemId,
                            itemDrop.MinAmount,
                            itemDrop.MaxAmount,
                            mob.CurrentLocation.MapId,
                            mob.CurrentLocation.X,
                            mob.CurrentLocation.Y
                        );

                        dropped++;

                        map.AddMapDrop(drop);

                        itemsReward.RemoveAll(x => x.Id == itemDrop.Id);
                        break;
                    }
                }
            }
        }

        private void RaidReward(GameMap map, MobConfigModel mob)
        {
            Console.WriteLine($"Raid {mob.Name} rankers {mob.RaidDamage.Count}.");
            var writer = new PacketWriter();
            writer.Type(1604);
            writer.WriteInt(mob.RaidDamage.Count);

            int i = 1;

            var updateItemList = new List<ItemListModel>();

            foreach (var raidTamer in mob.RaidDamage.OrderByDescending(x => x.Value))
            {
                var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == raidTamer.Key);

                writer.WriteInt(i);
                writer.WriteString(targetClient?.Tamer?.Name ?? $"Tamer{i}");
                writer.WriteString(targetClient?.Partner?.Name ?? $"Partner{i}");
                writer.WriteInt(raidTamer.Value);

                var bitsReward = mob.DropReward.BitsDrop;
                if (targetClient != null && bitsReward != null && bitsReward.Chance >= UtilitiesFunctions.RandomDouble())
                {
                    var drop = _dropManager.CreateBitDrop(
                        targetClient.Tamer.Id,
                        targetClient.Tamer.GeneralHandler,
                        bitsReward.MinAmount,
                        bitsReward.MaxAmount,
                        mob.CurrentLocation.MapId,
                        mob.CurrentLocation.X,
                        mob.CurrentLocation.Y
                    );

                    map.DropsToAdd.Add(drop);
                }

                var raidRewards = mob.DropReward.Drops;
                if (targetClient != null && raidRewards != null && raidRewards.Any())
                {
                    var rewards = raidRewards.Where(x => x.Rank == i);

                    if (rewards == null || !rewards.Any())
                        rewards = raidRewards.Where(x => x.Rank == raidRewards.Max(x => x.Rank));

                    foreach (var reward in rewards)
                    {
                        var newItem = new ItemModel();
                        newItem.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == reward.ItemId));

                        if (newItem.ItemInfo == null)
                        {
                            _logger.Warning($"No item info found with ID {reward.ItemId} for tamer {targetClient.TamerId}.");
                            targetClient.Send(new SystemMessagePacket($"No item info found with ID {reward.ItemId}."));
                            break;
                        }

                        newItem.ItemId = reward.ItemId;
                        newItem.Amount = UtilitiesFunctions.RandomInt(reward.MinAmount, reward.MaxAmount);

                        if (newItem.IsTemporary)
                            newItem.SetRemainingTime((uint)newItem.ItemInfo.UsageTimeMinutes);

                        var itemClone = (ItemModel)newItem.Clone();
                        if (targetClient.Tamer.Inventory.AddItem(newItem))
                        {
                            targetClient.Send(new ReceiveItemPacket(itemClone, InventoryTypeEnum.Inventory));
                            updateItemList.Add(targetClient.Tamer.Inventory);
                        }
                        else
                        {
                            targetClient.Send(new PickItemFailPacket(PickItemFailReasonEnum.InventoryFull));
                        }
                    }
                }

                i++;
            }

            map.BroadcastForTargetTamers(mob.RaidDamage.Select(x => x.Key).ToList(), writer.Serialize());
            //updateItemList.ForEach(itemList => { _sender.Send(new UpdateItemsCommand(itemList)); });
        }
    }
}