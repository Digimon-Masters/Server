using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Models.Summon;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;
using System.Diagnostics;
using System.Text;

namespace DigitalWorldOnline.GameHost
{
    public sealed partial class DungeonsServer
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

                CheckLocationDebuff(client);
                GetInViewMobs(map, tamer);
                GetInViewMobs(map, tamer, true);

                ShowOrHideTamer(map, tamer);

                PartnerAutoAttack(tamer);

                CheckMonthlyReward(client);

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

                    var buffToRemove = client.Tamer.Partner.BuffList.TamerBaseSkill();
                    if (buffToRemove != null)
                    {
                        BroadcastForTamerViewsAndSelf(client.TamerId, new RemoveBuffPacket(client.Partner.GeneralHandler, buffToRemove.BuffId).Serialize());
                    }

                    client.Tamer.RemovePartnerPassiveBuff();

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

                    client.Tamer.SetPartnerPassiveBuff();

                    client.Partner.AdjustHpAndDs(currentHp, currentMaxHp, currentDs, currentMaxDs);

                    foreach (var buff in client.Tamer.Partner.BuffList.ActiveBuffs)
                        buff.SetBuffInfo(_assets.BuffInfo.FirstOrDefault(x => x.SkillCode == buff.SkillId && buff.BuffInfo == null || x.DigimonSkillCode == buff.SkillId && buff.BuffInfo == null));

                    client.Send(new UpdateStatusPacket(tamer));

