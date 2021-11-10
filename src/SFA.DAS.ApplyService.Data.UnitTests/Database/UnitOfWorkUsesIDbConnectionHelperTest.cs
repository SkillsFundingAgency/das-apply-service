using System;
using System.Linq;
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

            var constructorParameters = unitOfWorkType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).SelectMany(c => c.GetParameters());
            var iDbConnectionHelperParameter = constructorParameters.FirstOrDefault(param => param.ParameterType == typeof(IDbConnectionHelper));

            Assert.IsNotNull(iDbConnectionHelperParameter, $"{unitOfWorkType.FullName} does not use {nameof(IDbConnectionHelper)}");
        }
    }
}
