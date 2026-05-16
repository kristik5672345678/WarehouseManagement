using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Enums;
using WarehouseManagement.Data.Interfaces;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;
using Xunit;

namespace WarehouseManagement.Tests
{

    public class ServiceInvoicesTests
    {
        private ApplicationContext _context;
        private Warehouse _testWarehouse;
        private Warehouse _otherWarehouse;
        private ServiceInvoices _service;

        public ServiceInvoicesTests()
        {
            _context = new ApplicationContext();
            _testWarehouse = _context.Whs.FirstOrDefault();
            _otherWarehouse = _context.Whs.LastOrDefault();
            _service = new ServiceInvoices(_context, _testWarehouse);
        }

        [Fact]
        public void Constructor_WithValidParameters_SetsContextAndWarehouse()
        {
            // Act
            var service = new ServiceInvoices(_context, _testWarehouse);

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
            var service = new ServiceInvoices(_context, null);

            // Assert
            Assert.Null(service.GetWarehouse());
        }

        [Fact]
        public void GetAll_ReturnsInvoicesForCurrentWarehouse()
        {
            // Arrange
            var invoice1 = new Invoice(_testWarehouse);
            var invoice2 = new Invoice(_testWarehouse);
            _context.Invcs.Add(invoice1);
            _context.Invcs.Add(invoice2);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, i => Assert.Equal(_testWarehouse.Id, i.WhsId));
        }

        [Fact]
        public void GetAll_ForWarehouseWithoutInvoices_ReturnsEmptyCollection()
        {
            // Arrange
            var emptyWarehouse = new Warehouse("Пустой склад", "Адрес", BigInteger.Zero);
            _context.Whs.Add(emptyWarehouse);
            var service = new ServiceInvoices(_context, emptyWarehouse);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Add_ValidInvoiceWithItems_ReturnsTrue()
        {
            // Arrange
            var product = _context.Prdcts.First();
            var invoice = new Invoice(_testWarehouse)
            {
                Type = InvoiceType.Incoming,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem(product)
                    {
                        Quantity = 10
                    }
                }
            };

            // Act
            bool result = _service.Add(invoice);

            // Assert
            Assert.True(result);
            Assert.Contains(invoice, _context.Invcs);
        }

        [Fact]
        public void Add_InvoiceWithEmptyItems_ReturnsFalse()
        {
            // Arrange
            var invoice = new Invoice(_testWarehouse)
            {
                Type = InvoiceType.Incoming,
                Items = new List<InvoiceItem>()
            };

            // Act
            bool result = _service.Add(invoice);

            // Assert
            Assert.False(result);
            Assert.DoesNotContain(invoice, _context.Invcs);
        }

        [Fact]
        public void Add_NullInvoice_ReturnsFalse()
        {
            // Act
            bool result = _service.Add(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Add_ValidInvoiceWithMultipleItems_ReturnsTrue()
        {
            // Arrange
            var product1 = _context.Prdcts.First();
            var product2 = _context.Prdcts.Last();
            var invoice = new Invoice(_testWarehouse)
            {
                Type = InvoiceType.Outgoing,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem(product1) { Quantity = 5 },
                    new InvoiceItem(product2) { Quantity = 3 }
                }
            };

            // Act
            bool result = _service.Add(invoice);

            // Assert
            Assert.True(result);
            Assert.Contains(invoice, _context.Invcs);
            Assert.Equal(2, invoice.Items.Count);
        }

        [Fact]
        public void Delete_ExistingInvoice_ReturnsTrue()
        {
            // Arrange
            var invoice = new Invoice(_testWarehouse)
            {
                Type = InvoiceType.Incoming,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem() { Name = "Тестовый товар", Quantity = 10 }
                }
            };
            _context.Invcs.Add(invoice);
            int initialCount = _context.Invcs.Count;

            // Act
            bool result = _service.Delete(invoice);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(invoice, _context.Invcs);
            Assert.Equal(initialCount - 1, _context.Invcs.Count);
        }

        [Fact]
        public void Delete_NonExistentInvoice_ReturnsFalse()
        {
            // Arrange
            var nonExistentInvoice = new Invoice(_testWarehouse)
            {
                Id = BigInteger.Zero
            };
            int initialCount = _context.Invcs.Count;

            // Act
            bool result = _service.Delete(nonExistentInvoice);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Invcs.Count);
        }

        [Fact]
        public void Delete_NullInvoice_ReturnsFalse()
        {
            // Arrange
            int initialCount = _context.Invcs.Count;

            // Act
            bool result = _service.Delete(null);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Invcs.Count);
        }

        [Fact]
        public void Edit_AnyInvoice_AlwaysReturnsFalse()
        {
            // Arrange
            var invoice = new Invoice(_testWarehouse)
            {
                Type = InvoiceType.Incoming,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem() { Name = "Тестовый товар", Quantity = 10 }
                }
            };

            // Act
            bool result = _service.Edit(invoice);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_NullInvoice_ReturnsFalse()
        {
            // Act
            bool result = _service.Edit(null);

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

        [Fact]
        public void Add_InvoiceWithItems_CorrectlyStoresItems()
        {
            // Arrange
            var product = _context.Prdcts.First();
            var invoice = new Invoice(_testWarehouse)
            {
                Type = InvoiceType.Outgoing,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem(product)
                    {
                        Quantity = 15,
                        DiscountPercent = 10
                    }
                }
            };

            // Act
            bool result = _service.Add(invoice);

            // Assert
            Assert.True(result);
            var addedInvoice = _context.Invcs.First(i => i.Id == invoice.Id);
            Assert.Single(addedInvoice.Items);
            Assert.Equal(product.Name, addedInvoice.Items[0].Name);
            Assert.Equal(15, addedInvoice.Items[0].Quantity);
            Assert.Equal(10, addedInvoice.Items[0].DiscountPercent);
        }
    }
}
