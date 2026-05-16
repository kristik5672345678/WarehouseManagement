using System;
using System.Linq;
using System.Numerics;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;
using Xunit;


namespace WarehouseManagement.Tests
{

    public class ServiceWhsTests
    {
        private ApplicationContext _context;
        private Organization _testOrg;
        private Organization _otherOrg;
        private ServiceWhs _service;

        public ServiceWhsTests()
        {
            _context = new ApplicationContext();
            _testOrg = _context.Orgs.FirstOrDefault();
            _otherOrg = _context.Orgs.LastOrDefault();
            _service = new ServiceWhs(_context, _testOrg);
        }

        [Fact]
        public void Constructor_WithValidContextAndOrg_InitializesCorrectly()
        {
            // Arrange & Act
            var service = new ServiceWhs(_context, _testOrg);

            // Assert
            Assert.NotNull(service.GetContext());
            Assert.NotNull(service.GetOrganization());
            Assert.Equal(_context, service.GetContext());
            Assert.Equal(_testOrg, service.GetOrganization());
        }

        [Fact]
        public void Constructor_WithNullContext_DoesNotInitialize()
        {
            // Act
            var service = new ServiceWhs(null, _testOrg);

            // Assert
            Assert.Null(service.GetContext());
            Assert.Null(service.GetOrganization());
        }

        [Fact]
        public void Constructor_WithNullOrg_DoesNotInitialize()
        {
            // Act
            var service = new ServiceWhs(_context, null);

            // Assert
            Assert.Null(service.GetContext());
            Assert.Null(service.GetOrganization());
        }

        [Fact]
        public void Constructor_WithOrgNotInContext_DoesNotInitialize()
        {
            // Arrange
            var orgNotInContext = new Organization("Несуществующая организация");

            // Act
            var service = new ServiceWhs(_context, orgNotInContext);

            // Assert
            Assert.NotNull(service.GetContext());
            Assert.Null(service.GetOrganization());
        }

        [Fact]
        public void GetAll_ReturnsOnlyWarehousesForCurrentOrganization()
        {
            // Arrange
            var expectedWhsCount = _context.Whs.Count(w => w.OrgId == _testOrg.Id);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedWhsCount, result.Count);
            Assert.All(result, w => Assert.Equal(_testOrg.Id, w.OrgId));
        }

        [Fact]
        public void Add_ValidWarehouse_ReturnsTrue()
        {
            // Arrange
            var newWh = new Warehouse("Новый склад", "Новый адрес", _testOrg.Id);

            // Act
            bool result = _service.Add(newWh);

            // Assert
            Assert.True(result);
            Assert.Contains(newWh, _context.Whs);
        }

        [Fact]
        public void Add_NullWarehouse_ReturnsFalse()
        {
            // Act
            bool result = _service.Add(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Add_WarehouseWithEmptyName_ReturnsFalse()
        {
            // Arrange
            var newWh = new Warehouse("", "Адрес", _testOrg.Id);

            // Act
            bool result = _service.Add(newWh);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Add_WarehouseWithNullName_ReturnsFalse()
        {
            // Arrange
            var newWh = new Warehouse(null, "Адрес", _testOrg.Id);

            // Act
            bool result = _service.Add(newWh);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Add_DuplicateWarehouse_ReturnsFalse()
        {
            // Arrange
            var existingWh = _context.Whs.FirstOrDefault(w => w.OrgId == _testOrg.Id);

            // Act
            bool result = _service.Add(existingWh);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_ValidWarehouse_ReturnsTrue()
        {
            // Arrange
            var whToDelete = _context.Whs.FirstOrDefault(w => w.OrgId == _testOrg.Id);
            int initialCount = _context.Whs.Count;

            // Act
            bool result = _service.Delete(whToDelete);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(whToDelete, _context.Whs);
            Assert.Equal(initialCount - 1, _context.Whs.Count);
        }

        [Fact]
        public void Delete_NullWarehouse_ReturnsFalse()
        {
            // Arrange
            int initialCount = _context.Whs.Count;

            // Act
            bool result = _service.Delete(null);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Whs.Count);
        }

        [Fact]
        public void Delete_NonExistentWarehouse_ReturnsFalse()
        {
            // Arrange
            var nonExistentWh = new Warehouse("Несуществующий", "Адрес", _testOrg.Id);
            nonExistentWh.Id = BigInteger.Zero;
            int initialCount = _context.Whs.Count;

            // Act
            bool result = _service.Delete(nonExistentWh);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Whs.Count);
        }

        [Fact]
        public void Delete_WarehouseFromDifferentOrganization_ReturnsFalse()
        {
            // Arrange
            var whFromOtherOrg = _context.Whs.FirstOrDefault(w => w.OrgId == _otherOrg.Id);
            int initialCount = _context.Whs.Count;

            // Act
            bool result = _service.Delete(whFromOtherOrg);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Whs.Count);
            Assert.Contains(whFromOtherOrg, _context.Whs);
        }

        [Fact]
        public void Edit_ValidWarehouse_ReturnsTrue()
        {
            // Arrange
            var whToEdit = _context.Whs.FirstOrDefault(w => w.OrgId == _testOrg.Id);
            var editedWh = new Warehouse("Обновленный склад", "Обновленный адрес", _testOrg.Id);
            editedWh.Id = whToEdit.Id;

            // Act
            bool result = _service.Edit(editedWh);

            // Assert
            Assert.True(result);
            var updatedWh = _context.Whs.FirstOrDefault(w => w.Id == whToEdit.Id);
            Assert.Equal("Обновленный склад", updatedWh.WhName);
            Assert.Equal("Обновленный адрес", updatedWh.WhAddress);
        }

        [Fact]
        public void Edit_NullWarehouse_ReturnsFalse()
        {
            // Act
            bool result = _service.Edit(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_NonExistentWarehouse_ReturnsFalse()
        {
            // Arrange
            var nonExistentWh = new Warehouse("Несуществующий", "Адрес", _testOrg.Id);
            nonExistentWh.Id = BigInteger.Zero;

            // Act
            bool result = _service.Edit(nonExistentWh);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetContext_ReturnsContext()
        {
            // Act
            var result = _service.GetContext();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_context, result);
        }

        [Fact]
        public void GetOrganization_ReturnsOrganization()
        {
            // Act
            var result = _service.GetOrganization();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testOrg, result);
        }
    }
}
