using System.Net.Http.Json;
using Web_API_Client.Models;

namespace Web_API_Client
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5221/") };

        static async Task Main(string[] args)
        {
            await RunCategoryOperations();
            await RunProductOperations();
        }

        static async Task RunCategoryOperations()
        {
            // Create a new category
            var newCategory = new Category { CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" };
            var createdCategory = await CreateCategory(newCategory);
            Console.WriteLine($"Created Category: {createdCategory.CategoryID}");

            // Get all categories
            var categories = await GetCategories();
            Console.WriteLine($"Total Categories: {categories.Count}");

            // Update the category
            createdCategory.Description = "Updated description";
            await UpdateCategory(createdCategory.CategoryID, createdCategory);
            Console.WriteLine($"Updated Category: {createdCategory.CategoryID}");

            // Delete the category
            await DeleteCategory(createdCategory.CategoryID);
            Console.WriteLine($"Deleted Category: {createdCategory.CategoryID}");
        }

        static async Task RunProductOperations()
        {
            // Ensure at least one category exists
            var categories = await GetCategories();
            Category category;
            if (categories.Count == 0)
            {
                category = new Category { CategoryName = "Default Category", Description = "Default description" };
                category = await CreateCategory(category);
                Console.WriteLine($"Created Default Category: {category.CategoryID}");
            }
            else
            {
                category = categories.First();
            }

            // Create a new product
            var newProduct = new Product
            {
                ProductName = "Chai",
                CategoryID = category.CategoryID,
                QuantityPerUnit = "10 boxes x 20 bags",
                UnitPrice = 18,
                UnitsInStock = 39,
                UnitsOnOrder = 0,
                ReorderLevel = 10,
                Discontinued = false,
            };
            var createdProduct = await CreateProduct(newProduct);
            Console.WriteLine($"Created Product: {createdProduct.ProductID}");

            // Get all products with pagination
            var products = await GetProducts(0, 10, null);
            Console.WriteLine($"Total Products: {products.Count}");

            // Update the product
            createdProduct.UnitPrice = 200;
            await UpdateProduct(createdProduct.ProductID, createdProduct);
            Console.WriteLine($"Updated Product: {createdProduct.ProductID}");

            // Delete the product
            await DeleteProduct(createdProduct.ProductID);
            Console.WriteLine($"Deleted Product: {createdProduct.ProductID}");
        }

        static async Task<Category> CreateCategory(Category category)
        {
            var response = await client.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Category>();
        }

        static async Task<List<Category>> GetCategories()
        {
            return await client.GetFromJsonAsync<List<Category>>("api/categories");
        }

        static async Task UpdateCategory(int id, Category category)
        {
            var response = await client.PutAsJsonAsync($"api/categories/{id}", category);
            response.EnsureSuccessStatusCode();
        }

        static async Task DeleteCategory(int id)
        {
            var response = await client.DeleteAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }

        static async Task<Product> CreateProduct(Product product)
        {
            var response = await client.PostAsJsonAsync("api/products", product);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");
                response.EnsureSuccessStatusCode();
            }
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        static async Task<List<Product>> GetProducts(int pageNumber, int pageSize, int? categoryId)
        {
            var url = $"api/products?pageNumber={pageNumber}&pageSize={pageSize}";
            if (categoryId.HasValue)
            {
                url += $"&categoryId={categoryId.Value}";
            }
            return await client.GetFromJsonAsync<List<Product>>(url);
        }

        static async Task UpdateProduct(int id, Product product)
        {
            var response = await client.PutAsJsonAsync($"api/products/{id}", product);
            response.EnsureSuccessStatusCode();
        }

        static async Task DeleteProduct(int id)
        {
            var response = await client.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
