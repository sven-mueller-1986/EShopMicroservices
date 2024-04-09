using EShopMicroservices.BuildingBlocks.Exceptions;

namespace EShopMicroservices.Services.Ordering.Application.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid id) : base(nameof(Order), id)
    { }
}
