using ServerLibrary.Parser;
using ServerLibrary.SyntaxTree;

namespace ClutchTests
{
    public class QueryTokenizationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        public void Test1(string sql)
        {
            var statement = (new QueryTypeCalculater().DetermineQueryType(sql).Value);
        }

        public static IEnumerable<string> GetTestCases()
        {
            var lines = File.ReadAllLines("QueryTestCases.txt");
            foreach (var line in lines)
            {
                yield return line;
            }
        }
    }
}