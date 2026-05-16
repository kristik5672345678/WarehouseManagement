using System;
using System.Linq;
using System.Numerics;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;
using Xunit;

namespace WarehouseManagement.Tests
{
    public class ServiceOrgsTests
    {
        private ApplicationContext _context;
        private ServiceOrgs _service;

        public ServiceOrgsTests()
        {
            _context = new ApplicationContext();
            _service = new ServiceOrgs(_context);
        }

        [Fact]
        public void Constructor_WithValidContext_SetsContext()
        {
            // Arrange & Act
            var service = new ServiceOrgs(_context);

            // Assert
            Assert.NotNull(service.GetContext());
            Assert.Equal(_context, service.GetContext());
        }

        [Fact]
        public void Constructor_WithNullContext_DoesNotSetContext()
        {
            // Arrange & Act
            var service = new ServiceOrgs(null);

            // Assert
            Assert.Null(service.GetContext());
        }

        [Fact]
        public void GetAll_ReturnsAllOrganizations()
        {
            // Arrange
            int expectedCount = _context.Orgs.Count;

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public void Add_ValidOrganization_ReturnsTrue()
        {
            // Arrange
            var newOrg = new Organization("Тестовая организация");

            // Act
            bool result = _service.Add(newOrg);

            // Assert
            Assert.True(result);
            Assert.Contains(newOrg, _context.Orgs);
            Assert.Equal(3, _context.Orgs.Count);
        }

        [Fact]
        public void Add_NullOrganization_ReturnsFalse()
        {
            // Act
            bool result = _service.Add(null);

            // Assert
            Assert.False(result);
            Assert.Equal(2, _context.Orgs.Count);
        }

        [Fact]
        public void Add_OrganizationWithEmptyName_ReturnsFalse()
        {
            // Arrange
            var newOrg = new Organization("");

            // Act
            bool result = _service.Add(newOrg);

            // Assert
            Assert.False(result);
            Assert.Equal(2, _context.Orgs.Count);
        }

        [Fact]
        public void Add_OrganizationWithNullName_ReturnsFalse()
        {
            // Arrange
            var newOrg = new Organization(null);

            // Act
            bool result = _service.Add(newOrg);

            // Assert
            Assert.False(result);
            Assert.Equal(2, _context.Orgs.Count);
        }

        [Fact]
        public void Add_DuplicateOrganization_ReturnsFalse()
        {
            // Arrange
            var org = _context.Orgs.First();

            // Act
            bool result = _service.Add(org);

            // Assert
            Assert.False(result);
            Assert.Equal(2, _context.Orgs.Count);
        }

        [Fact]
        public void Delete_ValidOrganization_ReturnsTrue()
        {
            // Arrange
            var orgToDelete = _context.Orgs.First();
            int initialCount = _context.Orgs.Count;

            // Act
            bool result = _service.Delete(orgToDelete);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(orgToDelete, _context.Orgs);
            Assert.Equal(initialCount - 1, _context.Orgs.Count);
        }

        [Fact]
        public void Delete_NullOrganization_ReturnsFalse()
        {
            // Arrange
            int initialCount = _context.Orgs.Count;

            // Act
            bool result = _service.Delete(null);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Orgs.Count);
        }

        [Fact]
        public void Delete_OrganizationWithEmptyName_ReturnsFalse()
        {
            // Arrange
            var orgWithEmptyName = new Organization("");
            int initialCount = _context.Orgs.Count;

            // Act
            bool result = _service.Delete(orgWithEmptyName);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Orgs.Count);
        }

        [Fact]
        public void Delete_NonExistentOrganization_ReturnsFalse()
        {
            // Arrange
            var nonExistentOrg = new Organization("Несуществующая организация");
            int initialCount = _context.Orgs.Count;

            // Act
            bool result = _service.Delete(nonExistentOrg);

            // Assert
            Assert.False(result);
            Assert.Equal(initialCount, _context.Orgs.Count);
        }

        [Fact]
        public void Edit_ValidOrganization_ReturnsTrue()
        {
            // Arrange
            var orgToEdit = _context.Orgs.First();
            var editedOrg = new Organization("Обновленное название");
            editedOrg.Id = orgToEdit.Id;

            // Act
            bool result = _service.Edit(editedOrg);

            // Assert
            Assert.True(result);
            Assert.Equal("Обновленное название", _context.Orgs.First(o => o.Id == orgToEdit.Id).OrgName);
        }

        [Fact]
        public void Edit_NullOrganization_ReturnsFalse()
        {
            // Act
            bool result = _service.Edit(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_OrganizationWithEmptyName_ReturnsFalse()
        {
            // Arrange
            var orgToEdit = _context.Orgs.First();
            var editedOrg = new Organization("");
            editedOrg.Id = orgToEdit.Id;

            // Act
            bool result = _service.Edit(editedOrg);

            // Assert
            Assert.False(result);
            Assert.NotEqual("", _context.Orgs.First(o => o.Id == orgToEdit.Id).OrgName);
        }

        [Fact]
        public void Edit_NonExistentOrganization_ReturnsFalse()
        {
            // Arrange
            var nonExistentOrg = new Organization("Несуществующая");
            nonExistentOrg.Id = BigInteger.Zero;

            // Act
            bool result = _service.Edit(nonExistentOrg);

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
        public void Add_IncrementsId()
        {
            // Arrange
            var org1 = new Organization("Орг 1");
            var org2 = new Organization("Орг 2");

            // Act
            _service.Add(org1);
            _service.Add(org2);

            // Assert
            Assert.True(org2.Id > org1.Id);
        }
    }
}
