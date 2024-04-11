namespace EShopMicroservices.WebApps.Web.Models.Catalog;

public record CreateProductResponse(Guid Id);
public record DeleteProductResponse(ProductModel? Product);
public record GetProductByIdResponse(ProductModel Product);
public record GetProductsResponse(IEnumerable<ProductModel> Products);
public record GetProductsByCategoryResponse(IEnumerable<ProductModel> Products);
public record UpdateProductResponse(bool IsSuccess);
