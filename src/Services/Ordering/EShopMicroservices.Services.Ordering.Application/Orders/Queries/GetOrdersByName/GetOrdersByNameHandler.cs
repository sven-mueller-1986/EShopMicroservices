namespace EShopMicroservices.Services.Ordering.Application.Orders.Commands.GetOrdersByName;

public class GetOrdersByNameQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new GetOrdersByNameResult(orders.ToDtoList());
    }
}
