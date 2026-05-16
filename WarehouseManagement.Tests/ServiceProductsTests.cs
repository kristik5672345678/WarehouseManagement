using System;
using System.Linq;
using System.Numerics;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Enums;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;
using Xunit;


namespace WarehouseManagement.Tests
{
    public class ServiceProductsTests
    {
        private ApplicationContext _context;
        private Warehouse _testWarehouse;
        private Warehouse _otherWarehouse;
        private ServiceProducts _service;

        public ServiceProductsTests()
        {
            _context = new ApplicationContext();
            _testWarehouse = _context.Whs.FirstOrDefault();
            _otherWarehouse = _context.Whs.LastOrDefault();
            _service = new ServiceProducts(_context, _testWarehouse);
        }

        [Fact]
        public void Constructor_WithValidParameters_SetsContextAndWarehouse()
        {
            // Act
            var service = new ServiceProducts(_context, _testWarehouse);

            // Assert
            Assert.NotNull(service.GetContext());
            Assert.NotNull(service.GetWarehouse());
            Assert.Equal(_context, service.GetContext());
            Assert.Equal(_testWarehouse, service.GetWarehouse());
        }

        [Fact]
        public void Constructor_WithNullWarehouse_DoesNotInitializeWarehouse()
        {
            // Act
            var service = new ServiceProducts(_context, null);

            // Assert
            Assert.Null(service.GetWarehouse());
        }

        [Fact]
        public void GetAll_ReturnsProductsForCurrentWarehouse()
        {
            // Act
            var result = _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, p => Assert.Equal(_testWarehouse.Id, p.WhsId));
        }

        [Fact]
        public void GetAll_ForWarehouseWithoutProducts_ReturnsEmptyCollection()
        {
            // Arrange
            var emptyWarehouse = new Warehouse("Пустой склад", "Адрес", BigInteger.Zero);
            _context.Whs.Add(emptyWarehouse);
            var service = new ServiceProducts(_context, emptyWarehouse);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WithValidFilter_ReturnsFilteredProducts()
        {
            // Act
            var result = _service.GetAll("рога");

            // Assert
            Assert.NotNull(result);
            Assert.All(result, p => p.Name.ToLower().Contains("рога"));
        }

        [Fact]
        public void GetAll_WithFilterNoMatches_ReturnsEmptyCollection()
        {
            // Act
            var result = _service.GetAll("несуществующийтовар");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Add_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var newProduct = new Product()
            {
                Article = "ART001",
                Name = "Новый товар",
                Unit = "шт",
                Price = 100,
                StockQuantity = 50,
                WhsId = _testWarehouse.Id,
                Category = new Category() { Name = "Тестовая категория" },
                Manufacturer = new Manufacturer() { Name = "Тестовый производитель" },
                Supplier = new Supplier() { Name = "Тестовый поставщик" }
            };

            // Act
            bool result = _service.Add(newProduct);

            // Assert
            Assert.True(result);
            Assert.Contains(newProduct, _context.Prdcts);
        }

        [Fact]
        public void Add_DuplicateProduct_ReturnsFalse()
        {
            // Arrange
            var existingProduct = _context.Prdcts.First();

            // Act
            bool result = _service.Add(existingProduct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Add_ProductWithNullName_ReturnsFalse()
        {
            // Arrange
            var invalidProduct = new Product()
            {
                Name = null,
                WhsId = _testWarehouse.Id
            };

            // Act
            bool result = _service.Add(invalidProduct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var productToEdit = _context.Prdcts.First();
            var editedProduct = new Product()
            {
                Id = productToEdit.Id,
                Article = productToEdit.Article,
                Name = "Обновленное название",
                Unit = productToEdit.Unit,
                Price = 200,
                StockQuantity = 150,
                WhsId = productToEdit.WhsId,
                Category = productToEdit.Category,
                Manufacturer = productToEdit.Manufacturer,
                Supplier = productToEdit.Supplier
            };

            // Act
            bool result = _service.Edit(editedProduct);

            // Assert
            Assert.True(result);
            var updatedProduct = _context.Prdcts.First(p => p.Id == productToEdit.Id);
            Assert.Equal("Обновленное название", updatedProduct.Name);
            Assert.Equal(200, updatedProduct.Price);
        }

        [Fact]
        public void Edit_NonExistentProduct_ReturnsFalse()
        {
            // Arrange
            var nonExistentProduct = new Product()
            {
                Id = BigInteger.Zero,
                Name = "Несуществующий товар",
                Unit = "шт",
                WhsId = _testWarehouse.Id
            };

            // Act
            bool result = _service.Edit(nonExistentProduct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var productToDelete = _context.Prdcts.First();
            int initialCount = _context.Prdcts.Count;

            // Act
            bool result = _service.Delete(productToDelete);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(productToDelete, _context.Prdcts);
            Assert.Equal(initialCount - 1, _context.Prdcts.Count);
        }

        [Fact]
        public void Delete_NonExistentProduct_ReturnsFalse()
        {
            // Arrange
            var nonExistentProduct = new Product()
            {
                Id = BigInteger.Zero,
                Name = "Несуществующий товар",
                Unit = "шт"
            };
            int initialCount = _context.Prdcts.Count;

            // Act
            bool result = _service.Delete(nonExistentProduct);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Prdcts.Count);
        }

        [Fact]
        public void Add_WithInvoiceItemIncoming_NewProduct_ReturnsTrue()
        {
            // Arrange
            var newItem = new InvoiceItem()
            {
                Article = "ART_NEW",
                Name = "Новый товар из накладной",
                Unit = "шт",
                Price = 150,
                Quantity = 30,
                DiscountPercent = 5,
                CategoryName = "Новая категория",
                ManufacturerName = "Новый производитель",
                SupplierName = "Новый поставщик"
            };

            // Act
            bool result = _service.Add(newItem, InvoiceType.Incoming);

            // Assert
            Assert.True(result);
            Assert.Contains(_context.Prdcts, p => p.Name == "Новый товар из накладной");
        }

        [Fact]
        public void Add_WithInvoiceItemIncoming_EmptyName_ReturnsFalse()
        {
            // Arrange
            var invalidItem = new InvoiceItem()
            {
                Name = "",
                Quantity = 10
            };

            // Act
            bool result = _service.Add(invalidItem, InvoiceType.Incoming);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void Add_WithInvoiceItemOutgoing_NonExistentProduct_ReturnsFalse()
        {
            // Arrange
            var nonExistentItem = new InvoiceItem()
            {
                Article = "NON_EXISTENT",
                Name = "Несуществующий товар",
                Unit = "шт",
                Price = 100,
                Quantity = 5
            };

            // Act
            bool result = _service.Add(nonExistentItem, InvoiceType.Outgoing);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetContext_ReturnsApplicationContext()
        {
            // Act
            var result = _service.GetContext();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_context, result);
        }

        [Fact]
        public void GetWarehouse_ReturnsWarehouse()
        {
            // Act
            var result = _service.GetWarehouse();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testWarehouse, result);
        }
    }
}
    