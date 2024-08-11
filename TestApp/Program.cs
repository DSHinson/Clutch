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
            string sql = "SELECT id, name FROM users WHERE id = '123'";
            Tokenizer tokenizer = new Tokenizer(sql);
            SqlParser parser = new SqlParser(tokenizer);

            SelectStatement statement = parser.ParseSelect();

            Console.WriteLine($"Columns: {string.Join(", ", statement.Columns)}");
            Console.WriteLine($"Table: {statement.TableName}");
            if (statement.WhereClause != null)
            {
                Console.WriteLine($"Where: {statement.WhereClause.Column} = {statement.WhereClause.Value}");
            }

            Console.ReadKey();
        }
    }
}