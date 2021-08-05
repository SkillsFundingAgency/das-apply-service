using System;
using System.Reflection;
using NUnit.Framework;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.UnitTests.Database
{
    [TestFixture]
    public class UnitOfWorkUsesIDbConnectionHelperTest
    {
        private readonly Type IDbConnectionHelperType = typeof(IDbConnectionHelper);

        [Test]
        public void UnitOfWork_Must_Use_IDbConnectionHelper()
        {
            var unitOfWorkType = typeof(UnitOfWork.UnitOfWork);

            var constructorWithIDbConnectionHelper = unitOfWorkType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] { IDbConnectionHelperType }, null);

            Assert.IsNotNull(constructorWithIDbConnectionHelper, $"{unitOfWorkType.FullName} does not use {IDbConnectionHelperType.FullName}");
        }
    }
}
