namespace EShopMicroservices.WebApps.Web.Models.Catalog;

public record CreateProductRequest(string Name, string Description, string ImageFile, decimal Price, List<string> Categories);
public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record UpdateProductRequest(Guid Id, string Name, string Description, string ImageFile, decimal Price, List<string> Categories);
