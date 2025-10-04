using AutoAuction_H2.Models;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class UserTests
    {
        [Fact]
        public void PrivateUser_ShouldNotOverdrawBalance()
        {
            var user = new PrivateUser("Test", "pw", 8000, 1000, "1111111111");
            bool result = user.Withdraw(2000);
            Assert.False(result);
            Assert.Equal(1000, user.Balance);
        }

        [Fact]
        public void CorporateUser_CanOverdrawWithinCredit()
        {
            var user = new CorporateUser("Firm", "pw", 9000, 5000, "12345678", 2000);
            bool result = user.Withdraw(6000);
            Assert.True(result);
            Assert.Equal(-1000, user.Balance);
        }
    }
}
