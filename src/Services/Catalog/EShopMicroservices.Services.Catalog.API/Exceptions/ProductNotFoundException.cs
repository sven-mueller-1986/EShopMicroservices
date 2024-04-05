using EShopMicroservices.BuildingBlocks.Exceptions;

namespace EShopMicroservices.Services.Catalog.API.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id) : base(nameof(Product), id)
    { }
}

