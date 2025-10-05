
using AutoAuction_H2.Services;
using Xunit;
using AutoAuction_H2.Models.Interfaces;

namespace AutoAuction_H2.Tests
{
    public class UserServiceCrudTests
    {
        private readonly IUserService _service;

        public UserServiceCrudTests()
        {
            _service = new UserService();
        }

        [Fact]
        public void CanUpdatePassword()
        {
            _service.CreatePrivateUser("User1", "oldpw", 8000, 500m, "1234567890");

            bool updated = _service.UpdatePassword("User1", "newpw");

            var user = _service.Authenticate("User1", "newpw");
            Assert.True(updated);
            Assert.NotNull(user);
        }

        [Fact]
        public void CanUpdateZipCode()
        {
            var user = _service.CreatePrivateUser("User2", "pw", 7000, 200m, "1111111111");

            bool updated = _service.UpdateZipCode("User2", 9999);

            Assert.True(updated);
            var found = _service.FindByUserName("User2");
            Assert.Equal(9999, found.ZipCode);
        }

        [Fact]
        public void CanUpdateBalance()
        {
            var user = _service.CreateCorporateUser("Firm1", "pw", 5000, 1000m, "87654321", 2000m);

            bool updated = _service.UpdateBalance("Firm1", 2000m);

            Assert.True(updated);
            var found = _service.FindByUserName("Firm1");
            Assert.Equal(2000m, found.Balance);
        }

        [Fact]
        public void CanDeleteUser()
        {
            _service.CreatePrivateUser("DeleteMe", "pw", 6000, 300m, "2222222222");

            bool deleted = _service.DeleteUser("DeleteMe");

            Assert.True(deleted);
            var user = _service.FindByUserName("DeleteMe");
            Assert.Null(user);
        }

        [Fact]
        public void DeleteUser_ShouldReturnFalse_IfUserNotFound()
        {
            bool deleted = _service.DeleteUser("Ghost");
            Assert.False(deleted);
        }
    }
}
