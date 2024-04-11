namespace EShopMicroservices.WebApps.Web.Services;

public interface ICatalogService
{
    [Post("/catalog-service/products")]
    Task<CreateProductResponse> CreateProduct([Body(BodySerializationMethod.Serialized)] CreateProductRequest request);

    [Get("/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int pageNumber = 0, int pageSize = 10);

    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProductById(Guid id);

    [Get("/catalog-service/products/category/{category}")]
    Task<GetProductsByCategoryResponse> GetProductsByCategory(string category);

    [Put("/catalog-service/products")]
    Task<UpdateProductResponse> UpdateProduct([Body(BodySerializationMethod.Serialized)] UpdateProductRequest request);

    [Delete("/catalog-service/products/{id}")]
    Task<DeleteProductResponse> DeleteProduct(Guid id);
}
