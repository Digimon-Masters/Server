using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Models.Map;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.MapServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Game.Managers;

using System.Text;

namespace DigitalWorldOnline.GameHost.EventsServer
{
    public sealed partial class EventServer
    {
        public Task TamerOperation(GameMap map)
        {
            if (!map.ConnectedTamers.Any())
            {
                map.SetNoTamers();
                return Task.CompletedTask;
            }

            foreach (var tamer in map.ConnectedTamers)
            {
                var client = map.Clients.FirstOrDefault(x => x.TamerId == tamer.Id);

                if (client == null || !client.IsConnected || client.Partner == null)
                    continue;

                GetInViewMobs(map, tamer);

                ShowOrHideTamer(map, tamer);

                PartnerAutoAttack(tamer);

                tamer.AutoRegen();
                tamer.ActiveEvolutionReduction();

              
                if (tamer.BreakEvolution)
                {
                    Console.WriteLine($"TamerOperation BreakEvolution Start");
                    tamer.ActiveEvolution.SetDs(0);
                    tamer.ActiveEvolution.SetXg(0);

                    map.BroadcastForTamerViewsAndSelf(tamer.Id,
                        new DigimonEvolutionSucessPacket(tamer.GeneralHandler,
                        tamer.Partner.GeneralHandler,
                        tamer.Partner.BaseType,
                        DigimonEvolutionEffectEnum.Back).Serialize());

                    //TODO: Ride mode
                    //client.Send(new StopRideMode(Tamer.Handle, Tamer.Partner.Handle).ToArray());
                    //tamer.RideMode = false;

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
                    _sender.Send(new UpdateCharacterBasicInfoCommand(client.Tamer));
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
                        }
                    }
                }

                if (tamer.SyncResourcesTime)
                {
                    tamer.UpdateSyncResourcesTime();

                    client?.Send(new UpdateCurrentResourcesPacket(tamer.GeneralHandler, (short)tamer.CurrentHp, (short)tamer.CurrentDs, (short)CharacterModel.Fatigue));
                    client?.Send(new UpdateCurrentResourcesPacket(tamer.Partner.GeneralHandler, (short)tamer.Partner.CurrentHp, (short)tamer.Partner.CurrentDs, 0));
                    map.BroadcastForTargetTamers(map.TamersView[tamer.Id], new UpdateCurrentHPRatePacket(tamer.GeneralHandler, tamer.HpRate).Serialize());
                    map.BroadcastForTargetTamers(map.TamersView[tamer.Id], new UpdateCurrentHPRatePacket(tamer.Partner.GeneralHandler, tamer.Partner.HpRate).Serialize());
                    map.BroadcastForTamerViewsAndSelf(tamer.Id, new SyncConditionPacket(tamer.GeneralHandler, tamer.CurrentCondition, tamer.ShopName).Serialize());
                }
            }

            return Task.CompletedTask;
        }

        private void GetInViewMobs(GameMap map, CharacterModel tamer)
        {
            map.Mobs.ForEach(mob =>
            {
                var distanceDifference = UtilitiesFunctions.CalculateDistance(
                    tamer.Location.X,
                    mob.CurrentLocation.X,
                    tamer.Location.Y,
                    mob.CurrentLocation.Y);

                if (distanceDifference <= _startToSee && !tamer.MobsInView.Contains(mob.Id))
                    tamer.MobsInView.Add(mob.Id);

                if (distanceDifference >= _stopSeeing && tamer.MobsInView.Contains(mob.Id))
                    tamer.MobsInView.Remove(mob.Id);
            });
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
#if DEBUG
                    var serialized = SerializeShowTamer(tamerToShow);
                    File.WriteAllText($"Shows\\Show{tamerToShow.Id}To{tamerToSeeId}_{DateTime.Now:dd_MM_yy_HH_mm_ss}.temp", serialized);
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
                    File.WriteAllText($"Hides\\Hide{tamerToHide.Id}To{tamerToBlindId}_{DateTime.Now:dd_MM_yy_HH_mm_ss}.temp", serialized);
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

            if (!tamer.Partner.IsAttacking && tamer.TargetMob != null && tamer.TargetMob.Alive)
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

                if (!tamer.GodMode && tamer.Partner.Level <= tamer.TargetMob.Level)
                {
                    missed = tamer.CanMissHit();
                }

                if (missed)
                {
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
                        BroadcastForTamerViewsAndSelf(
                            tamer.Id,
                            new HitPacket(
                                tamer.Partner.GeneralHandler,
                                tamer.TargetMob.GeneralHandler,
                                finalDmg,
                                tamer.TargetMob.HPValue,
                                newHp,
                                hitType).Serialize());

                        _logger.Verbose($"Character {tamer.Id} inflicted {finalDmg} to mob {tamer.TargetMob?.Id} - {tamer.TargetMob?.Name}.");
                    }
                    else
                    {
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

                        _logger.Verbose($"Character {tamer.Id} - {tamer.Name} killed mob {tamer.TargetMob?.Id} - {tamer.TargetMob?.Name} with {finalDmg} damage.");
                    }

                }

                tamer.Partner.UpdateLastHitTime();
            }

            if (tamer.TargetMob == null || !tamer.TargetMob.Alive)
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

        private static int CalculateDamage(CharacterModel tamer, out double critBonusMultiplier, out bool blocked)
        {
            var baseDamage = tamer.Partner.AT - tamer.TargetMob.DEValue + UtilitiesFunctions.RandomInt(1, 15);
            if (baseDamage < 0) baseDamage = 0;

            critBonusMultiplier = 0.00;
            double critChance = tamer.Partner.CC / 100;
            if (critChance >= UtilitiesFunctions.RandomDouble())
                critBonusMultiplier = tamer.Partner.CD;

            blocked = tamer.TargetMob.BLValue >= UtilitiesFunctions.RandomDouble();
            var levelBonusMultiplier = tamer.Partner.Level > tamer.TargetMob.Level ?
                (0.01f * (tamer.Partner.Level - tamer.TargetMob.Level)) : 0; //TODO: externalizar no portal

            var attributeMultiplier = 0.00;
            if (tamer.Partner.BaseInfo.Attribute.HasAttributeAdvantage(tamer.TargetMob.Attribute))
            {
                var vlrAtual = tamer.Partner.GetAttributeExperience();
                var bonusMax = 50.0; //TODO: externalizar?
                var expMax = 10000; //TODO: externalizar?

                attributeMultiplier = (bonusMax * vlrAtual) / expMax;
            }
            else if (tamer.TargetMob.Attribute.HasAttributeAdvantage(tamer.Partner.BaseInfo.Attribute))
            {
                attributeMultiplier = -0.25;
            }

            var elementMultiplier = 0.00;
            if (tamer.Partner.BaseInfo.Element.HasElementAdvantage(tamer.TargetMob.Element))
            {
                var vlrAtual = tamer.Partner.GetElementExperience();
                var bonusMax = 0.5; //TODO: externalizar?
                var expMax = 10000; //TODO: externalizar?

                elementMultiplier = (bonusMax * vlrAtual) / expMax;
            }
            else if (tamer.TargetMob.Element.HasElementAdvantage(tamer.Partner.BaseInfo.Element))
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