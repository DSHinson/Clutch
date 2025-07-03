using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Tests
{
    public class DependencyTests
    {
        [Test]
        public void test1()
        {
            var productionAssembly = Assembly.Load("ServerLibrary"); 

            var result = Types.InAssembly(productionAssembly)
                .ShouldNot()
                .HaveDependencyOnAny("*.Tests", "*.TestUtilities", "*.IntegrationTests")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True, $"{productionAssembly.GetName().Name} should not reference test projects.");
        }
    }
}
