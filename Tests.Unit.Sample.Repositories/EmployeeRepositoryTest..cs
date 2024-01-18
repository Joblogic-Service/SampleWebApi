using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sample.Domains;
using Sample.Repositories;

namespace Tests.Unit.Sample.Repositories
{
    [TestClass]
    public class EmployeeRepositoryTest
    {

        [TestMethod]
        public async Task Update_ValidEmployee_ReturnsTrue()
        {
            // Arrange
            var dbContextMock = new Mock<DBContext>();
            var employeeRepository = new EmployeeRepository(dbContextMock.Object);
            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Age = 30, Gender = "Male" };

            // Mocking the Update method
            dbContextMock.Setup(m => m.Set<Employee>().Attach(It.IsAny<Employee>()));
            dbContextMock.Setup(m => m.Entry(It.IsAny<Employee>()).State).Returns(Microsoft.EntityFrameworkCore.EntityState.Modified);

            // Act
            var result = await employeeRepository.Update(employee);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
