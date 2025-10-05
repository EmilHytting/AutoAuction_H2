using System;
using AutoAuction_H2.Models.Entities;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class UserTests
    {
        // ---------- PRIVATE USER TESTS ----------

        [Fact]
        public void PrivateUser_CanWithdrawWithinBalance()
        {
            var user = new PrivateUser("Test", "pw", 8000, 3000, "1111111111");
            bool result = user.Withdraw(1000);
            Assert.True(result);
            Assert.Equal(2000, user.Balance);
        }

        [Fact]
        public void PrivateUser_ShouldNotOverdrawBalance()
        {
            var user = new PrivateUser("Test", "pw", 8000, 1000, "1111111111");
            bool result = user.Withdraw(2000);
            Assert.False(result);
            Assert.Equal(1000, user.Balance);
        }

        [Fact]
        public void PrivateUser_InvalidCpr_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                new PrivateUser("Test", "pw", 8000, 1000, "123")); // for kort CPR
        }

        // ---------- CORPORATE USER TESTS ----------

        [Fact]
        public void CorporateUser_CanOverdrawWithinCredit()
        {
            var user = new CorporateUser("Firm", "pw", 9000, 5000, "12345678", 2000);
            bool result = user.Withdraw(6000); // 5000 - 6000 = -1000, indenfor kredit
            Assert.True(result);
            Assert.Equal(-1000, user.Balance);
        }

        [Fact]
        public void CorporateUser_CannotOverdrawBeyondCredit()
        {
            var user = new CorporateUser("Firm", "pw", 9000, 500, "12345678", 2000);
            bool result = user.Withdraw(3000); // max er 500 + 2000 = 2500
            Assert.False(result);
            Assert.Equal(500, user.Balance);
        }

        [Fact]
        public void CorporateUser_InvalidCvr_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                new CorporateUser("Firm", "pw", 9000, 500, "1234", 2000)); // forkert CVR
        }

        // ---------- COMMON TESTS ----------

        [Fact]
        public void Withdraw_Zero_ShouldThrow()
        {
            var user = new PrivateUser("ZeroTest", "pw", 8000, 2000, "1111111111");
            Assert.Throws<ArgumentException>(() => user.Withdraw(0));
        }

        [Fact]
        public void Withdraw_NegativeAmount_ShouldThrow()
        {
            var user = new PrivateUser("NegativeTest", "pw", 8000, 2000, "1111111111");
            Assert.Throws<ArgumentException>(() => user.Withdraw(-100));
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var user = new PrivateUser("DepositTest", "pw", 8000, 1000, "1111111111");
            user.Deposit(500);
            Assert.Equal(1500, user.Balance);
        }

        [Fact]
        public void Reserve_ShouldLockFunds()
        {
            var user = new PrivateUser("ReserveTest", "pw", 8000, 2000, "1111111111");
            bool result = user.Reserve(1500);
            Assert.True(result);
            Assert.Equal(1500, user.ReservedAmount);
            Assert.Equal(500, user.AvailableBalance);
        }

        [Fact]
        public void Release_ShouldUnlockFunds()
        {
            var user = new PrivateUser("ReleaseTest", "pw", 8000, 2000, "1111111111");
            user.Reserve(1000);
            user.Release(500);
            Assert.Equal(500, user.ReservedAmount);
            Assert.Equal(1500, user.AvailableBalance);
        }

        [Fact]
        public void Release_MoreThanReserved_ShouldThrow()
        {
            var user = new PrivateUser("ReleaseError", "pw", 8000, 2000, "1111111111");
            user.Reserve(500);
            Assert.Throws<InvalidOperationException>(() => user.Release(1000));
        }
    }
}
