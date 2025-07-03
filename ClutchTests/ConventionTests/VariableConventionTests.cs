using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClutchTests.ConventionTests
{
    internal class VariableConventionTests
    {
        [Test]
        public void PublicProperties_ShouldBePascalCase()
        {
            var assembly = Assembly.Load("ServerLibrary");

            var violations = assembly.GetTypes()
                .SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                .Where(p => !Helpers.TestHelpers.IsPascalCase(p.Name))
                .Select(p => $"{p.DeclaringType?.FullName}.{p.Name}")
                .ToList();

            Assert.That(violations, Is.Empty, "Public properties must be PascalCase:\n" + string.Join("\n", violations));
        }
    }
}
