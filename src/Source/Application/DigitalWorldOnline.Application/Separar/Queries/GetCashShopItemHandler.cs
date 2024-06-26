using DigitalWorldOnline.Commons.DTOs.Shop;
using DigitalWorldOnline.Commons.Interfaces;
using MediatR;

namespace DigitalWorldOnline.Application.Separar.Queries;

public class GetCashShopItemHandler : IRequestHandler<GetCashShopItem, CashShopDTO>
{
    private readonly ICashShopRepository _cashShopRepository;

    public GetCashShopItemHandler(ICashShopRepository cashShopRepository)
    {
        _cashShopRepository = cashShopRepository;
    }

    public async Task<CashShopDTO> Handle(GetCashShopItem request, CancellationToken cancellationToken)
    {
        return await _cashShopRepository.GetCashShopItemAsync(request.UniqueId);
    }
}