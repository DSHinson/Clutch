using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClutchTests.ConventionTests
{
    internal class MethodConventionTests
    {
        [Test]
        public void AllPublicMethods_ShouldBePascalCase()
        {
            var assembly = Assembly.Load("ServerLibrary");

            var violations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsNested) // optional filter
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                .Where(m => !Helpers.TestHelpers.IsPascalCase(m.Name))
                .Select(m => $"{m.DeclaringType?.FullName}.{m.Name}")
                .ToList();

            Assert.That(violations, Is.Empty,
                "The following public methods do not follow PascalCase:\n" + string.Join("\n", violations));
        }
    }
}
