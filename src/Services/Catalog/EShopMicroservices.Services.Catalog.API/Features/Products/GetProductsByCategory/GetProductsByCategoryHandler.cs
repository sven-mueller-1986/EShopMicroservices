namespace EShopMicroservices.Services.Catalog.API.Features.Products.CreateProduct;

public record GetProductsByCategoryQuery(string category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .Where(p => p.Categories.Contains(query.category))
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}
