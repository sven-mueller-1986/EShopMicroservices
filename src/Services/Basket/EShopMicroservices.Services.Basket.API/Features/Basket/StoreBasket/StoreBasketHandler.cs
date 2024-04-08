using Discount.Grpc;

namespace EShopMicroservices.Services.Basket.API.Features.Basket.GetBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null.");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required.");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountService)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart, cancellationToken);

        var basket = await repository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(basket.UserName);
    }

    public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.Name }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}

