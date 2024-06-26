﻿using AutoMapper;
using DigitalWorldOnline.Commons.DTOs.Events;
using DigitalWorldOnline.Commons.Models.Mechanics;


namespace DigitalWorldOnline.Infraestructure.Mapping
{
    public class ArenaProfile : Profile
    {
        public ArenaProfile()
        {
            CreateMap<ArenaRankingModel, ArenaRankingDTO>()
                .ReverseMap();


            CreateMap<ArenaRankingCompetitorModel, ArenaRankingCompetitorDTO>()
                .ReverseMap();
        }
    }
}