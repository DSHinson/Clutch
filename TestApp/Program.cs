using ServerLibrary.Parser;
using ServerLibrary.Statements;
using ServerLibrary.Tokenizer;
using System;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter select statement to be parsed, or EXIT to exit");
            string sql = Console.ReadLine();

            while (sql.ToUpper() != "EXIT") {
                TestParser(sql);
                Console.WriteLine("Enter select statement to be parsed, or EXIT to exit");
                sql = Console.ReadLine();
            }
        }

        static void TestParser(string sql)
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
                }
                ,
                (CreateTableStatement create) => {
                    Console.WriteLine($"This is an Create table query: {create.Query}");
                    matched = true;
                }
            );
        }
    }
}