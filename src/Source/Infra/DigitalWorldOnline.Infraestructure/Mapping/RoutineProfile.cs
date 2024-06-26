using AutoMapper;
using DigitalWorldOnline.Commons.DTOs.Routine;
using DigitalWorldOnline.Models.DTOs.Routine;

namespace DigitalWorldOnline.Infraestructure.Mapping
{
    public class RoutineProfile : Profile
    {
        public RoutineProfile()
        {
            CreateMap<RoutineDTO, RoutineModel>();
        }
    }
}