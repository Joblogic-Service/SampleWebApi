using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sample.Domains;
using Sample.Repositories;
using Sample.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests.Unit.Sample.Services
{
    [TestClass]
    public class EmployeeServiceTest
    {
        [TestMethod]
        public async Task GetAll_ReturnsCorrectListOfEmployees()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var mockContext = new Mock<DBContext>();
            var employeeService = new EmployeeService(mockRepository.Object, mockContext.Object);

            var searchModel = new EmployeeSearchModel
            {
                firstNameFilter = "John",
                lastNameFilter = "Doe",
                genderFilter = "Male"
            };

            var mockEmployeeList = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe", Gender = "Male" },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Gender = "Female" }
            };

            mockContext.Setup(c => c.Employee).Returns(MockDbSet(mockEmployeeList));

            // Act
            var result = employeeService.GetAll(searchModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockEmployeeList.Count, result.Data.Count);
            Assert.AreEqual("Employee Data", result.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public async Task Save_ValidEmployee_ReturnsDataSaved()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var mockContext = new Mock<DBContext>();
            var employeeService = new EmployeeService(mockRepository.Object, mockContext.Object);

            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Gender = "Male" };

            mockRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Employee)null);
            mockRepository.Setup(r => r.Insert(It.IsAny<Employee>())).ReturnsAsync(true);

            // Act
            var result = await employeeService.Save(employee);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data);
            Assert.AreEqual("Data Saved", result.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public async Task Save_InvalidEmployee_ReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var mockContext = new Mock<DBContext>();
            var employeeService = new EmployeeService(mockRepository.Object, mockContext.Object);

            // Act
            var result = await employeeService.Save(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Data);
            Assert.AreEqual("Wrong Data", result.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        private static DbSet<T> MockDbSet<T>(List<T> list) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(list.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(list.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(list.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => list.GetEnumerator());
            return mockSet.Object;
        }

    }
}
