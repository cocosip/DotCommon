using System;
using System.Collections.Generic;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Demonstrates multi-type conversion capabilities of EmitMapper
    /// </summary>
    public class MultiTypeConversionDemo
    {
        #region Test Models

        /// <summary>
        /// User model for testing
        /// </summary>
        public class User
        {
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Product model for testing
        /// </summary>
        public class Product
        {
            public string Title { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public double Rating { get; set; }
            public bool InStock { get; set; }
            public DateTime? LaunchDate { get; set; }
        }

        /// <summary>
        /// Order model for testing
        /// </summary>
        public class Order
        {
            public int OrderId { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public decimal TotalAmount { get; set; }
            public DateTime OrderDate { get; set; }
            public bool IsPaid { get; set; }
        }

        #endregion

        [Fact]
        public void MultipleTypes_ShouldConvertIndependently()
        {
            // Arrange - Create different types of objects
            var user = new User
            {
                Name = "John Doe",
                Age = 30,
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 15)
            };

            var product = new Product
            {
                Title = "Laptop",
                Price = 999.99m,
                Rating = 4.5,
                InStock = true,
                LaunchDate = new DateTime(2023, 6, 1)
            };

            var order = new Order
            {
                OrderId = 12345,
                CustomerName = "Jane Smith",
                TotalAmount = 1299.98m,
                OrderDate = new DateTime(2023, 12, 25),
                IsPaid = true
            };

            // Act - Convert objects to dictionaries
            var userDict = EmitMapper.ObjectToDictionary(user);
            var productDict = EmitMapper.ObjectToDictionary(product);
            var orderDict = EmitMapper.ObjectToDictionary(order);

            // Assert - Verify each conversion
            Assert.NotNull(userDict);
            Assert.Equal("John Doe", userDict["Name"]);
            Assert.Equal("30", userDict["Age"]);
            Assert.Equal("True", userDict["IsActive"]);

            Assert.NotNull(productDict);
            Assert.Equal("Laptop", productDict["Title"]);
            Assert.Equal("999.99", productDict["Price"]);
            Assert.Equal("4.5", productDict["Rating"]);
            Assert.Equal("True", productDict["InStock"]);

            Assert.NotNull(orderDict);
            Assert.Equal("12345", orderDict["OrderId"]);
            Assert.Equal("Jane Smith", orderDict["CustomerName"]);
            Assert.Equal("1299.98", orderDict["TotalAmount"]);
            Assert.Equal("True", orderDict["IsPaid"]);

            // Act - Convert dictionaries back to objects
            var userFromDict = EmitMapper.DictionaryToObject<User>(userDict);
            var productFromDict = EmitMapper.DictionaryToObject<Product>(productDict);
            var orderFromDict = EmitMapper.DictionaryToObject<Order>(orderDict);

            // Assert - Verify round-trip conversion
            Assert.NotNull(userFromDict);
            Assert.Equal(user.Name, userFromDict.Name);
            Assert.Equal(user.Age, userFromDict.Age);
            Assert.Equal(user.IsActive, userFromDict.IsActive);

            Assert.NotNull(productFromDict);
            Assert.Equal(product.Title, productFromDict.Title);
            Assert.Equal(product.Price, productFromDict.Price);
            Assert.Equal(product.Rating, productFromDict.Rating);
            Assert.Equal(product.InStock, productFromDict.InStock);

            Assert.NotNull(orderFromDict);
            Assert.Equal(order.OrderId, orderFromDict.OrderId);
            Assert.Equal(order.CustomerName, orderFromDict.CustomerName);
            Assert.Equal(order.TotalAmount, orderFromDict.TotalAmount);
            Assert.Equal(order.IsPaid, orderFromDict.IsPaid);
        }

        [Fact]
        public void ConcurrentConversions_ShouldWorkCorrectly()
        {
            // Arrange
            var users = new List<User>();
            var products = new List<Product>();
            
            for (int i = 0; i < 100; i++)
            {
                users.Add(new User 
                { 
                    Name = $"User{i}", 
                    Age = 20 + i, 
                    IsActive = i % 2 == 0,
                    CreatedAt = DateTime.Now.AddDays(-i)
                });
                
                products.Add(new Product 
                { 
                    Title = $"Product{i}", 
                    Price = 10.0m + i, 
                    Rating = 1.0 + (i % 5),
                    InStock = i % 3 == 0,
                    LaunchDate = DateTime.Now.AddMonths(-i)
                });
            }

            // Act & Assert - Convert multiple types concurrently
            System.Threading.Tasks.Parallel.ForEach(users, user =>
            {
                var dict = EmitMapper.ObjectToDictionary(user);
                var converted = EmitMapper.DictionaryToObject<User>(dict);
                
                Assert.NotNull(dict);
                Assert.NotNull(converted);
                Assert.Equal(user.Name, converted.Name);
                Assert.Equal(user.Age, converted.Age);
            });

            System.Threading.Tasks.Parallel.ForEach(products, product =>
            {
                var dict = EmitMapper.ObjectToDictionary(product);
                var converted = EmitMapper.DictionaryToObject<Product>(dict);
                
                Assert.NotNull(dict);
                Assert.NotNull(converted);
                Assert.Equal(product.Title, converted.Title);
                Assert.Equal(product.Price, converted.Price);
            });
        }

        [Fact]
        public void ConverterCaching_ShouldReuseGeneratedMethods()
        {
            // Act - Get converters multiple times
            var converter1 = EmitMapper.GetObjectToDictionaryConverter<User>();
            var converter2 = EmitMapper.GetObjectToDictionaryConverter<User>();
            var converter3 = EmitMapper.GetDictionaryToObjectConverter<User>();
            var converter4 = EmitMapper.GetDictionaryToObjectConverter<User>();

            // Assert - Should return the same cached instances
            Assert.Same(converter1, converter2);
            Assert.Same(converter3, converter4);

            // Different types should have different converters
            var productConverter = EmitMapper.GetObjectToDictionaryConverter<Product>();
            Assert.NotSame(converter1, productConverter);
        }
    }
}