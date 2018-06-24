using System;
using Xunit;
using SimpleUsers.Core.Services;

namespace SimpleUsers.Tests
{
    public class PasswordHasherTests
    {
        private readonly IPasswordHasher _hasher = new PasswordHasher();

        [Fact]
        public void PasswordHash_OK()
        {
            var pass = "test";
            var passHash = _hasher.HashPassword(pass);

            Assert.True(_hasher.VerifyPassword(pass, passHash));
        }
    }
}
