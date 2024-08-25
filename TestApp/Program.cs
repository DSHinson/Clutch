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
            Tokenizer tokenizer = new Tokenizer(sql);
            SqlParser parser = new SqlParser(tokenizer);

            SelectStatement statement = parser.ParseSelect();

            Console.WriteLine($"Columns: {string.Join(", ", statement.Columns)}");
            Console.WriteLine($"Table: {statement.TableName}");
            if (statement.WhereClause != null)
            {
                Console.WriteLine($"Where: {statement.WhereClause.Column} = {statement.WhereClause.Value}");
            }
        }
    }
}