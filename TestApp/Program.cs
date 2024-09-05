using ServerLibrary.Parser;
using ServerLibrary.SyntaxTree;
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
            var statement = (InsertStatement)(new QueryTypeCalculater().DetermineQueryType(sql).Value);

            Console.WriteLine($"Columns: {string.Join(", ", statement.Columns)}");
            Console.WriteLine($"Values: {string.Join(", ", statement.Values)}");
            Console.WriteLine($"Table: {statement.TableName}");
            //if (statement.WhereClause != null)
            //{
            //    Console.WriteLine($"Where: {statement.WhereClause.Column} = {statement.WhereClause.Value}");
            //}
        }
    }
}