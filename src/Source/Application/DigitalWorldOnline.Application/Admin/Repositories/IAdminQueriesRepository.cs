﻿using DigitalWorldOnline.Application.Admin.Queries;
using DigitalWorldOnline.Commons.Enums.Admin;

namespace DigitalWorldOnline.Application.Admin.Repositories
{
    public interface IAdminQueriesRepository
    {
        Task<GetAccountByIdQueryDto> GetAccountByIdAsync(long id);

        Task<GetAccountsQueryDto> GetAccountsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetMapsQueryDto> GetMapsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetMobsQueryDto> GetMobsAsync(long mapId, int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetRaidsQueryDto> GetRaidsAsync(long mapId, int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);
        
        Task<GetMapByIdQueryDto> GetMapByIdAsync(long id);

        Task<GetServerByIdQueryDto> GetServerByIdAsync(long id);

        Task<GetServersQueryDto> GetServersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetUserByIdQueryDto> GetUserByIdAsync(long id);

        Task<GetUsersQueryDto> GetUsersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string filter);

        Task<GetMobByIdQueryDto> GetMobByIdAsync(long id);

        Task<GetMobAssetQueryDto> GetMobAssetAsync(string filter);

        Task<GetRaidAssetQueryDto> GetRaidBossAssetAsync(string filter);

        Task<GetSpawnPointsQueryDto> GetSpawnPointsAssetAsync(int mapId, int limit, int offset, string sortColumn, SortDirectionEnum sortDirection);

        Task<GetItemAssetQueryDto> GetItemAssetAsync(string filter);

        Task<GetItemAssetByIdQueryDto> GetItemAssetByIdAsync(int id);

        Task<GetSpawnPointByIdQueryDto> GetSpawnPointByIdAsync(long id);

        Task<GetScansQueryDto> GetScansAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetScanByIdQueryDto> GetScanByIdAsync(long id);

        Task<GetContainersQueryDto> GetContainersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetContainerByIdQueryDto> GetContainerByIdAsync(long id);

        Task<GetClonsQueryDto> GetClonsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetClonByIdQueryDto> GetClonByIdAsync(long id);
        
        Task<GetHatchConfigByIdQueryDto> GetHatchConfigByIdAsync(long id);

        Task<GetHatchConfigsQueryDto> GetHatchConfigsAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);

        Task<GetPlayersQueryDto> GetPlayersAsync(int limit, int offset, string sortColumn, SortDirectionEnum sortDirection, string? filter);
    }
}