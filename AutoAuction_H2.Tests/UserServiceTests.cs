
using AutoAuction_H2.Models.Interfaces;
using AutoAuction_H2.Services;
using AutoAuction_H2.Tests.Fakes;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class UserServiceCrudTests
    {
        private readonly IUserService _userService;

        public UserServiceCrudTests()
        {
            _userService = new FakeUserService();
        }

        [Fact]
        public void CanUpdatePassword()
        {
            _userService.CreatePrivateUser("User1", "oldpw", 8000, 500m, "1234567890");

            bool updated = _userService.UpdatePassword("User1", "newpw");

            var user = _userService.Authenticate("User1", "newpw");
            Assert.True(updated);
            Assert.NotNull(user);
        }

        [Fact]
        public void CanUpdateZipCode()
        {
            var user = _userService.CreatePrivateUser("User2", "pw", 7000, 200m, "1111111111");

            bool updated = _userService.UpdateZipCode("User2", 9999);

            Assert.True(updated);
            var found = _userService.FindByUserName("User2");
            Assert.Equal(9999, found.ZipCode);
        }

        [Fact]
        public void CanUpdateBalance()
        {
            var user = _userService.CreateCorporateUser("Firm1", "pw", 5000, 1000m, "87654321", 2000m);

            bool updated = _userService.UpdateBalance("Firm1", 2000m);

            Assert.True(updated);
            var found = _userService.FindByUserName("Firm1");
            Assert.Equal(2000m, found.Balance);
        }

        [Fact]
        public void CanDeleteUser()
        {
            _userService.CreatePrivateUser("DeleteMe", "pw", 6000, 300m, "2222222222");

            bool deleted = _userService.DeleteUser("DeleteMe");

            Assert.True(deleted);
            var user = _userService.FindByUserName("DeleteMe");
            Assert.Null(user);
        }

        [Fact]
        public void DeleteUser_ShouldReturnFalse_IfUserNotFound()
        {
            bool deleted = _userService.DeleteUser("Ghost");
            Assert.False(deleted);
        }
    }
}
