using ECommerceProductCatalog.Models;
using Microsoft.Data.SqlClient;

namespace ECommerceProductCatalog.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly string _connectionString;

        public CategoryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Categories", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new Category
                            {
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }
    }
}
