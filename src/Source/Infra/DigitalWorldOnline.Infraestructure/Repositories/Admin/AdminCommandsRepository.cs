﻿using AutoMapper;
using DigitalWorldOnline.Application.Admin.Commands;
using DigitalWorldOnline.Commons.DTOs.Account;
using DigitalWorldOnline.Commons.DTOs.Assets;
using DigitalWorldOnline.Commons.DTOs.Character;
using DigitalWorldOnline.Commons.DTOs.Config;
using DigitalWorldOnline.Commons.DTOs.Server;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.Account;
using DigitalWorldOnline.Commons.Models.Account;
using DigitalWorldOnline.Commons.Repositories.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure.Repositories.Admin
{
    public class AdminCommandsRepository : IAdminCommandsRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public AdminCommandsRepository(
            DatabaseContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDTO> AddAccountAsync(AccountDTO account)
        {
            _context.Account.Add(account);

            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<ContainerAssetDTO> AddContainerConfigAsync(ContainerAssetDTO container)
        {
            _context.Container.Add(container);

            await _context.SaveChangesAsync();

            return container;
        }

        public async Task<MobConfigDTO> AddMobAsync(MobConfigDTO mob)
        {
            var targetMap = await _context.MapConfig
                .SingleAsync(x => x.Id == mob.GameMapConfigId);

            mob.Location.MapId = (short)targetMap.MapId;
            mob.DropReward?.Drops.ForEach(drop => drop.Id = 0);

            _context.MobConfig.Add(mob);
            await _context.SaveChangesAsync();

            return mob;
        }

        public async Task<ScanDetailAssetDTO> AddScanConfigAsync(ScanDetailAssetDTO scan)
        {
            _context.ScanDetail.Add(scan);

            await _context.SaveChangesAsync();

            return scan;
        }

        public async Task<ServerDTO> AddServerAsync(ServerDTO server)
        {
            _context.ServerConfig.Add(server);

            await _context.SaveChangesAsync();

            return server;
        }

        public async Task<MapRegionAssetDTO> AddSpawnPointAsync(MapRegionAssetDTO spawnPoint, int mapId)
        {
            var mapRegionList = await _context.MapRegionListAsset
                .AsNoTracking()
                .Include(x => x.Regions)
                .SingleOrDefaultAsync(x => x.MapId == mapId);

            if (mapRegionList != null)
            {
                spawnPoint.MapRegionListId = mapRegionList.Id;

                mapRegionList.Regions.Add(spawnPoint);

                _context.Update(mapRegionList);

                _context.MapRegionAsset.Add(spawnPoint);

                await _context.SaveChangesAsync();
            }

            return spawnPoint;
        }

        public async Task<UserDTO> AddUserAsync(UserDTO user)
        {
            _context.UserConfig.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAccountAsync(long id)
        {
            var dto = await _context.Account
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.RemoveRange(
                    await _context.Character
                    .Where(x => x.AccountId == id)
                    .ToListAsync()
                );

                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteContainerConfigAsync(long id)
        {
            var dto = await _context.Container
                .AsNoTracking()
                .Include(x => x.Rewards)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteMapMobsAsync(long id)
        {
            var dto = await _context.MapConfig
                .AsNoTracking()
                .Include(x => x.Mobs)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.RemoveRange(dto.Mobs);

                _context.SaveChanges();
            }
        }

        public async Task DeleteMobAsync(long id)
        {
            var dto = await _context.MobConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteScanConfigAsync(long id)
        {
            var dto = await _context.ScanDetail
                .AsNoTracking()
                .Include(x => x.Rewards)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteServerAsync(long id)
        {
            var dto = await _context.ServerConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteSpawnPointAsync(long id)
        {
            var dto = await _context.MapRegionAsset
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteUserAsync(long id)
        {
            var dto = await _context.UserConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task DuplicateMobAsync(long id)
        {
            var dto = await _context.MobConfig
                .AsNoTracking()
                .Include(x => x.Location)
                .Include(x => x.ExpReward)
                .Include(x => x.DropReward)
                    .ThenInclude(y => y.Drops)
                .Include(x => x.DropReward)
                    .ThenInclude(y => y.BitsDrop)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                var clonedEntity = (MobConfigDTO)dto.Clone();
                clonedEntity.Id = 0;

                if (clonedEntity.Location == null)
                    clonedEntity.Location = new MobLocationConfigDTO();
                else
                    clonedEntity.Location.Id = 0;

                if (clonedEntity.ExpReward == null)
                    clonedEntity.ExpReward = new MobExpRewardConfigDTO();
                else
                    clonedEntity.ExpReward.Id = 0;

                if (clonedEntity.DropReward == null)
                    clonedEntity.DropReward = new MobDropRewardConfigDTO();
                else
                {
                    clonedEntity.DropReward.Id = 0;
                    clonedEntity.DropReward.Drops.ToList().ForEach(drop => drop.Id = 0);
                    clonedEntity.DropReward.BitsDrop.Id = 0;
                }

                _context.Add(clonedEntity);

                _context.SaveChanges();
            }
        }

        public async Task UpdateAccountAsync(AccountDTO account)
        {
            var dto = await _context.Account
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == account.Id);

            if (dto != null)
            {
                dto.Username = account.Username;
                dto.Email = account.Email;
                dto.Premium = account.Premium;
                dto.Silk = account.Silk;
                dto.AccessLevel = account.AccessLevel;

                _context.Update(dto);

                _context.SaveChanges();
            }
        }

        public async Task UpdateScanConfigAsync(ScanDetailAssetDTO scan)
        {
            var dto = await _context.ScanDetail
                .AsNoTracking()
                .Include(x => x.Rewards)
                .SingleOrDefaultAsync(x => x.Id == scan.Id);

            if (dto == null)
            {
                _context.Add(scan);
            }
            else
            {
                var parameterIds = scan.Rewards.Select(x => x.Id);
                var removeItems = dto.Rewards.Where(x => !parameterIds.Contains(x.Id));
                foreach (var removeItem in removeItems)
                {
                    _context.Remove(removeItem);
                }

                var databaseIds = dto.Rewards.Select(x => x.Id);
                var newItems = scan.Rewards.Where(x => !databaseIds.Contains(x.Id));
                foreach (var newItem in newItems)
                {
                    newItem.Id = 0;
                    newItem.ScanDetailAssetId = dto.Id;

                    _context.Add(newItem);
                }

                dto.Rewards = scan.Rewards;
                dto.ItemId = scan.ItemId;
                dto.ItemName = scan.ItemName;
                dto.MinAmount = scan.MinAmount;
                dto.MaxAmount = scan.MaxAmount;

                _context.Update(dto);
            }

            _context.SaveChanges();
        }

        public async Task UpdateContainerConfigAsync(ContainerAssetDTO container)
        {
            var dto = await _context.Container
                .AsNoTracking()
                .Include(x => x.Rewards)
                .SingleOrDefaultAsync(x => x.Id == container.Id);

            if (dto == null)
            {
                _context.Add(container);
            }
            else
            {
                var parameterIds = container.Rewards.Select(x => x.Id);
                var removeItems = dto.Rewards.Where(x => !parameterIds.Contains(x.Id));
                foreach (var removeItem in removeItems)
                {
                    _context.Remove(removeItem);
                }

                var databaseIds = dto.Rewards.Select(x => x.Id);
                var newItems = container.Rewards.Where(x => !databaseIds.Contains(x.Id));
                foreach (var newItem in newItems)
                {
                    newItem.Id = 0;
                    newItem.ContainerAssetId = dto.Id;

                    _context.Add(newItem);
                }

                dto.Rewards = container.Rewards;
                dto.ItemId = container.ItemId;
                dto.ItemName = container.ItemName;
                dto.RewardAmount = container.RewardAmount;

                _context.Update(dto);
            }

            _context.SaveChanges();
        }

        public async Task UpdateServerAsync(ServerDTO server)
        {
            var dto = await _context.ServerConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == server.Id);

            if (dto != null)
            {
                dto.Name = server.Name;
                dto.Experience = server.Experience;
                dto.Maintenance = server.Maintenance;
                dto.New = dto.CreateDate.AddDays(7) >= DateTime.Now;
                dto.Type = server.Type;
                dto.Port = server.Port;

                _context.Update(dto);

                _context.SaveChanges();
            }
        }

        public async Task UpdateSpawnPointAsync(MapRegionAssetDTO spawnPoint, long mapId)
        {
            var dto = await _context.MapRegionAsset
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == spawnPoint.Id);

            if (dto != null)
            {
                dto.X = spawnPoint.X;
                dto.Y = spawnPoint.Y;
                dto.Index = spawnPoint.Index;
                dto.Name = spawnPoint.Name;

                _context.Update(dto);

                _context.SaveChanges();
            }
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            var dto = await _context.UserConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            if (dto != null)
            {
                dto.Username = user.Username;
                dto.AccessLevel = user.AccessLevel;

                _context.Update(dto);

                _context.SaveChanges();
            }
        }

        public async Task DeleteCloneConfigAsync(long id)
        {
            var dto = await _context.CloneConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task<CloneConfigDTO> AddCloneConfigAsync(CloneConfigDTO clone)
        {
            _context.CloneConfig.Add(clone);

            await _context.SaveChangesAsync();

            return clone;
        }

        public async Task UpdateCloneConfigAsync(CloneConfigDTO clone)
        {
            var dto = await _context.CloneConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == clone.Id);

            if (dto == null)
            {
                _context.Add(clone);
            }
            else
            {
                dto.Type = clone.Type;
                dto.Level = clone.Level;
                dto.SuccessChance = clone.SuccessChance;
                dto.BreakChance = clone.BreakChance;
                dto.MinAmount = clone.MinAmount;
                dto.MaxAmount = clone.MaxAmount;

                _context.Update(dto);
            }

            _context.SaveChanges();
        }

        public async Task<HatchConfigDTO> AddHatchConfigAsync(HatchConfigDTO hatch)
        {
            _context.HatchConfig.Add(hatch);

            await _context.SaveChangesAsync();

            return hatch;
        }

        public async Task DeleteHatchConfigAsync(long id)
        {
            var dto = await _context.HatchConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                _context.Remove(dto);

                _context.SaveChanges();
            }
        }

        public async Task UpdateHatchConfigAsync(HatchConfigDTO hatch)
        {
            var dto = await _context.HatchConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == hatch.Id);

            if (dto != null)
            {
                dto.Type = hatch.Type;
                dto.SuccessChance = hatch.SuccessChance;
                dto.BreakChance = hatch.BreakChance;

                _context.Update(dto);

                _context.SaveChanges();
            }
        }

        public async Task<AccountCreateResult> CreateAccountAsync(string username, string email, string discordId, string password)
        {
            var existentAccount = await _context.Account
                .FirstOrDefaultAsync(x =>
                    x.Username == username ||
                    x.Email == email ||
                    x.DiscordId == discordId);

            if (existentAccount != null)
            {
                if (existentAccount.Username == username)
                    return AccountCreateResult.UsernameInUse;

                if (existentAccount.Email == email)
                    return AccountCreateResult.EmailInUse;

                if (existentAccount.DiscordId == discordId)
                    return AccountCreateResult.DiscordInUse;
            }

            var dto = _mapper.Map<AccountDTO>(AccountModel.Create(username, email, discordId, password));

            _context.Add(dto);
            _context.SaveChanges();

            return AccountCreateResult.Created;
        }


        
    }
}
