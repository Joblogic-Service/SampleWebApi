using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using Sample.Domains;
using Sample.Services;
using Sample.Repositories;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Tests.Unit.Sample.Services
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private EmployeeService employeeService;
        private Mock<EmployeeDbContext> mockDbContext;
        private EmployeeRepository employeeRepository;
        private Mock<DbSet<Employee>> mockDbSet;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mocking DbSet<Employee> and DbContext
            mockDbSet = new Mock<DbSet<Employee>>();
            mockDbContext = new Mock<EmployeeDbContext>();

            // Create an empty list to store the added employees
            var addedEmployees = new List<Employee>();

            // Set up Add method of DbSet to capture the added employees
            mockDbSet.Setup(d => d.Add(It.IsAny<Employee>())).Callback<Employee>(employee => addedEmployees.Add(employee));

            // Set up the Employees property of DbContext to return the mocked DbSet
            mockDbContext.Setup(c => c.Employees).Returns(mockDbSet.Object);

            // Create an instance of EmployeeRepository using the mocked DbContext
            employeeRepository = new EmployeeRepository(mockDbContext.Object);

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            employeeService = new EmployeeService(mockEmployeeRepository.Object);

        }

        [TestMethod]
        public void Save_NewEmployee_ShouldInsertIntoRepository()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = "Sudhakar",
                LastName = "Mangalarapu",
                Age = 30,
                Gender = "Male"
            };

            // Act
            employeeService.Save(employee);

            // Assert
            mockEmployeeRepository.Verify(r => r.Insert(employee), Times.Once);
        }

        [TestMethod]
        public void Save_ExistingEmployee_ShouldUpdateInRepository()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "Sudhakar",
                LastName = "Mangalarapu",
                Age = 30,
                Gender = "Male"
            };

            // Act
            employeeService.Save(employee);

            // Assert
            mockEmployeeRepository.Verify(r => r.Update(employee), Times.Once);
            mockEmployeeRepository.Verify(r => r.Insert(It.IsAny<Employee>()), Times.Never);
        }

        [TestMethod]
        public void GetAll_NoFilters_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "Sudhakar", LastName = "Mangalarapu", Age = 30, Gender = "Male" },
            new Employee { Id = 2, FirstName = "Geetha", LastName = "Kokkirala", Age = 25, Gender = "Female" },
            new Employee { Id = 3, FirstName = "Abyanth", LastName = "Mangalarapu", Age = 2, Gender = "Male" }
        };

            mockEmployeeRepository.Setup(r => r.GetAll()).Returns(employees);

            // Act
            var result = employeeService.GetAll();

            // Assert
            CollectionAssert.AreEqual(employees, result.ToList());
        }

        [TestMethod]
        public void GetAll_WithFilters_ShouldReturnFilteredEmployees()
        {
            // Arrange
            var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "Sudhakar", LastName = "Mangalarapu", Age = 30, Gender = "Male" },
            new Employee { Id = 2, FirstName = "Geetha", LastName = "Kokkirala", Age = 25, Gender = "Female" },
            new Employee { Id = 3, FirstName = "Abyanth", LastName = "Mangalarapu", Age = 2, Gender = "Male" }
        };

            mockEmployeeRepository.Setup(r => r.GetAll()).Returns(employees);

            // Act
            var result = employeeService.GetAll("Sudhakar", null, "Male");

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Sudhakar", result.First().FirstName);
            Assert.AreEqual("Mangalarapu", result.First().LastName);
        }

    }
}
