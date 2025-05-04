using ECommerceProductCatalog.DTOs;
using ECommerceProductCatalog.Models;
using ECommerceProductCatalog.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? name, decimal? price, int? categoryId, int pageNo = 1, int pageSize= 10)
        {
            var filter = new ProductFilterDto
            {
                Name = name,
                Price = price,
                CategoryId = categoryId,
                PageNo = pageNo,
                PageSize = pageSize
            };

            var products = await _productService.GetFilteredProductsUsingAdo(filter);

            var categories =  _categoryService.GetAllCategories();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            ViewBag.PageNo = pageNo;
            ViewBag.PageSize = pageSize;

            return View(products);
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); 
        }

        [HttpPost]
        public IActionResult EditProduct(Product? product, List<IFormFile> ImageFiles)
        {

            _productService.UpdateProduct(product, ImageFiles);

            TempData["SuccessMessage"] = "Product updated successfully!";
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Product deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete the product.";
            }

            return RedirectToAction("Index");
        }
    }
}
