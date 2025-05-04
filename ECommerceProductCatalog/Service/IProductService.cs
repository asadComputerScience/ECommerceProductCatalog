using ECommerceProductCatalog.DTOs;
using ECommerceProductCatalog.Models;

namespace ECommerceProductCatalog.Service
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetFilteredProductsUsingAdo(ProductFilterDto filter);
        Task<bool> DeleteProductAsync(int productId);
        Task<Product> GetProductById(int id);
        void UpdateProduct(Product product, List<IFormFile> ImageFiles);
    }

}
