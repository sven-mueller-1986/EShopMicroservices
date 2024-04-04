namespace EShopMicroservices.Services.Catalog.API.Features.Products.CreateProduct;

public record GetProductsByCategoryQuery(string category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(Handle)} called with {query}");

        var products = await session.Query<Product>()
            .Where(p => p.Categories.Contains(query.category))
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}
