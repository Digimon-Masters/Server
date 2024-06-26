﻿using DigitalWorldOnline.Commons.DTOs.Config;

namespace DigitalWorldOnline.Commons.Interfaces
{
    public interface IConfigQueriesRepository
    {
        Task<IList<MapConfigDTO>> GetGameMapConfigsAsync();

        Task<IList<MobConfigDTO>> GetMapMobsConfigAsync(long configId);

        Task<MapConfigDTO?> GetGameMapConfigByMapIdAsync(int mapId);

        Task<MapConfigDTO?> GetGameMapConfigByIdAsync(long id);

        Task<List<MobConfigDTO>> GetMapMobsByIdAsync(int mapId);
    }
}