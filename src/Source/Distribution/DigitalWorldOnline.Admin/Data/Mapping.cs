using AutoMapper;
using DigitalWorldOnline.Commons.DTOs.Assets;
using DigitalWorldOnline.Commons.DTOs.Config;
using DigitalWorldOnline.Commons.ViewModel.Asset;
using DigitalWorldOnline.Commons.ViewModel.Mobs;

namespace DigitalWorldOnline.Admin.Data
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<MonsterBaseInfoAssetDTO, MobAssetViewModel>();
            CreateMap<MobAssetViewModel, MobCreationViewModel>();
            CreateMap<MobAssetViewModel, MobUpdateViewModel>();
            CreateMap<MobCreationViewModel, MobConfigDTO>();
            CreateMap<MobLocationViewModel, MobLocationConfigDTO>();
            CreateMap<MobExpRewardViewModel, MobExpRewardConfigDTO>();
            CreateMap<MobDropRewardViewModel, MobDropRewardConfigDTO>();
            CreateMap<MobItemDropViewModel, ItemDropConfigDTO>();
            CreateMap<MobBitDropViewModel, BitsDropConfigDTO>();
            CreateMap<ItemAssetDTO, ItemAssetViewModel>();
            CreateMap<MobConfigDTO, MobUpdateViewModel>()
                .ReverseMap();
        }
    }
}
