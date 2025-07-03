using ServerLibrary.Parser;
using ServerLibrary.Statements;

namespace ClutchTests.FunctionalTests
{
    public class QueryTokenizationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        public void SQLTokenizer_Should_Parse_Correctly(string sql)
        {
            var statement = QueryTypeCalculater.DetermineQueryType(sql);
            bool matched = false;

            statement.Switch(
                (insert) => {
                    Console.WriteLine($"This is an Insert query: {insert.Query}");
                    matched = true;
                },
                (select) => {
                    Console.WriteLine($"This is a Select query: {select.Query}");
                    matched = true;
                },
                (delete) => {
                    Console.WriteLine($"This is a Delete query: {delete.Query}");
                    matched = true;
                },
                (update) => {
                    Console.WriteLine($"This is an Update query: {update.Query}");
                    matched = true;
                },
                (create) => {
                    Console.WriteLine($"This is a create table statement: {create.Query}");
                    matched = true;
                }
            );

           Assert.IsTrue(matched);
        }

        public static IEnumerable<string> GetTestCases()
        {
            var lines = File.ReadAllLines("TestData/QueryTestCases.txt");
            foreach (var line in lines)
            {
                yield return line;
            }

            string CreatTableQuery = File.ReadAllText("TestData/CreateTableTests.txt");
            yield return CreatTableQuery;
        }
    }
}