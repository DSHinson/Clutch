using ServerLibrary.Parser;
using ServerLibrary.Statements;

namespace ClutchTests
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
            var statement = new QueryTypeCalculater().DetermineQueryType(sql);
            bool matched = false;

            statement.Switch(
                (InsertStatement insert) => {
                    Console.WriteLine($"This is an Insert query: {insert.Query}");
                    matched = true;
                },
                (SelectStatement select) => {
                    Console.WriteLine($"This is a Select query: {select.Query}");
                    matched = true;
                },
                (DeleteStatement delete) => {
                    Console.WriteLine($"This is a Delete query: {delete.Query}");
                    matched = true;
                },
                (UpdateStatement update) => {
                    Console.WriteLine($"This is an Update query: {update.Query}");
                    matched = true;
                },
                (CreateTableStatement create) => {
                    Console.WriteLine($"This is a create table statement: {create.Query}");
                    matched = true;
                }
            );

           Assert.IsTrue(matched);
        }

        public static IEnumerable<string> GetTestCases()
        {
            var lines = File.ReadAllLines("QueryTestCases.txt");
            foreach (var line in lines)
            {
                yield return line;
            }

            string CreatTableQuery = File.ReadAllText("CreateTableTests.txt");
            yield return CreatTableQuery;
        }
    }
}