                    if (client.Tamer.Partner.BuffList.TamerBaseSkill() != null)
                    {
                        var buffToApply = client.Tamer.Partner.BuffList.Buffs
                                    .Where(x => x.Duration == 0)
                                    .ToList();

                        buffToApply.ForEach(buffToApply =>
                        {

                            BroadcastForTamerViewsAndSelf(client.Tamer.Id, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffToApply.BuffId, buffToApply.SkillId, (short)buffToApply.TypeN, 0).Serialize());
                        });

                    }

                    var party = _partyManager.FindParty(client.TamerId);
                    if (party != null)
                    {
                        party.UpdateMember(party[client.TamerId]);

                        BroadcastForTargetTamers(party.GetMembersIdList(),
                            new PartyMemberInfoPacket(party[client.TamerId]).Serialize());
                    }

                    _sender.Send(new UpdatePartnerCurrentTypeCommand(client.Partner));
                    _sender.Send(new UpdateCharacterActiveEvolutionCommand(tamer.ActiveEvolution));
                    _sender.Send(new UpdateDigimonBuffListCommand(client.Partner.BuffList));
                }

                if (tamer.CheckExpiredItemsTime)
                {
                    tamer.SetLastExpiredItemsCheck();

                    foreach (var item in tamer.Inventory.EquippedItems)
                    {
                        if (item.ItemInfo != null && item.IsTemporary && item.Expired)
                        {

                            if (item.ItemInfo.UseTimeType == 2 || item.ItemInfo.UseTimeType == 3)
                            {
                                item.SetFirstExpired(false);

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabInven, item.Slot, item.ItemId, ExpiredTypeEnum.Quit));
                            }
                            else
                            {

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabInven, item.Slot, item.ItemId, ExpiredTypeEnum.Remove));
                                tamer.Inventory.RemoveOrReduceItem(item, item.Amount);
                            }
                        }
                    }

                    foreach (var item in tamer.Warehouse.EquippedItems)
                    {
                        if (item.ItemInfo != null && item.IsTemporary && item.Expired)
                        {

                            if (item.ItemInfo.UseTimeType == 2 || item.ItemInfo.UseTimeType == 3)
                            {
                                item.SetFirstExpired(false);

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabWarehouse, item.Slot, item.ItemId, ExpiredTypeEnum.Quit));
                            }
                            else
                            {

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabWarehouse, item.Slot, item.ItemId, ExpiredTypeEnum.Remove));
                                tamer.Inventory.RemoveOrReduceItem(item, item.Amount);
                            }
                        }
                    }

                    foreach (var item in tamer.AccountWarehouse.EquippedItems)
                    {
                        if (item.ItemInfo != null && item.IsTemporary && item.Expired)
                        {

                            if (item.ItemInfo.UseTimeType == 2 || item.ItemInfo.UseTimeType == 3)
                            {
                                item.SetFirstExpired(false);

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabShareStash, item.Slot, item.ItemId, ExpiredTypeEnum.Quit));
                            }
                            else
                            {

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabShareStash, item.Slot, item.ItemId, ExpiredTypeEnum.Remove));
                                tamer.Inventory.RemoveOrReduceItem(item, item.Amount);
                            }
                        }
                    }

                    foreach (var item in tamer.Equipment.EquippedItems)
                    {
                        if (item.ItemInfo != null && item.IsTemporary && item.Expired)
                        {

                            if (item.ItemInfo.UseTimeType == 2)
                            {
                                item.SetFirstExpired(false);

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabEquip, item.Slot, item.ItemId, ExpiredTypeEnum.Quit));
                            }
                            else
                            {

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabEquip, item.Slot, item.ItemId, ExpiredTypeEnum.Remove));
                                tamer.Equipment.RemoveOrReduceItem(item, item.Amount);
                            }
                        }
                    }

                    foreach (var item in tamer.ChipSets.EquippedItems)
                    {
                        if (item.ItemInfo != null && item.IsTemporary && item.Expired)
                        {

                            if (item.ItemInfo.UseTimeType == 2)
                            {
                                item.SetFirstExpired(false);

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabChipset, item.Slot, item.ItemId, ExpiredTypeEnum.Quit));
                            }
                            else
                            {

                                client.Send(new ItemExpiredPacket(InventorySlotTypeEnum.TabChipset, item.Slot, item.ItemId, ExpiredTypeEnum.Remove));
                                tamer.Equipment.RemoveOrReduceItem(item, item.Amount);
                            }
                        }
                    }



                    _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                    _sender.Send(new UpdateItemsCommand(client.Tamer.Warehouse));
                    _sender.Send(new UpdateItemsCommand(client.Tamer.AccountWarehouse));
                    _sender.Send(new UpdateItemsCommand(client.Tamer.Equipment));
                    _sender.Send(new UpdateItemsCommand(client.Tamer.ChipSets));
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
                        {
                            tamer.BuffList.Remove(buffToRemove.BuffId);
                            map.BroadcastForTamerViewsAndSelf(tamer.Id, new RemoveBuffPacket(tamer.GeneralHandler, buffToRemove.BuffId).Serialize());
                        });

                        if (buffsToRemove.Any())
                        {

                            client?.Send(new UpdateStatusPacket(tamer));
                            map.BroadcastForTargetTamers(tamer.Id, new UpdateCurrentHPRatePacket(tamer.GeneralHandler, tamer.HpRate).Serialize());
                            _sender.Send(new UpdateCharacterBuffListCommand(tamer.BuffList));

                        }
                    }

                    if (tamer.Partner.BuffList.HasActiveBuffs)
                    {
                        var buffsToRemove = tamer.Partner.BuffList.Buffs
                            .Where(x => x.Expired)
                            .ToList();



                        buffsToRemove.ForEach(buffToRemove =>
                        {
                            tamer.Partner.BuffList.Remove(buffToRemove.BuffId);
                            map.BroadcastForTamerViewsAndSelf(tamer.Id, new RemoveBuffPacket(tamer.Partner.GeneralHandler, buffToRemove.BuffId).Serialize());
                        });

                        if (buffsToRemove.Any())
                        {
                            client?.Send(new UpdateStatusPacket(tamer));
                            map.BroadcastForTargetTamers(tamer.Id, new UpdateCurrentHPRatePacket(tamer.Partner.GeneralHandler, tamer.Partner.HpRate).Serialize());
                            _sender.Send(new UpdateDigimonBuffListCommand(tamer.Partner.BuffList));
                        }
                    }
                }


                if (tamer.SyncResourcesTime)
                {
                    tamer.UpdateSyncResourcesTime();

                    client?.Send(new UpdateCurrentResourcesPacket(tamer.GeneralHandler, (short)tamer.CurrentHp, (short)tamer.CurrentDs, 0));
                    client?.Send(new UpdateCurrentResourcesPacket(tamer.Partner.GeneralHandler, (short)tamer.Partner.CurrentHp, (short)tamer.Partner.CurrentDs, 0));
                    client?.Send(new TamerXaiResourcesPacket(client.Tamer.XGauge, client.Tamer.XCrystals));

                    map.BroadcastForTargetTamers(tamer.Id, new UpdateCurrentHPRatePacket(tamer.GeneralHandler, tamer.HpRate).Serialize());
                    map.BroadcastForTargetTamers(tamer.Id, new UpdateCurrentHPRatePacket(tamer.Partner.GeneralHandler, tamer.Partner.HpRate).Serialize());
                    map.BroadcastForTamerViewsAndSelf(tamer.Id, new SyncConditionPacket(tamer.GeneralHandler, tamer.CurrentCondition, tamer.ShopName).Serialize());

                    var party = _partyManager.FindParty(tamer.Id);
                    if (party != null)
                    {
                        party.UpdateMember(party[tamer.Id]);

                        map.BroadcastForTargetTamers(party.GetMembersIdList(),
                            new PartyMemberInfoPacket(party[tamer.Id]).Serialize());
                    }
                }

                if (tamer.SaveResourcesTime)
                {
                    tamer.UpdateSaveResourcesTime();

                    var subStopWatch = new Stopwatch();
                    subStopWatch.Start();

                 
                    _sender.Send(new UpdateCharacterBasicInfoCommand(tamer));
                    _sender.Send(new UpdateEvolutionCommand(tamer.Partner.CurrentEvolution));
                    
                    subStopWatch.Stop();

                    if (subStopWatch.ElapsedMilliseconds >= 1500)
                    {
                        Console.WriteLine($"Save resources elapsed time: {subStopWatch.ElapsedMilliseconds}");
                    }
                }

                //if (tamer.ResetDailyQuestsTime)
                //{
                //    tamer.UpdateDailyQuestsSyncTime();

                //    var dailyQuestResetTime = _sender.Send(new DailyQuestResetTimeQuery());

                //    if (DateTime.Now >= dailyQuestResetTime.Result)
                //    {
                //        client.Send(new QuestDailyUpdatePacket());
                //    }
                //}
            }

            stopwatch.Stop();

            var totalTime = stopwatch.Elapsed.TotalMilliseconds;

            if (totalTime >= 1000)
                Console.WriteLine($"TamersOperation ({map.ConnectedTamers.Count}): {totalTime}.");
        }

        private void CheckLocationDebuff(GameClient client)
        {
            if (client.Tamer.DebuffTime)
            {
                client.Tamer.UpdateDebuffTime();

                if (client.Tamer.Location.MapId == 2001 || client.Tamer.Location.MapId == 2002)
                {
                    var debuff = client.Tamer.Partner.DebuffList.ActiveBuffs.FirstOrDefault(x => x.BuffId == 63000);
                    var evolutionType = _assets.DigimonBaseInfo.First(x => x.Type == client.Partner.CurrentType).EvolutionType;

                    if (debuff == null)
                    {

                        if ((EvolutionRankEnum)evolutionType == EvolutionRankEnum.Jogress|| (EvolutionRankEnum)evolutionType == EvolutionRankEnum.JogressX)
                        {

                            var duration = 0xffffffff;

                            var buffInfo = _assets.BuffInfo.FirstOrDefault(x => x.BuffId == 63000);

                            var newDigimonDebuff = DigimonDebuffModel.Create(buffInfo.BuffId, buffInfo.SkillCode, 0, 0);
                            newDigimonDebuff.SetBuffInfo(buffInfo);
                            client.Tamer.Partner.DebuffList.Buffs.Add(newDigimonDebuff);

                            BroadcastForTamerViewsAndSelf(client.TamerId, new AddBuffPacket(client.Tamer.Partner.GeneralHandler, buffInfo, (short)0, duration).Serialize());
                        }
                    }
                    else if ((EvolutionRankEnum)evolutionType != EvolutionRankEnum.Jogress && (EvolutionRankEnum)evolutionType != EvolutionRankEnum.JogressX)
                    {
                        BroadcastForTamerViewsAndSelf(client.TamerId, new RemoveBuffPacket(client.Tamer.Partner.GeneralHandler, debuff.BuffId).Serialize());
                        client.Tamer.Partner.DebuffList.Buffs.Remove(debuff);
                    }

                }
            }
        }

        private void GetInViewMobs(GameMap map, CharacterModel tamer)
        {
            List<long> mobsToAdd = new List<long>();
            List<long> mobsToRemove = new List<long>();

            // Criar uma cópia da lista de Mobs
            List<MobConfigModel> mobsCopy = new List<MobConfigModel>(map.Mobs);

            // Iterar sobre a cópia da lista
            mobsCopy.ForEach(mob =>
            {
                if (tamer.TempShowFullMap)
                {
                    if (!tamer.MobsInView.Contains(mob.Id))
                        mobsToAdd.Add(mob.Id);
                }
                else
                {
                    if (mob != null)
                    {
                        var distanceDifference = UtilitiesFunctions.CalculateDistance(
                            tamer.Location.X,
                            mob.CurrentLocation.X,
                            tamer.Location.Y,
                            mob.CurrentLocation.Y);

                        if (distanceDifference <= _startToSee && !tamer.MobsInView.Contains(mob.Id))
                            mobsToAdd.Add(mob.Id);

                        if (distanceDifference >= _stopSeeing && tamer.MobsInView.Contains(mob.Id))
                            mobsToRemove.Add(mob.Id);
                    }
                }
            });

            // Adicionar e remover os IDs de Mob na lista tamer.MobsInView após a iteração
            mobsToAdd.ForEach(id => tamer.MobsInView.Add(id));
            mobsToRemove.ForEach(id => tamer.MobsInView.Remove(id));
        }

        private void GetInViewMobs(GameMap map, CharacterModel tamer, bool Summon)
        {
            List<long> mobsToAdd = new List<long>();
            List<long> mobsToRemove = new List<long>();

            // Criar uma cópia da lista de Mobs
            List<SummonMobModel> mobsCopy = new List<SummonMobModel>(map.SummonMobs);

            // Iterar sobre a cópia da lista
            mobsCopy.ForEach(mob =>
            {
                if (tamer.TempShowFullMap)
                {
                    if (!tamer.MobsInView.Contains(mob.Id))
                        mobsToAdd.Add(mob.Id);
                }
                else
                {
                    var distanceDifference = UtilitiesFunctions.CalculateDistance(
                        tamer.Location.X,
                        mob.CurrentLocation.X,
                        tamer.Location.Y,
                        mob.CurrentLocation.Y);

                    if (distanceDifference <= _startToSee && !tamer.MobsInView.Contains(mob.Id))
                        mobsToAdd.Add(mob.Id);

                    if (distanceDifference >= _stopSeeing && tamer.MobsInView.Contains(mob.Id))
                        mobsToRemove.Add(mob.Id);
                }
            });

            // Adicionar e remover os IDs de Mob na lista tamer.MobsInView após a iteração
            mobsToAdd.ForEach(id => tamer.MobsInView.Add(id));
            mobsToRemove.ForEach(id => tamer.MobsInView.Remove(id));
        }
        /// <summary>
        /// Updates the current partners handler values;
        /// </summary>
        /// <param name="mapId">Current map id</param>
        /// <param name="digimons">Current digimons</param>
        public void SetDigimonHandlers(int mapId, List<DigimonModel> digimons)
        {
            Maps.FirstOrDefault(x => x.MapId == mapId)?.SetDigimonHandlers(digimons);
        }

        /// <summary>
        /// Swaps the digimons current handler.
        /// </summary>
        /// <param name="mapId">Target map handler manager</param>
        /// <param name="oldPartnerId">Old partner identifier</param>
        /// <param name="newPartner">New partner</param>
        public void SwapDigimonHandlers(int mapId, DigimonModel oldPartner, DigimonModel newPartner)
        {
            Maps.FirstOrDefault(x => x.MapId == mapId)?.SwapDigimonHandlers(oldPartner, newPartner);
        }

        private void ShowOrHideTamer(GameMap map, CharacterModel tamer)
        {
            foreach (var connectedTamer in map.ConnectedTamers.Where(x => x.Id != tamer.Id))
            {
                var distanceDifference = UtilitiesFunctions.CalculateDistance(
                    tamer.Location.X,
                    connectedTamer.Location.X,
                    tamer.Location.Y,
                    connectedTamer.Location.Y);

                if (distanceDifference <= _startToSee)
                    ShowTamer(map, tamer, connectedTamer.Id);
                else if (distanceDifference >= _stopSeeing)
                    HideTamer(map, tamer, connectedTamer.Id);
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
                    targetClient.Send(new LoadBuffsPacket(tamerToShow));
                    if (tamerToShow.InBattle)
                    {
                        targetClient.Send(new SetCombatOnPacket(tamerToShow.GeneralHandler));
                        targetClient.Send(new SetCombatOnPacket(tamerToShow.Partner.GeneralHandler));
                    }
#if DEBUG
                    var serialized = SerializeShowTamer(tamerToShow);
                    //File.WriteAllText($"Shows\\Show{tamerToShow.Id}To{tamerToSeeId}_{DateTime.Now:dd_MM_yy_HH_mm_ss}.temp", serialized);
#endif
                }
            }
        }

        private void HideTamer(GameMap map, CharacterModel tamerToHide, long tamerToBlindId)
        {
            if (map.ViewingTamer(tamerToHide.Id, tamerToBlindId))
            {
                map.HideTamer(tamerToHide.Id, tamerToBlindId);

                var targetClient = map.Clients.FirstOrDefault(x => x.TamerId == tamerToBlindId);

                if (targetClient != null)
                {
                    targetClient.Send(new UnloadTamerPacket(tamerToHide));

#if DEBUG
                    var serialized = SerializeHideTamer(tamerToHide);
                    //File.WriteAllText($"Hides\\Hide{tamerToHide.Id}To{tamerToBlindId}_{DateTime.Now:dd_MM_yy_HH_mm_ss}.temp", serialized);
#endif
                }
            }
        }

        private static string SerializeHideTamer(CharacterModel tamer)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Tamer{tamer.Id}{tamer.Name}");
            sb.AppendLine($"TamerHandler {tamer.GeneralHandler.ToString()}");
            sb.AppendLine($"TamerLocation {tamer.Location.X.ToString()}");
            sb.AppendLine($"TamerLocation {tamer.Location.Y.ToString()}");

            sb.AppendLine($"Partner{tamer.Partner.Id}{tamer.Partner.Name}");
            sb.AppendLine($"PartnerHandler {tamer.Partner.GeneralHandler.ToString()}");
            sb.AppendLine($"PartnerLocation {tamer.Partner.Location.X.ToString()}");
            sb.AppendLine($"PartnerLocation {tamer.Partner.Location.Y.ToString()}");

            return sb.ToString();
        }

        private static string SerializeShowTamer(CharacterModel tamer)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Partner{tamer.Partner.Id}");
            sb.AppendLine($"PartnerName {tamer.Partner.Name}");
            sb.AppendLine($"PartnerLocation {tamer.Partner.Location.X.ToString()}");
            sb.AppendLine($"PartnerLocation {tamer.Partner.Location.Y.ToString()}");
            sb.AppendLine($"PartnerHandler {tamer.Partner.GeneralHandler.ToString()}");
            sb.AppendLine($"PartnerCurrentType {tamer.Partner.CurrentType.ToString()}");
            sb.AppendLine($"PartnerSize {tamer.Partner.Size.ToString()}");
            sb.AppendLine($"PartnerLevel {tamer.Partner.Level.ToString()}");
            sb.AppendLine($"PartnerModel {tamer.Partner.Model.ToString()}");
            sb.AppendLine($"PartnerMS {tamer.Partner.MS.ToString()}");
            sb.AppendLine($"PartnerAS {tamer.Partner.AS.ToString()}");
            sb.AppendLine($"PartnerHPRate {tamer.Partner.HpRate.ToString()}");
            sb.AppendLine($"PartnerCloneTotalLv {tamer.Partner.Digiclone.CloneLevel.ToString()}");
            sb.AppendLine($"PartnerCloneAtLv {tamer.Partner.Digiclone.ATLevel.ToString()}");
            sb.AppendLine($"PartnerCloneBlLv {tamer.Partner.Digiclone.BLLevel.ToString()}");
            sb.AppendLine($"PartnerCloneCtLv {tamer.Partner.Digiclone.CTLevel.ToString()}");
            sb.AppendLine($"PartnerCloneEvLv {tamer.Partner.Digiclone.EVLevel.ToString()}");
            sb.AppendLine($"PartnerCloneHpLv {tamer.Partner.Digiclone.HPLevel.ToString()}");

            sb.AppendLine($"Tamer{tamer.Id}");
            sb.AppendLine($"TamerName {tamer.Name.ToString()}");
            sb.AppendLine($"TamerLocation {tamer.Location.X.ToString()}");
            sb.AppendLine($"TamerLocation {tamer.Location.Y.ToString()}");
            sb.AppendLine($"TamerHandler {tamer.GeneralHandler.ToString()}");
            sb.AppendLine($"TamerModel {tamer.Model.ToString()}");
            sb.AppendLine($"TamerLevel {tamer.Level.ToString()}");
            sb.AppendLine($"TamerMS {tamer.MS.ToString()}");
            sb.AppendLine($"TamerHpRate {tamer.HpRate.ToString()}");
            sb.AppendLine($"TamerEquipment {tamer.Equipment.ToString()}");
            sb.AppendLine($"TamerDigivice {tamer.Digivice.ToString()}");
            sb.AppendLine($"TamerCurrentCondition {tamer.CurrentCondition.ToString()}");
            sb.AppendLine($"TamerSize {tamer.Size.ToString()}");
            sb.AppendLine($"TamerCurrentTitle {tamer.CurrentTitle.ToString()}");
            sb.AppendLine($"TamerSealLeaderId {tamer.SealList.SealLeaderId.ToString()}");

            return sb.ToString();
        }

        private void PartnerAutoAttack(CharacterModel tamer)
        {
            if (!tamer.Partner.AutoAttack)
                return;

            if (!tamer.Partner.IsAttacking && tamer.TargetMob != null && tamer.TargetMob.Alive & tamer.Partner.Alive)
            {
                tamer.Partner.SetEndAttacking();
                tamer.SetHidden(false);

                if (!tamer.InBattle)
                {
                    _logger.Verbose($"Character {tamer.Id} engaged {tamer.TargetMob.Id} - {tamer.TargetMob.Name}.");
                    BroadcastForTamerViewsAndSelf(tamer.Id, new SetCombatOnPacket(tamer.Partner.GeneralHandler).Serialize());
                    tamer.StartBattle(tamer.TargetMob);
                }

                if (!tamer.TargetMob.InBattle)
                {
                    BroadcastForTamerViewsAndSelf(tamer.Id, new SetCombatOnPacket(tamer.TargetMob.GeneralHandler).Serialize());
                    tamer.TargetMob.StartBattle(tamer);
                }

                var missed = false;

                if (!tamer.GodMode)
                {
                    missed = tamer.CanMissHit();
                }

                if (missed)
                {
                    _logger.Verbose($"Partner {tamer.Partner.Id} missed hit on {tamer.TargetMob.Id} - {tamer.TargetMob.Name}.");
                    BroadcastForTamerViewsAndSelf(tamer.Id, new MissHitPacket(tamer.Partner.GeneralHandler, tamer.TargetMob.GeneralHandler).Serialize());
                }
                else
                {
                    #region Hit Damage
                    var critBonusMultiplier = 0.00;
                    var blocked = false;
                    var finalDmg = tamer.GodMode ? tamer.TargetMob.CurrentHP : CalculateDamage(tamer, out critBonusMultiplier, out blocked);
                    #endregion

                    if (finalDmg <= 0) finalDmg = 1;
                    if (finalDmg > tamer.TargetMob.CurrentHP) finalDmg = tamer.TargetMob.CurrentHP;

                    var newHp = tamer.TargetMob.ReceiveDamage(finalDmg, tamer.Id);

                    var hitType = blocked ? 2 : critBonusMultiplier > 0 ? 1 : 0;

                    if (newHp > 0)
                    {
                        _logger.Verbose($"Partner {tamer.Partner.Id} inflicted {finalDmg} to mob {tamer.TargetMob?.Id} - {tamer.TargetMob?.Name}({tamer.TargetMob?.Type}).");

                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new HitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetMob.GeneralHandler,
                                finalDmg,
                                tamer.TargetMob.HPValue,
                                newHp,
                                hitType).Serialize());
                    }
                    else
                    {
                        _logger.Verbose($"Partner {tamer.Partner.Id} killed mob {tamer.TargetMob?.Id} - {tamer.TargetMob?.Name}({tamer.TargetMob?.Type}) with {finalDmg} damage.");

                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new KillOnHitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetMob.GeneralHandler,
                                finalDmg,
                                hitType).Serialize());

                        tamer.TargetMob?.Die();


                        if (!MobsAttacking(tamer.Location.MapId, tamer.Id))
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

            if (!tamer.Partner.IsAttacking && tamer.TargetSummonMob != null && tamer.TargetSummonMob.Alive & tamer.Partner.Alive)
            {
                tamer.Partner.SetEndAttacking();
                tamer.SetHidden(false);

                if (!tamer.InBattle)
                {
                    _logger.Verbose($"Character {tamer.Id} engaged {tamer.TargetSummonMob.Id} - {tamer.TargetSummonMob.Name}.");
                    BroadcastForTamerViewsAndSelf(tamer.Id, new SetCombatOnPacket(tamer.Partner.GeneralHandler).Serialize());
                    tamer.StartBattle(tamer.TargetMob);
                }

                if (!tamer.TargetSummonMob.InBattle)
                {
                    BroadcastForTamerViewsAndSelf(tamer.Id, new SetCombatOnPacket(tamer.TargetSummonMob.GeneralHandler).Serialize());
                    tamer.TargetSummonMob.StartBattle(tamer);
                }

                var missed = false;

                if (!tamer.GodMode)
                {
                    missed = tamer.CanMissHit(true);
                }

                if (missed)
                {
                    _logger.Verbose($"Partner {tamer.Partner.Id} missed hit on {tamer.TargetSummonMob.Id} - {tamer.TargetSummonMob.Name}.");
                    BroadcastForTamerViewsAndSelf(tamer.Id, new MissHitPacket(tamer.Partner.GeneralHandler, tamer.TargetSummonMob.GeneralHandler).Serialize());
                }
                else
                {
                    #region Hit Damage
                    var critBonusMultiplier = 0.00;
                    var blocked = false;
                    var finalDmg = tamer.GodMode ? tamer.TargetSummonMob.CurrentHP : CalculateDamage(tamer, out critBonusMultiplier, out blocked);
                    #endregion

                    if (finalDmg <= 0) finalDmg = 1;
                    if (finalDmg > tamer.TargetSummonMob.CurrentHP) finalDmg = tamer.TargetSummonMob.CurrentHP;

                    var newHp = tamer.TargetSummonMob.ReceiveDamage(finalDmg, tamer.Id);

                    var hitType = blocked ? 2 : critBonusMultiplier > 0 ? 1 : 0;

                    if (newHp > 0)
                    {
                        _logger.Verbose($"Partner {tamer.Partner.Id} inflicted {finalDmg} to mob {tamer.TargetSummonMob?.Id} - {tamer.TargetSummonMob?.Name}({tamer.TargetSummonMob?.Type}).");

                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new HitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetSummonMob.GeneralHandler,
                                finalDmg,
                                tamer.TargetSummonMob.HPValue,
                                newHp,
                                hitType).Serialize());
                    }
                    else
                    {
                        _logger.Verbose($"Partner {tamer.Partner.Id} killed mob {tamer.TargetSummonMob?.Id} - {tamer.TargetSummonMob?.Name}({tamer.TargetSummonMob?.Type}) with {finalDmg} damage.");

                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new KillOnHitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetSummonMob.GeneralHandler,
                                finalDmg,
                                hitType).Serialize());

                        tamer.TargetSummonMob?.Die();

                        if (!MobsAttacking(tamer.Location.MapId, tamer.Id))
                        {
                            tamer.StopBattle(true);

                            BroadcastForTamerViewsAndSelf(
                                tamer.Id,
                                new SetCombatOffPacket(tamer.Partner.GeneralHandler).Serialize());
                        }
                    }
                }

                tamer.Partner.UpdateLastHitTime();
            }

            bool StopAttack = tamer.TargetSummonMob == null || !tamer.TargetSummonMob.Alive;

            if (tamer.TargetMob == null || !tamer.TargetMob.Alive && StopAttack)
                tamer.Partner?.StopAutoAttack();


        }

        private ReceiveExpResult ReceiveTamerExp(CharacterModel tamer, long tamerExpToReceive)
        {
            var tamerResult = _expManager.ReceiveTamerExperience(tamerExpToReceive, tamer);

            if (tamerResult.LevelGain > 0)
            {
                BroadcastForTamerViewsAndSelf(tamer.Id,
                    new LevelUpPacket(tamer.GeneralHandler, tamer.Level).Serialize());

                tamer.SetLevelStatus(
                    _statusManager.GetTamerLevelStatus(
                        tamer.Model,
                        tamer.Level
                    )
                );

                tamer.FullHeal();
            }

            return tamerResult;
        }

        private ReceiveExpResult ReceivePartnerExp(DigimonModel partner, MobConfigModel targetMob, long partnerExpToReceive)
        {
            var partnerResult = _expManager.ReceiveDigimonExperience(partnerExpToReceive, partner);

            _expManager.ReceiveAttributeExperience(partner, targetMob.Attribute, targetMob.Element, targetMob.ExpReward);

            partner.ReceiveSkillExp(targetMob.ExpReward.SkillExperience);

            if (partnerResult.LevelGain > 0)
            {
                partner.SetBaseStatus(
                    _statusManager.GetDigimonBaseStatus(
                        partner.CurrentType,
                        partner.Level,
                        partner.Size
                    )
                );

                BroadcastForTamerViewsAndSelf(partner.Character.Id,
                    new LevelUpPacket(partner.GeneralHandler, partner.Level).Serialize());

                partner.FullHeal();
            }

            return partnerResult;
        }
        private ReceiveExpResult ReceivePartnerExp(DigimonModel partner, SummonMobModel targetMob, long partnerExpToReceive)
        {
            var partnerResult = _expManager.ReceiveDigimonExperience(partnerExpToReceive, partner);

            _expManager.ReceiveAttributeExperience(partner, targetMob.Attribute, targetMob.Element, targetMob.ExpReward);

            partner.ReceiveSkillExp(targetMob.ExpReward.SkillExperience);

            if (partnerResult.LevelGain > 0)
            {
                partner.SetBaseStatus(
                    _statusManager.GetDigimonBaseStatus(
                        partner.CurrentType,
                        partner.Level,
                        partner.Size
                    )
                );

                BroadcastForTamerViewsAndSelf(partner.Character.Id,
                    new LevelUpPacket(partner.GeneralHandler, partner.Level).Serialize());

                partner.FullHeal();
            }

            return partnerResult;
        }

        private static int CalculateDamage(CharacterModel tamer, out double critBonusMultiplier, out bool blocked)
        {
            critBonusMultiplier = 0;

            var multiplier = 1;

            if (tamer.Partner.BaseInfo.Attribute.
                HasAttributeAdvantage(tamer.TargetMob.Attribute)
                || tamer.Partner.BaseInfo.Element
                .HasElementAdvantage(tamer.TargetMob.Element))
                multiplier = 2;

            var baseDamage = tamer.Partner.AT * multiplier;

            var random = new Random();
            // Gere um valor aleatório entre 0% e 5% a mais do valor original
            double percentagemBonus = random.NextDouble() * 0.05;

            // Calcule o valor final com o bônus
            baseDamage = (int)(baseDamage * (1.0 + percentagemBonus));

            if (baseDamage < 0) baseDamage = 0;

            critBonusMultiplier = 0.00;
            double critChance = tamer.Partner.CC / 100;


            blocked = tamer.TargetMob.BLValue >= UtilitiesFunctions.RandomDouble();

            //var levelBonusMultiplier = client.Tamer.Partner.Level > targetMob.Level ?
            //    (0.01f * (client.Tamer.Partner.Level - targetMob.Level)) : 0; //TODO: externalizar no portal

            //var attributeMultiplier = 0.00;
            //if (client.Tamer.Partner.BaseInfo.Attribute.HasAttributeAdvantage(targetMob.Attribute))
            //{
            //    var vlrAtual = client.Tamer.Partner.GetAttributeExperience();
            //    var bonusMax = 50.0; //TODO: externalizar?
            //    var expMax = 10000; //TODO: externalizar?

            //    attributeMultiplier = (bonusMax * vlrAtual) / expMax;
            //}
            //else if (targetMob.Attribute.HasAttributeAdvantage(client.Tamer.Partner.BaseInfo.Attribute))
            //{
            //    attributeMultiplier = -0.25;
            //}

            //var elementMultiplier = 0.00;
            //if (client.Tamer.Partner.BaseInfo.Element.HasElementAdvantage(targetMob.Element))
            //{
            //    var vlrAtual = client.Tamer.Partner.GetElementExperience();
            //    var bonusMax = 0.50; //TODO: externalizar?
            //    var expMax = 10000; //TODO: externalizar?

            //    elementMultiplier = (bonusMax * vlrAtual) / expMax;
            //}
            //else if (targetMob.Element.HasElementAdvantage(client.Tamer.Partner.BaseInfo.Element))
            //{
            //    elementMultiplier = -0.25;
            //}

            baseDamage /= blocked ? 2 : 1;

            if (critChance >= UtilitiesFunctions.RandomDouble() && tamer.Partner.CD > 0)
            {
                blocked = false;
                critBonusMultiplier = 1;
                return GetCurrentDamage(tamer);
            }

            return baseDamage;

            //return (int)Math.Floor(baseDamage +
            //    (baseDamage * critBonusMultiplier) +
            //    (baseDamage * levelBonusMultiplier) +
            //    (baseDamage * attributeMultiplier) +
            //    (baseDamage * elementMultiplier));
        }

        public static int GetCurrentDamage(CharacterModel tamer)
        {
            var ResultAT = tamer.Partner.AT;

            var ResultDamage = 0;
            var ResultCriticalDamage = 0;
            var ResultCriticalDamageDoubleAdvantage = 0;
            double criticalDamageValue = tamer.Partner.CD;
            var ResultDamageDoubleAdvantage = 0;
            double MultiplierAttribute = 0;

            var attributeVantage = tamer.Partner.BaseInfo.Attribute.
                HasAttributeAdvantage(tamer.TargetMob.Attribute);
            var elementVantage = tamer.Partner.BaseInfo.Element
                .HasElementAdvantage(tamer.TargetMob.Element);


            ResultDamage = ResultAT + (int)Math.Floor((double)0);
            double addedCriticalDamage = ResultDamage * 0.8;
            addedCriticalDamage *= (1 + ((criticalDamageValue + 0) / 100.0));
            ResultCriticalDamage = ResultDamage + (int)Math.Floor(addedCriticalDamage);

            if (tamer.Partner.AttributeExperience.CurrentAttributeExperience && attributeVantage)
            {

                MultiplierAttribute = (2 + ((tamer.Partner.ATT) / 200.0));

                ResultDamageDoubleAdvantage = (int)Math.Floor(0 + (MultiplierAttribute * ResultAT));

            }
            else if (tamer.Partner.AttributeExperience.CurrentElementExperience && elementVantage)
            {
                MultiplierAttribute = 2;
                ResultDamageDoubleAdvantage = (int)Math.Floor(0 + (MultiplierAttribute * ResultAT));
            }

            double FinalValue = ResultCriticalDamage + ResultDamageDoubleAdvantage;

            Random random = new Random();

            // Gere um valor aleatório entre 0% e 5% a mais do valor original
            double percentagemBonus = random.NextDouble() * 0.05;

            // Calcule o valor final com o bônus
            return (int)(FinalValue * (1.0 + percentagemBonus));

        }
        private void CheckMonthlyReward(GameClient client)
        {
            if (client.Tamer.AttendanceReward.ReedemRewards)
            {
                //client.Tamer.AttendanceReward.SetLastRewardDate();
                //client.Tamer.AttendanceReward.IncreaseTotalDays();
                //ReedemReward(client);
            }


            client.Send(new TamerAttendancePacket(client.Tamer.AttendanceReward.TotalDays));

        }

        private void ReedemReward(GameClient client)
        {
            var rewardInfo = _assets.MonthlyEvents.FirstOrDefault(x => x.CurrentDay == client.Tamer.AttendanceReward.TotalDays);

            if (rewardInfo != null)
            {
                var newItem = new ItemModel();
                newItem.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == rewardInfo.ItemId));

                if (newItem.ItemInfo == null)
                {
                    //_logger.Warning($"No item info found with ID {itemId} for tamer {client.TamerId}.");
                    //client.Send(new SystemMessagePacket($"No item info found with ID {itemId}."));
                    return;
                }

                newItem.ItemId = rewardInfo.ItemId;
                newItem.Amount = rewardInfo.ItemCount;

                if (newItem.IsTemporary)
                    newItem.SetRemainingTime((uint)newItem.ItemInfo.UsageTimeMinutes);

                var itemClone = (ItemModel)newItem.Clone();

                if (client.Tamer.AccountCashWarehouse.AddItem(newItem))
                {
                    _sender.Send(new UpdateItemsCommand(client.Tamer.AccountCashWarehouse));
                }
            }

            _sender.Send(new UpdateTamerAttendanceRewardCommand(client.Tamer.AttendanceReward));
        }
    }
}