﻿namespace EShopMicroservices.Services.Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,string Name, string Description, string ImageFile, decimal Price, List<string> Categories)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters.");

        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories are required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if(product is null)
            throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Categories = command.Categories;
        product.ImageFile = command.ImageFile;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
