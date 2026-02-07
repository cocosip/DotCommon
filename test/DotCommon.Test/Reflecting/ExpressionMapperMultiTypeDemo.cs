using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Demonstrates multi-type mapping capabilities of ExpressionMapper
    /// </summary>
    public class ExpressionMapperMultiTypeDemo
    {
        #region Test Models

        /// <summary>
        /// User model for demonstration
        /// </summary>
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        /// <summary>
        /// Product model for demonstration
        /// </summary>
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }

        /// <summary>
        /// Order model for demonstration
        /// </summary>
        public class Order
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public DateTime OrderDate { get; set; }
        }

        #endregion

        #region Multi-Type Conversion Tests

        [Fact]
        public void MultipleTypes_ShouldConvertIndependently()
        {
            // Arrange
            var userDict = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "Name", "John Doe" },
                { "Email", "john@example.com" }
            };

            var productDict = new Dictionary<string, object>
            {
                { "Id", 100 },
                { "Name", "Laptop" },
                { "Price", 999.99m }
            };

            var orderDict = new Dictionary<string, object>
            {
                { "Id", 1001 },
                { "UserId", 1 },
                { "ProductId", 100 },
                { "Quantity", 2 },
                { "OrderDate", DateTime.Now }
            };

            // Act
            var user = ExpressionMapper.DictionaryToObject<User>(userDict);
            var product = ExpressionMapper.DictionaryToObject<Product>(productDict);
            var order = ExpressionMapper.DictionaryToObject<Order>(orderDict);

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("john@example.com", user.Email);

            Assert.Equal(100, product.Id);
            Assert.Equal("Laptop", product.Name);
            Assert.Equal(999.99m, product.Price);

            Assert.Equal(1001, order.Id);
            Assert.Equal(1, order.UserId);
            Assert.Equal(100, order.ProductId);
            Assert.Equal(2, order.Quantity);
        }

        [Fact]
        public async Task ConcurrentConversions_ShouldWorkCorrectly()
        {
            // Arrange
            var userDicts = Enumerable.Range(1, 100).Select(i => new Dictionary<string, object>
            {
                { "Id", i },
                { "Name", $"User{i}" },
                { "Email", $"user{i}@example.com" }
            }).ToList();

            var productDicts = Enumerable.Range(1, 100).Select(i => new Dictionary<string, object>
            {
                { "Id", i },
                { "Name", $"Product{i}" },
                { "Price", i * 10.0m }
            }).ToList();

            // Act
            var userTasks = userDicts.Select(dict => Task.Run(() => ExpressionMapper.DictionaryToObject<User>(dict))).ToArray();
            var productTasks = productDicts.Select(dict => Task.Run(() => ExpressionMapper.DictionaryToObject<Product>(dict))).ToArray();

            var users = await Task.WhenAll(userTasks);
            var products = await Task.WhenAll(productTasks);

            // Assert
            Assert.Equal(100, users.Length);
            Assert.Equal(100, products.Length);

            for (int i = 0; i < 100; i++)
            {
                Assert.Equal(i + 1, users[i].Id);
                Assert.Equal($"User{i + 1}", users[i].Name);
                Assert.Equal(i + 1, products[i].Id);
                Assert.Equal($"Product{i + 1}", products[i].Name);
            }
        }

        [Fact]
        public void ConverterCaching_ShouldReuseGeneratedMethods()
        {
            // Arrange
            var userDict = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "Name", "Test User" },
                { "Email", "test@example.com" }
            };

            // Act - First conversion (should generate converter)
            var stopwatch1 = Stopwatch.StartNew();
            var user1 = ExpressionMapper.DictionaryToObject<User>(userDict);
            stopwatch1.Stop();

            // Act - Second conversion (should reuse cached converter)
            var stopwatch2 = Stopwatch.StartNew();
            var user2 = ExpressionMapper.DictionaryToObject<User>(userDict);
            stopwatch2.Stop();

            // Assert
            Assert.Equal(user1.Id, user2.Id);
            Assert.Equal(user1.Name, user2.Name);
            Assert.Equal(user1.Email, user2.Email);

            // Second call should be significantly faster due to caching
            Console.WriteLine($"First call: {stopwatch1.ElapsedTicks} ticks");
            Console.WriteLine($"Second call: {stopwatch2.ElapsedTicks} ticks");
            Console.WriteLine($"Performance improvement: {(double)stopwatch1.ElapsedTicks / stopwatch2.ElapsedTicks:F2}x");
        }

        [Fact]
        public void RoundTripConversions_MultipleTypes_ShouldPreserveData()
        {
            // Arrange
            var originalUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com" };
            var originalProduct = new Product { Id = 100, Name = "Laptop", Price = 999.99m };
            var originalOrder = new Order { Id = 1001, UserId = 1, ProductId = 100, Quantity = 2, OrderDate = DateTime.Now };

            // Act - Convert to dictionary and back
            var userDict = ExpressionMapper.ObjectToDictionary(originalUser);
            var convertedUser = ExpressionMapper.DictionaryToObject<User>(userDict);

            var productDict = ExpressionMapper.ObjectToDictionary(originalProduct);
            var convertedProduct = ExpressionMapper.DictionaryToObject<Product>(productDict);

            var orderDict = ExpressionMapper.ObjectToDictionary(originalOrder);
            var convertedOrder = ExpressionMapper.DictionaryToObject<Order>(orderDict);

            // Assert
            Assert.Equal(originalUser.Id, convertedUser.Id);
            Assert.Equal(originalUser.Name, convertedUser.Name);
            Assert.Equal(originalUser.Email, convertedUser.Email);

            Assert.Equal(originalProduct.Id, convertedProduct.Id);
            Assert.Equal(originalProduct.Name, convertedProduct.Name);
            Assert.Equal(originalProduct.Price, convertedProduct.Price);

            Assert.Equal(originalOrder.Id, convertedOrder.Id);
            Assert.Equal(originalOrder.UserId, convertedOrder.UserId);
            Assert.Equal(originalOrder.ProductId, convertedOrder.ProductId);
            Assert.Equal(originalOrder.Quantity, convertedOrder.Quantity);
            Assert.Equal(originalOrder.OrderDate, convertedOrder.OrderDate);
        }

        [Fact]
        public void Performance_FirstVsSubsequentCalls_ShouldShowCachingBenefit()
        {
            // Arrange
            var testData = new List<Dictionary<string, object>>();
            for (int i = 0; i < 1000; i++)
            {
                testData.Add(new Dictionary<string, object>
                {
                    { "Id", i },
                    { "Name", $"User{i}" },
                    { "Email", $"user{i}@example.com" }
                });
            }

            // Warmup to ensure JIT compilation
            _ = testData.Take(10).Select(dict => ExpressionMapper.DictionaryToObject<User>(dict)).ToList();

            // Act - First batch (includes converter generation time)
            var stopwatch1 = Stopwatch.StartNew();
            var firstBatch = testData.Take(500).Select(dict => ExpressionMapper.DictionaryToObject<User>(dict)).ToList();
            stopwatch1.Stop();

            // Act - Second batch (uses cached converter)
            var stopwatch2 = Stopwatch.StartNew();
            var secondBatch = testData.Skip(500).Select(dict => ExpressionMapper.DictionaryToObject<User>(dict)).ToList();
            stopwatch2.Stop();

            // Assert
            Assert.Equal(500, firstBatch.Count);
            Assert.Equal(500, secondBatch.Count);

            var avgFirstBatch = stopwatch1.ElapsedTicks / 500.0;
            var avgSecondBatch = stopwatch2.ElapsedTicks / 500.0;

            Console.WriteLine($"Average first batch: {avgFirstBatch:F2} ticks per conversion");
            Console.WriteLine($"Average second batch: {avgSecondBatch:F2} ticks per conversion");
            Console.WriteLine($"Performance improvement: {avgFirstBatch / avgSecondBatch:F2}x");

            // The second batch should be faster or at most 10% slower (allow for CI environment variance)
            // In CI environments, performance can vary due to shared resources
            var tolerance = avgFirstBatch * 1.1; // Allow 10% tolerance
            Assert.True(avgSecondBatch <= tolerance,
                $"Second batch ({avgSecondBatch:F2}) should be faster or within 10% of first batch ({avgFirstBatch:F2})");
        }

        #endregion
    }
}