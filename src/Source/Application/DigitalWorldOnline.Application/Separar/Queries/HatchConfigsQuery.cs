﻿using MediatR;
using DigitalWorldOnline.Commons.DTOs.Config;

namespace DigitalWorldOnline.Application.Separar.Queries
{
    public class HatchConfigsQuery : IRequest<List<HatchConfigDTO>>
    {
    }
}