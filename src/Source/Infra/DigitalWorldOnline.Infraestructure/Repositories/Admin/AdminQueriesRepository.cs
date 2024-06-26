﻿using DigitalWorldOnline.Application.Admin.Queries;
using DigitalWorldOnline.Application.Admin.Repositories;
using DigitalWorldOnline.Commons.DTOs.Assets;
using DigitalWorldOnline.Commons.Enums.Admin;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DigitalWorldOnline.Infraestructure.Repositories.Admin
{
    public class AdminQueriesRepository : IAdminQueriesRepository
    {
        private readonly DatabaseContext _context;

        public AdminQueriesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<GetAccountByIdQueryDto> GetAccountByIdAsync(long id)
        {
            var result = new GetAccountByIdQueryDto();

            result.Register = await _context.Account
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetAccountsQueryDto> GetAccountsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetAccountsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Desc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.Account
                    .AsNoTracking()
                    .Where(x => x.Username.Contains(filter) || x.Email.Contains(filter))
                    .CountAsync();

                result.Registers = await _context.Account
                    .AsNoTracking()
                    .Where(x => x.Username.Contains(filter) || x.Email.Contains(filter))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.Account
                    .AsNoTracking()
                    .CountAsync();

                result.Registers = await _context.Account
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetMapsQueryDto> GetMapsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetMapsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.MapConfig
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(filter) || x.MapId.ToString().Contains(filter))
                    .CountAsync();

                result.Registers = await _context.MapConfig
                    .AsNoTracking()
                    .Include(x => x.Mobs)
                    .Where(x => x.Name.Contains(filter) || x.MapId.ToString().Contains(filter))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.MapConfig
                    .AsNoTracking()
                    .CountAsync();

                result.Registers = await _context.MapConfig
                    .AsNoTracking()
                    .Include(x => x.Mobs)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetMobsQueryDto> GetMobsAsync(long mapId, int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetMobsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Desc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.MobConfig
                    .AsNoTracking()
                    .Where(x => x.GameMapConfigId == mapId && (x.Name.Contains(filter) || x.Type.ToString().Contains(filter)))
                    .CountAsync();

                result.Registers = await _context.MobConfig
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .Include(x => x.ExpReward)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.Drops)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.BitsDrop)
                    .Where(x => x.GameMapConfigId == mapId && (x.Name.Contains(filter) || x.Type.ToString().Contains(filter)))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.MobConfig
                    .AsNoTracking()
                    .Where(x => x.GameMapConfigId == mapId)
                    .CountAsync();

                result.Registers = await _context.MobConfig
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .Include(x => x.ExpReward)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.Drops)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.BitsDrop)
                    .Where(x => x.GameMapConfigId == mapId)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetRaidsQueryDto> GetRaidsAsync(long mapId, int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetRaidsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Desc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.MobConfig
                    .AsNoTracking()
                    .Where(x => x.GameMapConfigId == mapId && x.Class == 8 && (x.Name.Contains(filter) || x.Type.ToString().Contains(filter)))
                    .CountAsync();

                result.Registers = await _context.MobConfig
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .Include(x => x.ExpReward)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.Drops)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.BitsDrop)
                    .Where(x => x.GameMapConfigId == mapId && x.Class == 8 && (x.Name.Contains(filter) || x.Type.ToString().Contains(filter)))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.MobConfig
                    .AsNoTracking()
                    .Where(x => x.GameMapConfigId == mapId)
                    .CountAsync();

                result.Registers = await _context.MobConfig
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .Include(x => x.ExpReward)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.Drops)
                    .Include(x => x.DropReward)
                        .ThenInclude(y => y.BitsDrop)
                    .Where(x => x.GameMapConfigId == mapId && x.Class == 8)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetMapByIdQueryDto> GetMapByIdAsync(long id)
        {
            var result = new GetMapByIdQueryDto();

            result.Register = await _context.MapConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetServerByIdQueryDto> GetServerByIdAsync(long id)
        {
            var result = new GetServerByIdQueryDto();

            result.Register = await _context.ServerConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetServersQueryDto> GetServersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetServersQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Desc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.ServerConfig
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(filter))
                    .CountAsync();

                result.Registers = await _context.ServerConfig
                    .AsNoTracking()
                    .Where(x => x.Name.Contains(filter))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.ServerConfig
                    .AsNoTracking()
                    .CountAsync();

                result.Registers = await _context.ServerConfig
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetUserByIdQueryDto> GetUserByIdAsync(long id)
        {
            var result = new GetUserByIdQueryDto();

            result.Register = await _context.UserConfig
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetUsersQueryDto> GetUsersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string filter)
        {
            var result = new GetUsersQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Desc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.UserConfig
                    .AsNoTracking()
                    .Where(x => x.Id > 1 && x.Username.Contains(filter))
                    .CountAsync();

                result.Registers = await _context.UserConfig
                    .AsNoTracking()
                    .Where(x => x.Id > 1 && x.Username.Contains(filter))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.UserConfig
                    .AsNoTracking()
                    .Where(x => x.Id > 1)
                    .CountAsync();

                result.Registers = await _context.UserConfig
                    .AsNoTracking()
                    .Where(x => x.Id > 1)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetMobByIdQueryDto> GetMobByIdAsync(long id)
        {
            var result = new GetMobByIdQueryDto();

            result.Register = await _context.MobConfig
                .AsNoTracking()
                .Include(x => x.Location)
                .Include(x => x.ExpReward)
                .Include(x => x.DropReward)
                    .ThenInclude(y => y.Drops)
                .Include(x => x.DropReward)
                    .ThenInclude(y => y.BitsDrop)
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetMobAssetQueryDto> GetMobAssetAsync(string filter)
        {
            var result = new GetMobAssetQueryDto();

            result.Registers = await _context.MonsterBaseInfoAsset
                .AsNoTracking()
                .Where(x =>
                //x.Class != 8 && 
                (x.Type.ToString().Contains(filter) || x.Name.Contains(filter)))
                .ToListAsync();

            return result;
        }

        public async Task<GetRaidAssetQueryDto> GetRaidBossAssetAsync(string filter)
        {
            var result = new GetRaidAssetQueryDto();

            result.Registers = await _context.MonsterBaseInfoAsset
                .AsNoTracking()
                .Where(x => x.Class == 8 && (x.Type.ToString().Contains(filter) || x.Name.Contains(filter)))
                .ToListAsync();

            return result;
        }

        public async Task<GetItemAssetQueryDto> GetItemAssetAsync(string filter)
        {
            var result = new GetItemAssetQueryDto();

            result.Registers = await _context.ItemAsset
                .AsNoTracking()
                .Where(x => x.ItemId.ToString().Contains(filter) || x.Name.Contains(filter))
                .ToListAsync();

            return result;
        }

        public async Task<GetItemAssetByIdQueryDto> GetItemAssetByIdAsync(int id)
        {
            var result = new GetItemAssetByIdQueryDto();

            result.Register = await _context.ItemAsset
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ItemId == id);

            return result;
        }

        public async Task<GetSpawnPointsQueryDto> GetSpawnPointsAssetAsync(int mapId, int limit, int offset, string sortColumn, SortDirectionEnum sortDirection)
        {
            var result = new GetSpawnPointsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            var dto = await _context.MapRegionListAsset
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MapId == mapId);

            if (dto != null)
            {
                result.TotalRegisters = await _context.MapRegionAsset
                    .AsNoTracking()
                    .CountAsync(x => x.MapRegionListId == dto.Id);

                result.Registers = await _context.MapRegionAsset
                    .AsNoTracking()
                    .Where(x => x.MapRegionListId == dto.Id)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = 0;
                result.Registers = new List<MapRegionAssetDTO>();
            }

            return result;
        }

        public async Task<GetSpawnPointByIdQueryDto> GetSpawnPointByIdAsync(long id)
        {
            var result = new GetSpawnPointByIdQueryDto();

            var dto = await _context.MapRegionAsset
               .AsNoTracking()
               .Include(x => x.MapRegionList)
               .SingleOrDefaultAsync(x => x.Id == id);

            if (dto != null)
            {
                result.Register = dto;
                var mapDto = await _context.MapConfig.SingleAsync(x => x.MapId == dto.MapRegionList.MapId);
                result.MapId = mapDto.Id;
                result.MapName = mapDto.Name;
            }

            return result;
        }

        public async Task<GetScansQueryDto> GetScansAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetScansQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.ScanDetail
                    .AsNoTracking()
                    .Where(x => x.ItemName.Contains(filter) || x.ItemId.ToString().Contains(filter))
                    .CountAsync();

                result.Registers = await _context.ScanDetail
                    .AsNoTracking()
                    .Include(x => x.Rewards)
                    .Where(x => x.ItemName.Contains(filter) || x.ItemId.ToString().Contains(filter))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.ScanDetail
                    .AsNoTracking()
                    .CountAsync();

                result.Registers = await _context.ScanDetail
                    .AsNoTracking()
                    .Include(x => x.Rewards)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetScanByIdQueryDto> GetScanByIdAsync(long id)
        {
            var result = new GetScanByIdQueryDto();

            result.Register = await _context.ScanDetail
               .AsNoTracking()
               .Include(x => x.Rewards)
               .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetContainersQueryDto> GetContainersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetContainersQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.Container
                    .AsNoTracking()
                    .Where(x => x.ItemName.Contains(filter) || x.ItemId.ToString().Contains(filter))
                    .CountAsync();

                result.Registers = await _context.Container
                    .AsNoTracking()
                    .Include(x => x.Rewards)
                    .Where(x => x.ItemName.Contains(filter) || x.ItemId.ToString().Contains(filter))
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.Container
                    .AsNoTracking()
                    .CountAsync();

                result.Registers = await _context.Container
                    .AsNoTracking()
                    .Include(x => x.Rewards)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }

        public async Task<GetContainerByIdQueryDto> GetContainerByIdAsync(long id)
        {
            var result = new GetContainerByIdQueryDto();

            result.Register = await _context.Container
               .AsNoTracking()
               .Include(x => x.Rewards)
               .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetClonsQueryDto> GetClonsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetClonsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            result.TotalRegisters = await _context.CloneConfig
                    .AsNoTracking()
                    .CountAsync();

            result.Registers = await _context.CloneConfig
                .AsNoTracking()
                .Skip(offset)
                .Take(limit)
                .OrderBy($"{sortColumn} {sortDirection}")
                .ToListAsync();

            return result;
        }

        public async Task<GetClonByIdQueryDto> GetClonByIdAsync(long id)
        {
            var result = new GetClonByIdQueryDto();

            result.Register = await _context.CloneConfig
               .AsNoTracking()
               .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetHatchConfigByIdQueryDto> GetHatchConfigByIdAsync(long id)
        {
            var result = new GetHatchConfigByIdQueryDto();

            result.Register = await _context.HatchConfig
               .AsNoTracking()
               .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<GetHatchConfigsQueryDto> GetHatchConfigsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetHatchConfigsQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            result.TotalRegisters = await _context.HatchConfig
                .AsNoTracking()
                .CountAsync();

            result.Registers = await _context.HatchConfig
                .AsNoTracking()
                .Skip(offset)
                .Take(limit)
                .OrderBy($"{sortColumn} {sortDirection}")
                .ToListAsync();

            return result;
        }

        public async Task<GetPlayersQueryDto> GetPlayersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter)
        {
            var result = new GetPlayersQueryDto();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Id";

            if (filter?.Length < 3)
                filter = string.Empty;

            if (sortDirection == SortDirectionEnum.None)
                sortDirection = SortDirectionEnum.Asc;

            if (!string.IsNullOrEmpty(filter))
            {
                result.TotalRegisters = await _context.Character
                    .AsNoTracking()
                    .Where(x=> x.Id.ToString().Contains(filter) || x.Name.Contains(filter))
                    .CountAsync();

                result.Registers = await _context.Character
                    .AsNoTracking()
                    .Where(x => x.Id.ToString().Contains(filter) || x.Name.Contains(filter))
                    .Include(x => x.Location)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }
            else
            {
                result.TotalRegisters = await _context.Character
                    .AsNoTracking()
                    .CountAsync();

                result.Registers = await _context.Character
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .Skip(offset)
                    .Take(limit)
                    .OrderBy($"{sortColumn} {sortDirection}")
                    .ToListAsync();
            }

            return result;
        }
    }
}
