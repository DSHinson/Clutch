using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClutchTests.Helpers
{
    internal static class TestHelpers
    {
        public static bool IsPascalCase(string name)
        {
            return Regex.IsMatch(name, @"^[A-Z][a-zA-Z0-9]*$");
        }

        public static bool IsCamelCaseWithUnderscore(string name)
        {
            return Regex.IsMatch(name, @"^_[a-z][a-zA-Z0-9]*$");
        }
    }
}
