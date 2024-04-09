namespace EShopMicroservices.Services.Ordering.Application.Orders.Commands.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

public class GetOrdersValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersValidator()
    {
        RuleFor(x => x.PaginationRequest).NotNull().WithMessage("PaginationRequest is required.");
    }
}
