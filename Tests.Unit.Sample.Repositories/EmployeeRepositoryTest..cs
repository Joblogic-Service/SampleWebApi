using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sample.Domains;
using Sample.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Tests.Unit.Sample.Repositories
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private Mock<EmployeeDbContext> mockDbContext;
        private EmployeeRepository employeeRepository;
        private List<Employee> employeeData;
        private Mock<DbSet<Employee>> mockDbSet;

        [TestInitialize]
        public void TestInitialize()
        {
            employeeData = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Sudhakar", LastName = "Mangalarapu", Age=30, Gender="Male"},
                new Employee { Id = 2, FirstName = "Geetha", LastName = "Kokkirala" , Age=25, Gender="Female"},
                // Add more sample employees as needed
            };

            mockDbSet = new Mock<DbSet<Employee>>();

            // Convert the employeeData to an IQueryable
            var employeeQuery = employeeData.AsQueryable();

            // Set up the necessary behavior for the DbSet methods and properties
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeeQuery.Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeeQuery.Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeeQuery.ElementType);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => employeeQuery.GetEnumerator());
            mockDbSet.Setup(d => d.Find(It.IsAny<int>())).Returns<int>(id => employeeData.FirstOrDefault(e => e.Id == id));
            mockDbSet.Setup(d => d.Remove(It.IsAny<Employee>())).Callback<Employee>(employee => employeeData.Remove(employee));

            mockDbContext = new Mock<EmployeeDbContext>();
            mockDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            employeeRepository = new EmployeeRepository(mockDbContext.Object);
        }

        [TestMethod]
        public void Delete_ExistingEmployee_ShouldRemoveFromDbContext()
        {
            // Arrange
            var existingEmployee = employeeData.First();

            // Act
            employeeRepository.Delete(existingEmployee.Id);

            // Assert
            mockDbSet.Verify(d => d.Remove(existingEmployee), Times.Once);
            mockDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Delete_NonExistingEmployee_ShouldNotRemoveFromDbContext()
        {
            // Arrange
            var nonExistingEmployeeId = 999;

            // Act
            employeeRepository.Delete(nonExistingEmployeeId);

            // Assert
            mockDbSet.Verify(d => d.Remove(It.IsAny<Employee>()), Times.Never);
            mockDbContext.Verify(c => c.SaveChanges(), Times.Never);
        }
    }
}
