using ECommerceProductCatalog.DTOs;
using ECommerceProductCatalog.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ECommerceProductCatalog.Service
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<List<ProductResponseDto>> GetFilteredProductsUsingAdo(ProductFilterDto filter)
        {
            var products = new List<ProductResponseDto>();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("GetFilteredProducts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(filter.Name) ? (object)DBNull.Value : filter.Name);
                    cmd.Parameters.AddWithValue("@Price", filter.Price.HasValue ? filter.Price.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CategoryId", filter.CategoryId.HasValue ? filter.CategoryId.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNo", filter.PageNo > 0 ? filter.PageNo : 1);
                    cmd.Parameters.AddWithValue("@PageSize", filter.PageSize > 0 ? filter.PageSize : 10);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new ProductResponseDto
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Name = reader["Name"]?.ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Description = reader["Description"]?.ToString(),
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                SKU = reader["SKU"]?.ToString(),
                                CategoryName = reader["CategoryName"]?.ToString(),
                                FirstImagePath = reader["FirstImagePath"]?.ToString()
                            });
                        }
                    }
                }
            }

            return products;
        }


        public async Task<Product> GetProductById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = (int)reader["ProductId"],
                                Name = reader["Name"].ToString(),
                                SKU = reader["SKU"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                CategoryId = (int)reader["CategoryId"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateProduct(Product product, List<IFormFile> ImageFiles)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string productQuery = "UPDATE Products SET Name = @Name, SKU = @SKU, Price = @Price, CategoryId = @CategoryId WHERE ProductId = @ProductId";
                    using (SqlCommand productCommand = new SqlCommand(productQuery, connection, transaction))
                    {
                        productCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                        productCommand.Parameters.AddWithValue("@Name", product.Name ?? (object)DBNull.Value);
                        productCommand.Parameters.AddWithValue("@SKU", product.SKU ?? (object)DBNull.Value);
                        productCommand.Parameters.AddWithValue("@Price", product.Price);
                        productCommand.Parameters.AddWithValue("@CategoryId", product.CategoryId);

                        int rowsAffected = productCommand.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected in update: {rowsAffected}");
                    }


                    string deleteImagesQuery = "DELETE FROM ProductImages WHERE ProductId = @ProductId";
                    using (SqlCommand deleteImagesCommand = new SqlCommand(deleteImagesQuery, connection, transaction))
                    {
                        deleteImagesCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                        deleteImagesCommand.ExecuteNonQuery();
                    }

                    if (ImageFiles != null && ImageFiles.Any())
                    {
                        foreach (var file in ImageFiles)
                        {
                            if (file != null && file.Length > 0)
                            {
                                string imagePath = SaveFile(file, "Files/ProductImage");

                                string insertImageQuery = "INSERT INTO ProductImages (ProductId, ImagePath) VALUES (@ProductId, @ImagePath)";
                                using (SqlCommand insertImageCommand = new SqlCommand(insertImageQuery, connection, transaction))
                                {
                                    insertImageCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                                    insertImageCommand.Parameters.AddWithValue("@ImagePath", imagePath);
                                    insertImageCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }


                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                                            
                    Console.Error.WriteLine($"Error updating product: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DeleteProductWithImages", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }


        //We can save it in Generic Or Helper Functions as well
        public static string SaveFile(IFormFile file, string folderPath)
        {
            if (file == null) return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullFolderPath = Path.Combine(wwwrootPath, folderPath);
            fullFolderPath = fullFolderPath.Replace('/', '\\');

            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }
            var filePath = Path.Combine(fullFolderPath, fileName);
            var existingFilePath = Path.Combine(fullFolderPath, file.FileName);
            if (File.Exists(existingFilePath))
            {
                return existingFilePath.Substring(wwwrootPath.Length).Replace("\\", "/");
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var relativePath = filePath.Substring(wwwrootPath.Length).Replace("\\", "/");

            return relativePath;
        }
    }
}
