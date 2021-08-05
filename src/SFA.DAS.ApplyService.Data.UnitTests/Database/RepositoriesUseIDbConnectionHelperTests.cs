using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.UnitTests.Database
{
    [TestFixture]
    public class RepositoriesUseIDbConnectionHelperTests
    {
        private readonly Type IDbConnectionHelperType = typeof(IDbConnectionHelper);

        private readonly List<string> _repositoriesThatDoNotRequireIDbConnectionHelper = new List<string>()
        {
            "AuditRepository"
        };

        [Test]
        public void Repositories_Must_Use_IDbConnectionHelper()
        {
            var webAssembly = typeof(ApplyRepository).GetTypeInfo().Assembly;

            var repositories = webAssembly.DefinedTypes.Where(c => c.Name.EndsWith("Repository")).ToList();

            foreach (var repository in repositories.Where(repo => !_repositoriesThatDoNotRequireIDbConnectionHelper.Contains(repo.Name)))
            {
                var constructorWithIDbConnectionHelper = repository.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] { IDbConnectionHelperType }, null);

                Assert.IsNotNull(constructorWithIDbConnectionHelper, $"{repository.FullName} does not use {IDbConnectionHelperType.FullName}");
            }
        }
    }
}
