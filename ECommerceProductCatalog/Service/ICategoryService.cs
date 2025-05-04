using ECommerceProductCatalog.Models;

namespace ECommerceProductCatalog.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
    }
}
