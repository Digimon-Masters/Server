using DigitalWorldOnline.Commons.DTOs.Shop;
using MediatR;

namespace DigitalWorldOnline.Application.Separar.Queries;

public record GetCashShopItem(int UniqueId) : IRequest<CashShopDTO>;